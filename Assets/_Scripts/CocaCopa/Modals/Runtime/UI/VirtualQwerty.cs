using UnityEngine;
using UnityEngine.UI;

namespace CocaCopa.Modal.Runtime.UI {
    internal class VirtualQwerty : VirtualKeyboardBase {
        [Header("Special")]
        [SerializeField] private Button shift;
        [SerializeField] private Button backspace;
        [SerializeField] private Button spacebar;

        [Header("Row 0")]
        [SerializeField] private Button n1;
        [SerializeField] private Button n2;
        [SerializeField] private Button n3;
        [SerializeField] private Button n4;
        [SerializeField] private Button n5;
        [SerializeField] private Button n6;
        [SerializeField] private Button n7;
        [SerializeField] private Button n8;
        [SerializeField] private Button n9;
        [SerializeField] private Button n0;

        [Header("Row 1")]
        [SerializeField] private Button Q;
        [SerializeField] private Button W;
        [SerializeField] private Button E;
        [SerializeField] private Button R;
        [SerializeField] private Button T;
        [SerializeField] private Button Y;
        [SerializeField] private Button U;
        [SerializeField] private Button I;
        [SerializeField] private Button O;
        [SerializeField] private Button P;

        [Header("Row 2")]
        [SerializeField] private Button A;
        [SerializeField] private Button S;
        [SerializeField] private Button D;
        [SerializeField] private Button F;
        [SerializeField] private Button G;
        [SerializeField] private Button H;
        [SerializeField] private Button J;
        [SerializeField] private Button K;
        [SerializeField] private Button L;

        [Header("Row 3")]
        [SerializeField] private Button Z;
        [SerializeField] private Button X;
        [SerializeField] private Button C;
        [SerializeField] private Button V;
        [SerializeField] private Button B;
        [SerializeField] private Button N;
        [SerializeField] private Button M;

        protected override void AddListeners() {

        }
    }
}
