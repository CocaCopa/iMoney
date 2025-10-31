using CocaCopa.Core.Math;

namespace CocaCopa.Core.Animation {
    public sealed class ValueAnimator {
        private readonly IEasing easing;
        private readonly float from;
        private readonly float to;
        private readonly float defaultSpeed;
        private float speed;

        private float animationPoints;

        /// <summary>
        /// Indicates whether the animation has reached or exceeded its target end (normalized progress ≥ 1).
        /// </summary>
        public bool IsComplete => animationPoints >= 1f;
        /// <summary>
        /// Gets the current normalized progress of the animation, ranging from 0 (start) to 1 (end).
        /// </summary>
        public float Progress => animationPoints;
        /// <summary>
        /// Overrides the current animation speed with a new value for dynamic control during playback.
        /// </summary>
        /// <param name="newSpeed">The new speed value to apply to the animation.</param>
        public void OverrideSpeed(float newSpeed) => speed = newSpeed;
        /// <summary>
        /// Restores the animation speed to its original value specified at initialization.
        /// </summary>
        public void ResetSpeedToDefault() => speed = defaultSpeed;
        public void ResetAnimator() => animationPoints = 0f;

        public ValueAnimator(float from, float to, float speed, IEasing easing) {
            this.from = from;
            this.to = to;
            this.speed = defaultSpeed = speed;
            this.easing = easing;
            animationPoints = 0f;
        }

        /// <summary>
        /// Advance the animation and get the current value.
        /// </summary>
        /// <param name="deltaTime">how much time passed since last step</param>
        /// <returns>The interpolated value between from..to using easing.</returns>
        public float EvaluateUnclamped(float deltaTime) => Step(deltaTime, true);
        /// <summary>
        /// Advance the animation and get the current value.
        /// </summary>
        /// <param name="deltaTime">how much time passed since last step</param>
        /// <returns>The interpolated value between from..to using easing.</returns>
        public float Evaluate(float deltaTime) => Step(deltaTime, false);

        private float Step(float deltaTime, bool unclamped) {
            animationPoints += speed * deltaTime;
            animationPoints = CCMath.Clamp01(animationPoints);
            float curvedT = easing.Evaluate(animationPoints);

            if (unclamped) { return CCMath.LerpUnclamped(from, to, curvedT); }
            else { return CCMath.Lerp(from, to, curvedT); }
        }
    }
}
