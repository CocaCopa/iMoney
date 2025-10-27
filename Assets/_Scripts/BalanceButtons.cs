using System.Threading;
using CocaCopa.Logger;
using CocaCopa.Modal.Contracts;
using CocaCopa.Modal.UI;
using UnityEngine;

public class BalanceButtons : MonoBehaviour {
    [SerializeField] private BalanceButtonsUI buttonsUi;
    [SerializeField] private ModalUI numpadModal;

    private CancellationTokenSource lifetimeCts;

    private ModalOptions addModalOptions = new ModalOptions(ModalOptions.AppearFrom.Left);
    private ModalOptions spendModalOptions = new ModalOptions(ModalOptions.AppearFrom.Right);

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

    private async void Buttons_OnAddPressed() {
        var modalResult = await numpadModal.ShowAsync(addModalOptions, lifetimeCts.Token);
        if (modalResult.Confirmed) {
            CustomDebug.Log($"Value: {modalResult.Value.Value} | Multiplier: {modalResult.Value.Multiplier}", LogColor.Blue);
        }
        else {
            numpadModal.Hide();
        }
    }

    private async void Buttons_OnSpendPressed() {
        var modalResult = await numpadModal.ShowAsync(spendModalOptions, lifetimeCts.Token);
    }
}
