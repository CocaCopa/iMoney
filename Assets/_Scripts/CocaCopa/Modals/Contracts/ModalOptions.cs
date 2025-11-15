namespace CocaCopa.Modal.Contracts {
    [System.Serializable]
    public struct ModalOptions {
        public CachedInputValue cachedInputValue;
        public AnimOptions inputAnimOpt;
        public AnimOptions vkAnimOpt;
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
