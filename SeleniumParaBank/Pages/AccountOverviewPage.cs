using OpenQA.Selenium;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Page Object for the Accounts Overview page.
    /// URL: /parabank/overview.htm
    /// </summary>
    public class AccountOverviewPage : BasePage
    {
        // ── Locators ──────────────────────────────────────────────────────────
        private readonly By _welcomeMessage    = By.CssSelector("#rightPanel .smallText");
        private readonly By _accountsTable     = By.Id("accountTable");
        private readonly By _accountRows       = By.CssSelector("#accountTable tbody tr");
        private readonly By _overviewHeading   = By.CssSelector("#rightPanel h1");
        private readonly By _totalBalance      = By.CssSelector("#accountTable tfoot td:nth-child(2)");
        private readonly By _overviewLink      = By.LinkText("Accounts Overview");

        // ── Constructor ───────────────────────────────────────────────────────
        public AccountOverviewPage(IWebDriver driver) : base(driver) { }

        // ── Actions ───────────────────────────────────────────────────────────

        public void ClickOverviewLink()
            => Click(_overviewLink);

        public void ClickAccountById(string accountNumber)
            => Click(By.LinkText(accountNumber));

        // ── Assertions / Getters ─────────────────────────────────────────────

        public bool IsAccountsTableDisplayed()
            => IsDisplayed(_accountsTable);

        public string GetHeadingText()
            => IsDisplayed(_overviewHeading) ? GetText(_overviewHeading) : string.Empty;

        public int GetAccountRowCount()
        {
            try
            {
                var rows = Driver.FindElements(_accountRows);
                return rows.Count(r => r.FindElements(By.TagName("td")).Count > 1);
            }
            catch { return 0; }
        }

        public bool HasAccountInTable(string accountNumber)
        {
            try
            {
                return Driver.FindElements(By.LinkText(accountNumber)).Count > 0;
            }
            catch { return false; }
        }

        public string GetTotalBalance()
            => IsDisplayed(_totalBalance) ? GetText(_totalBalance) : string.Empty;
    }
}
