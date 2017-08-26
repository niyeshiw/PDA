using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;

namespace Update
{
    static class Program
    {
        private static string _mCurrentPath;
        private static string Platform
        {
            get
            {
                return Environment.OSVersion.Platform.ToString();
            }
        }
        public static string CurrentPath
        {
            get
            {
                if (Platform.Equals("WinCE"))
                {
                    _mCurrentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                }
                else if (Platform.Equals("Win32NT"))
                {
                    _mCurrentPath = System.IO.Directory.GetCurrentDirectory();
                }
                return _mCurrentPath;
            }
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            //Application.Run(new Form1());
            XmlDocument doc = new XmlDocument();
            doc.Load(CurrentPath + "\\SysConfig.xml");
            string strFtpAddress = doc.SelectSingleNode(@"SystemConfig/Server/Url").InnerText.Trim();//更新的网站的服务器地址
            strFtpAddress = strFtpAddress.Remove(strFtpAddress.LastIndexOf("/") + 1, 15) + "PDA";
            //下载xml配置文件
            HttpWebRequest Request = (HttpWebRequest)System.Net.WebRequest.Create(strFtpAddress + "/SysConfig.xml");
            HttpWebResponse Response = (HttpWebResponse)(WebResponse)Request.GetResponse();
            System.IO.BinaryReader sr = new BinaryReader(Response.GetResponseStream());
            long fileLenth = Response.ContentLength;
            byte[] content = sr.ReadBytes((Int32)fileLenth);
            FileStream so = new FileStream(CurrentPath + "/SysConfig.xml", FileMode.Create);
            BinaryWriter fileWriter = new BinaryWriter(so);
            fileWriter.Write(content, 0, (Int32)fileLenth);
            fileWriter.Close();
            //下载xml文件结束

            //下载exe程序
            HttpWebRequest Request1 = (HttpWebRequest)System.Net.WebRequest.Create(strFtpAddress + "/PDA.exe");
            HttpWebResponse Response1 = (HttpWebResponse)(WebResponse)Request1.GetResponse();
            System.IO.BinaryReader sr1 = new BinaryReader(Response1.GetResponseStream());
            long fileLenth1 = Response1.ContentLength;
            byte[] content1 = sr1.ReadBytes((Int32)fileLenth1);
            FileStream so1 = new FileStream(CurrentPath + "/PDA.exe", FileMode.Create);
            BinaryWriter fileWriter1 = new BinaryWriter(so1);
            fileWriter1.Write(content1, 0, (Int32)fileLenth1);
            fileWriter1.Close();
            //下载exe程序结束
            MessageBox.Show("程序更新完成！");
            System.Diagnostics.Process.Start(CurrentPath + "/PDA.exe", null);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}