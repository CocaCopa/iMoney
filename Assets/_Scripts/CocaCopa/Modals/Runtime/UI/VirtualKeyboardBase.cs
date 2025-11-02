using CocaCopa.Modal.Runtime.Internal;
using CocaCopa.Core.Extensions;
using UnityEngine;

namespace CocaCopa.Modal.Runtime.UI {
    internal abstract class VirtualKeyboardBase : MonoBehaviour {
        internal event System.Action<NumpadInput> OnVirtualKeyPressed;

        protected virtual void Awake() {
            AddListeners();
        }

        protected abstract void AddListeners();

        protected void InvokeOnKeyPressed(NumpadInput input) => OnVirtualKeyPressed?.SafeInvoke(input, nameof(OnVirtualKeyPressed));
    }
}
