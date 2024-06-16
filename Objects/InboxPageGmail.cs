using Locators.Exceptions;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
    public class InboxPageGmail(IWebDriver driver) : AbstractObject(driver) {
        public const string inboxGoogleUrl = @"https://mail.google.com/mail/u/0/?tab=rm&ogbl#inbox";

        #region WebElements
        [FindsBy(How = How.XPath, Using = "//div[contains(text(), 'Написати')]")]
        private readonly IWebElement writeLetterButton = null!;

        [FindsBy(How = How.XPath, Using = "//div[@name='to']//input")]
        private readonly IWebElement receiverInput = null!;

        [FindsBy(How = How.Name, Using = "subjectbox")]
        private readonly IWebElement titleInput = null!;

        [FindsBy(How = How.XPath, Using = "//div[@aria-label='Текст повідомлення']")]
        private readonly IWebElement contentInput = null!;

        [FindsBy(How = How.XPath, Using = "//div[contains(@aria-label, 'Надіслати')]")]
        private readonly IWebElement sendLetterButton = null!;

        [FindsBy(How = How.Id, Using = "link_enable_notifications_hide")]
        private readonly IWebElement closeNotification = null!;
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

            // Starts writing the letter
            writeLetterButton.Click();

            // Enters the provided data and sends it
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@name='to']//input")));
            receiverInput.SendKeys(receivers);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("subjectbox")));
            titleInput.SendKeys(subject);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@aria-label='Текст повідомлення']")));
            contentInput.SendKeys(content);
            try {
                closeNotification.Click();
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }

            try {
                sendLetterButton.Click();
            } catch (ElementNotInteractableException) {
                throw new IncorrectEmailException($"The email has missing/incorrect data which cannot be sent");
            }
            

            try {
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[contains(text(), 'Повідомлення надіслано')]")));
            } catch (WebDriverTimeoutException) {
                throw new SendEmailFailedException($"The email was not sent. Subject: '{subject}' to users '{receivers}' with content: '{content}'");
            }

            return this;
        }
    }
}
