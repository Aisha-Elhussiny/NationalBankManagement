using System;
using System.Collections.Generic;
using System.Text;

namespace NationalBankManagement.Models
{
    public class CustomerAccount
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string AccountNumber { get; set; } = "";
        public Account Account { get; set; } = null!;

        public string OwnershipType { get; set; } = "";
        public DateTime OwnershipStartDate { get; set; }
        public string AccountStatus { get; set; } = "";
    }
}
