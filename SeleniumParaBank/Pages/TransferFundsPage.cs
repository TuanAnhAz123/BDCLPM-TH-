using OpenQA.Selenium;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Page Object for the Transfer Funds page.
    /// URL: /parabank/transfer.htm
    /// </summary>
    public class TransferFundsPage : BasePage
    {
        // ── Locators ──────────────────────────────────────────────────────────
        private readonly By _amountInput       = By.Id("amount");
        private readonly By _fromAccountSelect = By.Id("fromAccountId");
        private readonly By _toAccountSelect   = By.Id("toAccountId");
        private readonly By _transferButton    = By.CssSelector("input[value='Transfer']");
        private readonly By _successMessage    = By.CssSelector("#showResult h1, #rightPanel h1.title, #rightPanel .title");
        private readonly By _errorMessage      = By.CssSelector("#showError .error, #rightPanel .error, #rightPanel p.error");
        private readonly By _transferLink      = By.LinkText("Transfer Funds");

        // ── Constructor ───────────────────────────────────────────────────────
        public TransferFundsPage(IWebDriver driver) : base(driver) { }

        // ── Actions ───────────────────────────────────────────────────────────

        public void ClickTransferLink()
        {
            Click(_transferLink);
            // Wait for AJAX-loaded form elements
            WaitForAjax(1500);
        }

        public void EnterAmount(string amount)
            => Type(_amountInput, amount);

        public void SelectFromAccount(int index = 0)
        {
            var opts = GetOptions(_fromAccountSelect);
            if (opts.Count > index) opts[index].Click();
        }

        public void SelectToAccount(int index = 1)
        {
            var opts = GetOptions(_toAccountSelect);
            if (opts.Count > index) opts[index].Click();
        }

        public void SelectToAccountByValue(string value)
            => SelectByValue(_toAccountSelect, value);

        public void ClickTransferButton()
            => Click(_transferButton);

        public void Transfer(string amount, int fromIndex = 0, int toIndex = 1)
        {
            EnterAmount(amount);
            SelectFromAccount(fromIndex);
            SelectToAccount(toIndex);
            ClickTransferButton();
            // Wait for AJAX result
            WaitForAjax(2000);
        }

        // ── Assertions / Getters ─────────────────────────────────────────────

        public bool IsTransferSuccessful()
        {
            if (IsDisplayed(_successMessage) &&
                GetText(_successMessage).Contains("Transfer Complete"))
                return true;
            return Driver.PageSource.Contains("Transfer Complete");
        }

        public bool IsErrorMessageDisplayed()
            => IsDisplayed(_errorMessage) || Driver.PageSource.Contains("error");

        public string GetSuccessMessage()
            => IsDisplayed(_successMessage) ? GetText(_successMessage) : string.Empty;

        public string GetErrorMessage()
            => IsDisplayed(_errorMessage) ? GetText(_errorMessage) : string.Empty;

        public bool IsAmountInputDisplayed()
            => IsDisplayed(_amountInput);

        public bool IsFromAccountDropdownDisplayed()
            => IsDisplayed(_fromAccountSelect);

        public bool IsToAccountDropdownDisplayed()
            => IsDisplayed(_toAccountSelect);
    }
}
