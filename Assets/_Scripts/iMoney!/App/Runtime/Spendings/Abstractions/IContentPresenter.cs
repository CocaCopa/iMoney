namespace iMoney.App.Spendings.Runtime {
    internal interface IContentPresenter {
        public void AddTransactionRow(string title, decimal amount, TransactionType trType);
    }
}
