using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
    internal class ChangeUsernamePageGmail {
        public const string changeUsernameUrl = @"https://myaccount.google.com/profile/name?continue=https%3A%2F%2Fmyaccount.google.com%2Fpersonal-info%3Fhl%3Duk%26utm_source%3DOGB%26utm_medium%3Dact&hl=uk&utm_source=OGB&utm_medium=act";
        readonly IWebDriver driver;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driver">Driver which runs the search engine</param>
        public ChangeUsernamePageGmail(IWebDriver driver) {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        #region WebElements
        [FindsBy(How = How.XPath, Using = "//button[@aria-label='Змінити поле &quot;Ім’я&quot;']")]
        [CacheLookup]
        private readonly IWebElement changeUsernameButton;

        [FindsBy(How = How.XPath, Using = "//input[1]")]
        [CacheLookup]
        private readonly IWebElement enterNameInput;

        [FindsBy(How = How.XPath, Using = "//input[2]")]
        [CacheLookup]
        private readonly IWebElement enterSurnameInput;

        [FindsBy(How = How.LinkText, Using = "Зберегти")]
        [CacheLookup]
        private readonly IWebElement saveSettingsButton;
        #endregion

        /// <summary>
        /// Sets a new name and surname for logged in user
        /// </summary>
        /// <param name="name">New name for the user</param>
        /// <param name="surname">New surname for the user</param>
        public void ChangeUsername(string name, string surname) {
            driver.Navigate().GoToUrl(changeUsernameUrl);
            int waitTimeSecs = 3;

            // Proceeds with changing
            changeUsernameButton.Click();

            // Waits until inputs appear
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(waitTimeSecs));
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//input[2]")));

            // Inputs the data and proceeds
            enterNameInput.SendKeys(name);
            enterSurnameInput.SendKeys(surname);
            saveSettingsButton.Click();
        }
    }
}
