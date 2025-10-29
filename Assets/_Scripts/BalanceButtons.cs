using System.Threading;
using CocaCopa.Modal.Contracts;
using UnityEngine;

public class BalanceButtons : MonoBehaviour {
    [SerializeField] private BalanceButtonsUI buttonsUi;
    [SerializeField] private MonoBehaviour numpadModal;

    private IModalService NumpadModal => numpadModal as IModalService;

    private CancellationTokenSource lifetimeCts;

    private ModalOptions addModalOptions = new ModalOptions(AppearFrom.Left, CachedInputValue.Erase);
    private ModalOptions spendModalOptions = new ModalOptions(AppearFrom.Right, CachedInputValue.Erase);

    private void Awake() {
        lifetimeCts = new CancellationTokenSource();
    }

    private void OnDestroy() {
        lifetimeCts.Cancel();
    }

    private void Start() {
        buttonsUi.OnAddPressed += Buttons_OnAddPressed;
        buttonsUi.OnSpendPressed += Buttons_OnSpendPressed;
    }

    private void Buttons_OnAddPressed() {
        ActivateModal(addModalOptions);
    }

    private void Buttons_OnSpendPressed() {
        ActivateModal(spendModalOptions);
    }

    private async void ActivateModal(ModalOptions options) {
        var modalResult = await NumpadModal.ShowAsync(options, lifetimeCts.Token);
        if (modalResult.Confirmed) {
            NumpadModal.Hide();
        }
        else {
            NumpadModal.Hide();
        }
    }
}
