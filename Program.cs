
using Microsoft.EntityFrameworkCore;
using NationalBankManagement.Data;
using NationalBankManagement.Models;

namespace NationalBankManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using BankContext db = new BankContext();

           
            if (!db.Branches.Any())
            {
                db.Branches.AddRange(
                    new Branch
                    {
                        Code = "CAI-01",
                        Name = "Cairo Main Branch",
                        Address = "Cairo",
                        PhoneNumber = "0223456789"
                    },
                    new Branch
                    {
                        Code = "ALX-01",
                        Name = "Alexandria Branch",
                        Address = "Alexandria",
                        PhoneNumber = "0323456789"
                    }
                );

                db.SaveChanges();
            }

         
            if (!db.Managers.Any())
            {
                db.Managers.AddRange(
                    new Manager
                    {
                        FullName = "Ahmed Ali",
                        Email = "ahmed@bank.com",
                        PhoneNumber = "01012345678",
                        HireDate = DateTime.Today,
                        BranchCode = "CAI-01"
                    },
                    new Manager
                    {
                        FullName = "Mona Hassan",
                        Email = "mona@bank.com",
                        PhoneNumber = "01112345678",
                        HireDate = DateTime.Today,
                        BranchCode = "ALX-01"
                    }
                );

                db.SaveChanges();
            }

          
            while (true)
            {
                Console.Clear();

                Console.WriteLine("====================================");
                Console.WriteLine("     National Bank - Management");
                Console.WriteLine("====================================");
                Console.WriteLine("1) Add a new Customer");
                Console.WriteLine("2) Open a new Account for a Customer");
                Console.WriteLine("3) Update Account Status");
                Console.WriteLine("4) Remove an Account from a Customer");
                Console.WriteLine("5) List all Customers (with accounts)");
                Console.WriteLine("0) Exit");
                Console.WriteLine("====================================");

                int choice = ReadInt("Enter choice: ");

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        AddCustomer(db);
                        break;

                    case 2:
                        OpenAccount(db);
                        break;

                    case 3:
                        UpdateAccountStatus(db);
                        break;

                    case 4:
                        RemoveAccount(db);
                        break;

                    case 5:
                        ListCustomers(db);
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }

        static int ReadInt(string message)
        {
            int number;

            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out number))
                    return number;

                Console.WriteLine("Invalid number. Please try again.");
            }
        }

        static string ReadText(string message)
        {
            while (true)
            {
                Console.Write(message);
                string value = Console.ReadLine()?.Trim() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                    return value;

                Console.WriteLine("This field is required.");
            }
        }

     
        static DateTime ReadDate(string message)
        {
            DateTime date;

            while (true)
            {
                Console.Write(message);

                if (DateTime.TryParse(Console.ReadLine(), out date))
                    return date;

                Console.WriteLine("Invalid date. Please try again.");
            }
        }

        // Option 1
        static void AddCustomer(BankContext db)
        {
            Console.WriteLine("--- Add New Customer ---");

            string name = ReadText("Full Name: ");

            string nationalId;

            while (true)
            {
                nationalId = ReadText("National ID: ");

                if (nationalId.Length != 14 ||
                    !nationalId.All(char.IsDigit))
                {
                    Console.WriteLine("National ID must contain 14 digits.");
                    continue;
                }

                if (db.Customers.Any(c => c.NationalId == nationalId))
                {
                    Console.WriteLine("This National ID already exists.");
                    continue;
                }

                break;
            }

            DateTime birthDate;

            while (true)
            {
                birthDate = ReadDate("Date of Birth: ");

                if (birthDate.Date < DateTime.Today)
                    break;

                Console.WriteLine("Date of birth must be in the past.");
            }

            string email;

            while (true)
            {
                email = ReadText("Email: ");

                if (email.Contains('@') &&
                    email.Contains('.') &&
                    !email.Contains(' '))
                    break;

                Console.WriteLine("Invalid email.");
            }

            string phone;

            while (true)
            {
                phone = ReadText("Phone: ");

                if (phone.Length == 11 &&
                    phone.All(char.IsDigit) &&
                    (phone.StartsWith("010") ||
                     phone.StartsWith("011") ||
                     phone.StartsWith("012") ||
                     phone.StartsWith("015")))
                    break;

                Console.WriteLine("Enter a valid Egyptian phone number.");
            }

            string address = ReadText("Address: ");

            Console.WriteLine("Customer Type:");
            Console.WriteLine("1) Individual");
            Console.WriteLine("2) Business");

            int type;

            do
            {
                type = ReadInt("Choice: ");

                if (type != 1 && type != 2)
                    Console.WriteLine("Please choose 1 or 2.");

            } while (type != 1 && type != 2);

            Customer customer = new Customer
            {
                FullName = name,
                NationalId = nationalId,
                DateOfBirth = birthDate,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = type == 1 ? "Individual" : "Business"
            };

            db.Customers.Add(customer);
            db.SaveChanges();

            Console.WriteLine();
            Console.WriteLine(
                $"Customer created successfully. CustomerID = {customer.Id}");
        }

        // Option 2
        static void OpenAccount(BankContext db)
        {
            Console.WriteLine("--- Open New Account ---");

            string accountNumber = ReadText("Account Number: ");

            if (db.Accounts.Any(a => a.AccountNumber == accountNumber))
            {
                Console.WriteLine("This account number already exists.");
                return;
            }

            Console.WriteLine("Account Type:");
            Console.WriteLine("1) Savings");
            Console.WriteLine("2) Current");
            Console.WriteLine("3) Business");

            int type;

            do
            {
                type = ReadInt("Choice: ");

                if (type < 1 || type > 3)
                    Console.WriteLine("Please choose 1, 2 or 3.");

            } while (type < 1 || type > 3);

            string accountType = type == 1 ? "Savings" :
                                 type == 2 ? "Current" : "Business";

            string branchCode = ReadText("Branch Code: ");

            Branch? branch = db.Branches
                .FirstOrDefault(b => b.Code == branchCode);

            if (branch == null)
            {
                Console.WriteLine("Branch not found.");
                return;
            }

            int customerId = ReadInt("Customer Id: ");

            Customer? customer = db.Customers
                .FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.WriteLine("Ownership Role:");
            Console.WriteLine("1) Primary");
            Console.WriteLine("2) CoHolder");

            int role;

            do
            {
                role = ReadInt("Choice: ");

                if (role != 1 && role != 2)
                    Console.WriteLine("Please choose 1 or 2.");

            } while (role != 1 && role != 2);

            string ownership = role == 1 ? "Primary" : "CoHolder";

            Account account = new Account
            {
                AccountNumber = accountNumber,
                AccountType = accountType,
                CurrentBalance = 0,
                OpeningDate = DateTime.Today,
                BranchCode = branchCode
            };

            CustomerAccount customerAccount = new CustomerAccount
            {
                CustomerId = customerId,
                AccountNumber = accountNumber,
                OwnershipType = ownership,
                OwnershipStartDate = DateTime.Today,
                AccountStatus = "Active"
            };

            db.Accounts.Add(account);
            db.CustomerAccounts.Add(customerAccount);

            db.SaveChanges();

            Console.WriteLine();
            Console.WriteLine(
                $"Account '{accountNumber}' created and linked to customer {customerId} as {ownership} owner.");
        }

        // Option 3
        static void UpdateAccountStatus(BankContext db)
        {
            Console.WriteLine("--- Update Account Status ---");

            string accountNumber = ReadText("Account Number: ");
            int customerId = ReadInt("Customer Id: ");

            CustomerAccount? customerAccount = db.CustomerAccounts
                .FirstOrDefault(ca =>
                    ca.AccountNumber == accountNumber &&
                    ca.CustomerId == customerId);

            if (customerAccount == null)
            {
                Console.WriteLine("Account ownership not found.");
                return;
            }

            customerAccount.AccountStatus =
                customerAccount.AccountStatus == "Active"
                ? "Closed" : "Active";

            db.SaveChanges();

            Console.WriteLine(
                $"Status updated to {customerAccount.AccountStatus}.");
        }

        // Option 4
        static void RemoveAccount(BankContext db)
        {
            Console.WriteLine("--- Remove Account From Customer ---");

            string accountNumber = ReadText("Account Number: ");
            int customerId = ReadInt("Customer Id: ");

            CustomerAccount? customerAccount = db.CustomerAccounts
                .FirstOrDefault(ca =>
                    ca.AccountNumber == accountNumber &&
                    ca.CustomerId == customerId);

            if (customerAccount == null)
            {
                Console.WriteLine("Account ownership not found.");
                return;
            }

            db.CustomerAccounts.Remove(customerAccount);
            db.SaveChanges();

            Console.WriteLine("Ownership link deleted.");

            bool hasOwners = db.CustomerAccounts
                .Any(ca => ca.AccountNumber == accountNumber);

            if (!hasOwners)
            {
                Account? account = db.Accounts
                    .FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (account != null)
                {
                    bool hasTransactions = db.Transactions
                        .Any(t => t.AccountNumber == accountNumber);

                    if (!hasTransactions)
                    {
                        db.Accounts.Remove(account);
                        db.SaveChanges();

                        Console.WriteLine(
                            "That was the last owner - account was also removed.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Account kept because it has transactions.");
                    }
                }
            }
        }

        // Option 5
        static void ListCustomers(BankContext db)
        {
            Console.WriteLine("--- All Customers ---");

            var customers = db.Customers
                .Include(c => c.CustomerAccounts)
                    .ThenInclude(ca => ca.Account)
                        .ThenInclude(a => a.Branch)
                .ToList();

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            foreach (var customer in customers)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"#{customer.Id} {customer.FullName} ({customer.CustomerType})");

                if (customer.CustomerAccounts.Count == 0)
                {
                    Console.WriteLine("   (no accounts)");
                    continue;
                }

                foreach (var ca in customer.CustomerAccounts)
                {
                    Console.WriteLine(
                        $"   {ca.AccountNumber}  " +
                        $"{ca.Account.AccountType}  " +
                        $"Balance: {ca.Account.CurrentBalance:N2}  " +
                        $"{ca.OwnershipType}  " +
                        $"{ca.AccountStatus}  " +
                        $"@ {ca.Account.Branch.Name}");
                }
            }
        }
    }
}
