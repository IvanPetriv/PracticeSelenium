using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
    internal class InboxPageOutlook {
        public const string inboxOutlookUrl = @"https://outlook.office365.com/mail/";
        readonly IWebDriver driver;
        WebDriverWait wait;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driver">Driver which runs the search engine</param>
        public InboxPageOutlook(IWebDriver driver) {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(driver, this);
        }

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
        public object ReadLetter(int index=0) {
            // Checks if logged in
            try {
                wait.Until(ExpectedConditions.UrlMatches(inboxOutlookUrl));
            } catch (WebDriverTimeoutException) {
                return false;
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

            return new { Sender = mailSender.Text, Subject = mailSubject.Text, Content = mailContent.Text };
        }
    }
}
