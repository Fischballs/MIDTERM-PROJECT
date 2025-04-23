
namespace BudgetTracker.ohaha
{
    public interface IBudgetManager
    {
        List<Transaction> Transactions { get; set; }

        void AddTransaction(Transaction transaction);
        Dictionary<string, decimal> GetCategoryWiseExpenses();
        List<Transaction> GetFilteredTransactions(string category = "", DateTime? startDate = null, DateTime? endDate = null);
        decimal GetNetSavings();
        decimal GetTotalExpenses();
        decimal GetTotalIncome();
        void RemoveMultipleTransactions(List<Transaction> selectedTransactions);
        void RemoveTransaction(Transaction transaction);
    }
}
