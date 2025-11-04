using CocaCopa.Modal.Runtime.Internal;

namespace CocaCopa.Modal.Runtime.Domain {
    internal class VirtualCaret {
        private readonly KeyboardType keyboardType;
        private readonly string caretColor;

        internal static VirtualCaret NumpadCaret(KeyboardType keyboardType, string caretColor) => new VirtualCaret(keyboardType, caretColor);

        private VirtualCaret(KeyboardType keyboardType, string caretColor) {
            this.keyboardType = keyboardType;
            this.caretColor = caretColor;
        }

        internal string ApplyCaret(string targetString, int index) {
            return keyboardType switch {
                KeyboardType.Numpad => ColorizeAtIndex(targetString, index, caretColor),
                KeyboardType.QWERTY => targetString,
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
