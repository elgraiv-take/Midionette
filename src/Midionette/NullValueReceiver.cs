using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    internal class NullValueReceiver : IValueReceiver
    {
        public string Name => "null";

        public void OnValueReceived(byte value)
        {
            //nop
        }
        private NullValueReceiver() { }

        public static NullValueReceiver Value { get; } = new NullValueReceiver();

    }
}
