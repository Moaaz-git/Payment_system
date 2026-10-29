using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace Payment_system
{
    class UserStore
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");


        public List<UserAccount> LoadAccounts()
        {
            if (!File.Exists(_filePath))
            {
                return new List<UserAccount>();
            }

            // Read the JSON file and deserialize it into a list of UserAccount objects
            string textContent = File.ReadAllText(_filePath);
            // Deserialize the JSON into a list of UserAccount objects
            List<UserAccount> userAccounts = JsonConvert.DeserializeObject<List<UserAccount>>(textContent);
            //if the deserialization returns null, return an empty list instead
            return userAccounts ?? new List<UserAccount>();

        }

        public UserAccount FindByEmail(string email)
        {
            foreach(var account in LoadAccounts())
            {
                if (string.Equals(account.Email, email, StringComparison.OrdinalIgnoreCase))
                {
                    return account;
                }
            }
            return null;
        }

    }
}
