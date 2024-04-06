﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app
{
    public class NPCStruc
    {
        Form1 Form1_0;

        public void SetForm1(Form1 form1_1)
        {
            Form1_0 = form1_1;
        }


        public string getTownNPC(int txtFileNo)
        {
            switch (txtFileNo)
            {
                case 146: return "DeckardCain";
                case 154: return "Charsi";
                case 147: return "Gheed";
                case 150: return "Kashya";
                case 155: return "Warriv";
                case 148: return "Akara";
                case 244: return "DeckardCain";
                case 210: return "Meshif";
                case 175: return "Warriv";
                case 199: return "Elzix";
                case 198: return "Greiz";
                case 177: return "Drognan";
                case 178: return "Fara";
                case 201: return "Jerhyn";
                case 202: return "Lysander";
                case 176: return "Atma";
                case 200: return "Geglash";
                case 331: return "Kaelan";
                case 245: return "DeckardCain";
                case 264: return "Meshif";
                case 255: return "Ormus";
                case 252: return "Asheara";
                case 254: return "Alkor";
                case 253: return "Hratli";
                case 297: return "Natalya";
                case 246: return "DeckardCain";
                case 251: return "Tyrael";
                case 338: return "DeadCorpse";
                case 367: return "Tyrael";
                case 521: return "Tyrael";
                case 257: return "Halbu";
                case 405: return "Jamella";
                case 265: return "DeckardCain";
                case 520: return "DeckardCain";
                case 512: return "Anya";
                case 527: return "Anya";
                case 515: return "Qual-Kehk";
                case 513: return "Malah";
                case 511: return "Larzuk";
                case 514: return "Nihlathak Town";
                case 266: return "navi";
                case 408: return "Malachai";
                case 406: return "Izual";
            }
            return "";
        }

        public string getNPC_ID(int txtFileNo)
        {
            switch (txtFileNo)
            {
                case 156: return "Andariel";
                case 211: return "Duriel";
                case 229: return "Radament";
                case 744: return "Radament2";
                case 242: return "Mephisto";
                case 243: return "Diablo";
                case 250: return "Summoner";
                case 256: return "Izual";
                case 267: return "Bloodraven";
                case 333: return "Diabloclone";
                case 365: return "Griswold";
                case 526: return "Nihlathak";
                case 544: return "Baal";
                case 570: return "Baalclone";
                case 702: return "BaalThrone";
                case 704: return "Uber Mephisto";
                case 705: return "Uber Diablo";
                case 706: return "Uber Izual";
                case 707: return "Uber Andariel";
                case 708: return "Uber Duriel";
                case 709: return "Uber Baal";
                case 745: return "CubeNPC";

                case 271: return "roguehire";
                //case 338: return "act2hire";
                case 359: return "act3hire";
                case 560: return "act5hire1";
                case 561: return "act5hire2";
                case 289: return "ClayGolem";
                case 290: return "BloodGolem";
                case 291: return "IronGolem";
                case 292: return "FireGolem";
                case 363: return "NecroSkeleton";
                case 364: return "NecroMage";
                case 417: return "ShadowWarrior";
                case 418: return "ShadowMaster";
                case 419: return "DruidHawk";
                case 420: return "DruidSpiritWolf";
                case 421: return "DruidFenris";
                case 423: return "HeartOfWolverine";
                case 424: return "OakSage";
                case 428: return "DruidBear";
                case 357: return "Valkyrie";
                //case 359: return "IronWolf";

                case 0: return "Bonebreak";
                case 5: return "Corpsefire";
                case 11: return "Pitspawn Fouldog";
                case 20: return "Rakanishu";
                case 24: return "Treehead WoodFist";
                case 31: return "Fire Eye";
                case 45: return "The Countess";
                case 47: return "Sarina the Battlemaid";
                case 62: return "Baal Subject 1";
                case 66: return "Flamespike the Crawler";
                case 75: return "Fangskin";
                case 83: return "Bloodwitch the Wild";
                case 92: return "Beetleburst";
                case 97: return "Leatherarm";
                case 103: return "Ancient Kaa the Soulless";
                case 105: return "Baal Subject 2";
                case 120: return "The Tormentor";
                case 125: return "Web Mage the Burning";
                case 129: return "Stormtree";
                case 138: return "Icehawk Riftwing";
                case 160: return "Coldcrow";
                case 276: return "Boneash";
                case 281: return "Witch Doctor Endugu";
                case 284: return "Coldworm the Burrower";
                case 299: return "Taintbreeder";
                case 306: return "Grand Vizier of Chaos";
                case 308: return "Riftwraith the Cannibal";
                case 312: return "Lord De Seis";
                // case 345: return "Council Member";
                // case 346: return "Council Member";
                // case 347: return "Council Member";
                case 362: return "Winged Death";
                case 402: return "The Smith";
                case 409: return "The Feature Creep";
                case 437: return "Bonesaw Breaker";
                case 440: return "Pindleskin";
                case 443: return "Threash Socket";
                case 449: return "Frozenstein";
                case 453: return "Megaflow Rectifier";
                case 472: return "Anodized Elite";
                case 475: return "Vinvear Molech";
                case 479: return "Siege Boss";
                case 481: return "Sharp Tooth Sayer";
                case 494: return "Dac Farren";
                case 496: return "Magma Torquer";
                case 501: return "Snapchip Shatter";
                case 508: return "Axe Dweller";
                case 529: return "Eyeback Unleashed";
                case 533: return "Blaze Ripper";
                case 540: return "Ancient Barbarian 1";
                case 541: return "Ancient Barbarian 2";
                case 542: return "Ancient Barbarian 3";
                case 557: return "Baal Subject 3";
                case 558: return "Baal Subject 4";
                case 571: return "Baal Subject 5";
                case 735: return "The Cow King";
                case 736: return "Dark Elder";

                case 146: return "DeckardCain";
                case 154: return "Charsi";
                case 147: return "Gheed";
                case 150: return "Kashya";
                case 155: return "Warriv";
                case 148: return "Akara";
                case 244: return "DeckardCain";
                case 210: return "Meshif";
                case 175: return "Warriv";
                case 199: return "Elzix";
                case 198: return "Greiz";
                case 177: return "Drognan";
                case 178: return "Fara";
                case 201: return "Jerhyn";
                case 202: return "Lysander";
                case 176: return "Atma";
                case 200: return "Geglash";
                case 331: return "Kaelan";
                case 245: return "DeckardCain";
                case 264: return "Meshif";
                case 255: return "Ormus";
                case 252: return "Asheara";
                case 254: return "Alkor";
                case 253: return "Hratli";
                case 297: return "Natalya";
                case 246: return "DeckardCain";
                case 251: return "Tyrael";
                case 338: return "DeadCorpse";
                case 367: return "Tyrael";
                case 521: return "Tyrael";
                case 257: return "Halbu";
                case 405: return "Jamella";
                case 265: return "DeckardCain";
                case 520: return "DeckardCain";
                case 512: return "Anya";
                case 527: return "Anya";
                case 515: return "Qual-Kehk";
                case 513: return "Malah";
                case 511: return "Larzuk";
                case 514: return "Nihlathak Town";
                case 266: return "navi";
                case 408: return "Malachai";
                case 406: return "Izual";
            }
            return "";
        }

        /*public bool IsDeadBody(int txtFileNo)
        {
            if (txtFileNo == 338)
            {
                return true;
            }
            return false;
        }*/


        // certain NPCs we don't want to see such as mercs
        public int HideNPC(int txtFileNo)
        {
            switch (txtFileNo)
            {
                case 0: return 1; //UNKOWN
                case 149: return 1; //Chicken
                case 151: return 1; //Rat
                case 152: return 1; //Rogue
                case 153: return 1; //HellMeteor
                case 157: return 1; //Bird
                case 158: return 1; //Bird2
                case 159: return 1; //Bat
                case 194: return 1; //Hadriel
                case 195: return 1; //Act2Male
                case 196: return 1; //Act2Female
                case 197: return 1; //Act2Child
                case 179: return 1; //Cow
                case 185: return 1; //Camel
                case 203: return 1; //Act2Guard
                case 204: return 1; //Act2Vendor
                case 205: return 1; //Act2Vendor2
                case 227: return 1; //Maggot
                case 268: return 1; //Bug
                case 269: return 1; //Scorpion
                // case 271: return 1; //Rogue2
                case 272: return 1; //Rogue3
                case 283: return 1; //Larva
                case 293: return 1; //Familiar
                case 294: return 1; //Act3Male
                // case 289: return 1; //ClayGolem
                // case 290: return 1; //BloodGolem
                // case 291: return 1; //IronGolem
                // case 292: return 1; //FireGolem
                case 296: return 1; //Act3Female
                case 318: return 1; //Snake
                case 319: return 1; //Parrot
                case 320: return 1; //Fish
                case 321: return 1; //EvilHole
                case 322: return 1; //EvilHole2
                case 323: return 1; //EvilHole3
                case 324: return 1; //EvilHole4
                case 325: return 1; //EvilHole5
                case 326: return 1; //FireboltTrap
                case 327: return 1; //HorzMissileTrap
                case 328: return 1; //VertMissileTrap
                case 329: return 1; //PoisonCloudTrap
                case 330: return 1; //LightningTrap
                case 332: return 1; //InvisoSpawner
                // case 338: return 1; //Guard or DEAD BODY!!!
                case 339: return 1; //MiniSpider
                case 344: return 1; //BoneWall
                case 351: return 1; //Hydra
                case 352: return 1; //Hydra2
                case 353: return 1; //Hydra3
                case 355: return 1; //SevenTombs
                // case 357: return 1; //Valkyrie
                // case 359: return 1; //IronWolf
                // case 363: return 1; //NecroSkeleton
                // case 364: return 1; //NecroMage
                case 366: return 1; //CompellingOrb},
                case 370: return 1; //SpiritMummy
                case 377: return 1; //Act2Guard4
                case 378: return 1; //Act2Guard5
                case 392: return 1; //Window
                case 393: return 1; //Window2
                case 401: return 1; //MephistoSpirit
                case 410: return 1; //WakeOfDestruction
                case 411: return 1; //ChargedBoltSentry
                case 412: return 1; //LightningSentry
                case 414: return 1; //InvisiblePet
                case 415: return 1; //InfernoSentry
                case 416: return 1; //DeathSentry
                // case 417: return 1; //ShadowWarrior
                // case 418: return 1; //ShadowMaster
                // case 419: return 1; //DruidHawk
                // case 420: return 1; //DruidSpiritWolf
                // case 421: return 1; //DruidFenris
                // case 423: return 1; //HeartOfWolverine
                // case 424: return 1; //OakSage
                // case 428: return 1; //DruidBear
                case 543: return 1; //BaalThrone
                case 567: return 1; //InjuredBarbarian
                case 568: return 1; //InjuredBarbarian2
                case 569: return 1; //InjuredBarbarian3
                case 711: return 1; //DemonHole
            }
            return 0;
        }
    }
}
