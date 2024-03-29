﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using SilentEvil;

namespace ReadWriteMemory
{

    internal class ProcessMemory
    {
        // Fields
        protected int BaseAddress;
        protected Process MyProcess;
        protected ProcessModule myProcessModule;
        private const uint PAGE_EXECUTE = 16;
        private const uint PAGE_EXECUTE_READ = 32;
        private const uint PAGE_EXECUTE_READWRITE = 64;
        private const uint PAGE_EXECUTE_WRITECOPY = 128;
        private const uint PAGE_GUARD = 256;
        private const uint PAGE_NOACCESS = 1;
        private const uint PAGE_NOCACHE = 512;
        private const uint PAGE_READONLY = 2;
        private const uint PAGE_READWRITE = 4;
        private const uint PAGE_WRITECOPY = 8;
        private const uint PROCESS_ALL_ACCESS = 2035711;
        protected int processHandle;
        protected string ProcessName;
        protected int processID;

        // Methods
        public ProcessMemory(int processID)
        {
            this.processID = processID;
        }

        public bool CheckProcess()
        {
            MessageBox.Show(ProcessName);
            return (Process.GetProcessesByName(this.ProcessName).Length > 0);
        }

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(int hObject);
        public string CutString(string mystring)
        {
            char[] chArray = mystring.ToCharArray();
            string str = "";
            for (int i = 0; i < mystring.Length; i++)
            {
                if ((chArray[i] == ' ') && (chArray[i + 1] == ' '))
                {
                    return str;
                }
                if (chArray[i] == '\0')
                {
                    return str;
                }
                str = str + chArray[i].ToString();
            }
            return mystring.TrimEnd(new char[] { '0' });
        }

        public int DllImageAddress(string dllname)
        {
            ProcessModuleCollection modules = this.MyProcess.Modules;

            foreach (ProcessModule procmodule in modules)
            {
                if (dllname == procmodule.ModuleName)
                {
                    return (int)procmodule.BaseAddress;
                }
            }
            return -1;

        }

        public int DllImageAddressSize(string dllname, ref int size)
        {
            ProcessModuleCollection modules = this.MyProcess.Modules;

            foreach (ProcessModule procmodule in modules)
            {
                if (dllname == procmodule.ModuleName)
                {
                    size = (int)procmodule.ModuleMemorySize;
                    return (int)procmodule.BaseAddress;
                }
            }
            return -1;

        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern int FindWindowByCaption(int ZeroOnly, string lpWindowName);
        public int ImageAddress()
        {
            this.BaseAddress = 0;
            this.myProcessModule = this.MyProcess.MainModule;
            this.BaseAddress = (int)this.myProcessModule.BaseAddress;
            return this.BaseAddress;


        }

        public int ImageAddress(int pOffset)
        {
            this.BaseAddress = 0;
            this.myProcessModule = this.MyProcess.MainModule;
            this.BaseAddress = (int)this.myProcessModule.BaseAddress;
            return (pOffset + this.BaseAddress);
        }
        public string MyProcessName()
        {
            return this.ProcessName;
        }

        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        public int Pointer(bool AddToImageAddress, int pOffset)
        {
            return this.ReadInt(this.ImageAddress(pOffset));
        }

        public int Pointer(string Module, int pOffset)
        {
            return this.ReadInt(this.DllImageAddress(Module) + pOffset);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2)
        {

            if (AddToImageAddress)
                return (this.ReadInt(this.ImageAddress() + pOffset) + pOffset2);
            else
                return (this.ReadInt(pOffset) + pOffset2);
        }

        public int Pointer(string Module, int pOffset, int pOffset2)
        {
            return (this.ReadInt(this.DllImageAddress(Module) + pOffset) + pOffset2);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3)
        {
            return (this.ReadInt(this.ReadInt(this.ImageAddress(pOffset)) + pOffset2) + pOffset3);
        }

        public int Pointer(string Module, int pOffset, int pOffset2, int pOffset3)
        {
            return (this.ReadInt(this.ReadInt(this.DllImageAddress(Module) + pOffset) + pOffset2) + pOffset3);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4)
        {
            return (this.ReadInt(this.ReadInt(this.ReadInt(this.ImageAddress(pOffset)) + pOffset2) + pOffset3) + pOffset4);
        }

        public int Pointer(string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4)
        {
            return (this.ReadInt(this.ReadInt(this.ReadInt(this.DllImageAddress(Module) + pOffset) + pOffset2) + pOffset3) + pOffset4);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5)
        {
            return (this.ReadInt(this.ReadInt(this.ReadInt(this.ReadInt(this.ImageAddress(pOffset)) + pOffset2) + pOffset3) + pOffset4) + pOffset5);
        }

        public int Pointer(string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5)
        {
            return (this.ReadInt(this.ReadInt(this.ReadInt(this.ReadInt(this.DllImageAddress(Module) + pOffset) + pOffset2) + pOffset3) + pOffset4) + pOffset5);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5, int pOffset6)
        {
            return (this.ReadInt(this.ReadInt(this.ReadInt(this.ReadInt(this.ReadInt(this.ImageAddress(pOffset)) + pOffset2) + pOffset3) + pOffset4) + pOffset5) + pOffset6);
        }

        public int Pointer(string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5, int pOffset6)
        {
            return (this.ReadInt(this.ReadInt(this.ReadInt(this.ReadInt(this.ReadInt(this.DllImageAddress(Module) + pOffset) + pOffset2) + pOffset3) + pOffset4) + pOffset5) + pOffset6);
        }

        public byte ReadByte(int pOffset)
        {
            byte[] buffer = new byte[1];
            ReadProcessMemory(this.processHandle, pOffset, buffer, 1, 0);
            return buffer[0];
        }

        public bool ReadBool(int pOffset)
        {
            byte[] buffer = new byte[1];
            ReadProcessMemory(this.processHandle, pOffset, buffer, 1, 0);
            return Convert.ToBoolean(buffer[0]);
        }

        public byte ReadByte(bool AddToImageAddress, int pOffset)
        {
            byte[] buffer = new byte[1];
            int lpBaseAddress = AddToImageAddress ? this.ImageAddress(pOffset) : pOffset;
            ReadProcessMemory(this.processHandle, lpBaseAddress, buffer, 1, 0);
            return buffer[0];
        }

        public byte ReadByte(string Module, int pOffset)
        {
            byte[] buffer = new byte[1];
            ReadProcessMemory(this.processHandle, this.DllImageAddress(Module) + pOffset, buffer, 1, 0);
            return buffer[0];
        }

        public float ReadFloat(int pOffset)
        {
            return BitConverter.ToSingle(this.ReadMem(pOffset, 4), 0);
        }

        public float ReadFloat(bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToSingle(this.ReadMem(pOffset, 4, AddToImageAddress), 0);
        }

        public float ReadFloat(string Module, int pOffset)
        {
            return BitConverter.ToSingle(this.ReadMem(this.DllImageAddress(Module) + pOffset, 4), 0);
        }

        public int ReadInt(int pOffset)
        {
            return BitConverter.ToInt32(this.ReadMem(pOffset, 4), 0);
        }

        public int ReadInt(bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToInt32(this.ReadMem(pOffset, 4, AddToImageAddress), 0);
        }

        public int ReadInt(string Module, int pOffset)
        {
            return BitConverter.ToInt32(this.ReadMem(this.DllImageAddress(Module) + pOffset, 4), 0);
        }

        public byte[] ReadMem(int pOffset, int pSize)
        {
            byte[] buffer = new byte[pSize];
            ReadProcessMemory(this.processHandle, pOffset, buffer, pSize, 0);
            return buffer;
        }

        public byte[] ReadMem(int pOffset, int pSize, bool AddToImageAddress)
        {
            byte[] buffer = new byte[pSize];
            int lpBaseAddress = AddToImageAddress ? this.ImageAddress(pOffset) : pOffset;
            ReadProcessMemory(this.processHandle, lpBaseAddress, buffer, pSize, 0);
            return buffer;
        }

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);
        public short ReadShort(int pOffset)
        {
            return BitConverter.ToInt16(this.ReadMem(pOffset, 2), 0);
        }

        public short ReadShort(bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToInt16(this.ReadMem(pOffset, 2, AddToImageAddress), 0);
        }

        public short ReadShort(string Module, int pOffset)
        {
            return BitConverter.ToInt16(this.ReadMem(this.DllImageAddress(Module) + pOffset, 2), 0);
        }

        public string ReadStringAscii(int pOffset, int pSize)
        {
            return this.CutString(Encoding.ASCII.GetString(this.ReadMem(pOffset, pSize)));
        }

        public string ReadStringAscii(bool AddToImageAddress, int pOffset, int pSize)
        {
            return this.CutString(Encoding.ASCII.GetString(this.ReadMem(pOffset, pSize, AddToImageAddress)));
        }

        public string ReadStringAscii(string Module, int pOffset, int pSize)
        {
            return this.CutString(Encoding.ASCII.GetString(this.ReadMem(this.DllImageAddress(Module) + pOffset, pSize)));
        }

        public string ReadStringUnicode(int pOffset, int pSize)
        {
            return this.CutString(Encoding.Unicode.GetString(this.ReadMem(pOffset, pSize)));
        }

        public string ReadStringUnicode(bool AddToImageAddress, int pOffset, int pSize)
        {
            return this.CutString(Encoding.Unicode.GetString(this.ReadMem(pOffset, pSize, AddToImageAddress)));
        }

        public string ReadStringUnicode(string Module, int pOffset, int pSize)
        {
            return this.CutString(Encoding.Unicode.GetString(this.ReadMem(this.DllImageAddress(Module) + pOffset, pSize)));
        }

        public uint ReadUInt(int pOffset)
        {
            return BitConverter.ToUInt32(this.ReadMem(pOffset, 4), 0);
        }

        public uint ReadUInt(bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToUInt32(this.ReadMem(pOffset, 4, AddToImageAddress), 0);
        }

        public uint ReadUInt(string Module, int pOffset)
        {
            return BitConverter.ToUInt32(this.ReadMem(this.DllImageAddress(Module) + pOffset, 4), 0);
        }

        public Vector2 ReadVec2(int pOffset)
        {
            Vector2 buffer;
            buffer.x = ReadFloat(pOffset);
            buffer.y = ReadFloat(pOffset + 0x4);
            return buffer;
        }

        public Vector3 ReadVec3(int pOffset)
        {
            Vector3 buffer;
            buffer.x = ReadFloat(pOffset);
            buffer.y = ReadFloat(pOffset + 0x4);
            buffer.z = ReadFloat(pOffset + 0x8);
            return buffer;
        }

        public bool StartProcess()
        {
            if (this.ProcessName != "")
            {
                this.MyProcess = Process.GetProcessById(this.processID);
                if (this.MyProcess.Id == 0)
                {
                    MessageBox.Show(this.ProcessName + " is not running or has not been found. Please check and try again", "Process Not Found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                this.processHandle = OpenProcess(2035711, false, this.MyProcess.Id);
                if (this.processHandle == 0)
                {
                    MessageBox.Show(this.ProcessName + " is not running or has not been found. Please check and try again", "Process Not Found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                return true;
            }
            MessageBox.Show("Define process name first!");
            return false;
        }

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(int hProcess, int lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);
        public void WriteByte(int pOffset, byte pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes((short)pBytes));
        }

        public void WriteBool(int pOffset, bool pBool)
        {

            WriteProcessMemory(processHandle, pOffset, BitConverter.GetBytes(pBool), sizeof(bool), 0);
        }

        public void WriteByte(bool AddToImageAddress, int pOffset, byte pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes((short)pBytes), AddToImageAddress);
        }

        public void WriteByte(string Module, int pOffset, byte pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes((short)pBytes));
        }

        public void WriteDouble(int pOffset, double pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteDouble(bool AddToImageAddress, int pOffset, double pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WriteDouble(string Module, int pOffset, double pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteFloat(int pOffset, float pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteFloat(bool AddToImageAddress, int pOffset, float pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WriteFloat(string Module, int pOffset, float pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteInt(int pOffset, int pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteInt(bool AddToImageAddress, int pOffset, int pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WriteInt(string Module, int pOffset, int pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteMem(int pOffset, byte[] pBytes)
        {
            WriteProcessMemory(this.processHandle, pOffset, pBytes, pBytes.Length, 0);
        }

        public void WriteMem(int pOffset, byte[] pBytes, bool AddToImageAddress)
        {
            int lpBaseAddress = AddToImageAddress ? this.ImageAddress(pOffset) : pOffset;
            WriteProcessMemory(this.processHandle, lpBaseAddress, pBytes, pBytes.Length, 0);
        }

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);
        public void WriteShort(int pOffset, short pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteShort(bool AddToImageAddress, int pOffset, short pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WriteShort(string Module, int pOffset, short pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteStringAscii(int pOffset, string pBytes)
        {
            this.WriteMem(pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"));
        }

        public void WriteStringAscii(bool AddToImageAddress, int pOffset, string pBytes)
        {
            this.WriteMem(pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"), AddToImageAddress);
        }

        public void WriteStringAscii(string Module, int pOffset, string pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"));
        }

        public void WriteStringUnicode(int pOffset, string pBytes)
        {
            this.WriteMem(pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"));
        }

        public void WriteStringUnicode(bool AddToImageAddress, int pOffset, string pBytes)
        {
            this.WriteMem(pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"), AddToImageAddress);
        }

        public void WriteStringUnicode(string Module, int pOffset, string pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"));
        }

        public void WriteUInt(int pOffset, uint pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WriteUInt(bool AddToImageAddress, int pOffset, uint pBytes)
        {
            this.WriteMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WriteUInt(string Module, int pOffset, uint pBytes)
        {
            this.WriteMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

       public void WriteVec2(int pOffset,Vector2 vec)
       {
           WriteFloat(pOffset, vec.x);
           WriteFloat(pOffset + 0x4, vec.y);
       }

       public void WriteVec3(int pOffset, Vector3 vec)
       {
           WriteFloat(pOffset, vec.x);
           WriteFloat(pOffset + 0x4, vec.y);
           WriteFloat(pOffset + 0x8, vec.z);
       }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 2035711,
            CreateThread = 2,
            DupHandle = 64,
            QueryInformation = 1024,
            SetInformation = 512,
            Synchronize = 1048576,
            Terminate = 1,
            VMOperation = 8,
            VMRead = 16,
            VMWrite = 32
        }
    }
}