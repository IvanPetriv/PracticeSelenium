using OpenQA.Selenium;

namespace Locators.Driver {
    internal class SessionScreenshot {
        public const string SCREENSHOTS_DIR = @"D:\LPNU\University\3-year\2-term\STP\Code-2\Locators\Output\Screenshots\";

        public static void TakeScreenshot(IWebDriver driver, DriverEngine engine, string funcName) {
            ArgumentNullException.ThrowIfNull(driver);

            Console.WriteLine(DateTime.Now);

            string screenshotName = $"[{DateTime.Now:dd-MM-yyyy_HH.mm.ss}]_'{funcName}'_on_'{engine}'.png";

            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(SCREENSHOTS_DIR + screenshotName);
        }
    }
}
