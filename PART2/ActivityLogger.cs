using System;
using System.Collections.Generic;
using System.Linq;

namespace POEPART2
{
    public class ActivityLogger
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public void Log(string action)
        { 
            try
            {
                var log = new Log
                {
                    Description = action,
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")  // ← THIS IS CRITICAL
                };
                db.Logs.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the error to debug output
                System.Diagnostics.Debug.WriteLine($"Log error: {ex.Message}");
                throw;
            }
        }

        public List<string> GetRecentLogs(int count = 10)
        {
            try
            {
                var logs = db.Logs.OrderByDescending(l => l.Id).Take(count).ToList();
                var result = new List<string>();
                int index = 1;
                foreach (var log in logs.OrderBy(l => l.Id))
                {
                    result.Add($"{index}. {log.Description} ({log.CreatedAt})");
                    index++;
                }
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetRecentLogs error: {ex.Message}");
                return new List<string> { $"Error loading logs: {ex.Message}" };
            }
        }

        public List<string> GetAllLogs()
        {
            try
            {
                var logs = db.Logs.OrderBy(l => l.Id).ToList();
                var result = new List<string>();
                int index = 1;
                foreach (var log in logs)
                {
                    result.Add($"{index}. {log.Description} ({log.CreatedAt})");
                    index++;
                }
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllLogs error: {ex.Message}");
                return new List<string> { $"Error loading logs: {ex.Message}" };
            }
        }

        public int GetCount()
        {
            try
            {
                return db.Logs.Count();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetCount error: {ex.Message}");
                return 0;
            }
        }
    }
}