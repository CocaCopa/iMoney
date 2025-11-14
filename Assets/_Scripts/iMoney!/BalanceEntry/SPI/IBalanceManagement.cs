using System;

namespace iMoney.BalanceEntry.SPI {
    internal interface IBalanceManagement {
        event Action OnAddPressed;
        event Action OnSpendPressed;
        void HideAddButton();
        void HideSpendButton();
        void ShowAddButton();
        void ShowSpendButton();
        void SetNewBalance(float amount);
    }
}
