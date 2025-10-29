namespace CocaCopa.Modal.Contracts {
    public readonly struct ModalResult {
        public bool Confirmed { get; }
        public ModalValue Value { get; }
        private ModalResult(bool confirmed, ModalValue modalValue) {
            Confirmed = confirmed;
            Value = modalValue;
        }
        public static ModalResult Cancel() => new ModalResult(false, new ModalValue());
        public static ModalResult Confirm(ModalValue value) => new ModalResult(true, value);
    }
}