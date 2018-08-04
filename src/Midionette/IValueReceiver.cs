namespace Elgraiv.Midionette
{
    public interface IValueReceiver
    {
        string Name { get; }

        void OnValueReceived(byte value);
    }
}
