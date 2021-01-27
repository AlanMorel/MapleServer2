using System;
using System.Diagnostics;
using System.Threading;

namespace GameDataParser.Files
{
    public class Spinner
    {
        private readonly string[] sequence = new string[] { "/", "-", "\\", "|" };
        private int counter = 0;
        private readonly int delay;
        private bool active;
        private readonly Thread thread;
        private readonly Stopwatch stopWatch = new Stopwatch();

        public Spinner(int delay = 500)
        {
            this.delay = delay;
            this.thread = new Thread(Spin);
            this.stopWatch.Start();
        }

        public void Start()
        {
            active = true;
            if (!thread.IsAlive)
            {
                thread.Start();
            }
        }

        public void Stop()
        {
            active = false;
            Draw(" ");
        }

        public TimeSpan getRuntime()
        {
            return this.stopWatch.Elapsed;
        }

        private void Spin()
        {
            while (active)
            {
                Turn();
                Thread.Sleep(delay);
            }
        }

        private void Draw(string c)
        {
            Console.Write($"\r{c}");
        }

        private void Turn()
        {
            Draw(sequence[++counter % sequence.Length]);
        }
    }
}
