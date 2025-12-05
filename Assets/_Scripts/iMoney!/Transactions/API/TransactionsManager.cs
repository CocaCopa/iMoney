using System.Collections.Generic;
using iMoney.Transactions.Contracts;

namespace iMoney.Transactions.API {
    public static class TransactionsManager {
        private static ITransaction impl;

        internal static void Initialize(ITransaction impl) {
            if (TransactionsManager.impl != null) throw new System.Exception($"[TransactionsManager] The {nameof(ITransaction)} implementation has already been wired");
            TransactionsManager.impl = impl ?? throw new System.NullReferenceException("[TransactionsManager]");
        }

        public static void AddEntry(Transaction transaction) => impl?.AddEntry(transaction);
        public static void RemoveEntry(string id) => impl?.RemoveEntry(id);
        public static List<Transaction> GetDailyTransactions() => impl?.GetDailyTransactions();
    }
}
