using System;
using System.IO;

namespace TwitterBot.Infrastructure.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _target;

        public FileLogger(string logfile)
        {
            _target = logfile;
        }

        public void Log(string str)
        {
            var message = $"{DateTime.Now:yyyy-MM-dd hh:mm:ss}: {str}";
            WriteToFile(message);
        }

        public void Error(string error)
        {
            var message = $"Error {DateTime.Now:yyyy-MM-dd hh:mm:ss}: {error}";
            WriteToFile(message);
        }

        public void Separator()
        {
            WriteToFile("---");
        }

        public void WriteToFile(string message)
        {
            using (var output = File.AppendText(_target))
            {
                output.WriteLine(message);
            }
        }
    }
}