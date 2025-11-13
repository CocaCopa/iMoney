using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Core.Math;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Runtime.Domain;
using CocaCopa.Modal.Runtime.Internal;
using CocaCopa.Modal.SPI;

namespace CocaCopa.Modal.Runtime {
    internal class ModalFlow : IModalService {
        private TaskCompletionSource<ModalResult> tcs;
        private CancellationTokenRegistration ctorCtr;
        private CancellationTokenRegistration showCtr;

        private readonly IModalAnimator modalAnimator;
        private readonly IModalView modalView;
        private readonly IVirtualKeyboard vk;
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

        public bool IsActive { get; private set; }

        internal ModalFlow(IModalAnimator modalAnimator, IModalView modalView, IVirtualKeyboard vk, CaretOptions caretOpt, CancellationToken ct) {
            if (ct.CanBeCanceled) ctorCtr = ct.Register(() => { if (tcs != null) Complete(ModalResult.Cancel()); });
            this.modalAnimator = modalAnimator;
            this.modalView = modalView;
            this.vk = vk;
            strCtor = new VKStringConstructor(vk.KeyboardType);
            vCaret = VirtualCaret.NumpadCaret(vk.KeyboardType, caretOpt.htmlStrRGBA);

            caretOnDuration = caretOpt.onDuration;
            caretOffDuration = caretOpt.offDuration;
            caretState = CaretState.OnTimer;
            caretIsOn = false;
            normalTxt = string.Empty;
            colorizedTxt = string.Empty;
            inputValue = string.Empty;
        }

        private void View_OnConfirmIntent() {
            if (!IsActive) { return; }
            Complete(ModalResult.Confirm(CurrentValue));
        }

        private void View_OnCancelIntent() {
            if (!IsActive) { return; }
            Complete(ModalResult.Cancel());
        }

        internal void ResetInput() {
            strCtor.ResetStr();
            modalView.SetInputFieldStr(string.Empty);
            normalTxt = string.Empty;
            colorizedTxt = string.Empty;
        }

        internal void OnVirtualKeyPressed(Enum input) {
            KeyboardState state = strCtor.Apply(input);
            inputValue = state.text;
            vk.EngageShift(state.shiftActive, state.shiftLocked);
            normalTxt = inputValue;
            colorizedTxt = vCaret.ApplyCaret(normalTxt, strCtor.CaretIndex);

            bool validInput = inputValue != string.Empty;

            caretIsOn = false;
            caretState = CaretState.ValidateState;

            if (validInput) {
                modalView.SetInputFieldStr(colorizedTxt);
            }
        }

        internal void TickCaret(float deltaTime) {
            switch (caretState) {
                case CaretState.OnTimer:
                    caretTimer -= deltaTime;
                    caretTimer = CCMath.Max(0f, caretTimer);
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
                    modalView.SetInputFieldStr(textToShow);
                    break;
            }
        }

        public Task<ModalResult> ShowAsync(ModalOptions options, CancellationToken ct = default) {
            if (IsActive) { throw new InvalidOperationException("Modal already active"); }
            modalView.OnConfirmIntent += View_OnConfirmIntent;
            modalView.OnCancelIntent += View_OnCancelIntent;
            vk.OnVirtualKeyPressed += OnVirtualKeyPressed;
            if (options.cachedInputValue == CachedInputValue.Erase) {
                ResetInput();
            }
            IsActive = true;
            tcs = new TaskCompletionSource<ModalResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            _ = modalAnimator.PlayShowAsync(options.inputAnimOpt, options.vkAnimOpt);

            if (ct.CanBeCanceled) {
                showCtr = ct.Register(() => { Complete(ModalResult.Cancel()); });
            }

            return tcs.Task;
        }

        public Task Hide() {
            if (IsActive) { throw new Exception("Cannot hide modal before result"); }
            return modalAnimator.PlayHideAsync();
        }

        private void Complete(ModalResult result) {
            modalView.OnConfirmIntent -= View_OnConfirmIntent;
            modalView.OnCancelIntent -= View_OnCancelIntent;
            vk.OnVirtualKeyPressed -= OnVirtualKeyPressed;
            IsActive = false;
            showCtr.Dispose();
            ctorCtr.Dispose();
            var tmp_tcs = tcs;
            tcs = null;
            tmp_tcs.TrySetResult(result);
        }

        #region Class Data
        private enum CaretState {
            OnTimer, ValidateState
        };

        internal struct CaretOptions {
            public string htmlStrRGBA;
            public float onDuration;
            public float offDuration;
            public CaretOptions(string htmlStrRGBA, float onDuration, float offDuration) {
                this.htmlStrRGBA = htmlStrRGBA;
                this.onDuration = onDuration;
                this.offDuration = offDuration;
            }
        }
        #endregion
    }
}
