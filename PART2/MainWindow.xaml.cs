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
        // ----- FIELDS -----
        private User currentUser;
        private ChatbotEngine chatbot;
        private string audioPath;
        private ActivityLogger logger;
        private TaskManager taskManager;
        private QuizManager quizManager;

        // ----- CONSTRUCTOR -----
        public MainWindow()
        {
            InitializeComponent();

            // Initialize user
            currentUser = new User();

            // ===== INITIALIZE PART 3 OBJECTS (MUST BE DONE FIRST) =====
            logger = new ActivityLogger();
            taskManager = new TaskManager(logger);
            quizManager = new QuizManager(logger);

            // Pass dependencies to ChatbotEngine
            chatbot = new ChatbotEngine(currentUser, logger, taskManager);

            // Audio path
            audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"C:\Users\Student\Downloads\greeting.wav");

            // Load tasks from database
            LoadTasks();

            // Play voice greeting
            PlayVoiceGreeting();

            // Initial chat messages
            AppendToChat("System", "🔊 Welcome to LearnMate! I'm your AI cybersecurity assistant.");
            AppendToChat("Bot", "Please click 'Enter Your Name' and tell me your name.");
        }

        // ----- VOICE GREETING -----
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
                    AppendToChat("System", "✅ Voice greeting played.");
                }
                else
                {
                    AppendToChat("System", "⚠️ Greeting.wav not found.");
                }
            }
            catch (Exception ex)
            {
                AppendToChat("System", $"❌ Audio error: {ex.Message}");
            }
        }

        // ----- CHAT METHODS -----
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
                logger.Log($"User registered: {currentUser.Name}");
            }
            else
            {
                AppendToChat("Bot", "Name cannot be empty. Please click the button again.");
            }
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ProcessUserMessage();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserMessage();
        }

        private void ProcessUserMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
                return;

            string userMessage = MessageTextBox.Text.Trim();
            AppendToChat(currentUser.Name ?? "You", userMessage);

            if (string.IsNullOrEmpty(currentUser.Name))
            {
                AppendToChat("Bot", "Please click 'Enter Your Name' first.");
                MessageTextBox.Clear();
                return;
            }

            string botResponse = chatbot.ProcessInput(userMessage);
            AppendToChat("Bot", botResponse);
            MessageTextBox.Clear();
        }

        private void AppendToChat(string sender, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            ChatHistoryListBox.Items.Add($"[{timestamp}] {sender}: {message}");
            if (ChatHistoryListBox.Items.Count > 0)
                ChatHistoryListBox.ScrollIntoView(ChatHistoryListBox.Items[ChatHistoryListBox.Items.Count - 1]);
        }

        // ===== TAB NAVIGATION =====
        private void ChatTab_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Visible;
            TasksPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Collapsed;
        }

        private void TasksTab_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TasksPanel.Visibility = Visibility.Visible;
            QuizPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Collapsed;
            LoadTasks();
        }

        private void QuizTab_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TasksPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Visible;
            LogPanel.Visibility = Visibility.Collapsed;

            if (quizManager != null && quizManager.GetTotalQuestions() == 0)
            {
                QuizQuestionText.Text = "Click 'Start New Quiz' to begin testing your cybersecurity knowledge!";
                QuizStartButton.Visibility = Visibility.Visible;
            }
        }

        private void LogTab_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TasksPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Visible;
            RefreshLog();
        }

        // ===== TASK METHODS =====
        private void LoadTasks()
        {
            try
            {
                if (taskManager == null)
                {
                    TaskListBox.Items.Add("Error: TaskManager not initialized");
                    return;
                }

                var tasks = taskManager.GetAllTasks();
                TaskListBox.Items.Clear();

                if (tasks.Count == 0)
                {
                    TaskListBox.Items.Add("No tasks found. Add a task to get started!");
                    return;
                }

                foreach (var task in tasks)
                {
                    string status = task.IsComplete ? "✅" : "⏳";
                    TaskListBox.Items.Add($"{status} ID:{task.Id} - {task.Title} (Reminder: {task.Reminder})");
                }
            }
            catch (Exception ex)
            {
                TaskListBox.Items.Add($"Error loading tasks: {ex.Message}");
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = TaskTitleBox.Text.Trim();
                string description = TaskDescriptionBox.Text.Trim();
                string reminder = TaskReminderBox.Text.Trim();

                if (string.IsNullOrEmpty(title))
                {
                    MessageBox.Show("Please enter a task title.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string result = taskManager.AddTask(title, description, reminder);
                AppendToChat("Bot", result);
                LoadTasks();
                TaskTitleBox.Clear();
                TaskDescriptionBox.Clear();
                TaskReminderBox.Clear();
                logger.Log($"Task added via UI: {title}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a task to complete.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var tasks = taskManager.GetAllTasks();
                if (TaskListBox.SelectedIndex < tasks.Count)
                {
                    var task = tasks[TaskListBox.SelectedIndex];
                    string result = taskManager.MarkAsComplete(task.Id);
                    AppendToChat("Bot", result);
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error completing task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a task to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var tasks = taskManager.GetAllTasks();
                if (TaskListBox.SelectedIndex < tasks.Count)
                {
                    var task = tasks[TaskListBox.SelectedIndex];
                    string result = taskManager.DeleteTask(task.Id);
                    AppendToChat("Bot", result);
                    LoadTasks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
            AppendToChat("System", "🔄 Task list refreshed.");
        }

        // ===== QUIZ METHODS =====
        private void QuizStartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (quizManager == null)
                {
                    MessageBox.Show("Quiz manager not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                quizManager.ResetQuiz();
                QuizStartButton.Visibility = Visibility.Collapsed;
                QuizSubmitButton.Visibility = Visibility.Visible;
                QuizSubmitButton.IsEnabled = true;
                QuizNextButton.Visibility = Visibility.Collapsed;
                QuizResetButton.Visibility = Visibility.Collapsed;
                QuizFeedbackText.Text = "";
                ShowQuizQuestion();
                logger.Log("Quiz started");
                AppendToChat("Bot", "🎯 Quiz started! Answer the questions to test your cybersecurity knowledge!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowQuizQuestion()
        {
            try
            {
                if (quizManager == null)
                {
                    QuizQuestionText.Text = "Quiz manager not available.";
                    return;
                }

                var question = quizManager.GetCurrentQuestion();
                if (question == null)
                {
                    ShowQuizResults();
                    return;
                }

                QuizProgressText.Text = $"Question {quizManager.GetCurrentIndex() + 1} of {quizManager.GetTotalQuestions()}";
                QuizQuestionText.Text = question.Question;
                QuizOptionsPanel.Children.Clear();

                string[] optionLabels = { "A", "B", "C", "D" };
                for (int i = 0; i < question.Options.Count; i++)
                {
                    var rb = new System.Windows.Controls.RadioButton
                    {
                        Content = $"{optionLabels[i]}) {question.Options[i]}",
                        Tag = optionLabels[i],
                        Margin = new Thickness(5, 2, 0, 2),
                        FontSize = 14,
                        GroupName = "QuizOptions"
                    };
                    QuizOptionsPanel.Children.Add(rb);
                }

                QuizScoreText.Text = $"Score: {quizManager.GetFinalScore()}";
            }
            catch (Exception ex)
            {
                QuizQuestionText.Text = $"Error loading question: {ex.Message}";
            }
        }

        private void QuizSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (quizManager == null)
                {
                    MessageBox.Show("Quiz manager not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (quizManager.IsFinished())
                {
                    ShowQuizResults();
                    return;
                }

                string selectedAnswer = null;
                foreach (var child in QuizOptionsPanel.Children)
                {
                    if (child is System.Windows.Controls.RadioButton rb && rb.IsChecked == true)
                    {
                        selectedAnswer = rb.Tag.ToString();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(selectedAnswer))
                {
                    MessageBox.Show("Please select an answer.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool isCorrect = quizManager.SubmitAnswer(selectedAnswer);
                string feedback = quizManager.GetFeedback(isCorrect);
                QuizFeedbackText.Text = feedback;
                QuizScoreText.Text = $"Score: {quizManager.GetFinalScore()}";

                QuizSubmitButton.IsEnabled = false;
                QuizNextButton.Visibility = Visibility.Visible;

                logger.Log($"Quiz answered: {selectedAnswer} - {(isCorrect ? "Correct" : "Incorrect")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting answer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void QuizNextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                QuizNextButton.Visibility = Visibility.Collapsed;
                QuizSubmitButton.IsEnabled = true;
                QuizFeedbackText.Text = "";

                if (quizManager.IsFinished())
                {
                    ShowQuizResults();
                    return;
                }

                ShowQuizQuestion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading next question: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void QuizResetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (quizManager == null)
                {
                    MessageBox.Show("Quiz manager not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                quizManager.ResetQuiz();
                QuizResetButton.Visibility = Visibility.Collapsed;
                QuizSubmitButton.Visibility = Visibility.Visible;
                QuizSubmitButton.IsEnabled = true;
                QuizNextButton.Visibility = Visibility.Collapsed;
                QuizFeedbackText.Text = "";
                QuizScoreText.Text = "";
                QuizOptionsPanel.Children.Clear();
                ShowQuizQuestion();
                logger.Log("Quiz reset");
                AppendToChat("Bot", "🔄 Quiz reset. Start again when you're ready!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowQuizResults()
        {
            try
            {
                if (quizManager == null)
                {
                    QuizQuestionText.Text = "Quiz manager not available.";
                    return;
                }

                QuizQuestionText.Text = "🎉 Quiz Complete!";
                QuizOptionsPanel.Children.Clear();
                QuizFeedbackText.Text = "";
                QuizScoreText.Text = $"Final Score: {quizManager.GetFinalScore()}";
                QuizSubmitButton.Visibility = Visibility.Collapsed;
                QuizNextButton.Visibility = Visibility.Collapsed;
                QuizResetButton.Visibility = Visibility.Visible;
                QuizProgressText.Text = "Quiz Completed!";
                logger.Log($"Quiz completed - Score: {quizManager.GetFinalScore()}");

                MessageBox.Show(quizManager.GetFinalMessage(), "Quiz Results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing results: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ===== ACTIVITY LOG METHODS =====
        private void RefreshLog()
        {
            try
            {
                if (logger == null)
                {
                    LogListBox.Items.Add("Error: Logger not initialized");
                    return;
                }

                var logs = logger.GetRecentLogs(10);
                LogListBox.Items.Clear();

                if (logs.Count == 0)
                {
                    LogListBox.Items.Add("No activity logged yet.");
                    ShowMoreLogButton.Visibility = Visibility.Collapsed;
                    return;
                }

                foreach (var log in logs)
                {
                    LogListBox.Items.Add(log);
                }

                if (logger.GetCount() > 10)
                    ShowMoreLogButton.Visibility = Visibility.Visible;
                else
                    ShowMoreLogButton.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                LogListBox.Items.Add($"Error loading log: {ex.Message}");
            }
        }

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshLog();
            AppendToChat("System", "🔄 Log refreshed.");
        }

        private void ShowMoreLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (logger == null)
                {
                    LogListBox.Items.Add("Error: Logger not initialized");
                    return;
                }

                var allLogs = logger.GetAllLogs();
                LogListBox.Items.Clear();
                foreach (var log in allLogs)
                    LogListBox.Items.Add(log);

                ShowMoreLogButton.Visibility = Visibility.Collapsed;
                AppendToChat("System", "📖 Showing all log entries.");
            }
            catch (Exception ex)
            {
                LogListBox.Items.Add($"Error showing all logs: {ex.Message}");
            }
        }
    }
}