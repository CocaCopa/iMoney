using CocaCopa.Modal.Runtime.Domain;
using CocaCopa.Modal.Runtime.Internal;

namespace CocaCopa.Modal.Runtime.Domain {
    internal class VKStringConstructor {
        private const int MaxDecimal = 2;
        private NumpadState state;

        internal int CaretIndex => state.Caret;

        internal VKStringConstructor() {
            state = NumpadState.EmptyState(MaxDecimal);
        }

        internal NumpadData Apply(NumpadInput input) {
            state = NumpadRules.Apply(state, input);
            return NumpadRules.ExtractData(state.Text);
        }

        internal KeyboardData Apply(QwertyInput input) {
            throw new System.NotImplementedException();
        }

        internal void ResetStr() => state = NumpadState.EmptyState(MaxDecimal);
    }
}
