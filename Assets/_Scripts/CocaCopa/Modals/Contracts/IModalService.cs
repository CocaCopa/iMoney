using System.Threading;
using System.Threading.Tasks;

namespace CocaCopa.Modal.Contracts {
    public readonly struct ModalValue {
        public int Value { get; }
        public int Multiplier { get; }
        public ModalValue(int value, int decimalCount) {
            Value = value;
            Multiplier = (int)System.Math.Pow(10, decimalCount);
        }
    }

    public readonly struct ModalResult {
        public bool Confirmed { get; }
        public ModalValue Value { get; }
        private ModalResult(bool confirmed, ModalValue modalValue) {
            Confirmed = confirmed;
            Value = modalValue;
        }
        public static ModalResult Cancel() => new ModalResult(false, new ModalValue());
        public static ModalResult Confirm(ModalValue value) => new ModalResult(true, value);
    }

    public readonly struct ModalOptions {
        public enum AppearFrom { Left, Right };
        public readonly AppearFrom appearFrom;
        public ModalOptions(AppearFrom appearFrom) {
            this.appearFrom = appearFrom;
        }
    }

    public interface IModalService {
        Task<ModalResult> ShowAsync(ModalOptions options, CancellationToken ct);
        void Hide();
        bool IsActive { get; }
    }
}
