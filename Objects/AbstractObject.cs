using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Locators.Objects {
    public abstract class AbstractObject {
        protected readonly IWebDriver driver;
        protected readonly WebDriverWait wait;

        public AbstractObject(IWebDriver driver) {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            PageFactory.InitElements(driver, this);
        }
    }
}
