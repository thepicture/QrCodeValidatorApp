namespace QrCodeValidatorApp.Services
{
    public class MessageBoxService : IMessageService
    {
        public void Show(string text)
        {
            System.Windows.MessageBox.Show(text,
                                           "Информация",
                                           System.Windows.MessageBoxButton.OK,
                                           System.Windows.MessageBoxImage.Information,
                                           System.Windows.MessageBoxResult.OK,
                                           System.Windows.MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
