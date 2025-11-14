using System;
using System.Threading;
using CocaCopa.Modal.API;
using CocaCopa.Modal.Contracts;
using iMoney.BalanceEntry.Runtime;
using iMoney.BalanceEntry.SPI;
using UnityEngine;

namespace iMoney.BalanceEntry.Unity {
    internal class BalanceFlowInstaller : MonoBehaviour {
        [SerializeField] private MonoBehaviour balanceManagementUI;
        [SerializeField] private ModalAdapter balanceModal;
        [SerializeField] private ModalAdapter categoryModal;

        private IModalService BalanceService => balanceModal.GetService();
        private IModalService CategoryService => categoryModal.GetService();

        private BalanceFlow balanceFlow;
        private IBalanceManagement BalanceIntent => (IBalanceManagement)balanceManagementUI;
        private CancellationTokenSource cts;

        private void OnValidate() {
            if (balanceManagementUI == null) { throw new ArgumentNullException(nameof(balanceManagementUI)); }
            if (balanceManagementUI is not IBalanceManagement) { throw new Exception($"The assigned script '{nameof(balanceManagementUI)} does not implement the {nameof(IBalanceManagement)} interface"); }
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
