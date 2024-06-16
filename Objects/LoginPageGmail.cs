using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using Locators.Exceptions;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
    internal class LoginPageGmail(IWebDriver driver) : AbstractObject(driver) {
        public const string loginUrl = @"https://accounts.google.com/v3/signin/identifier?continue=https%3A%2F%2Fmail.google.com%2Fmail%2F&ifkv=AS5LTASaHn2VZ_hVI5-sXkLJ79kuqw1yNXW7mH5aMVjItFjUIwLYW8AjmQsbKtTJTM4H7yRyudyR&rip=1&sacu=1&service=mail&flowName=GlifWebSignIn&flowEntry=ServiceLogin&dsh=S755983298%3A1718092658242668&ddm=0";

        #region WebElements
        [FindsBy(How = How.Name, Using = "identifier")]
        [CacheLookup]
        private readonly IWebElement loginField = null!;

        [FindsBy(How = How.Name, Using = "Passwd")]
        [CacheLookup]
        private readonly IWebElement passwordField = null!;

        [FindsBy(How = How.XPath, Using = "//div[@id='identifierNext']//button")]
        [CacheLookup]
        private readonly IWebElement proceedLoginButton = null!;

        [FindsBy(How = How.XPath, Using = "//div[@id='passwordNext']//button")]
        [CacheLookup]
        private readonly IWebElement proceedPasswordButton = null!;
        #endregion

        /// <summary>
        /// Enters the specified login on a Gmail login page and proceeds with it.
        /// </summary>
        /// <param name="login">Text to specify as the login</param>
        /// <param name="password">Text to specify as the password</param>
        /// <param name="waitTimeSecs"></param>
        public InboxPageGmail EnterLoginAndProceed(string login, string? password=null) {
            driver.Navigate().GoToUrl(loginUrl);

            // Initially on login page
            // Enters login and proceeds
            loginField.SendKeys(login);
            proceedLoginButton.Click();

            // Waits for the password page
            try {
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("Passwd")));
            } catch (WebDriverTimeoutException) {
                throw new LoginFailedException($"Log in Gmail has failed for login '{login}'");
            }

            // Enters password and proceeds
            passwordField.SendKeys(password);
            proceedPasswordButton.Click();

            // Returns if redirected to the main page
            try {
                wait.Until(driver => driver.Title.Contains("Вхідні"));
            } catch (WebDriverTimeoutException) {
                throw new LoginFailedException($"Log in Gmail has failed for login '{login}' and password '{password}'");
            }

            return new InboxPageGmail(driver);
        }
    }
}
