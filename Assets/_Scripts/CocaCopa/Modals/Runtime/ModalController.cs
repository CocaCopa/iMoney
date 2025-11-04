using System;
using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Runtime.Animation;
using CocaCopa.Modal.Runtime.Internal;
using CocaCopa.Modal.Runtime.UI;
using UnityEngine;

namespace CocaCopa.Modal.Runtime {
    internal class ModalController : MonoBehaviour, IModalService {
        [Header("References")]
        [SerializeField] private ModalAnimation modalAnim;
        [SerializeField] private ModalUI modalUI;
        [SerializeField] private VirtualKeyboardBase vkBase;

        [Header("Caret")]
        [SerializeField] private Color caretColor = Color.white;
        [SerializeField] private CaretInterval caretInterval;

        private ModalFlow modalFlow;

        private TaskCompletionSource<ModalResult> tcs;
        private CancellationTokenRegistration ctr;

        public bool IsActive { get; private set; }
        public bool IsAnimating => modalAnim.IsAnimating;

        private void Awake() {
            KeyboardType kt = vkBase is VirtualNumpad ? KeyboardType.Numpad : KeyboardType.QWERTY;
            modalFlow = new ModalFlow(kt, ColorUtility.ToHtmlStringRGBA(caretColor), caretInterval.onDuration, caretInterval.offDuration);
            CheckComponents();
        }

        private void Start() {
            SubscribeToEvents();
            modalAnim.Init_Start();
        }

        private void Update() {
            CaretManagement();
        }

        private void OnDestroy() {
            vkBase.OnVirtualKeyPressed -= VK_OnKeyPressed;
            modalUI.OnConfirmIntent -= ModalUI_OnConfirmIntent;
            modalUI.OnCancelIntent -= ModalUI_OnCancelIntent;

            if (IsActive && tcs != null) {
                Complete(ModalResult.Cancel());
            }
        }

        private void CheckComponents() {
            if (modalAnim == null) { throw new Exception("[ModalController] ModalAnimation not serialized"); }
            if (modalUI == null) { throw new Exception("[ModalController] ModalUI not serialized"); }
            if (vkBase == null) { throw new Exception("[ModalController] VirtualNumpad not serialized"); }
        }

        private void SubscribeToEvents() {
            vkBase.OnVirtualKeyPressed += VK_OnKeyPressed;
            modalUI.OnConfirmIntent += ModalUI_OnConfirmIntent;
            modalUI.OnCancelIntent += ModalUI_OnCancelIntent;
        }

        private void CaretManagement() {
            var caretResult = modalFlow.TickCaret(Time.unscaledDeltaTime);

            if (caretResult.updateText) {
                modalUI.SetInputFieldStr(caretResult.text);
            }
        }

        private void VK_OnKeyPressed(Enum input) {
            var stateResult = modalFlow.OnVirtualKeyPressed(input);
            if (stateResult.isValid) {
                modalUI.SetInputFieldStr(stateResult.displayedText);
            }
        }

        private void ModalUI_OnConfirmIntent() {
            if (!IsActive) { return; }
            Complete(ModalResult.Confirm(modalFlow.CurrentValue));
        }

        private void ModalUI_OnCancelIntent() {
            if (!IsActive) { return; }
            Complete(ModalResult.Cancel());
        }

        public Task<ModalResult> ShowAsync(ModalOptions options, CancellationToken ct) {
            if (IsActive) { throw new InvalidOperationException("Modal already active"); }
            if (options.cachedInputValue == CachedInputValue.Erase) {
                modalFlow.ResetInput();
                modalUI.SetInputFieldStr(string.Empty);
            }
            IsActive = true;
            tcs = new TaskCompletionSource<ModalResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            modalAnim.SetActive(true, options.inputAnimOpt, options.vkAnimOpt);

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
    }
    #endregion
}
