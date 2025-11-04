using CocaCopa.Modal.Runtime.Internal;

namespace CocaCopa.Modal.Runtime.Domain {
    internal class VKStringConstructor {
        private readonly KeyboardType kType;
        private readonly int MaxDecimal = 2;

        private NumpadState numpadState;
        private QwertyState qwertyState;

        internal int CaretIndex => kType == KeyboardType.Numpad ? numpadState.Caret : qwertyState.Caret;

        internal VKStringConstructor(KeyboardType kt) {
            kType = kt;

            if (kType == KeyboardType.Numpad) {
                numpadState = NumpadState.EmptyState(MaxDecimal);
            }
            else qwertyState = QwertyState.EmptyState();
        }

        internal string Apply(System.Enum input) {
            if (input is NumpadInput numpad) {
                numpadState = NumpadRules.Apply(numpadState, numpad);
                return numpadState.Text;
            }
            else if (input is QwertyInput qwerty) {
                qwertyState = QwertyRules.Apply(new QwertyState(), qwerty);
                return qwertyState.Text;
            }

            throw new System.Exception("Could not read the provided input");
        }

        internal void ResetStr() {
            if (kType == KeyboardType.Numpad) {
                numpadState = NumpadState.EmptyState(MaxDecimal);
            }
            else qwertyState = QwertyState.EmptyState();
        }
    }
}
