using System;

namespace CybersecurityChatbot
{
    public class User
    {
        public string Name { get; set; }
        public int QuestionsAsked { get; set; }
        public DateTime SessionStartTime { get; set; }

        public User()
        {
            SessionStartTime = DateTime.Now;
            QuestionsAsked = 0;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public double GetSessionDuration()
        {
            return (DateTime.Now - SessionStartTime).TotalMinutes;
        }
    }
}