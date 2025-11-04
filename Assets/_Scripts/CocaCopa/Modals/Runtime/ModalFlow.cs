using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Runtime.Domain;
using CocaCopa.Modal.Runtime.Internal;
using UnityEngine;

namespace CocaCopa.Modal.Runtime {
    internal class ModalFlow {
        private readonly VKStringConstructor strCtor;
        private readonly VirtualCaret vCaret;
        private readonly float caretOnDuration;
        private readonly float caretOffDuration;

        private string inputValue;
        private string normalTxt;
        private string colorizedTxt;

        private CaretState caretState;
        private float caretTimer;
        private bool caretIsOn;

        internal string CurrentValue => inputValue;

        internal ModalFlow(KeyboardType keyboardType, string htmlStrRGBA, float caretOnDuration, float caretOffDuration) {
            strCtor = new VKStringConstructor(keyboardType);
            vCaret = VirtualCaret.NumpadCaret(keyboardType, htmlStrRGBA);

            this.caretOnDuration = caretOnDuration;
            this.caretOffDuration = caretOffDuration;
            caretState = CaretState.OnTimer;
            caretIsOn = false;
            normalTxt = string.Empty;
            colorizedTxt = string.Empty;
            inputValue = string.Empty;
        }

        internal void ResetInput() {
            strCtor.ResetStr();
            normalTxt = string.Empty;
            colorizedTxt = string.Empty;
        }

        internal FlowStateResult OnVirtualKeyPressed(System.Enum input) {
            inputValue = strCtor.Apply(input);
            string IFtext = inputValue.Length > 0 ? $"{inputValue}€" : string.Empty;
            normalTxt = IFtext;
            colorizedTxt = vCaret.ApplyCaret(normalTxt, strCtor.CaretIndex);

            bool validInput = inputValue != string.Empty;

            caretIsOn = false;
            caretState = CaretState.ValidateState;

            return new FlowStateResult(colorizedTxt, validInput);
        }

        internal CaretUpdateResult TickCaret(float deltaTime) {
            switch (caretState) {
                case CaretState.OnTimer:
                    caretTimer -= deltaTime;
                    caretTimer = Mathf.Max(0f, caretTimer);
                    if (caretTimer == 0f) {
                        caretState = CaretState.ValidateState;
                    }
                    break;

                case CaretState.ValidateState:
                    string textToShow = caretIsOn ? normalTxt : colorizedTxt;
                    float duration = caretIsOn ? caretOnDuration : caretOffDuration;

                    caretTimer = duration;
                    caretIsOn = !caretIsOn;
                    caretState = CaretState.OnTimer;

                    return new CaretUpdateResult(updateText: true, text: textToShow);
            }

            return new CaretUpdateResult(updateText: false, text: string.Empty);
        }

        #region Class Data
        private enum CaretState {
            OnTimer, ValidateState
        };

        internal readonly struct CaretUpdateResult {
            public readonly bool updateText;
            public readonly string text;
            public CaretUpdateResult(bool updateText, string text) {
                this.updateText = updateText;
                this.text = text;
            }
        }

        internal readonly struct FlowStateResult {
            public readonly string displayedText;
            public readonly bool isValid;

            public FlowStateResult(string displayedText, bool isValid) {
                this.displayedText = displayedText;
                this.isValid = isValid;
            }
        }
    }
    #endregion
}
