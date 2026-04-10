using System;
using System.Media;
using System.IO;

namespace CybersecurityChatbot
{
    public class AudioPlayer
    {
        private string audioPath;

        public AudioPlayer()
        {
            audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Greeting.wav");
        }

        public void PlayGreeting()
        {
            try
            {
                if (File.Exists(audioPath))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n🔊 Audio path: {audioPath}");
                    Console.WriteLine("Playing voice greeting...");
                    Console.ResetColor();

                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.PlaySync();
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✅ Voice greeting played successfully!\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n⚠️ Warning: Greeting.wav not found");
                    Console.WriteLine($"📁 Expected location: {audioPath}");
                    Console.WriteLine("ℹ️ Voice greeting will be skipped until you add the file.\n");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error playing audio: {ex.Message}\n");
                Console.ResetColor();
            }
        }
    }
}