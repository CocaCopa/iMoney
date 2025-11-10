using System;

namespace iMoney.BalanceEntry.SPI {
    internal interface IBalanceIntent {
        event Action OnAddPressed;
        event Action OnSpendPressed;
        void HideAddButton();
        void HideSpendButton();
        void ShowAddButton();
        void ShowSpendButton();
    }
}
