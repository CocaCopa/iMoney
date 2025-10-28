namespace CocaCopa.Modal.Domain {
    public readonly struct NumpadState {
        public string Text { get; }
        public int Caret { get; }
        public bool OnDecimals { get; }
        public int MaxDecimals { get; }
        public NumpadState(string text, int caret, bool onDecimals, int maxDecimals) {
            Text = text; Caret = caret; OnDecimals = onDecimals; MaxDecimals = maxDecimals;
        }
    }
}
