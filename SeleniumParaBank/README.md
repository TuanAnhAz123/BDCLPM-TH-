# SeleniumParaBank – Automation Testing Project

Selenium WebDriver + NUnit + C# với Page Object Model (POM)  
Website: https://parabank.parasoft.com/parabank/index.htm

---

## Cấu trúc Project

```
SeleniumParaBank/
├── Pages/                      ← Page Object Model classes
│   ├── BasePage.cs             ← Base class (shared helpers)
│   ├── LoginPage.cs
│   ├── RegisterPage.cs
│   ├── AccountOverviewPage.cs
│   ├── OpenAccountPage.cs
│   ├── TransferFundsPage.cs
│   ├── BillPayPage.cs
│   ├── FindTransactionsPage.cs
│   └── UpdateProfilePage.cs    ← chứa cả RequestLoanPage
│
├── Tests/                      ← Test classes (NUnit)
│   ├── BaseTest.cs             ← Setup / TearDown / Screenshot on fail
│   ├── LoginTests.cs           ← GUI + Functional: Login, Logout
│   ├── RegisterTests.cs        ← GUI + Functional: Registration
│   ├── AccountTests.cs         ← Account Overview + Open New Account
│   ├── TransferFundsTests.cs   ← GUI + Functional: Transfer
│   ├── BillPayTests.cs         ← GUI + Functional: Bill Pay
│   └── UpdateProfileLoanSmokeTests.cs ← UpdateProfile + RequestLoan + Smoke
│
├── Utilities/
│   ├── DriverFactory.cs        ← ChromeDriver singleton
│   ├── WaitHelper.cs           ← Explicit waits
│   ├── ScreenshotHelper.cs     ← Screenshot on failure
│   └── TestDataHelper.cs       ← Load data from users.json
│
├── TestData/
│   └── users.json              ← Tất cả test data
│
├── Reports/
│   └── Screenshots/            ← Screenshot tự động khi FAIL
│
└── SeleniumParaBank.csproj
```

---

## Yêu cầu cài đặt

1. **.NET 8 SDK** – https://dotnet.microsoft.com/download
2. **Google Chrome** (phiên bản mới nhất)
3. **Visual Studio 2022** hoặc **VS Code** + C# extension

---

## Cách chạy

### 1. Restore packages
```bash
dotnet restore
```

### 2. Build project
```bash
dotnet build
```

### 3. Chạy tất cả test
```bash
dotnet test
```

### 4. Chạy theo category
```bash
# Chỉ chạy Smoke tests
dotnet test --filter "Category=Smoke"

# Chỉ chạy GUI tests
dotnet test --filter "Category=GUI"

# Chỉ chạy Functional tests
dotnet test --filter "Category=Functional"

# Chạy một test class cụ thể
dotnet test --filter "FullyQualifiedName~LoginTests"
```

### 5. Chạy Headless (không mở Chrome)
Mở `Utilities/DriverFactory.cs`, bỏ comment dòng:
```csharp
// options.AddArgument("--headless=new");
```

---

## Test Data

Chỉnh sửa trong `TestData/users.json`:
- `existingUser` – tài khoản sẵn có trên ParaBank (mặc định: john/demo)
- `users[0]` – dữ liệu dùng cho test đăng ký
- `transfer`, `billPay`, `loan` – dữ liệu cho từng chức năng

---

## Screenshot khi FAIL

Tự động lưu vào: `Reports/Screenshots/<TestName>_<timestamp>.png`

---

## Phân công (gợi ý)

| Thành viên | Test Class                    | Scripts |
|------------|-------------------------------|---------|
| SV 1       | LoginTests, RegisterTests     | 10 TCs  |
| SV 2       | TransferFundsTests, BillPayTests | 10 TCs |
| SV 3       | AccountTests, FindTransactions | 10 TCs |
| SV 4       | UpdateProfile, RequestLoan, Smoke | 10 TCs |
