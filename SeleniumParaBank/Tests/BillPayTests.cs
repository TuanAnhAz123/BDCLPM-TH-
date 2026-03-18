using NUnit.Framework;
using SeleniumParaBank.Pages;
using SeleniumParaBank.Utilities;

namespace SeleniumParaBank.Tests
{
    /// <summary>
    /// Test class for Bill Pay functionality.
    /// Covers TC-GUI-012, TC-GUI-019, TC-FN-014, TC-FN-015, TC-FN-016.
    /// </summary>
    [TestFixture]
    [Category("BillPay")]
    public class BillPayTests : BaseTest
    {
        private LoginPage   _loginPage   = null!;
        private BillPayPage _billPayPage = null!;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            _loginPage   = new LoginPage(Driver);
            _billPayPage = new BillPayPage(Driver);

            _loginPage.Login(TestDataHelper.ExistingUser, TestDataHelper.ExistingPass);
            _billPayPage.ClickBillPayLink();
        }

        // ── GUI Tests ─────────────────────────────────────────────────────────

        [Test, Category("GUI")]
        [Description("TC-GUI-012: Form Bill Pay phải hiển thị đủ tất cả các field")]
        public void GUI_BillPayForm_AllFieldsShouldBeDisplayed()
        {
            Assert.That(_billPayPage.IsAllFieldsDisplayed(), Is.True,
                "All Bill Pay fields should be visible");
        }

        [Test, Category("GUI")]
        [Description("TC-GUI-019: Button 'Send Payment' phải hiển thị và click được")]
        public void GUI_SendPaymentButton_ShouldBeDisplayed()
        {
            Assert.That(_billPayPage.IsSendPaymentButtonDisplayed(), Is.True,
                "Send Payment button should be displayed");
        }

        // ── Functional Tests ──────────────────────────────────────────────────

        [Test, Category("Functional")]
        [Description("TC-FN-014: Bill Pay thành công với đầy đủ thông tin hợp lệ")]
        public void BillPay_WithValidData_ShouldShowSuccessMessage()
        {
            _billPayPage.PayBill(
                TestDataHelper.Get("billPay.payeeName"),
                TestDataHelper.Get("billPay.address"),
                TestDataHelper.Get("billPay.city"),
                TestDataHelper.Get("billPay.state"),
                TestDataHelper.Get("billPay.zipCode"),
                TestDataHelper.Get("billPay.phone"),
                TestDataHelper.Get("billPay.account"),
                TestDataHelper.Get("billPay.verifyAccount"),
                TestDataHelper.Get("billPay.amount")
            );

            Assert.That(_billPayPage.IsBillPaySuccessful(), Is.True,
                "Bill payment should complete successfully");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-015: Thiếu Payee Name phải hiển thị lỗi")]
        public void BillPay_MissingPayeeName_ShouldShowError()
        {
            // Leave Payee Name empty, fill rest
            _billPayPage.FillAddress(TestDataHelper.Get("billPay.address"));
            _billPayPage.FillCity(TestDataHelper.Get("billPay.city"));
            _billPayPage.FillState(TestDataHelper.Get("billPay.state"));
            _billPayPage.FillZipCode(TestDataHelper.Get("billPay.zipCode"));
            _billPayPage.FillPhone(TestDataHelper.Get("billPay.phone"));
            _billPayPage.FillAccount(TestDataHelper.Get("billPay.account"));
            _billPayPage.FillVerifyAccount(TestDataHelper.Get("billPay.verifyAccount"));
            _billPayPage.FillAmount(TestDataHelper.Get("billPay.amount"));
            _billPayPage.ClickSendPayment();

            Assert.That(_billPayPage.IsPayeeNameErrorDisplayed(), Is.True,
                "Payee name error should display");
        }

        [Test, Category("Functional")]
        [Description("TC-FN-016: Account Number không hợp lệ phải hiển thị lỗi")]
        public void BillPay_WithInvalidAccountNumber_ShouldShowError()
        {
            _billPayPage.FillPayeeName("Test Payee");
            _billPayPage.FillAddress("1 Test St");
            _billPayPage.FillCity("TestCity");
            _billPayPage.FillState("CA");
            _billPayPage.FillZipCode("12345");
            _billPayPage.FillPhone("555-0000");
            _billPayPage.FillAccount("INVALID_ACC");
            _billPayPage.FillVerifyAccount("DIFFERENT");
            _billPayPage.FillAmount("10");
            _billPayPage.ClickSendPayment();

            // ParaBank may show validation error for mismatched accounts
            // or may reject the payment with an error response
            Assert.That(_billPayPage.IsBillPaySuccessful(), Is.False,
                "Bill payment should fail with invalid account number");
        }
    }
}
