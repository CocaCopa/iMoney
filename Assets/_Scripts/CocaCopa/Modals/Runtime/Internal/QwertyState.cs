namespace CocaCOpa.Modal.Runtime.Internal {
    internal readonly struct QwertyState {
        internal readonly string Text { get; }
        internal readonly int Caret { get; }

        internal QwertyState(string text, int caret) {
            Text = text; Caret = caret;
        }
        internal static QwertyState EmptyState() => new QwertyState(string.Empty, 0);
    }
}
