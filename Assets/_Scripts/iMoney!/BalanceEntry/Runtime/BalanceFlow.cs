using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Modal.Contracts;
using iMoney.BalanceEntry.SPI;

namespace iMoney.BalanceEntry.Runtime {
    internal sealed class BalanceFlow : IDisposable {
        private readonly IBalanceIntent balanceIntent;
        private readonly IModalService balanceModal;
        private readonly IModalService categoryModal;
        private readonly CancellationToken ct;

        private readonly ModalOptions balanceAddOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Left, AnimOptions.Bottom);
        private readonly ModalOptions balanceSpendOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Right, AnimOptions.Bottom);
        private readonly ModalOptions categoryAddOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Right, AnimOptions.Bottom);

        internal BalanceFlow(IBalanceIntent balanceIntent, IModalService balanceModal, IModalService categoryModal, CancellationToken ct) {
            this.balanceIntent = balanceIntent;
            this.balanceModal = balanceModal;
            this.categoryModal = categoryModal;
            this.ct = ct;

            this.balanceIntent.OnAddPressed += HandleAddIntent;
            this.balanceIntent.OnSpendPressed += HandleSpendIntent;
        }

        public void Dispose() {
            balanceIntent.OnAddPressed -= HandleAddIntent;
            balanceIntent.OnSpendPressed -= HandleSpendIntent;
        }

        private void HandleAddIntent() {
            if (balanceModal.IsActive /* || modalService.IsAnimating */) { return; }
            balanceIntent.HideSpendButton();
            _ = HandleButtonPress(balanceAddOptions, HandleButton.Add);
        }

        private void HandleSpendIntent() {
            if (balanceModal.IsActive /* || modalService.IsAnimating */) { return; }
            balanceIntent.HideAddButton();
            _ = HandleButtonPress(balanceSpendOptions, HandleButton.Spend);
        }

        private async Task HandleButtonPress(ModalOptions options, HandleButton btn) {
            try {
                ModalResult modalResult = await balanceModal.ShowAsync(options, ct);

                if (modalResult.Confirmed) {
                    _ = balanceModal.Hide();
                    await Task.Delay(310);
                    await categoryModal.ShowAsync(categoryAddOptions, ct);
                    return;
                }
                else {

                }

                ct.ThrowIfCancellationRequested();
                await balanceModal.Hide();
            }
            catch { return; }

            if (btn == HandleButton.Add) { balanceIntent.ShowSpendButton(); }
            else if (btn == HandleButton.Spend) { balanceIntent.ShowAddButton(); }
        }

        private enum HandleButton { Add, Spend };
        private struct ModalData {
            public string balanceAmount;
            public string categoryName;
            public ModalData(string balanceAmount, string categoryName) {
                this.balanceAmount = balanceAmount;
                this.categoryName = categoryName;
            }
        };
    }
}
