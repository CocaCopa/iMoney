using System.Threading;
using System.Threading.Tasks;
using CocaCopa.Logger;
using CocaCopa.Modal.Core;
using CocaCopa.Modal.UI;
using UnityEngine;

public class Tester : MonoBehaviour {
    [Header("Input Modal")]
    [SerializeField] private ModalUI numpadModal;
    [SerializeField] private bool visible;
    [SerializeField] private bool hidden;

    private CancellationTokenSource lifetimeCts;

    private void Awake() {
        lifetimeCts = new CancellationTokenSource();
    }

    private void OnDestroy() {
        lifetimeCts.Cancel();
    }

    private void Update() {
        InputModal();
    }

    private async void InputModal() {
        if (visible) {
            visible = false;

        }
        if (hidden) {
            hidden = false;

        }
    }
}
