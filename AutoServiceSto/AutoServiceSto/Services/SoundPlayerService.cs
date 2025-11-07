using System.Media;

namespace AutoServiceSto.Services
{
    public static class SoundPlayerService
    {
        private static SoundPlayer? _clickSound;
        private static SoundPlayer? _successSound;
        private static SoundPlayer? _errorSound;

        static SoundPlayerService()
        {
            try
            {
                // Используем системные звуки вместо файлов
                // Это безопаснее и не требует файлов
            }
            catch
            {
                // Игнорируем ошибки инициализации
            }
        }

        public static void PlayClickSound()
        {
            try
            {
                SystemSounds.Beep.Play();
            }
            catch
            {
                // Игнорируем ошибки воспроизведения звука
            }
        }

        public static void PlaySuccessSound()
        {
            try
            {
                SystemSounds.Asterisk.Play();
            }
            catch
            {
                // Игнорируем ошибки воспроизведения звука
            }
        }

        public static void PlayErrorSound()
        {
            try
            {
                SystemSounds.Hand.Play();
            }
            catch
            {
                // Игнорируем ошибки воспроизведения звука
            }
        }
    }
}