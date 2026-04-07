using System;
using System.Collections.Generic;
using System.Threading;

namespace CybersecurityChatbot
{
    public class Chatbot
    {
        private Dictionary<string, string> responses;

        public Chatbot()
        {
            LoadResponses();
        }

        private void LoadResponses()
        {
            responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Password safety
                { "password", "🔐 PASSWORD SAFETY:\n• Use at least 12 characters\n• Mix uppercase, lowercase, numbers, and symbols\n• Never reuse passwords across sites\n• Use a password manager\n• Enable 2FA whenever possible" },
                { "strong password", "A strong password is at least 12 characters long and includes uppercase letters, lowercase letters, numbers, and special symbols!" },
                
                // Phishing
                { "phishing", "🎣 PHISHING SCAMS:\n• Never click suspicious links in emails\n• Check sender email addresses carefully\n• Look for spelling errors in URLs\n• Don't share personal info via email\n• When in doubt, contact the company directly" },
                { "what is phishing", "Phishing is when scammers try to trick you into giving personal information by pretending to be a legitimate company!" },
                
                // Safe browsing
                { "safe browsing", "🌐 SAFE BROWSING HABITS:\n• Look for 'https://' in website URLs\n• Keep your browser updated\n• Don't download files from unknown sources\n• Use ad blockers\n• Clear cookies regularly" },
                
                // 2FA
                { "2fa", "🔑 TWO-FACTOR AUTHENTICATION (2FA):\n• Adds an extra security layer\n• Requires password + second verification (code, fingerprint, etc.)\n• Use authenticator apps (Google Authenticator, Authy)\n• Avoid SMS 2FA when possible" },
                { "two-factor authentication", "2FA requires two different verification methods - something you know (password) and something you have (phone, token, etc.)!" },
                
                // Malware/Viruses
                { "virus", "🛡️ VIRUS PROTECTION:\n• Install reputable antivirus software\n• Keep it updated\n• Run regular scans\n• Don't open suspicious email attachments\n• Download only from official sources" },
                { "malware", "Malware is malicious software designed to damage or infiltrate your computer. Protect yourself with antivirus and careful browsing!" },
                
                // Basic questions
                { "how are you", "I'm doing great! Thanks for asking! Ready to help you learn about cybersecurity! 😊" },
                { "what is your purpose", "My purpose is to educate you about online safety and protect you from cyber threats! I'm your personal cybersecurity guide." },
                { "what can i ask you about", "You can ask me about:\n• Password safety\n• Phishing attacks\n• Safe browsing habits\n• Two-Factor Authentication (2FA)\n• Virus/Malware protection" }
            };
        }

        public void DisplayAsciiArt()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ╔════════════════════════════════════════════════════════════════════════╗
    ║                                                                            ║
    ║     ██████╗██╗   ██╗██████╗ ███████╗██████╗ ███████╗██╗   ██╗██████╗       ║
    ║    ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔════╝╚██╗ ██╔╝██╔══██╗      ║
    ║    ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝███████╗ ╚████╔╝ ██████╔╝      ║
    ║    ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗╚════██║  ╚██╔╝  ██╔══██╗      ║
    ║    ╚██████╗   ██║   ██████╔╝███████╗██║  ██║███████║   ██║   ██████╔╝      ║
    ║     ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝   ╚═╝   ╚═════╝       ║
    ║                                                                            ║
    ║                    🔒  CYBERSECURITY AWARENESS BOT  🔒                     ║
    ║                                                                            ║
    ╚════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void DrawBorder(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', title.Length + 2) + "╗");
            Console.WriteLine($"║ {title} ║");
            Console.WriteLine("╚" + new string('═', title.Length + 2) + "╝");
            Console.ResetColor();
        }

        public void DrawBottomBorder()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('─', 50));
            Console.ResetColor();
        }

        public void WriteColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public void TypeText(string text, int delayMs)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs);
            }
            Console.WriteLine();
        }

        public string ProcessUserInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "I didn't catch that. Could you please say something? 🤔";
            }

            foreach (var keyword in responses.Keys)
            {
                if (input.ToLower().Contains(keyword.ToLower()))
                {
                    return responses[keyword];
                }
            }

            return "I'm not sure about that. Try asking about passwords, phishing, safe browsing, or 2FA! Type 'help' for more options.";
        }

        public void DisplayHelp()
        {
            DrawBorder("HELP - WHAT I CAN DO");
            Console.WriteLine();
            TypeText("💬 BASIC QUESTIONS:", 20);
            TypeText("   • 'How are you?'", 20);
            TypeText("   • 'What is your purpose?'", 20);
            TypeText("   • 'What can I ask you about?'", 20);
            Console.WriteLine();
            TypeText("🔐 CYBERSECURITY TOPICS:", 20);
            TypeText("   • 'What is phishing?'", 20);
            TypeText("   • 'How do I create a strong password?'", 20);
            TypeText("   • 'Safe browsing tips'", 20);
            TypeText("   • 'What is 2FA?'", 20);
            TypeText("   • 'How to protect against viruses?'", 20);
            Console.WriteLine();
            TypeText("🎮 COMMANDS:", 20);
            TypeText("   • 'help' - Show this menu", 20);
            TypeText("   • 'exit', 'quit', 'bye' - End conversation", 20);
            DrawBottomBorder();
        }
    }
}