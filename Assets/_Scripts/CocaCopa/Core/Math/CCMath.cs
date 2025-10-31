namespace CocaCopa.Core.Math {

    public class CCMath {
        public static float Lerp(float a, float b, float t) {
            t = t < 0f ? 0f : (t > 1f ? 1f : t);
            return a + (b - a) * t;
        }

        public static float LerpUnclamped(float a, float b, float t) {
            return a + (b - a) * t;
        }

        public static float Clamp(float value, float min, float max) {
            if (value < min) { return min; }
            if (value > max) { return max; }
            return value;
        }

        public static float Clamp01(float value) {
            if (value < 0) { return 0; }
            if (value > 1) { return 1; }
            return value;
        }
    }
}
