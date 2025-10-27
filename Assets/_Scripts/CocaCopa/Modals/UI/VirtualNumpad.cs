using System;
using CocaCopa.Extensions;
using CocaCopa.Logger;
using CocaCopa.Modal.Core;
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

        public event Action<NumpadData> OnVirtualStringChanged;

        private VKStringConstructor stringCtor;
        private NumpadData numpadData;

        private void Awake() {
            stringCtor = new VKStringConstructor();
            numpadData = new NumpadData();
            AddListeners();
        }

        private void AddListeners() {
            numpad1.onClick.AddListener(() => AddCharacter('1'));
            numpad2.onClick.AddListener(() => AddCharacter('2'));
            numpad3.onClick.AddListener(() => AddCharacter('3'));
            numpad4.onClick.AddListener(() => AddCharacter('4'));
            numpad5.onClick.AddListener(() => AddCharacter('5'));
            numpad6.onClick.AddListener(() => AddCharacter('6'));
            numpad7.onClick.AddListener(() => AddCharacter('7'));
            numpad8.onClick.AddListener(() => AddCharacter('8'));
            numpad9.onClick.AddListener(() => AddCharacter('9'));
            numpad0.onClick.AddListener(() => AddCharacter('0'));
            numpadDot.onClick.AddListener(() => AddCharacter('.'));
            numpadBackspace.onClick.AddListener(() => EraseLastChar());
        }

        private void AddCharacter(char character) {
            numpadData = stringCtor.VirtualString(character, ConstructionMode.New);
            OnVirtualStringChanged?.SafeInvoke(numpadData, nameof(OnVirtualStringChanged));
        }

        private void EraseLastChar() {
            if (numpadData.VirtualString.Length == 0) { return; }
            numpadData = stringCtor.EraseLastChar();
            OnVirtualStringChanged?.SafeInvoke(numpadData, nameof(OnVirtualStringChanged));
        }
    }
}
