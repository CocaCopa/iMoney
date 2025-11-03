using CocaCopa.Modal.Runtime.Internal;

namespace CocaCopa.Modal.Runtime.Domain {
    internal class VKStringConstructor {
        private const int MaxDecimal = 2;
        private NumpadState state;

        internal int CaretIndex => state.Caret;

        internal VKStringConstructor() {
            state = NumpadState.EmptyState(MaxDecimal);
        }

        internal string Apply(System.Enum input) {
            if (input is NumpadInput numpad) {
                state = NumpadRules.Apply(state, numpad);
                return state.Text;
            }

            throw new System.Exception();
        }

        private KeyboardData ApplyQwerty(QwertyInput input) {
            throw new System.NotImplementedException();
        }

        internal void ResetStr() => state = NumpadState.EmptyState(MaxDecimal);
    }
}
