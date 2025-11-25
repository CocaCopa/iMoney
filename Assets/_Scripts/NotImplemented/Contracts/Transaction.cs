using System;

namespace iMoney.Transactions.Contracts {
    [Serializable]
    /// <summary>
    /// <para> A single financial entry recorded by the user. </para>
    /// <para> Stores timestamp, amount, type, and category information. </para>
    /// </summary>
    public struct Transaction {
        /// <summary>
        /// Timestamp of when the transaction occurred.
        /// Stored as a long for serialization efficiency.
        /// </summary>
        public long timestamp;
        public TransactionAmount amount;
        /// <summary>
        /// The kind of transaction performed.
        /// Could be "add" or "spend".
        /// </summary>
        public string type;
        /// <summary>
        /// Category this transaction belongs to (e.g., food, bills, salary).
        /// </summary>
        public string category;

        /// <summary>
        /// A single financial entry recorded by the user.
        /// </summary>
        /// <param name="timestamp">Timestamp of when the transaction occurred.</param>
        /// <param name="amountInt">The raw integer value of the amount (unscaled).</param>
        /// <param name="amountMultiplier">The scale factor of the stored integer amount.</param>
        /// <param name="type">The kind of transaction performed.</param>
        /// <param name="category">Category this transaction belongs to.</param>
        public Transaction(long timestamp, TransactionAmount amount, string type, string category) {
            this.timestamp = timestamp;
            this.amount = amount;
            this.type = type;
            this.category = category;
        }
    }

    [Serializable]
    public struct TransactionAmount {
        public int value;
        public int multiplier;
        public TransactionAmount(int value, int multiplier) {
            this.value = value;
            this.multiplier = multiplier;
        }
    }
}
