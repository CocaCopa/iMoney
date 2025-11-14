using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Logger.API;
using CocaCopa.Modal.Contracts;
using iMoney.BalanceEntry.SPI;

namespace iMoney.BalanceEntry.Runtime {
    internal sealed class BalanceFlow : IDisposable {
        private readonly IBalanceManagement balanceManagement;
        private readonly IModalService balanceModal;
        private readonly IModalService categoryModal;
        private readonly CancellationToken ct;

        private readonly ModalOptions balanceAddOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Left, AnimOptions.Bottom);
        private readonly ModalOptions balanceSpendOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Right, AnimOptions.Bottom);
        private readonly ModalOptions categoryAddOptions = new ModalOptions(CachedInputValue.Erase, AnimOptions.Right, AnimOptions.Bottom);

        internal BalanceFlow(IBalanceManagement balanceManagement, IModalService balanceModal, IModalService categoryModal, CancellationToken ct) {
            this.balanceManagement = balanceManagement;
            this.balanceModal = balanceModal;
            this.categoryModal = categoryModal;
            this.ct = ct;

            this.balanceManagement.OnAddPressed += HandleAddIntent;
            this.balanceManagement.OnSpendPressed += HandleSpendIntent;
        }

        public void Dispose() {
            balanceManagement.OnAddPressed -= HandleAddIntent;
            balanceManagement.OnSpendPressed -= HandleSpendIntent;
        }

        private async void HandleAddIntent() {
            if (balanceModal.IsActive) { return; }
            balanceManagement.HideSpendButton();
            ModalData data = await GetModalData();
            Log.Info($"Add | {data.categoryName}: {data.balanceAmount}");
            balanceManagement.ShowSpendButton();
        }

        private async void HandleSpendIntent() {
            if (balanceModal.IsActive) { return; }
            balanceManagement.HideAddButton();
            ModalData data = await GetModalData();
            Log.Info($"Spend | {data.categoryName}: {data.balanceAmount}");
            balanceManagement.ShowAddButton();
        }

        private async Task<ModalData> GetModalData() {
            string balance = await ResolveModal(balanceModal, balanceAddOptions, awaitHide: false);
            if (balance.Equals(string.Empty)) { return ModalData.Invalid; }
            await Task.Delay(280);
            string category = await ResolveModal(categoryModal, categoryAddOptions, awaitHide: true);
            if (category.Equals(string.Empty)) { return ModalData.Invalid; }

            return ModalData.CreateValid(balance, category);
        }

        private async Task<string> ResolveModal(IModalService service, ModalOptions options, bool awaitHide) {
            string modalText = string.Empty;
            try {
                ModalResult modalResult = await service.ShowAsync(options, ct);

                if (modalResult.Confirmed) { modalText = modalResult.Text; }

                ct.ThrowIfCancellationRequested();
                if (awaitHide) { await service.Hide(); }
                else { _ = service.Hide(); }
            }
            catch { return string.Empty; }

            return modalText;
        }

        private readonly struct ModalData {
            public readonly bool isValid;
            public readonly string balanceAmount;
            public readonly string categoryName;
            public static ModalData Invalid => new ModalData(false, string.Empty, string.Empty);
            public static ModalData CreateValid(string balanceAmount, string categoryName) => new ModalData(true, balanceAmount, categoryName);
            private ModalData(bool isValid, string balanceAmount, string categoryName) {
                this.isValid = isValid;
                this.balanceAmount = balanceAmount;
                this.categoryName = categoryName;
            }
        };
    }
}
