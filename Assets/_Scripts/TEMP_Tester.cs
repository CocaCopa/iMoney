using System.Collections.Generic;
using CocaCopa.SaveSystem.API;
using iMoney.Transactions.Contracts;
using UnityEngine;

public class TEMP_Tester : MonoBehaviour {
    [SerializeField] private bool save1;
    [SerializeField] private bool load;

    // Update is called once per frame
    void Update() {
        if (save1) {
            save1 = false;
            Transaction tr = new Transaction(123123, new TransactionAmount(1200, 10), "Spend", "Games");
            TransactionsContainer cont = new TransactionsContainer(new List<Transaction> { tr });
            SaveStorage.Save(cont, "transactions.json");
        }
        if (load) {
            load = false;
            if (SaveStorage.Load("transactions.json", out TransactionsContainer cont)) {
                Debug.Log(cont.transactions[0].type);
            }
        }
    }
}
