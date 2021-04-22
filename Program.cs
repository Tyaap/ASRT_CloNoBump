using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CloNoBump
{
    public class Program
    {
        [DllImport("kernel32")]
        private static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImport("kernel32")]
        private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
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

                processHandle = OpenProcess(0x38, 0, process.Id);
                if (processHandle == 0)
                {
                    MessageBox.Show("Could not access the game, please run as administrator!", "CloNoBump", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SetBytes(ReadInt(0xBD0174) + 0x68 * 0 + 0x18, new byte[] { 60 });
                SetBytes(ReadInt(0xBD0174) + 0x68 * 14 + 0x18, new byte[] { 60 });
                SetBytes(0x47B483, new byte[] { 0xE9, 0xE2, 0xF9, 0x53, 0x00, 0x90 });
                SetBytes(0x47C91D, new byte[] { 0xEB });
                SetBytes(0x47CD99, new byte[] { 0xE9, 0x85, 0x00, 0x00, 0x00, 0x90 });
                SetBytes(0x47D384, new byte[] { 0xE9, 0x16, 0x01, 0x00, 0x00, 0x90 });
                SetBytes(0x4EEDC5, new byte[] { 0xEB });
                SetBytes(0x72CE65, new byte[] { 0xE9, 0x89, 0x00, 0x00, 0x00, 0x90 });
                SetBytes(0x7C4721, new byte[] { 0xEB });
                SetBytes(0x8503C6, new byte[] { 0x90, 0x90 });
                SetBytes(0x85042D, new byte[] { 0xEB });
                SetBytes(0x9BAE6A, new byte[] { 0x50, 0xA0, 0x86, 0xAE, 0x9B, 0x00, 0x24, 0x0F, 0x3C, 0x00, 0x74, 0x04, 0xC6, 0x45, 0x0C, 0x45, 0x58, 0x8B, 0x45, 0x10, 0x83, 0xEC, 0x18, 0xE9, 0x03, 0x06, 0xAC, 0xFF });
                SetBytes(0x9BAE6C, BitConverter.GetBytes(ReadInt(0xEC1A88) + 0x101D6C));
            }

            if (gameProcesses > 0)
            {
                MessageBox.Show(
                    "In custom games you are now:\n\n" +
                    "● Free to choose the same character multiple times!\n" +
                    "● Free from bumps!\n" +
                    "● Free from AI players!\n" +
                    "● Able to race 60 seconds before getting DNF!\n\n" +
                    "Enjoy! :)", "CloNoBump", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please start the game first!", "CloNoBump", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static int ReadInt(int Address)
        {
            return BitConverter.ToInt32(ReadBytes(Address, 4), 0);
        }

        private static byte[] ReadBytes(int Address, int ByteCount)
        {
            byte[] Bytes = new byte[ByteCount];
            ReadProcessMemory(processHandle, Address, Bytes, ByteCount, 0);
            return Bytes;
        }

        public static void SetBytes(int Address, byte[] Bytes)
        {
            WriteProcessMemory(processHandle, Address, Bytes, Bytes.Length, 0);
        }
    }
}
