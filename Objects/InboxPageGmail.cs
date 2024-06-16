using Locators.Exceptions;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class InboxPageGmail(IWebDriver driver) : AbstractObject(driver) {
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public const string inboxGoogleUrl = @"https://mail.google.com/mail/u/0/?tab=rm&ogbl#inbox";

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
        public InboxPageGmail SendLetter(string receivers, string subject, string content) {
            driver.Navigate().GoToUrl(inboxGoogleUrl);

            // Checks if logged in
            try {
                wait.Until(ExpectedConditions.UrlMatches(inboxGoogleUrl));
            } catch (WebDriverTimeoutException) {
                throw new NotLoggedInException("The user is not logged in");
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
            } catch (WebDriverTimeoutException) {
                throw new SendEmailFailedException($"The email was not sent. Subject: '{subject}' to users '{receivers}' with content: '{content}'");
            }

            return this;
        }
    }
}
