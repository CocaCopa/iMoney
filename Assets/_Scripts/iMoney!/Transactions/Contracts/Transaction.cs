using System;

namespace iMoney.Transactions.Contracts {
    [Serializable]
    /// <summary>
    /// <para> A single financial entry recorded by the user. </para>
    /// <para> Stores timestamp, amount, type, and category information. </para>
    /// </summary>
    public struct Transaction {
        public string ID;
        /// <summary>
        /// Timestamp of when the transaction occurred.
        /// Stored as a long for serialization efficiency.
        /// </summary>
        public long Timestamp;
        public TransactionAmount Amount;
        /// <summary>
        /// The kind of transaction performed.
        /// Could be "add" or "spend".
        /// </summary>
        public string Type;
        /// <summary>
        /// Category this transaction belongs to (e.g., food, bills, salary).
        /// </summary>
        public string Category;

        public static Transaction Create(long timestamp, TransactionAmount amount, string type, string category) {
            string id = Guid.NewGuid().ToString("N");
            return new Transaction(id, timestamp, amount, type, category);
        }

        public static Transaction Default() {
            return new Transaction(
                id: "-1",
                timestamp: 0,
                amount: new TransactionAmount(),
                type: "N/A",
                category: "NA"
            );
        }

        private Transaction(string id, long timestamp, TransactionAmount amount, string type, string category) {
            ID = id;
            Timestamp = timestamp;
            Amount = amount;
            Type = type;
            Category = category;
        }
    }

    [Serializable]
    public struct TransactionAmount {
        public int Value;
        public int Multiplier;

        public TransactionAmount(int value, int multiplier) {
            Value = value;
            Multiplier = multiplier;
        }
    }
}
