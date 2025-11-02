namespace CocaCopa.Modal.Runtime.Internal {
    internal readonly struct KeyboardData {
        public readonly KeyboardType type;
        public readonly NumpadInput numpadInput;
        public readonly QwertyInput qwertyInput;

        public KeyboardData(KeyboardType type, NumpadInput numpadInput, QwertyInput qwertyInput) {
            this.type = type;
            this.numpadInput = numpadInput;
            this.qwertyInput = qwertyInput;
        }
    }
}
