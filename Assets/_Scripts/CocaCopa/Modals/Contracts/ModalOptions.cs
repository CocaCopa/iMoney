namespace CocaCopa.Modal.Contracts {
    public readonly struct ModalOptions {
        public readonly AppearFrom appearFrom;
        public readonly CachedInputValue cachedInputValue;
        public ModalOptions(AppearFrom appearFrom, CachedInputValue cachedInputValue) {
            this.appearFrom = appearFrom;
            this.cachedInputValue = cachedInputValue;
        }
    }
}
