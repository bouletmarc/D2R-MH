using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static app.MapAreaStruc;

namespace app
{
    public class Town
    {
        Form1 Form1_0;

        public void SetForm1(Form1 form1_1)
        {
            Form1_0 = form1_1;
        }

        public string getAreaName(int areaNum)
        {
            switch (areaNum)
            {
                case 1: return "Rogue Encampment";
                case 2: return "Blood Moor";
                case 3: return "Cold Plains";
                case 4: return "Stony Field";
                case 5: return "Dark Wood";
                case 6: return "Black Marsh";
                case 7: return "Tamoe Highland";
                case 8: return "Den of Evil";
                case 9: return "Cave Level 1";
                case 10: return "Underground Passage Level 1";
                case 11: return "Hole Level 1";
                case 12: return "Pit Level 1";
                case 13: return "Cave Level 2";
                case 14: return "Underground Passage Level 2";
                case 15: return "Hole Level 2";
                case 16: return "Pit Level 2";
                case 17: return "Burial Grounds";
                case 18: return "Crypt";
                case 19: return "Mausoleum";
                case 20: return "Forgotten Tower";
                case 21: return "Tower Cellar Level 1";
                case 22: return "Tower Cellar Level 2";
                case 23: return "Tower Cellar Level 3";
                case 24: return "Tower Cellar Level 4";
                case 25: return "Tower Cellar Level 5";
                case 26: return "Monastery Gate";
                case 27: return "Outer Cloister";
                case 28: return "Barracks";
                case 29: return "Jail Level 1";
                case 30: return "Jail Level 2";
                case 31: return "Jail Level 3";
                case 32: return "Inner Cloister";
                case 33: return "Cathedral";
                case 34: return "Catacombs Level 1";
                case 35: return "Catacombs Level 2";
                case 36: return "Catacombs Level 3";
                case 37: return "Catacombs Level 4";
                case 38: return "Tristram";
                case 39: return "Moo Moo Farm";
                case 40: return "Lut Gholein";
                case 41: return "Rocky Waste";
                case 42: return "Dry Hills";
                case 43: return "Far Oasis";
                case 44: return "Lost City";
                case 45: return "Valley of Snakes";
                case 46: return "Canyon of the Magi";
                case 47: return "Sewers Level 1";
                case 48: return "Sewers Level 2";
                case 49: return "Sewers Level 3";
                case 50: return "Harem Level 1";
                case 51: return "Harem Level 2";
                case 52: return "Palace Cellar Level 1";
                case 53: return "Palace Cellar Level 2";
                case 54: return "Palace Cellar Level 3";
                case 55: return "Stony Tomb Level 1";
                case 56: return "Halls of the Dead Level 1";
                case 57: return "Halls of the Dead Level 2";
                case 58: return "Claw Viper Temple Level 1";
                case 59: return "Stony Tomb Level 2";
                case 60: return "Halls of the Dead Level 3";
                case 61: return "Claw Viper Temple Level 2";
                case 62: return "Maggot Lair Level 1";
                case 63: return "Maggot Lair Level 2";
                case 64: return "Maggot Lair Level 3";
                case 65: return "Ancient Tunnels";
                case 66: return "Tal Rasha's Tomb #1";
                case 67: return "Tal Rasha's Tomb #2";
                case 68: return "Tal Rasha's Tomb #3";
                case 69: return "Tal Rasha's Tomb #4";
                case 70: return "Tal Rasha's Tomb #5";
                case 71: return "Tal Rasha's Tomb #6";
                case 72: return "Tal Rasha's Tomb #7";
                case 73: return "Duriel's Lair";
                case 74: return "Arcane Sanctuary";
                case 75: return "Kurast Docktown";
                case 76: return "Spider Forest";
                case 77: return "Great Marsh";
                case 78: return "Flayer Jungle";
                case 79: return "Lower Kurast";
                case 80: return "Kurast Bazaar";
                case 81: return "Upper Kurast";
                case 82: return "Kurast Causeway";
                case 83: return "Travincal";
                case 84: return "Arachnid Lair";
                case 85: return "Spider Cavern";
                case 86: return "Swampy Pit Level 1";
                case 87: return "Swampy Pit Level 2";
                case 88: return "Flayer Dungeon Level 1";
                case 89: return "Flayer Dungeon Level 2";
                case 90: return "Swampy Pit Level 3";
                case 91: return "Flayer Dungeon Level 3";
                case 92: return "Sewers Level 1";
                case 93: return "Sewers Level 2";
                case 94: return "Ruined Temple";
                case 95: return "Disused Fane";
                case 96: return "Forgotten Reliquary";
                case 97: return "Forgotten Temple";
                case 98: return "Ruined Fane";
                case 99: return "Disused Reliquary";
                case 100: return "Durance of Hate Level 1";
                case 101: return "Durance of Hate Level 2";
                case 102: return "Durance of Hate Level 3";
                case 103: return "Pandemonium Fortress";
                case 104: return "Outer Steppes";
                case 105: return "Plains of Despair";
                case 106: return "City of the Damned";
                case 107: return "River of Flame";
                case 108: return "Chaos Sanctuary";
                case 109: return "Harrogath";
                case 110: return "Bloody Foothills";
                case 111: return "Frigid Highlands";
                case 112: return "Arreat Plateau";
                case 113: return "Crystalline Passage";
                case 114: return "Frozen River";
                case 115: return "Glacial Trail";
                case 116: return "Drifter Cavern";
                case 117: return "Frozen Tundra";
                case 118: return "Ancients' Way";
                case 119: return "Icy Cellar";
                case 120: return "Arreat Summit";
                case 121: return "Nihlathaks Temple";
                case 122: return "Halls of Anguish";
                case 123: return "Halls of Death's Calling";
                case 124: return "Halls of Vaught";
                case 125: return "Abaddon";
                case 126: return "Pit of Acheron";
                case 127: return "Infernal Pit";
                case 128: return "Worldstone Keep Level 1";
                case 129: return "Worldstone Keep Level 2";
                case 130: return "Worldstone Keep Level 3";
                case 131: return "Throne of Destruction";
                case 132: return "Worldstone Chamber";
                case 133: return "Pandemonium Run 1";
                case 134: return "Pandemonium Run 2";
                case 135: return "Pandemonium Run 3";
                case 136: return "Tristram";
            }

            return "";
        }
    }
}
