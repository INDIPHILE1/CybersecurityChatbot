using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEPART2
{
    public class User
    {  
        public string Name { get; set; } = ""; 
        public string FavoriteTopic { get; set; } = "";
        public int QuestionsAsked { get; set; }
        public DateTime SessionStartTime { get; set; }

        public User()
        {
            SessionStartTime = DateTime.Now;
            QuestionsAsked = 0;
            FavoriteTopic = "";
        }
         
        public void IncrementQuestions() => QuestionsAsked++;
        public double GetSessionDuration() => (DateTime.Now - SessionStartTime).TotalMinutes;
    }
}