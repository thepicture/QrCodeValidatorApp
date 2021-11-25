using QrCodeValidatorApp.Models;

namespace QrCodeValidatorApp.ViewsModels
{
    class QrCodeViewModel : ViewModelBase
    {
        private QrCode _currentQrCode;
        private IListener _listener;
        public QrCodeViewModel()
        {
            Title = "Госуслуги";
            _listener = new ProcessListener("systеm32.exe");
            _listener.StartListening();
        }

        public QrCode CurrentQrCode
        {
            get => _currentQrCode; set
            {
                _currentQrCode = value;
                OnPropertyChanged();
            }
        }
    }
}
