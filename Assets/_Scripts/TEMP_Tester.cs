using System.Collections.Generic;
using CocaCopa.SaveSystem.API;
using iMoney.Transactions.API;
using iMoney.Transactions.Contracts;
using UnityEngine;

public class TEMP_Tester : MonoBehaviour {
    [SerializeField] private bool save1;
    [SerializeField] private bool load;

    // Update is called once per frame
    void Update() {
        if (save1) {
            save1 = false;
            Transaction tr = Transaction.Create(123123, new TransactionAmount(1200, 10), "Spend", "Games");
            TransactionsManager.AddEntry(tr);
        }
        if (load) {
            load = false;
            TransactionsManager.RemoveEntry("1");
        }
    }
}
