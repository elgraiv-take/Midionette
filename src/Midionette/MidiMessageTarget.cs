namespace Elgraiv.Midionette
{
    public struct MidiMessageTarget
    {
        public byte Channel { get; }
        public byte Param { get; }
        public MidiMessageTarget(byte channel,byte param)
        {
            Channel = channel;
            Param = param;
        }
    }
}
