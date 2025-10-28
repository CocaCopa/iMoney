using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Extensions.Core;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CocaCopa.Modal.UI {
    public class ModalUI : MonoBehaviour, IModalService {
        [Header("References")]
        [SerializeField] private VirtualNumpad vNumpad;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        [Header("Caret")]
        [SerializeField] private Color caretColor = Color.white;
        [SerializeField] private CaretInterval caretInterval;

        private ModalAnimationUI modalAnim;
        private VirtualCaret vCaret;
        private ModalValue inputValue;
        private string normalTxt;
        private string colorizedTxt;

        private CaretState caretState;
        private float caretTimer;
        private bool caretIsOn;

        private TaskCompletionSource<ModalResult> tcs;
        private CancellationTokenRegistration ctr;
        public bool IsActive { get; private set; }

        internal event Action OnConfirmIntent;
        internal event Action OnCancelIntent;

        private void Awake() {
            modalAnim = GetComponentInParent<ModalAnimationUI>();
            if (modalAnim == null) { throw new Exception("ModalAnimationUI not found in parent"); }
            inputField.DeactivateInputField();
            inputField.richText = true;
            vCaret = VirtualCaret.NumpadCaret(ColorUtility.ToHtmlStringRGBA(caretColor));
            confirmButton.interactable = false;
            vNumpad.OnVirtualStringChanged += Numpad_OnStringChanged;

            confirmButton.onClick.AddListener(OnConfirmClicked);
            cancelButton.onClick.AddListener(OnCancelClicked);
        }

        private void Update() {
            ManageCaret();
        }

        private void Numpad_OnStringChanged(VirtualNumpad.InputFieldInfo data) {
            inputValue = new ModalValue(data.IntValue, data.DecimalCount);
            confirmButton.interactable = data.Text != string.Empty && data.IntValue != 0;
            string IFtext = data.Text.Length > 0 ? $"{data.Text}€" : string.Empty;
            normalTxt = IFtext;
            colorizedTxt = vCaret.ApplyCaret(normalTxt, data.CaretIndex);

            inputField.text = colorizedTxt;
            caretIsOn = false;
            caretState = CaretState.ValidateState;
        }

        public Task<ModalResult> ShowAsync(ModalOptions options, CancellationToken ct) {
            if (IsActive) { throw new InvalidOperationException("Modal already active"); }
            IsActive = true;
            tcs = new TaskCompletionSource<ModalResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            modalAnim.SetActive(true, options.appearFrom);

            if (ct.CanBeCanceled) {
                ctr = ct.Register(() => { Complete(ModalResult.Cancel()); });
            }
            else throw new Exception("Cancellation token was not provided");

            return tcs.Task;
        }

        public void Hide() {
            if (IsActive) { throw new Exception("Cannot hide modal before result"); }
            modalAnim.SetActive(false);
        }

        private void OnConfirmClicked() {
            if (!IsActive) { return; }
            OnConfirmIntent?.SafeInvoke(nameof(OnConfirmIntent));
            Complete(ModalResult.Confirm(inputValue));
        }

        private void OnCancelClicked() {
            if (!IsActive) { return; }
            OnCancelIntent?.SafeInvoke(nameof(OnCancelIntent));
            Complete(ModalResult.Cancel());
        }

        private void Complete(ModalResult result) {
            IsActive = false;
            ctr.Dispose();
            var tmp_tcs = tcs;
            tcs = null;
            tmp_tcs.TrySetResult(result);
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
        private struct CaretInterval {
            public float onDuration;
            public float offDuration;
        };

        private enum CaretState {
            OnTimer, ValidateState
        };

    }
}
