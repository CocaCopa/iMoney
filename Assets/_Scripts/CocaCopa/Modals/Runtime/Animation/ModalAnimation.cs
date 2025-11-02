using CocaCopa.Modal.Contracts;
using UnityEngine;

namespace CocaCopa.Modal.Runtime.Animation {
    internal class ModalAnimation : MonoBehaviour {
        [Header("References")]
        [SerializeField] private RectTransform inputRect;
        [SerializeField] private RectTransform vkRect;

        [Header("Animation Flow")]
        [SerializeField] private float delayTime;


        [Header("Input Animation")]
        [SerializeField] private AnimationCurve inputCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private float inputAnimSpeed = 1.5f;

        [Header("Virtual Keyboard Animation")]
        [SerializeField] private AnimationCurve vkCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private float vkAnimSpeed = 1.5f;

        private ModalAnimationFlow animFlow;
        private Coroutine animRoutine;

        internal bool IsAnimating => !animFlow.Completed;

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
                new ModalAnimationFlow.ModalObject(AnimOptions.Left, inputRect, inputCurve, inputAnimSpeed),
                new ModalAnimationFlow.ModalObject(AnimOptions.Bottom, vkRect, vkCurve, vkAnimSpeed),
                new ModalAnimationFlow.FlowOptions(ModalAnimationFlow.AnimateFirst.Input, delayTime)
            );
        }

        internal void SetActive(bool active, AnimOptions inputOpt, AnimOptions vkOpt) {
            animFlow.OverrideAnimOptions(inputOpt, vkOpt);
            SetActive(active);
        }
        internal void SetActive(bool active) => animRoutine ??= StartCoroutine(TickAnim(!active));


        private System.Collections.IEnumerator TickAnim(bool reverse) {
            do {
                animFlow.TickSequence(Time.unscaledDeltaTime, reverse);
                yield return null;
            } while (!animFlow.Completed);
            animRoutine = null;
        }
    }
}
