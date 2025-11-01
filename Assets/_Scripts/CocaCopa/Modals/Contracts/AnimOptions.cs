namespace CocaCopa.Modal.Contracts {
    public readonly struct AnimOptions {
        public readonly Appear appear;
        public readonly Disappear disappear;
        public AnimOptions(Appear appear, Disappear disappear) {
            this.appear = appear;
            this.disappear = disappear;
        }
    }

    public enum Appear {
        Left, Right, Bottom
    }

    public enum Disappear {
        Left, Right, Bottom
    }
}
