namespace iMoney.App.Spendings.Runtime {
    internal interface IContentPresenter {
        void AddSpendRow(bool useDarkColor, string title, string amount, string type);
    }
}
