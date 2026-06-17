using System;
using System.Collections.Generic;

namespace POEPART2
{
    public class ChatbotEngine
    {
        private User currentUser;
        private string lastTopic = "";
        private int followUpCount = 0;

        private Dictionary<string, string> primaryResponses;
        private Dictionary<string, List<string>> randomResponses;
        private Dictionary<string, string> sentimentResponses;
        private ActivityLogger _logger;
        private TaskManager _taskManager;

        // ===== CONSTRUCTORS =====
        public ChatbotEngine(User user)
        {
            currentUser = user;
            LoadPrimaryResponses();
            LoadRandomResponses();
            LoadSentimentResponses();
        }

        public ChatbotEngine(User user, ActivityLogger logger, TaskManager taskManager) : this(user)
        {
            _logger = logger;
            _taskManager = taskManager;
        }

        // ===== LOAD RESPONSES =====
        private void LoadPrimaryResponses()
        {
            primaryResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", "🔐 Passwords should be at least 12 characters with uppercase, lowercase, numbers, and symbols. Never reuse passwords." },
                { "scam", "⚠️ Scams often create urgency. Always verify unexpected messages through official channels before acting." },
                { "privacy", "🛡️ Privacy is about controlling who sees your data. Review app permissions and use privacy settings on social media." },
                { "phishing", "🎣 Phishing emails may have spelling errors, mismatched URLs, or requests for personal info. Don't click suspicious links." },
                { "2fa", "🔑 Two-factor authentication adds an extra layer of security. Use an authenticator app instead of SMS when possible." },
                { "malware", "🦠 Malware includes viruses, ransomware, spyware. Keep antivirus updated and avoid unknown downloads." }
            };
        }

        private void LoadRandomResponses()
        {
            randomResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "phishing tip", new List<string> {
                    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                    "Hover over links before clicking to see the actual URL. If it looks suspicious, don't click.",
                    "Legitimate companies will never ask for your password via email. When in doubt, call them directly.",
                    "Check the sender's email address carefully – scammers use addresses that look similar to real ones."
                }},
                { "password tip", new List<string> {
                    "Use a password manager to generate and store unique passwords for each account.",
                    "Enable two-factor authentication wherever possible – it adds an extra layer of protection.",
                    "Avoid using personal details like your name or birthday in passwords.",
                    "Change passwords immediately if you suspect a breach."
                }},
                { "safe browsing tip", new List<string> {
                    "Look for 'https://' and a padlock icon in the address bar before entering sensitive information.",
                    "Keep your browser and extensions updated to protect against known vulnerabilities.",
                    "Use private browsing mode when using public computers, and clear your history afterwards.",
                    "Install an ad blocker to reduce exposure to malicious advertisements."
                }}
            };
        }

        private void LoadSentimentResponses()
        {
            sentimentResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "worried", "It's completely understandable to feel worried. Online threats can be scary. Let me give you some practical tips to help you feel more in control." },
                { "curious", "That's great! Being curious about cybersecurity is the first step to staying safe. Let me share something useful with you." },
                { "frustrated", "I hear you. It can be frustrating when things aren't clear. Let me try to explain in a simpler way." },
                { "scared", "You're not alone. Many people feel the same way. The good news is that with a few simple habits, you can protect yourself effectively." }
            };
        }

        // ===== SENTIMENT DETECTION =====
        public string DetectSentiment(string userInput)
        {
            string lower = userInput.ToLower();
            if (lower.Contains("worried") || lower.Contains("nervous") || lower.Contains("anxious"))
                return "worried";
            if (lower.Contains("curious") || lower.Contains("interested") || lower.Contains("tell me about"))
                return "curious";
            if (lower.Contains("frustrated") || lower.Contains("annoying") || lower.Contains("confusing"))
                return "frustrated";
            if (lower.Contains("scared") || lower.Contains("afraid") || lower.Contains("panic"))
                return "scared";
            return null;
        }

        // ===== PART 3: NLP PROCESS INPUT =====
        public string ProcessInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please type a message. I'm here to help!";

            string lower = userInput.ToLower();

            // Step 1: Check for Activity Log intent
            if (IsLogRequest(lower))
            {
                _logger?.Log("NLP recognised log request");
                return GetActivityLog();
            }

            // Step 2: Check for Task intent
            if (IsTaskRequest(lower))
            {
                string taskName = ExtractTaskName(userInput);
                _logger?.Log($"NLP recognised task intent: '{taskName}'");
                return AddTaskFromNLP(taskName);
            }

            // Step 3: Check for Reminder intent
            if (IsReminderRequest(lower))
            {
                string reminderText = ExtractReminderText(userInput);
                _logger?.Log($"NLP recognised reminder: '{reminderText}'");
                return SetReminderFromNLP(reminderText);
            }

            // Step 4: Check for Quiz intent
            if (IsQuizRequest(lower))
            {
                _logger?.Log("NLP recognised quiz request");
                return "🎯 Great! Let's start the cybersecurity quiz. Click the 'Quiz' tab in the menu or type 'start quiz' again!";
            }

            // Step 5: Fall through to existing Part 2 logic
            return GetResponse(userInput);
        }

        // ===== PART 3: INTENT DETECTION =====
        private bool IsLogRequest(string input)
        {
            return input.Contains("show activity log") ||
                   input.Contains("what have you done") ||
                   input.Contains("what did you do") ||
                   input.Contains("show log") ||
                   input.Contains("recent actions") ||
                   input.Contains("activity log") ||
                   input.Contains("log");
        }

        private bool IsTaskRequest(string input)
        {
            return input.Contains("add task") ||
                   input.Contains("add a task") ||
                   input.Contains("create task") ||
                   input.Contains("new task") ||
                   input.Contains("enable") ||
                   input.Contains("set up") ||
                   input.Contains("need to") ||
                   input.Contains("i should");
        }

        private bool IsReminderRequest(string input)
        {
            return input.Contains("remind me") ||
                   input.Contains("set a reminder") ||
                   input.Contains("reminder") ||
                   input.Contains("don't forget") ||
                   input.Contains("remember to");
        }

        private bool IsQuizRequest(string input)
        {
            return input.Contains("start quiz") ||
                   input.Contains("take quiz") ||
                   input.Contains("test my knowledge") ||
                   input.Contains("quiz me") ||
                   input.Contains("play the game") ||
                   input.Contains("let's quiz") ||
                   input.Contains("do the quiz");
        }

        // ===== PART 3: EXTRACTION METHODS =====
        private string ExtractTaskName(string input)
        {
            string task = input;
            string[] removePhrases = {
                "add task", "add a task", "create task", "new task",
                "enable", "set up", "i need to", "i should", "please"
            };
            foreach (var phrase in removePhrases)
            {
                if (task.ToLower().Contains(phrase.ToLower()))
                    task = task.Replace(phrase, "", StringComparison.OrdinalIgnoreCase);
            }
            return task.Trim();
        }

        private string ExtractReminderText(string input)
        {
            string reminder = input;
            string[] removePhrases = {
                "remind me", "set a reminder", "reminder", "don't forget",
                "remember to", "please remind me", "remind me to"
            };
            foreach (var phrase in removePhrases)
            {
                if (reminder.ToLower().Contains(phrase.ToLower()))
                    reminder = reminder.Replace(phrase, "", StringComparison.OrdinalIgnoreCase);
            }
            return reminder.Trim();
        }

        // ===== PART 3: ACTION METHODS =====
        private string AddTaskFromNLP(string taskName)
        {
            if (string.IsNullOrEmpty(taskName))
                return "I couldn't understand what task you want to add. Please be more specific.";

            if (_taskManager != null)
            {
                string result = _taskManager.AddTask(taskName, $"Added via NLP: {taskName}", "No reminder set");
                _logger?.Log($"NLP added task: '{taskName}'");
                return $"{result}\n\nWould you like to set a reminder for this task? (Type 'Yes, remind me in X days' or 'No')";
            }
            else
            {
                return $"✅ Task added: '{taskName}'. (TaskManager not available)";
            }
        }

        private string SetReminderFromNLP(string reminderText)
        {
            if (string.IsNullOrEmpty(reminderText))
                return "What would you like to be reminded about?";

            string timePhrase = "soon";
            if (reminderText.ToLower().Contains("tomorrow"))
                timePhrase = "tomorrow";
            else if (reminderText.ToLower().Contains("day") && reminderText.ToLower().Contains("in"))
                timePhrase = "in the coming days";

            _logger?.Log($"NLP set reminder: '{reminderText}'");
            return $"⏰ Reminder set for '{reminderText}' ({timePhrase}). I'll remind you as requested.";
        }

        private string GetActivityLog()
        {
            if (_logger == null)
                return "📋 Activity log is not available.";

            var logs = _logger.GetRecentLogs(10);
            if (logs.Count == 0)
                return "📋 No recent activity to show.";

            string logMessage = "📋 Here's a summary of recent actions:\n";
            foreach (var log in logs)
                logMessage += log + "\n";

            return logMessage;
        }

        // ===== PART 2: MAIN RESPONSE METHOD (ONLY ONE) =====
        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please type a message. I'm here to help!";

            currentUser.IncrementQuestions();

            // Sentiment detection
            string sentiment = DetectSentiment(userInput);
            if (sentiment != null && sentimentResponses.ContainsKey(sentiment))
            {
                string empathy = sentimentResponses[sentiment];
                string answer = GetCybersecurityAnswer(userInput);
                return empathy + "\n\n" + answer;
            }

            // Follow-up requests
            if (IsFollowUpRequest(userInput))
                return GetFollowUpResponse();

            // Store favourite topic (memory)
            StoreFavoriteTopic(userInput);

            // Get cybersecurity answer
            string response = GetCybersecurityAnswer(userInput);
            UpdateContext(userInput, response);
            return response;
        }

        // ===== PART 2: HELPER METHODS =====
        private bool IsFollowUpRequest(string input)
        {
            string lower = input.ToLower();
            return lower.Contains("tell me more") || lower.Contains("another tip") ||
                   lower.Contains("explain more") || lower.Contains("more about") ||
                   lower.Contains("elaborate") || lower.Contains("continue");
        }

        private string GetFollowUpResponse()
        {
            if (string.IsNullOrEmpty(lastTopic))
                return "What would you like to know more about? Try asking about passwords, phishing, or privacy.";

            followUpCount++;
            string topicKey = lastTopic + " tip";
            if (randomResponses.ContainsKey(topicKey))
            {
                var list = randomResponses[topicKey];
                var random = new Random();
                string tip = list[random.Next(list.Count)];
                return $"Here's another tip about {lastTopic}: {tip}";
            }

            if (primaryResponses.ContainsKey(lastTopic))
                return $"As a reminder: {primaryResponses[lastTopic]}";

            return "I can give you more tips. Ask me about password safety, phishing, or safe browsing.";
        }

        private string GetCybersecurityAnswer(string input)
        {
            string lower = input.ToLower();

            // Check random responses first
            foreach (var kvp in randomResponses)
            {
                if (lower.Contains(kvp.Key.ToLower()))
                {
                    var random = new Random();
                    return kvp.Value[random.Next(kvp.Value.Count)];
                }
            }

            // Check primary responses
            foreach (var kvp in primaryResponses)
            {
                if (lower.Contains(kvp.Key.ToLower()))
                    return kvp.Value;
            }

            // Default error handling
            return "I'm not sure I understand. Can you try rephrasing? You can ask about passwords, phishing, privacy, scams, or 2FA.";
        }

        private void StoreFavoriteTopic(string input)
        {
            string lower = input.ToLower();
            if (lower.Contains("interested in") || lower.Contains("i like") || lower.Contains("tell me about"))
            {
                foreach (var topic in primaryResponses.Keys)
                {
                    if (lower.Contains(topic.ToLower()))
                    {
                        currentUser.FavoriteTopic = topic;
                        break;
                    }
                }
            }
        }

        private void UpdateContext(string input, string response)
        {
            foreach (var topic in primaryResponses.Keys)
            {
                if (input.ToLower().Contains(topic.ToLower()))
                {
                    lastTopic = topic;
                    followUpCount = 0;
                    break;
                }
            }
        }

        public string GetPersonalisedGreeting()
        {
            if (!string.IsNullOrEmpty(currentUser.Name))
            {
                if (!string.IsNullOrEmpty(currentUser.FavoriteTopic))
                    return $"Welcome back, {currentUser.Name}! Since you're interested in {currentUser.FavoriteTopic}, here's a quick tip: {GetCybersecurityAnswer(currentUser.FavoriteTopic)}";
                else
                    return $"Good to see you again, {currentUser.Name}! What cybersecurity topic would you like to explore today?";
            }
            else
            {
                return "Hello! I'm your Cybersecurity Awareness Bot. Please tell me your name so I can personalise our chat.";
            }
        }
    }
}