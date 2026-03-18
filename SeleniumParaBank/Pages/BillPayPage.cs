using OpenQA.Selenium;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Page Object for the Bill Pay page.
    /// URL: /parabank/billpay.htm
    /// </summary>
    public class BillPayPage : BasePage
    {
        // ── Locators ──────────────────────────────────────────────────────────
        private readonly By _payeeNameInput    = By.Name("payee.name");
        private readonly By _addressInput      = By.Name("payee.address.street");
        private readonly By _cityInput         = By.Name("payee.address.city");
        private readonly By _stateInput        = By.Name("payee.address.state");
        private readonly By _zipCodeInput      = By.Name("payee.address.zipCode");
        private readonly By _phoneInput        = By.Name("payee.phoneNumber");
        private readonly By _accountInput      = By.Name("payee.accountNumber");
        private readonly By _verifyAccountInput= By.Name("verifyAccount");
        private readonly By _amountInput       = By.Name("amount");
        private readonly By _fromAccountSelect = By.Name("fromAccountId");
        private readonly By _sendPaymentButton = By.CssSelector("input[value='Send Payment']");
        private readonly By _successTitle      = By.CssSelector("#billpayResult h1, #rightPanel h1.title, #rightPanel .title");
        private readonly By _payeeNameError    = By.Id("validationModel-name");
        private readonly By _amountError       = By.Id("validationModel-amount");
        private readonly By _billPayLink       = By.LinkText("Bill Pay");

        // ── Constructor ───────────────────────────────────────────────────────
        public BillPayPage(IWebDriver driver) : base(driver) { }

        // ── Actions ───────────────────────────────────────────────────────────

        public void ClickBillPayLink()
        {
            Click(_billPayLink);
            WaitForAjax(1500);
        }

        public void FillPayeeName(string v)       => Type(_payeeNameInput, v);
        public void FillAddress(string v)         => Type(_addressInput, v);
        public void FillCity(string v)            => Type(_cityInput, v);
        public void FillState(string v)           => Type(_stateInput, v);
        public void FillZipCode(string v)         => Type(_zipCodeInput, v);
        public void FillPhone(string v)           => Type(_phoneInput, v);
        public void FillAccount(string v)         => Type(_accountInput, v);
        public void FillVerifyAccount(string v)   => Type(_verifyAccountInput, v);
        public void FillAmount(string v)          => Type(_amountInput, v);
        public void ClickSendPayment()            => Click(_sendPaymentButton);

        public void PayBill(string name, string address, string city, string state,
            string zip, string phone, string account, string verifyAccount, string amount)
        {
            FillPayeeName(name);
            FillAddress(address);
            FillCity(city);
            FillState(state);
            FillZipCode(zip);
            FillPhone(phone);
            FillAccount(account);
            FillVerifyAccount(verifyAccount);
            FillAmount(amount);
            ClickSendPayment();
            // Wait for AJAX result
            WaitForAjax(2000);
        }

        // ── Assertions / Getters ─────────────────────────────────────────────

        public bool IsBillPaySuccessful()
        {
            if (IsDisplayed(_successTitle) &&
                GetText(_successTitle).Contains("Bill Payment Complete"))
                return true;
            return Driver.PageSource.Contains("Bill Payment Complete");
        }

        public string GetSuccessTitle()
            => IsDisplayed(_successTitle) ? GetText(_successTitle) : string.Empty;

        public bool IsPayeeNameErrorDisplayed()  => IsDisplayed(_payeeNameError);
        public bool IsAmountErrorDisplayed()     => IsDisplayed(_amountError);
        public string GetPayeeNameError()        => GetText(_payeeNameError);

        public bool IsSendPaymentButtonDisplayed() => IsDisplayed(_sendPaymentButton);
        public bool IsAllFieldsDisplayed()
            => IsDisplayed(_payeeNameInput) && IsDisplayed(_addressInput)
            && IsDisplayed(_cityInput)      && IsDisplayed(_stateInput)
            && IsDisplayed(_zipCodeInput)   && IsDisplayed(_phoneInput)
            && IsDisplayed(_accountInput)   && IsDisplayed(_amountInput);
    }
}
