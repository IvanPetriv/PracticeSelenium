using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
    internal class ChangeUsernamePageGmail(IWebDriver driver) : AbstractObject(driver) {
        public const string changeUsernameUrl = @"https://myaccount.google.com/profile/name?continue=https%3A%2F%2Fmyaccount.google.com%2Fpersonal-info%3Fhl%3Duk%26utm_source%3DOGB%26utm_medium%3Dact&hl=uk&utm_source=OGB&utm_medium=act";

        #region WebElements
        [FindsBy(How = How.XPath, Using = "//button[contains(@aria-label, 'Змінити поле')]")]
        private readonly IWebElement changeUsernameButton = null!;

        [FindsBy(How = How.Id, Using = "i6")]
        private readonly IWebElement enterNameInput = null!;

        [FindsBy(How = How.Id, Using = "i11")]
        private readonly IWebElement enterSurnameInput = null!;

        [FindsBy(How = How.XPath, Using = "//span[contains(text(), 'Зберегти')]")]
        private readonly IWebElement saveSettingsButton = null!;
        #endregion

        /// <summary>
        /// Sets a new name and surname for logged in user
        /// </summary>
        /// <param name="name">New name for the user</param>
        /// <param name="surname">New surname for the user</param>
        public ChangeUsernamePageGmail ChangeUsername(string name, string surname) {
            driver.Navigate().GoToUrl(changeUsernameUrl);

            // Proceeds with changing
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(@aria-label, 'Змінити поле')]")));
            changeUsernameButton.Click();

            // Waits until inputs appear
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("i6")));

            // Inputs the data and proceeds
            enterNameInput.Clear();
            enterSurnameInput.Clear();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("i6")));
            enterNameInput.SendKeys(name);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("i11")));
            enterSurnameInput.SendKeys(surname);
            saveSettingsButton.Click();

            return this;
        }

        public List<string> GetUsername() {
            List<string> data = [];

            driver.Navigate().GoToUrl(changeUsernameUrl);

            // Proceeds with changing
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(@aria-label, 'Змінити поле')]")));
            changeUsernameButton.Click();

            // Waits until inputs appear
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("i6")));

            data.Add(enterNameInput.GetAttribute("value"));
            data.Add(enterSurnameInput.GetAttribute("value"));

            return data;
        }
    }
}
