namespace CocaCopa.Modal.Core {
    public class NumpadData {
        internal string virtualString;
        internal int virtualValue;
        internal int decimalCount;

        public string VirtualString => virtualString;
        public int VirtualValue => virtualValue;
        public int DecimalCount => decimalCount;

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
