using NUnit.Framework;
using SeleniumParaBank.Pages;
using SeleniumParaBank.Utilities;

namespace SeleniumParaBank.Tests
{
    /// <summary>
    /// Test class for User Registration functionality.
    /// Covers TC-GUI-008, TC-GUI-016, TC-FN-001, TC-FN-002, TC-FN-025.
    /// </summary>
    [TestFixture]
    [Category("Registration")]
    public class RegisterTests : BaseTest
    {
        private LoginPage    _loginPage    = null!;
        private RegisterPage _registerPage = null!;

        // Unique username per test run to avoid duplicates
        private static string UniqueUsername
            => $"auto_{DateTime.Now:yyyyMMddHHmmss}_{Random.Shared.Next(1000, 9999)}";

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _loginPage    = new LoginPage(Driver);
            _registerPage = new RegisterPage(Driver);
            _loginPage.ClickRegisterLink();
        }

        // ── GUI Tests ─────────────────────────────────────────────────────────

        [Test, Category("GUI")]
        [Description("TC-GUI-008: Lỗi 'First name is required' phải hiển thị khi thiếu First Name")]
        public void GUI_Register_MissingFirstName_ShouldShowError()
        {
            var user = TestDataHelper.GetUser("user1");

            _registerPage.FillLastName(user["lastName"]!.ToString());
            _registerPage.FillAddress(user["address"]!.ToString());
            _registerPage.FillCity(user["city"]!.ToString());
            _registerPage.FillState(user["state"]!.ToString());
            _registerPage.FillZipCode(user["zipCode"]!.ToString());
            _registerPage.FillUsername(UniqueUsername);
            _registerPage.FillPassword("Test@1234");
            _registerPage.FillConfirmPassword("Test@1234");
            _registerPage.ClickRegister();

            Assert.That(_registerPage.IsFirstNameErrorDisplayed(), Is.True,
                "First name error should display");
            Assert.That(_registerPage.GetFirstNameError(),
                Does.Contain("required").IgnoreCase);
        }

        [Test, Category("GUI")]
        [Description("TC-GUI-016: Lỗi password mismatch phải hiển thị rõ ràng")]
        public void GUI_Register_PasswordMismatch_ShouldShowError()
        {
            var user = TestDataHelper.GetUser("user1");

            // Fill ALL required fields to ensure only password mismatch error shows
            _registerPage.FillFirstName(user["firstName"]!.ToString());
            _registerPage.FillLastName(user["lastName"]!.ToString());
            _registerPage.FillAddress(user["address"]!.ToString());
            _registerPage.FillCity(user["city"]!.ToString());
            _registerPage.FillState(user["state"]!.ToString());
            _registerPage.FillZipCode(user["zipCode"]!.ToString());
            _registerPage.FillPhone(user["phone"]!.ToString());
            _registerPage.FillSsn(user["ssn"]!.ToString());
            _registerPage.FillUsername(UniqueUsername);
            _registerPage.FillPassword("Test@1234");
            _registerPage.FillConfirmPassword("DifferentPass@999");
            _registerPage.ClickRegister();

            Assert.That(_registerPage.IsConfirmPassErrorDisplayed(), Is.True,
                "Confirm password error should display");
            Assert.That(_registerPage.GetConfirmPassError(),
                Does.Contain("match").IgnoreCase);
        }

        // ── Functional Tests ──────────────────────────────────────────────────

        [Test, Category("Functional")]
        [Description("TC-FN-001: Đăng ký thành công với đầy đủ thông tin hợp lệ")]
        public void Register_WithValidData_ShouldCreateAccount()
        {
            var user     = TestDataHelper.GetUser("user1");
            var username = UniqueUsername;

            _registerPage.RegisterUser(
                user["firstName"]!.ToString(),
                user["lastName"]!.ToString(),
                user["address"]!.ToString(),
                user["city"]!.ToString(),
                user["state"]!.ToString(),
                user["zipCode"]!.ToString(),
                user["phone"]!.ToString(),
                user["ssn"]!.ToString(),
                username,
                "Test@1234"
            );

            Assert.That(_registerPage.IsRegistrationSuccessful(), Is.True,
                "Registration should succeed with valid data");
            Assert.That(_registerPage.GetSuccessMessage(),
                Does.Contain("created successfully").IgnoreCase);
        }

        [Test, Category("Functional")]
        [Description("TC-FN-002: Đăng ký với username đã tồn tại phải hiển thị lỗi")]
        public void Register_WithExistingUsername_ShouldShowError()
        {
            var user = TestDataHelper.GetUser("user1");

            _registerPage.RegisterUser(
                user["firstName"]!.ToString(),
                user["lastName"]!.ToString(),
                user["address"]!.ToString(),
                user["city"]!.ToString(),
                user["state"]!.ToString(),
                user["zipCode"]!.ToString(),
                user["phone"]!.ToString(),
                user["ssn"]!.ToString(),
                TestDataHelper.ExistingUser,  // username that already exists
                "Test@1234"
            );

            Assert.That(_registerPage.IsUsernameErrorDisplayed(), Is.True,
                "Error should display for duplicate username");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-025: Đăng ký với password không khớp")]
        public void Register_WithPasswordMismatch_ShouldFail()
        {
            var user = TestDataHelper.GetUser("user1");

            _registerPage.FillFirstName(user["firstName"]!.ToString());
            _registerPage.FillLastName(user["lastName"]!.ToString());
            _registerPage.FillAddress(user["address"]!.ToString());
            _registerPage.FillUsername(UniqueUsername);
            _registerPage.FillPassword("Test@1234");
            _registerPage.FillConfirmPassword("WrongConfirm@99");
            _registerPage.ClickRegister();

            Assert.That(_registerPage.IsConfirmPassErrorDisplayed(), Is.True);
        }

        [Test, Category("Functional")]
        [Description("TC-FN: Đăng ký thiếu Last Name phải hiển thị lỗi")]
        public void Register_MissingLastName_ShouldShowError()
        {
            _registerPage.FillFirstName("John");
            _registerPage.FillUsername(UniqueUsername);
            _registerPage.FillPassword("Test@1234");
            _registerPage.FillConfirmPassword("Test@1234");
            _registerPage.ClickRegister();

            Assert.That(_registerPage.IsLastNameErrorDisplayed(), Is.True,
                "Last name error should display");
        }
    }
}
