using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.ServiceModel;
using Microsoft.Win32;

namespace PDA
{
    class FunPublic
    {
        public static string CurrentUser;
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

        public static string url;

        /// <summary>
        /// 从XML文件中读取节点数据
        /// </summary>
        /// <param name="node">要读取的节点</param>
        /// <returns>DataTable</returns>
        public static DataTable LoadServerXml(string node)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("nm");
            dt.Columns.Add("cd");
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(CurrentPath + "\\SysConfig.xml");
                XmlElement rootElem = doc.DocumentElement;   //获取根节点  
                XmlNodeList Nodes = rootElem.GetElementsByTagName(node); //获取子节点集合  
                foreach (XmlNode item in Nodes)
                {

                    DataRow newRow = dt.NewRow();
                    newRow["nm"] = item["ServerName"].InnerText;
                    newRow["cd"] = item["Url"].InnerText;
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }

        }

        public static DataTable GetDt(string StrSql)
        {
            try
            {
                System.ServiceModel.Channels.Binding binding = ServiceBaseClient.CreateDefaultBinding();
                string remoteAddress = url;
                EndpointAddress endpoint = new EndpointAddress(remoteAddress);
                ServiceBaseClient client = new ServiceBaseClient(binding, endpoint);

                return client.GetDt(StrSql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //执行sql
        public static Result RunSql(string StrSql)
        {
            try
            {
                System.ServiceModel.Channels.Binding binding = ServiceBaseClient.CreateDefaultBinding();
                string remoteAddress = url;
                EndpointAddress endpoint = new EndpointAddress(remoteAddress);
                ServiceBaseClient client = new ServiceBaseClient(binding, endpoint);

                return client.RunSql(StrSql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //批量执行sql
        public static Result RunSqls(List<string> strSqls)
        {
            try
            {
                System.ServiceModel.Channels.Binding binding = ServiceBaseClient.CreateDefaultBinding();
                string remoteAddress = url;
                EndpointAddress endpoint = new EndpointAddress(remoteAddress);
                ServiceBaseClient client = new ServiceBaseClient(binding, endpoint);

                return client.RunSqls(strSqls.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        //获取DataTable从SAP数据源
        public static DataTable GetDtSap(string StrSql)
        {
            try
            {
                System.ServiceModel.Channels.Binding binding = ServiceBaseClient.CreateDefaultBinding();
                string remoteAddress = url;
                EndpointAddress endpoint = new EndpointAddress(remoteAddress);
                ServiceBaseClient client = new ServiceBaseClient(binding, endpoint);
                return client.GetDtSap(StrSql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取当前设备名
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceName()
        {
            RegistryKey folders;
            folders = OpenRegistryPath(Registry.LocalMachine, @"/Ident");
            string name = folders.GetValue("Name").ToString();
            return name;
        }
        private static RegistryKey OpenRegistryPath(RegistryKey root, string s)
        {
            s = s.Remove(0, 1) + @"/";
            while (s.IndexOf(@"/") != -1)
            {
                root = root.OpenSubKey(s.Substring(0, s.IndexOf(@"/")));
                s = s.Remove(0, s.IndexOf(@"/") + 1);
            }
            return root;
        }

        public static void View_ListView(DataTable ds_source, ListView LV)
        {
            LV.Items.Clear();
            for (int i = 0; i < ds_source.Rows.Count; i++)
            {
                int Color_RowLine = 0;

                ListViewItem tempitem = new ListViewItem();

                tempitem.Checked = false;

                LV.Items.Add(tempitem);

                for (int j = 0; j < ds_source.Columns.Count; j++)
                {
                    LV.Items[i].SubItems.Add(ds_source.Rows[i][j].ToString());
                }
            }
        }
    }
}
