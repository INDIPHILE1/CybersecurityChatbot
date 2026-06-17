using System;

namespace POEPART2
{
    public class Log
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
    public Log()
        {
            CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}