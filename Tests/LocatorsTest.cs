using OpenQA.Selenium;
using Locators.Objects;
using Locators.Driver;
using Locators.Exceptions;
using Locators.Models;
using Locators.Utils;

namespace Locators.Tests {
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class LocatorsTests : BaseTest {
        private static readonly IEnumerable<TestCaseData> LoginGmailData = [
            new TestCaseData(new User("gm.sg", null)).Returns(typeof(LoginFailedException)),
            new TestCaseData(new User("ivan.petriv.pz.2021@lpnu.ua", "dA0Fk8vs")).Returns(typeof(InboxPageGmail)),
        ];

        private static readonly IEnumerable<TestCaseData> LoginOutlookData = [
            new TestCaseData(new User("gm.sg", null)).Returns(typeof(LoginFailedException)),
            new TestCaseData(new User("ivan.petriv.pz.2021@edu.lpnu.ua", "dA0Fk8vs")).Returns(typeof(InboxPageOutlook)),
        ];

        private static IEnumerable<TestCaseData> LoginGmailDataa() => xmlParser.GetTestData("LocatorsTests", "LoginGmailOnChromeTest", "TestSuite");

        //[Test, TestCaseSource(nameof(LoginGmailData))]
        [Test, TestCaseSource(nameof(LoginGmailDataa))]
        public Type LoginGmailOnChromeTest(User user) {
            return LoginGmailTests(user, DriverEngine.Chrome);
        }

        [Test, TestCaseSource(nameof(LoginGmailData))]
        public Type LoginGmailOnFirefoxTest(User user) {
            return LoginGmailTests(user, DriverEngine.Firefox);
        }

        [Test, TestCaseSource(nameof(LoginGmailData))]
        public Type LoginGmailOnEdgeTest(User user) {
            return LoginGmailTests(user, DriverEngine.Edge);
        }

        private Type LoginGmailTests(User user, DriverEngine engine) {
            using IWebDriver driver = DriverSetup.GetDriverSetup(engine);
            try {
                var loginPage = new LoginPageGmail(driver);

                AbstractObject resultPage = loginPage.EnterLoginAndProceed(user.Login, user.Password);

                return resultPage.GetType();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return ex.GetType();
            }
        }

        [Test, TestCaseSource(nameof(LoginOutlookData))]
        public Type LoginOutlookTests(User user) {
            using IWebDriver driver = DriverSetup.GetDriverSetup(DriverEngine.Chrome);
            try {
                var loginPage = new LoginPageOutlook(driver);

                AbstractObject resultPage = loginPage.EnterLoginAndProceed(user.Login, user.Password);

                return resultPage.GetType();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return ex.GetType();
            }
        }

        /*

        [TestCase("ivan.petriv.pz.2021@edu.lpnu.ua", "Test Subject", "Test Body")]
        public void SendEmail(string recipient, string subject, string body) {
            using DriverInstance driverInstance = new();
            IWebDriver driver = driverInstance.Driver;
            var gmailLoginPage = new LoginPageGmail(driver);

            gmailLoginPage.EnterLoginAndProceed(credentials.Google[0].Login, credentials.Google[0].Password);

            var inboxPage = new InboxPageGmail(driver);
            inboxPage.SendLetter(recipient, subject, body);
        }

        [TestCase("ivan.petriv.pz.2021@edu.lpnu.ua", "test", "Hi hello")]
        public void CheckIfReceivedCorrectEmail(string receiver, string subject, string content) {
            using DriverInstance driverInstance = new();
            IWebDriver driver = driverInstance.Driver;
            var gmailLoginPage = new LoginPageGmail(driver);

            gmailLoginPage.EnterLoginAndProceed(credentials.Google[0].Login, credentials.Google[0].Password);

            var sendingLetter = new InboxPageGmail(driver);
            sendingLetter.SendLetter(receiver, subject, content);

            var outlookLoginPage = new LoginPageOutlook(driver);
            gmailLoginPage.EnterLoginAndProceed(credentials.Microsoft[0].Login, credentials.Microsoft[0].Password);

            var outlookInboxPage = new InboxPageOutlook(driver);
            outlookInboxPage.ReadLetter();
        }

        [TestCase("Ivan", "Petriv")]
        public void ChangeUsername(string name, string username) {
            using DriverInstance driverInstance = new();
            IWebDriver driver = driverInstance.Driver;
            driver.Navigate().GoToUrl(LoginPageGmail.loginUrl);
            var gmailLoginPage = new LoginPageGmail(driver);

            gmailLoginPage.EnterLoginAndProceed(credentials.Google[0].Login, credentials.Google[0].Password);

            var usernamePage = new ChangeUsernamePageGmail(driver);
            usernamePage.ChangeUsername(name, username);
        }*/
    }
}