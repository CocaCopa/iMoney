using System.Collections.Generic;
using CocaCopa.Logger.API;
using CocaCopa.SaveSystem.API;
using iMoney.Transactions.API;
using iMoney.Transactions.Contracts;
using UnityEngine;

public class TEMP_Tester : MonoBehaviour {
    [SerializeField] private bool save1;
    [SerializeField] private bool remove;
    [SerializeField] private bool daily;
    [SerializeField] private bool weekly;

    // Update is called once per frame
    void Update() {
        if (save1) {
            save1 = false;
            Transaction tr = Transaction.Create(123123, new TransactionAmount(1200, 10), "Spend", "Games");
            TransactionsManager.AddEntry(tr);
        }
        if (remove) {
            remove = false;
            Debug.Log("Debug removing entry doesn't work right now");
            // TransactionsManager.RemoveEntry("1");
        }
        if (daily) {
            daily = false;
            var entries = TransactionsManager.GetDailyTransactions();
            foreach (var entry in entries) {
                Debug.Log($"Type: {entry.Type} | Amount: {entry.Amount.Value / (float)entry.Amount.Multiplier} | Category: {entry.Category}");
            }
        }
        if (weekly) {
            weekly = false;
            var week = TransactionsManager.GetCurrentWeekTransactions();
            for (int i = 0; i < week.Count; i++) {
                Log.Warning($"Day: {IndexToDay(i)}", LogColor.Yellow);
                foreach (var entry in week[IndexToDay(i)]) {
                    Log.Info($"Type: {entry.Type} | Amount: {entry.Amount.Value / (float)entry.Amount.Multiplier} | Category: {entry.Category}", LogColor.White);
                }
            }
        }
    }

    private string IndexToDay(int index) {
        return index switch {
            0 => "Monday",
            1 => "Tuesday",
            2 => "Wednesday",
            3 => "Thursday",
            4 => "Friday",
            5 => "Saturday",
            6 => "Sunday",
            _ => "Unknown",
        };
    }
}
