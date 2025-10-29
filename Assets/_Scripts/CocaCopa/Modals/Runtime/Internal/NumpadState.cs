namespace CocaCopa.Modal.Runtime.Internal {
    public readonly struct NumpadState {
        internal string Text { get; }
        internal int Caret { get; }
        internal bool OnDecimals { get; }
        internal int MaxDecimals { get; }
        internal NumpadState(string text, int caret, bool onDecimals, int maxDecimals) {
            Text = text; Caret = caret; OnDecimals = onDecimals; MaxDecimals = maxDecimals;
        }
        internal static NumpadState EmptyState(int decimals) => new NumpadState(string.Empty, 0, false, decimals);
    }
}
