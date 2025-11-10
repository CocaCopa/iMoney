using System.Threading.Tasks;
using CocaCopa.Modal.Contracts;

namespace CocaCopa.Modal.SPI {
    internal interface IModalAnimator {
        Task PlayShowAsync(AnimOptions input, AnimOptions vk);
        Task PlayHideAsync(AnimOptions input, AnimOptions vk);
        Task PlayShowAsync();
        Task PlayHideAsync();
        bool IsVisible { get; }
    }
}
