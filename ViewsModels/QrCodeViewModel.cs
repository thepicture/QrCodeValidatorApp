using QrCodeValidatorApp.Commands;
using QrCodeValidatorApp.Models;
using QrCodeValidatorApp.Services;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace QrCodeValidatorApp.ViewsModels
{
    class QrCodeViewModel : ViewModelBase
    {
        private QrCode _currentQrCode;
        private bool _isQrCodeWriteMode = false;
        private string _qrCodeText;
        private int _attemptsLeft = 3;
        private readonly IMessageService _messageService;
        private readonly ISoundPlayService _soundPlayService;
        private bool _isAppRunning = true;
        private readonly DispatcherTimer _timer;
        public QrCodeViewModel()
        {
            Title = "Госуслуги";
            SetQrCodeInputAsKeyboard = new RelayCommand(null,
                                                       SetQrCodeMode);
            CheckWrittenQrCode = new RelayCommand(IsWrittenQrCodeValid,
                                                  ShowInvalidAttemptError);
            _messageService = new MessageBoxService();
            _soundPlayService = new WavSoundPlayService();
            CloseApp = new RelayCommand(null, CloseCurrentApp);
            DispatchCommandArgs();
        }

        private void CloseCurrentApp(object obj)
        {
            IsAppRunning = false;
        }

        private void DispatchCommandArgs()
        {
            try
            {
                if (HasCommandLineArgs())
                {
                    if (bool.Parse(Environment.GetCommandLineArgs()[1]))
                    {
                        ShowExploitAttemptMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ShowExploitAttemptMessage()
        {
            _messageService.Show("Обнаружена попытка " +
                                        "обхода системы проверки QR-кода. " +
                                        "Ваши действия отправляются заведующему отделением. " +
                                        Environment.NewLine +
                                        Environment.NewLine +
                                        "Дата нарушения: " +
                                        DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +
                                        Environment.NewLine +
                                        "Компьютер нарушителя: " +
                                        Environment.MachineName);
        }

        private static bool HasCommandLineArgs()
        {
            return Environment.GetCommandLineArgs().Length > 2
                && double.TryParse(
                    Environment.GetCommandLineArgs()[1],
                    out _)
                && bool.TryParse(
                    Environment.GetCommandLineArgs()[2],
                    out _);
        }

        private void OnCloseApp(object sender, EventArgs e)
        {
            IsAppRunning = true;
            (sender as DispatcherTimer).Stop();
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
        public ICommand CloseApp { get; set; }

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

        public bool IsAppRunning
        {
            get => _isAppRunning; set
            {
                _isAppRunning = value;
                foreach (System.Windows.Window window in App.Current.Windows)
                {
                    if (_isAppRunning)
                    {
                        window.Show();
                        _soundPlayService.Play(Properties.Resources.greetingSound);
                    }
                    else
                    {
                        window.Hide();
                    }
                }
                OnPropertyChanged();
            }
        }
    }
}
