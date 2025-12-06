using System;
using System.Collections.Generic;
using System.Linq;
using CocaCopa.Core.Dates;
using CocaCopa.Core.Dates.Group;
using CocaCopa.SaveSystem.API;
using iMoney.Transactions.Contracts;

namespace iMoney.Transactions.Runtime {
    internal sealed class TransactionsFlow : ITransaction {
        private Exception FileLoadFailedException => new Exception("[Transactions] Could not load transactions file");
        private readonly string fileName;
        private readonly List<Transaction> dailyTransactions = new List<Transaction>();
        private readonly Dictionary<string, List<Transaction>> weekTransactions = new() {
            ["Monday"] = new List<Transaction>(),
            ["Tuesday"] = new List<Transaction>(),
            ["Wednesday"] = new List<Transaction>(),
            ["Thursday"] = new List<Transaction>(),
            ["Friday"] = new List<Transaction>(),
            ["Saturday"] = new List<Transaction>(),
            ["Sunday"] = new List<Transaction>()
        };

        public TransactionsFlow(string fileName) {
            this.fileName = fileName;
            CalculateDailyTransactions();
            CalculateCurrentWeekTransactions();
        }

        private void CalculateDailyTransactions() {
            if (SaveStorage.Load<TransactionsContainer>(fileName, out var cont)) {
                List<Transaction> entries = cont.transactions;
                var opts = new GroupOptions {
                    Period = GroupByPeriod.Day,
                    Format = "{day}/{month}/{year}"
                };
                var grouped = entries.GroupBy(e => e.Timestamp.FromLocalTicks(), opts);
                dailyTransactions.AddRange(grouped[DateTime.Now.FormatDate()]);
            }
            else { throw FileLoadFailedException; }
        }

        private void CalculateCurrentWeekTransactions() {
            if (!SaveStorage.Load<TransactionsContainer>(fileName, out var cont)) {
                throw FileLoadFailedException;
            }

            List<Transaction> entries = cont.transactions;
            DateTime today = DateTime.Now.Date;

            // Find the Monday of the current week (week = Monday–Sunday)
            int offsetFromMonday = ((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime weekStart = today.AddDays(-offsetFromMonday); // Monday
            DateTime weekEndExclusive = weekStart.AddDays(7); // Next Monday

            for (int i = 0; i < entries.Count; i++) {
                Transaction tr = entries[i];
                DateTime date = tr.Timestamp.FromLocalTicks().Date;
                if (date >= weekStart && date < weekEndExclusive) {
                    string dayName = date.DayOfWeek.ToString();
                    if (weekTransactions.ContainsKey(dayName)) {
                        weekTransactions[dayName].Add(tr);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the daily transactions
        /// </summary>
        /// <returns>Today's transactions</returns>
        public List<Transaction> GetDailyTransactions() => dailyTransactions;
        /// <summary>
        /// Gets the current week transactions
        /// </summary>
        /// <returns>This week's transactions</returns>
        public Dictionary<string, List<Transaction>> GetCurrentWeekTransactions() => weekTransactions;

        public void AddEntry(Transaction transaction) {
            if (SaveStorage.Load<TransactionsContainer>(fileName, out var cont)) {
                cont.transactions.Add(transaction);
            }
            else cont = new TransactionsContainer(new List<Transaction> { transaction });

            weekTransactions[DateTime.Now.DayOfWeek.ToString()].Add(transaction);
            dailyTransactions.Add(transaction);
            SaveStorage.Save(cont, fileName);
        }

        public void RemoveEntry(string id) {
            if (!SaveStorage.Load<TransactionsContainer>(fileName, out var cont)) {
                throw FileLoadFailedException;
            }

            int index = cont.transactions.FindIndex(t => string.Equals(t.ID, id, StringComparison.Ordinal));

            if (index < 0) {
                throw new KeyNotFoundException($"Transaction with ID '{id}' was not found.");
            }
            cont.transactions.RemoveAt(index);
            SaveStorage.Save(cont, fileName);
        }
    }
}
