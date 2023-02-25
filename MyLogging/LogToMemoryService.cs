using CollageApp.MyLogging;

namespace CollegeApp.MyLogging
{
    public class LogToMemoryService : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogtoDB");
            //write your own logic to save the logs to MemoryService
        }
    }
}
