namespace Elgraiv.Midionette
{
    public interface IKeyReceiver
    {
        string Name { get; }
        
        void OnKeyDown(byte velocity);
        void OnKeyUp(byte velocity);
    }
}
