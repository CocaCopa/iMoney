using CocaCopa.Modal.Contracts;
using UnityEngine;

namespace CocaCopa.Modal.Unity.Animation {
    public class ModalAnimationFlow {
        private ModalObject inputObj;
        private ModalObject vkObj;
        private readonly FlowOptions flowOptions;

        private RectPositions inputPositions;
        private RectPositions vkPositions;

        private ModalObject firstAnimatable;
        private ModalObject secondAnimatable;
        private RectPositions firstAnimPositions;
        private RectPositions secondAnimPositions;

        private float firstAnimPoints;
        private float secondAnimPoints;
        private float delayTimer;

        internal bool Completed { get; private set; }

        internal ModalAnimationFlow(ModalObject inputObj, ModalObject vkObj, FlowOptions options) {
            this.inputObj = inputObj;
            this.vkObj = vkObj;
            flowOptions = options;
            delayTimer = flowOptions.delay;
            Completed = true;

            CalcInputPositions();
            CalcVkPositions();

            SetAnimOrder(flowOptions.animateFirst);
            firstAnimatable.rectTransform.anchoredPosition = firstAnimPositions.hiddenLeft;
            secondAnimatable.rectTransform.anchoredPosition = secondAnimPositions.hiddenLeft;
        }

        internal void OverrideAnimOptions(AnimOptions inputOpt, AnimOptions vkOpt) {
            inputObj.animOptions = inputOpt;
            vkObj.animOptions = vkOpt;
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

        internal void TickSequence(float deltaTime, bool reverse) {
            float limit = reverse ? 0f : 1f;
            int m = reverse ? -1 : 1;
            Lerp(firstAnimatable, ref firstAnimPoints, firstAnimPositions, deltaTime * m);
            delayTimer -= deltaTime;
            delayTimer = Mathf.Max(0f, delayTimer);
            if (delayTimer == 0f) {
                Lerp(secondAnimatable, ref secondAnimPoints, secondAnimPositions, deltaTime * m);
            }

            Completed = firstAnimPoints == limit && secondAnimPoints == limit;
            if (Completed) { delayTimer = flowOptions.delay; }
        }

        private void Lerp(ModalObject obj, ref float animPoints, RectPositions positions, float dt) {
            animPoints += obj.animSpeed * dt;
            animPoints = Mathf.Clamp01(animPoints);
            float time = obj.animCurve.Evaluate(animPoints);
            Vector2 hiddenPos = dt > 0 ?
            obj.animOptions.appear switch {
                Appear.Left => positions.hiddenLeft,
                Appear.Right => positions.hiddenRight,
                Appear.Bottom => positions.hiddenBottom,
                _ => positions.hiddenLeft
            } :
            obj.animOptions.disappear switch {
                Disappear.Left => positions.hiddenLeft,
                Disappear.Right => positions.hiddenRight,
                Disappear.Bottom => positions.hiddenBottom,
                _ => positions.hiddenLeft
            };
            obj.rectTransform.anchoredPosition = Vector2.LerpUnclamped(hiddenPos, positions.visible, time);
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
            Input, VirtualKeyBoard
        }
    }
}
