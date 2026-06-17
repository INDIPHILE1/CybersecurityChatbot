using System;
using System.Collections.Generic;

namespace POEPART2
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;
        private ActivityLogger _logger;

        public QuizManager(ActivityLogger logger)
        {
            _logger = logger;
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                // Topic: Phishing (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    CorrectAnswer = "C",
                    Explanation = "Reporting phishing emails helps prevent scams and protects others.",
                    IsTrueFalse = false
                },
                // Topic: Password Safety (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What makes a strong password?",
                    Options = new List<string> { "Your birthday", "A common word", "A mix of letters, numbers, and symbols", "Your pet's name" },
                    CorrectAnswer = "C",
                    Explanation = "Strong passwords use a combination of uppercase, lowercase, numbers, and special characters.",
                    IsTrueFalse = false
                },
                // Topic: Password Safety (True/False)
                new QuizQuestion
                {
                    Question = "You should use the same password for all your accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Using the same password everywhere is dangerous. If one account is breached, all are at risk.",
                    IsTrueFalse = true
                },
                // Topic: Safe Browsing (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What does 'HTTPS' indicate in a website URL?",
                    Options = new List<string> { "The site is safe", "The site is popular", "The site is encrypted", "The site is free" },
                    CorrectAnswer = "C",
                    Explanation = "HTTPS means the connection is encrypted, protecting your data from interception.",
                    IsTrueFalse = false
                },
                // Topic: Social Engineering (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What is social engineering in cybersecurity?",
                    Options = new List<string> { "Building social networks", "Manipulating people to reveal information", "Engineering social media", "Creating social apps" },
                    CorrectAnswer = "B",
                    Explanation = "Social engineering tricks people into sharing confidential information through manipulation.",
                    IsTrueFalse = false
                },
                // Topic: Two-Factor Authentication (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What is Two-Factor Authentication (2FA)?",
                    Options = new List<string> { "A password manager", "A second verification step", "A VPN service", "An antivirus program" },
                    CorrectAnswer = "B",
                    Explanation = "2FA adds an extra layer of security by requiring a second form of verification.",
                    IsTrueFalse = false
                },
                // Topic: Social Engineering (True/False)
                new QuizQuestion
                {
                    Question = "Scammers often create a sense of urgency to trick you.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Urgency is a common tactic used in social engineering to prevent you from thinking critically.",
                    IsTrueFalse = true
                },
                // Topic: Malware/Ransomware (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What type of malware locks your files and demands payment?",
                    Options = new List<string> { "Virus", "Spyware", "Ransomware", "Adware" },
                    CorrectAnswer = "C",
                    Explanation = "Ransomware encrypts files and demands payment for decryption.",
                    IsTrueFalse = false
                },
                // Topic: Privacy Settings (Multiple Choice)
                new QuizQuestion
                {
                    Question = "Why should you review app permissions on your phone?",
                    Options = new List<string> { "To save battery", "To protect your privacy", "To improve speed", "To remove ads" },
                    CorrectAnswer = "B",
                    Explanation = "Reviewing permissions helps ensure apps don't access unnecessary personal data.",
                    IsTrueFalse = false
                },
                // Topic: Safe Browsing (True/False)
                new QuizQuestion
                {
                    Question = "It is safe to use public Wi-Fi for online banking.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Public Wi-Fi is often unsecured, making it risky for sensitive transactions.",
                    IsTrueFalse = true
                },
                // Topic: Password Safety (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What is a password manager?",
                    Options = new List<string> { "A tool to store passwords", "A password guesser", "A hacker tool", "A virus" },
                    CorrectAnswer = "A",
                    Explanation = "Password managers securely store and generate strong passwords for all your accounts.",
                    IsTrueFalse = false
                },
                // Topic: Phishing (Multiple Choice)
                new QuizQuestion
                {
                    Question = "What is a common sign of a phishing email?",
                    Options = new List<string> { "Personalized greeting", "Suspicious links", "Known sender", "Professional font" },
                    CorrectAnswer = "B",
                    Explanation = "Suspicious links or requests for personal information are red flags for phishing.",
                    IsTrueFalse = false
                }
            };
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentIndex < _questions.Count)
                return _questions[_currentIndex];
            return null;
        }

        public bool SubmitAnswer(string answer)
        {
            var current = GetCurrentQuestion();
            if (current == null) return false;

            bool isCorrect = answer.Equals(current.CorrectAnswer, StringComparison.OrdinalIgnoreCase);
            if (isCorrect)
                _score++;

            _currentIndex++;
            return isCorrect;
        }

        public string GetFeedback(bool isCorrect)
        {
            var current = _questions[_currentIndex - 1];
            string result = isCorrect ? "✅ Correct!" : "❌ Incorrect.";
            return $"{result}\n{current.Explanation}";
        }

        public bool IsFinished()
        {
            return _currentIndex >= _questions.Count;
        }

        public string GetFinalScore()
        {
            return $"{_score} out of {_questions.Count}";
        }

        public string GetFinalMessage()
        {
            double percentage = (_score * 100.0) / _questions.Count;
            if (percentage >= 80) return "🌟 Great job! You're a cybersecurity pro!";
            else if (percentage >= 60) return "👍 Good effort! Keep learning to stay safe online!";
            else return "📚 Keep learning! Cybersecurity is important for everyone!";
        }

        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            _logger.Log("Quiz started - new attempt");
        }

        public int GetTotalQuestions()
        {
            return _questions.Count;
        }

        public int GetCurrentIndex()
        {
            return _currentIndex;
        }
    }
}