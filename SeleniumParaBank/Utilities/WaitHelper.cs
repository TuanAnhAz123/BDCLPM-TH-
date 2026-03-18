using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumParaBank.Utilities
{
    /// <summary>
    /// Helper class for explicit waits — no SeleniumExtras dependency needed.
    /// </summary>
    public class WaitHelper
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WaitHelper(IWebDriver driver, int timeoutSeconds = 15)
        {
            _driver = driver;
            _wait   = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        }

        public IWebElement WaitForVisible(By locator)
        {
            return _wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(locator);
                    return (el != null && el.Displayed) ? el : null;
                }
                catch (NoSuchElementException) { return null; }
                catch (StaleElementReferenceException) { return null; }
            })!;
        }

        public IWebElement WaitForClickable(By locator)
        {
            return _wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(locator);
                    return (el != null && el.Displayed && el.Enabled) ? el : null;
                }
                catch (NoSuchElementException) { return null; }
                catch (StaleElementReferenceException) { return null; }
            })!;
        }

        public bool WaitForTextPresent(By locator, string text)
        {
            try
            {
                return _wait.Until(d =>
                {
                    try { return d.FindElement(locator).Text.Contains(text); }
                    catch (StaleElementReferenceException) { return false; }
                    catch (NoSuchElementException) { return false; }
                });
            }
            catch (WebDriverTimeoutException) { return false; }
        }

        public bool WaitForUrlContains(string urlFragment)
        {
            try
            {
                return _wait.Until(d => d.Url.Contains(urlFragment));
            }
            catch (WebDriverTimeoutException) { return false; }
        }

        public bool WaitForElementPresent(By locator)
        {
            try
            {
                _wait.Until(d =>
                {
                    try { return d.FindElement(locator) != null; }
                    catch (NoSuchElementException) { return false; }
                    catch (StaleElementReferenceException) { return false; }
                });
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        /// <summary>Wait for document.readyState to be 'complete'.</summary>
        public void WaitForPageReady()
        {
            try
            {
                _wait.Until(d =>
                {
                    try
                    {
                        return ((IJavaScriptExecutor)d)
                            .ExecuteScript("return document.readyState")
                            ?.ToString() == "complete";
                    }
                    catch { return false; }
                });
            }
            catch (WebDriverTimeoutException) { }
        }

        /// <summary>Wait for an element's attribute to have a non-empty value.</summary>
        public bool WaitForAttributeNotEmpty(By locator, string attribute)
        {
            try
            {
                return _wait.Until(d =>
                {
                    try
                    {
                        var el = d.FindElement(locator);
                        var val = el.GetAttribute(attribute);
                        return !string.IsNullOrEmpty(val);
                    }
                    catch (NoSuchElementException) { return false; }
                    catch (StaleElementReferenceException) { return false; }
                });
            }
            catch (WebDriverTimeoutException) { return false; }
        }

        /// <summary>Wait until either of two conditions is met (e.g. success or error).</summary>
        public bool WaitForEither(By locator1, By locator2)
        {
            try
            {
                return _wait.Until(d =>
                {
                    try
                    {
                        var els1 = d.FindElements(locator1);
                        if (els1.Count > 0 && els1[0].Displayed) return true;
                    }
                    catch (StaleElementReferenceException) { }

                    try
                    {
                        var els2 = d.FindElements(locator2);
                        if (els2.Count > 0 && els2[0].Displayed) return true;
                    }
                    catch (StaleElementReferenceException) { }

                    return false;
                });
            }
            catch (WebDriverTimeoutException) { return false; }
        }
    }
}
