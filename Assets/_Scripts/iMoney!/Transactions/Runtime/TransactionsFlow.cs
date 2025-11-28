using System.Collections.Generic;
using CocaCopa.SaveSystem.API;
using iMoney.Transactions.Contracts;

namespace iMoney.Transactions.Runtime {
    internal sealed class TransactionsFlow : ITransaction {
        private readonly string fileName;

        public TransactionsFlow(string fileName) {
            this.fileName = fileName;
        }

        public void AddEntry(Transaction transaction) {
            if (SaveStorage.Load<TransactionsContainer>(fileName, out var cont)) {
                cont.transactions.Add(transaction);
            }
            else cont = new TransactionsContainer(new List<Transaction> { transaction });
            SaveStorage.Save(cont, fileName);
        }

        public void RemoveEntry(string id) {
            if (SaveStorage.Load<TransactionsContainer>(fileName, out var cont)) {
                for (int i = 0; i < cont.transactions.Count; i++) {
                    string entryId = cont.transactions[i].ID;
                    if (entryId == id) {
                        Transaction tr = cont.transactions[i];
                        cont.transactions.Remove(tr);
                        break;
                    }
                }
                SaveStorage.Save(cont, fileName);
            }
            else throw new System.Exception("Could not remove requested entry");
        }
    }
}
