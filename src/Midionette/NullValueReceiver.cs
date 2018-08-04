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
