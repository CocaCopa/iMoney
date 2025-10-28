using System;
using CocaCopa.Extensions.Core;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace CocaCopa.Modal.UI {
    public class VirtualNumpad : MonoBehaviour {
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

        internal event Action<InputFieldInfo> OnVirtualStringChanged;

        private VKStringConstructor strCtor;

        private void Awake() {
            strCtor = new VKStringConstructor();
            AddListeners();
        }

        private void AddListeners() {
            numpad1.onClick.AddListener(() => AddCharacter(NumpadInput.Digit1));
            numpad2.onClick.AddListener(() => AddCharacter(NumpadInput.Digit2));
            numpad3.onClick.AddListener(() => AddCharacter(NumpadInput.Digit3));
            numpad4.onClick.AddListener(() => AddCharacter(NumpadInput.Digit4));
            numpad5.onClick.AddListener(() => AddCharacter(NumpadInput.Digit5));
            numpad6.onClick.AddListener(() => AddCharacter(NumpadInput.Digit6));
            numpad7.onClick.AddListener(() => AddCharacter(NumpadInput.Digit7));
            numpad8.onClick.AddListener(() => AddCharacter(NumpadInput.Digit8));
            numpad9.onClick.AddListener(() => AddCharacter(NumpadInput.Digit9));
            numpad0.onClick.AddListener(() => AddCharacter(NumpadInput.Digit0));
            numpadDot.onClick.AddListener(() => AddCharacter(NumpadInput.DecimalPoint));
            numpadBackspace.onClick.AddListener(() => AddCharacter(NumpadInput.Backspace));
        }

        private void AddCharacter(NumpadInput input) {
            var data = strCtor.Apply(input);
            var IFParams = new InputFieldInfo(data.VirtualString, data.VirtualValue, data.DecimalCount, strCtor.CaretIndex);
            OnVirtualStringChanged?.SafeInvoke(IFParams, nameof(OnVirtualStringChanged));
        }

        internal class InputFieldInfo {
            private readonly string text;
            private readonly int intVal;
            private readonly int decCount;
            private readonly int caretIdx;

            internal string Text => text;
            internal int IntValue => intVal;
            internal int DecimalCount => decCount;
            internal int CaretIndex => caretIdx;

            internal InputFieldInfo(string text, int intVal, int decCount, int caretIdx) {
                this.text = text;
                this.intVal = intVal;
                this.decCount = decCount;
                this.caretIdx = caretIdx;
            }
        }
    }
}
