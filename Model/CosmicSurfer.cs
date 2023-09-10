using ExternalRPM.Data;
using ExternalRPM.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ExternalRPM.Model
{
    public static class CosmicSurfer
    {
        public static float GetWindupTime()
        {
            LocalPlayer localPlayer = LocalPlayer.GetInstance();
            float bonusAttackSpeed = localPlayer.GetBonusAttackSpeed();
            float currentAttackTime = 1/localPlayer.GetAttackSpeed(bonusAttackSpeed);
            return currentAttackTime * localPlayer.WindupPercent * (1 + bonusAttackSpeed * localPlayer.WindupModifier);
        }
        public static float GetCurrentAttackTime()
        {
            LocalPlayer localPlayer = LocalPlayer.GetInstance();
            float bonusAttackSpeed = localPlayer.GetBonusAttackSpeed();
            float currentAttackTime = 1 / localPlayer.GetAttackSpeed(bonusAttackSpeed);
            return currentAttackTime;
        }

        public static void Walk()
        {
            double canAttackTime = 0;
            LocalPlayer localPlayer = LocalPlayer.GetInstance();
            InputSimulator simulator = new InputSimulator();
            var champions = ChampionReader.ReadChampions();
            List<Offsets.GameObject> enemyChampionsList = new List<Offsets.GameObject>();
            foreach (var champion in champions)
            {
                if (champion.TeamID != champions[0].TeamID)
                {
                    enemyChampionsList.Add(champion);
                }
            }
            Offsets.GameObject[] enemyChampions = enemyChampionsList.ToArray();

            while (true)
            {
                float gameTime = Offsets.Instances.GetGameTime();
                float bonusAttackSpeed = localPlayer.GetBonusAttackSpeed();
                float attackSpeed = localPlayer.GetAttackSpeed(bonusAttackSpeed);
                if (canAttackTime < gameTime)
                {
                    canAttackTime = gameTime + 1.0 / attackSpeed;
                    Point currentMousePos = GetCursorPosition();
                    for (int i = 0; i < enemyChampions.Length; i++)
                    {
                        enemyChampions[i].Position = ChampionReader.ReadChampionPosition(enemyChampions[i].memoryID);
                        Renderer.WorldToScreen(enemyChampions[i].Position, out enemyChampions[i].ScreenPosition);
                    }
                    SetCursorPosition((int)enemyChampions[0].ScreenPosition.X, (int)enemyChampions[0].ScreenPosition.Y);
                    simulator.Mouse.RightButtonDown(); // Mouse right button down
                    Thread.Sleep(33); // Adjust this sleep duration as needed
                    simulator.Mouse.RightButtonUp();
                    SetCursorPosition(currentMousePos.X, currentMousePos.Y);
                    Debug.WriteLine("Attack!");
                    Thread.Sleep((int)(GetWindupTime() * 1000));
                }
                else
                {
                    Thread.Sleep(3); // Adjust this sleep duration as needed

                }

            }
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return new Point(lpPoint.X, lpPoint.Y);
        }

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }
    }
}
