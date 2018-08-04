using System;

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
