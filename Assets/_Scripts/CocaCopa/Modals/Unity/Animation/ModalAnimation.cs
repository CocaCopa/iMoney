using System.Threading.Tasks;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.SPI;
using UnityEngine;

namespace CocaCopa.Modal.Unity.Animation {
    [RequireComponent(typeof(CanvasGroup))]
    internal class ModalAnimation : MonoBehaviour, IModalAnimator {
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

        public bool IsVisible { get; private set; }

        private void Awake() {
            var cg = GetComponent<CanvasGroup>();
            cg.alpha = 1f;
        }

        private void Start() {
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

        public async Task PlayShowAsync() => await TickAnim(reverse: false);
        public async Task PlayShowAsync(AnimOptions input, AnimOptions vk) {
            animFlow.OverrideAnimOptions(input, vk);
            await TickAnim(reverse: false);
        }

        public async Task PlayHideAsync() => await TickAnim(reverse: true);
        public async Task PlayHideAsync(AnimOptions input, AnimOptions vk) {
            animFlow.OverrideAnimOptions(input, vk);
            await TickAnim(reverse: true);
        }

        private async Task TickAnim(bool reverse) {
            do {
                animFlow.TickSequence(Time.unscaledDeltaTime, reverse);
                await Task.Yield();
            } while (!animFlow.Completed);
        }
    }
}
