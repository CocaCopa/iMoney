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

        private readonly static AnimOptions inputAnimAdd = new AnimOptions(Appear.Left, Disappear.Left);
        private readonly static AnimOptions vkAnimAdd = new AnimOptions(Appear.Bottom, Disappear.Bottom);
        private readonly static AnimOptions inputAnimSpend = new AnimOptions(Appear.Right, Disappear.Left);
        private readonly static AnimOptions vkAnimSpend = new AnimOptions(Appear.Bottom, Disappear.Bottom);
        private readonly ModalOptions addOptions = new ModalOptions(CachedInputValue.Erase, inputAnimAdd, vkAnimAdd);
        private readonly ModalOptions spendOptions = new ModalOptions(CachedInputValue.Erase, inputAnimSpend, vkAnimSpend);

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
            if (modalService.IsActive || modalService.IsAnimating) { return; }
            buttonsUI.HideSpendButton();
            _ = HandleButtonPress(addOptions, HandleButton.Add);
        }

        private void HandleSpendIntent() {
            if (modalService.IsActive || modalService.IsAnimating) { return; }
            buttonsUI.HideAddButton();
            _ = HandleButtonPress(spendOptions, HandleButton.Spend);
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
