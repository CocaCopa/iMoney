namespace CocaCopa.Modal.Runtime.Internal {
    internal class NumpadData {
        internal string virtualString;
        internal int virtualValue;
        internal int decimalCount;

        internal string VirtualString => virtualString;
        internal int VirtualValue => virtualValue;
        internal int DecimalCount => decimalCount;

        internal NumpadData(string virtualString, int virtualValue, int decimalCount) {
            this.virtualString = virtualString;
            this.virtualValue = virtualValue;
            this.decimalCount = decimalCount;
        }

        public NumpadData() {
            virtualString = "";
            virtualValue = 0;
            decimalCount = 0;
        }
    }
}
