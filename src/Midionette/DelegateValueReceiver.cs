﻿using System;

namespace Elgraiv.Midionette
{
    public class DelegateValueReceiver : IValueReceiver
    {
        public string Name { get; }
        private Action<byte> _action;

        public void OnValueReceived(byte value)
        {
            _action?.Invoke(value);
        }

        public DelegateValueReceiver(string name,Action<byte> action)
        {
            Name = name;
            _action = action;
        }
    }
}
