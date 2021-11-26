using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrCodeValidatorApp.Models
{
    class AutoStartSettler : ISettler
    {
        public void Set()
        {
            try
            {
                const string applicationName = "system35";
                const string pathRegistryKeyStartup =
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

                using (RegistryKey registryKeyStartup =
                            Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup,
                    true))
                {
                    registryKeyStartup.SetValue(
                        applicationName,
                        string.Format("\"{0}\"",
                            System.Reflection.Assembly.GetExecutingAssembly().Location));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
