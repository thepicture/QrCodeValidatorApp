using System.IO;
using System.Media;

namespace QrCodeValidatorApp.Services
{
    public class WavSoundPlayService : ISoundPlayService
    {
        private readonly SoundPlayer _soundPlayer;
        public WavSoundPlayService()
        {
            _soundPlayer = new SoundPlayer();
        }

        public void Play(object sound)
        {
            _soundPlayer.Stream = (UnmanagedMemoryStream)sound;
            try
            {
                _soundPlayer.Play();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }
    }
}
