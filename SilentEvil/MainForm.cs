using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Diagnostics;
using ReadWriteMemory;
using PatternScan;
using SilentEvil.Offsets;

namespace SilentEvil
{


    struct Vector3
    {
       public float x, y, z;

        public Vector3(float x, float y,float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                Vector3 v = (Vector3)obj;
                if (v.x == x && v.y == y && v.z == z)
                    return obj.GetType().Equals(this.GetType());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{{x={0}, y={1}, z={2}}}", x, y,z);
        }
    }

    struct Vector2
    {
        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                Vector2 v = (Vector2)obj;
                if (v.x == x && v.y == y)
                    return obj.GetType().Equals(this.GetType());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{{x={0}, y={1}}}", x, y);
        }
    }

    public partial class MainForm : Form
    {

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        enum CSGO_Weapon_ID
        {
            weapon_none,
            weapon_deagle,
            weapon_elite,
            weapon_fiveseven,
            weapon_glock,
            weapon_p228,
            weapon_usp,
            weapon_ak47,
            weapon_aug,
            weapon_awp,
            weapon_famas,
            weapon_g3sg1,
            weapon_galil,
            weapon_galilar,
            weapon_m249,
            weapon_m3,
            weapon_m4a1,
            weapon_mac10,
            weapon_mp5navy,
            weapon_p90,
            weapon_scout,
            weapon_sg550,
            weapon_sg552,
            weapon_tmp,
            weapon_ump45,
            weapon_xm1014,
            weapon_bizon,
            weapon_mag7,
            weapon_negev,
            weapon_sawedoff,
            weapon_tec9,
            weapon_taser,
            weapon_hkp2000,
            weapon_mp7,
            weapon_mp9,
            weapon_nova,
            weapon_p250,
            weapon_scar17,
            weapon_scar20,
            weapon_sg556,
            weapon_ssg08,
            weapon_knifegg,
            weapon_knife,
            weapon_flashbang,
            weapon_hegrenade,
            weapon_smokegrenade,
            weapon_molotov,
            weapon_decoy,
            weapon_incgrenade,
            weapon_c4
        };

        bool IsWeaponNonAim(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_knifegg || iWeaponID == (int)CSGO_Weapon_ID.weapon_knife || iWeaponID == (int)CSGO_Weapon_ID.weapon_flashbang 
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_hegrenade || iWeaponID == (int)CSGO_Weapon_ID.weapon_smokegrenade
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_molotov || iWeaponID == (int)CSGO_Weapon_ID.weapon_decoy || iWeaponID == (int)CSGO_Weapon_ID.weapon_incgrenade 
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_c4);
        }

        bool IsWeaponPistol(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_deagle || iWeaponID == (int)CSGO_Weapon_ID.weapon_elite || iWeaponID == (int)CSGO_Weapon_ID.weapon_fiveseven 
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_glock
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_p228 || iWeaponID == (int)CSGO_Weapon_ID.weapon_usp || iWeaponID == (int)CSGO_Weapon_ID.weapon_tec9 
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_taser || iWeaponID == (int)CSGO_Weapon_ID.weapon_hkp2000 || iWeaponID == 61 
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_p250);
        }

        bool IsWeaponRifle(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_ak47 || iWeaponID == (int)CSGO_Weapon_ID.weapon_aug || iWeaponID == (int)CSGO_Weapon_ID.weapon_famas
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_galilar || iWeaponID == (int)CSGO_Weapon_ID.weapon_m4a1 || iWeaponID == 39 || iWeaponID == 60);
        }

        bool IsWeaponSMG(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_mac10 || iWeaponID == (int)CSGO_Weapon_ID.weapon_p90 || iWeaponID == (int)CSGO_Weapon_ID.weapon_ump45 
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_bizon || iWeaponID == (int)CSGO_Weapon_ID.weapon_mp9 
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_mp7 || iWeaponID == (int)CSGO_Weapon_ID.weapon_negev || iWeaponID == (int)CSGO_Weapon_ID.weapon_m249);
        }

        bool IsWeaponSniper(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_awp || iWeaponID == (int)CSGO_Weapon_ID.weapon_g3sg1 || iWeaponID == (int)CSGO_Weapon_ID.weapon_scar20 
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_ssg08);
        }

        bool IsWeaponShotgun(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_xm1014 || iWeaponID == (int)CSGO_Weapon_ID.weapon_mag7 || iWeaponID == (int)CSGO_Weapon_ID.weapon_nova || iWeaponID == (int)CSGO_Weapon_ID.weapon_sawedoff);
        }

        ProcessMemory Mem;
        struct Module
        {
            public int Base;
            public int Size;
        }

         Module Client, Engine;

        Offsets.Offsets Offs = new Offsets.Offsets();

        [DllImport("user32.dll")]
        static extern ushort GetAsyncKeyState(int vKey);

        public static bool IsKeyPushedDown(System.Windows.Forms.Keys vKey)
        {
            return 0 != (GetAsyncKeyState((int)vKey) & 0x8000);
        }

        public Thread checkKeysThread;
        public Thread tESP;
        public Thread tTrigger;
        public Thread tSkinChanger;
        public Thread tRCS;
        public Thread tMisc;
        public Thread tAim;
        class KeySett
        {
            public int ESP { get; set; }
            public int AimLock { get; set; }
            public int AimRifle { get; set; }
            public int AimSniper { get; set; }
            public int AimPistol { get; set; }
            public int Trigger { get; set; }
            public int SkinChanger { get; set; }
            public int RCS { get; set; }
            public int AimLockHold { get; set; }
            public int TriggerHold { get; set; }

            public KeySett()
            {
                ESP = 0;
                AimLock = 0;
                AimRifle = 0;
                AimSniper = 0;
                AimPistol = 0;
                Trigger = 0;
                SkinChanger = 0;
                RCS = 0;
                AimLockHold = 0;
                TriggerHold = 0;
            }
        }

        KeySett KeySettings = new KeySett();

        public void CheckKeys()
        {
            while (true)
            {
                int selected = 0;
                this.Invoke((MethodInvoker)delegate
                {
                    selected = nsTabControl2.SelectedIndex;
                });
                if (selected != 6)
                {
                    if (KeySettings.ESP != 0 && IsKeyPushedDown((Keys)KeySettings.ESP))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            nsOnOffBox2.Checked = !nsOnOffBox2.Checked;
                        });
                        Thread.Sleep(500);
                    }

                    if (KeySettings.AimLock != 0 && IsKeyPushedDown((Keys)KeySettings.AimLock))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            AimSettings[0].enabled = !AimSettings[0].enabled;
                        });
                        Thread.Sleep(500);
                    }

                    if (KeySettings.AimRifle != 0 && IsKeyPushedDown((Keys)KeySettings.AimRifle))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            AimSettings[1].enabled = !AimSettings[1].enabled;
                        });
                        Thread.Sleep(500);
                    }

                    if (KeySettings.AimSniper != 0 && IsKeyPushedDown((Keys)KeySettings.AimSniper))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            AimSettings[2].enabled = !AimSettings[2].enabled;
                        });
                        Thread.Sleep(500);
                    }

                    if (KeySettings.AimPistol != 0 && IsKeyPushedDown((Keys)KeySettings.AimPistol))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            AimSettings[3].enabled = !AimSettings[3].enabled;
                        });
                        Thread.Sleep(500);
                    }

                    if (KeySettings.AimPistol != 0 && IsKeyPushedDown((Keys)KeySettings.AimPistol))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            AimSettings[3].enabled = !AimSettings[3].enabled;
                        });
                        Thread.Sleep(500);
                    }

                    if (KeySettings.Trigger != 0 && IsKeyPushedDown((Keys)KeySettings.Trigger))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            nsOnOffBox5.Checked = !nsOnOffBox5.Checked;
                        });
                        Thread.Sleep(500);
                    }

                    if (KeySettings.SkinChanger != 0 && IsKeyPushedDown((Keys)KeySettings.SkinChanger))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            nsOnOffBox11.Checked = !nsOnOffBox11.Checked;
                        });
                        Thread.Sleep(500);
                    }

                    if (KeySettings.RCS != 0 && IsKeyPushedDown((Keys)KeySettings.RCS))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            nsOnOffBox14.Checked = !nsOnOffBox14.Checked;
                        });
                        Thread.Sleep(500);
                    }
                }
                Thread.Sleep(1);
            }
        }

        class AimSett
        {
            public bool enabled { get; set; }
            public int FOV { get; set; }
            public int smooth { get; set; }
            public bool noLock { get; set; }
            public bool noSticky { get; set; }
            public int aimBone { get; set; }
            public bool RCS { get; set; }
            public bool visCheck { get; set; }
            public bool aimFlashed { get; set; }

            public AimSett()
            {
                enabled = false;
                FOV = 180;
                smooth = 15;
                noLock = true;
                noSticky = true;
                aimBone = 6;
                RCS = true;
                visCheck = true;
                aimFlashed = false;
            }
        }
        AimSett[] AimSettings = new AimSett[4];

        class Skins
        {
            public int skinId { get; set; }

            public Skins()
            {
                skinId = 0;
            }
        }

        Skins[] CustomSkins = new Skins[33];

        List<string>[] skinList = new List<string>[33];



        public MainForm()
        {
            InitializeComponent();
            for (int i = 0; i < 4; i++)
            {
                AimSettings[i] = new AimSett();
            }
            nsComboBox1.SelectedIndex = 0;

            for (int i = 0; i < 33; i++)
            {
                skinList[i] = new List<string>();
                CustomSkins[i] = new Skins();
            }

            skinList[0].InsertRange(skinList[0].Count, new string[] { "None", "37 - Blaze", "347 - Pilot", "468 - Midnight Storm", "469 - Sunset Storm", "5 - Forest DDPAT", "232 - Crimson Web", "17 - Urban DDPAT", "40 - Night", "61 - Hypnotic", "90 - Mudder", "235 - VariCamo", "185 - Golden Koi", "231 - Cobalt Disruption", "237 - Urban Rubble", "397 - Naga", "328 - Hand Cannon", "273 - Heirloom", "296 - Meteorite", "351 - Conspiracy", "425 - Bronze Deco", "470 - Sunset Storm" });
            skinList[1].InsertRange(skinList[1].Count, new string[] { "None", "28 - Anodized Navy", "36 - Ossified", "43 - Stained", "46 - Contractor", "47 - Colony", "153 - Demolition", "190 - Black Limba", "248 - Red Quartz", "249 - Cobalt Quartz", "220 - Hemoglobin", "396 - Urban Shock", "261 - Marina", "276 - Panther", "307 - Retribution", "330 - Briar", "447 - Duelist", "450 - Moon in Libra" });
            skinList[2].InsertRange(skinList[2].Count, new string[] { "None", "3 - Candy Apple", "27 - Bone Mask", "44 - Case Hardened", "46 - Contractor", "78 - Forest Night", "141 - Orange Peel", "151 - Jungle", "254 - Nitro", "248 - Red Quartz", "210 - Anodized Gunmetal", "223 - Nightshade", "252 - Silver Quartz", "265 - Kami", "274 - Copper Galaxy", "464 - Neon Kimono", "352 - Fowl Play", "377 - Hot Shot", "387 - Urban Hazard", "427 - Monkey Business" });
            skinList[3].InsertRange(skinList[3].Count, new string[] { "None", "2 - Groundwater", "3 - Candy Apple", "38 - Fade", "40 - Night", "48 - Dragon Tattoo", "437 - Twilight Galaxy", "99 - Sand Dune", "159 - Brass", "399 - Catacombs", "208 - Sand Dune", "230 - Steel Disruption", "278 - Blue Fissure", "293 - Death Rattle", "353 - Water Elemental", "367 - Reactor", "381 - Grinder", "479 - Bunsen Burner" });
            skinList[4].InsertRange(skinList[4].Count, new string[] { "None", "341 - First Class", "14 - Red Laminate", "22 - Contrast Spray", "44 - Case Hardened", "72 - Safari Mesh", "122 - Jungle Spray", "170 - Predator", "172 - Black Laminate", "180 - Fire Serpent", "394 - Cartel", "300 - Emerald Pinstripe", "226 - Blue Laminate", "282 - Redline", "302 - Vulcan", "316 - Jaguar", "340 - Jet Set", "380 - Wasteland Rebel", "422 - Elite Build", "456 - Hydroponic", "474 - Aquamarine Revenge" });
            skinList[5].InsertRange(skinList[5].Count, new string[] { "None", "73 - Wings", "10 - Copperhead", "9 - Bengal Tiger", "28 - Anodized Navy", "167 - Radiation Hazard", "110 - Condemned", "33 - Hot Rod", "100 - Storm", "46 - Contractor", "47 - Colony", "197 - Anodized Navy", "280 - Chameleon", "305 - Torque", "375 - Radiation Hazard", "442 - Asterion", "444 - Daedalus", "455 - Akihabara Accept" });
            skinList[6].InsertRange(skinList[6].Count, new string[] { "None", "174 - BOOM", "344 - Dragon Lore", "84 - Pink DDPAT", "30 - Snake Camo", "51 - Lightning Strike", "72 - Safari Mesh", "181 - Corticera", "259 - Redline", "395 - Man-o'-war", "212 - Graphite", "227 - Electric Hive", "251 - Pit Viper", "279 - Asiimov", "424 - Worm God", "446 - Medusa", "451 - Sun in Leo", "475 - Hyper Beast" });
            skinList[7].InsertRange(skinList[7].Count, new string[] { "None", "22 - Contrast Spray", "47 - Colony", "92 - Cyanospatter", "429 - Djinn", "154 - Afterimage", "178 - Doomkitty", "194 - Spitfire", "244 - Teardown", "218 - Hexane", "260 - Pulse", "288 - Sergeant", "371 - Styx", "477 - Neural Net" });
            skinList[8].InsertRange(skinList[8].Count, new string[] { "None", "8 - Desert Storm", "6 - Arctic Camo", "27 - Bone Mask", "46 - Contractor", "72 - Safari Mesh", "74 - Polar Camo", "147 - Jungle Dashed", "235 - VariCamo", "170 - Predator", "195 - Demeter", "229 - Azure Zebra", "294 - Green Apple", "465 - Orange Kimono", "464 - Neon Kimono", "382 - Murky", "438 - Chronos" });
            skinList[9].InsertRange(skinList[9].Count, new string[] { "None", "5 - Forest DDPAT", "22 - Contrast Spray", "83 - Orange DDPAT", "428 - Eco", "76 - Winter Forest", "119 - Sage Spray", "235 - VariCamo", "235 - VariCamo", "398 - Chatterbox", "192 - Shattered", "308 - Kami", "216 - Blue Titanium", "237 - Urban Rubble", "241 - Hunting Blind", "264 - Sandstorm", "297 - Tuxedo", "379 - Cerberus", "460 - Aqua Terrace", "478 - Rocket Pop" });
            skinList[10].InsertRange(skinList[10].Count, new string[] { "None", "22 - Contrast Spray", "75 - Blizzard Marbleized", "202 - Jungle DDPAT", "243 - Gator Mesh", "266 - Magma", "401 - System Lock", "452 - Shipping Forecast", "472 - Impact Drill" });
            skinList[11].InsertRange(skinList[11].Count, new string[] { "None", "8 - Desert Storm", "101 - Tornado", "5 - Forest DDPAT", "167 - Radiation Hazard", "164 - Modern Hunter", "16 - Jungle Tiger", "17 - Urban DDPAT", "155 - Bullet Rain", "170 - Predator", "176 - Faded Zebra", "187 - Zirka", "255 - Asiimov", "309 - Howl", "215 - X-Ray", "336 - Desert-Strike", "384 - Griffin", "400 - (Dragon King)", "449 - Poseidon", "471 - Daybreak", "480 - Evil Daimyo" });
            skinList[12].InsertRange(skinList[12].Count, new string[] { "None", "101 - Tornado", "3 - Candy Apple", "32 - Silver", "5 - Forest DDPAT", "17 - Urban DDPAT", "38 - Fade", "433 - Neon Rider", "98 - Ultraviolet", "157 - Palm", "188 - Graven", "337 - Tatter", "246 - Amber Fade", "284 - Heat", "310 - Curse", "333 - Indigo", "343 - Commuter", "372 - Nuclear Garden", "402 - Malachite" });
            skinList[13].InsertRange(skinList[13].Count, new string[] { "None", "342 - Leather", "20 - Virus", "22 - Contrast Spray", "100 - Storm", "67 - Cold Blooded", "111 - Glacier Mesh", "124 - Sand Spray", "156 - Death by Kitty", "234 - Ash Wood", "169 - Fallout Warning", "175 - Scorched", "182 - Emerald Dragon", "244 - Teardown", "228 - Blind Spot", "283 - Trigon", "311 - Desert Warfare", "335 - Module", "359 - Asiimov", "486 - Elite Build" });
            skinList[14].InsertRange(skinList[14].Count, new string[] { "None", "37 - Blaze", "5 - Forest DDPAT", "15 - Gunsmoke", "17 - Urban DDPAT", "436 - Grand Prix", "70 - Carbon Fiber", "93 - Caramel", "169 - Fallout Warning", "175 - Scorched", "193 - Bone Pile", "392 - Delusion", "281 - Corporal", "333 - Indigo", "362 - Labyrinth", "441 - Minotaur's Labyrinth", "488 - Riot" });
            skinList[15].InsertRange(skinList[15].Count, new string[] { "None", "166 - Blaze Orange", "238 - VariCamo Blue", "27 - Bone Mask", "42 - Blue Steel", "96 - Blue Spruce", "95 - Grassland", "135 - Urban Perforated", "151 - Jungle", "235 - VariCamo", "235 - VariCamo", "169 - Fallout Warning", "205 - Jungle", "240 - CaliCamo", "251 - Pit Viper", "393 - Tranquility", "320 - Red Python", "314 - Heaven Guard", "348 - Red Leather", "370 - Bone Machine", "407 - Quicksilver" });
            skinList[16].InsertRange(skinList[16].Count, new string[] { "None", "13 - Blue Streak", "164 - Modern Hunter", "25 - Forest Leaves", "27 - Bone Mask", "70 - Carbon Fiber", "148 - Sand Dashed", "149 - Urban Dashed", "159 - Brass", "235 - VariCamo", "171 - Irradiated Alert", "203 - Rust Coat", "224 - Water Sigil", "236 - Night Ops", "267 - Cobalt Halftone", "306 - Antique", "323 - Rust Coat", "349 - Osiris", "376 - Chemical Green", "457 - Bamboo Print", "459 - Bamboo Forest" });
            skinList[17].InsertRange(skinList[17].Count, new string[] { "None", "462 - Counter Terrace", "34 - Metallic DDPAT", "32 - Silver", "100 - Storm", "39 - Bulldozer", "431 - Heat", "99 - Sand Dune", "171 - Irradiated Alert", "177 - Memento", "198 - Hazard", "291 - Heaven Guard", "385 - Firestarter", "473 - Seabird" });
            skinList[18].InsertRange(skinList[18].Count, new string[] { "None", "28 - Anodized Navy", "432 - Man-o'-war", "157 - Palm", "235 - VariCamo", "201 - Palm", "240 - CaliCamo", "285 - Terrain", "298 - Army Sheen", "317 - Bratatat", "355 - Desert-Strike", "369 - Nuclear Waste", "483 - Loudmouth" });
            skinList[19].InsertRange(skinList[19].Count, new string[] { "None", "345 - First Class", "5 - Forest DDPAT", "22 - Contrast Spray", "30 - Snake Camo", "83 - Orange DDPAT", "38 - Fade", "41 - Copper", "434 - Origami", "119 - Sage Spray", "235 - VariCamo", "171 - Irradiated Alert", "204 - Mosaico", "405 - Serenity", "246 - Amber Fade", "250 - Full Stop", "390 - Highwayman", "256 - The Kraken", "323 - Rust Coat", "458 - Bamboo Shadow", "459 - Bamboo Forest" });
            skinList[20].InsertRange(skinList[20].Count, new string[] { "None", "101 - Tornado", "2 - Groundwater", "5 - Forest DDPAT", "463 - Terrace", "17 - Urban DDPAT", "36 - Ossified", "439 - Hades", "159 - Brass", "235 - VariCamo", "179 - Nuclear Threat", "248 - Red Quartz", "206 - Tornado", "216 - Blue Titanium", "242 - Army Mesh", "272 - Titanium Bit", "289 - Sandstorm", "303 - Isaac", "374 - Toxic", "459 - Bamboo Forest" });
            skinList[21].InsertRange(skinList[21].Count, new string[] { "None", "104 - Grassland Leaves", "32 - Silver", "21 - Granite Marbleized", "25 - Forest Leaves", "36 - Ossified", "485 - Handgun", "38 - Fade", "71 - Scorpion", "95 - Grassland", "184 - Corticera", "211 - Ocean Foam", "338 - Pulse", "246 - Amber Fade", "275 - Red FragCam", "327 - Chainmail", "346 - Coach Class", "357 - Ivory", "389 - Fire Elemental", "442 - Asterion", "443 - Pathfinder" });
            skinList[22].InsertRange(skinList[22].Count, new string[] { "None", "2 - Groundwater", "102 - Whiteout", "5 - Forest DDPAT", "28 - Anodized Navy", "11 - Skulls", "15 - Gunsmoke", "22 - Contrast Spray", "27 - Bone Mask", "36 - Ossified", "141 - Orange Peel", "235 - VariCamo", "245 - Army Recon", "209 - Groundwater", "213 - Ocean Foam", "250 - Full Stop", "354 - Urban Hazard", "365 - Olive Plaid", "423 - Armor Core", "442 - Asterion", "481 - Nemesis" });
            skinList[23].InsertRange(skinList[23].Count, new string[] { "None", "482 - Ruby Poison Dart", "27 - Bone Mask", "33 - Hot Rod", "100 - Storm", "39 - Bulldozer", "61 - Hypnotic", "148 - Sand Dashed", "141 - Orange Peel", "199 - Dry Season", "329 - Dark Age", "262 - Rose Iron", "366 - Green Plaid", "368 - Setting Sun", "386 - Dart", "403 - Deadly Poison", "448 - Pandora's Box" });
            skinList[24].InsertRange(skinList[24].Count, new string[] { "None", "3 - Candy Apple", "166 - Blaze Orange", "164 - Modern Hunter", "25 - Forest Leaves", "62 - Bloomstick", "99 - Sand Dune", "107 - Polar Mesh", "158 - Walnut", "170 - Predator", "191 - Tempest", "214 - Graphite", "225 - Ghost Camo", "263 - Rising Skull", "286 - Antique", "294 - Green Apple", "299 - Caged Steel", "356 - Koi", "450 - Moon in Libra", "484 - Ranger" });
            skinList[25].InsertRange(skinList[25].Count, new string[] { "None", "102 - Whiteout", "34 - Metallic DDPAT", "162 - Splash", "15 - Gunsmoke", "164 - Modern Hunter", "27 - Bone Mask", "77 - Boreal Forest", "99 - Sand Dune", "168 - Nuclear Threat", "258 - Mehndi", "207 - Facets", "219 - Hive", "404 - Muertos", "230 - Steel Disruption", "271 - Undertow", "295 - Franklin", "464 - Neon Kimono", "358 - Supernova", "373 - Contamination", "388 - Cartel", "426 - Valence", "466 - Crimson Kimono", "467 - Mint Kimono" });
            skinList[26].InsertRange(skinList[26].Count, new string[] { "None", "165 - Splash Jam", "100 - Storm", "46 - Contractor", "70 - Carbon Fiber", "116 - Sand Mesh", "157 - Palm", "196 - Emerald", "232 - Crimson Web", "391 - Cardiac", "298 - Army Sheen", "312 - Cyrex", "406 - Grotto", "453 - Emerald" });
            skinList[27].InsertRange(skinList[27].Count, new string[] { "None", "101 - Tornado", "28 - Anodized Navy", "22 - Contrast Spray", "27 - Bone Mask", "39 - Bulldozer", "98 - Ultraviolet", "136 - Waves Perforated", "410 - Damascus Steel", "169 - Fallout Warning", "186 - Wave Spray", "243 - Gator Mesh", "247 - Damascus Steel", "287 - Pulse", "298 - Army Sheen", "363 - Traveler", "378 - Fallout Warning", "487 - Cyrex" });
            skinList[28].InsertRange(skinList[28].Count, new string[] { "None", "26 - Lichen Dashed", "60 - Dark Water", "96 - Blue Spruce", "99 - Sand Dune", "157 - Palm", "200 - Mayan Dreams", "222 - Blood in the Water", "233 - Tropical Storm", "253 - Acid Fade", "304 - Slashed", "319 - Detour", "361 - Abyss" });
            skinList[29].InsertRange(skinList[29].Count, new string[] { "None", "430 - Hyper Beast", "77 - Boreal Forest", "235 - VariCamo", "254 - Nitro", "189 - Bright Water", "301 - Atomic Alloy", "217 - Blood Tiger", "257 - Guardian", "321 - Master Piece", "326 - Knight", "360 - Cyrex", "383 - Basilisk", "440 - Icarus Fell", "445 - Hot Rod" });
            skinList[30].InsertRange(skinList[30].Count, new string[] { "None", "25 - Forest Leaves", "60 - Dark Water", "235 - VariCamo", "183 - Overgrowth", "339 - Caiman", "217 - Blood Tiger", "221 - Serum", "236 - Night Ops", "277 - Stainless", "290 - Guardian", "313 - Orion", "318 - Road Rash", "332 - Royal Blue", "364 - Business Class", "454 - Para Green", "489 - Torque" });
            skinList[31].InsertRange(skinList[31].Count, new string[] { "None", "435 - Pole Position", "12 - Crimson Web", "254 - Nitro", "218 - Hexane", "268 - Tread Plate", "269 - The Fuschia Is Now", "270 - Victoria", "297 - Tuxedo", "315 - Poison Dart", "322 - Nitro", "325 - Chalice", "334 - Twist", "350 - Tigris", "366 - Green Plaid", "453 - Emerald", "476 - Yellow Jacket" });
            skinList[32].InsertRange(skinList[32].Count, new string[] { "None", "552 - Fade", "523 - Amber Fade", "12 - Crimson Web", "27 - Bone Mask" });

            //set listboxes
            listBox2.Items.Clear();
            listBox2.Items.AddRange(skinList[0].ToArray());
        }

        private void PaintPanels()
        {
            panel1.BackColor = Color.FromArgb(255, nsTrackBar1.Value, nsTrackBar2.Value, nsTrackBar3.Value);
            panel2.BackColor = Color.FromArgb(255, nsTrackBar4.Value, nsTrackBar5.Value, nsTrackBar6.Value);
        }

        private void glowPlayer(int mObj, float r, float g, float b, float a)
        {
            Mem.WriteFloat(mObj + 0x4, r);
            Mem.WriteFloat(mObj + 0x8, g);
            Mem.WriteFloat(mObj + 0xC, b);
            Mem.WriteFloat(mObj + 0x10, a);
            Mem.WriteBool(mObj + 0x24, true);
            Mem.WriteBool(mObj + 0x25, false);
        }

        public void ESPmain()
        {
            while(true)
            {
                if (nsOnOffBox2.Checked || nsOnOffBox3.Checked || nsOnOffBox4.Checked)
                {
                    int Epointer = Mem.ReadInt(Engine.Base + Offs.m_dwClientState);
                    string active = GetActiveWindowTitle();
                    if (Mem.ReadInt(Epointer + Offs.m_dwInGame) == 6 && active == "Counter-Strike: Global Offensive")
                    {
                        int LocalPlayer = Mem.ReadInt(Client.Base + Offs.m_dwLocalPlayer);
                        int LocalTeam = Mem.ReadInt(LocalPlayer + Offs.m_iTeamNum);
                        int maxObj = Mem.ReadInt(Client.Base + Offs.m_dwGlowObject + 0x4);
                        int GlowPointer = Mem.ReadInt(Client.Base + Offs.m_dwGlowObject);

                        for (int i = 0; i < maxObj; i++)
                        {
                            int mObj = GlowPointer + i * 56;
                            int dwBase = Mem.ReadInt(mObj);
                            if (dwBase != 0)
                            {
                                int EntityTeam = Mem.ReadInt(dwBase + Offs.m_iTeamNum);
                                bool EntityDormant = Mem.ReadBool(dwBase + Offs.m_bDormant);
                                bool EntityLifeState = Mem.ReadBool(dwBase + Offs.m_lifeState);
                                int classid = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(dwBase + 0x8) + 2 * 0x4) + 0x1) + 20);

                                if (nsOnOffBox2.Checked && classid == 35)
                                {
                                    if (!EntityLifeState)
                                    {
                                        if (!EntityDormant)
                                        {
                                            if (LocalTeam == EntityTeam)
                                            {
                                                if(nsRadioButton1.Checked)
                                                glowPlayer(mObj, (float)(nsTrackBar1.Value) / 255, (float)(nsTrackBar2.Value) / 255, (float)(nsTrackBar3.Value) / 255, 0.877f);
                                                if (nsRadioButton2.Checked)
                                                {
                                                    glowPlayer(mObj, 1, 1, 0, 0.63f);
                                                }
                                                if(nsRadioButton3.Checked)
                                                {
                                                    glowPlayer(mObj, (float)(nsTrackBar1.Value) / 255, (float)(nsTrackBar2.Value) / 255, (float)(nsTrackBar3.Value) / 255, 0.877f);
                                                }
                                            }
                                            if (LocalTeam != EntityTeam)
                                            {
                                                if (nsRadioButton1.Checked)
                                                glowPlayer(mObj, (float)(nsTrackBar4.Value) / 255, (float)(nsTrackBar5.Value) / 255, (float)(nsTrackBar6.Value) / 255, 0.877f);
                                                if(nsRadioButton2.Checked)
                                                {
                                                    int Health = Mem.ReadInt(dwBase + Offs.m_iHealth);
                                                    float red = 255.0f - ((float)Health * 2.55f);
                                                    float green = (float)Health * 2.55f;
                                                    glowPlayer(mObj, red / 255, green / 255, 0, 0.78f);
                                                }
                                                if (nsRadioButton3.Checked)
                                                {
                                                    bool vulnerable = false;
                                                    int armor = Mem.ReadInt(dwBase + Offs.m_ArmorValue);
                                                    if(!vulnerable && nsCheckBox4.Checked && armor == 0)
                                                    {
                                                        vulnerable = true;
                                                    }
                                                    float flashDur = Mem.ReadFloat(dwBase + Offs.m_flFlashDuration);
                                                    if (!vulnerable && flashDur > 0.5f && nsCheckBox5.Checked)
                                                    {
                                                        vulnerable = true;
                                                    }
                                                    int Health = Mem.ReadInt(dwBase + Offs.m_iHealth);
                                                    if (!vulnerable && nsCheckBox6.Checked && Health < 30) 
                                                    {
                                                        vulnerable = true;
                                                    }
                                                    if(vulnerable)
                                                    {

                                                        glowPlayer(mObj, 1, 0.5f, 0, 0.877f);
                                                    }
                                                    else
                                                    {
                                                        glowPlayer(mObj, (float)(nsTrackBar4.Value) / 255, (float)(nsTrackBar5.Value) / 255, (float)(nsTrackBar6.Value) / 255, 0.877f);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if(nsOnOffBox3.Checked && classid == 29)
                                {
                                    if (!EntityDormant)
                                    {
                                        glowPlayer(mObj, 0, 1, 1, 0.877f);
                                    }
                                }
                                if (nsOnOffBox4.Checked && (classid == 240 || classid == 127 || classid == 94 || classid == 41 || classid == 9 || classid == 13 || classid == 236 || classid == 231 || classid == 126 || classid == 228 || classid == 224 || classid == 220 || classid == 8 || classid == 85 || classid == 93 || classid == 212 || classid == 90 || classid == 89 || classid == 82 || classid == 64 || classid == 203 || classid == 40 || classid == 39 || classid == 235 || classid == 211 || classid == 234 || classid == 233 || classid == 232 || classid == 230 || classid == 229 || classid == 227 || classid == 226 || classid == 225 || classid == 122 || classid == 223 || classid == 222 || classid == 221 || classid == 219 || classid == 218 || classid == 217 || classid == 216 || classid == 215 || classid == 214 || classid == 213 || classid == 210 || classid == 209 || classid == 208 || classid == 207 || classid == 206 || classid == 205 || classid == 204 || classid == 199 || classid == 198 || classid == 197 || classid == 1 || classid == 201 || classid == 200 || classid == 8 ||  classid == 176 || classid == 104 || classid == 83 || classid == 115))
                                {
                                    if(!EntityDormant)
                                    {
                                        glowPlayer(mObj, 1, 1, 1, 0.877f);
                                    }
                                }
                            }

                        }
                    }
                }
                
                Thread.Sleep(1);
            }
        }

        
        public void TriggerMain()
        {
            while(true)
            {
                int Epointer = Mem.ReadInt(Engine.Base + Offs.m_dwClientState);
                string active = GetActiveWindowTitle();
                if (Mem.ReadInt(Epointer + Offs.m_dwInGame) == 6 && nsOnOffBox5.Checked && active == "Counter-Strike: Global Offensive") 
                {
                    if (IsKeyPushedDown((Keys)KeySettings.TriggerHold) || nsOnOffBox7.Checked ) 
                    {
                        int LocalPlayer = Mem.ReadInt(Client.Base + Offs.m_dwLocalPlayer);
                        int LocalTeam = Mem.ReadInt(LocalPlayer + Offs.m_iTeamNum);
                        int CHID = Mem.ReadInt(LocalPlayer + Offs.m_iCrossHairID) - 1;
                        bool isValidWep = false;
                        int weaponIndex = Mem.ReadInt(LocalPlayer + Offs.m_hActiveWeapon) & 0xFFF;
                        int weaponEntity = Mem.ReadInt((Client.Base + Offs.m_dwEntityList + weaponIndex * 0x10) - 0x10);
                        int WeaponID = Mem.ReadInt(weaponEntity + Offs.m_iItemDefinitionIndex);

                        if (nsCheckBox16.Checked && IsWeaponPistol(WeaponID)) isValidWep = true;
                        if (nsCheckBox15.Checked && IsWeaponRifle(WeaponID)) isValidWep = true;
                        if (nsCheckBox14.Checked && IsWeaponShotgun(WeaponID)) isValidWep = true;
                        if (nsCheckBox13.Checked && IsWeaponSniper(WeaponID)) isValidWep = true;
                        if (nsCheckBox12.Checked && IsWeaponSMG(WeaponID)) isValidWep = true;
                        int maxObj = Mem.ReadInt(Client.Base + Offs.m_dwGlowObject + 0x4);
                        if (CHID >=0 && CHID < maxObj) 
                        {
                            int BaseEntity = Mem.ReadInt(Client.Base + Offs.m_dwEntityList + (0x10 * CHID));
                            int EntityTeam = Mem.ReadInt(BaseEntity + Offs.m_iTeamNum);
                            if(!Mem.ReadBool(BaseEntity + Offs.m_lifeState) && Mem.ReadInt(Client.Base + Offs.m_dwForceAttack) == 4)
                            {
                                if(!Mem.ReadBool(BaseEntity + Offs.m_bDormant) && !Mem.ReadBool(BaseEntity + Offs.m_bGunGameImmunity))
                                {
                                    if((nsOnOffBox6.Checked && (EntityTeam ==3 || EntityTeam ==2 )) || (!nsOnOffBox6.Checked) && (EntityTeam != LocalTeam))
                                    {
                                        if(isValidWep)
                                        {
                                            if((nsOnOffBox8.Checked && Mem.ReadBool(LocalPlayer + Offs.m_bIsScoped))|| !nsOnOffBox8.Checked || !IsWeaponSniper(WeaponID))
                                            {
                                                if(nsOnOffBox9.Checked)
                                                {
                                                    if (nsOnOffBox10.Checked)
                                                    {
                                                        if(Mem.ReadInt(weaponEntity + Offs.m_iClip1)>=nsTrackBar10.Value && !IsWeaponPistol(WeaponID) && !IsWeaponShotgun(WeaponID) && !IsWeaponSniper(WeaponID))
                                                        {
                                                            Thread.Sleep(nsTrackBar9.Value);
                                                            while(Mem.ReadInt(LocalPlayer + Offs.m_iShotsFired)<nsTrackBar10.Value)
                                                            {
                                                                Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 5);
                                                            }
                                                            Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 4);
                                                        }
                                                        else
                                                        {
                                                            Thread.Sleep(nsTrackBar9.Value);
                                                            Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 5);
                                                            Thread.Sleep(10);
                                                            Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 4);
                                                            Thread.Sleep(10);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Thread.Sleep(nsTrackBar9.Value);
                                                        Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 5);
                                                        Thread.Sleep(10);
                                                        Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 4);
                                                        Thread.Sleep(10);
                                                    }
                                                }
                                                if(!nsOnOffBox9.Checked)
                                                {
                                                    if (nsOnOffBox10.Checked)
                                                    {
                                                        if (Mem.ReadInt(weaponEntity + Offs.m_iClip1) >= nsTrackBar10.Value && !IsWeaponPistol(WeaponID) && !IsWeaponShotgun(WeaponID) && !IsWeaponSniper(WeaponID))
                                                        {
                                                            while (Mem.ReadInt(LocalPlayer + Offs.m_iShotsFired) < nsTrackBar10.Value)
                                                            {
                                                                Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 5);
                                                            }
                                                            Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 4);
                                                        }
                                                        else
                                                        {
                                                            Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 5);
                                                            Thread.Sleep(10);
                                                            Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 4);
                                                            Thread.Sleep(10);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 5);
                                                        Thread.Sleep(10);
                                                        Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 4);
                                                        Thread.Sleep(10);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        public void SkinChangerMain()
        {
            int oldWep = 0;
            while (true)
            {
                int Epointer = Mem.ReadInt(Engine.Base + Offs.m_dwClientState);
                string active = GetActiveWindowTitle();
                if (Mem.ReadInt(Epointer + Offs.m_dwInGame) == 6 && nsOnOffBox11.Checked && active == "Counter-Strike: Global Offensive")
                {
                    int LocalPlayer = Mem.ReadInt(Client.Base + Offs.m_dwLocalPlayer);
                    int weaponIndex = Mem.ReadInt(LocalPlayer + Offs.m_hActiveWeapon) & 0xFFF;
                    int weaponEntity = Mem.ReadInt((Client.Base + Offs.m_dwEntityList + weaponIndex * 0x10) - 0x10);
                    int WeaponID = Mem.ReadInt(weaponEntity + Offs.m_iItemDefinitionIndex);
                    if(oldWep != WeaponID)
                    {
                        int realId = 0;
                        if (WeaponID == 1) realId = 1;
                        if (WeaponID == 2) realId = 2;
                        if (WeaponID == 3) realId = 3;
                        if (WeaponID == 4) realId = 4;
                        if (WeaponID == 7) realId = 5;
                        if (WeaponID == 8) realId = 6;
                        if (WeaponID == 9) realId = 7;
                        if (WeaponID == 10) realId = 8;
                        if (WeaponID == 11) realId = 9;
                        if (WeaponID == 13) realId = 10;
                        if (WeaponID == 14) realId = 11;
                        if (WeaponID == 16) realId = 12;
                        if (WeaponID == 17) realId = 13;
                        if (WeaponID == 19) realId = 14;
                        if (WeaponID == 24) realId = 15;
                        if (WeaponID == 25) realId = 16;
                        if (WeaponID == 26) realId = 17;
                        if (WeaponID == 27) realId = 18;
                        if (WeaponID == 28) realId = 19;
                        if (WeaponID == 29) realId = 20;
                        if (WeaponID == 30) realId = 21;
                        if (WeaponID == 32) realId = 22;
                        if (WeaponID == 33) realId = 23;
                        if (WeaponID == 34) realId = 24;
                        if (WeaponID == 35) realId = 25;
                        if (WeaponID == 36) realId = 26;
                        if (WeaponID == 38) realId = 27;
                        if (WeaponID == 39) realId = 28;
                        if (WeaponID == 40) realId = 29;
                        if (WeaponID == 60) realId = 30;
                        if (WeaponID == 61) realId = 31;
                        if (WeaponID == 63) realId = 32;
                        if (WeaponID == 64) realId = 33;

                        if (realId != 0 && CustomSkins[realId - 1].skinId != 0 && !IsWeaponNonAim(WeaponID)) 
                        {
                            realId--;
                            Mem.WriteInt(weaponEntity + Offs.m_iAccountID, Mem.ReadInt(weaponEntity + Offs.m_OriginalOwnerXuidLow));
                            Mem.WriteInt(weaponEntity + Offs.m_iItemIDHigh, 1);
                            Mem.WriteInt(weaponEntity + Offs.m_iEntityQuality, 1);
                            Mem.WriteInt(weaponEntity + Offs.m_nFallbackPaintKit, CustomSkins[realId].skinId);
                            Mem.WriteFloat(weaponEntity + Offs.m_flFallbackWear, 0.001f);
                            if(nsOnOffBox12.Checked)
                            {
                                Mem.WriteInt(weaponEntity + Offs.m_nFallbackStatTrak, (int)numericUpDown2.Value);
                            }
                            if(nsOnOffBox25.Checked && Mem.ReadInt(weaponEntity + Offs.m_iClip1) != -1)
                            {
                                if (Mem.ReadInt(Epointer + 0x16C) != -1) 
                                Mem.WriteInt(Epointer + 0x16C, -1);
                            }
                            if(nsOnOffBox13.Checked)
                            {
                                Mem.WriteStringAscii(weaponEntity + Offs.m_szCustomName, nsTextBox1.Text);
                            }
                        }

                        oldWep = Mem.ReadInt(weaponEntity + Offs.m_iItemDefinitionIndex);
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        public float GetRandomNumber(Random random, double minimum, double maximum)
        {
            return (float)(random.NextDouble() * (maximum - minimum) + minimum);
        }

        public void RCSMain()
        {
            Random random = new Random();
            Vector2 prevPunch = new Vector2(0, 0);
            while(true)
            {
                int Epointer = Mem.ReadInt(Engine.Base + Offs.m_dwClientState);
                string active = GetActiveWindowTitle();
                if (Mem.ReadInt(Epointer + Offs.m_dwInGame) == 6 && nsOnOffBox14.Checked && active == "Counter-Strike: Global Offensive")
                {
                    float modifier = 2.0f;
                    if (nsOnOffBox15.Checked)
                    {
                        modifier = GetRandomNumber(random, 1.7, 2.0);
                    }

                    float vertical = modifier * ((float)nsTrackBar11.Value / 100);
                    float horizontal = modifier * ((float)nsTrackBar12.Value / 100);
                    int LocalPlayer = Mem.ReadInt(Client.Base + Offs.m_dwLocalPlayer);
                    int shotsFired = Mem.ReadInt(LocalPlayer + Offs.m_iShotsFired);
                    int weaponIndex = Mem.ReadInt(LocalPlayer + Offs.m_hActiveWeapon) & 0xFFF;
                    int weaponEntity = Mem.ReadInt((Client.Base + Offs.m_dwEntityList + weaponIndex * 0x10) - 0x10);
                    int WeaponID = Mem.ReadInt(weaponEntity + Offs.m_iItemDefinitionIndex);

                    Vector2 punch = Mem.ReadVec2(LocalPlayer + Offs.m_vecPunch);
                    punch.x *= vertical;
                    punch.y *= horizontal;
                    if(shotsFired>1 &&!IsWeaponNonAim(WeaponID) && !IsWeaponPistol(WeaponID) && !IsWeaponShotgun(WeaponID) && !IsWeaponSniper(WeaponID)) 
                    {
                        Vector2 angles = Mem.ReadVec2(Epointer + Offs.m_dwViewAngles);
                        angles.x -= punch.x - prevPunch.x;
                        angles.y -= punch.y - prevPunch.y;
                        Mem.WriteVec2(Epointer + Offs.m_dwViewAngles, angles);
                    }
                    prevPunch = punch; 
                }
                Thread.Sleep(2);
            }
        }

        public void MiscMain()
        {
            float mainSens = Mem.ReadFloat(Client.Base + Offs.m_dwSensitivity);
            while(true)
            {
                int Epointer = Mem.ReadInt(Engine.Base + Offs.m_dwClientState);
                string active = GetActiveWindowTitle();
                if (Mem.ReadInt(Epointer + Offs.m_dwInGame) == 6 && (nsOnOffBox16.Checked || nsOnOffBox17.Checked || nsOnOffBox18.Checked || nsOnOffBox19.Checked || nsOnOffBox20.Checked) && active == "Counter-Strike: Global Offensive")
                {
                    int LocalPlayer = Mem.ReadInt(Client.Base + Offs.m_dwLocalPlayer);
                    if(nsOnOffBox16.Checked)
                    {
                        if(Mem.ReadFloat(LocalPlayer + Offs.m_flFlashDuration)>0)
                        {
                            Mem.WriteFloat(LocalPlayer + Offs.m_flFlashDuration, 0);
                        }
                    
                    }
                    if (nsOnOffBox17.Checked)
                    {
                        int fflags = Mem.ReadInt(LocalPlayer + Offs.m_fFlags);
                        bool mouseEnabled = Mem.ReadBool(Client.Base + Offs.m_dwMouseEnable);
                        if ((mouseEnabled) && IsKeyPushedDown(Keys.Space)) 
                        {
                            if(fflags ==257)
                            Mem.WriteByte(Client.Base + Offs.m_dwForceJump, 5);
                            if (fflags != 257)
                            Mem.WriteByte(Client.Base + Offs.m_dwForceJump, 4);
                        }
                    }
                    if(nsOnOffBox18.Checked)
                    {
                        int localTeam = Mem.ReadInt(LocalPlayer + Offs.m_bSpotted);
                        for(int i=0;i<64;i++)
                        {
                            int BaseEntity = Mem.ReadInt(Client.Base + Offs.m_dwEntityList + (i * 0x10));
                            int EntityTeam = Mem.ReadInt(BaseEntity + Offs.m_iTeamNum);
                            if(EntityTeam != localTeam && (EntityTeam ==2 || EntityTeam ==3))
                            {
                                if(!Mem.ReadBool(BaseEntity + Offs.m_bDormant))
                                {
                                    if(!Mem.ReadBool(BaseEntity + Offs.m_lifeState))
                                    {
                                        Mem.WriteBool(BaseEntity + Offs.m_bSpotted, true);
                                    }
                                }
                            }
                        }
                    }
                    if(nsOnOffBox19.Checked)
                    {
                        int weaponIndex = Mem.ReadInt(LocalPlayer + Offs.m_hActiveWeapon) & 0xFFF;
                        int weaponEntity = Mem.ReadInt((Client.Base + Offs.m_dwEntityList + weaponIndex * 0x10) - 0x10);
                        int WeaponID = Mem.ReadInt(weaponEntity + Offs.m_iItemDefinitionIndex);
                        if (IsWeaponPistol(WeaponID))
                        {
                            bool mouseEnabled = Mem.ReadBool(Client.Base + Offs.m_dwMouseEnable);
                            if (IsKeyPushedDown(Keys.LButton) && mouseEnabled)
                            {
                                Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 5);
                                Thread.Sleep(10);
                                Mem.WriteByte(Client.Base + Offs.m_dwForceAttack, 4);
                                Thread.Sleep(10);
                            }
                        }
                    }
                    if(nsOnOffBox20.Checked)
                    {
                        int CHID = Mem.ReadInt(LocalPlayer + Offs.m_iCrossHairID) -1;
                        int localTeam = Mem.ReadInt(LocalPlayer + Offs.m_bSpotted);
                        if (CHID >= 0 && CHID < 64)
                        {
                            int BaseEntity = Mem.ReadInt(Client.Base + Offs.m_dwEntityList + (0x10 * CHID));
                            int EntityTeam = Mem.ReadInt(BaseEntity + Offs.m_iTeamNum);
                            if(EntityTeam != localTeam && (EntityTeam == 2 || EntityTeam ==3))
                            {
                                if(!Mem.ReadBool(BaseEntity + Offs.m_bDormant))
                                {
                                    if (!Mem.ReadBool(BaseEntity + Offs.m_lifeState))
                                    {
                                        float newSens = mainSens *((float)nsTrackBar13.Value / 100);
                                        Mem.WriteFloat(Client.Base + Offs.m_dwSensitivity, newSens);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if(Mem.ReadFloat(Client.Base + Offs.m_dwSensitivity) != mainSens)
                            {
                                Mem.WriteFloat(Client.Base + Offs.m_dwSensitivity, mainSens);
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        float calcDist(Vector3 myPos, Vector3 enPos)
        {
            return (float)Math.Sqrt((myPos.x - enPos.x) * (myPos.x - enPos.x) + (myPos.y - enPos.y) * (myPos.y - enPos.y) + (myPos.z - enPos.z) * (myPos.z - enPos.z));
        }

        Vector2 CalcAngle(Vector3 src, Vector3 dst, float origin)
        {
            Vector2 ReturnVec = new Vector2();


            Vector3 delta = new Vector3((src.x - dst.x), (src.y - dst.y), (src.z + origin - dst.z));
            float hyp = (float)Math.Sqrt((double)(delta.x * delta.x + delta.y * delta.y));
            ReturnVec.x = (float)Math.Atan((double)(delta.z / hyp));
            ReturnVec.y = (float)Math.Atan((double)(delta.y / delta.x));
            ReturnVec.x *= (180.0f / (float)Math.PI);
            ReturnVec.y *= (180.0f / (float)Math.PI);
            if(delta.x >=0.0f)
            {
                ReturnVec.y += 180.0f;
            }
            return ReturnVec;
        }

        bool verifyAngles(Vector2 angle)
        {
            if (angle.x <= 89.0f && angle.x >= -89.0f && angle.y <= 180.0f && angle.y >= -180.0f) return true;
            return false;
        }

        Vector2 clampAngles(Vector2 angles)
        {
            while (angles.y < -180.0f) angles.y += 360.0f;
            while (angles.y > 180.0f) angles.y -= 360.0f;
            if (angles.x > 89.0f) angles.x = 89.0f;
            if (angles.x < -89.0f) angles.x = -89.0f;

            return angles;
        }

        Vector2 GetFov(Vector2 pAimAngles, Vector2 pLocalAngles)
        {
            Vector2 pFov;
            pFov.x = (pAimAngles.x - pLocalAngles.x);
            pFov.y = (pAimAngles.y - pLocalAngles.y);
            if (pFov.x > 180) pFov.x -= 360;
            if (pFov.y > 180) pFov.y -= 360;
            if (pFov.x < -180) pFov.x += 360;
            if (pFov.y < -180) pFov.y += 360;
            if (pFov.x < 0) pFov.x *= (-1);
            if (pFov.y < 0) pFov.y *= (-1);
            return pFov;
        }

        float AngleDistance(float next, float cur)
        {
            float delta = next - cur;

            if (delta < -180)
                delta += 360;
            else if (delta > 180)
                delta -= 360;

            return delta;
        }

        Vector2 Smooth(Vector2 src, Vector2 flLocalAngles, int smooth)
        {
            if (smooth<=0)
            {
                smooth =1;
            }
            Vector2 back;
            Vector2 smoothdiff;
            src.x -= flLocalAngles.x;
            src.y -= flLocalAngles.y;
            if (src.x > 180) src.x -= 360;
            if (src.y > 180) src.y -= 360;
            if (src.x < -180) src.x += 360;
            if (src.y < -180) src.y += 360;
            smoothdiff.x = src.x / smooth;
            smoothdiff.y = src.y / smooth;
            back.x = flLocalAngles.x + smoothdiff.x;
            back.y = flLocalAngles.y + smoothdiff.y;
            if (back.x > 180) back.x -= 360;
            if (back.y > 180) back.y -= 360;
            if (back.x < -180) back.x += 360;
            if (back.y < -180) back.y += 360;
            return back;
        }

        float Dist2d(Vector2 first, Vector2 second)
        {
            float dx = first.x - second.x;
            float dy = first.y - first.y;
            return (float)(Math.Sqrt((double)(dx*dx + dy*dy)));
        }

        public void AimMain()
        {
            while(true)
            {
                int Epointer = Mem.ReadInt(Engine.Base + Offs.m_dwClientState);
                string active = GetActiveWindowTitle();
                if (Mem.ReadInt(Epointer + Offs.m_dwInGame) == 6 && (AimSettings[0].enabled || AimSettings[1].enabled ||AimSettings[2].enabled ||AimSettings[3].enabled) && active == "Counter-Strike: Global Offensive")
                {
                    //local
                    int LocalPlayer = Mem.ReadInt(Client.Base + Offs.m_dwLocalPlayer);
                    int LocalTeam = Mem.ReadInt(LocalPlayer + Offs.m_iTeamNum);
                    int LocalId = Mem.ReadInt(LocalPlayer + Offs.m_dwIndex);

                    //wep
                    int weaponIndex = Mem.ReadInt(LocalPlayer + Offs.m_hActiveWeapon) & 0xFFF;
                    int weaponEntity = Mem.ReadInt((Client.Base + Offs.m_dwEntityList + weaponIndex * 0x10) - 0x10);
                    int WeaponID = Mem.ReadInt(weaponEntity + Offs.m_iItemDefinitionIndex);
                    int asd = weaponEntity + Offs.m_iItemDefinitionIndex;

                    Vector2 LocalAngles = Mem.ReadVec2(Epointer + Offs.m_dwViewAngles);
                    Vector3 LocalPos = Mem.ReadVec3(LocalPlayer + Offs.m_vecOrigin);
                    float origin = Mem.ReadFloat(LocalPlayer + Offs.m_vecViewOffset + 0x8);

                    if (AimSettings[0].enabled && IsKeyPushedDown((Keys)KeySettings.AimLockHold)) // AIMLOCK
                    {
                        if (!IsWeaponNonAim(WeaponID))
                        {
                            if (IsKeyPushedDown((Keys)KeySettings.AimLockHold))
                            {
                                float tempFov = 99999.0f;
                                int selectedEntity = -1;
                                for (int i = 0; i < 64; i++)
                                {
                                    int BaseEntity = Mem.ReadInt(Client.Base + Offs.m_dwEntityList + (0x10 * i));
                                    int EntityTeam = Mem.ReadInt(BaseEntity + Offs.m_iTeamNum);
                                    int classid = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(BaseEntity + 0x8) + 2 * 0x4) + 0x1) + 20);
                                    if (classid == 35)
                                    {
                                        if (!Mem.ReadBool(BaseEntity + Offs.m_bGunGameImmunity))
                                        {
                                            if (!Mem.ReadBool(BaseEntity + Offs.m_lifeState))
                                            {
                                                if (!Mem.ReadBool(BaseEntity + Offs.m_bDormant))
                                                {
                                                    if (EntityTeam != LocalTeam && (EntityTeam == 2 || EntityTeam == 3))
                                                    {
                                                        int tempMask = Mem.ReadInt(BaseEntity + Offs.m_bSpottedByMask);
                                                        int pid = LocalId - 1;
                                                        if ((tempMask & (0x1 << pid)) != 0 || !AimSettings[0].visCheck)
                                                        {
                                                            Vector3 enemyPos = new Vector3();
                                                            int BoneMatrix = Mem.ReadInt(BaseEntity + Offs.m_dwBoneMatrix);
                                                            enemyPos.x = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[0].aimBone + 0x0C);
                                                            enemyPos.y = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[0].aimBone + 0x1C);
                                                            enemyPos.z = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[0].aimBone + 0x2C);

                                                            Vector2 tempAngle = CalcAngle(LocalPos, enemyPos, origin);
                                                            Vector2 fov = GetFov(tempAngle, LocalAngles);
                                                            float fovSum = fov.x + fov.y;
                                                            if (fovSum < tempFov && fov.x < AimSettings[0].FOV && fov.y < AimSettings[0].FOV)
                                                            {
                                                                tempFov = fovSum;
                                                                selectedEntity = i;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                while(IsKeyPushedDown((Keys)KeySettings.AimLockHold) && selectedEntity !=-1)
                                {
                                    LocalAngles = Mem.ReadVec2(Epointer + Offs.m_dwViewAngles);
                                    LocalPos = Mem.ReadVec3(LocalPlayer + Offs.m_vecOrigin);
                                    origin = Mem.ReadFloat(LocalPlayer + Offs.m_vecViewOffset + 0x8);

                                    int BaseEntity = Mem.ReadInt(Client.Base + Offs.m_dwEntityList + (0x10 * selectedEntity));

                                    if(AimSettings[0].noSticky && Mem.ReadBool(BaseEntity + Offs.m_lifeState))
                                    {
                                        break;
                                    }
                                    if(AimSettings[0].aimFlashed && Mem.ReadFloat(LocalPlayer + Offs.m_flFlashDuration) > 0.0f)
                                    {
                                        break;
                                    }

                                    int tempMask = Mem.ReadInt(BaseEntity + Offs.m_bSpottedByMask);
                                    int pid = LocalId - 1;
                                    if ((tempMask & (0x1 << pid)) != 0 || !AimSettings[0].visCheck)
                                    {
                                        Vector3 enemyPos = new Vector3();
                                        int BoneMatrix = Mem.ReadInt(BaseEntity + Offs.m_dwBoneMatrix);
                                        enemyPos.x = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[0].aimBone + 0x0C);
                                        enemyPos.y = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[0].aimBone + 0x1C);
                                        enemyPos.z = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[0].aimBone + 0x2C);
                                        float distance = calcDist(LocalPos, enemyPos);

                                        Vector2 Angle = CalcAngle(LocalPos, enemyPos, origin);
                                        if(AimSettings[0].RCS)
                                        {
                                            Vector2 punch = Mem.ReadVec2(LocalPlayer + Offs.m_vecPunch);
                                            punch.x *= 2.0f;
                                            punch.y *= 2.0f; 
                                            Angle.x -= punch.x;
                                            Angle.y -= punch.y;
                                            
                                        }
                                        if (AimSettings[0].noLock)
                                        {
                                            Angle = Smooth(Angle, LocalAngles, AimSettings[0].smooth);
                                        }
                                        else
                                        {
                                            Vector2 fov = GetFov(Angle, LocalAngles);

                                            if (fov.x < AimSettings[0].FOV && fov.y < AimSettings[0].FOV)
                                            if (Mem.ReadInt(LocalPlayer + Offs.m_iCrossHairID) -1  == selectedEntity)
                                            {
                                                Angle = Smooth(Angle, LocalAngles, AimSettings[0].smooth / 3);
                                            }
                                            else
                                            {
                                                Angle = Smooth(Angle, LocalAngles, AimSettings[0].smooth);
                                            }
                                        }
                                       
                                        Angle = clampAngles(Angle);
                                        if(verifyAngles(Angle))
                                        {
                                            Mem.WriteVec2(Epointer + Offs.m_dwViewAngles, Angle);
                                        }
                                    
                                    }
                                    Thread.Sleep(10);
                                }
                            }
                        }
                    }
                    else
                    {
                        if(AimSettings[1].enabled ||AimSettings[2].enabled ||AimSettings[3].enabled) // AIM ASSIST
                        {
                            if (!IsWeaponNonAim(WeaponID))
                            {
                                int wepGroup = -1;
                                if (IsWeaponRifle(WeaponID)) wepGroup = 1;
                                if (IsWeaponSniper(WeaponID)) wepGroup = 2;
                                if (IsWeaponPistol(WeaponID)) wepGroup = 3;

                                if(Mem.ReadByte(Client.Base + Offs.m_dwForceAttack) == 5 && wepGroup !=-1 && AimSettings[wepGroup].enabled)
                                {
                                    float tempFov = 99999.0f;
                                    int selectedEntity = -1;
                                    for (int i = 0; i < 64; i++)
                                    {
                                        int BaseEntity = Mem.ReadInt(Client.Base + Offs.m_dwEntityList + (0x10 * i));
                                        int EntityTeam = Mem.ReadInt(BaseEntity + Offs.m_iTeamNum);
                                        int classid = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(BaseEntity + 0x8) + 2 * 0x4) + 0x1) + 20);
                                        if (classid == 35)
                                        {
                                            if (!Mem.ReadBool(BaseEntity + Offs.m_bGunGameImmunity))
                                            {
                                                if (!Mem.ReadBool(BaseEntity + Offs.m_lifeState))
                                                {
                                                    if (!Mem.ReadBool(BaseEntity + Offs.m_bDormant))
                                                    {
                                                        if (EntityTeam != LocalTeam && (EntityTeam == 2 || EntityTeam == 3))
                                                        {
                                                            int tempMask = Mem.ReadInt(BaseEntity + Offs.m_bSpottedByMask);
                                                            int pid = LocalId - 1;
                                                            if ((tempMask & (0x1 << pid)) != 0 || !AimSettings[wepGroup].visCheck)
                                                            {
                                                                Vector3 enemyPos = new Vector3();
                                                                int BoneMatrix = Mem.ReadInt(BaseEntity + Offs.m_dwBoneMatrix);
                                                                enemyPos.x = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[wepGroup].aimBone + 0x0C);
                                                                enemyPos.y = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[wepGroup].aimBone + 0x1C);
                                                                enemyPos.z = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[wepGroup].aimBone + 0x2C);

                                                                Vector2 tempAngle = CalcAngle(LocalPos, enemyPos, origin);
                                                                Vector2 fov = GetFov(tempAngle, LocalAngles);
                                                                float fovSum = fov.x + fov.y;
                                                                if (fovSum < tempFov && fov.x < AimSettings[wepGroup].FOV && fov.y < AimSettings[wepGroup].FOV)
                                                                {
                                                                    tempFov = fovSum;
                                                                    selectedEntity = i;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    while (Mem.ReadByte(Client.Base + Offs.m_dwForceAttack) == 5 && selectedEntity != -1)
                                    {
                                        LocalAngles = Mem.ReadVec2(Epointer + Offs.m_dwViewAngles);
                                        LocalPos = Mem.ReadVec3(LocalPlayer + Offs.m_vecOrigin);
                                        origin = Mem.ReadFloat(LocalPlayer + Offs.m_vecViewOffset + 0x8);

                                        int BaseEntity = Mem.ReadInt(Client.Base + Offs.m_dwEntityList + (0x10 * selectedEntity));

                                        if (AimSettings[wepGroup].noSticky && Mem.ReadBool(BaseEntity + Offs.m_lifeState))
                                        {
                                            break;
                                        }
                                        if (AimSettings[wepGroup].aimFlashed && Mem.ReadFloat(LocalPlayer + Offs.m_flFlashDuration) > 0.0f)
                                        {
                                            break;
                                        }

                                        int tempMask = Mem.ReadInt(BaseEntity + Offs.m_bSpottedByMask);
                                        int pid = LocalId - 1;
                                        if ((tempMask & (0x1 << pid)) != 0 || !AimSettings[wepGroup].visCheck)
                                        {
                                            Vector3 enemyPos = new Vector3();
                                            int BoneMatrix = Mem.ReadInt(BaseEntity + Offs.m_dwBoneMatrix);
                                            enemyPos.x = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[wepGroup].aimBone + 0x0C);
                                            enemyPos.y = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[wepGroup].aimBone + 0x1C);
                                            enemyPos.z = Mem.ReadFloat(BoneMatrix + 0x30 * AimSettings[wepGroup].aimBone + 0x2C);
                                            float distance = calcDist(LocalPos, enemyPos);

                                            Vector2 Angle = CalcAngle(LocalPos, enemyPos, origin);
                                            if (AimSettings[wepGroup].RCS)
                                            {
                                                Vector2 punch = Mem.ReadVec2(LocalPlayer + Offs.m_vecPunch);
                                                punch.x *= 2.0f;
                                                punch.y *= 2.0f;
                                                Angle.x -= punch.x;
                                                Angle.y -= punch.y;

                                            }
                                            if (AimSettings[wepGroup].noLock)
                                            {
                                                Angle = Smooth(Angle, LocalAngles, AimSettings[wepGroup].smooth);
                                            }
                                            else
                                            {
                                                Vector2 fov = GetFov(Angle, LocalAngles);

                                                if (fov.x < AimSettings[wepGroup].FOV && fov.y < AimSettings[wepGroup].FOV)
                                                    if (Mem.ReadInt(LocalPlayer + Offs.m_iCrossHairID) - 1 == selectedEntity)
                                                    {
                                                        Angle = Smooth(Angle, LocalAngles, 3);
                                                    }
                                                    else
                                                    {
                                                        Angle = Smooth(Angle, LocalAngles, AimSettings[wepGroup].smooth);
                                                    }
                                            }

                                            Angle = clampAngles(Angle);
                                            if (true)//verifyAngles(Angle))
                                            {
                                                Mem.WriteVec2(Epointer + Offs.m_dwViewAngles, Angle);
                                            }

                                        }
                                        Thread.Sleep(10);
                                    }
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bool found = false;
            timer1.Start();
            checkKeysThread = new Thread(() => CheckKeys());
            checkKeysThread.Start();

            tESP = new Thread(() => ESPmain());
            tTrigger = new Thread(() => TriggerMain());
            tSkinChanger = new Thread(() => SkinChangerMain());
            tRCS = new Thread(() => RCSMain());
            tMisc = new Thread(() => MiscMain());
            tAim = new Thread(() => AimMain());

            Process csgoProc = new Process();
            Process[] localByName = Process.GetProcesses();
            foreach (Process process in localByName)
            {
                if (process.ProcessName == "csgo")
                {
                    csgoProc = process;
                    Mem = new ProcessMemory(process.Id);
                    Mem.StartProcess();
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                MessageBox.Show("CS:GO not found");
                Application.Exit();
            }
            if(found)
            {
                while (Client.Base == 0 || Engine.Base == 0)
                {
                    Client.Base = Mem.DllImageAddressSize("client.dll", ref Client.Size);
                    Engine.Base = Mem.DllImageAddressSize("engine.dll", ref Engine.Size);
                }

                Offs.Update(csgoProc, Client.Base, Client.Size, Engine.Base, Engine.Size);
                tESP.Start();
                tTrigger.Start();
                tSkinChanger.Start();
                tRCS.Start();
                tMisc.Start();
                tAim.Start();
            }
        }


        private void nsControlButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void nsTheme1_Click(object sender, EventArgs e)
        {
            // Form1 Frm = Form1;
            // MessageBox.Show(Frm.Hsh.hash);
        }

        private void nsControlButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void nsButton1_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Width = 1000;
            foreach (Form a in Application.OpenForms)
            {
                if (a.Visible && a != this)
                {
                    Form ff = a;
                    Form1 asd = (Form1)ff;
                    MessageBox.Show(asd.Hsh.hash);
                }
            }
        }

        private void nsTheme1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void nsTrackBar1_Scroll(object sender)
        {
            PaintPanels();
        }

        private void nsTrackBar2_Scroll(object sender)
        {
            PaintPanels();
        }

        private void nsTrackBar3_Scroll(object sender)
        {
            PaintPanels();
        }

        private void nsTrackBar4_Scroll(object sender)
        {
            PaintPanels();
        }

        private void nsTrackBar5_Scroll(object sender)
        {
            PaintPanels();
        }

        private void nsTrackBar6_Scroll(object sender)
        {
            PaintPanels();
        }

        private void nsCheckBox1_CheckedChanged(object sender)
        {
            if (nsCheckBox1.Checked)
            {
                nsTrackBar1.Value = 70;
                nsTrackBar2.Value = 180;
                nsTrackBar3.Value = 70;
                Update();
                PaintPanels();
                nsTrackBar1.Enabled = false;
                nsTrackBar2.Enabled = false;
                nsTrackBar3.Enabled = false;
            }
            else
            {
                PaintPanels();
                nsTrackBar1.Enabled = true;
                nsTrackBar2.Enabled = true;
                nsTrackBar3.Enabled = true;
            }
        }


        private void nsCheckBox2_CheckedChanged(object sender)
        {
            if (nsCheckBox2.Checked)
            {
                nsTrackBar4.Value = 70;
                nsTrackBar5.Value = 70;
                nsTrackBar6.Value = 180;
                Update();
                PaintPanels();
                nsTrackBar4.Enabled = false;
                nsTrackBar5.Enabled = false;
                nsTrackBar6.Enabled = false;
            }
            else
            {
                PaintPanels();
                nsTrackBar4.Enabled = true;
                nsTrackBar5.Enabled = true;
                nsTrackBar6.Enabled = true;
            }
        }

        private void nsOnOffBox2_CheckedChanged(object sender)
        {
            bool state = nsOnOffBox2.Checked;
            nsRadioButton1.Visible = state;
            nsRadioButton2.Visible = state;
            nsRadioButton3.Visible = state;
            if (!state)
            {
                nsCheckBox4.Visible = state;
                nsCheckBox5.Visible = state;
                nsCheckBox6.Visible = state;
            }
            if (state && nsRadioButton3.Checked)
            {
                nsCheckBox4.Visible = state;
                nsCheckBox5.Visible = state;
                nsCheckBox6.Visible = state;
            }
        }

        private void nsRadioButton3_CheckedChanged(object sender)
        {
            bool state = nsRadioButton3.Checked;
            nsCheckBox4.Enabled = state;
            nsCheckBox5.Enabled = state;
            nsCheckBox6.Enabled = state;
            nsCheckBox4.Visible = state;
            nsCheckBox5.Visible = state;
            nsCheckBox6.Visible = state;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //AIM LOCK
            nsOnOffBox21.Checked = AimSettings[0].enabled;
            nsLabel9.Value1 = "FOV: " + AimSettings[0].FOV;
            nsLabel10.Value1 = "Smooth: " + AimSettings[0].smooth;
            nsLabel11.Value1 = "No Lock: " + AimSettings[0].noLock;
            nsLabel12.Value1 = "No Sticky: " + AimSettings[0].noSticky;
            nsLabel13.Value1 = "AimBone: " + AimSettings[0].aimBone;
            nsLabel14.Value1 = "RCS: " + AimSettings[0].RCS;
            nsLabel15.Value1 = "Vis Check: " + AimSettings[0].visCheck;
            nsLabel16.Value1 = "Aim Flashed: " + AimSettings[0].aimFlashed;

            //RIFLE
            nsOnOffBox22.Checked = AimSettings[1].enabled;
            nsLabel24.Value1 = "FOV: " + AimSettings[1].FOV;
            nsLabel23.Value1 = "Smooth: " + AimSettings[1].smooth;
            nsLabel22.Value1 = "No Lock: " + AimSettings[1].noLock;
            nsLabel21.Value1 = "No Sticky: " + AimSettings[1].noSticky;
            nsLabel20.Value1 = "AimBone: " + AimSettings[1].aimBone;
            nsLabel19.Value1 = "RCS: " + AimSettings[1].RCS;
            nsLabel18.Value1 = "Vis Check: " + AimSettings[1].visCheck;
            nsLabel17.Value1 = "Aim Flashed: " + AimSettings[1].aimFlashed;

            //SNIPER
            nsOnOffBox23.Checked = AimSettings[2].enabled;
            nsLabel32.Value1 = "FOV: " + AimSettings[2].FOV;
            nsLabel31.Value1 = "Smooth: " + AimSettings[2].smooth;
            nsLabel30.Value1 = "No Lock: " + AimSettings[2].noLock;
            nsLabel29.Value1 = "No Sticky: " + AimSettings[2].noSticky;
            nsLabel28.Value1 = "AimBone: " + AimSettings[2].aimBone;
            nsLabel27.Value1 = "RCS: " + AimSettings[2].RCS;
            nsLabel26.Value1 = "Vis Check: " + AimSettings[2].visCheck;
            nsLabel25.Value1 = "Aim Flashed: " + AimSettings[2].aimFlashed;

            //PISTOL
            nsOnOffBox24.Checked = AimSettings[3].enabled;
            nsLabel40.Value1 = "FOV: " + AimSettings[3].FOV;
            nsLabel39.Value1 = "Smooth: " + AimSettings[3].smooth;
            nsLabel38.Value1 = "No Lock: " + AimSettings[3].noLock;
            nsLabel37.Value1 = "No Sticky: " + AimSettings[3].noSticky;
            nsLabel36.Value1 = "AimBone: " + AimSettings[3].aimBone;
            nsLabel35.Value1 = "RCS: " + AimSettings[3].RCS;
            nsLabel34.Value1 = "Vis Check: " + AimSettings[3].visCheck;
            nsLabel33.Value1 = "Aim Flashed: " + AimSettings[3].aimFlashed;

            KeysConverter kc = new KeysConverter();

            //KEYS
            if (KeySettings.ESP == 0)
            {
                button1.Text = "Click To Configure";
            }
            else
            {
                button1.Text = kc.ConvertToString(KeySettings.ESP);
            }

            if (KeySettings.AimLock == 0)
            {
                button2.Text = "Click To Configure";
            }
            else
            {
                button2.Text = kc.ConvertToString(KeySettings.AimLock);
            }

            if (KeySettings.AimRifle == 0)
            {
                button3.Text = "Click To Configure";
            }
            else
            {
                button3.Text = kc.ConvertToString(KeySettings.AimRifle);
            }

            if (KeySettings.AimSniper == 0)
            {
                button4.Text = "Click To Configure";
            }
            else
            {
                button4.Text = kc.ConvertToString(KeySettings.AimSniper);
            }

            if (KeySettings.AimPistol == 0)
            {
                button5.Text = "Click To Configure";
            }
            else
            {
                button5.Text = kc.ConvertToString(KeySettings.AimPistol);
            }

            if (KeySettings.Trigger == 0)
            {
                button6.Text = "Click To Configure";
            }
            else
            {
                button6.Text = kc.ConvertToString(KeySettings.Trigger);
            }

            if (KeySettings.SkinChanger == 0)
            {
                button7.Text = "Click To Configure";
            }
            else
            {
                button7.Text = kc.ConvertToString(KeySettings.SkinChanger);
            }

            if (KeySettings.RCS == 0)
            {
                button8.Text = "Click To Configure";
            }
            else
            {
                button8.Text = kc.ConvertToString(KeySettings.RCS);
            }

            if (KeySettings.AimLockHold == 0)
            {
                button9.Text = "Click To Configure";
            }
            else
            {
                button9.Text = kc.ConvertToString(KeySettings.AimLockHold);
            }

            if (KeySettings.TriggerHold == 0)
            {
                button10.Text = "Click To Configure";
            }
            else
            {
                button10.Text = kc.ConvertToString(KeySettings.TriggerHold);
            }
        }

        private void nsButton1_Click_2(object sender, EventArgs e)
        {
            AimSettings[0] = new AimSett();
        }

        private void nsButton2_Click(object sender, EventArgs e)
        {
            AimSettings[1] = new AimSett();
        }

        private void nsButton3_Click(object sender, EventArgs e)
        {
            AimSettings[2] = new AimSett();
        }

        private void nsButton4_Click(object sender, EventArgs e)
        {
            AimSettings[3] = new AimSett();
        }

        private void nsTrackBar7_Scroll(object sender)
        {
            nsLabel41.Value1 = "FOV: " + nsTrackBar7.Value;
        }

        private void nsTrackBar8_Scroll(object sender)
        {
            nsLabel42.Value1 = "Smooth: " + nsTrackBar8.Value;
        }

        private void nsButton5_Click(object sender, EventArgs e)
        {
            int i = nsComboBox1.SelectedIndex;
            if (i >= 0 && i <= 3)
            {
                AimSettings[i].FOV = nsTrackBar7.Value;
                AimSettings[i].smooth = nsTrackBar8.Value;
                AimSettings[i].noLock = nsCheckBox7.Checked;
                AimSettings[i].noSticky = nsCheckBox8.Checked;
                if (radioButton1.Checked)
                {
                    AimSettings[i].aimBone = 6;
                }
                if (radioButton3.Checked)
                {
                    AimSettings[i].aimBone = 4;
                }
                if (radioButton2.Checked)
                {
                    AimSettings[i].aimBone = Convert.ToInt32(numericUpDown1.Value);
                }
                AimSettings[i].RCS = nsCheckBox9.Checked;
                AimSettings[i].visCheck = nsCheckBox10.Checked;
                AimSettings[i].aimFlashed = nsCheckBox11.Checked;
            }
            else
            {
                MessageBox.Show("Invalid Group");
            }
        }

        private void nsTrackBar9_Scroll(object sender)
        {
            nsLabel48.Value1 = "Delay: " + nsTrackBar9.Value + "ms";
        }

        private void nsTrackBar10_Scroll(object sender)
        {
            nsLabel49.Value1 = "Burst Shots: " + nsTrackBar10.Value + " shots";
        }


        private void nsOnOffBox11_CheckedChanged(object sender)
        {
        }

        private void nsButton10_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (checkKeysThread.IsAlive)
            {
                checkKeysThread.Abort();
            }
            if (tESP.IsAlive)
            {
                tESP.Abort();
            }
            if(tTrigger.IsAlive)
            {
                tTrigger.Abort();
            }
            if(tSkinChanger.IsAlive)
            {
                tSkinChanger.Abort();
            }
            if(tRCS.IsAlive)
            {
                tRCS.Abort();
            }
            if(tMisc.IsAlive)
            {
                tMisc.Abort();
            }
            if(tAim.IsAlive)
            {
                tAim.Abort();
            }
            Application.Exit();
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox2.Items.AddRange(skinList[listBox1.SelectedIndex].ToArray());

            if (CustomSkins[listBox1.SelectedIndex].skinId != 0)
            {
                string tempSkin = Convert.ToString(CustomSkins[listBox1.SelectedIndex].skinId) + " -";
                int tempFind = listBox2.FindString(tempSkin);
                if (tempFind != -1)
                {
                    listBox2.SelectedIndex = tempFind;
                }
            }
            if (CustomSkins[listBox1.SelectedIndex].skinId == 0)
            {
                listBox2.SelectedIndex = 0;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tempSkin = Convert.ToString(listBox2.SelectedItem);
            string b = string.Empty;
            int val = new int();

            for (int i = 0; i < tempSkin.Length; i++)
            {
                if (Char.IsDigit(tempSkin[i]))
                    b += tempSkin[i];
            }

            if (b.Length > 0)
                val = int.Parse(b);

            CustomSkins[listBox1.SelectedIndex].skinId = val;
        }

        private void nsOnOffBox12_CheckedChanged(object sender)
        {
            numericUpDown2.Enabled = nsOnOffBox12.Checked;
        }

        private void nsOnOffBox13_CheckedChanged(object sender)
        {
            nsTextBox1.Enabled = nsOnOffBox13.Checked;
        }

        private void nsTrackBar11_Scroll(object sender)
        {
            nsLabel55.Value1 = "Vertical Compensation: " + nsTrackBar11.Value + "%";
        }

        private void nsOnOffBox14_CheckedChanged(object sender)
        {
        }

        private void nsTrackBar12_Scroll(object sender)
        {
            nsLabel60.Value1 = "Horizontal Compensation: " + nsTrackBar12.Value + "%";
        }

        private void nsTrackBar13_Scroll(object sender)
        {
            nsLabel67.Value1 = "Slow Aim: " + nsTrackBar13.Value + "%";
        }

        private void nsOnOffBox20_CheckedChanged(object sender)
        {
            nsTrackBar13.Enabled = nsOnOffBox20.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void nsButton10_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(KeySettings.ESP));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button1.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.ESP = i;
                            return;
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button2.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.AimLock = i;
                            return;
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button3.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.AimRifle = i;
                            return;
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button4.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.AimSniper = i;
                            return;
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button5.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.AimPistol = i;
                            return;
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button6.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.Trigger = i;
                            return;
                        }
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button7.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.SkinChanger = i;
                            return;
                        }
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button8.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.RCS = i;
                            return;
                        }
                    }
                }
            }
        }

        private void nsOnOffBox21_CheckedChanged(object sender)
        {
            AimSettings[0].enabled = nsOnOffBox21.Checked;
        }

        private void nsOnOffBox22_CheckedChanged(object sender)
        {
            AimSettings[1].enabled = nsOnOffBox22.Checked;
        }

        private void nsOnOffBox23_CheckedChanged(object sender)
        {
            AimSettings[2].enabled = nsOnOffBox23.Checked;
        }

        private void nsOnOffBox24_CheckedChanged(object sender)
        {
            AimSettings[3].enabled = nsOnOffBox24.Checked;
        }

        private void nsButton7_Click(object sender, EventArgs e)
        {
            LoadConfig("Config1");
        }

        private void nsButton6_Click(object sender, EventArgs e)
        {
            SaveConfig("Config1");
        }

        private void nsOnOffBox5_CheckedChanged(object sender)
        {

        }

        private void nsOnOffBox9_CheckedChanged(object sender)
        {
        }

        private void nsOnOffBox10_CheckedChanged(object sender)
        {
        }

        public void SaveConfig(string name)
        {
            string currentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currentPath, "SEConfig")))
                Directory.CreateDirectory(Path.Combine(currentPath, "SEConfig"));

            string lines="[ESP]";// ----------ESP--ESP--ESP--ESP
            lines += "\r\nPlayer=" + nsOnOffBox2.Checked;
            lines += "\r\nBomb=" + nsOnOffBox3.Checked;
            lines += "\r\nItem=" + nsOnOffBox4.Checked;
            lines += "\r\nCustom=" + nsRadioButton1.Checked;
            lines += "\r\nHealthBased=" + nsRadioButton2.Checked;
            lines += "\r\nVulnerable=" + nsRadioButton3.Checked;
            lines += "\r\nNoArmor=" + nsCheckBox4.Checked;
            lines += "\r\nFlashed=" + nsCheckBox5.Checked;
            lines += "\r\nLowHealth=" + nsCheckBox6.Checked;
            lines += "\r\nTeamR=" + nsTrackBar1.Value;
            lines += "\r\nTeamG=" + nsTrackBar2.Value;
            lines += "\r\nTeamB=" + nsTrackBar3.Value;
            lines += "\r\nEnemyR=" + nsTrackBar4.Value;
            lines += "\r\nEnemyG=" + nsTrackBar5.Value;
            lines += "\r\nEnemyB=" + nsTrackBar6.Value;
            lines += "\r\nTeamDefault=" + nsCheckBox1.Checked;
            lines += "\r\nEnemyDefault=" + nsCheckBox2.Checked;

            lines += "\r\n\r\n[Aim]"; // ----------AIM--AIM--AIM--AIM
            for (int i = 0; i < 4; i++)
            {
                if (i != 0)
                    lines += "\r\n";
                string modifier = "";
                if (i == 0)
                    modifier = "\r\nAimLock ";
                if (i == 1)
                    modifier = "\r\nRifle ";
                if (i == 2)
                    modifier = "\r\nSniper ";
                if (i == 3)
                    modifier = "\r\nPistol ";
                lines += modifier + "Enabled=" + AimSettings[i].enabled;
                lines += modifier + "FOV=" + AimSettings[i].FOV;
                lines += modifier + "Smooth=" + AimSettings[i].smooth;
                lines += modifier + "NoLock=" + AimSettings[i].noLock;
                lines += modifier + "NoSticky=" + AimSettings[i].noSticky;
                lines += modifier + "AimBone=" + AimSettings[i].aimBone;
                lines += modifier + "RCS=" + AimSettings[i].RCS;
                lines += modifier + "VisCheck=" + AimSettings[i].visCheck;
                lines += modifier + "AimFlashed=" + AimSettings[i].aimFlashed;
            }

            lines += "\r\n\r\n[Trigger]"; // ----------TRIGGER--TRIGGER--TRIGGER--TRIGGER
            lines += "\r\nTrigger=" + nsOnOffBox5.Checked;
            lines += "\r\nShootTeammates=" + nsOnOffBox6.Checked;
            lines += "\r\nAutoFire=" + nsOnOffBox7.Checked;
            lines += "\r\nScopedOnly=" + nsOnOffBox8.Checked;
            lines += "\r\nUseDelay=" + nsOnOffBox9.Checked;
            lines += "\r\nUseBurstShots=" + nsOnOffBox10.Checked;
            lines += "\r\nDelay=" + nsTrackBar9.Value;
            lines += "\r\nBurstShots=" + nsTrackBar10.Value;
            lines += "\r\nPistol=" + nsCheckBox16.Checked;
            lines += "\r\nRifle=" + nsCheckBox15.Checked;
            lines += "\r\nShotgun=" + nsCheckBox14.Checked;
            lines += "\r\nSniper=" + nsCheckBox13.Checked;
            lines += "\r\nSMG=" + nsCheckBox12.Checked;

            lines += "\r\n\r\n[SkinChanger]"; // ----------SkinChanger--SkinChanger--SkinChanger--SkinChanger
            lines += "\r\nSkinChanger=" + nsOnOffBox11.Checked;
            lines += "\r\nStatTrak=" + nsOnOffBox12.Checked;
            lines += "\r\nStatTrakValue=" + numericUpDown2.Value;
            lines += "\r\nCustomName=" + nsOnOffBox13.Checked;
            lines += "\r\nCustomNameValue=" + nsTextBox1.Text;
            lines += "\r\nAlwaysForce=" + nsOnOffBox25.Checked;

            for (int i = 0; i < 33; i++)
            {
                string modifier = "\r\nSkin";
                modifier += Convert.ToString(i);
                lines += modifier + "=" + CustomSkins[i].skinId;
            }

            lines += "\r\n\r\n[Misc]"; // ----------MISC--MISC--MISC--MISC
            lines += "\r\nRCS=" + nsOnOffBox14.Checked;
            lines += "\r\nVertical=" + nsTrackBar11.Value;
            lines += "\r\nHorizontal=" + nsTrackBar12.Value;
            lines += "\r\nRandomMoves=" + nsOnOffBox15.Checked;
            lines += "\r\nNoFlash=" + nsOnOffBox16.Checked;
            lines += "\r\nAutoBhop=" + nsOnOffBox17.Checked;
            lines += "\r\nRadarHack=" + nsOnOffBox18.Checked;
            lines += "\r\nAuto-Pistol=" + nsOnOffBox19.Checked;
            lines += "\r\nSlowAim=" + nsOnOffBox20.Checked;
            lines += "\r\nSlowAimValue=" + nsTrackBar13.Value;

            lines += "\r\n\r\n[Keys]"; // ----------KEYS--KEYS--KEYS--KEYS
            lines += "\r\nESP=" + KeySettings.ESP;
            lines += "\r\nAimLock=" + KeySettings.AimLock;
            lines += "\r\nAimRifle=" + KeySettings.AimRifle;
            lines += "\r\nAimSniper=" + KeySettings.AimSniper;
            lines += "\r\nAimPistol=" + KeySettings.AimPistol;
            lines += "\r\nTrigger=" + KeySettings.Trigger;
            lines += "\r\nSkinChanger=" + KeySettings.SkinChanger;
            lines += "\r\nRCS=" + KeySettings.RCS;
            lines += "\r\nAimLockHold=" + KeySettings.AimLockHold;
            lines += "\r\nTriggerHold=" + KeySettings.TriggerHold;
            System.IO.StreamWriter file = new System.IO.StreamWriter("SEConfig/" + name + ".ini");
            file.WriteLine(lines);

            file.Close();
        }

        void ReadOnOff(string sectionName, string settingName, Control control, IniParser parser)
        {
            if (control is NSOnOffBox)
            {
                if (parser.GetSetting(sectionName, settingName) != null)
                {
                    (control as NSOnOffBox).Checked = Convert.ToBoolean(parser.GetSetting(sectionName, settingName));
                }
            }
            if (control is NSRadioButton) 
            {
                if (parser.GetSetting(sectionName, settingName) != null)
                {
                    (control as NSRadioButton).Checked = Convert.ToBoolean(parser.GetSetting(sectionName, settingName));
                }
            }
            if (control is NSCheckBox)
            {
                if (parser.GetSetting(sectionName, settingName) != null)
                {
                    (control as NSCheckBox).Checked = Convert.ToBoolean(parser.GetSetting(sectionName, settingName));
                }
            }
        }

        void ReadValue(string sectionName, string settingName, Control control, IniParser parser)
        {
            if (control is NSTrackBar)
            {
                if (parser.GetSetting(sectionName, settingName) != null)
                {
                    (control as NSTrackBar).Value = Convert.ToInt32(parser.GetSetting(sectionName, settingName));
                }
            }
            if(control is NumericUpDown)
            {
                if (parser.GetSetting(sectionName, settingName) != null)
                {
                    (control as NumericUpDown).Value = Convert.ToInt32(parser.GetSetting(sectionName, settingName));
                }
            }
            if(control is NSTextBox)
            {
                if (parser.GetSetting(sectionName, settingName) != null)
                {
                    (control as NSTextBox).Text = Convert.ToString(parser.GetSetting(sectionName, settingName));
                }
            }
        }

        int ReadInteger(string sectionName, string settingName, IniParser parser)
        {
            if (parser.GetSetting(sectionName, settingName) != null)
            {
                return Convert.ToInt32(parser.GetSetting(sectionName, settingName));
            }
            return 0;
        }
        bool ReadBoolean(string sectionName, string settingName, IniParser parser)
        {
            if (parser.GetSetting(sectionName, settingName) != null)
            {
                return Convert.ToBoolean(parser.GetSetting(sectionName, settingName));
            }
            return false;
        }
        public void LoadConfig(string name)
        {
            string path = "SEConfig/" + name + ".ini";
            if (!File.Exists(path))
            {
                MessageBox.Show("Config Not Found");
                return;
            }

            IniParser parser = new IniParser(path);
            //ESP
            ReadOnOff("ESP", "Player", nsOnOffBox2, parser);
            ReadOnOff("ESP", "Bomb", nsOnOffBox3, parser);
            ReadOnOff("ESP", "Item", nsOnOffBox4, parser);
            ReadOnOff("ESP", "Custom", nsRadioButton1, parser);
            ReadOnOff("ESP", "HealthBased", nsRadioButton2, parser);
            ReadOnOff("ESP", "Vulnerable", nsRadioButton3, parser);
            ReadOnOff("ESP", "NoArmor", nsCheckBox4, parser);
            ReadOnOff("ESP", "Flashed", nsCheckBox5, parser);
            ReadOnOff("ESP", "LowHealth", nsCheckBox6, parser);
            ReadValue("ESP", "TeamR", nsTrackBar1, parser);
            ReadValue("ESP", "TeamG", nsTrackBar2, parser);
            ReadValue("ESP", "TeamB", nsTrackBar3, parser);
            ReadValue("ESP", "EnemyR", nsTrackBar4, parser);
            ReadValue("ESP", "EnemyG", nsTrackBar5, parser);
            ReadValue("ESP", "EnemyB", nsTrackBar6, parser);
            ReadOnOff("ESP", "TeamDefault", nsCheckBox1, parser);
            ReadOnOff("ESP", "EnemyDefault", nsCheckBox2, parser);
            //AIM
            for(int i=0; i<4;i++)
            {
                string modifier = "";
                if (i == 0)
                    modifier = "AimLock ";
                if (i == 1)
                    modifier = "Rifle ";
                if (i == 2)
                    modifier = "Sniper ";
                if (i == 3)
                    modifier = "Pistol ";
                AimSettings[i].enabled = ReadBoolean("Aim", modifier + "Enabled", parser);
                AimSettings[i].FOV = ReadInteger("Aim", modifier + "FOV", parser);
                AimSettings[i].smooth = ReadInteger("Aim", modifier + "Smooth", parser);
                AimSettings[i].noLock = ReadBoolean("Aim", modifier + "NoLock", parser);
                AimSettings[i].noSticky = ReadBoolean("Aim", modifier + "NoSticky", parser);
                AimSettings[i].aimBone = ReadInteger("Aim", modifier + "AimBone", parser);
                AimSettings[i].RCS = ReadBoolean("Aim", modifier + "RCS", parser);
                AimSettings[i].visCheck = ReadBoolean("Aim", modifier + "VisCheck", parser);
                AimSettings[i].aimFlashed = ReadBoolean("Aim", modifier + "AimFlashed", parser);
            }
            //TRIGGER
            ReadOnOff("Trigger", "Trigger", nsOnOffBox5, parser);
            ReadOnOff("Trigger", "ShootTeammates", nsOnOffBox6, parser);
            ReadOnOff("Trigger", "AutoFire", nsOnOffBox7, parser);
            ReadOnOff("Trigger", "ScopedOnly", nsOnOffBox8, parser);
            ReadOnOff("Trigger", "UseDelay", nsOnOffBox9, parser);
            ReadOnOff("Trigger", "UseBurstShots", nsOnOffBox10, parser);
            ReadValue("Trigger", "Delay", nsTrackBar9, parser);
            ReadValue("Trigger", "BurstShots", nsTrackBar10, parser);
            ReadOnOff("Trigger", "Pistol", nsCheckBox16, parser);
            ReadOnOff("Trigger", "Rifle", nsCheckBox15, parser);
            ReadOnOff("Trigger", "ShotGun", nsCheckBox14, parser);
            ReadOnOff("Trigger", "Sniper", nsCheckBox13, parser);
            ReadOnOff("Trigger", "SMG", nsCheckBox12, parser);
            //SKINCHANGER
            ReadOnOff("SkinChanger", "SkinChanger", nsOnOffBox11, parser);
            ReadOnOff("SkinChanger", "StatTrak", nsOnOffBox12, parser);
            ReadValue("SkinChanger", "StatTrakValue", numericUpDown2, parser);
            ReadOnOff("SkinChanger", "CustomName", nsOnOffBox13, parser);
            ReadValue("SkinChanger", "CustomNameValue", nsTextBox1, parser);
            for(int i=0;i<33;i++)
            {
                string modifier = "Skin" + i;
                CustomSkins[i].skinId = ReadInteger("SkinChanger", modifier, parser);
            }
            ReadOnOff("SkinChanger", "AlwaysForce", nsOnOffBox25, parser);
            //MISC
            ReadOnOff("Misc", "RCS", nsOnOffBox14, parser);
            ReadValue("Misc", "Vertical", nsTrackBar11, parser);
            ReadValue("Misc", "Horizontal", nsTrackBar12, parser);
            ReadOnOff("Misc", "RandomMoves", nsOnOffBox15, parser);
            ReadOnOff("Misc", "NoFlash", nsOnOffBox16, parser);
            ReadOnOff("Misc", "AutoBhop", nsOnOffBox17, parser);
            ReadOnOff("Misc", "RadarHack", nsOnOffBox18, parser);
            ReadOnOff("Misc", "Auto-Pistol", nsOnOffBox19, parser);
            ReadOnOff("Misc", "SlowAim", nsOnOffBox20, parser);
            ReadValue("Misc", "SlowAimValue", nsTrackBar13, parser);
            //KEYS
            KeySettings.ESP = ReadInteger("Keys", "ESP", parser);
            KeySettings.AimLock = ReadInteger("Keys", "AimLock", parser);
            KeySettings.AimRifle = ReadInteger("Keys", "AimRifle", parser);
            KeySettings.AimPistol = ReadInteger("Keys", "AimPistol", parser);
            KeySettings.AimSniper = ReadInteger("Keys", "AimSniper", parser);
            KeySettings.Trigger = ReadInteger("Keys", "Trigger", parser);
            KeySettings.SkinChanger = ReadInteger("Keys", "SkinChanger", parser);
            KeySettings.RCS = ReadInteger("Keys", "RCS", parser);
            KeySettings.AimLockHold = ReadInteger("Keys", "AimLockHold", parser);
            KeySettings.TriggerHold = ReadInteger("Keys", "TriggerHold", parser);
        }
        private void nsButton9_Click(object sender, EventArgs e)
        {
            SaveConfig("Config2");
        }

        private void nsButton11_Click(object sender, EventArgs e)
        {
            SaveConfig("Config3");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button9.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.AimLockHold = i;
                            return;
                        }
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Application.DoEvents();
                button10.Text = "Waiting For Key";
                for (int i = 0; i < 255; i++)
                {
                    if (IsKeyPushedDown((Keys)i))
                    {
                        if (i == 1 || i == 2)
                        {
                            return;
                        }
                        else
                        {
                            KeySettings.TriggerHold = i;
                            return;
                        }
                    }
                }
            }
        }

        private void nsButton13_Click(object sender, EventArgs e)
        {
            SaveConfig("Config4");
        }

        private void nsButton12_Click(object sender, EventArgs e)
        {
            LoadConfig("Config4");
        }


    }
}
