using System;
using System.Threading;
using CocaCopa.Modal.Contracts;
using iMoney.BalanceEntry.Runtime.UI;
using UnityEngine;

namespace iMoney.BalanceEntry.Runtime {
    public class BalanceFlowController : MonoBehaviour {
        [SerializeField] private BalanceButtonsUI buttonsUI;
        [SerializeField] private MonoBehaviour modalService;

        private IModalService ModalService => (IModalService)modalService;

        private BalanceFlow balanceFlow;
        private CancellationTokenSource cts;

        private void Awake() {
            cts = new CancellationTokenSource();
            balanceFlow = new BalanceFlow(buttonsUI, ModalService, cts.Token);
        }

        private void OnDestroy() {
            balanceFlow.Dispose();
            cts.Cancel();
            cts.Dispose();
        }
    }
}
