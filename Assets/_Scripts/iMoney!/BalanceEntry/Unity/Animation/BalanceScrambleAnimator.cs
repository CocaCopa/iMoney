using TMPro;
using UnityEngine;

namespace iMoney.BalanceEntry.Unity.Animation {
    [RequireComponent(typeof(TextMeshProUGUI))]
    internal sealed class BalanceScrambleAnimator : MonoBehaviour {
        [SerializeField] private float duration = 0.8f;
        [SerializeField] private float rate = 0.04f;

        private TextMeshProUGUI targetText;

        private void Awake() {
            targetText = GetComponent<TextMeshProUGUI>();
        }

        internal System.Collections.IEnumerator ScrambleText(int lettersCount, System.Action onComplete) {
            float totalDuration = 0f;
            float scrambleTimer = 0f;
            int min = (int)Mathf.Pow(10, lettersCount);         // min = 100 -> 10^2 = 100
            int max = (int)Mathf.Pow(10, lettersCount + 1) - 1; // max = 999 -> 10^3 - 1 = 1000 - 1
            while (totalDuration < duration) {
                if (scrambleTimer <= 0f) {
                    scrambleTimer = rate;
                    int randomNum = UnityEngine.Random.Range(min, max);
                    targetText.SetText($"{randomNum}");
                }
                scrambleTimer -= Time.unscaledDeltaTime;
                totalDuration += Time.unscaledDeltaTime;
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
