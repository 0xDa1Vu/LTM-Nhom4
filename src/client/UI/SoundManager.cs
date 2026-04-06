using System.Media;
using System.Windows.Forms;

namespace CoTuongOnline.Client
{
    public static class SoundManager
    {
        private static string basePath = Application.StartupPath + "\\Sounds\\";

        public static void PlayMove()
        {
            SoundPlayer player = new SoundPlayer(basePath + "move.wav");
            player.Play();
        }

        public static void PlayCapture()
        {
            SoundPlayer player = new SoundPlayer(basePath + "capture.wav");
            player.Play();
        }
    }
}