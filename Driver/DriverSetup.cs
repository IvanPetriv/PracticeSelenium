using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Locators.Driver {
    public class DriverSetup : IDisposable {
        private const string CHROME_DRIVER_DIRECTORY = @"D:\Development\WebDrivers\chromedriver.exe";
        private const string FIREFOX_DRIVER_DIRECTORY = @"D:\Development\WebDrivers\geckodriver.exe";
        private const string EDGE_DRIVER_DIRECTORY = @"D:\Development\WebDrivers\msedgedriver.exe";

        public static IWebDriver? Driver { get; private set; } = null;

        public static IWebDriver GetDriverSetup(DriverEngine engine = DriverEngine.None) {
            if (Driver is null) {
                switch (engine) {
                    case DriverEngine.Chrome: {
                        var options = new ChromeOptions();
                        // options.AddArguments("--headless");
                        Driver = new ChromeDriver(CHROME_DRIVER_DIRECTORY, options, TimeSpan.FromSeconds(300));
                        break;
                    }
                    case DriverEngine.Firefox: {
                        var options = new FirefoxOptions();
                        // options.AddArguments("--headless");
                        Driver = new FirefoxDriver(FIREFOX_DRIVER_DIRECTORY, options, TimeSpan.FromSeconds(300));
                        break;
                    }
                    case DriverEngine.Edge: {
                        var options = new EdgeOptions();
                        // options.AddArguments("--headless");
                        Driver = new EdgeDriver(EDGE_DRIVER_DIRECTORY, options, TimeSpan.FromSeconds(300));
                        break;
                    }
                    default: {
                        throw new ArgumentException($"The driver '{engine}' is not supported");
                    }
                }
            }

            return Driver;
        }

        public void Dispose() {
            Driver?.Quit();
            Driver?.Dispose();
            Driver = null;
        }
    }
}
