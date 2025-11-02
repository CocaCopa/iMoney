using CocaCopa.Modal.Contracts;
using UnityEngine;

namespace CocaCopa.Modal.Runtime.Animation {
    internal class ModalAnimation : MonoBehaviour {
        [Header("References")]
        [SerializeField] private RectTransform inputRect;
        [SerializeField] private RectTransform vkRect;

        [Header("Animation")]
        [SerializeField] private AnimationCurve visibilityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private float visibilitySpeed = 1f;
        [SerializeField] private float delayTime;

        private ModalAnimationFlow animFlow;
        private Coroutine animRoutine;

        internal bool IsAnimating {
            get; private set;
        }

        private AnimOptions inputAnimOpt = AnimOptions.Left;
        private AnimOptions vkAnimOpt = AnimOptions.Bottom;

        private bool isInitialized = false;

        internal void Init_Start() {
            if (isInitialized) { return; }
            isInitialized = true;
            Canvas.ForceUpdateCanvases();
            animFlow = CreateFlow();
            gameObject.SetActive(true);
        }

        private ModalAnimationFlow CreateFlow() {
            return new ModalAnimationFlow(
                new ModalAnimationFlow.ModalObject(AnimOptions.Left, inputRect, visibilityCurve, visibilitySpeed),
                new ModalAnimationFlow.ModalObject(AnimOptions.Bottom, vkRect, visibilityCurve, visibilitySpeed),
                new ModalAnimationFlow.FlowOptions(ModalAnimationFlow.AnimateFirst.Input, delayTime)
            );
        }

        internal void SetInputAnimOptions(AnimOptions opt) => inputAnimOpt = opt;
        internal void SetVkAnimOptions(AnimOptions opt) => vkAnimOpt = opt;

        internal void SetActive(bool active, ModalOptions options) {

        }
        internal void SetActive(bool active) => animRoutine ??= StartCoroutine(TickAnim(active));


        private System.Collections.IEnumerator TickAnim(bool active) {
            do {
                animFlow.TickSequence(Time.unscaledDeltaTime, !active);
                yield return null;
            } while (!animFlow.Completed);
            animRoutine = null;
        }
    }
}
