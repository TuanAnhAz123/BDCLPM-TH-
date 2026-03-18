using OpenQA.Selenium;

namespace SeleniumParaBank.Pages
{
    /// <summary>
    /// Page Object for the Login page.
    /// URL: /parabank/index.htm
    /// </summary>
    public class LoginPage : BasePage
    {
        // ── Locators ──────────────────────────────────────────────────────────
        private readonly By _usernameInput  = By.Name("username");
        private readonly By _passwordInput  = By.Name("password");
        private readonly By _loginButton    = By.CssSelector("input[value='Log In']");
        private readonly By _errorMessage   = By.CssSelector(".error");
        private readonly By _registerLink   = By.LinkText("Register");
        private readonly By _logoutLink     = By.LinkText("Log Out");

        // ── Constructor ───────────────────────────────────────────────────────
        public LoginPage(IWebDriver driver) : base(driver) { }

        // ── Actions ───────────────────────────────────────────────────────────

        public void NavigateTo(string baseUrl)
            => Driver.Navigate().GoToUrl(baseUrl);

        public void EnterUsername(string username)
            => Type(_usernameInput, username);

        public void EnterPassword(string password)
            => Type(_passwordInput, password);

        public void ClickLoginButton()
            => Click(_loginButton);

        /// <summary>Performs a full login and waits for the page to transition.</summary>
        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLoginButton();
            // Wait for page transition after login
            WaitForAjax(2000);
        }

        public void ClickRegisterLink()
            => Click(_registerLink);

        public void ClickLogoutLink()
            => Click(_logoutLink);

        // ── Assertions / Getters ─────────────────────────────────────────────

        public bool IsLoginButtonDisplayed()
            => IsDisplayed(_loginButton);

        public bool IsUsernameInputDisplayed()
            => IsDisplayed(_usernameInput);

        public bool IsPasswordInputDisplayed()
            => IsDisplayed(_passwordInput);

        public bool IsErrorMessageDisplayed()
            => IsDisplayed(_errorMessage);

        public string GetErrorMessage()
            => IsDisplayed(_errorMessage) ? GetText(_errorMessage) : string.Empty;

        public bool IsLogoutLinkDisplayed()
            => IsDisplayed(_logoutLink);

        public string GetUsernameInputType()
            => GetAttribute(_usernameInput, "type");

        public string GetPasswordInputType()
            => GetAttribute(_passwordInput, "type");

        public string GetUsernameInputPlaceholder()
            => GetAttribute(_usernameInput, "placeholder");

        public string GetPasswordInputPlaceholder()
            => GetAttribute(_passwordInput, "placeholder");
    }
}
