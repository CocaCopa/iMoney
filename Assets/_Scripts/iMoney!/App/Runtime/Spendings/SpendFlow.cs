using iMoney.Transactions.API;
using iMoney.Transactions.Contracts;

namespace iMoney.App.Spendings.Runtime {
    internal class SpendFlow {
        private readonly IContentPresenter dailyPresenter;
        private readonly IContentPresenter weeklyPresenter;

        public SpendFlow(IContentPresenter dailyPresenter, IContentPresenter weeklyPresenter) {
            this.dailyPresenter = dailyPresenter;
            this.weeklyPresenter = weeklyPresenter;

            ManageDailySpendings();
        }

        private void ManageDailySpendings() {
            var entries = TransactionsManager.GetDailyTransactions();
            for (int i = 0; i < entries.Count; i++) {
                Transaction entry = entries[i];
                string category = entry.Category;
                string amount = (entry.Amount.Value / (float)entry.Amount.Multiplier).ToString("0.00");
                bool useDarkColor = i % 2 != 0;
                dailyPresenter.AddSpendRow(useDarkColor, category, amount, entry.Type);
            }
        }
    }
}
