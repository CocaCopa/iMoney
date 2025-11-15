using CocaCopa.Modal.Contracts;
using UnityEngine;

namespace CocaCopa.Modal.Unity.Animation {
    public sealed class ModalAnimationFlow {
        private readonly ModalObject inputObj;
        private readonly ModalObject vkObj;
        private readonly FlowOptions flowOptions;

        private RectPositions inputPositions;
        private RectPositions vkPositions;

        private ModalObject primaryObj;
        private ModalObject secondaryObj;
        private RectPositions primaryPositions;
        private RectPositions secondaryPositions;

        private float primaryProgress;
        private float secondaryProgress;
        private float remainingDelay;

        internal bool Completed { get; private set; }

        internal ModalAnimationFlow(ModalObject inputObj, ModalObject vkObj, FlowOptions options) {
            this.inputObj = inputObj;
            this.vkObj = vkObj;
            flowOptions = options;

            remainingDelay = flowOptions.delay;
            Completed = true;

            CalcInputPositions();
            CalcVkPositions();

            SetAnimOrder(flowOptions.animateFirst);

            primaryProgress = 0f;
            secondaryProgress = 0f;

            primaryObj.rectTransform.anchoredPosition =
                GetHiddenPosition(primaryObj, primaryPositions, isAppearing: true);

            secondaryObj.rectTransform.anchoredPosition =
                GetHiddenPosition(secondaryObj, secondaryPositions, isAppearing: true);
        }

        internal void OverrideAnimOptions(AnimOptions inputOpt, AnimOptions vkOpt) {
            inputObj.animOptions = inputOpt;
            vkObj.animOptions = vkOpt;
        }

        internal void SetAnimOrder(AnimateFirst first) {
            if (first == AnimateFirst.Input) {
                primaryObj = inputObj;
                primaryPositions = inputPositions;

                secondaryObj = vkObj;
                secondaryPositions = vkPositions;
            }
            else {
                primaryObj = vkObj;
                primaryPositions = vkPositions;

                secondaryObj = inputObj;
                secondaryPositions = inputPositions;
            }
        }

        private void CalcInputPositions() {
            var inputRect = inputObj.rectTransform;
            var modalWidth = inputRect.rect.width;

            var visible = inputRect.anchoredPosition;
            var hiddenLeft = visible + Vector2.left * modalWidth;
            var hiddenRight = visible + Vector2.right * modalWidth;

            // hiddenBottom is not yet supported yet
            inputPositions = new RectPositions(visible, hiddenLeft, hiddenRight, Vector2.zero);
        }

        private void CalcVkPositions() {
            var vkRect = vkObj.rectTransform;
            var visible = vkRect.anchoredPosition;

            var vkHeight = vkRect.rect.height;
            var vkWidth = vkRect.rect.width;

            var hiddenBottom = visible + Vector2.down * vkHeight;
            var hiddenLeft = visible + Vector2.left * vkWidth;
            var hiddenRight = visible + Vector2.right * vkWidth;

            vkPositions = new RectPositions(visible, hiddenLeft, hiddenRight, hiddenBottom);
        }

        /// <summary>
        /// Drives the animation sequence.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="reverse">true  → animate from visible → hidden (progress 1 → 0) | false → animate from hidden → visible (progress 0 → 1)</param>
        internal void TickSequence(float deltaTime, bool reverse) {
            if (Completed && deltaTime <= 0f) {
                return;
            }

            float targetProgress = reverse ? 0f : 1f;
            float direction = reverse ? -1f : 1f;
            float signedDelta = deltaTime * direction;

            StepObject(primaryObj, primaryPositions, ref primaryProgress, signedDelta);

            remainingDelay -= deltaTime;
            remainingDelay = Mathf.Max(0f, remainingDelay);

            if (remainingDelay == 0f) {
                StepObject(secondaryObj, secondaryPositions, ref secondaryProgress, signedDelta);
            }

            // Completed once both have reached their respective targets
            bool primaryDone = Mathf.Approximately(primaryProgress, targetProgress);
            bool secondaryDone = Mathf.Approximately(secondaryProgress, targetProgress);

            Completed = primaryDone && secondaryDone;

            if (Completed) {
                primaryProgress = targetProgress;
                secondaryProgress = targetProgress;
                remainingDelay = flowOptions.delay;
            }
        }

        private void StepObject(ModalObject obj, RectPositions positions, ref float progress, float signedDelta) {
            if (obj == null) {
                return;
            }

            progress += obj.animSpeed * signedDelta;
            progress = Mathf.Clamp01(progress);

            float t = obj.animCurve.Evaluate(progress);

            bool isAppearing = signedDelta > 0f;
            Vector2 hiddenPos = GetHiddenPosition(obj, positions, isAppearing);

            obj.rectTransform.anchoredPosition = Vector2.LerpUnclamped(hiddenPos, positions.visible, t);
        }

        private static Vector2 GetHiddenPosition(ModalObject obj, RectPositions positions, bool isAppearing) {
            if (isAppearing) {
                return obj.animOptions.appear switch {
                    Appear.Left => positions.hiddenLeft,
                    Appear.Right => positions.hiddenRight,
                    Appear.Bottom => positions.hiddenBottom,
                    _ => positions.hiddenLeft
                };
            }
            else {
                return obj.animOptions.disappear switch {
                    Disappear.Left => positions.hiddenLeft,
                    Disappear.Right => positions.hiddenRight,
                    Disappear.Bottom => positions.hiddenBottom,
                    _ => positions.hiddenLeft
                };
            }
        }

        internal class ModalObject {
            public AnimOptions animOptions;
            public readonly RectTransform rectTransform;
            public readonly AnimationCurve animCurve;
            public readonly float animSpeed;

            public ModalObject(AnimOptions animOptions, RectTransform rectTransform, AnimationCurve animCurve, float animSpeed) {
                this.animOptions = animOptions;
                this.rectTransform = rectTransform;
                this.animCurve = animCurve;
                this.animSpeed = animSpeed;
            }
        }

        internal readonly struct FlowOptions {
            public readonly AnimateFirst animateFirst;
            public readonly float delay;

            public static FlowOptions Default => new FlowOptions(AnimateFirst.Input, 0f);

            public FlowOptions(AnimateFirst animateFirst, float delay) {
                this.animateFirst = animateFirst;
                this.delay = delay;
            }
        }

        internal enum AnimateFirst {
            Input,
            VirtualKeyBoard
        }
    }
}
