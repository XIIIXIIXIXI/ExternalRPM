using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM
{
    public class CommandLineUI
    {
        private readonly JungleCamp[] _jungleCamps;
        private readonly Thread[] _countdownThreads;
        private readonly object _outputLock = new object();

        public CommandLineUI(JungleCamp[] jungleCamps)
        {
            this._jungleCamps = jungleCamps;
            _countdownThreads = new Thread[jungleCamps.Length];
        }

        public void StartCountdownThreads()
        {
            for (int i = 0; i < _jungleCamps.Length; i++)
            {
                _countdownThreads[i] = new Thread(OutputCountdown);
                _countdownThreads[i].Start(i);
            }
        }

        public void StartSingleCountdownThread()
        {
            _countdownThreads[1] = new Thread(OutputCountdown);
            _countdownThreads[1].Start(1);
        }

        public void OutputCountdown(object campIndexObj)
        {
            int campIndex = (int) campIndexObj;
            JungleCamp camp = _jungleCamps[campIndex];

            while (true)
            {
                string output;
                ConsoleColor colorCode = camp.Color == Color.Green ? ConsoleColor.Green : ConsoleColor.White;
                if (camp.IsAlive || camp.RemainingRespawnTime <= TimeSpan.Zero)
                {
                    output = $"{camp.Name}: Alive";
                }
                else
                {
                    output = $"{camp.Name}: {camp.RemainingRespawnTime.Minutes}:{camp.RemainingRespawnTime.Seconds:D2}";
                }

                lock (_outputLock)
                {
                    Console.ForegroundColor = colorCode;
                    Console.SetCursorPosition(0, campIndex);
                    Console.WriteLine(output);
                }
                Thread.Sleep(1000);
            }
        }
        public void StopCountdownThreads()
        {
            foreach (var thread in _countdownThreads)
            {
                thread?.Join();
            }
        }
    }
}
