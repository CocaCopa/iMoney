using System;
using System.Threading;
using CocaCopa.Modal.API;
using CocaCopa.Modal.Contracts;
using iMoney.BalanceEntry.Runtime;
using iMoney.BalanceEntry.SPI;
using UnityEngine;

namespace iMoney.BalanceEntry.Unity {
    internal class BalanceFlowInstaller : MonoBehaviour {
        [SerializeField] private MonoBehaviour buttonsUI;
        [SerializeField] private ModalAdapter balanceModal;
        [SerializeField] private ModalAdapter categoryModal;

        private IModalService BalanceService => balanceModal.GetService();
        private IModalService CategoryService => categoryModal.GetService();

        private BalanceFlow balanceFlow;
        private IBalanceIntent BalanceIntent => (IBalanceIntent)buttonsUI;
        private CancellationTokenSource cts;

        private void OnValidate() {
            if (buttonsUI == null) { throw new ArgumentNullException(nameof(buttonsUI)); }
            if (buttonsUI is not IBalanceIntent) { throw new Exception($"The assigned script '{nameof(buttonsUI)} does not implement the {nameof(IBalanceIntent)} interface"); }
        }

        private void Start() {
            cts = new CancellationTokenSource();
            balanceFlow = new BalanceFlow(BalanceIntent, BalanceService, CategoryService, cts.Token);
        }

        private void OnDestroy() {
            balanceFlow.Dispose();
            cts.Cancel();
            cts.Dispose();
        }
    }
}
