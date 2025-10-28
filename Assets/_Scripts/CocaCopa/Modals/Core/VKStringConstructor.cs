using CocaCopa.Modal.Contracts;

namespace CocaCopa.Modal.Domain {
    public class VKStringConstructor {
        private const int MaxDecimal = 2;
        private NumpadState state;

        public int CaretIndex => state.Caret;

        public VKStringConstructor() {
            state = new NumpadState(string.Empty, caret: 0, onDecimals: false, MaxDecimal);
        }

        public NumpadData Apply(NumpadInput input) {
            state = NumpadRules.Apply(state, input);
            return NumpadRules.ExtractData(state.Text);
        }
    }
}
