using System;
using System.Collections.Generic;
using iMoney.Transactions.Contracts;
using UnityEngine;

namespace iMoney.Transactions.EditorTools {
    [Serializable]
    public class TransactionRoot {
        public List<Transaction> transactions;
    }

    public static class TransactionTestDataGenerator {
        private static readonly System.Random Rng = new System.Random();

        private static readonly string[] Categories = {
        "food", "market", "rent", "bills", "fun",
        "coffee", "salary", "fuel", "snacks", "other"
    };

        public static TransactionRoot Generate(TransactionGenerationSettings settings) {
            var root = new TransactionRoot {
                transactions = new List<Transaction>()
            };

            for (int year = settings.StartYear; year <= settings.EndYear; year++) {
                for (int month = 1; month <= 12; month++) {
                    if (!settings.Months[month - 1]) continue;
                    AddMonth(root.transactions, year, month, settings.EntriesPerWeek, settings.Weeks);
                }
            }

            Debug.Log($"[TransactionTestDataGenerator] Generated {root.transactions.Count} transactions " +
                      $"for years {settings.StartYear}–{settings.EndYear}.");
            return root;
        }

        private static void AddMonth(
            List<Transaction> list,
            int year,
            int month,
            int entriesPerWeek,
            bool[] weekSelected
        ) {
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // For each "week bucket": 0..4
            for (int weekIndex = 0; weekIndex < 5; weekIndex++) {
                if (!weekSelected[weekIndex])
                    continue;

                // Get all days in this week
                List<int> daysInWeek = new List<int>();

                for (int day = 1; day <= daysInMonth; day++) {
                    if (GetWeekOfMonthIndex(day) == weekIndex)
                        daysInWeek.Add(day);
                }

                if (daysInWeek.Count == 0)
                    continue;

                // Spread entriesPerWeek over these days
                int basePerDay = entriesPerWeek / daysInWeek.Count;
                int remainder = entriesPerWeek % daysInWeek.Count;

                int dayCounter = 0;

                foreach (int day in daysInWeek) {
                    int entriesThisDay = basePerDay + (dayCounter < remainder ? 1 : 0);
                    dayCounter++;

                    for (int i = 0; i < entriesThisDay; i++) {
                        DateTime dt = new DateTime(year, month, day, 8, 0, 0)
                            .AddMinutes(i * 60); // 1-hour step per entry in that day

                        var t = new Transaction {
                            ID = Guid.NewGuid().ToString("N"),
                            Timestamp = dt.Ticks,
                            Amount = new TransactionAmount {
                                Value = RandomAmount(),
                                Multiplier = 100
                            },
                            Type = (Rng.Next(0, 2) == 0) ? "Add" : "Spend",
                            Category = Categories[Rng.Next(Categories.Length)]
                        };

                        list.Add(t);
                    }
                }
            }

            Debug.Log($"[TransactionTestDataGenerator] {year:D4}-{month:D2}: Week-based entries added.");
        }

        // Week-of-month index: 0..4
        // 0: days 1-7, 1: 8-14, 2: 15-21, 3: 22-28, 4: 29-31
        private static int GetWeekOfMonthIndex(int day) => (day - 1) / 7;

        private static int RandomAmount() {
            // 1.00 to 200.00 (Value represents "cents" with Multiplier = 100)
            return Rng.Next(100, 20000);
        }
    }
}
