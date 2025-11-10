using System;

namespace CocaCopa.Modal.SPI
{
    internal interface IModalView
    {
        public event Action OnConfirmIntent;
        public event Action OnCancelIntent;
        void SetInputFieldStr(string txt, bool allowConfirm = true);
    }
}
