using System;
using CocaCopa.Extensions.Core;
using CocaCopa.Modal.Runtime.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace CocaCopa.Modal.Runtime.UI {
    internal class VirtualNumpad : MonoBehaviour {
        [Header("Digit Rows")]
        [SerializeField] private Button numpad1;
        [SerializeField] private Button numpad2;
        [SerializeField] private Button numpad3;
        [SerializeField] private Button numpad4;
        [SerializeField] private Button numpad5;
        [SerializeField] private Button numpad6;
        [SerializeField] private Button numpad7;
        [SerializeField] private Button numpad8;
        [SerializeField] private Button numpad9;

        [Header("Side Buttons")]
        [SerializeField] private Button numpadBackspace;
        [SerializeField] private Button numpadDot;
        [SerializeField] private Button numpad0;

        internal event Action<NumpadInput> OnVirtualKeyPressed;

        private void Awake() {
            AddListeners();
        }

        private void AddListeners() {
            numpad1.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit1));
            numpad2.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit2));
            numpad3.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit3));
            numpad4.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit4));
            numpad5.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit5));
            numpad6.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit6));
            numpad7.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit7));
            numpad8.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit8));
            numpad9.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit9));
            numpad0.onClick.AddListener(() => RaiseEvent(NumpadInput.Digit0));
            numpadDot.onClick.AddListener(() => RaiseEvent(NumpadInput.DecimalPoint));
            numpadBackspace.onClick.AddListener(() => RaiseEvent(NumpadInput.Backspace));
        }

        private void RaiseEvent(NumpadInput input) {
            OnVirtualKeyPressed?.SafeInvoke(input, nameof(OnVirtualKeyPressed));
        }
    }
}
