using Locators.Models;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System.Text.RegularExpressions;

namespace Locators.Objects {
    internal partial class InboxPageOutlook(IWebDriver driver) : AbstractObject(driver) {
        public const string inboxOutlookUrl = @"https://outlook.office365.com/mail/";

        #region WebElements
        [FindsBy(How = How.Id, Using = "MailList")]
        [CacheLookup]
        private readonly IWebElement mailList = null!;
        #endregion

        /// <summary>
        /// Reads the letter by the specified index
        /// </summary>
        /// <param name="index">Letter index (zero-based)</param>
        /// <returns>An anonymous class with `Sender`, `Subject`, `Content` fields</returns>
        public InboxPageOutlook ReadLetter(out Letter data, int index=0) {
            driver.Navigate().GoToUrl(inboxOutlookUrl);

            // Checks if the mail list is loaded
            wait.Until(ExpectedConditions.ElementExists(By.Id("MailList")));

            // Gets the nth element (by index)
            IWebElement nthMail = mailList.FindElement(By.XPath($"//div[contains(@id, 'AQAAA')][{index+1}]"));
            nthMail.Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@aria-label, 'Вміст повідомлення')]/div/div/div/div")));
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@aria-label='Електронний лист']/div/div/div/div/span/span/div/span")));

            // Gets the letter data
            IWebElement mailContent = driver.FindElement(By.XPath("//div[contains(@aria-label, 'Вміст повідомлення')]/div/div/div/div"));
            IWebElement mailSender = driver.FindElement(By.XPath("//div[@aria-label='Електронний лист']/div/div/div/div/span/span/div/span"));
            IWebElement mailSubject = driver.FindElement(By.XPath("//div[@id='ConversationReadingPaneContainer']//span[@title and string-length(@title) > 0]"));

            string mailEmail = SenderEmailPart().Match(mailSender.Text).Groups[1].Value;
            data = new Letter(Receiver: mailEmail, Subject: mailSubject.Text, Content: mailContent.Text);
            return this;
        }

        [GeneratedRegex(@"<([^>]*)>")]
        private static partial Regex SenderEmailPart();
    }
}
