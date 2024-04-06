using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static app.Form1;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace app
{
    public partial class Form1 : Form
    {

        public string MHVersion = "V1.0";

        public string ThisEndPath = Application.StartupPath + @"\";
        public string SettingsFile = Application.StartupPath + @"\Settings.txt";

        public int ScreenX = 1920;
        public int ScreenY = 1080;
        public int CenterX = 0;
        public int CenterY = 0;
        public int D2Widht = 0;
        public int D2Height = 0;
        public int ScreenXOffset = 0;
        public int ScreenYOffset = 0;

        public Process process;
        public Dictionary<string, IntPtr> offsets = new Dictionary<string, IntPtr>();
        public IntPtr BaseAddress = (IntPtr)0;
        public IntPtr processHandle = (IntPtr)0;
        public System.Timers.Timer LoopTimer;
        public byte[] bufferRead = new byte[] { };
        public byte[] buffer = new byte[] { };
        public bool Running = false;
        public bool HasPointers = false;
        public int UnitStrucOffset = -32;
        public int hWnd = 0;
        public Rectangle D2Rect = new Rectangle();
        public bool PrintedGameTime = false;
        public int FoundPlayerPointerTryCount = 0;

        public Mem Mem_0;
        public Form1 Form1_0;
        public PatternsScan PatternsScan_0;
        public OverlayForm OverlayForm_0;
        public MapAreaStruc MapAreaStruc_0;
        public PlayerScan PlayerScan_0;
        public MobsStruc MobsStruc_0;
        public GameStruc GameStruc_0;
        public NPCStruc NPCStruc_0;
        public ObjectsStruc ObjectsStruc_0;
        public Town Town_0;

        // REQUIRED CONSTS
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int SYNCHRONIZE = 0x00100000;

        // REQUIRED METHODS
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("user32.dll")]
        private static extern int FindWindow(string ClassName, string WindowName);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(int hwnd, out Rectangle rect);


        // REQUIRED STRUCTS
        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }

        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        public Form1()
        {
            InitializeComponent();

            this.Text = "D2R - MH (" + MHVersion + ") by BMDevs";
            Form1_0 = this;

            LoopTimer = new System.Timers.Timer(1);
            LoopTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            PatternsScan_0 = new PatternsScan();
            OverlayForm_0 = new OverlayForm(Form1_0);
            MapAreaStruc_0 = new MapAreaStruc();
            PlayerScan_0 = new PlayerScan();
            MobsStruc_0 = new MobsStruc();
            GameStruc_0 = new GameStruc();
            NPCStruc_0 = new NPCStruc();
            ObjectsStruc_0 = new ObjectsStruc();
            Town_0 = new Town();
            Mem_0 = new Mem();

            PatternsScan_0.SetForm1(Form1_0);
            MapAreaStruc_0.SetForm1(Form1_0);
            PlayerScan_0.SetForm1(Form1_0);
            MobsStruc_0.SetForm1(Form1_0);
            GameStruc_0.SetForm1(Form1_0);
            NPCStruc_0.SetForm1(Form1_0);
            ObjectsStruc_0.SetForm1(Form1_0);
            Town_0.SetForm1(Form1_0);
            Mem_0.SetForm1(Form1_0);

            OverlayForm_0.Show();

            LoadSettings();
        }

        public void SaveSettings()
        {
            string AllTxt = "";
            AllTxt += textBoxD2Path.Text + Environment.NewLine;
            AllTxt += textBoxCharName.Text + Environment.NewLine;

            File.Create(SettingsFile).Dispose();
            File.WriteAllText(SettingsFile, AllTxt);
        }

        public void LoadSettings()
        {
            if (File.Exists(SettingsFile))
            {
                string[] AllTxtLine = File.ReadAllLines(SettingsFile);
                textBoxD2Path.Text = AllTxtLine[0];
                textBoxCharName.Text = AllTxtLine[1];
            }
        }

        public void method_1(string string_3, Color ThisColor)
        {
            try
            {
                if (richTextBox1.InvokeRequired)
                {
                    // Call this same method but append THREAD2 to the text
                    Action safeWrite = delegate { method_1(string_3, ThisColor); };
                    richTextBox1.Invoke(safeWrite);
                }
                else
                {
                    //Console.WriteLine(string_3);
                    richTextBox1.SelectionColor = ThisColor;
                    richTextBox1.AppendText(string_3 + Environment.NewLine);
                    Application.DoEvents();
                }
            }
            catch { }
        }

        public void Startt()
        {
            try
            {
                SYSTEM_INFO sys_info = new SYSTEM_INFO();
                GetSystemInfo(out sys_info);

                Process[] ProcList = Process.GetProcessesByName("D2R");
                if (!IsGameRunning())
                {
                    method_1("D2R is not running!", Color.Red);
                    return;
                }
                else
                {
                    hWnd = FindWindow(null, "Diablo II: Resurrected");
                    GetWindowRect(hWnd, out D2Rect);

                    ScreenX = Screen.PrimaryScreen.Bounds.Width;
                    ScreenY = Screen.PrimaryScreen.Bounds.Height;
                    CenterX = ScreenX / 2;
                    CenterY = ScreenY / 2;
                    D2Widht = D2Rect.Width;
                    D2Height = D2Rect.Height;
                    ScreenXOffset = D2Rect.Location.X;
                    ScreenYOffset = D2Rect.Location.Y;

                    method_1("D2R is running...", Color.DarkGreen);

                    process = Process.GetProcessesByName("D2R")[0];
                    processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, process.Id);
                    
                    foreach (ProcessModule module in process.Modules)
                    {
                        if (module.ModuleName == "D2R.exe")
                        {
                            this.BaseAddress = module.BaseAddress;
                            method_1("D2R module BaseAddress: 0x" + this.BaseAddress.ToString("X"), Color.Black);
                        }
                    }

                    int bytesRead = 0;
                    buffer = new byte[0x3FFFFFF];
                    Mem_0.ReadMemory(BaseAddress, ref buffer, buffer.Length, ref bytesRead);

                    PatternsScan_0.PatternScan();

                    buffer = null;
                    buffer = new byte[0];

                    LoopTimer.Start();
                }
            }
            catch (Exception message)
            {
                method_1("Error:" + Environment.NewLine + message, Color.Red);
                return;

            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LoopTimer.Stop();

            
            if (IsGameRunning())
            {
                bool isInGame = IsInGame();
                if (isInGame)
                {
                    if (!HasPointers)
                    {
                        PrintedGameTime = false;
                        PlayerScan_0.scanForPlayer(true);
                        if (PlayerScan_0.FoundPlayer)
                        {
                            HasPointers = true;
                        }
                        else
                        {
                            //didn't found player pointer
                            PlayerScan_0.scanForPlayer(false);
                            if (PlayerScan_0.FoundPlayer)
                            {
                                HasPointers = true;
                            }
                            else
                            {
                                FoundPlayerPointerTryCount++;

                                if (FoundPlayerPointerTryCount >= 300)
                                {
                                    method_1("Player pointer not found!", Color.Red);
                                    if (Running) LoopTimer.Start();
                                    return;
                                }
                            }
                        }
                    }
                    if (HasPointers)
                    {
                        PlayerScan_0.GetPositions();
                        if (MapAreaStruc_0.AllMapData.Count == 0) MapAreaStruc_0.ScanMapStruc();
                        OverlayForm_0.SetAllOverlay();
                        PrintedGameTime = false;
                    }
                }
                else
                {
                    if (!PrintedGameTime)
                    {
                        FoundPlayerPointerTryCount = 0;
                        OverlayForm_0.ClearAllOverlay();
                        Form1_0.method_1("Waiting to be in game!", Color.Red);
                        PrintedGameTime = true;
                    }
                    HasPointers = false;
                }
            }

            if (Running) LoopTimer.Start();
        }

        public void WaitDelay(int DelayTime)
        {
            int CurrentWait = 0;
            while (CurrentWait < DelayTime)
            {
                Thread.Sleep(1);
                Application.DoEvents();
                CurrentWait++;
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (!Running)
            {
                button1.Text = "STOP";
                Running = true;
                Startt();
            }
            else
            {
                button1.Text = "START";
                OverlayForm_0.ClearAllOverlay();
                Running = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
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

            if (unitTableBuffer[0] == 0x01)
            {
                return true;
            }
            return false;
        }
    }
}
