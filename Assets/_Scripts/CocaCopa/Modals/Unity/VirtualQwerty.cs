using CocaCopa.Modal.Runtime.Internal;
using CocaCopa.Modal.SPI;
using UnityEngine;
using UnityEngine.UI;

namespace CocaCopa.Modal.Unity {
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

        protected override KeyboardType VKType => KeyboardType.QWERTY;

        protected override void AddListeners() {
            shift.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Shift));
            backspace.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Backspace));
            spacebar.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Spacebar));

            n1.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha1));
            n2.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha2));
            n3.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha3));
            n4.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha4));
            n5.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha5));
            n6.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha6));
            n7.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha7));
            n8.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha8));
            n9.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha9));
            n0.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Alpha0));

            Q.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Q));
            W.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.W));
            E.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.E));
            R.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.R));
            T.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.T));
            Y.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Y));
            U.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.U));
            I.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.I));
            O.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.O));
            P.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.P));

            A.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.A));
            S.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.S));
            D.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.D));
            F.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.F));
            G.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.G));
            H.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.H));
            J.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.J));
            K.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.K));
            L.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.L));

            Z.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.Z));
            X.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.X));
            C.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.C));
            V.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.V));
            B.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.B));
            N.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.N));
            M.onClick.AddListener(() => InvokeOnKeyPressed(QwertyInput.M));
        }
    }
}
