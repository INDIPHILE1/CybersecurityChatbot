using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ChatbotEngine(User user)
        {
            currentUser = user;
            LoadPrimaryResponses(); 
            LoadRandomResponses();
            LoadSentimentResponses();
        }

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

        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please type a message. I'm here to help!";

            currentUser.IncrementQuestions();

            string sentiment = DetectSentiment(userInput);
            if (sentiment != null && sentimentResponses.ContainsKey(sentiment))
            {
                string empathy = sentimentResponses[sentiment];
                string answer = GetCybersecurityAnswer(userInput);
                return empathy + "\n\n" + answer;
            }

            if (IsFollowUpRequest(userInput))
                return GetFollowUpResponse();

            StoreFavoriteTopic(userInput);

            string response = GetCybersecurityAnswer(userInput);
            UpdateContext(userInput, response);
            return response;
        }

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

            foreach (var kvp in randomResponses)
            {
                if (lower.Contains(kvp.Key.ToLower()))
                {
                    var random = new Random();
                    return kvp.Value[random.Next(kvp.Value.Count)];
                }
            }

            foreach (var kvp in primaryResponses)
            {
                if (lower.Contains(kvp.Key.ToLower()))
                    return kvp.Value;
            }

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