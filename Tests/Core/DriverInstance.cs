using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locators.Tests.Core {
    internal class DriverInstance : IDisposable {
        public IWebDriver Driver { get; private set; }

        public DriverInstance() {
            Driver = GetChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Creates a Chrome driver to simulate actions in Google Chrome
        /// </summary>
        /// <returns>Chrome driver</returns>
        private static ChromeDriver GetChromeDriver() {
            string driverPath = @"D:\Development\ChromeDriver\";
            var options = new ChromeOptions();
            // options.AddArguments("--headless");

            return new ChromeDriver(driverPath, options, TimeSpan.FromSeconds(300));
        }

        public void Dispose() {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
