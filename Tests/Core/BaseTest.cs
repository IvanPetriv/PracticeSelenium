using OpenQA.Selenium.Chrome;

namespace Locators.Tests.Core {
    public class BaseTest {
        /// <summary>
        /// Creates a Chrome driver to simulate actions in Google Chrome
        /// </summary>
        /// <returns>Chrome driver</returns>
        public static ChromeDriver GetChromeDriver() {
            string driverPath = @"D:\Development\ChromeDriver\";
            var options = new ChromeOptions();
            // options.AddArguments("--headless");

            return new ChromeDriver(driverPath, options, TimeSpan.FromSeconds(300));
        }
    }
}
