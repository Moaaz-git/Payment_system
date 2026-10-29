using System;
using System.Collections.Generic;


namespace Payment_system
{
    public class BankAccount
    {
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public string OwnerEmail { get; set; }

        //this function is used to get the current balance of the account, it is read-only and cannot be set from outside the class

 
        public BankAccount() {  }
        public BankAccount(string ownerEmail, decimal initialBalance)
        {
            OwnerEmail = ownerEmail;
            Balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive.");
            }
            Balance += amount;
            Transactions.Add(new Transaction(TransactionType.Deposit, amount, Balance));
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException(" amount must be positive.");
            }
            if(amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds for withdrawal.");
            }
            Balance -= amount;
            Transactions.Add(new Transaction(TransactionType.Withdrawal, amount, Balance));
        }

        //this function is used to get the list of transactions in the account,
        //it returns a list of strings that describe each transaction
        public List<string> GetStatementLines()
        {
            List<string> transactionStatements = new List<string>();
            foreach (var t in Transactions)
            {
                transactionStatements.Add(t.Describe());
            }

            return transactionStatements;
        }

        public enum TransactionType
        {
            Deposit,
            Withdrawal
        }

        //this class is used to represent a transaction in the account, it has properties for the type of transaction,
        //the amount, the balance after the transaction, and the date of the transaction
        public class Transaction
        {
            public TransactionType Type { get; set; }
            public decimal Amount { get; set; }
            public decimal BalanceAfter { get; set; }

            public DateTime Date { get; set; } 

            public Transaction() { }
            public Transaction(TransactionType type, decimal amount, decimal balanceAfter)
            {
                Type = type;
                Amount = amount;
                BalanceAfter = balanceAfter;
                Date = DateTime.Now; // Set the date to the current date and time when the transaction is created
            }

            public string Describe() =>
                $"{Date} | {Type,-9} | {Amount,10:C} | Balance: {BalanceAfter:C}";
        }
    }
}
