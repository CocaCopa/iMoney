using CocaCopa.Modal.Contracts;

namespace CocaCopa.Modal.Domain {
    public class NumpadRules {
        internal static NumpadData ExtractData(string text) {
            if (string.IsNullOrEmpty(text) || text == ".") return new NumpadData(string.Empty, 0, 0);
            var dot = text.IndexOf('.');
            if (dot < 0) {
                return int.TryParse(text, out var vValue)
                    ? new NumpadData(text, vValue, 0)
                    : new NumpadData(string.Empty, 0, 0);
            }
            var left = text[..dot];
            var right = dot + 1 < text.Length ? text[(dot + 1)..] : "";
            var decCount = right.Length;
            var compact = (left.Length == 0 ? "0" : left) + right;
            return int.TryParse(compact, out var val)
                ? new NumpadData(text, val, decCount)
                : new NumpadData(string.Empty, 0, 0);
        }

        internal static NumpadState Apply(NumpadState currentState, NumpadInput input) {
            return input switch {
                NumpadInput.DecimalPoint => Input_DecimalInput(currentState),
                NumpadInput.Backspace => Input_Backspace(currentState),
                _ => Input_Digit(currentState, input)
            };
        }

        private static NumpadState Input_DecimalInput(NumpadState s) {
            if (s.Caret == s.Text.Length && s.OnDecimals) {
                return s;
            }
            else if (s.Text.Length == 0) {
                string newStr = "0.00";
                return new NumpadState(newStr, 2, true, s.MaxDecimals);
            }
            else if (!s.OnDecimals) {
                return new NumpadState(s.Text, s.Caret + 1, true, s.MaxDecimals);
            }
            else /* if (OnDecimals) */ {
                return s;
            }
        }

        public static NumpadState Input_Backspace(NumpadState s) {
            if (s.Caret == 0) { return s; }
            int caretIndex = s.Caret - 1;
            bool onDecimals = s.OnDecimals;
            string currStr = s.Text;

            if (caretIndex == s.Text.Length - 1 - s.MaxDecimals) {
                caretIndex--;
                onDecimals = false;
            }

            if (onDecimals) {
                var rmCount = currStr.Length - caretIndex;
                currStr = currStr[..^rmCount];
                for (int i = 0; i < rmCount; i++) {
                    currStr += '0';
                }
            }
            else {
                string[] parts = currStr.Split('.');
                if (parts[0].Length > 1) {
                    currStr = parts[0][..^1];
                    currStr += $".{parts[1]}";
                }
                else { currStr = string.Empty; }
            }
            return new NumpadState(currStr, caretIndex, onDecimals, s.MaxDecimals);
        }

        private static NumpadState Input_Digit(NumpadState s, NumpadInput input) {
            if ((s.Text.Length > 6 && !s.OnDecimals) || (s.Caret == s.Text.Length && s.OnDecimals)) {
                return s;
            }

            string strInput = input.ToString();
            if (!strInput.Contains("Digit")) { throw new System.Exception("[NumpadRules] Could not read input"); }
            strInput = strInput.Replace("Digit", "");
            string newStr = "";

            if (s.Text.Length == 0 && input == NumpadInput.Digit0) {
                newStr = "0.00";
                return new NumpadState(newStr, 2, true, s.MaxDecimals);
            }

            if (s.Text.Length == 0) {
                newStr = $"{strInput}.00";
                return new NumpadState(newStr, s.Caret + 1, s.OnDecimals, s.MaxDecimals);
            }

            for (int i = 0; i < s.Text.Length; i++) {
                newStr += s.Text[i];
                if (s.Caret == i + 1) {
                    newStr += strInput;
                }
            }
            if (s.OnDecimals) { newStr = newStr[..^1]; }

            return new NumpadState(newStr, s.Caret + 1, s.OnDecimals, s.MaxDecimals);
        }
    }
}
