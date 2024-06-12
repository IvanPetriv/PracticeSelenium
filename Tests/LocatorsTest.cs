using OpenQA.Selenium;
using Locators.Objects;
using Locators.Tests.Core;

namespace Locators.Tests {
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class LocatorsTests {
        [TestCase("gm.sg", ExpectedResult = false)]
        [TestCase("justfobos426@gmail.com", ExpectedResult = true)]
        [TestCase("ivan.petriv.pz.2021@lpnu.ua", "dA0Fk8vs", ExpectedResult = true)]
        public bool LoginGmailValidityTest(string login, string? password=null) {
            using DriverInstance driverInstance = new();
            IWebDriver driver = driverInstance.Driver;
            driver.Navigate().GoToUrl(LoginPageGmail.loginUrl);
            var test = new LoginPageGmail(driver);

            bool result = test.EnterLoginAndProceed(login, password, 10);

            return result;
        }

        //[TestCase("gm.sg", ExpectedResult = false)]
        //[TestCase("justfobos426@gmail.com", ExpectedResult = true)]
        [TestCase("ivan.petriv.pz.2021@edu.lpnu.ua", "dA0Fk8vs", ExpectedResult = true)]
        public bool LoginOutlookValidityTest(string login, string? password = null) {
            using DriverInstance driverInstance = new();
            IWebDriver driver = driverInstance.Driver;
            driver.Navigate().GoToUrl(LoginPageOutlook.loginUrl);
            var test = new LoginPageOutlook(driver);

            bool result = test.EnterLoginAndProceed(login, password, 10);

            return result;
        }

        [TestCase("justfobos426@gmail.com", "test", "Hi hello")]
        public void SendEmailAndCheckIfReceivedTest(string sender, string title, string letter) {
            using DriverInstance driverInstance = new();
            IWebDriver driver = driverInstance.Driver;
            driver.Navigate().GoToUrl(LoginPageGmail.loginUrl);
            var login = new LoginPageGmail(driver);

            bool result = login.EnterLoginAndProceed("ivan.petriv.pz.2021@lpnu.ua", "dA0Fk8vs", waitTimeSecs: 10);
            Assert.That(result, Is.True, "Could not log in");

            var sendingLetter = new InboxPageGmail(driver);
            sendingLetter.SendLetter(sender, title, letter);
        }

        [TestCase("ivan.petriv.pz.2021@edu.lpnu.ua", "test", "Hi hello")]
        public void CheckIfReceivedCorrectEmail(string receiver, string subject, string content) {
            using DriverInstance driverInstance = new();
            IWebDriver driver = driverInstance.Driver;
            driver.Navigate().GoToUrl(LoginPageOutlook.loginUrl);
            //var login = new LoginPageGmail(driver);

            //bool result = login.EnterLoginAndProceed("ivan.petriv.pz.2021@lpnu.ua", "dA0Fk8vs", waitTimeSecs: 10);
            //Assert.That(result, Is.True, "Could not log in");

            //var sendingLetter = new InboxPageGmail(driver);
            //sendingLetter.SendLetter(sender, title, letter);

            var login = new LoginPageOutlook(driver);
            login.EnterLoginAndProceed("ivan.petriv.pz.2021@edu.lpnu.ua", "dA0Fk8vs");

            var receive = new InboxPageOutlook(driver);
            receive.ReadLetter();
        }

        [TestCase("Ivan", "Petriv")]
        public void ChangeUsername(string name, string username) {
            using DriverInstance driverInstance = new();
            IWebDriver driver = driverInstance.Driver;
            driver.Navigate().GoToUrl(LoginPageGmail.loginUrl);
            var login = new LoginPageGmail(driver);

            bool result = login.EnterLoginAndProceed("ivan.petriv.pz.2021@lpnu.ua", "dA0Fk8vs", waitTimeSecs: 10);
            Assert.That(result, Is.True, "Could not log in");

            var sendingLetter = new InboxPageGmail(driver);
            sendingLetter.ChangeUsername(name, username);
        }
    }
}