using System;
using System.Diagnostics;

namespace CqrsRadio.Common.StatsD
{
    class Timer : IDisposable
    {
        private readonly Stopwatch _sw;
        private readonly Action<TimeSpan> _action;

        public Timer(Action<TimeSpan> action)
        {
            _action = action ?? (elapsed => Console.WriteLine(elapsed));

            _sw = new Stopwatch();
            _sw.Start();
        }

        public void Dispose()
        {
            _sw.Stop();
            _action(_sw.Elapsed);
        }
    }
}
