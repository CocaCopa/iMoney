using iMoney.Transactions.API;
using iMoney.Transactions.Contracts;

namespace iMoney.App.Spendings.Runtime {
    internal class SpendFlow {
        private readonly IContentPresenter dailyPresenter;
        private readonly IContentPresenter weeklyPresenter;

        internal SpendFlow(IContentPresenter dailyPresenter, IContentPresenter weeklyPresenter) {
            this.dailyPresenter = dailyPresenter;
            this.weeklyPresenter = weeklyPresenter;
        }

        internal void Initialize() {
            ManageDailySpendings();
        }

        private void ManageDailySpendings() {
            var entries = TransactionsManager.GetDailyTransactions();
            for (int i = 0; i < entries.Count; i++) {
                Transaction entry = entries[i];
                string category = entry.Category;
                decimal amount = entry.Amount.Value / (decimal)entry.Amount.Multiplier;
                TransactionType trType = MapTransactionType(entry.Type);
                dailyPresenter.AddTransactionRow(category, amount, trType);
            }
        }

        private TransactionType MapTransactionType(string type) {
            return type switch {
                "Add" => TransactionType.Add,
                "Spend" => TransactionType.Spend,
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
