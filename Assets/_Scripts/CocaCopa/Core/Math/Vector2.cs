namespace CocaCopa.Core.Math {
    public struct Vector2 {
        public float x;
        public float y;

        public static readonly Vector2 One = new Vector2(1f, 1f);
        public static readonly Vector2 Up = new Vector2(0f, 1f);
        public static readonly Vector2 Down = new Vector2(0f, -1f);
        public static readonly Vector2 Left = new Vector2(-1f, 0f);
        public static readonly Vector2 Right = new Vector2(1f, 0f);

        public Vector2(float x, float y) {
            this.x = x;
            this.y = y;
        }
    }
}
