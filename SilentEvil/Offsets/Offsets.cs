using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PatternScan;
using System.Diagnostics;
using ReadWriteMemory;

namespace SilentEvil.Offsets
{
    class Offsets
    {
        public int m_dwEntityList { get; set; }
        public int m_dwLocalPlayer { get; set; }
        public int m_dwViewMatrix { get; set; }
        public int m_dwGlowObject { get; set; }
        public int m_dwForceJump { get; set; }
        public int m_dwForceAttack { get; set; }
        public int m_dwSensitivity { get; set; }
        public int m_dwMouseEnable { get; set; }
        public int m_iCrossHairID { get; set; }
        public int m_dwClientState { get; set; }
        public int m_dwViewAngles { get; set; }
        public int m_dwRadarBase { get; set; }
        public int m_iGlowIndex { get; set; }

        public int m_bSpotted = 0x939;
        public int m_bSpottedByMask = 0x97C;
        public int m_vecOrigin = 0x134;
        public int m_iTeamNum = 0xF0;
        public int m_iShotsFired = 0xA2B0;
        public int m_lifeState = 0x25B;
        public int m_fFlags = 0x100;
        public int m_iHealth = 0xFC;
        public int m_vecViewOffset = 0x104;
        public int m_vecPunch = 0x3018;
        public int m_dwBoneMatrix = 0x2698;
        public int m_bDormant = 0xE9;
        public int m_dwInGame = 0x100;
        public int m_dwRadarBasePointer = 0x50;
        public int m_ArmorValue = 0xA8F8;
        public int m_hActiveWeapon = 0x2EE8;
        public int m_flFlashDuration = 0xA2F8;
        public int m_iWeaponId = 0x32DC;
        public int m_iItemDefinitionIndex = 0x2F80;
        public int m_bIsScoped = 0x388C;
        public int m_iClip1 = 0x31F4;
        public int m_dwIndex = 0x64;
        public int m_bGunGameImmunity = 0x38A0;

        //skinchanger REGION
        public int m_iItemIDHigh = 0x2F98;
        public int m_nFallbackStatTrak = 0x316C;
        public int m_iEntityQuality = 0x2F84;
        public int m_nFallbackPaintKit = 0x3160;
        public int m_flFallbackWear = 0x3168;
        public int m_iAccountID = 0x2FA0;
        public int m_OriginalOwnerXuidLow = 0x3158;
        public int m_szCustomName = 0x3014;

        public Offsets()
        {
            m_dwEntityList = 0;
            m_dwLocalPlayer = 0;
            m_dwViewMatrix = 0;
            m_dwGlowObject = 0;
            m_dwForceJump = 0;
            m_dwForceAttack = 0;
            m_dwSensitivity = 0;
            m_dwMouseEnable = 0;
            m_iCrossHairID = 0;
            m_dwClientState = 0;
            m_dwClientState = 0;
            m_dwViewAngles = 0;
            m_dwRadarBase = 0;
            m_iGlowIndex = 0;
        }

        public void Update(Process proc, int ClientBase, int ClientSize,int EngineBase, int EngineSize)
        {
            ProcessMemory Mem = new ProcessMemory(proc.Id);
            Mem.StartProcess();

            SigScan ClientScan = new SigScan(proc, new IntPtr(ClientBase), ClientSize);
            SigScan EngineScan = new SigScan(proc, new IntPtr(EngineBase), EngineSize);

            IntPtr ptr = ClientScan.FindPattern(new byte[] { 0x05, 0x00, 0x00, 0x00, 0x00, 0xC1, 0xE9, 0x00, 0x39, 0x48, 0x04 }, "x????xx?xxx", 0);
            int p1 = Mem.ReadInt((int)ptr + 1);
            byte p2 = Mem.ReadByte((int)ptr + 7);
            m_dwEntityList = (p1 + p2) - ClientBase;

            ptr = ClientScan.FindPattern(new byte[] { 0x8D, 0x34, 0x85, 0x00, 0x00, 0x00, 0x00, 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x41, 0x08, 0x8B, 0x48, 0x00 }, "xxx????xx????xxxxx?", 0);
            p1 = Mem.ReadInt((int)ptr + 3);
            p2 = Mem.ReadByte((int)ptr + 18);
            m_dwLocalPlayer = (p1 + p2) - ClientBase;

            ptr = ClientScan.FindPattern(new byte[] { 0xF3, 0x0F, 0x6F, 0x05, 0x00, 0x00, 0x00, 0x00, 0x8D, 0x85 }, "xxxx????xx", 0);
            p1 = Mem.ReadInt((int)ptr + 4) + 0xB0;
            m_dwViewMatrix = p1 - ClientBase;

            ptr = ClientScan.FindPattern(new byte[] { 0xA1, 0x00, 0x00, 0x00, 0x00, 0xA8, 0x01, 0x75, 0x00, 0x0F, 0x57, 0xC0, 0xC7, 0x05 }, "x????xxx?xxxxx", 0);
            m_dwGlowObject = Mem.ReadInt((int)ptr + 0x58) - ClientBase;

            ptr = ClientScan.FindPattern(new byte[] { 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, 0xF6, 0xC2, 0x03, 0x74, 0x03, 0x83, 0xCE, 0x08, 0xA8, 0x08, 0xBF }, "xx????xx????xxxxxxxxxxx", 0);
            m_dwForceJump = Mem.ReadInt((int)ptr + 2) - ClientBase;

            ptr = ClientScan.FindPattern(new byte[] { 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, 0xF6, 0xC2, 0x03, 0x74, 0x03, 0x83, 0xCE, 0x04, 0xA8, 0x04, 0xBF }, "xx????xx????xxxxxxxxxxx", 0);
            m_dwForceAttack = Mem.ReadInt((int)ptr + 2) - ClientBase;

            ptr = ClientScan.FindPattern(new byte[] { 0xF3, 0x0F, 0x10, 0x05, 0x00, 0x00, 0x00, 0x00, 0xEB, 0x0F, 0x8B, 0x01, 0x8B, 0x40, 0x30, 0xFF, 0xD0, 0xD9, 0x5D, 0x0C, 0xF3, 0x0F, 0x10, 0x45, 0x0C, 0xF3, 0x0F, 0x11 }, "xxxx????xxxxxxxxxxxxxxxxxxxx", 0);
            m_dwSensitivity = Mem.ReadInt((int)ptr + 4) - ClientBase;
            m_dwMouseEnable = m_dwSensitivity + 0x5C;

            ptr = ClientScan.FindPattern(new byte[] { 0x56, 0x57, 0x8B, 0xF9, 0xC7, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x81, 0xF9, 0x00, 0x00, 0x00, 0x00, 0x75, 0x07, 0xA1, 0x00, 0x00, 0x00, 0x00, 0xEB, 0x07 }, "xxxxxx????????xx????xx????xxx????xx", 0);
            m_iCrossHairID = Mem.ReadInt((int)ptr + 6);

            ptr = EngineScan.FindPattern(new byte[] { 0xF3, 0x0F, 0x5C, 0xC1, 0xF3, 0x0F, 0x10, 0x15, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x2F, 0xD0, 0x76, 0x04, 0xF3, 0x0F, 0x58, 0xC1, 0xA1, 0x00, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x11, 0x80, 0x00, 0x00, 0x00, 0x00, 0xD9, 0x46, 0x04 }, "xxxxxxxx????xxxxxxxxxx????xxxx????xxx", 0);
            m_dwClientState = Mem.ReadInt((int)ptr + 22) - EngineBase;
            m_dwViewAngles = Mem.ReadInt((int)ptr + 30);

            ptr = ClientScan.FindPattern(new byte[] { 0xA1, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x0C, 0xB0, 0x8B, 0x01, 0xFF, 0x50, 0x00, 0x46, 0x3B, 0x35, 0x00, 0x00, 0x00, 0x00, 0x7C, 0xEA, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00 }, "x????xxxxxxx?xxx????xxxx????", 0);
            m_dwRadarBase = Mem.ReadInt((int)ptr + 1) - ClientBase;

            ptr = ClientScan.FindPattern(new byte[] { 0xF3, 0x0F, 0x10, 0x96, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x57, 0xDB, 0x0F, 0x2F, 0xD3, 0x0F, 0x86, 0x00, 0x00, 0x00, 0x00 }, "xxxx????xxxxxxxx????", 0);
            m_iGlowIndex = Mem.ReadInt((int)ptr + 4) + 0x18;
        }
    }
}
