using OpenQA.Selenium;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Page Object for Find Transactions page.
    /// URL: /parabank/findtrans.htm
    /// </summary>
    public class FindTransactionsPage : BasePage
    {
        // ── Locators ──────────────────────────────────────────────────────────
        private readonly By _accountSelect        = By.Id("accountId");
        private readonly By _findByDateButton     = By.Id("findById");
        private readonly By _transactionIdInput   = By.Id("criteria.transactionId");
        private readonly By _findByDateInput      = By.Id("criteria.onDate");
        private readonly By _findByDateBtn        = By.CssSelector("button[ng-click*='onDate']");
        private readonly By _findByAmountInput    = By.Id("criteria.amount");
        private readonly By _findByAmountBtn      = By.CssSelector("button[ng-click*='amount']");
        private readonly By _resultsTable         = By.Id("transactionTable");
        private readonly By _noResultsMessage     = By.CssSelector("#rightPanel .error");
        private readonly By _findTransLink        = By.LinkText("Find Transactions");

        // ── Constructor ───────────────────────────────────────────────────────
        public FindTransactionsPage(IWebDriver driver) : base(driver) { }

        // ── Actions ───────────────────────────────────────────────────────────

        public void ClickFindTransLink()
            => Click(_findTransLink);

        public void SelectAccount(int index = 0)
        {
            var opts = GetOptions(_accountSelect);
            if (opts.Count > index) opts[index].Click();
        }

        public void FindByDate(string date)
        {
            Type(_findByDateInput, date);
            Click(_findByDateBtn);
        }

        public void FindByAmount(string amount)
        {
            Type(_findByAmountInput, amount);
            Click(_findByAmountBtn);
        }

        // ── Assertions / Getters ─────────────────────────────────────────────

        public bool IsResultsTableDisplayed()
            => IsDisplayed(_resultsTable);

        public bool IsNoResultMessageDisplayed()
            => IsDisplayed(_noResultsMessage);

        public int GetResultRowCount()
        {
            try
            {
                var rows = Driver.FindElements(By.CssSelector("#transactionTable tbody tr"));
                return rows.Count;
            }
            catch { return 0; }
        }

        public string GetNoResultMessage()
            => IsDisplayed(_noResultsMessage) ? GetText(_noResultsMessage) : string.Empty;
    }
}
