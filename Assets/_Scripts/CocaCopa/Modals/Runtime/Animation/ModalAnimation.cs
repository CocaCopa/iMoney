using CocaCopa.Modal.Contracts;
using UnityEngine;

namespace CocaCopa.Modal.Runtime.Animation {
    internal class ModalAnimation : MonoBehaviour {
        [Header("References")]
        [SerializeField] private RectTransform modalRect;
        [SerializeField] private RectTransform vkRect;

        [Header("Animation")]
        [SerializeField] private AnimationCurve visibilityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private float visibilitySpeed = 1f;
        [SerializeField] private float delayTime;

        internal bool IsAnimating {
            get; private set;
        }

        private AnimOptions inputAnimOpt = AnimOptions.Left;
        private AnimOptions vkAnimOpt = AnimOptions.Bottom;

        private RectPositions modalPositionsLeft;
        private RectPositions modalPositionsRight;
        private RectPositions vkPositionsLeft;
        private RectPositions vkPositionsRight;
        private RectPositions vkPositionsBottom;

        private RectPositions modalPositions;
        private RectPositions vkPositions;

        private float modalAnimPoints;
        private float vkAnimPoints;

        private float animationSpeed;
        private float delayTimer;

        private bool isInitialized = false;

        internal void Init_Start() {
            if (isInitialized) { return; }
            isInitialized = true;
            Canvas.ForceUpdateCanvases();
            modalRect.anchoredPosition = modalPositionsLeft.hidden;
            vkRect.anchoredPosition = vkPositions.hidden;
            delayTimer = delayTime;
            animationSpeed = visibilitySpeed;
            gameObject.SetActive(true);
            enabled = false;
        }

        private void Update() {
            PlaySequence();
        }

        internal void SetInputAnimOptions(AnimOptions opt) => inputAnimOpt = opt;
        internal void SetVkAnimOptions(AnimOptions opt) => vkAnimOpt = opt;
        internal void SetActive(bool active) {
            if (active) {
                modalPositions = GetInputShowPositions(inputAnimOpt.appear);
                vkPositions = GetVkShowPositions(vkAnimOpt.appear);
            }
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

        private RectPositions GetInputShowPositions(Appear appear) {
            return appear switch {
                Appear.Left => modalPositionsLeft,
                Appear.Right => modalPositionsRight,
                Appear.Bottom => throw new System.NotImplementedException(),
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }

        private RectPositions GetVkShowPositions(Appear appear) {
            return appear switch {
                Appear.Left => vkPositionsLeft,
                Appear.Right => vkPositionsRight,
                Appear.Bottom => vkPositionsBottom,
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }

        private void LerpRectTransform(RectTransform rectTransform, ref float animPoints, RectPositions positions) {
            animPoints += animationSpeed * Time.unscaledDeltaTime;
            animPoints = Mathf.Clamp01(animPoints);
            float vkTime = visibilityCurve.Evaluate(animPoints);
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(positions.hidden, positions.visible, vkTime);
        }
    }
}
