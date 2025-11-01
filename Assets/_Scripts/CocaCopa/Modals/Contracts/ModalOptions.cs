namespace CocaCopa.Modal.Contracts {
    public readonly struct ModalOptions {
        public readonly CachedInputValue cachedInputValue;
        public readonly AnimOptions inputAnimOpt;
        public readonly AnimOptions vkAnimOpt;
        public ModalOptions(CachedInputValue cachedInputValue, AnimOptions inputAnimOpt, AnimOptions vkAnimOpt) {
            this.cachedInputValue = cachedInputValue;
            this.inputAnimOpt = inputAnimOpt;
            this.vkAnimOpt = vkAnimOpt;
        }
    }

    public enum CachedInputValue {
        Erase, Keep
    }
}
