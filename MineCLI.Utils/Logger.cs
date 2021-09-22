using System;
namespace MineCLI.Utils
{
    public static class Logger
    {
        public static void Log(string type, string message)
        {
            Console.WriteLine("[" + type + "] " + message);
        }

        public static void Info(string message)
        {
            Logger.Log("Info", message);
        }
    }
}