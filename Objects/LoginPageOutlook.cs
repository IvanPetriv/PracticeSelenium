using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Locators.Objects {
    internal class LoginPageOutlook {
        public const string loginUrl = @"https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=153&ct=1718192229&rver=7.0.6738.0&wp=MBI_SSL&wreply=https%3a%2f%2foutlook.live.com%2fowa%2f%3fnlp%3d1%26cobrandid%3dab0455a0-8d03-46b9-b18b-df2f57b9e44c%26culture%3duk-ua%26country%3dua%26RpsCsrfState%3dea4458c4-24f8-43e3-14e9-877334bcd4e2&id=292841&aadredir=1&CBCXT=out&lw=1&fl=dob%2cflname%2cwld&cobrandid=ab0455a0-8d03-46b9-b18b-df2f57b9e44c";
        readonly IWebDriver driver;
        WebDriverWait wait;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driver">Driver which runs the search engine</param>
        public LoginPageOutlook(IWebDriver driver) {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(driver, this);
        }

        #region WebElements
        [FindsBy(How = How.Name, Using = "loginfmt")]
        [CacheLookup]
        private readonly IWebElement loginField;

        [FindsBy(How = How.Name, Using = "passwd")]
        [CacheLookup]
        private readonly IWebElement passwordField;

        [FindsBy(How = How.Id, Using = "idSIButton9")]
        [CacheLookup]
        private readonly IWebElement proceedLoginButton;

        [FindsBy(How = How.Id, Using = "idSIButton9")]
        [CacheLookup]
        private readonly IWebElement proceedPasswordButton;

        [FindsBy(How = How.Id, Using = "idSIButton9")]
        [CacheLookup]
        private readonly IWebElement proceedDontRemindButton;
        #endregion

        /// <summary>
        /// Enters the specified login on a Gmail login page and proceeds with it.
        /// </summary>
        /// <param name="login">Text to specify as the login</param>
        /// <param name="password">Text to specify as the password</param>
        /// <param name="waitTimeSecs"></param>
        /// <returns></returns>
        public bool EnterLoginAndProceed(string login, string? password=null, int waitTimeSecs = 10) {
            driver.Navigate().GoToUrl(loginUrl);

            // Initially on login page
            // Enters login and proceeds
            loginField.SendKeys(login);
            proceedLoginButton.Click();

            // Waits for the password page
            try {
                wait.Until(d => !d.Url.Equals(loginUrl, StringComparison.OrdinalIgnoreCase));

                // Enters the password and proceeds, proceeds with "Remember me"
                wait.Until(ExpectedConditions.ElementExists(By.Name("passwd")));
                passwordField.SendKeys(password);
                proceedPasswordButton.Click();
                proceedDontRemindButton.Click();

                return wait.Until(driver => driver.Title.Contains("Пошта"));
            } catch (WebDriverTimeoutException) {
                return false;
            }
        }
    }
}
