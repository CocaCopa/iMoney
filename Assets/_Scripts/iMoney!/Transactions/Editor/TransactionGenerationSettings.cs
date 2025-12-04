using System;

namespace iMoney.Transactions.EditorTools {
    [Serializable]
    public struct TransactionGenerationSettings {
        public int StartYear;
        public int EndYear;
        public bool[] Months;      // length 12, Jan..Dec
        public bool[] Weeks;       // length 5, week buckets (1–4 + overflow)
        public int EntriesPerWeek;
    }
}
