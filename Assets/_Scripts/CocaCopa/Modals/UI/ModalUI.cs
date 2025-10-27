using System;
using CocaCopa.Modal.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CocaCopa.Modal.UI {
    public class ModalUI : MonoBehaviour {
        [SerializeField] private VirtualNumpad vNumpad;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private void Awake() {
            inputField.DeactivateInputField();
            inputField.richText = true;
            vNumpad.OnVirtualStringChanged += Numpad_OnStringChanged;
        }

        private void Numpad_OnStringChanged(NumpadData data) {
            string currencySymbol = data.VirtualString.Length > 0 ? "€" : string.Empty;
            inputField.text = $"{data.VirtualString}{currencySymbol}";
        }
    }
}
