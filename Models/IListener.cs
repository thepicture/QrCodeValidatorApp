namespace QrCodeValidatorApp.Models
{
    public interface IListener
    {
        void StartListening();
        void StopListening();
    }
}
