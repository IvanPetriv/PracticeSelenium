using Locators.Exceptions;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal class InboxPageOutlook(IWebDriver driver) : AbstractObject(driver) {
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public const string inboxOutlookUrl = @"https://outlook.office365.com/mail/";

        #region WebElements
        [FindsBy(How = How.Id, Using = "MailList")]
        [CacheLookup]
        private readonly IWebElement mailList;
        #endregion

        /// <summary>
        /// Reads the letter by the specified index
        /// </summary>
        /// <param name="index">Letter index (zero-based)</param>
        /// <returns>An anonymous class with `Sender`, `Subject`, `Content` fields</returns>
        public InboxPageOutlook ReadLetter(out object data, int index=0) {
            driver.Navigate().GoToUrl(inboxOutlookUrl);

            // Checks if logged in
            try {
                wait.Until(ExpectedConditions.UrlMatches(inboxOutlookUrl));
            } catch (WebDriverTimeoutException) {
                throw new NotLoggedInException("The user is not logged in");
            }

            // Checks if the mail list is loaded
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("MailList")));

            // Gets the nth element (by index)
            IWebElement nthMail = mailList.FindElement(By.XPath($"//div[contains(@id, 'AQAAA')][{index+1}]"));
            nthMail.Click();

            // Gets the letter data
            IWebElement mailContent = driver.FindElement(By.XPath("//div[contains(@id, 'UniqueMessageBody')]/div/div/div/div"));
            IWebElement mailSender = driver.FindElement(By.XPath("//div[@aria-label='Електронний лист']/div/div/div/div/span/span/div/span"));
            IWebElement mailSubject = driver.FindElement(By.XPath("//div[@id='ConversationReadingPaneContainer']//span[@title and string-length(@title) > 0]"));
            data = new { Sender = mailSender.Text, Subject = mailSubject.Text, Content = mailContent.Text };

            return this;
        }
    }
}
