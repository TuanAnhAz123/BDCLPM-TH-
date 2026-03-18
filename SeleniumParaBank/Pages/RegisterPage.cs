using OpenQA.Selenium;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Page Object for the Registration page.
    /// URL: /parabank/register.htm
    /// </summary>
    public class RegisterPage : BasePage
    {
        // ── Locators ──────────────────────────────────────────────────────────
        private readonly By _firstNameInput      = By.Id("customer.firstName");
        private readonly By _lastNameInput       = By.Id("customer.lastName");
        private readonly By _addressInput        = By.Id("customer.address.street");
        private readonly By _cityInput           = By.Id("customer.address.city");
        private readonly By _stateInput          = By.Id("customer.address.state");
        private readonly By _zipCodeInput        = By.Id("customer.address.zipCode");
        private readonly By _phoneInput          = By.Id("customer.phoneNumber");
        private readonly By _ssnInput            = By.Id("customer.ssn");
        private readonly By _usernameInput       = By.Id("customer.username");
        private readonly By _passwordInput       = By.Id("customer.password");
        private readonly By _confirmPassInput    = By.Id("repeatedPassword");
        private readonly By _registerButton      = By.CssSelector("input[value='Register']");
        private readonly By _firstNameError      = By.Id("customer.firstName.errors");
        private readonly By _lastNameError       = By.Id("customer.lastName.errors");
        private readonly By _addressError        = By.Id("customer.address.street.errors");
        private readonly By _usernameError       = By.Id("customer.username.errors");
        private readonly By _passwordError       = By.Id("customer.password.errors");
        private readonly By _confirmPassError    = By.Id("repeatedPassword.errors");
        private readonly By _successMessage      = By.CssSelector("#rightPanel p");
        private readonly By _pageHeading         = By.CssSelector("#rightPanel h1");

        // ── Constructor ───────────────────────────────────────────────────────
        public RegisterPage(IWebDriver driver) : base(driver) { }

        // ── Actions ───────────────────────────────────────────────────────────

        public void FillFirstName(string v)    => Type(_firstNameInput, v);
        public void FillLastName(string v)     => Type(_lastNameInput, v);
        public void FillAddress(string v)      => Type(_addressInput, v);
        public void FillCity(string v)         => Type(_cityInput, v);
        public void FillState(string v)        => Type(_stateInput, v);
        public void FillZipCode(string v)      => Type(_zipCodeInput, v);
        public void FillPhone(string v)        => Type(_phoneInput, v);
        public void FillSsn(string v)          => Type(_ssnInput, v);
        public void FillUsername(string v)     => Type(_usernameInput, v);
        public void FillPassword(string v)     => Type(_passwordInput, v);
        public void FillConfirmPassword(string v) => Type(_confirmPassInput, v);

        public void ClickRegister()
        {
            Click(_registerButton);
            WaitForAjax(1500);
        }

        /// <summary>Fills all registration fields and submits.</summary>
        public void RegisterUser(string firstName, string lastName, string address,
            string city, string state, string zip, string phone, string ssn,
            string username, string password)
        {
            FillFirstName(firstName);
            FillLastName(lastName);
            FillAddress(address);
            FillCity(city);
            FillState(state);
            FillZipCode(zip);
            FillPhone(phone);
            FillSsn(ssn);
            FillUsername(username);
            FillPassword(password);
            FillConfirmPassword(password);
            ClickRegister();
        }

        // ── Assertions / Getters ─────────────────────────────────────────────

        public bool IsRegistrationSuccessful()
        {
            if (IsDisplayed(_successMessage) &&
                GetText(_successMessage).Contains("created successfully"))
                return true;
            // Fallback: check page heading or page source
            if (IsDisplayed(_pageHeading) &&
                GetText(_pageHeading).Contains("Welcome"))
                return true;
            return Driver.PageSource.Contains("created successfully");
        }

        public string GetSuccessMessage()
            => IsDisplayed(_successMessage) ? GetText(_successMessage) : string.Empty;

        public bool IsFirstNameErrorDisplayed()  => IsDisplayed(_firstNameError);
        public bool IsLastNameErrorDisplayed()   => IsDisplayed(_lastNameError);
        public bool IsAddressErrorDisplayed()    => IsDisplayed(_addressError);
        public bool IsUsernameErrorDisplayed()   => IsDisplayed(_usernameError);
        public bool IsPasswordErrorDisplayed()   => IsDisplayed(_passwordError);
        public bool IsConfirmPassErrorDisplayed()=> IsDisplayed(_confirmPassError);

        public string GetFirstNameError()   => GetText(_firstNameError);
        public string GetLastNameError()    => GetText(_lastNameError);
        public string GetUsernameError()    => GetText(_usernameError);
        public string GetConfirmPassError() => GetText(_confirmPassError);
    }
}
