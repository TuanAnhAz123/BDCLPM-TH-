using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumParaBank.Utilities;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Base class for all Page Objects.
    /// Contains shared helper methods.
    /// </summary>
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WaitHelper Wait;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait   = new WaitHelper(driver);
        }

        protected IWebElement FindElement(By locator)
            => Wait.WaitForVisible(locator);

        protected void Click(By locator)
            => Wait.WaitForClickable(locator).Click();

        protected void Type(By locator, string text)
        {
            var el = Wait.WaitForVisible(locator);
            el.Clear();
            el.SendKeys(text);
        }

        protected string GetText(By locator)
            => FindElement(locator).Text;

        protected bool IsDisplayed(By locator)
        {
            try
            {
                var el = Driver.FindElement(locator);
                return el.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                try { return Driver.FindElement(locator).Displayed; }
                catch { return false; }
            }
            catch { return false; }
        }

        protected void SelectByText(By locator, string text)
        {
            var select = new SelectElement(FindElement(locator));
            select.SelectByText(text);
        }

        protected void SelectByValue(By locator, string value)
        {
            var select = new SelectElement(FindElement(locator));
            select.SelectByValue(value);
        }

        protected IList<IWebElement> GetOptions(By locator)
        {
            var select = new SelectElement(FindElement(locator));
            return select.Options;
        }

        protected string GetAttribute(By locator, string attr)
            => FindElement(locator).GetAttribute(attr) ?? string.Empty;

        /// <summary>Short pause to let AJAX responses load.</summary>
        protected void WaitForAjax(int ms = 1000)
            => Thread.Sleep(ms);
    }
}
