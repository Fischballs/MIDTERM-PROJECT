using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace BudgetTracker.ohaha
{
    public class BudgetManager
    {
        public string FilePath { get; set; } = "Trans.csv";
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public BudgetManager()
        {
            LoadTransactions();
        }

        // Add a transaction and save to CSV
        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
            SaveTransactions();
        }

        // Remove a single transaction
        public void RemoveTransaction(Transaction transaction)
        {
            Transactions.Remove(transaction);
            SaveTransactions();
        }

        // Remove multiple selected transactions
        public void RemoveMultipleTransactions(List<Transaction> selectedTransactions)
        {
            foreach (var transaction in selectedTransactions)
            {
                Transactions.Remove(transaction);
            }
            SaveTransactions();
        }

        // Save transactions to CSV file
        private void SaveTransactions()
        {
            try
            {
                File.WriteAllLines(FilePath, Transactions.Select(t =>
                    $"{t.Date:yyyy-MM-dd},{t.Description},{t.Amount:F2},{t.Type},{t.Category}"));
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }

        // Load transactions from CSV file
        private void LoadTransactions()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    Transactions = File.ReadAllLines(FilePath)
                                       .Select(Transaction.FromCSV)
                                       .ToList();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
            }
        }

        // Summary Calculations
        public decimal GetTotalIncome() => Transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
        public decimal GetTotalExpenses() => Transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
        public decimal GetNetSavings() => GetTotalIncome() - GetTotalExpenses();

        // Category-wise expenses
        public Dictionary<string, decimal> GetCategoryWiseExpenses()
        {
            return Transactions
                .Where(t => t.Type == "Expense")
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }

        // Filter transactions
        public List<Transaction> GetFilteredTransactions(string category = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            return Transactions.Where(t =>
                (string.IsNullOrEmpty(category) || t.Category.Equals(category, StringComparison.OrdinalIgnoreCase)) &&
                (!startDate.HasValue || t.Date >= startDate) &&
                (!endDate.HasValue || t.Date <= endDate)
            ).ToList();
        }
    }
}
