using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace Locators.Objects {
    internal class LoginPageGmail {
        public const string loginUrl = @"https://accounts.google.com/v3/signin/identifier?continue=https%3A%2F%2Fmail.google.com%2Fmail%2F&ifkv=AS5LTASaHn2VZ_hVI5-sXkLJ79kuqw1yNXW7mH5aMVjItFjUIwLYW8AjmQsbKtTJTM4H7yRyudyR&rip=1&sacu=1&service=mail&flowName=GlifWebSignIn&flowEntry=ServiceLogin&dsh=S755983298%3A1718092658242668&ddm=0";
        readonly IWebDriver driver;
        WebDriverWait wait;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driver">Driver which runs the search engine</param>
        public LoginPageGmail(IWebDriver driver) {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(driver, this);
        }

        #region WebElements
        [FindsBy(How = How.Name, Using = "identifier")]
        [CacheLookup]
        private readonly IWebElement loginField;

        [FindsBy(How = How.Name, Using = "Passwd")]
        [CacheLookup]
        private readonly IWebElement passwordField;

        [FindsBy(How = How.XPath, Using = "//div[@id='identifierNext']//button")]
        [CacheLookup]
        private readonly IWebElement proceedLoginButton;

        [FindsBy(How = How.XPath, Using = "//div[@id='passwordNext']//button")]
        [CacheLookup]
        private readonly IWebElement proceedPasswordButton;
        #endregion

        /// <summary>
        /// Enters the specified login on a Gmail login page and proceeds with it.
        /// </summary>
        /// <param name="login">Text to specify as the login</param>
        /// <param name="password">Text to specify as the password</param>
        /// <param name="waitTimeSecs"></param>
        /// <returns></returns>
        public bool EnterLoginAndProceed(string login, string? password=null) {
            driver.Navigate().GoToUrl(loginUrl);
            // Initially on login page
            // Enters login and proceeds
            loginField.SendKeys(login);
            proceedLoginButton.Click();

            // Waits for the password page
            try {
                wait.Until(d => !d.Url.Equals(loginUrl, StringComparison.OrdinalIgnoreCase));

                // Enters password and proceeds
                // ExpectedConditions.ElementExists(By.Name("Passwd"));
                passwordField.SendKeys(password);
                proceedPasswordButton.Click();

                // Returns if redirected to the main page
                return wait.Until(driver => driver.Title.Contains("Вхідні"));
            } catch (WebDriverTimeoutException) {
                return false;
            }
        }
    }
}
