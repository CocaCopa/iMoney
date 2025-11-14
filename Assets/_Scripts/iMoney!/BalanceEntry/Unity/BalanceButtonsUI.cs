using System;
using CocaCopa.Core.Events;
using iMoney.BalanceEntry.SPI;
using iMoney.BalanceEntry.Unity.Animation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace iMoney.BalanceEntry.Unity {
    internal class BalanceManagementUI : MonoBehaviour, IBalanceManagement {
        [SerializeField] private TextMeshProUGUI balanceTxt;
        [SerializeField] private Button addBtn;
        [SerializeField] private Button spendBtn;
        [Space(10)]
        [SerializeField] private BalanceButtonsAnimation buttonsAnim;
        [SerializeField] private BalanceScrambleAnimator scrambleAnim;

        public event Action OnAddPressed;
        public event Action OnSpendPressed;

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

        public void HideAddButton() => buttonsAnim.FadeAddMask(FadeMode.In);
        public void ShowAddButton() => buttonsAnim.FadeAddMask(FadeMode.Out);
        public void HideSpendButton() => buttonsAnim.FadeSpendMask(FadeMode.In);
        public void ShowSpendButton() => buttonsAnim.FadeSpendMask(FadeMode.Out);
        public void SetNewBalance(float amount) {
            StartCoroutine(scrambleAnim.ScrambleText(4, () => {
                balanceTxt.SetText($"{amount}€");
            }));
        }


    }
}
