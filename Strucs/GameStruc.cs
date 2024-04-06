using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.AxHost;
using System.Threading;
using static app.Enums;

namespace app
{
    public class GameStruc
    {
        Form1 Form1_0;

        public string GameName = "";
        public string GameDifficulty = "";
        public string GameOwnerName = "";
        public List<string> AllGamesNames = new List<string>();
        public List<int> AllGamesPlayersCount = new List<int>();

        public List<string> AllPlayersNames = new List<string>();
        public int SelectedGamePlayerCount = 0;
        public int SelectedGameTime = 0;

        public string SelectedGameName = "";

        public List<string> AllGamesTriedNames = new List<string>();
        public bool AlreadyChickening = false;

        [DllImport("user32.dll")] static extern short VkKeyScan(char ch);

        [StructLayout(LayoutKind.Explicit)]
        struct Helper
        {
            [FieldOffset(0)] public short Value;
            [FieldOffset(0)] public byte Low;
            [FieldOffset(1)] public byte High;
        }

        public void SetForm1(Form1 form1_1)
        {
            Form1_0 = form1_1;
        }

        public Dictionary<string, int> World2Screen(long playerX, long playerY, long targetx, long targety)
        {
            //; scale = 27
            //double scale = Form1_0.centerModeScale * Form1_0.renderScale * 100;
            double scale = 40.8;
            long xdiff = targetx - playerX;
            long ydiff = targety - playerY;

            double angle = 0.785398; //45 deg
            double x = xdiff * Math.Cos(angle) - ydiff * Math.Sin(angle);
            double y = xdiff * Math.Sin(angle) + ydiff * Math.Cos(angle);
            int xS = (int) (Form1_0.CenterX + (x * scale));
            //int yS = (int) (Form1_0.CenterY + (y * scale * 0.5) - 10);
            int yS = (int)(Form1_0.CenterY + (y * scale * 0.5) - 30);

            Dictionary<string, int> NewDict = new Dictionary<string, int>();
            NewDict["x"] = xS;
            NewDict["y"] = yS;
            return NewDict;
        }

        public bool IsGameRunning()
        {
            Process[] ProcList = Process.GetProcessesByName("D2R");
            if (ProcList.Length == 0)
            {
                return false;
            }
            return true;
        }

        public bool IsInGame()
        {
            long baseAddress = (long)Form1_0.BaseAddress + (long)Form1_0.offsets["unitTable"] - 64;
            byte[] unitTableBuffer = new byte[1];
            Form1_0.Mem_0.ReadRawMemory(baseAddress, ref unitTableBuffer, 1);

            //Console.WriteLine(unitTableBuffer[0]);
            if (unitTableBuffer[0] == 0x01)
            {
                return true;
            }

            return false;
        }
    }
}
