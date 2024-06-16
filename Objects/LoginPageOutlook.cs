using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using Locators.Exceptions;

namespace Locators.Objects {
    internal class LoginPageOutlook(IWebDriver driver) : AbstractObject(driver) {
        public const string loginUrl = @"https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=153&ct=1718192229&rver=7.0.6738.0&wp=MBI_SSL&wreply=https%3a%2f%2foutlook.live.com%2fowa%2f%3fnlp%3d1%26cobrandid%3dab0455a0-8d03-46b9-b18b-df2f57b9e44c%26culture%3duk-ua%26country%3dua%26RpsCsrfState%3dea4458c4-24f8-43e3-14e9-877334bcd4e2&id=292841&aadredir=1&CBCXT=out&lw=1&fl=dob%2cflname%2cwld&cobrandid=ab0455a0-8d03-46b9-b18b-df2f57b9e44c";

        #region WebElements
        [FindsBy(How = How.Name, Using = "loginfmt")]
        private readonly IWebElement loginField = null!;

        [FindsBy(How = How.Name, Using = "passwd")]
        private readonly IWebElement passwordField = null!;

        [FindsBy(How = How.Id, Using = "idSIButton9")]
        private readonly IWebElement proceedLoginButton = null!;

        [FindsBy(How = How.Id, Using = "idSIButton9")]
        private readonly IWebElement proceedPasswordButton = null!;

        [FindsBy(How = How.Id, Using = "idSIButton9")]
        private readonly IWebElement proceedDontRemindButton = null!;
        #endregion

        /// <summary>
        /// Enters the specified login on a Gmail login page and proceeds with it.
        /// </summary>
        /// <param name="login">Text to specify as the login</param>
        /// <param name="password">Text to specify as the password</param>
        /// <param name="waitTimeSecs"></param>
        /// <returns></returns>
        public InboxPageOutlook EnterLoginAndProceed(string login, string? password=null) {
            driver.Navigate().GoToUrl(loginUrl);

            // Initially on login page
            // Enters login and proceeds
            try {
                wait.Until(ExpectedConditions.ElementExists(By.Name("loginfmt")));
            } catch (WebDriverTimeoutException) {
                throw new IncorrectUrlException($"Url does not have the specified element");
            }
            loginField.SendKeys(login);
            proceedLoginButton.Click();

            // Waits for the password page
            try {
                wait.Until(ExpectedConditions.ElementExists(By.Name("passwd")));
            } catch (WebDriverTimeoutException) {
                throw new LoginFailedException($"Log in Outlook has failed for login '{login}'");
            }

            // Enters the password and proceeds, proceeds with "Remember me"
            passwordField.SendKeys(password);
            proceedPasswordButton.Click();
            proceedDontRemindButton.Click();

            try {
                wait.Until(driver => driver.Title.Contains("Пошта"));
            } catch (WebDriverTimeoutException) {
                throw new LoginFailedException($"Log in Outlook has failed for login '{login}' and password '{password}'");
            }

            return new InboxPageOutlook(driver);
        }
    }
}
