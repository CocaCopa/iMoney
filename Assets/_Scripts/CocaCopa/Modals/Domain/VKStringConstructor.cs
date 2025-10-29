using CocaCopa.Modal.Contracts;

namespace CocaCopa.Modal.Domain {
    public class VKStringConstructor {
        private const int MaxDecimal = 2;
        private NumpadState state;

        public int CaretIndex => state.Caret;

        public VKStringConstructor() {
            state = NumpadState.EmptyState(MaxDecimal);
        }

        public NumpadData Apply(NumpadInput input) {
            state = NumpadRules.Apply(state, input);
            return NumpadRules.ExtractData(state.Text);
        }

        public void ResetStr() => state = NumpadState.EmptyState(MaxDecimal);
    }
}
