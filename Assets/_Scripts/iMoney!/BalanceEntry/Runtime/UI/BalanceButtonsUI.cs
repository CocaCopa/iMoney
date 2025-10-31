using System;
using CocaCopa.Core.Extensions;
using iMoney.BalanceEntry.Runtime.Animation;
using UnityEngine;
using UnityEngine.UI;

namespace iMoney.BalanceEntry.Runtime.UI {
    public class BalanceButtonsUI : MonoBehaviour {
        [SerializeField] private Button addBtn;
        [SerializeField] private Button spendBtn;
        [Space(10)]
        [SerializeField] private BalanceButtonsAnimation buttonsAnim;

        internal event Action OnAddPressed;
        internal event Action OnSpendPressed;

        private void Awake() {
            AddListeners();
        }

        private void OnDestroy() {
            addBtn.onClick.RemoveAllListeners();
            spendBtn.onClick.RemoveAllListeners();
        }

        private void AddListeners() {
            addBtn.onClick.AddListener(() => OnAddPressed?.SafeInvoke(nameof(OnAddPressed)));
            spendBtn.onClick.AddListener(() => OnSpendPressed?.SafeInvoke(nameof(OnSpendPressed)));
        }

        internal void HideAddButton() => buttonsAnim.FadeAddMask(FadeMode.In);
        internal void ShowAddButton() => buttonsAnim.FadeAddMask(FadeMode.Out);
        internal void HideSpendButton() => buttonsAnim.FadeSpendMask(FadeMode.In);
        internal void ShowSpendButton() => buttonsAnim.FadeSpendMask(FadeMode.Out);
    }
}
