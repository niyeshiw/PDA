using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace PDA
{
    public partial class FrmPDALogin : Form
    {
        private DataTable serverDT = new DataTable();

        public FrmPDALogin()
        {
            InitializeComponent();
        }

        private void FrmPDALogin_Load(object sender, EventArgs e)
        {
            serverDT = FunPublic.LoadServerXml("Server");
            foreach (DataRow dr in serverDT.Rows)
            {
                cmbServer.Items.Add(dr[0]);
            }
            cmbServer.SelectedIndex = 0;
            //FunPublic.url = serverDT.Rows[0][1].ToString();
            txtUserCode.Focus();
        }

        private void cmbServer_SelectedValueChanged(object sender, EventArgs e)
        {
            FunPublic.url = serverDT.Select("nm = '" + cmbServer.Text + "'")[0][1].ToString();
        }

        /// <summary>
        /// 测试服务器端连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void button1_Click(object sender, EventArgs e)
        //{

        //    System.ServiceModel.Channels.Binding binding = ServiceBaseClient.CreateDefaultBinding();
        //    string remoteAddress = ServiceBaseClient.EndpointAddress.Uri.ToString();

        //    // Server IP Address
        //    //remoteAddress = remoteAddress.Replace("localhost", "10.16.172.204");
        //    EndpointAddress endpoint = new EndpointAddress(remoteAddress);
        //    ServiceBaseClient client = new ServiceBaseClient(binding, endpoint);
        //    try
        //    {
        //        //MessageBox.Show(client.Add(10, 20).ToString());
        //        DataTable dt = client.GetDt("select * from Ts_Config");
        //        MessageBox.Show(dt.Rows[0][0].ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //    //PlatformServer.ServiceBase server = new PDA.PlatformServer.ServiceBase();

        //    //DataTable dt = server.GetDt("select * from Ts_Config");



        //}

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if ((txtUserCode.Text == "" || txtPassword.Text == ""))
            {
                MessageBox.Show("请输入用户编号和密码！");
                return;
            }
            string strSql = string.Format("SELECT OrgId,UserId FROM Core_User WHERE UserId='{0}' and Pwd='{1}' and DropFlag<>1", txtUserCode.Text, txtPassword.Text);
            DataTable dt = FunPublic.GetDt(strSql);
            if (dt.TableName == "Error")
            {
                MessageBox.Show(dt.Rows[0][0].ToString(), "提示");
                return;
            }
            if (dt != null && dt.Rows.Count <= 0)
            {
                MessageBox.Show("用户错误", "提示");
                return;
            }
            FunPublic.CurrentUser = txtUserCode.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}