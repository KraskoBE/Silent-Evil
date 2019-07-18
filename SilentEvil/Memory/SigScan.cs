using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace PatternScan
{
    public class SigScan
    {
       
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out()] byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead
            );

        private byte[] m_vDumpedRegion;

        private Process m_vProcess;

        private IntPtr m_vAddress;

        private Int32 m_vSize;


        #region "sigScan Class Construction"

        public SigScan()
        {
            this.m_vProcess = null;
            this.m_vAddress = IntPtr.Zero;
            this.m_vSize = 0;
            this.m_vDumpedRegion = null;
        }
        public SigScan(Process proc, IntPtr addr, int size)
        {
            this.m_vProcess = proc;
            this.m_vAddress = addr;
            this.m_vSize = size;
        }
        #endregion

        #region "sigScan Class Private Methods"
        private bool DumpMemory()
        {
            try
            {
                if (this.m_vProcess == null)
                    return false;
                if (this.m_vProcess.HasExited == true)
                    return false;
                if (this.m_vAddress == IntPtr.Zero)
                    return false;
                if (this.m_vSize == 0)
                    return false;

                this.m_vDumpedRegion = new byte[this.m_vSize];

                bool bReturn = false;
                int nBytesRead = 0;

                bReturn = ReadProcessMemory(
                    this.m_vProcess.Handle, this.m_vAddress, this.m_vDumpedRegion, this.m_vSize, out nBytesRead
                    );

                if (bReturn == false || nBytesRead != this.m_vSize)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool MaskCheck(int nOffset, byte[] btPattern, string strMask)
        {
            for (int x = 0; x < btPattern.Length; x++)
            {
                if (strMask[x] == '?')
                    continue;

                if ((strMask[x] == 'x') && (btPattern[x] != this.m_vDumpedRegion[nOffset + x]))
                    return false;
            }

            return true;
        }
        #endregion

        #region "sigScan Class Public Methods"
        public IntPtr FindPattern(byte[] btPattern, string strMask, int nOffset)
        {
            try
            {
                if (this.m_vDumpedRegion == null || this.m_vDumpedRegion.Length == 0)
                {
                    if (!this.DumpMemory())
                        return IntPtr.Zero;
                }

                if (strMask.Length != btPattern.Length)
                    return IntPtr.Zero;

                for (int x = 0; x < this.m_vDumpedRegion.Length; x++)
                {
                    if (this.MaskCheck(x, btPattern, strMask))
                    {
                        return new IntPtr((int)this.m_vAddress + (x + nOffset));
                    }
                }

                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                return IntPtr.Zero;
            }
        }

        public void ResetRegion()
        {
            this.m_vDumpedRegion = null;
        }
        #endregion

        #region "sigScan Class Properties"
        public Process Process
        {
            get { return this.m_vProcess; }
            set { this.m_vProcess = value; }
        }
        public IntPtr Address
        {
            get { return this.m_vAddress; }
            set { this.m_vAddress = value; }
        }
        public Int32 Size
        {
            get { return this.m_vSize; }
            set { this.m_vSize = value; }
        }
        #endregion

    }
}