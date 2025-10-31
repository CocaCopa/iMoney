using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Modal.Contracts;
using iMoney.BalanceEntry.Runtime.UI;

namespace iMoney.BalanceEntry.Runtime {
    internal sealed class BalanceFlow : IDisposable {
        private readonly BalanceButtonsUI buttonsUI;
        private readonly IModalService modalService;
        private readonly CancellationToken ct;

        internal BalanceFlow(BalanceButtonsUI buttonsUI, IModalService modalService, CancellationToken ct) {
            this.buttonsUI = buttonsUI;
            this.modalService = modalService;
            this.ct = ct;

            this.buttonsUI.OnAddPressed += HandleAddIntent;
            this.buttonsUI.OnSpendPressed += HandleSpendIntent;
        }

        public void Dispose() {
            buttonsUI.OnAddPressed -= HandleAddIntent;
            buttonsUI.OnSpendPressed -= HandleSpendIntent;
        }

        private void HandleAddIntent() {
            ModalOptions options = new ModalOptions(AppearFrom.Left, CachedInputValue.Erase);
            buttonsUI.HideSpendButton();
            _ = HandleButtonPress(options, HandleButton.Add);
        }

        private void HandleSpendIntent() {
            ModalOptions options = new ModalOptions(AppearFrom.Right, CachedInputValue.Erase);
            buttonsUI.HideAddButton();
            _ = HandleButtonPress(options, HandleButton.Spend);
        }

        private async Task HandleButtonPress(ModalOptions options, HandleButton btn) {
            ModalResult modalResult = await modalService.ShowAsync(options, ct);

            if (modalResult.Confirmed) {

            }
            else {

            }

            modalService.Hide();
            if (btn == HandleButton.Add) { buttonsUI.ShowSpendButton(); }
            else if (btn == HandleButton.Spend) { buttonsUI.ShowAddButton(); }
        }

        private enum HandleButton { Add, Spend };
    }
}
