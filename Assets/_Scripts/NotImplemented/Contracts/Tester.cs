// using System;
// using System.Collections.Generic;
// using System.IO;
// using iMoney.Transactions.Contracts;
// using UnityEngine;

// namespace iMoney.Transactions.Runtime {
//     public class Tester : MonoBehaviour {
//         [SerializeField] private bool save1;
//         [SerializeField] private bool save2;
//         [SerializeField] private bool load;

//         private TransactionsManager manager;

//         private void Awake() {
//             manager = new TransactionsManager();
//         }

//         private void Update() {
//             if (save1) {
//                 save1 = false;
//                 Transaction tr = new Transaction(DateTime.Now.Ticks, new TransactionAmount(1500, 100), "Spend", "Food");
//                 AddEntry(tr);
//             }
//             if (save2) {
//                 save2 = false;
//                 Transaction tr = new Transaction(DateTime.Now.Ticks, new TransactionAmount(1500, 100), "Add", "Food");
//                 AddEntry(tr);
//             }
//             if (load) {
//                 load = false;
//                 var path = Path.Combine(Application.persistentDataPath, "transactions.json");
//                 if (manager.Load<TransactionsContainer>(path, out var result)) {
//                     Debug.Log(result.transactions[0].type);
//                     Debug.Log(result.transactions[0].amount.value);

//                     Debug.Log("");

//                     Debug.Log(result.transactions[1].type);
//                     Debug.Log(result.transactions[1].amount.value);
//                 }
//             }
//         }

//         private void AddEntry(Transaction tr) {
//             var path = Path.Combine(Application.persistentDataPath, "transactions.json");
//             if (manager.Load<TransactionsContainer>(path, out var cont)) {
//                 cont.transactions.Add(tr);
//             }
//             else cont = new TransactionsContainer(new List<Transaction> { tr });
//             manager.Save(cont, path);
//         }
//     }
// }
