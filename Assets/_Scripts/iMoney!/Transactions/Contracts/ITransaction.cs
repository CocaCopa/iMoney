using System.Collections.Generic;

namespace iMoney.Transactions.Contracts {
    public interface ITransaction {
        void AddEntry(Transaction transaction);
        void RemoveEntry(string id);
        List<Transaction> GetDailyTransactions();
        Dictionary<string, List<Transaction>> GetCurrentWeekTransactions();
    }
}
