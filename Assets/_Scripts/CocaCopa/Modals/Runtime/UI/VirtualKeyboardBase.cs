using System;
using CocaCopa.Core.Extensions;
using CocaCopa.Modal.Runtime.Internal;
using UnityEngine;

namespace CocaCopa.Modal.Runtime.UI {
    internal abstract class VirtualKeyboardBase : MonoBehaviour {
        internal event Action<Enum> OnVirtualKeyPressed;

        protected virtual void Awake() {
            AddListeners();
        }

        protected abstract void AddListeners();

        protected void InvokeOnKeyPressed<T>(T input) where T : Enum {
            OnVirtualKeyPressed?.SafeInvoke(input, nameof(OnVirtualKeyPressed));
        }
    }
}
