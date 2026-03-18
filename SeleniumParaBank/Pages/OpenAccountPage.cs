using OpenQA.Selenium;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Page Object for the Open New Account page.
    /// URL: /parabank/openaccount.htm
    /// </summary>
    public class OpenAccountPage : BasePage
    {
        // ── Locators ──────────────────────────────────────────────────────────
        private readonly By _accountTypeSelect  = By.Id("type");
        private readonly By _fromAccountSelect  = By.Id("fromAccountId");
        private readonly By _openAccountButton  = By.CssSelector("input[value='Open New Account']");
        private readonly By _newAccountNumber   = By.Id("newAccountId");
        private readonly By _successHeading     = By.CssSelector("#openAccountResult h1, #rightPanel h1.title, #rightPanel .title");
        private readonly By _openAccountLink    = By.LinkText("Open New Account");

        // ── Constructor ───────────────────────────────────────────────────────
        public OpenAccountPage(IWebDriver driver) : base(driver) { }

        // ── Actions ───────────────────────────────────────────────────────────

        public void ClickOpenAccountLink()
        {
            Click(_openAccountLink);
            // Wait for AJAX-loaded form
            WaitForAjax(1500);
        }

        /// <summary>account type: "CHECKING" or "SAVINGS"</summary>
        public void SelectAccountType(string type)
            => SelectByText(_accountTypeSelect, type);

        public void SelectFromAccount(int index = 0)
        {
            var options = GetOptions(_fromAccountSelect);
            if (options.Count > index)
                options[index].Click();
        }

        public void ClickOpenButton()
            => Click(_openAccountButton);

        public string OpenNewAccount(string accountType)
        {
            SelectAccountType(accountType);
            SelectFromAccount();
            ClickOpenButton();
            // Wait for AJAX result
            WaitForAjax(2000);
            return GetNewAccountNumber();
        }

        // ── Assertions / Getters ─────────────────────────────────────────────

        public bool IsOpenSuccessful()
        {
            // Primary check: success heading
            if (IsDisplayed(_successHeading) &&
                GetText(_successHeading).Contains("Account Opened"))
                return true;
            // Fallback: check if new account number is displayed (proves account was created)
            if (IsDisplayed(_newAccountNumber))
                return true;
            // Fallback: check page source
            return Driver.PageSource.Contains("Account Opened");
        }

        public string GetNewAccountNumber()
            => IsDisplayed(_newAccountNumber) ? GetText(_newAccountNumber) : string.Empty;

        public IList<IWebElement> GetAccountTypeOptions()
            => GetOptions(_accountTypeSelect);

        public bool IsCheckingOptionAvailable()
            => GetAccountTypeOptions().Any(o => o.Text.Contains("CHECKING"));

        public bool IsSavingsOptionAvailable()
            => GetAccountTypeOptions().Any(o => o.Text.Contains("SAVINGS"));
    }
}
