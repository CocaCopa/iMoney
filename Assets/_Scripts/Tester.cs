using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Tester : MonoBehaviour {
    [Header("Input Modal")]
    [SerializeField] private bool visible;
    [SerializeField] private bool hidden;

    private void Update() {
        InputModal();
    }

    private void InputModal() {
        if (visible) {
            visible = false;

        }
        if (hidden) {
            hidden = false;

        }
    }
}
