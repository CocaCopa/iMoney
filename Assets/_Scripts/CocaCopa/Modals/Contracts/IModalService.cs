using System.Threading;
using System.Threading.Tasks;

namespace CocaCopa.Modal.Contracts {
    public enum AppearFrom {
        Left, Right
    }

    public enum CachedInputValue {
        Erase, Keep
    }

    public interface IModalService {
        Task<ModalResult> ShowAsync(ModalOptions options, CancellationToken ct);
        void Hide();
        bool IsActive { get; }
    }
}
