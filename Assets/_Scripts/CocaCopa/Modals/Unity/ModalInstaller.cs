using System;
using System.Threading;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Runtime;
using CocaCopa.Modal.SPI;
using UnityEngine;

namespace CocaCopa.Modal.Unity {
    internal class ModalInstaller : MonoBehaviour {
        [Header("References")]
        [SerializeField] private MonoBehaviour modalAnimator;
        [SerializeField] private MonoBehaviour modalView;
        [SerializeField] private MonoBehaviour virtualKeyboard;

        [Header("Caret")]
        [SerializeField] private Color caretColor = Color.white;
        [SerializeField] private CaretInterval caretInterval;

        private ModalFlow modalFlow;
        private IModalAnimator ModalAnimator => (IModalAnimator)modalAnimator;
        private IModalView ModalView => (IModalView)modalView;
        private IVirtualKeyboard VirtualKeyboard => (IVirtualKeyboard)virtualKeyboard;
        private CancellationTokenSource lifetimeCts;

        internal IModalService ModalService => modalFlow;

        public bool IsActive { get; private set; }

        private void OnValidate() {
            if (modalAnimator == null) { throw new Exception($"[{nameof(ModalInstaller)}] {nameof(modalAnimator)} not serialized"); }
            if (modalAnimator is not IModalAnimator) { throw new Exception($"[{nameof(ModalInstaller)}] The {nameof(modalAnimator)} assigned does not implement the '{nameof(IModalAnimator)}' interface"); }

            if (modalView == null) { throw new Exception($"[{nameof(ModalInstaller)}] {nameof(modalView)} not serialized"); }
            if (modalView is not IModalView) { throw new Exception($"[{nameof(ModalInstaller)}] The {nameof(modalView)} assigned does not implement the '{nameof(IModalView)}' interface"); }

            if (virtualKeyboard == null) { throw new Exception($"[{nameof(ModalInstaller)}] {nameof(virtualKeyboard)} not serialized"); }
            if (virtualKeyboard is not IVirtualKeyboard) { throw new Exception($"[{nameof(ModalInstaller)}] The MonoBehaviour assigned does not implement the '{nameof(IVirtualKeyboard)}' interface"); }
        }

        private void Awake() {
            lifetimeCts = new CancellationTokenSource();
            var caretOptions = new ModalFlow.CaretOptions(ColorUtility.ToHtmlStringRGBA(caretColor), caretInterval.onDuration, caretInterval.offDuration);
            modalFlow = new ModalFlow(ModalAnimator, ModalView, VirtualKeyboard, caretOptions, lifetimeCts.Token);
        }

        private void Update() {
            if (modalFlow.IsActive) modalFlow.TickCaret(Time.unscaledDeltaTime);
        }

        private void OnDestroy() {
            lifetimeCts.Cancel();
            lifetimeCts.Dispose();
        }

        #region Class Data
        [Serializable]
        private struct CaretInterval {
            public float onDuration;
            public float offDuration;
        };
        #endregion
    }
}
