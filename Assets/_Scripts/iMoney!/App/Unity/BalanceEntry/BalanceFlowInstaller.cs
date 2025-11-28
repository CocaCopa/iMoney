using System;
using System.Threading;
using CocaCopa.Modal.API;
using CocaCopa.Modal.Contracts;
using iMoney.App.BalanceEntry.Runtime;
using UnityEngine;

namespace iMoney.App.BalanceEntry.Unity {
    internal class BalanceFlowInstaller : MonoBehaviour {
        [Header("References")]
        [SerializeField] private MonoBehaviour balanceManagementUI;
        [SerializeField] private ModalAdapter balanceModal;
        [SerializeField] private ModalAdapter categoryModal;

        [Header("General")]
        [Tooltip("Delay in milliseconds in which the modal will appear")]
        [SerializeField, Range(0, 500)] private int balanceDelay = 0;
        [Tooltip("Delay in milliseconds in which the modal will appear")]
        [SerializeField, Range(0, 500)] private int categoryDelay = 280;

        [Header("Add Config")]
        [SerializeField] private ModalOptions balanceAddOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Left, AnimOptions.Bottom);
        [SerializeField] private ModalOptions categoryAddOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Right, AnimOptions.Bottom);

        [Header("Spend Config")]
        [SerializeField] private ModalOptions balanceSpendOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Right, AnimOptions.Bottom);
        [SerializeField] private ModalOptions categorySpendOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Right, AnimOptions.Bottom);

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
            var balanceConfig = new BalanceFlow.Config(BalanceService, balanceAddOptions, balanceSpendOptions, balanceDelay);
            var categoryConfig = new BalanceFlow.Config(CategoryService, categoryAddOptions, categorySpendOptions, categoryDelay);
            balanceFlow = new BalanceFlow(BalanceIntent, balanceConfig, categoryConfig, cts.Token);
        }

        private void OnDestroy() {
            balanceFlow.Dispose();
            cts.Cancel();
            cts.Dispose();
        }
    }
}
