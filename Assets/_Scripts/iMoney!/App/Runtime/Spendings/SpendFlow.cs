using System.Collections.Generic;
using iMoney.Transactions.API;
using iMoney.Transactions.Contracts;

namespace iMoney.App.Spendings.Runtime {
    internal class SpendFlow {
        private readonly IContentPresenter dailyPresenter;
        private readonly IContentPresenter weeklyPresenter;

        private readonly List<RowData> dailyRows = new List<RowData>();

        internal SpendFlow(IContentPresenter dailyPresenter, IContentPresenter weeklyPresenter) {
            this.dailyPresenter = dailyPresenter;
            this.weeklyPresenter = weeklyPresenter;
        }

        internal void Initialize() {
            ManageDailySpendings();
            ManageWeeklySpendings();
        }

        private void ManageWeeklySpendings() {
            var entries = TransactionsManager.GetCurrentWeekTransactions();
            foreach (var entry in entries) {
                string day = entry.Key;
                var transactions = entry.Value;
                decimal total = 0;
                foreach (var transaction in transactions) {
                    total += transaction.Amount.Value / (decimal)transaction.Amount.Multiplier;
                }
                weeklyPresenter.AddTransactionRow(day, total, TransactionType.Neutral);
            }
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

        private readonly struct RowData {
            public readonly string Title;
            public readonly decimal Amount;
            public readonly TransactionType TrType;
            public RowData(string title, decimal amount, TransactionType trType) {
                Title = title;
                Amount = amount;
                TrType = trType;
            }
        }
    }
}
