using UnityEngine;

namespace CocaCopa.Modal.UI {
    public class ModalAnimationUI : MonoBehaviour {
        [Header("References")]
        [SerializeField] private RectTransform modalRect;
        [SerializeField] private RectTransform vkRect;

        [Header("Animation")]
        [SerializeField] private AnimationCurve visibilityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private float visibilitySpeed = 1f;
        [SerializeField] private float delayTime;

        public enum AppearFrom { Left, Right };
        public bool IsAnimating {
            get; private set;
        }

        private RectPositions modalPositionsLeft;
        private RectPositions modalPositionsRight;
        private RectPositions vkPositions;

        private RectPositions modalPositions;

        private float modalAnimPoints;
        private float vkAnimPoints;

        private float animationSpeed;
        private float delayTimer;

        private void Start() {
            Canvas.ForceUpdateCanvases();
            FindPositions();
            modalRect.anchoredPosition = modalPositionsLeft.hidden;
            vkRect.anchoredPosition = vkPositions.hidden;
            delayTimer = delayTime;
            animationSpeed = visibilitySpeed;
            enabled = false;
        }

        private void FindPositions() {
            var modalVisible = modalRect.anchoredPosition;
            var vkVisible = vkRect.anchoredPosition;

            var modalWidth = modalRect.rect.width;
            var vkHeight = vkRect.rect.height;
            var modalHiddenLeft = modalVisible + Vector2.left * modalWidth;
            var modalHiddenRight = modalVisible + Vector2.right * modalWidth;
            var vkHidden = vkVisible + Vector2.down * vkHeight;

            modalPositionsLeft = new RectPositions(modalVisible, modalHiddenLeft);
            modalPositionsRight = new RectPositions(modalVisible, modalHiddenRight);
            vkPositions = new RectPositions(vkVisible, vkHidden);
        }

        private void Update() {
            PlaySequence();
        }

        public void SetActive(bool active) {
            SetActive(active, AppearFrom.Left);
        }
        public void SetActive(bool active, AppearFrom appearFrom) {
            if (active) { modalPositions = appearFrom == AppearFrom.Left ? modalPositionsLeft : modalPositionsRight; }
            IsAnimating = true;
            enabled = true;
            delayTimer = delayTime;
            animationSpeed = active ? visibilitySpeed : -visibilitySpeed;
        }

        private void PlaySequence() {
            LerpRectTransform(vkRect, ref vkAnimPoints, vkPositions);

            delayTimer -= Time.unscaledDeltaTime;
            delayTimer = Mathf.Max(0f, delayTimer);

            if (delayTimer == 0f) {
                LerpRectTransform(modalRect, ref modalAnimPoints, modalPositions);
            }

            if ((modalAnimPoints == 1f && vkAnimPoints == 1f) ||
                (modalAnimPoints == 0f && vkAnimPoints == 0f)) {
                enabled = false;
                IsAnimating = false;
            }
        }

        private void LerpRectTransform(RectTransform rectTransform, ref float animPoints, RectPositions positions) {
            animPoints += animationSpeed * Time.unscaledDeltaTime;
            animPoints = Mathf.Clamp01(animPoints);
            float vkTime = visibilityCurve.Evaluate(animPoints);
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(positions.hidden, positions.visible, vkTime);
        }
    }
}
