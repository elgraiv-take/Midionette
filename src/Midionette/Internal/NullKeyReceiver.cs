using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette.Internal
{
    internal class NullKeyReceiver : IKeyReceiver
    {
        public string Name => "null";

        public void OnKeyDown(byte velocity)
        {
            //nop
        }

        public void OnKeyUp(byte velocity)
        {
            //nop
        }

        private NullKeyReceiver() { }
        public static NullKeyReceiver Value { get; } = new NullKeyReceiver();
    }
}
