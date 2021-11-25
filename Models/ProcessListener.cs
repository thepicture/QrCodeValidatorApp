using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace QrCodeValidatorApp.Models
{
    public class ProcessListener : IListener
    {
        private readonly string _fileName;
        private readonly DispatcherTimer _watcher;
        private Process _currentProcess;

        public ProcessListener(string fileName)
        {
            _fileName = fileName;
            _watcher = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _watcher.Tick += KeepChildProcess;
        }

        private void KeepChildProcess(object sender, EventArgs e)
        {
            if (_currentProcess.HasExited)
            {
                InitializeChildProcess();
            }
        }

        public void StartListening()
        {
            InitializeChildProcess();
        }

        private void InitializeChildProcess()
        {
            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName));
        }

        public void StopListening()
        {
            _currentProcess.Kill();
        }
    }
}
