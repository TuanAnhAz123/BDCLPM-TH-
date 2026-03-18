using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using SeleniumParaBank.Utilities;

namespace SeleniumParaBank.Tests
{
    /// <summary>
    /// Base class for all test classes.
    /// Handles driver setup/teardown and screenshot on failure.
    /// </summary>
    [TestFixture]
    public abstract class BaseTest
    {
        protected IWebDriver Driver   = null!;
        protected string     BaseUrl  = null!;

        [SetUp]
        public void SetUp()
        {
            Driver  = DriverFactory.GetDriver();
            BaseUrl = TestDataHelper.BaseUrl;
            Driver.Navigate().GoToUrl(BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            // Capture screenshot on any failure
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                if (Driver != null)
                {
                    var testName = TestContext.CurrentContext.Test.Name;
                    ScreenshotHelper.Capture(Driver, testName);
                }
                else
                {
                    Console.WriteLine("[Screenshot] Driver is null, skipping screenshot capture.");
                }
            }
            DriverFactory.QuitDriver();
        }
    }
}
