using System;
using CocaCopa.Logger;
using CocaCopa.Modal.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CocaCopa.Modal.UI {
    public class ModalUI : MonoBehaviour {
        [Header("References")]
        [SerializeField] private VirtualNumpad vNumpad;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        [Header("Caret")]
        [SerializeField] private Color caretColor = Color.white;
        [SerializeField] private CaretInterval caretInterval;

        private VirtualCaret vCaret;
        private string normalTxt;
        private string colorizedTxt;

        private CaretState caretState;
        private float caretTimer;
        private bool caretIsOn;

        private void Awake() {
            inputField.DeactivateInputField();
            inputField.richText = true;
            vCaret = VirtualCaret.NumpadCaret(ColorUtility.ToHtmlStringRGBA(caretColor));
            vNumpad.OnVirtualStringChanged += Numpad_OnStringChanged;
        }

        private void Numpad_OnStringChanged(VirtualNumpad.InputFieldParams data) {
            string IFtext = data.Text.Length > 0 ? $"{data.Text}€" : string.Empty;
            normalTxt = IFtext;
            colorizedTxt = vCaret.ApplyCaret(normalTxt, data.CaretIndex);

            inputField.text = colorizedTxt;
            caretIsOn = false;
            caretState = CaretState.ValidateState;
        }

        private void Update() {
            switch (caretState) {
                case CaretState.OnTimer:
                    caretTimer -= Time.unscaledDeltaTime;
                    caretTimer = Mathf.Max(0f, caretTimer);
                    if (caretTimer == 0f) {
                        caretState = CaretState.ValidateState;
                    }
                    break;

                case CaretState.ValidateState:
                    if (caretIsOn) {
                        caretTimer = caretInterval.onDuration;
                        inputField.text = normalTxt;
                    }
                    else {
                        caretTimer = caretInterval.offDuration;
                        inputField.text = colorizedTxt;
                    }
                    caretIsOn = !caretIsOn;
                    caretState = CaretState.OnTimer;
                    break;
            }
        }

        [Serializable]
        private class CaretInterval {
            public float onDuration;
            public float offDuration;
        };

        private enum CaretState {
            OnTimer, ValidateState
        };
    }
}
