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

        private ModalValue inputValue;
        private string normalTxt;
        private string colorizedTxt;

        private CaretState caretState;
        private float caretTimer;
        private bool caretIsOn;

        internal ModalValue CurrentValue => inputValue;

        internal ModalFlow(string htmlStrRGBA, float caretOnDuration, float caretOffDuration) {
            strCtor = new VKStringConstructor();
            vCaret = VirtualCaret.NumpadCaret(htmlStrRGBA);

            this.caretOnDuration = caretOnDuration;
            this.caretOffDuration = caretOffDuration;
            caretState = CaretState.OnTimer;
            caretIsOn = false;
            normalTxt = string.Empty;
            colorizedTxt = string.Empty;
            inputValue = new ModalValue(0, 0);
        }

        internal void ResetInput() {
            strCtor.ResetStr();
            normalTxt = string.Empty;
            colorizedTxt = string.Empty;
        }

        internal FlowStateResult OnVirtualKeyPressed(NumpadInput input) {
            var data = strCtor.Apply(input);

            inputValue = new ModalValue(data.virtualValue, data.DecimalCount);
            string IFtext = data.virtualString.Length > 0 ? $"{data.virtualString}€" : string.Empty;
            normalTxt = IFtext;
            colorizedTxt = vCaret.ApplyCaret(normalTxt, strCtor.CaretIndex);

            bool validInput = data.virtualString != string.Empty && data.virtualValue != 0;

            caretIsOn = false;
            caretState = CaretState.ValidateState;

            return new FlowStateResult(colorizedTxt, validInput, inputValue);
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
            public readonly ModalValue currentValue;

            public FlowStateResult(string displayedText, bool isValid, ModalValue currentValue) {
                this.displayedText = displayedText;
                this.isValid = isValid;
                this.currentValue = currentValue;
            }
        }
    }
    #endregion
}
