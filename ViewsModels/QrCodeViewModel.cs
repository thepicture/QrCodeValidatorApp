using QrCodeValidatorApp.Commands;
using QrCodeValidatorApp.Models;
using QrCodeValidatorApp.Services;
using System;
using System.Windows.Input;

namespace QrCodeValidatorApp.ViewsModels
{
    class QrCodeViewModel : ViewModelBase
    {
        private QrCode _currentQrCode;
        private bool _isQrCodeWriteMode = false;
        private string _qrCodeText;
        private int _attemptsLeft = 5;
        private readonly IMessageService _messageService;
        private readonly ISoundPlayService _soundPlayService;
        public QrCodeViewModel()
        {
            Title = "Госуслуги";
            SetQrCodeInputAsKeyboard = new RelayCommand(null,
                                                       SetQrCodeMode);
            CheckWrittenQrCode = new RelayCommand(IsWrittenQrCodeValid,
                                                  ShowInvalidAttemptError);
            _messageService = new MessageBoxService();
            _soundPlayService = new WavSoundPlayService();
        }

        private void ShowInvalidAttemptError(object obj)
        {
            AttemptsLeft--;
            if (AttemptsLeft > 0)
            {
                _soundPlayService.Play(Properties.Resources.badAttempt);
                _messageService.Show(string.Format("Не удалось прочитать QR-код. " +
                    "Пожалуйста, попробуйте ещё раз. {0}{0}" +
                    "Осталось попыток до блокировки системы: " +
                    $"{AttemptsLeft}", Environment.NewLine));
            }
        }

        private bool IsWrittenQrCodeValid(object arg)
        {
            return !string.IsNullOrWhiteSpace(QrCodeText) && AttemptsLeft > 0;
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

        public int AttemptsLeft
        {
            get => _attemptsLeft; set
            {
                _attemptsLeft = value;
                if (_attemptsLeft == 0)
                {
                    _soundPlayService.Play(Properties.Resources.fatalError);
                }
                OnPropertyChanged();
            }
        }
    }
}
