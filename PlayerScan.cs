using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static app.Enums;
using static System.Runtime.InteropServices.ComTypes.IStream;
using static System.Windows.Forms.AxHost;

namespace app
{
    public class PlayerScan
    {
        Form1 Form1_0;

        public long PlayerPointer = 0;
        public long PlayerNamePointer = 0;
        public long actAddress = 0;
        public long mapSeedAddress = 0;
        public Int64 pathAddress = 0;
        public bool FoundPlayer = false;

        public long pAct = 0;
        public uint mapSeedValue = 0;
        public ushort xPos = 0;
        public ushort yPos = 0;
        public ushort xPosOffset = 0;
        public ushort yPosOffset = 0;
        public int xPosOffsetPercent = 0;
        public int yPosOffsetPercent = 0;
        public ushort xPosFinal = 0;
        public ushort yPosFinal = 0;
        public string pName = "";
        public long pRoom1Address = 0;
        public long pRoom2Address = 0;
        public long pLevelAddress = 0;
        public long levelNo = 0;

        public ushort difficulty = 0;
        public uint lastdwInitSeedHash1 = 0;
        public uint lastdwInitSeedHash2 = 0;
        public string sFile = "";

        public uint unitId = 0;

        public string pNameOther = "";
        public uint unitIdOther = 0;


        public void SetForm1(Form1 form1_1)
        {
            Form1_0 = form1_1;
        }

        public void GetPositions()
        {
            pathAddress = Form1_0.Mem_0.ReadInt64Raw((IntPtr) (PlayerPointer + 0x38));
            xPos = Form1_0.Mem_0.ReadUInt16Raw((IntPtr) (pathAddress + 0x02));
            yPos = Form1_0.Mem_0.ReadUInt16Raw((IntPtr) (pathAddress + 0x06));
            xPosOffset = Form1_0.Mem_0.ReadUInt16Raw((IntPtr) (pathAddress + 0x00));
            yPosOffset = Form1_0.Mem_0.ReadUInt16Raw((IntPtr) (pathAddress + 0x04));
            xPosOffsetPercent = (xPosOffset / 65536); //get percentage
            yPosOffsetPercent = (yPosOffset / 65536); //get percentage
            xPosFinal = (ushort)(xPos + xPosOffsetPercent);
            yPosFinal = (ushort)(yPos + yPosOffsetPercent);

            //; get the level number
            pRoom1Address = Form1_0.Mem_0.ReadInt64Raw((IntPtr)(pathAddress + 0x20));
            pRoom2Address = Form1_0.Mem_0.ReadInt64Raw((IntPtr)(pRoom1Address + 0x18));
            pLevelAddress = Form1_0.Mem_0.ReadInt64Raw((IntPtr)(pRoom2Address + 0x90));
            levelNo = Form1_0.Mem_0.ReadUInt32Raw((IntPtr)(pLevelAddress + 0x1F8));

            //; get/check for bad pointer
            if (levelNo == 0 && xPosFinal == 0 && yPosFinal == 0)
            {
                Form1_0.HasPointers = false;
            }

            //#####################################################################################################
            //#####################################################################################################
            //#####################################################################################################
            //; get the difficulty
            actAddress = Form1_0.Mem_0.ReadInt64Raw((IntPtr)(PlayerPointer + 0x20));
            long aActUnk2 = Form1_0.Mem_0.ReadInt64Raw((IntPtr)(actAddress + 0x78));
            difficulty = Form1_0.Mem_0.ReadUInt16Raw((IntPtr)(aActUnk2 + 0x830));

            //; get the map seed
            long actMiscAddress = Form1_0.Mem_0.ReadInt64Raw((IntPtr)(actAddress + 0x78)); //0x0000023a64ed4780; 2449824630656
            uint dwInitSeedHash1 = Form1_0.Mem_0.ReadUInt32Raw((IntPtr)(actMiscAddress + 0x840));
            uint dwInitSeedHash2 = Form1_0.Mem_0.ReadUInt32Raw((IntPtr)(actMiscAddress + 0x844));
            uint dwEndSeedHash1 = Form1_0.Mem_0.ReadUInt32Raw((IntPtr)(actMiscAddress + 0x868));

            /*byte[] buffData = new byte[0x100];
            Form1_0.Mem_0.ReadRawMemory(actMiscAddress + 0x800, ref buffData, buffData.Length);
            string SavePathh = Form1_0.ThisEndPath + "DumpHashStruc";
            File.Create(SavePathh).Dispose();
            File.WriteAllBytes(SavePathh, buffData);*/

            var mapSeed = GetMapSeed((uint)dwInitSeedHash1, (uint)dwEndSeedHash1);
            if (!mapSeed.Item2)
            {
                throw new Exception("Error calculating map seed");
            }


            mapSeedValue = mapSeed.Item1;

            //Form1_0.method_1("SEED: " + mapSeed.Item1.ToString(), Color.Red);
            //Form1_0.method_1("Difficulty: " + ((Difficulty) difficulty).ToString(), Color.Red);
            //Form1_0.GetMapData(mapSeed.Item1.ToString(), (Difficulty) difficulty);
            //#####################################################################################################
            //#####################################################################################################
            //#####################################################################################################

            //get player name
            PlayerNamePointer = Form1_0.Mem_0.ReadInt64Raw((IntPtr)(PlayerPointer + 0x10));
            pName = Form1_0.Mem_0.ReadMemString(PlayerNamePointer);

        }

        private const int MapHashDivisor = 1 << 16;

        // Logic stolen from MapAssist, credits to them
        public static (uint, bool) GetMapSeed(uint initHashSeed, uint endHashSeed)
        {
            uint gameSeedXor = 0;
            var (seed, found) = ReverseMapSeedHash(endHashSeed);
            if (found)
            {
                gameSeedXor = initHashSeed ^ seed;
            }

            if (gameSeedXor == 0)
            {
                return (0, false);
            }

            return (seed, true);
        }

        private static (uint, bool) ReverseMapSeedHash(uint hash)
        {
            uint incrementalValue = 1;

            for (uint startValue = 0; startValue < uint.MaxValue; startValue += incrementalValue)
            {
                uint seedResult = (startValue * 0x6AC690C5 + 666) & 0xFFFFFFFF;

                if (seedResult == hash)
                {
                    return (startValue, true);
                }

                if (incrementalValue == 1 && (seedResult % MapHashDivisor) == (hash % MapHashDivisor))
                {
                    incrementalValue = (uint)MapHashDivisor;
                }
            }

            return (0, false);
        }

        public void scanForPlayer(bool QuickScan)   //scanning for self
        {
            FoundPlayer = false;

            int SizeArray = 0;
            int SizeIncrement = 0;
            byte[] unitTableBufferT = new byte[] { };
            if (QuickScan)
            {
                SizeArray = (128 + 516) * 8;
                SizeIncrement = 8;
                unitTableBufferT = new byte[SizeArray];
                long UnitOffset = (long)Form1_0.BaseAddress + (long)Form1_0.offsets["unitTable"] + Form1_0.UnitStrucOffset;
                Form1_0.Mem_0.ReadRawMemory(UnitOffset, ref unitTableBufferT, SizeArray);
            }
            else
            {
                Form1_0.PatternsScan_0.scanForUnitsPointer("player");
                SizeArray = Form1_0.PatternsScan_0.AllPlayersPointers.Count;
                SizeIncrement = 1;
            }

            for (int i = 0; i < SizeArray; i += SizeIncrement)
            {
                long UnitPointerLocation = 0;
                if (QuickScan)
                {
                    UnitPointerLocation = BitConverter.ToInt64(unitTableBufferT, i);
                }
                else
                {
                    UnitPointerLocation = Form1_0.PatternsScan_0.AllPlayersPointers[i];
                }

                if (UnitPointerLocation > 0)
                {
                    byte[] itemdatastruc = new byte[144];
                    Form1_0.Mem_0.ReadRawMemory(UnitPointerLocation, ref itemdatastruc, 144);

                    // Do ONLY UnitType:0 && TxtFileNo:3
                    //if (BitConverter.ToUInt32(itemdatastruc, 0) == 0 && BitConverter.ToUInt32(itemdatastruc, 4) == 3)
                    if (BitConverter.ToUInt32(itemdatastruc, 0) == 0)
                    {
                        //Form1_0.method_1("PPointerLocation: 0x" + (UnitPointerLocation).ToString("X"));

                        long pUnitDataPtr = BitConverter.ToInt64(itemdatastruc, 0x10);
                        byte[] pUnitData = new byte[144];
                        Form1_0.Mem_0.ReadRawMemory(pUnitDataPtr, ref pUnitData, 144);

                        string name = "";
                        for (int i2 = 0; i2 < 16; i2++)
                        {
                            if (pUnitData[i2] != 0x00)
                            {
                                name += (char)pUnitData[i2];
                            }
                        }
                        //name = name.Replace("?", "");
                        //Form1_0.method_1("PNAME: " + name, Color.Red);

                        //Console.WriteLine(BitConverter.ToUInt32(itemdatastruc, 0));
                        //Console.WriteLine(BitConverter.ToUInt32(itemdatastruc, 4));

                        long ppath = BitConverter.ToInt64(itemdatastruc, 0x38);
                        byte[] ppathData = new byte[144];
                        Form1_0.Mem_0.ReadRawMemory(ppath, ref ppathData, 144);

                        //if posX equal not zero
                        if (BitConverter.ToInt16(ppathData, 2) != 0 && name == Form1_0.textBoxCharName.Text)
                        {
                            Form1_0.method_1("------------------------------------------", Color.DarkBlue);
                            PlayerPointer = UnitPointerLocation;
                            //Form1_0.Grid_SetInfos("Pointer", "0x" + PlayerPointer.ToString("X"));
                            FoundPlayer = true;
                            unitId = BitConverter.ToUInt32(itemdatastruc, 0x08);
                            Form1_0.method_1("Player ID: 0x" + unitId.ToString("X"), Color.DarkBlue);

                            /*string SavePathh = Form1_0.ThisEndPath + "DumpPlayerStruc";
                            File.Create(SavePathh).Dispose();
                            File.WriteAllBytes(SavePathh, itemdatastruc);
                            SavePathh = Form1_0.ThisEndPath + "DumpPlayerUnitData";
                            File.Create(SavePathh).Dispose();
                            File.WriteAllBytes(SavePathh, pUnitData);
                            SavePathh = Form1_0.ThisEndPath + "DumpPlayerPath";
                            File.Create(SavePathh).Dispose();
                            File.WriteAllBytes(SavePathh, ppathData);*/

                            return;
                        }
                    }
                }
            }
        }

    }
}
