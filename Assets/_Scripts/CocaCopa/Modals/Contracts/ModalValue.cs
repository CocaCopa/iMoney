namespace CocaCopa.Modal.Contracts {
    public readonly struct ModalValue {
        public int Value { get; }
        public int Multiplier { get; }
        public ModalValue(int value, int decimalCount) {
            Value = value;
            Multiplier = (int)System.Math.Pow(10, decimalCount);
        }
    }
}
