using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Payment_system
{
    public class AccountStore
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounts.json");


        public List<BankAccount> LoadBankAccounts()
        {
            if (!File.Exists(_filePath))
            {
                // If the file doesn't exist, return an empty list
                return new List<BankAccount>();
            }
            
            string textContent = File.ReadAllText(_filePath);

            List<BankAccount> bankAccounts = JsonConvert.DeserializeObject<List<BankAccount>>(textContent);

            //we do this to ensure that the Transactions list is initialized for each account
            //, even if it was null in the JSON file
            if(bankAccounts != null)
            {
                foreach (var account in bankAccounts)
                {
                    if (account.Transactions == null)
                    {
                        account.Transactions = new List<BankAccount.Transaction>();
                    }
                }
            }
            return bankAccounts ?? new List<BankAccount>();

        }

        public BankAccount FindByOwnerEmail(string email)
        {
            foreach(var acc in LoadBankAccounts())
            {
                if (string.Equals(acc.OwnerEmail, email, StringComparison.OrdinalIgnoreCase))
                {
                    return acc;
                }
            }
            return null;
        }

        public void SaveAccount(BankAccount account)
        {
            if (account != null && account.Transactions != null)
            {
                List<BankAccount> accounts = LoadBankAccounts();
                bool accountExists = false;
                for (int i = 0; i < accounts.Count; i++)
                {
                    if (string.Equals(accounts[i].OwnerEmail, account.OwnerEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        accountExists = true;
                        accounts[i] = account;
                        break;
                    }
                }
                if (!accountExists)
                {
                    accounts.Add(account);
                }

                // Serialize the list of accounts to JSON and write it to the file
                string jsonContent = JsonConvert.SerializeObject(accounts, Formatting.Indented);
                File.WriteAllText(_filePath, jsonContent);
            }
        }

        public BankAccount GetOrCreateAccount(string email)
        {
            BankAccount bankAccount = FindByOwnerEmail(email);
            if(bankAccount != null)
            {
                return bankAccount;
            }
            else
            {
                bankAccount = new BankAccount(email, 0);
                SaveAccount(bankAccount);
            }

            return bankAccount;
        }
    }
}
