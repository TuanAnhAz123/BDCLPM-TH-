using OpenQA.Selenium;

namespace SeleniumParaBank.Utilities
{
    /// <summary>
    /// Helper class to capture screenshots on test failure.
    /// </summary>
    public static class ScreenshotHelper
    {
        private static readonly string ScreenshotDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Reports", "Screenshots");

        public static string? Capture(IWebDriver? driver, string testName)
        {
            if (driver == null)
            {
                Console.WriteLine("[Screenshot] Driver is null. Cannot capture screenshot.");
                return null;
            }

            Directory.CreateDirectory(ScreenshotDir);

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName  = $"{testName}_{timestamp}.png";
            var fullPath  = Path.Combine(ScreenshotDir, fileName);

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            Console.WriteLine($"[Screenshot] Saved: {fullPath}");
            return fullPath;
        }
    }
}
