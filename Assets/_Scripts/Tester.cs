using CocaCopa.Modal.UI;
using UnityEngine;

public class Tester : MonoBehaviour {
    [Header("Input Modal")]
    [SerializeField] private ModalAnimationUI inputModal;
    [SerializeField] private bool visible;
    [SerializeField] private bool hidden;

    private void Update() {
        InputModal();
    }

    private void InputModal() {
        if (visible) {
            visible = false;
            inputModal.SetActive(true);
        }
        if (hidden) {
            hidden = false;
            inputModal.SetActive(false);
        }
    }
}
