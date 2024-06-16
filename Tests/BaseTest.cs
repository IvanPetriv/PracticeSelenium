using Locators.Driver;
using Locators.Utils;
using NLog;
using OpenQA.Selenium;

namespace Locators.Tests
{
    public class BaseTest {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected static Credentials credentials;
        protected static XmlTestsParser xmlParser = new(@"D:\LPNU\University\3-year\2-term\STP\Code-2\Locators\TestData\testsDev.xml");


        [OneTimeSetUp]
        public void SetUp() {
            //credentials = Credentials.DeserializeJson(@"..\..\..\TestData\TestCredentials.json");
        }

        [TearDown]
        public void TearDown() {

        }

        protected static void ActAfterTest(IWebDriver driver, DriverEngine engine) {
            Console.WriteLine(TestContext.CurrentContext.Result.FailCount);
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed) {
                Console.WriteLine(2);
                string testName = TestContext.CurrentContext.Test.Name;
                SessionScreenshot.TakeScreenshot(driver, engine, testName);
            }
        }
    }
}
