using System.Collections;
using CocaCopa.Core.Animation;
using UnityEngine;
using UnityEngine.UI;

namespace iMoney.BalanceEntry.Runtime.Animation {
    internal class BalanceButtonsAnimation : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Image addMask;
        [SerializeField] private Image spendMask;

        [Header("Animation")]
        [SerializeField] private AnimationCurve visibilityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private float visibilitySpeed = 1f;

        [Header("Alphas")]
        [SerializeField] private float addAlpha;
        [SerializeField] private float spendAlpa;

        private ValueAnimator addMaskAnimIn;
        private ValueAnimator spendMaskAnimIn;
        private ValueAnimator addMaskAnimOut;
        private ValueAnimator spendMaskAnimOut;

        private void Awake() {
            CreateAnimators();
        }

        private void CreateAnimators() {
            float speed = visibilitySpeed;
            IEasing easeCurve = new CurveEasing(visibilityCurve);
            addMaskAnimIn = new ValueAnimator(0f, addAlpha / 255f, speed, easeCurve);
            spendMaskAnimIn = new ValueAnimator(0f, spendAlpa / 255f, speed, easeCurve);
            addMaskAnimOut = new ValueAnimator(addAlpha / 255f, 0f, speed, easeCurve);
            spendMaskAnimOut = new ValueAnimator(spendAlpa / 255f, 0f, speed, easeCurve);
        }

        internal Coroutine FadeAddMask(FadeMode mode) {
            if (mode == FadeMode.In) { return StartCoroutine(FadeMask(addMask, addMaskAnimIn, true)); }
            else { return StartCoroutine(FadeMask(addMask, addMaskAnimOut, false)); }
        }

        internal Coroutine FadeSpendMask(FadeMode mode) {
            if (mode == FadeMode.In) { return StartCoroutine(FadeMask(spendMask, spendMaskAnimIn, true)); }
            else { return StartCoroutine(FadeMask(spendMask, spendMaskAnimOut, false)); }
        }

        private IEnumerator FadeMask(Image maskImg, ValueAnimator maskAnimator, bool rayTarget) {
            maskAnimator.ResetAnimator();
            addMask.raycastTarget = true;
            spendMask.raycastTarget = true;
            Color maskColor = maskImg.color;
            while (!maskAnimator.IsComplete) {
                float t = maskAnimator.Evaluate(Time.unscaledDeltaTime);
                maskColor.a = t;
                maskImg.color = maskColor;
                yield return null;
            }
            addMask.raycastTarget = rayTarget;
            spendMask.raycastTarget = rayTarget;
        }
    }
}