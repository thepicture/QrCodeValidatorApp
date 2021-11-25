using QrCodeValidatorApp.Models;

namespace QrCodeValidatorApp.ViewsModels
{
    class QrCodeViewModel : ViewModelBase
    {
        private QrCode _currentQrCode;
        public QrCodeViewModel()
        {
            Title = "Госуслуги";
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
