using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Runtime.Animation;
using CocaCopa.Modal.Runtime.Domain;
using CocaCopa.Modal.Runtime.UI;
using UnityEngine;

namespace CocaCopa.Modal.Runtime {
    internal class ModalController : MonoBehaviour, IModalService {
        [Header("Caret")]
        [SerializeField] private Color caretColor = Color.white;
        [SerializeField] private CaretInterval caretInterval;

        private ModalAnimation modalAnim;
        private ModalUI modalUI;
        private VirtualNumpad vNumpad;

        private VirtualCaret vCaret;
        private ModalValue inputValue;
        private string normalTxt;
        private string colorizedTxt;

        private CaretState caretState;
        private float caretTimer;
        private bool caretIsOn;

        private TaskCompletionSource<ModalResult> tcs;
        private CancellationTokenRegistration ctr;

        public bool IsActive {
            get; private set;
        }

        private void Awake() {
            vCaret = VirtualCaret.NumpadCaret(ColorUtility.ToHtmlStringRGBA(caretColor));
            CacheComponents();
        }

        private void CacheComponents() {
            modalAnim = GetComponentInChildren<ModalAnimation>();
            modalUI = GetComponentInChildren<ModalUI>();
            vNumpad = GetComponentInChildren<VirtualNumpad>();

            if (modalAnim == null) { throw new Exception("[ModalController] ModalAnimationUI not found in children"); }
            if (modalUI == null) { throw new Exception("[ModalController] ModalUI not found in children"); }
            if (vNumpad == null) { throw new Exception("[ModalController] VirtualNumpad not found in children"); }
        }

        private void Start() {
            SubscribeToEvents();
        }

        private void Update() {
            ManageCaret();
        }

        private void SubscribeToEvents() {
            vNumpad.OnVirtualStringChanged += Numpad_OnStringChanged;
            modalUI.OnConfirmIntent += ModalUI_OnConfirmIntent;
            modalUI.OnCancelIntent += ModalUI_OnCancelIntent;
        }

        private void Numpad_OnStringChanged(VirtualNumpad.InputFieldInfo info) {
            inputValue = new ModalValue(info.IntValue, info.DecimalCount);
            string IFtext = info.Text.Length > 0 ? $"{info.Text}€" : string.Empty;
            normalTxt = IFtext;
            colorizedTxt = vCaret.ApplyCaret(normalTxt, info.CaretIndex);

            bool validInput = info.Text != string.Empty && info.IntValue != 0;
            modalUI.SetInputFieldStr(colorizedTxt, validInput);

            caretIsOn = false;
            caretState = CaretState.ValidateState;
        }

        private void ModalUI_OnConfirmIntent() {
            if (!IsActive) { return; }
            Complete(ModalResult.Confirm(inputValue));
        }

        private void ModalUI_OnCancelIntent() {
            if (!IsActive) { return; }
            Complete(ModalResult.Cancel());
        }

        private void ManageCaret() {
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
                        modalUI.SetInputFieldStr(normalTxt);
                    }
                    else {
                        caretTimer = caretInterval.offDuration;
                        modalUI.SetInputFieldStr(colorizedTxt);
                    }
                    caretIsOn = !caretIsOn;
                    caretState = CaretState.OnTimer;
                    break;
            }
        }

        public Task<ModalResult> ShowAsync(ModalOptions options, CancellationToken ct) {
            if (IsActive) { throw new InvalidOperationException("Modal already active"); }
            if (options.cachedInputValue == CachedInputValue.Erase) {
                normalTxt = colorizedTxt = string.Empty;
                VirtualNumpad.ClearCahcedStr(vNumpad);
                modalUI.SetInputFieldStr(string.Empty);
            }
            IsActive = true;
            tcs = new TaskCompletionSource<ModalResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            modalAnim.SetActive(true, options.appearFrom);

            if (ct.CanBeCanceled) {
                ctr = ct.Register(() => { Complete(ModalResult.Cancel()); });
            }
            else throw new Exception("Cancellation token was not provided");

            return tcs.Task;
        }

        private void Complete(ModalResult result) {
            IsActive = false;
            ctr.Dispose();
            var tmp_tcs = tcs;
            tcs = null;
            tmp_tcs.TrySetResult(result);
        }

        public void Hide() {
            if (IsActive) { throw new Exception("Cannot hide modal before result"); }
            modalAnim.SetActive(false);
        }

        #region Class Data
        [Serializable]
        private struct CaretInterval {
            public float onDuration;
            public float offDuration;
        };

        private enum CaretState {
            OnTimer, ValidateState
        };
    }
    #endregion
}
