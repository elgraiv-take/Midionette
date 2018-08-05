using System;

namespace Elgraiv.Midionette
{
    public class DelegateKeyReceiver : IKeyReceiver
    {
        public string Name => throw new NotImplementedException();

        private Action<byte> _keyDownAction;
        private Action<byte> _keyUpAction;

        public void OnKeyDown(byte velocity)
        {
            _keyDownAction?.Invoke(velocity);
        }

        public void OnKeyUp(byte velocity)
        {
            _keyUpAction?.Invoke(velocity);
        }

        public DelegateKeyReceiver(Action<byte> keyDownAction, Action<byte> keyUpAction)
        {
            _keyDownAction = keyDownAction;
            _keyUpAction = keyUpAction;
        }

        public DelegateKeyReceiver(Action keyDownAction, Action keyUpAction)
        {
            if (keyDownAction != null)
            {
                _keyDownAction = (v) => keyDownAction();
            }
            if (keyUpAction != null)
            {
                _keyUpAction = (v) => keyUpAction();
            }
        }
        public DelegateKeyReceiver(Action<bool> keyChangeAction)
        {
            if (keyChangeAction != null)
            {
                _keyDownAction = (v) => keyChangeAction(true);
                _keyUpAction = (v) => keyChangeAction(false);
            }
        }
        public DelegateKeyReceiver(Action<bool,byte> keyChangeAction)
        {
            if (keyChangeAction != null)
            {
                _keyDownAction = (v) => keyChangeAction(true, v);
                _keyUpAction = (v) => keyChangeAction(false, v);
            }
        }
    }
}
