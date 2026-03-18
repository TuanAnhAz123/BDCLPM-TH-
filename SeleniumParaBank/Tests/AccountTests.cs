using NUnit.Framework;
using SeleniumParaBank.Pages;
using SeleniumParaBank.Utilities;

namespace SeleniumParaBank.Tests
{
    /// <summary>
    /// Test class for Account Overview and Open New Account.
    /// Covers TC-GUI-011, TC-FN-007, TC-FN-008, TC-FN-009, TC-FN-010, TC-FN-029.
    /// </summary>
    [TestFixture]
    [Category("Account")]
    public class AccountTests : BaseTest
    {
        private LoginPage            _loginPage       = null!;
        private AccountOverviewPage  _overviewPage    = null!;
        private OpenAccountPage      _openAccountPage = null!;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _loginPage       = new LoginPage(Driver);
            _overviewPage    = new AccountOverviewPage(Driver);
            _openAccountPage = new OpenAccountPage(Driver);

            _loginPage.Login(TestDataHelper.ExistingUser, TestDataHelper.ExistingPass);
        }

        // ── Account Overview Tests ─────────────────────────────────────────────

        [Test, Category("Functional")]
        [Description("TC-FN-007: Accounts Overview phải hiển thị bảng danh sách tài khoản")]
        public void AccountOverview_ShouldDisplayAccountsTable()
        {
            _overviewPage.ClickOverviewLink();

            Assert.That(_overviewPage.IsAccountsTableDisplayed(), Is.True,
                "Accounts table should be displayed");
            Assert.That(_overviewPage.GetAccountRowCount(), Is.GreaterThan(0),
                "At least one account should be listed");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-008: Click vào tài khoản phải chuyển sang trang lịch sử giao dịch")]
        public void AccountOverview_ClickAccount_ShouldShowTransactionHistory()
        {
            _overviewPage.ClickOverviewLink();
            // Click the first account link found
            var firstLink = Driver.FindElements(
                OpenQA.Selenium.By.CssSelector("#accountTable tbody tr td a")).FirstOrDefault();

            if (firstLink == null)
                Assert.Inconclusive("No account links found to click");

            firstLink!.Click();

            Assert.That(Driver.Url, Does.Contain("activity"),
                "Should navigate to account activity page");
        }

        // ── Open New Account Tests ─────────────────────────────────────────────

        [Test, Category("GUI")]
        [Description("TC-GUI-011: Dropdown loại tài khoản phải có CHECKING và SAVINGS")]
        public void GUI_OpenAccount_TypeDropdown_ShouldHaveCheckingAndSavings()
        {
            _openAccountPage.ClickOpenAccountLink();

            Assert.Multiple(() =>
            {
                Assert.That(_openAccountPage.IsCheckingOptionAvailable(), Is.True,
                    "CHECKING option should be available");
                Assert.That(_openAccountPage.IsSavingsOptionAvailable(), Is.True,
                    "SAVINGS option should be available");
            });
        }

        [Test, Category("Functional")]
        [Description("TC-FN-009: Mở tài khoản Checking mới phải thành công")]
        public void OpenAccount_Checking_ShouldCreateNewAccount()
        {
            _openAccountPage.ClickOpenAccountLink();
            var newAccountNumber = _openAccountPage.OpenNewAccount("CHECKING");

            Assert.That(_openAccountPage.IsOpenSuccessful(), Is.True,
                "New CHECKING account should be created successfully");
            Assert.That(newAccountNumber, Is.Not.Empty,
                "New account number should be returned");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-010: Mở tài khoản Savings mới phải thành công")]
        public void OpenAccount_Savings_ShouldCreateNewAccount()
        {
            _openAccountPage.ClickOpenAccountLink();
            var newAccountNumber = _openAccountPage.OpenNewAccount("SAVINGS");

            Assert.That(_openAccountPage.IsOpenSuccessful(), Is.True,
                "New SAVINGS account should be created successfully");
            Assert.That(newAccountNumber, Is.Not.Empty,
                "New account number should be returned");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-029: Tài khoản mới phải xuất hiện trong Accounts Overview")]
        public void OpenAccount_NewAccount_ShouldAppearInOverview()
        {
            _openAccountPage.ClickOpenAccountLink();
            var newAccountNumber = _openAccountPage.OpenNewAccount("CHECKING");

            _overviewPage.ClickOverviewLink();

            Assert.That(_overviewPage.HasAccountInTable(newAccountNumber), Is.True,
                $"Newly created account {newAccountNumber} should appear in overview");
        }
    }
}
