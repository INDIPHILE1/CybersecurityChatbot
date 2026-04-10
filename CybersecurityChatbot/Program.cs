using System;

namespace CybersecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.SetWindowSize(100, 40);

            AudioPlayer audioPlayer = new AudioPlayer();
            Chatbot bot = new Chatbot();
            User user = new User();

            bot.DisplayAsciiArt();

            Console.WriteLine();
            bot.WriteColored("[🔊] Playing voice greeting...\n", ConsoleColor.Yellow);
            audioPlayer.PlayGreeting();

            Console.WriteLine();
            bot.DrawBorder("WELCOME");
            Console.WriteLine();
            bot.TypeText("Hello! Welcome to the Cybersecurity Awareness Bot.", 30);
            bot.TypeText("I'm here to help you stay safe online.", 30);
            Console.WriteLine();
            bot.DrawBottomBorder();

            Console.WriteLine();
            bot.WriteColored("What is your name? ", ConsoleColor.Green);
            string userName = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(userName))
            {
                bot.WriteColored("Please enter a valid name: ", ConsoleColor.Green);
                userName = Console.ReadLine();
            }
            user.SetName(userName);
            Console.WriteLine();
            bot.TypeText($"Nice to meet you, {user.Name}!", 30);
            bot.TypeText($"Welcome, {user.Name}! I'm your personal cybersecurity guide.", 30);

            Console.WriteLine();
            bot.DrawBorder("WHAT I CAN HELP WITH");
            bot.TypeText("• Password safety tips", 25);
            bot.TypeText("• How to spot phishing scams", 25);
            bot.TypeText("• Safe browsing habits", 25);
            bot.DrawBottomBorder();

            Console.WriteLine();
            bot.TypeText("Try asking me: 'What is phishing?' or 'password safety'", 30);
            bot.TypeText("Type 'exit' to quit.\n", 30);

            bool running = true;
            while (running)
            {
                bot.WriteColored($"{user.Name}> ", ConsoleColor.Green);
                string input = Console.ReadLine();
                if (input != null && (input.ToLower() == "exit" || input.ToLower() == "quit"))
                {
                    Console.WriteLine();
                    bot.DrawBorder("GOODBYE");
                    bot.TypeText($"Thanks for learning, {user.Name}! Stay safe online! 👋", 30);
                    bot.DrawBottomBorder();
                    running = false;
                    continue;
                }
                string response = bot.ProcessUserInput(input);
                if (!string.IsNullOrEmpty(response))
                {
                    bot.WriteColored("🤖 CyberBot> ", ConsoleColor.Cyan);
                    bot.TypeText(response, 25);
                    Console.WriteLine();
                }
                user.IncrementQuestions();
            }
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}