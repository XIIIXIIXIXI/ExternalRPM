using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Model;
using ExternalRPM.Model.Kindred;

namespace ExternalRPM.Presentation
{
    public class CommandLineUI
    {
        private readonly JungleCamp[] _jungleCamps;
        private readonly Thread[] _countdownThreads;
        private readonly KindredTracker _kindredTracker;
        private readonly object _outputLock = new object();

        public CommandLineUI(JungleCamp[] jungleCamps, KindredTracker kindredTracker)
        {
            _jungleCamps = jungleCamps;
            _kindredTracker = kindredTracker;
            _countdownThreads = new Thread[jungleCamps.Length + 1];
        }

        public void StartCountdownThreads()
        {
            for (int i = 0; i < _jungleCamps.Length; i++)
            {
                _countdownThreads[i] = new Thread(OutputCountdown);
                _countdownThreads[i].Start(i);
            }
            int index = _countdownThreads.Length - 1;
            _countdownThreads[index] = new Thread(KindredMarkTimer);
            _countdownThreads[index].Start(index);
        }

        public void StartSingleCountdownThread()
        {
            _countdownThreads[1] = new Thread(OutputCountdown);
            _countdownThreads[1].Start(1);
        }

        public void OutputCountdown(object campIndexObj)
        {
            int campIndex = (int)campIndexObj;
            JungleCamp camp = _jungleCamps[campIndex];

            while (true)
            {
                string output;
                //ConsoleColor colorCode = camp.Color == Color.Green ? ConsoleColor.Green : ConsoleColor.White;
                if (camp.IsAlive || camp.RemainingRespawnTime <= TimeSpan.Zero)
                {
                    output = $"{camp.Name}: Alive";
                }
                else
                {
                    output = $"{camp.Name}: {camp.RemainingRespawnTime.Minutes}:{camp.RemainingRespawnTime.Seconds:D2} ";
                }

                lock (_outputLock)
                {
                    //Console.ForegroundColor = colorCode;
                    Console.SetCursorPosition(0, campIndex);
                    Console.WriteLine(output);
                }
                Thread.Sleep(1000);
            }
        }

        public void KindredMarkTimer(object campIndexObj)
        {
            int campIndex = (int)campIndexObj;
            while (true)
            {
                string output;
                if (_kindredTracker.markTracker.MarkTimerRun)
                {
                    output =
                        $"Mark: {_kindredTracker.markTracker.MarkRespawnTime.Seconds:D2}       ";
                }
                else
                {
                    output = $"Mark: Alive";
                }
                lock (_outputLock)
                {
                    Console.ForegroundColor = ConsoleColor.White;
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
