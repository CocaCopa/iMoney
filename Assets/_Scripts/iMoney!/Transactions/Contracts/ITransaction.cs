namespace iMoney.Transactions.Contracts {
    public interface ITransaction {
        void AddEntry(Transaction transaction);
        void RemoveEntry(string id);
    }
}
