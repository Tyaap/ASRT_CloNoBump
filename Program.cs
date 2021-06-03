using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static MemoryHelper;

namespace CloNoBump
{
    public class Program
    {
        [DllImport("kernel32")]
        private static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImport("kernel32")]
        private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, int lpNumberOfBytesRead);

        private static int processHandle = 0;
        static void Main()
        {
            Application.EnableVisualStyles();

            int gameProcesses = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if(process.MainWindowTitle != "Sonic & All-Stars Racing Transformed - BUILD (532043 - Jan 15 2014 10:38:42)")
                {
                    continue;
                }
                gameProcesses++;

                if (!MemoryHelper.Initialise(process.Id))
                {
                    MessageBox.Show("Could not access the game, please run as administrator!", "CloNoBump", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Write(0x47B483, new byte[] { 0xE9, 0xE2, 0xF9, 0x53, 0x00, 0x90 });
                Write(0x47C91D, new byte[] { 0xEB });
                Write(0x47CD99, new byte[] { 0xE9, 0x85, 0x00, 0x00, 0x00, 0x90 });
                Write(0x4EEDC5, new byte[] { 0xEB });
                Write(0x72CE65, new byte[] { 0xE9, 0x89, 0x00, 0x00, 0x00, 0x90 });
                Write(0x8503C6, new byte[] { 0x90, 0x90 });
                Write(0x85042D, new byte[] { 0xEB });
                Write(0x9BAE6A, new byte[] { 0x50, 0xA0, 0x86, 0xAE, 0x9B, 0x00, 0x24, 0x0F, 0x3C, 0x00, 0x74, 0x04, 0xC6, 0x45, 0x0C, 0x45, 0x58, 0x8B, 0x45, 0x10, 0x83, 0xEC, 0x18, 0xE9, 0x03, 0x06, 0xAC, 0xFF });
                Write(0x9BAE6C, ReadInt(0xEC1A88) + 0x101D6C);
                byte[] myCode = new byte[] { 0x50, 0x53, 0x8B, 0x44, 0x24, 0x14, 0x8B, 0x5C, 0x24, 0x10, 0xF6, 0x40, 0x38, 0x70, 0x74, 0x0D, 0xF6, 0x43, 0x38, 0x70, 0x74, 0x07, 0x5B, 0x58, 0x31, 0xC0, 0xC2, 0x0C, 0x00, 0x5B, 0x58, 0x68, 0xC0, 0xAD, 0x43, 0x00, 0xC3 };
                int myCodePtr = Allocate(0, myCode.Length);
                Write(myCodePtr, myCode);
                Write(0x43389A + 1, myCodePtr - (0x43389A + 5));
                Write(0x439A7D + 1, myCodePtr - (0x439A7D + 5));
                Write(0x43A34E + 1, myCodePtr - (0x43A34E + 5));
                Write(0x473343, new byte[] { 0x31, 0xC0, 0x90 });
                Write(ReadInt(0xEC1A88) + 0x3D4, 0);
            }

            if (gameProcesses > 0)
            {
                MessageBox.Show(
                    "CloNoBump is ENABLED!\n\n" +
                    "In custom games you are now:\n" +
                    " ➤ Free to choose the same character multiple times!\n" +
                    " ➤ Free from bumps!\n" +
                    " ➤ Free from AI players!\n" +
                    "Enjoy! :)", "CloNoBump", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please start Sonic & All-Stars Racing Transformed first!", "CloNoBump", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
