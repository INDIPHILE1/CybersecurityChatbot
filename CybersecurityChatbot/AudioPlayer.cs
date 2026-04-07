using System;
using System.Media;
using System.IO;

namespace CybersecurityChatbot
{
    public class AudioPlayer
    {
        private string audioFilePath;

        public AudioPlayer()
        {
            audioFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
        }

        public void PlayGreeting()
        {
            try
            {
                if (File.Exists(audioFilePath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioFilePath))
                    {
                        player.PlaySync();
                    }
                }
                else
                {
                    Console.WriteLine("⚠️ Voice greeting file not found. Continuing...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing audio: {ex.Message}");
            }
        }
    }
}