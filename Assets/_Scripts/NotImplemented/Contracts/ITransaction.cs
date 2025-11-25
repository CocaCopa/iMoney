namespace iMoney.Transactions.Contracts {
    public interface ITransaction {
        void Save<T>(T data, string filePath);
        bool Load<T>(string filePath, out T result);
    }
}
