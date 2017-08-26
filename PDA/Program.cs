using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net;
using System.Runtime.InteropServices;

namespace PDA
{
    static class Program
    {
        [DllImport("Toolhelp.dll")]
        public static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);
        [DllImport("Coredll.dll")]
        public static extern int CloseHandle(IntPtr handle);
        [DllImport("Toolhelp.dll")]
        public static extern int Process32First(IntPtr handle, ref PROCESSENTRY32 pe);
        [DllImport("Toolhelp.dll")]
        public static extern int Process32Next(IntPtr handle, ref PROCESSENTRY32 pe);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            IntPtr handle = CreateToolhelp32Snapshot((uint)SnapShotFlags.TH32CS_SNAPPROCESS, 0);
            if ((int)handle != -1)
            {
                PROCESSENTRY32 pe32 = new PROCESSENTRY32();
                pe32.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));
                int bMore = Process32First(handle, ref pe32);
                PROCESSENTRY32 pe;
                string servername = "";
                while (bMore == 1)
                {
                    IntPtr temp = Marshal.AllocHGlobal((int)pe32.dwSize);
                    Marshal.StructureToPtr(pe32, temp, true);
                    pe = (PROCESSENTRY32)Marshal.PtrToStructure(temp, typeof(PROCESSENTRY32));
                    Marshal.FreeHGlobal(temp);
                    //MessageBox.Show(pe32.szExeFile);
                    if (pe32.szExeFile == "PDA.exe")
                    {
                        if (servername == "")
                        {
                            servername = pe32.szExeFile;
                        }
                        else if (servername == "PDA.exe")
                        {
                            return;
                        }
                    }
                    bMore = Process32Next(handle, ref pe32);
                }
            }
            CloseHandle(handle);

            if (Directory.Exists(FunPublic.CurrentPath + "/temp") == false)
            {
                Directory.CreateDirectory(FunPublic.CurrentPath + "/temp");
            }
            if (System.IO.File.Exists(FunPublic.CurrentPath + "\\Update.exe"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(FunPublic.CurrentPath + "\\SysConfig.xml");
                string strFtpAddress = doc.SelectSingleNode(@"SystemConfig/Server/Url").InnerText.Trim();//更新的网站的服务器地址
                strFtpAddress = strFtpAddress.Remove(strFtpAddress.LastIndexOf("/") + 1, 15) + "PDA";
                string version = doc.SelectSingleNode(@"SystemConfig/Version").InnerText.Trim();//版本号
                //下载xml配置文件
                HttpWebRequest Request = (HttpWebRequest)System.Net.WebRequest.Create(strFtpAddress + "/SysConfig.xml");
                HttpWebResponse Response = (HttpWebResponse)(WebResponse)Request.GetResponse();
                System.IO.BinaryReader sr = new BinaryReader(Response.GetResponseStream());
                long fileLenth = Response.ContentLength;
                byte[] content = sr.ReadBytes((Int32)fileLenth);
                FileStream so = new FileStream(FunPublic.CurrentPath + "/temp/" + "SysConfig.xml", FileMode.Create);
                BinaryWriter fileWriter = new BinaryWriter(so);
                fileWriter.Write(content, 0, (Int32)fileLenth);
                fileWriter.Close();
                //下载xml文件结束
                doc.Load(FunPublic.CurrentPath + "/temp/" + "SysConfig.xml");
                if (doc.SelectSingleNode(@"SystemConfig/Version").InnerText.Trim() != version)
                {
                    if (MessageBox.Show("检测到有新版本，是否更新程序?", "消息框", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start(FunPublic.CurrentPath + "/Update.exe", null);
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                }
            }
            FrmPDALogin FrmPDALogin = new FrmPDALogin();
            FrmPDALogin.ShowDialog();
            if (FrmPDALogin.DialogResult == DialogResult.OK)
            {
                Application.Run(new FrmMenu());
            }
            else
            {
                return;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESSENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]//注意，此处为宽字符  
        public string szExeFile;
        public uint th32MemoryBase;
        public uint th32AccessKey;
    }

    public enum SnapShotFlags : uint
    {
        TH32CS_SNAPHEAPLIST = 0x00000001,
        TH32CS_SNAPPROCESS = 0x00000002,
        TH32CS_SNAPTHREAD = 0x00000004,
        TH32CS_SNAPMODULE = 0x00000008,
        TH32CS_SNAPALL = (TH32CS_SNAPHEAPLIST | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD | TH32CS_SNAPMODULE),
        TH32CS_GETALLMODS = 0x80000000
    }
}