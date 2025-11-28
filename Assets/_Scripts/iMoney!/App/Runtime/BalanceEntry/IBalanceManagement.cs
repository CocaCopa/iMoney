using System;

namespace iMoney.App.BalanceEntry.Runtime {
    internal interface IBalanceManagement {
        event Action OnAddPressed;
        event Action OnSpendPressed;
        void HideAddButton();
        void HideSpendButton();
        void ShowAddButton();
        void ShowSpendButton();
        void SetNewBalance(string amount);
        string GetBalanceText();
    }
}
