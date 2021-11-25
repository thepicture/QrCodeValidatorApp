using QrCodeValidatorApp.Commands;
using QrCodeValidatorApp.Models;
using System;
using System.Windows.Input;

namespace QrCodeValidatorApp.ViewsModels
{
    class QrCodeViewModel : ViewModelBase
    {
        private QrCode _currentQrCode;
        private bool _isQrCodeWriteMode = false;
        private string _qrCodeText;
        public QrCodeViewModel()
        {
            Title = "Госуслуги";
            SetQrCodeInputAsKeyboard = new RelayCommand(null,
                                                       SetQrCodeMode);
            CheckWrittenQrCode = new RelayCommand(IsWrittenQrCodeValid, ShowInvalidAttemptError);
        }

        private void ShowInvalidAttemptError(object obj)
        {
            throw new NotImplementedException();
        }

        private bool IsWrittenQrCodeValid(object arg)
        {
            return !string.IsNullOrWhiteSpace(QrCodeText);
        }

        private void SetQrCodeMode(object parameter)
        {
            IsQrCodeInWriteMode = Convert.ToBoolean(parameter);
        }

        public QrCode CurrentQrCode
        {
            get => _currentQrCode; set
            {
                _currentQrCode = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetQrCodeInputAsKeyboard { get; set; }
        public ICommand CheckWrittenQrCode { get; set; }

        public bool IsQrCodeInWriteMode
        {
            get => _isQrCodeWriteMode; set
            {
                _isQrCodeWriteMode = value;
                OnPropertyChanged();
            }
        }

        public string QrCodeText
        {
            get => _qrCodeText; set
            {
                _qrCodeText = value;
                OnPropertyChanged();
            }
        }
    }
}
