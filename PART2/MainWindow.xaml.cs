using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualBasic;

namespace POEPART2
{
    public partial class MainWindow : Window
    {
        private User currentUser;
        private ChatbotEngine chatbot;
        private string audioPath;  

        public MainWindow()
        {
            InitializeComponent();
            currentUser = new User();
            chatbot = new ChatbotEngine(currentUser);
            audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"C:\Users\Student\Downloads\greeting.wav");
            PlayVoiceGreeting();
            AppendToChat("System", "🔊 Welcome to LearnMate! I'm your AI cybersecurity assistant.");
            AppendToChat("Bot", "Please click 'Enter Your Name' and tell me your name.");
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.PlaySync();
                    }
                }
                else
                {
                    MessageBox.Show("Greeting audio file not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Audio error: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(currentUser.Name))
            {
                AppendToChat("Bot", $"Your name is already {currentUser.Name}. Type your message.");
                return;
            }
            string name = Interaction.InputBox("Please enter your name:", "LearnMate - Name", "");
            if (!string.IsNullOrWhiteSpace(name))
            {
                currentUser.Name = name.Trim();
                AppendToChat("Bot", $"Nice to meet you, {currentUser.Name}! I'll remember that.");
                AppendToChat("Bot", "You can ask me about passwords, phishing, privacy, scams, or 2FA.");
            }
            else AppendToChat("Bot", "Name cannot be empty. Please click the button again.");
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ProcessUserMessage();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserMessage();
        }

        private void ProcessUserMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageTextBox.Text)) return;
            string userMessage = MessageTextBox.Text.Trim();
            AppendToChat(currentUser.Name ?? "You", userMessage);
            if (string.IsNullOrEmpty(currentUser.Name))
            {
                AppendToChat("Bot", "Please click 'Enter Your Name' first.");
                MessageTextBox.Clear();
                return;
            }
            string botResponse = chatbot.GetResponse(userMessage);
            AppendToChat("Bot", botResponse);
            MessageTextBox.Clear();
        }

        private void AppendToChat(string sender, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");

            ChatHistoryListBox.Items.Add(
                $"[{timestamp}] {sender}: {message}"
            );

            ChatHistoryListBox.ScrollIntoView(
                ChatHistoryListBox.Items[ChatHistoryListBox.Items.Count - 1]
            );
        }
    }

}