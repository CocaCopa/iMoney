using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.Unity;
using UnityEngine;

namespace CocaCopa.Modal.API {
    [RequireComponent(typeof(ModalInstaller))]
    public class ModalAdapter : MonoBehaviour {
        private IModalService Service { get; set; }
        public IModalService GetService() => Service ?? GetComponent<ModalInstaller>().ModalService ?? throw new System.Exception("Service not ready. If you tried to access the service inside Awake(), try moving your code inside Start() and see if that fixes the issue.");

        private void Awake() {
            var installer = GetComponent<ModalInstaller>();
            Service = installer.ModalService;
        }
    }
}
