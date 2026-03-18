using NUnit.Framework;
using SeleniumParaBank.Pages;
using SeleniumParaBank.Utilities;

namespace SeleniumParaBank.Tests
{
    /// <summary>
    /// Test class for Login and Logout functionality.
    /// Covers TC-GUI-001 to TC-GUI-003, TC-GUI-007, TC-GUI-018,
    ///         TC-FN-003 to TC-FN-006, TC-FN-026.
    /// </summary>
    [TestFixture]
    [Category("Login")]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage = null!;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _loginPage = new LoginPage(Driver);
        }

        // ── GUI Tests ─────────────────────────────────────────────────────────

        [Test, Category("GUI")]
        [Description("TC-GUI-001: Username textbox phải hiển thị và nhập được")]
        public void GUI_UsernameTextbox_ShouldBeDisplayedAndEditable()
        {
            Assert.That(_loginPage.IsUsernameInputDisplayed(), Is.True,
                "Username input is not displayed");
        }

        [Test, Category("GUI")]
        [Description("TC-GUI-002: Password field phải hiển thị dạng password (ẩn ký tự)")]
        public void GUI_PasswordField_ShouldBePasswordType()
        {
            Assert.That(_loginPage.GetPasswordInputType(), Is.EqualTo("password"),
                "Password input is not of type 'password'");
        }

        [Test, Category("GUI")]
        [Description("TC-GUI-003: Button Log In phải hiển thị và click được")]
        public void GUI_LoginButton_ShouldBeDisplayed()
        {
            Assert.That(_loginPage.IsLoginButtonDisplayed(), Is.True,
                "Login button is not displayed");
        }

        [Test, Category("GUI")]
        [Description("TC-GUI-007: Thông báo lỗi hiển thị khi đăng nhập sai password")]
        public void GUI_ErrorMessage_ShouldDisplayOnWrongPassword()
        {
            _loginPage.Login("john", "wrongpassword");

            Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                "Error message should be displayed for wrong password");

            var msg = _loginPage.GetErrorMessage();
            Assert.That(msg, Is.Not.Empty, "Error message text should not be empty");
        }

        [Test, Category("GUI")]
        [Description("TC-GUI-015: Log Out link chỉ xuất hiện sau khi đăng nhập")]
        public void GUI_LogoutLink_ShouldOnlyAppearAfterLogin()
        {
            Assert.That(_loginPage.IsLogoutLinkDisplayed(), Is.False,
                "Logout link should NOT be visible before login");

            _loginPage.Login(TestDataHelper.ExistingUser, TestDataHelper.ExistingPass);

            Assert.That(_loginPage.IsLogoutLinkDisplayed(), Is.True,
                "Logout link SHOULD be visible after login");
        }

        // ── Functional Tests ──────────────────────────────────────────────────

        [Test, Category("Functional"), Category("Smoke")]
        [Description("TC-FN-003: Đăng nhập thành công với thông tin hợp lệ")]
        public void Login_WithValidCredentials_ShouldRedirectToAccountOverview()
        {
            _loginPage.Login(TestDataHelper.ExistingUser, TestDataHelper.ExistingPass);

            // Verify login succeeded by checking logout link is visible
            Assert.That(_loginPage.IsLogoutLinkDisplayed(), Is.True,
                "Login should succeed — Logout link should be visible");
            Assert.That(Driver.Url, Does.Contain("overview").Or.Contain("parabank"),
                "Should redirect to Account Overview after successful login");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-004: Đăng nhập sai password phải hiển thị lỗi")]
        public void Login_WithWrongPassword_ShouldShowError()
        {
            _loginPage.Login(TestDataHelper.ExistingUser, "WrongPass999!");

            Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                "Error message should be displayed");
            Assert.That(Driver.Url, Does.Not.Contain("overview"),
                "Should NOT redirect to overview on failed login");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-005: Đăng nhập với username không tồn tại")]
        public void Login_WithNonExistentUsername_ShouldShowError()
        {
            _loginPage.Login("user_does_not_exist_xyz", "Test@1234");

            Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                "Error message should display for non-existent user");
        }

        [Test, Category("Functional"), Category("Smoke")]
        [Description("TC-FN-006: Logout thành công, redirect về trang Home")]
        public void Logout_AfterLogin_ShouldRedirectToHomePage()
        {
            _loginPage.Login(TestDataHelper.ExistingUser, TestDataHelper.ExistingPass);
            _loginPage.ClickLogoutLink();

            Assert.That(_loginPage.IsLoginButtonDisplayed(), Is.True,
                "Login button should be visible after logout");
            Assert.That(_loginPage.IsLogoutLinkDisplayed(), Is.False,
                "Logout link should be hidden after logout");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-026: Đăng nhập bỏ trống cả hai field")]
        public void Login_WithEmptyFields_ShouldShowValidationError()
        {
            _loginPage.ClickLoginButton();

            Assert.That(_loginPage.IsErrorMessageDisplayed(), Is.True,
                "Validation error should appear when both fields are empty");
        }
    }
}
