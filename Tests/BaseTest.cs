using Locators.Driver;
using Locators.Utils;
using NLog;
using OpenQA.Selenium;

namespace Locators.Tests
{
    public class BaseTest {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        protected static DriverEngine driverEngine = DriverEngine.Firefox;
        protected static string environment = Environment.GetEnvironmentVariable("env")
            ?? throw new ArgumentNullException($"Could not receive 'env' parameter"); // dev, qa
        protected static string testSuite = Environment.GetEnvironmentVariable("testSuite")
            ?? throw new ArgumentNullException($"Could not receive 'testSuite' parameter"); // Smoke, All

        protected static XmlTestsParser xmlParser = new(@"D:\LPNU\University\3-year\2-term\STP\Code-2\Locators\TestData\" + (environment == "dev" ? "testsDev.xml" : "testsQa.xml"));


        [OneTimeSetUp]
        public void SetUp() {
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
