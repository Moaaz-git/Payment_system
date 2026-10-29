# 💳 Payment System

A desktop banking and payment management application built with C# and Windows Forms, designed to simulate a simple financial system with user authentication, balance tracking, and transaction history.

This project demonstrates core concepts such as:
- User login and account validation
- Bank account management
- Deposit and withdrawal operations
- Transaction history and statement generation
- Persisting data in JSON files
- A polished modern WinForms UI

## ✨ Features

- Secure sign-in form with validation
- Dashboard for signed-in users
- Deposit money into an account
- Withdraw money with balance validation
- Track every transaction with timestamps and amounts
- View account statement in a generated transaction file
- Save account data locally in `accounts.json`
- Save user data locally in `users.json`
- Modern glassmorphism-inspired UI design

## 🏗️ Project Structure

```text
Payment_system/
├── AccountStore.cs          # Handles reading/writing bank account data
├── BankAccount.cs           # Core banking logic and transaction model
├── DashboardForm.cs         # Main dashboard UI for account operations
├── DashboardForm.Designer.cs
├── Form1.cs                 # Login screen logic and validation
├── Form1.Designer.cs
├── LoginControls.cs         # Shared login UI controls
├── LoginTheme.cs            # Custom theme and visual styling
├── Program.cs               # Application entry point
├── UserStore.cs             # Loads and searches user records
├── UserِAccount.cs          # User account model
├── accounts.json            # Stored bank account information
├── users.json               # Stored user credentials and profile data
├── Transactions.txt         # Generated transaction statements
├── App.config
├── packages.config
├── Payment_system.csproj
├── Payment_system.sln
└── README.md
```

## 🧠 Core Logic

The system is built around two main data models:

- `UserAccount` – represents a user with email, password, and basic account information
- `BankAccount` – represents a bank account with owner email, balance, and transactions

The `BankAccount` class includes:
- `Deposit(decimal amount)`
- `Withdraw(decimal amount)`
- `GetStatementLines()`
- Transaction logging with date, amount, type, and resulting balance

The `AccountStore` and `UserStore` classes are responsible for reading and writing JSON data files to persist state between sessions.

## 🧪 How It Works

1. User opens the application.
2. The login form validates the email and password.
3. If authentication succeeds, a dashboard opens.
4. The user can deposit or withdraw funds.
5. Each transaction is stored in the account object.
6. Data is serialized to JSON and saved locally.
7. The user can print a statement, which writes the transaction history to `Transactions.txt`.

## 🚀 Getting Started

### Prerequisites

- Windows OS
- Visual Studio 2019 or later
- .NET Framework / C# WinForms support

### Run the Project

1. Open `Payment_system.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Press `F5` to run the application.

## 💾 Data Storage

The project uses JSON files to simulate persistent storage:

- `users.json` stores registered users
- `accounts.json` stores account data and transaction history
- `Transactions.txt` stores the printable account statement

## 🎨 UI Design

The application uses a custom modern interface with:
- rounded glass panels
- subtle gradients
- glowing visual background effects
- custom themed input controls
- dashboard-style layout

## 📌 Example Workflow

```text
Login
  -> Enter email and password
  -> Dashboard opens
  -> Deposit/Withdraw funds
  -> Transaction saved
  -> View or print statement
```

## ⚠️ Notes

This project is a practical training/demo application and is intended for educational and learning purposes. It focuses on demonstrating desktop app logic, file persistence, and UI styling rather than production-grade banking security.

## 🛠️ Technologies Used

- C#
- WinForms
- .NET Framework
- Newtonsoft.Json
- Local JSON file storage

## 📚 Future Improvements

- Add registration functionality
- Add stronger password hashing
- Add account transfer features
- Add admin dashboard
- Add database integration (SQL Server / SQLite)
- Improve validation and error handling

## Author

Moaaz Git

---

If you want, I can also create a more premium version of this README in a specific style, such as:
- Minimal & modern
- Arabic-only
- Developer portfolio style
- Professional GitHub project style
- Dark mode premium README
