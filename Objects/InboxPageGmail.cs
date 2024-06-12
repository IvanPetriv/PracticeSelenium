using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
    internal class InboxPageGmail {
        public const string inboxGoogleUrl = @"https://mail.google.com/mail/u/0/?tab=rm&ogbl#inbox";
        readonly IWebDriver driver;
        WebDriverWait wait;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driver">Driver which runs the search engine</param>
        public InboxPageGmail(IWebDriver driver) {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(driver, this);
        }

        #region WebElements
        [FindsBy(How = How.XPath, Using = "//div[contains(text(), 'Написати')]")]
        [CacheLookup]
        private readonly IWebElement writeLetterButton;

        [FindsBy(How = How.XPath, Using = "//div[@name='to']//input")]
        [CacheLookup]
        private readonly IWebElement receiverInput;

        [FindsBy(How = How.Name, Using = "subjectbox")]
        [CacheLookup]
        private readonly IWebElement titleInput;

        [FindsBy(How = How.XPath, Using = "//div[@aria-label='Текст повідомлення']")]
        [CacheLookup]
        private readonly IWebElement contentInput;

        [FindsBy(How = How.XPath, Using = "//div[@aria-label='Надіслати ‪(Ctrl –Enter)‬']")]
        [CacheLookup]
        private readonly IWebElement sendLetterButton;
        #endregion

        /// <summary>
        /// Sends a letter to specified receivers with the specified subject and content
        /// </summary>
        /// <param name="receivers">Receivers who will get the letter</param>
        /// <param name="subject">Subject of the letter</param>
        /// <param name="content">Content of the letter</param>
        /// <returns>`true` if the letter was sent, `false` otherwise</returns>
        public bool SendLetter(string receivers, string subject, string content) {
            driver.Navigate().GoToUrl(inboxGoogleUrl);

            // Checks if logged in
            try {
                wait.Until(ExpectedConditions.UrlMatches(inboxGoogleUrl));
            } catch (WebDriverTimeoutException) {
                return false;
            }

            // Starts writing the letter
            writeLetterButton.Click();
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@name='to']//input")));

            // Enters the provided data and sends it
            receiverInput.SendKeys(receivers);
            titleInput.SendKeys(subject);
            contentInput.SendKeys(content);
            sendLetterButton.Click();

            try {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(text(), 'Повідомлення надіслано')]")));
                return true;
            } catch (WebDriverTimeoutException) {
                return false;
            }
        }
    }
}
