using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public class MessageTypeEventArgs:EventArgs
    {
        public byte Channel { get; }
        public byte Param { get; }
        public MessageTypeEventArgs(byte channel,byte param)
        {
            Channel = channel;
            Param = param;
        }
    }
}
