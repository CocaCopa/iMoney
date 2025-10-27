using System;
using CocaCopa.Modal.UI;
using UnityEngine;

public class BalanceButtons : MonoBehaviour {
    [SerializeField] private BalanceButtonsUI buttonsUi;
    [SerializeField] private ModalAnimationUI modalAnim;

    private void Start() {
        buttonsUi.OnAddPressed += Buttons_OnAddPressed;
        buttonsUi.OnSpendPressed += Buttons_OnSpendPressed;
    }

    private void Buttons_OnAddPressed() {
        modalAnim.SetActive(true, ModalAnimationUI.AppearFrom.Left);
    }

    private void Buttons_OnSpendPressed() {
        modalAnim.SetActive(true, ModalAnimationUI.AppearFrom.Right);
    }
}
