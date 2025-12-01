using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Core.Numerics;
using CocaCopa.Logger.API;
using CocaCopa.Modal.Contracts;
using iMoney.Transactions.API;
using iMoney.Transactions.Contracts;

namespace iMoney.App.BalanceEntry.Runtime {
    internal sealed class BalanceFlow : IDisposable {
        private readonly IBalanceManagement balanceManagement;
        private readonly CancellationToken ct;
        private readonly Config balanceConfig;
        private readonly Config categoryConfig;

        internal BalanceFlow(IBalanceManagement balanceManagement, Config balanceConfig, Config categoryConfig, CancellationToken ct) {
            this.balanceManagement = balanceManagement;
            this.balanceConfig = balanceConfig;
            this.categoryConfig = categoryConfig;
            this.ct = ct;

            this.balanceManagement.OnAddPressed += HandleAddIntent;
            this.balanceManagement.OnSpendPressed += HandleSpendIntent;
        }

        public void Dispose() {
            balanceManagement.OnAddPressed -= HandleAddIntent;
            balanceManagement.OnSpendPressed -= HandleSpendIntent;
        }

        private void HandleAddIntent() => _ = HandleIntentAsync(isAdd: true);
        private void HandleSpendIntent() => _ = HandleIntentAsync(isAdd: false);

        private async Task HandleIntentAsync(bool isAdd) {
            if (balanceConfig.ModalService.IsActive || categoryConfig.ModalService.IsActive) {
                return;
            }

            ToggleButtons(isAdd, hide: true);
            try {
                ModalData data = await GetModalData(addOptions: isAdd);
                if (!data.IsValid) return;

                string type = isAdd ? "Add" : "Spend";
                Transaction tr = CreateTransaction(data.BalanceAmount, type, data.CategoryName);

                if (tr.Equals(Transaction.Default())) {
                    // TODO: Notification + fallback
                    Log.Error("[BalanceFlow] Could not create transaction");
                    return;
                }

                TransactionsManager.AddEntry(tr);

                string currentBalance = balanceManagement.GetBalanceText();
                string newBalance = isAdd
                    ? BalanceCalculator.Add(currentBalance, data.BalanceAmount)
                    : BalanceCalculator.Subtract(currentBalance, data.BalanceAmount);

                balanceManagement.SetNewBalance(newBalance);
            }
            finally {
                ToggleButtons(isAdd, hide: false);
            }
        }

        private void ToggleButtons(bool isAdd, bool hide) {
            if (isAdd) {
                if (hide) balanceManagement.HideSpendButton();
                else balanceManagement.ShowSpendButton();
            }
            else {
                if (hide) balanceManagement.HideAddButton();
                else balanceManagement.ShowAddButton();
            }
        }

        private static Transaction CreateTransaction(string amount, string type, string category) {
            if (BalanceCalculator.TryParseAmount(amount, out int value, out int scale)) {
                TransactionAmount trAmount = new TransactionAmount(value, scale);
                Transaction newTr = Transaction.Create(DateTime.Now.Ticks, trAmount, type, category);
                return newTr;
            }
            else return Transaction.Default();
        }

        private async Task<ModalData> GetModalData(bool addOptions) {
            await Task.Delay(balanceConfig.AppearDelay, ct);
            var balanceOptions = addOptions ? balanceConfig.AddOptions : balanceConfig.SpendOptions;
            string balance = await ResolveModal(balanceConfig.ModalService, balanceOptions, awaitHide: false);
            if (balance.Equals(string.Empty)) { return ModalData.Invalid; }
            await Task.Delay(categoryConfig.AppearDelay, ct);
            var categoryOptions = addOptions ? categoryConfig.AddOptions : categoryConfig.SpendOptions;
            string category = await ResolveModal(categoryConfig.ModalService, categoryOptions, awaitHide: true);
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
            public readonly bool IsValid;
            public readonly string BalanceAmount;
            public readonly string CategoryName;
            public static ModalData Invalid => new ModalData(false, string.Empty, string.Empty);
            public static ModalData CreateValid(string balanceAmount, string categoryName) => new ModalData(true, balanceAmount, categoryName);
            private ModalData(bool isValid, string balanceAmount, string categoryName) {
                IsValid = isValid;
                BalanceAmount = balanceAmount;
                CategoryName = categoryName;
            }
        };

        internal readonly struct Config {
            public readonly IModalService ModalService;
            public readonly ModalOptions AddOptions;
            public readonly ModalOptions SpendOptions;
            public readonly int AppearDelay;
            public Config(IModalService modalService, ModalOptions addOptions, ModalOptions spendOptions, int appearDelay = 0) {
                ModalService = modalService;
                AddOptions = addOptions;
                SpendOptions = spendOptions;
                AppearDelay = appearDelay;
            }
        }
    }
}
