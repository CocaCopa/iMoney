namespace CocaCopa.Modal.Contracts {
    [System.Serializable]
    public sealed class AnimOptions {
        public Appear appear;
        public Disappear disappear;
        public AnimOptions(Appear appear, Disappear disappear) {
            this.appear = appear;
            this.disappear = disappear;
        }
        public static AnimOptions Left => new AnimOptions(Appear.Left, Disappear.Left);
        public static AnimOptions Right => new AnimOptions(Appear.Right, Disappear.Right);
        public static AnimOptions Bottom => new AnimOptions(Appear.Bottom, Disappear.Bottom);
    }

    public enum Appear {
        Left, Right, Bottom
    }

    public enum Disappear {
        Left, Right, Bottom
    }
}
