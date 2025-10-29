namespace CocaCopa.Modal.Runtime.Domain {
    internal class VirtualCaret {
        private enum StringType {
            DecimalNumpad,
        }

        private readonly StringType stringType;
        private readonly string caretColor;

        internal static VirtualCaret NumpadCaret(string caretColor) => new VirtualCaret(StringType.DecimalNumpad, caretColor);

        private VirtualCaret(StringType stringType, string caretColor) {
            this.stringType = stringType;
            this.caretColor = caretColor;
        }

        internal string ApplyCaret(string targetString, int index) {
            return stringType switch {
                StringType.DecimalNumpad => ColorizeAtIndex(targetString, index, caretColor),
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }

        private static string ColorizeAtIndex(string targetString, int index, string color) {
            if (targetString == string.Empty) { return targetString; }
            if (!color.Contains('#')) { color = $"#{color}"; }
            var dotIdx = targetString.IndexOf('.');
            var newStr = targetString.Replace(".", "");
            var caret = index - 1;
            var colorizedStr = "";
            for (int i = 0; i < newStr.Length; i++) {
                if (i == caret && i < newStr.Length - 1) {
                    colorizedStr += $"<color={color}>{newStr[i]}</color>";
                }
                else colorizedStr += newStr[i];
                if (i == dotIdx - 1) {
                    colorizedStr += '.';
                }
            }
            return colorizedStr;
        }
    }
}
