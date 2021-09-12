using System.Diagnostics;

namespace GameDataParser.Files
{
    public class Spinner
    {
        private readonly string[] Sequence = new string[] { "/", "-", "\\", "|" };
        private int Counter = 0;
        private readonly int Delay;
        private bool Active;
        private readonly Thread Thread;
        private readonly Stopwatch Stopwatch = new Stopwatch();

        public Spinner()
        {
            Stopwatch.Start();
        }

        public Spinner(int delay)
        {
            Delay = delay;
            Thread = new Thread(Spin);
            Stopwatch.Start();
        }

        public void Start()
        {
            Active = true;
            if (!Thread.IsAlive)
            {
                Thread.Start();
            }
        }

        public void Stop()
        {
            Active = false;
            Draw(" ");
        }

        public TimeSpan GetRuntime()
        {
            return Stopwatch.Elapsed;
        }

        private void Spin()
        {
            while (Active)
            {
                Turn();
                Thread.Sleep(Delay);
            }
        }

        private static void Draw(string c)
        {
            Console.Write($"\r{c}");
        }

        private void Turn()
        {
            Draw(Sequence[++Counter % Sequence.Length]);
        }
    }
}
