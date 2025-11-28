using System;
using System.Collections.Generic;

namespace iMoney.Transactions.Contracts {
    [Serializable]
    public class TransactionsContainer {
        public List<Transaction> transactions;
        public TransactionsContainer() {
            transactions = new List<Transaction>();
        }
        public TransactionsContainer(List<Transaction> transactions) {
            this.transactions = transactions;
        }
    }
}