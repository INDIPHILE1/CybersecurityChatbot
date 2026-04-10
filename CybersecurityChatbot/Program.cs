using System;

namespace CybersecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.SetWindowSize(100, 40);

            // Create objects (STEP 7: Proper code structure)
            AudioPlayer audioPlayer = new AudioPlayer();
            Chatbot bot = new Chatbot();
            User user = new User();

            // STEP 2: Display ASCII art
            bot.DisplayAsciiArt();

            // STEP 1: Play voice greeting
            Console.WriteLine();
            bot.WriteColored("[🔊] Playing voice greeting...\n", ConsoleColor.Yellow);
            audioPlayer.PlayGreeting();

            // STEP 3 & 6: Welcome message with border
            Console.WriteLine();
            bot.DrawBorder("WELCOME");
            Console.WriteLine();
            bot.TypeText("Hello! Welcome to the Cybersecurity Awareness Bot.", 30);
            bot.TypeText("I'm here to help you stay safe online.", 30);
            Console.WriteLine();
            bot.DrawBottomBorder();

            // STEP 3 & 5: Get user name with validation
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

            // STEP 4: Show available topics
            Console.WriteLine();
            bot.DrawBorder("WHAT I CAN HELP WITH");
            bot.TypeText("• Password safety tips", 25);
            bot.TypeText("• How to spot phishing scams", 25);
            bot.TypeText("• Safe browsing habits", 25);
            bot.TypeText("• Two-Factor Authentication (2FA)", 25);
            bot.TypeText("• Protection against viruses", 25);
            bot.DrawBottomBorder();

            Console.WriteLine();
            bot.TypeText("Try asking me: 'What is phishing?' or 'How do I create a strong password?'", 30);
            bot.TypeText("Type 'exit' to quit.\n", 30);

            // STEP 4,5,6: Main chat loop
            bool running = true;

            while (running)
            {
                bot.WriteColored($"{user.Name}> ", ConsoleColor.Green);
                string input = Console.ReadLine();

                // Check for exit command
                if (input != null && (input.ToLower() == "exit" || input.ToLower() == "quit" || input.ToLower() == "bye"))
                {
                    Console.WriteLine();
                    bot.DrawBorder("GOODBYE");
                    bot.TypeText($"Thanks for learning about cybersecurity, {user.Name}! Stay safe online! 👋", 30);
                    bot.TypeText($"You asked {user.QuestionsAsked} questions during this session.", 30);
                    bot.DrawBottomBorder();
                    running = false;
                    continue;
                }

                // Process input and get response
                string response = bot.ProcessUserInput(input);

                // Display response if not empty
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