using System;
using CocaCopa.Core.Extensions;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Runtime.Animation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CocaCopa.Modal.Runtime.UI {
    internal class ModalUI : MonoBehaviour {
        [Header("References")]
        [SerializeField] private ModalAnimation modalAnim;
        [SerializeField] private VirtualNumpad vNumpad;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        internal event Action OnConfirmIntent;
        internal event Action OnCancelIntent;

        private void Awake() {
            inputField.DeactivateInputField();
            inputField.richText = true;
            confirmButton.interactable = false;

            confirmButton.onClick.AddListener(OnConfirmClicked);
            cancelButton.onClick.AddListener(OnCancelClicked);
        }

        internal void SetInputFieldStr(string txt, bool allowConfirm = true) {
            confirmButton.interactable = allowConfirm;
            inputField.text = txt;
        }

        private void OnConfirmClicked() {
            OnConfirmIntent?.SafeInvoke(nameof(OnConfirmIntent));
        }

        private void OnCancelClicked() {
            OnCancelIntent?.SafeInvoke(nameof(OnCancelIntent));
        }

        internal void ShowModal(Appear appearFrom) => modalAnim.SetActive(true, appearFrom);
        internal void HideModal() => modalAnim.SetActive(false);
    }
}
