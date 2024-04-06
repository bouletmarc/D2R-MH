﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static app.Enums;

namespace app
{
    public class PatternsScan
    {
        Form1 Form1_0;

        public byte[] UnitBuffer = new byte[] { };

        public List<long> AllItemsPointers = new List<long>();
        public List<long> AllObjectsPointers = new List<long>(); //->refer to all gameobjects
        public List<long> AllPlayersPointers = new List<long>();
        public List<long> AllNPCPointers = new List<long>();

        public List<long> AllPossibleItemsPointers = new List<long>();
        public List<long> AllPossiblePlayerPointers = new List<long>();
        public List<long> AllPossibleObjectsPointers = new List<long>();
        public List<long> AllPossibleNPCPointers = new List<long>();

        //"04 00 00 00 ?? ?? 00 00";
        byte[] ThisCheckbytes = new byte[] { 0x04, 0x00, 0x00, 0x00, 0xff, 0xff, 0x00, 0x00 };
        public int[] DontCheckUnitIndexes = new int[] { 0, 0, 0, 0, 1, 1, 0, 0 };
        public int[] DontCheckIndexes = new int[0];

        public long StartIndexItemLast = long.MaxValue;
        //public int ScanUnitsNumber = 3000;
        public int ScanUnitsNumber = 2600;
        //public int ScanUnitsNumber = 2200;
        //public int ScanUnitsNumber = 6000;
        public int ScanUnitsNegativeOffset = 128;

        public void SetForm1(Form1 form1_1)
        {
            Form1_0 = form1_1;
        }

        IntPtr modulePatternScan(string Tpattern)
        {
            IntPtr ThisAddr = (IntPtr)0;

            //method_1("Patterns: " + Tpattern);
            byte[] ThisCheckbytes = StringToByteArray(Tpattern.Replace(" ", ""));
            long ThisAddrF = Search(Form1_0.buffer, ThisCheckbytes, 0, "pattern");
            if (ThisAddrF > 0)
            {
                return (IntPtr)ThisAddrF;
            }

            return ThisAddr;
        }

        public byte[] StringToByteArray(string hex)
        {
            DontCheckIndexes = new int[hex.Length / 2];

            int Inddd = 0;
            for (int i = 0; i < hex.Length; i++)
            {
                if (hex[i] == '?')
                {
                    DontCheckIndexes[Inddd] = 1;
                }
                else
                {
                    DontCheckIndexes[Inddd] = 0;
                }
                Inddd++;
                i++;
            }

            hex = hex.Replace("??", "00");

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public long Search(byte[] arrayToSearchThrough, byte[] patternToFind, long StartOffset, string SearchType)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length) return -1;

            int DontCheckCount = 0;
            for (long i = StartOffset; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (long j = 0; j < patternToFind.Length; j++)
                {
                    int DontChh = 0;
                    if (SearchType == "item" || SearchType == "player" || SearchType == "objects" || SearchType == "NPC")
                    {
                        DontChh = DontCheckUnitIndexes[j];
                    }
                    else
                    {
                        DontChh = DontCheckIndexes[j];
                    }

                    if (DontChh == 0)
                    {
                        if (arrayToSearchThrough[i + j] != patternToFind[j])
                        {
                            found = false;
                            break;
                        }
                    }

                    //check if TxtFileNo are within range
                    if (SearchType == "item")
                    {
                        if (DontChh == 1)
                        {
                            //not higher than 0x02 (0x2FF)
                            if (DontCheckCount == 1 && arrayToSearchThrough[i + j] > 0x02)
                            {
                                found = false;
                                break;
                            }
                            DontCheckCount++;
                        }
                    }
                }

                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        public void PatternScan()
        {
            //SetFormat, Integer, Hex
            //; unit table
            string pattern = "48 03 C7 49 8B 8C C6";
            IntPtr patternAddress = modulePatternScan(pattern);
            IntPtr unitTable = (IntPtr)Form1_0.Mem_0.ReadInt(patternAddress + 7);
            Form1_0.offsets["unitTable"] = unitTable;
            Form1_0.method_1("UnitTable offset: 0x" + unitTable.ToString("X"), Color.Black);

            //; game data(IP and name)
            //pattern = "44 88 25 ?? ?? ?? ?? 66 44 89 25 ?? ?? ?? ??";
            pattern = "44 88 25 ?? ?? ?? ?? 66 44 89 25";
            patternAddress = modulePatternScan(pattern);
            IntPtr offsetBuffer = (IntPtr)Form1_0.Mem_0.ReadInt(patternAddress + 0x3);
            //IntPtr gameDataOffset = (IntPtr) (((Int64) patternAddress - (Int64) BaseAddress) - 0x121 + (Int64) offsetBuffer);
            IntPtr gameDataOffset = (IntPtr)(((Int64)patternAddress) - 0x121 + (Int64)offsetBuffer);
            Form1_0.offsets["gameDataOffset"] = gameDataOffset;
            Form1_0.method_1("Game data offset: 0x" + gameDataOffset.ToString("X"), Color.Black);
        }




        public void scanForUnitsPointer(string SearchUnitsType)
        {
            long UnitOffset = (long)Form1_0.BaseAddress + (long)Form1_0.offsets["unitTable"] + Form1_0.UnitStrucOffset;

            if (SearchUnitsType == "item") AllPossibleItemsPointers = new List<long>();
            if (SearchUnitsType == "player") AllPossiblePlayerPointers = new List<long>();
            if (SearchUnitsType == "objects") AllPossibleObjectsPointers = new List<long>();
            if (SearchUnitsType == "NPC") AllPossibleNPCPointers = new List<long>();

            for (int i = 0; i < ((128 + 516) * 10); i += 8)
            {
                long UnitPointerLocation = Form1_0.Mem_0.ReadInt64Raw((IntPtr)(UnitOffset + i));
                if (UnitPointerLocation > 0)
                {
                    uint UnitTypeT = Form1_0.Mem_0.ReadUInt32Raw((IntPtr)(UnitPointerLocation));
                    uint TxtFileNoT = Form1_0.Mem_0.ReadUInt32Raw((IntPtr)(UnitPointerLocation + 4));

                    // Do ONLY UnitType:4 && TxtFileNo:!3
                    if (SearchUnitsType == "item")
                    {
                        if (UnitTypeT == (int)UnitType.Item && TxtFileNoT != 3)
                        {
                            AllPossibleItemsPointers.Add(UnitPointerLocation);
                        }
                    }
                    if (SearchUnitsType == "player")
                    {
                        if (UnitTypeT == (int)UnitType.Player)
                        {
                            AllPossiblePlayerPointers.Add(UnitPointerLocation);
                        }
                    }
                    if (SearchUnitsType == "objects")
                    {
                        if (UnitTypeT == (int)UnitType.GameObject)
                        {
                            AllPossibleObjectsPointers.Add(UnitPointerLocation);
                        }
                    }
                    if (SearchUnitsType == "NPC")
                    {
                        if (UnitTypeT == (int)UnitType.NPC)
                        {
                            AllPossibleNPCPointers.Add(UnitPointerLocation);
                        }
                    }
                }
            }

            UnitPatternScan(SearchUnitsType);
            checkForMissingPointers(SearchUnitsType);
        }

        public void UnitPatternScan(string SearchUnitsType)
        {
            if (SearchUnitsType == "item") AllItemsPointers = new List<long>();
            if (SearchUnitsType == "player") AllPlayersPointers = new List<long>();
            if (SearchUnitsType == "objects") AllObjectsPointers = new List<long>();
            if (SearchUnitsType == "NPC") AllNPCPointers = new List<long>();

            long StartIndexItem = long.MaxValue;
            long StartIndexPlayer = long.MaxValue;
            long StartIndexMobs = long.MaxValue;
            long StartIndexNPC = long.MaxValue;

            //set start index for searching
            if (SearchUnitsType == "item")
            {
                for (int i = 0; i < AllPossibleItemsPointers.Count; i++)
                {
                    if (AllPossibleItemsPointers[i] < StartIndexItem)
                    {
                        StartIndexItem = AllPossibleItemsPointers[i];
                    }
                }
            }
            if (SearchUnitsType == "player")
            {
                for (int i = 0; i < AllPossiblePlayerPointers.Count; i++)
                {
                    if (AllPossiblePlayerPointers[i] < StartIndexPlayer)
                    {
                        StartIndexPlayer = AllPossiblePlayerPointers[i];
                    }
                }
            }
            if (SearchUnitsType == "objects")
            {
                for (int i = 0; i < AllPossibleObjectsPointers.Count; i++)
                {
                    if (AllPossibleObjectsPointers[i] < StartIndexMobs)
                    {
                        StartIndexMobs = AllPossibleObjectsPointers[i];
                    }
                }
            }
            if (SearchUnitsType == "NPC")
            {
                for (int i = 0; i < AllPossibleNPCPointers.Count; i++)
                {
                    if (AllPossibleNPCPointers[i] < StartIndexNPC)
                    {
                        StartIndexNPC = AllPossibleNPCPointers[i];
                    }
                }
            }

            //######
            if (StartIndexItem < StartIndexItemLast)
            {
                //0xD87F7298
                //0x31F38
                //0x4830
                if (StartIndexItemLast != long.MaxValue)
                {
                    long DiffVal = (StartIndexItemLast - StartIndexItem);
                    int UnitNumberDiff = (int)(DiffVal) / (0x48 + 0x170);
                    if (DiffVal < 0xFFFFF)  //here
                    {
                        ScanUnitsNumber += UnitNumberDiff;
                        Form1_0.method_1("Item start diff: 0x" + (DiffVal).ToString("X") + ", scann for: " + ScanUnitsNumber + " +" + UnitNumberDiff, Color.DarkOrchid);
                    }
                    else
                    {
                        StartIndexItem = StartIndexItemLast; //correct the pointer
                        //Form1_0.method_1("BAD Item start diff: 0x" + (DiffVal).ToString("X") + ", scann for: " + ScanUnitsNumber + " +" + UnitNumberDiff, Color.Red);
                    }
                }
                StartIndexItemLast = StartIndexItem;
            }
            //######

            //search
            long CheckThisI = StartIndexItem;
            long CheckThisP = StartIndexPlayer;
            long CheckThisM = StartIndexMobs;
            long CheckThisN = StartIndexNPC;
            UnitBuffer = new byte[9];

            CheckThisI -= (0x48 + 0x170) * ScanUnitsNegativeOffset;  //offseting in negative here
            for (int i = 0; i < ScanUnitsNumber; i++)
            //for (int i = 0; i < 2048; i++)
            //for (int i = 0; i < 2500; i++)
            {
                if ((i % 2) == 1)
                {
                    CheckThisI += 0x48;
                    CheckThisP += 0x48;
                    CheckThisM += 0x48;
                    CheckThisN += 0x48;
                }
                else
                {
                    CheckThisI += 0x170;
                    CheckThisP += 0x170;
                    CheckThisM += 0x170;
                    CheckThisN += 0x170;
                }

                if (SearchUnitsType == "item")
                {
                    ThisCheckbytes = new byte[] { (byte)UnitType.Item, 0x00, 0x00, 0x00, 0xff, 0xff, 0x00, 0x00 };
                    Form1_0.Mem_0.ReadRawMemory(CheckThisI, ref UnitBuffer, UnitBuffer.Length);
                    unitPatternScan(CheckThisI, "item");
                }

                if (SearchUnitsType == "player")
                {
                    ThisCheckbytes = new byte[] { (byte)UnitType.Player, 0x00, 0x00, 0x00, 0xff, 0xff, 0x00, 0x00 };
                    Form1_0.Mem_0.ReadRawMemory(CheckThisP, ref UnitBuffer, UnitBuffer.Length);
                    unitPatternScan(CheckThisP, "player");
                }

                if (SearchUnitsType == "objects")
                {
                    ThisCheckbytes = new byte[] { (byte)UnitType.GameObject, 0x00, 0x00, 0x00, 0xff, 0xff, 0x00, 0x00 };
                    Form1_0.Mem_0.ReadRawMemory(CheckThisM, ref UnitBuffer, UnitBuffer.Length);
                    unitPatternScan(CheckThisM, "objects");
                }

                if (SearchUnitsType == "NPC")
                {
                    ThisCheckbytes = new byte[] { (byte)UnitType.NPC, 0x00, 0x00, 0x00, 0xff, 0xff, 0x00, 0x00 };
                    Form1_0.Mem_0.ReadRawMemory(CheckThisN, ref UnitBuffer, UnitBuffer.Length);
                    unitPatternScan(CheckThisN, "NPC");
                }
            }
        }

        public void checkForMissingPointers(string SearchUnitsType)
        {
            if (SearchUnitsType == "item")
            {
                for (int i = 0; i < AllPossibleItemsPointers.Count; i++)
                {
                    AddPointerToList(AllPossibleItemsPointers[i], "item");
                }
            }
            if (SearchUnitsType == "player")
            {
                for (int i = 0; i < AllPlayersPointers.Count; i++)
                {
                    AddPointerToList(AllPlayersPointers[i], "player");
                }
            }
            if (SearchUnitsType == "objects")
            {
                for (int i = 0; i < AllObjectsPointers.Count; i++)
                {
                    AddPointerToList(AllObjectsPointers[i], "objects");
                }
            }
            if (SearchUnitsType == "NPC")
            {
                for (int i = 0; i < AllNPCPointers.Count; i++)
                {
                    AddPointerToList(AllNPCPointers[i], "NPC");
                }
            }
        }

        public void unitPatternScan(long AtPoiinter, string SearchType)
        {
            long SearchOffset = 0;
            long ThisAddrF = Search(UnitBuffer, ThisCheckbytes, SearchOffset, SearchType);

            //while (ThisAddrF >= 0)
            if (ThisAddrF >= 0)
            {
                AddPointerToList(AtPoiinter + ThisAddrF, SearchType);
            }
        }

        public void AddPointerToList(long TPoiinter, string SearchType)
        {
            if (SearchType == "item")
            {
                if (AllItemsPointers.Count > 0)
                {
                    for (int i = 0; i < AllItemsPointers.Count; i++)
                    {
                        if (AllItemsPointers[i] == TPoiinter)
                        {
                            return;
                        }
                    }
                    AllItemsPointers.Add(TPoiinter);
                }
                else
                {
                    AllItemsPointers.Add(TPoiinter);
                }
            }

            if (SearchType == "player")
            {
                if (AllPlayersPointers.Count > 0)
                {
                    for (int i = 0; i < AllPlayersPointers.Count; i++)
                    {
                        if (AllPlayersPointers[i] == TPoiinter)
                        {
                            return;
                        }
                    }
                    AllPlayersPointers.Add(TPoiinter);
                }
                else
                {
                    AllPlayersPointers.Add(TPoiinter);
                }
            }

            if (SearchType == "objects")
            {
                if (AllObjectsPointers.Count > 0)
                {
                    for (int i = 0; i < AllObjectsPointers.Count; i++)
                    {
                        if (AllObjectsPointers[i] == TPoiinter)
                        {
                            return;
                        }
                    }
                    AllObjectsPointers.Add(TPoiinter);
                }
                else
                {
                    AllObjectsPointers.Add(TPoiinter);
                }
            }

            if (SearchType == "NPC")
            {
                if (AllNPCPointers.Count > 0)
                {
                    for (int i = 0; i < AllNPCPointers.Count; i++)
                    {
                        if (AllNPCPointers[i] == TPoiinter)
                        {
                            return;
                        }
                    }
                    AllNPCPointers.Add(TPoiinter);
                }
                else
                {
                    AllNPCPointers.Add(TPoiinter);
                }
            }
        }


        /*public const long OneKB = 1024;
        public const long OneMB = OneKB * OneKB;
        public const long OneGB = OneMB * OneKB;
        public const long OneTB = OneGB * OneKB;

        public string BytesToHumanReadable(long bytes)
        {
            if (bytes < 0) return "ERROR";
            if (bytes < OneKB) return $"{bytes}B";
            if (bytes >= OneKB && bytes < OneMB) return $"{bytes / OneKB}KB";
            if (bytes >= OneMB && bytes < OneGB) return $"{bytes / OneMB}MB";
            if (bytes >= OneGB && bytes < OneTB) return $"{bytes / OneMB}GB";
            if (bytes >= OneTB) return $"{bytes / OneTB}";
            return "TOO BIG";
        }*/
    }
}
