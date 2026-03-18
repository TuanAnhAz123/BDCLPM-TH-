using OpenQA.Selenium;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Page Object for Update Contact Info page.
    /// URL: /parabank/updateprofile.htm
    /// </summary>
    public class UpdateProfilePage : BasePage
    {
        private readonly By _firstNameInput  = By.Id("customer.firstName");
        private readonly By _lastNameInput   = By.Id("customer.lastName");
        private readonly By _addressInput    = By.Id("customer.address.street");
        private readonly By _cityInput       = By.Id("customer.address.city");
        private readonly By _stateInput      = By.Id("customer.address.state");
        private readonly By _zipInput        = By.Id("customer.address.zipCode");
        private readonly By _phoneInput      = By.Id("customer.phoneNumber");
        private readonly By _updateButton    = By.CssSelector("input[value='Update Profile']");
        private readonly By _successMessage  = By.CssSelector("#updateProfileResult h1, #rightPanel h1.title, #rightPanel .title");
        private readonly By _firstNameError  = By.Id("customer.firstName.errors");
        private readonly By _updateLink      = By.LinkText("Update Contact Info");

        public UpdateProfilePage(IWebDriver driver) : base(driver) { }

        public void ClickUpdateLink()
        {
            Click(_updateLink);
            // Wait for form to load and pre-fill via AJAX
            WaitForAjax(2000);
        }

        public void ClearFirstName()
        {
            var el = FindElement(_firstNameInput);
            el.Clear();
        }

        public void FillFirstName(string v) => Type(_firstNameInput, v);
        public void FillAddress(string v)   => Type(_addressInput, v);

        public void ClickUpdateButton()
        {
            Click(_updateButton);
            WaitForAjax(2000);
        }

        public bool IsUpdateSuccessful()
        {
            if (IsDisplayed(_successMessage) &&
                GetText(_successMessage).Contains("Profile Updated"))
                return true;
            return Driver.PageSource.Contains("Profile Updated");
        }

        public bool IsFirstNameErrorDisplayed() => IsDisplayed(_firstNameError);
        public string GetFirstNameError()       => GetText(_firstNameError);

        public bool IsFormPreFilled()
        {
            try
            {
                var val = FindElement(_firstNameInput).GetAttribute("value");
                return !string.IsNullOrEmpty(val);
            }
            catch { return false; }
        }
    }

    /// <summary>
    /// Page Object for Request Loan page.
    /// URL: /parabank/requestloan.htm
    /// </summary>
    public class RequestLoanPage : BasePage
    {
        private readonly By _loanAmountInput  = By.Id("amount");
        private readonly By _downPaymentInput = By.Id("downPayment");
        private readonly By _fromAccountSelect= By.Id("fromAccountId");
        private readonly By _applyButton      = By.CssSelector("input[value='Apply Now']");
        private readonly By _resultContainer  = By.Id("loanStatus");
        private readonly By _approvalText     = By.CssSelector("#loanStatus h1, #loanStatus .title, #loanRequestApproved h1, #loanRequestDenied h1");
        private readonly By _loanAmountError  = By.Id("amount.errors");
        private readonly By _downPaymentError = By.Id("downPayment.errors");
        private readonly By _loanLink         = By.LinkText("Request Loan");

        public RequestLoanPage(IWebDriver driver) : base(driver) { }

        public void ClickLoanLink()
        {
            Click(_loanLink);
            WaitForAjax(1500);
        }

        public void EnterLoanAmount(string v)    => Type(_loanAmountInput, v);
        public void EnterDownPayment(string v)   => Type(_downPaymentInput, v);

        public void SelectFromAccount(int idx = 0)
        {
            var opts = GetOptions(_fromAccountSelect);
            if (opts.Count > idx) opts[idx].Click();
        }

        public void ClickApplyButton()
        {
            Click(_applyButton);
            WaitForAjax(3000);
        }

        public void ApplyLoan(string amount, string downPayment)
        {
            EnterLoanAmount(amount);
            EnterDownPayment(downPayment);
            SelectFromAccount();
            ClickApplyButton();
        }

        public bool IsLoanApproved()
        {
            try
            {
                if (IsDisplayed(_approvalText))
                {
                    var text = GetText(_approvalText);
                    if (text.Contains("Approved") || text.Contains("Congratulations"))
                        return true;
                }
                // Fallback: check page source
                var source = Driver.PageSource;
                return source.Contains("Approved") && !source.Contains("Denied");
            }
            catch { return false; }
        }

        public bool IsLoanDenied()
        {
            try
            {
                if (IsDisplayed(_approvalText))
                {
                    var text = GetText(_approvalText);
                    if (text.Contains("Denied"))
                        return true;
                }
                return Driver.PageSource.Contains("Denied");
            }
            catch { return false; }
        }

        public bool IsDownPaymentErrorDisplayed() => IsDisplayed(_downPaymentError);
        public bool IsLoanAmountErrorDisplayed()   => IsDisplayed(_loanAmountError);

        public bool IsLoanAmountInputDisplayed()     => IsDisplayed(_loanAmountInput);
        public bool IsDownPaymentInputDisplayed()    => IsDisplayed(_downPaymentInput);
        public bool IsFromAccountDropdownDisplayed() => IsDisplayed(_fromAccountSelect);
    }
}
