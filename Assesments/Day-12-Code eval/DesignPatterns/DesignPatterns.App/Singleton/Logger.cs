namespace DesignPatterns.App.Singleton
{
    public class Logger
    {
        private static Logger? _instance;
        private static readonly object _lock = new object();

        private Logger() { }

        public static Logger GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                }
            }
            return _instance;
        }

        public void Log(string message)
        {
            Console.WriteLine($"[LOG] {DateTime.Now:HH:mm:ss} - {message}");
        }
    }
}
