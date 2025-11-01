using CocaCopa.Modal.Contracts;
using UnityEngine;

namespace CocaCopa.Modal.Runtime.Animation {
    public class ModalAnimationFlow {
        private readonly ModalObject inputObj;
        private readonly ModalObject vkObj;
        private readonly FlowOptions flowOptions;
        private readonly AnimateFirst animateFirst;
        private readonly float delayTime;

        private readonly ModalObject firstAnimatable;
        private readonly ModalObject secondAnimatable;

        private RectPositions inputPositionsLeft;
        private RectPositions inputPositionsRight;
        private RectPositions vkPositionsLeft;
        private RectPositions vkPositionsRight;
        private RectPositions vkPositionsBottom;

        private RectPositions inputPositions;
        private RectPositions vkPositions;

        internal ModalAnimationFlow(ModalObject inputObj, ModalObject vkObj, FlowOptions options) {
            this.inputObj = inputObj;
            this.vkObj = vkObj;
            flowOptions = options;

            (firstAnimatable, secondAnimatable) = GetAnimatables();
            CalcInputPositions();
            CalcVkPositions();
        }

        private (ModalObject, ModalObject) GetAnimatables() {
            if (flowOptions.animateFirst == AnimateFirst.Input) {
                return (inputObj, vkObj);
            }
            else {
                return (vkObj, inputObj);
            }
        }

        private void CalcInputPositions() {
            var inputRect = inputObj.rectTransform;
            var modalWidth = inputRect.rect.width;

            var inputVisible = inputRect.anchoredPosition;
            var modalHiddenLeft = inputVisible + Vector2.left * modalWidth;
            var modalHiddenRight = inputVisible + Vector2.right * modalWidth;

            inputPositionsLeft = new RectPositions(inputVisible, modalHiddenLeft);
            inputPositionsRight = new RectPositions(inputVisible, modalHiddenRight);
        }

        private void CalcVkPositions() {
            var vkRect = vkObj.rectTransform;
            var vkVisible = vkRect.anchoredPosition;

            var vkHeight = vkRect.rect.height;
            var vkWidth = vkRect.rect.width;
            var vkHiddenBottom = vkVisible + Vector2.down * vkHeight;
            var vkHiddenLeft = vkVisible + Vector2.left * vkWidth;
            var vkHiddenRight = vkVisible + Vector2.right * vkWidth;

            vkPositionsLeft = new RectPositions(vkVisible, vkHiddenLeft);
            vkPositionsRight = new RectPositions(vkVisible, vkHiddenRight);
            vkPositionsBottom = new RectPositions(vkVisible, vkHiddenBottom);
        }

        private System.Collections.IEnumerator TickSequence(float deltaTime, bool reverse) {
            float limit = reverse ? 0f : 1f;
            float m = reverse ? -1 : 1;
            float inputAnimSpeed = m * inputObj.animSpeed;
            float vkAnimSpeed = m * vkObj.animSpeed;
            float delayTimer = flowOptions.delay;
            float firstAnimPoints = 0f;
            float secondAnimPoints = 0f;

            while (firstAnimPoints == limit && secondAnimPoints == limit) {
                // Lerp(firstAnimatable, firstAnimPoints, );
                delayTimer -= deltaTime;
                delayTimer = Mathf.Max(0f, delayTimer);
                if (delayTimer == 0f) {
                    // TODO: Lerp 2nd animatable
                }
                yield return null;
            }
        }

        private void Lerp(ModalObject obj, ref float animPoints, RectPositions positions, float dt) {
            animPoints += obj.animSpeed * dt;
            animPoints = Mathf.Clamp01(animPoints);
            float time = obj.animCurve.Evaluate(animPoints);
            obj.rectTransform.anchoredPosition = Vector2.LerpUnclamped(positions.hidden, positions.visible, time);
        }

        private RectPositions GetInputShowPositions(Appear appear) {
            return appear switch {
                Appear.Left => inputPositionsLeft,
                Appear.Right => inputPositionsRight,
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


        internal readonly struct ModalObject {
            public readonly ModalOptions modalOptions;
            public readonly RectTransform rectTransform;
            public readonly AnimationCurve animCurve;
            public readonly float animSpeed;

            public ModalObject(ModalOptions modalOptions, RectTransform rectTransform, AnimationCurve animCurve, float animSpeed) {
                this.modalOptions = modalOptions;
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
