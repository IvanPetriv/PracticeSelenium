using OpenQA.Selenium;
using Locators.Objects;
using Locators.Driver;
using Locators.Models;
using Locators.Exceptions;

namespace Locators.Tests {
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class LocatorsTests : BaseTest {
        private static IEnumerable<TestCaseData> LoginGmailData() => xmlParser.GetTestData("LocatorsTests", "LoginGmailTest", "TestSuite");
        private static IEnumerable<TestCaseData> LoginOutlookData() => xmlParser.GetTestData("LocatorsTests", "LoginOutlookTest", "TestSuite");
        private static IEnumerable<TestCaseData> SendEmailData() => xmlParser.GetTestData("LocatorsTests", "SendEmail", "TestSuite");
        private static IEnumerable<TestCaseData> ChangeUsernameData() => xmlParser.GetTestData("LocatorsTests", "ChangeUsername", "TestSuite");


        [Test, TestCaseSource(nameof(LoginGmailData))]
        public Type LoginGmailTests(User user) {
            using IWebDriver driver = DriverSetup.GetDriverSetup(driverEngine);
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
            using IWebDriver driver = DriverSetup.GetDriverSetup(driverEngine);
            try {
                var loginPage = new LoginPageOutlook(driver);

                AbstractObject resultPage = loginPage.EnterLoginAndProceed(user.Login, user.Password);

                return resultPage.GetType();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return ex.GetType();
            }
        }

        [Test, TestCaseSource(nameof(SendEmailData))]
        public void SendEmail(User gmailUser, User outlookUser, Letter letter) {
            using IWebDriver driver = DriverSetup.GetDriverSetup(driverEngine);
            try {
                AbstractObject sendingFromGoogle = new LoginPageGmail(driver)
                    .EnterLoginAndProceed(gmailUser.Login, gmailUser.Password)
                    .SendLetter(letter.Receiver, letter.Subject, letter.Content);

                Thread.Sleep(15000);

                AbstractObject receivingInOutlook = new LoginPageOutlook(driver)
                    .EnterLoginAndProceed(outlookUser.Login, outlookUser.Password)
                    .ReadLetter(out Letter letterData, 0);

                Assert.Multiple(() => {
                    Assert.That(letterData.Receiver, Is.EqualTo(gmailUser.Login));
                    Assert.That(letterData.Subject, Is.EqualTo(letter.Subject));
                    Assert.That(letterData.Content, Is.EqualTo(letter.Content));
                });
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Assert.That(ex.GetType(), Is.EqualTo(typeof(IncorrectEmailException)));
            }
        }

        [Test, TestCaseSource(nameof(ChangeUsernameData))]
        public void ChangeUsername(User user, string name, string surname) {
            using IWebDriver driver = DriverSetup.GetDriverSetup(driverEngine);
            try {
                var loginPage = new LoginPageGmail(driver);

                AbstractObject resultPage = loginPage.EnterLoginAndProceed(user.Login, user.Password);
                var changePage = new ChangeUsernamePageGmail(driver);
                changePage.ChangeUsername(name, surname);

                var data = changePage.GetUsername();
                Assert.Multiple(() => {
                    Assert.That(data[0], Is.EqualTo(name));
                    Assert.That(data[1], Is.EqualTo(surname));
                });

                //return resultPage.GetType();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
                //return ex.GetType();
            }
        }
    }
}