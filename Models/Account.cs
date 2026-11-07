using System;
using System.Collections.Generic;
using System.Text;

namespace NationalBankManagement.Models
{
    public class Account
    {
        public string AccountNumber { get; set; } = "";
        public string AccountType { get; set; } = "";
        public decimal CurrentBalance { get; set; }
        public DateTime OpeningDate { get; set; }

        public string BranchCode { get; set; } = "";
        public Branch Branch { get; set; } = null!;

        public List<CustomerAccount> CustomerAccounts { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();
    }
}
