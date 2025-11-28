using iMoney.Transactions.API;
using iMoney.Transactions.Runtime;
using UnityEngine;

namespace iMoney.Transactions.Unity {
    internal class TransactionsInstaller : MonoBehaviour {
        private const string FileName = "iMoney.json";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init() {
            TransactionsFlow flow = new TransactionsFlow(FileName);
            TransactionsManager.Initialize(flow);
        }
    }
}
