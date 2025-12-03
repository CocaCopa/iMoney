using System;

namespace CocaCopa.Core.MathUtilities {
    public static class CCMath {

        /// <summary>
        /// Linearly interpolates between <paramref name="a"/> and <paramref name="b"/> by <paramref name="t"/>, 
        /// clamping <paramref name="t"/> to the [0, 1] range.
        /// </summary>
        public static float Lerp(float a, float b, float t) {
            t = t < 0f ? 0f : (t > 1f ? 1f : t);
            return a + (b - a) * t;
        }

        /// <summary>
        /// Linearly interpolates between <paramref name="a"/> and <paramref name="b"/> by <paramref name="t"/> 
        /// without clamping.
        /// </summary>
        public static float LerpUnclamped(float a, float b, float t) {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Clamps <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>.
        /// Returns <paramref name="min"/> if below range, <paramref name="max"/> if above.
        /// </summary>
        public static float Clamp(float value, float min, float max) {
            if (value < min) { return min; }
            if (value > max) { return max; }
            return value;
        }

        /// <summary>
        /// Clamps the given <paramref name="value"/> to the [0, 1] range.
        /// </summary>
        public static float Clamp01(float value) {
            if (value < 0) { return 0; }
            if (value > 1) { return 1; }
            return value;
        }

        /// <summary>
        /// Returns the larger of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static float Max(float a, float b) {
            if (a > b) return a;
            else return b;
        }

        /// <summary>
        /// Returns the smaller of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static float Min(float a, float b) {
            if (a < b) return a;
            else return b;
        }

        /// <summary>
        /// Returns true if <paramref name="a"/> and <paramref name="b"/> are approximately equal,
        /// using a relative epsilon-based comparison similar to Unity's Mathf.Approximately.
        /// </summary>
        public static bool Approximately(float a, float b) {
            const float epsilon = 1e-06f;
            return Math.Abs(a - b) < epsilon * Math.Max(1f, Math.Max(Math.Abs(a), Math.Abs(b)));
        }
    }
}
