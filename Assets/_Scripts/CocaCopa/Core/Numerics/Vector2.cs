using System;
using System.Runtime.CompilerServices;

namespace CocaCopa.Core.Numerics.Geometry {
    /// <summary>
    /// Unity-free 2D vector. Immutable to avoid footguns.
    /// </summary>
    public readonly struct Vector2 : IEquatable<Vector2> {
        public readonly float X;
        public readonly float Y;

        public static readonly Vector2 Zero = new(0f, 0f);
        public static readonly Vector2 One = new(1f, 1f);
        public static readonly Vector2 Up = new(0f, 1f);
        public static readonly Vector2 Down = new(0f, -1f);
        public static readonly Vector2 Left = new(-1f, 0f);
        public static readonly Vector2 Right = new(1f, 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float x, float y) { X = x; Y = y; }

        // Basic ops
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator -(Vector2 v) => new(-v.X, -v.Y);
        public static Vector2 operator *(Vector2 v, float s) => new(v.X * s, v.Y * s);
        public static Vector2 operator *(float s, Vector2 v) => new(v.X * s, v.Y * s);
        public static Vector2 operator /(Vector2 v, float s) => new(v.X / s, v.Y / s);

        // Metrics
        public float Length() => MathF.Sqrt(X * X + Y * Y);
        public float LengthSquared() => X * X + Y * Y;

        // Products
        public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;
        public static float Cross(Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X; // 2D pseudo-cross

        // Normalization (safe)
        public Vector2 Normalized() {
            var lsq = LengthSquared();
            if (lsq <= 0f) return Zero;
            var inv = 1f / MathF.Sqrt(lsq);
            return new Vector2(X * inv, Y * inv);
        }

        // Utilities commonly needed without dragging a separate Math module
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t) {
            t = t < 0f ? 0f : (t > 1f ? 1f : t);
            return new Vector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        public static Vector2 ClampMagnitude(Vector2 v, float max) {
            var lsq = v.LengthSquared();
            if (lsq <= max * max) return v;
            var inv = max / MathF.Sqrt(lsq);
            return new Vector2(v.X * inv, v.Y * inv);
        }

        // Equality
        public bool Equals(Vector2 other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is Vector2 v && Equals(v);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"({X}, {Y})";

        // Withers for convenience
        public Vector2 WithX(float x) => new(x, Y);
        public Vector2 WithY(float y) => new(X, y);
    }
}
