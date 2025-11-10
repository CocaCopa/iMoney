using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Modal.Contracts;
using iMoney.BalanceEntry.SPI;

namespace iMoney.BalanceEntry.Runtime {
    internal sealed class BalanceFlow : IDisposable {
        private readonly IBalanceIntent balanceIntent;
        private readonly IModalService modalService;
        private readonly CancellationToken ct;

        private readonly static AnimOptions inputAnimAdd = new AnimOptions(Appear.Left, Disappear.Left);
        private readonly static AnimOptions vkAnimAdd = new AnimOptions(Appear.Bottom, Disappear.Bottom);
        private readonly static AnimOptions inputAnimSpend = new AnimOptions(Appear.Right, Disappear.Left);
        private readonly static AnimOptions vkAnimSpend = new AnimOptions(Appear.Bottom, Disappear.Bottom);
        private readonly ModalOptions addOptions = new ModalOptions(CachedInputValue.Erase, inputAnimAdd, vkAnimAdd);
        private readonly ModalOptions spendOptions = new ModalOptions(CachedInputValue.Erase, inputAnimSpend, vkAnimSpend);

        internal BalanceFlow(IBalanceIntent balanceIntent, IModalService modalService, CancellationToken ct) {
            this.balanceIntent = balanceIntent;
            this.modalService = modalService;
            this.ct = ct;

            this.balanceIntent.OnAddPressed += HandleAddIntent;
            this.balanceIntent.OnSpendPressed += HandleSpendIntent;
        }

        public void Dispose() {
            balanceIntent.OnAddPressed -= HandleAddIntent;
            balanceIntent.OnSpendPressed -= HandleSpendIntent;
        }

        private void HandleAddIntent() {
            if (modalService.IsActive /* || modalService.IsAnimating */) { return; }
            balanceIntent.HideSpendButton();
            _ = HandleButtonPress(addOptions, HandleButton.Add);
        }

        private void HandleSpendIntent() {
            if (modalService.IsActive /* || modalService.IsAnimating */) { return; }
            balanceIntent.HideAddButton();
            _ = HandleButtonPress(spendOptions, HandleButton.Spend);
        }

        private async Task HandleButtonPress(ModalOptions options, HandleButton btn) {
            try {
                ModalResult modalResult = await modalService.ShowAsync(options, ct);

                if (modalResult.Confirmed) {

                }
                else {

                }

                ct.ThrowIfCancellationRequested();
                await modalService.Hide();
            }
            catch { return; }

            if (btn == HandleButton.Add) { balanceIntent.ShowSpendButton(); }
            else if (btn == HandleButton.Spend) { balanceIntent.ShowAddButton(); }
        }

        private enum HandleButton { Add, Spend };
    }
}
