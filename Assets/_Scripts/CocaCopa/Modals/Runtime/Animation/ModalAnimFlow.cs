using CocaCopa.Modal.Contracts;
using UnityEngine;

namespace CocaCopa.Modal.Runtime.Animation {
    public class ModalAnimationFlow {
        private readonly ModalObject inputObj;
        private readonly ModalObject vkObj;
        private readonly FlowOptions flowOptions;

        private RectPositions inputPositions;
        private RectPositions vkPositions;

        private ModalObject firstAnimatable;
        private ModalObject secondAnimatable;
        private RectPositions firstAnimPositions;
        private RectPositions secondAnimPositions;

        internal ModalAnimationFlow(ModalObject inputObj, ModalObject vkObj, FlowOptions options) {
            this.inputObj = inputObj;
            this.vkObj = vkObj;
            flowOptions = options;

            CalcInputPositions();
            CalcVkPositions();

            SetAnimOrder(flowOptions.animateFirst);
            firstAnimatable.rectTransform.anchoredPosition = firstAnimPositions.hiddenLeft;
            secondAnimatable.rectTransform.anchoredPosition = secondAnimPositions.hiddenLeft;
        }

        internal void SetAnimOrder(AnimateFirst first) {
            if (first == AnimateFirst.Input) {
                firstAnimatable = inputObj;
                firstAnimPositions = inputPositions;
                secondAnimatable = vkObj;
                secondAnimPositions = vkPositions;
            }
            else {
                firstAnimatable = vkObj;
                firstAnimPositions = vkPositions;
                secondAnimatable = inputObj;
                secondAnimPositions = inputPositions;
            }
        }

        private void CalcInputPositions() {
            var inputRect = inputObj.rectTransform;
            var modalWidth = inputRect.rect.width;

            var inputVisible = inputRect.anchoredPosition;
            var modalHiddenLeft = inputVisible + Vector2.left * modalWidth;
            var modalHiddenRight = inputVisible + Vector2.right * modalWidth;

            inputPositions = new RectPositions(inputVisible, modalHiddenLeft, modalHiddenRight, Vector2.zero);
        }

        private void CalcVkPositions() {
            var vkRect = vkObj.rectTransform;
            var vkVisible = vkRect.anchoredPosition;

            var vkHeight = vkRect.rect.height;
            var vkWidth = vkRect.rect.width;
            var vkHiddenBottom = vkVisible + Vector2.down * vkHeight;
            var vkHiddenLeft = vkVisible + Vector2.left * vkWidth;
            var vkHiddenRight = vkVisible + Vector2.right * vkWidth;

            vkPositions = new RectPositions(vkVisible, vkHiddenLeft, vkHiddenRight, vkHiddenBottom);
        }

        internal System.Collections.IEnumerator TickSequence(float deltaTime, bool reverse) {
            float limit = reverse ? 0f : 1f;
            float m = reverse ? -1 : 1; // Will multiply with dt to reverse the animation if reverse = true
            float delayTimer = flowOptions.delay;
            float firstAnimPoints = 0f;
            float secondAnimPoints = 0f;

            while (firstAnimPoints == limit && secondAnimPoints == limit) {
                Lerp(firstAnimatable, ref firstAnimPoints, firstAnimPositions, deltaTime * m);
                delayTimer -= deltaTime;
                delayTimer = Mathf.Max(0f, delayTimer);
                if (delayTimer == 0f) {
                    Lerp(secondAnimatable, ref secondAnimPoints, secondAnimPositions, deltaTime);
                }
                yield return null;
            }
        }

        private void Lerp(ModalObject obj, ref float animPoints, RectPositions positions, float dt) {
            animPoints += obj.animSpeed * dt;
            animPoints = Mathf.Clamp01(animPoints);
            float time = obj.animCurve.Evaluate(animPoints);
            Vector2 targetPos = obj.animOptions.appear switch {
                Appear.Left => positions.hiddenLeft,
                Appear.Right => positions.hiddenLeft,
                Appear.Bottom => positions.hiddenBottom,
                _ => positions.hiddenLeft
            };
            obj.rectTransform.anchoredPosition = Vector2.LerpUnclamped(targetPos, positions.visible, time);
        }

        internal readonly struct ModalObject {
            public readonly AnimOptions animOptions;
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
            Input, VirtualKeyBoard
        }
    }
}
