using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace NationalBankManagement.Models
{
    public class Branch
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";

        public Manager? Manager { get; set; }

        public List<Account> Accounts { get; set; } = new();
    }
}
