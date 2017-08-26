using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PDA
{
    public partial class FrmRecRFID : Form
    {
        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();

        public FrmRecRFID()
        {
            InitializeComponent();
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹
        }

        private void FrmRecRFID_Load(object sender, EventArgs e)
        {
            RFID_15693.InitModule();

            if (!System.IO.File.Exists(CommonClass.Path))//如果文件不存在，表示首次打开程序,将扫描头未开启的常用条码保存在文件里面
            {
                List<string> listData = new List<string>();
                for (int k = 0; k < CommonClass.strCodeType.Length / 3; k++)
                {
                    if (CommonClass.strCodeType[k, 0, 0] != "Composite CC-A/B")//Composite CC-A/B默认不开启，所以不记录
                    {
                        listData.Add(CommonClass.strCodeType[k, 0, 0] + "," + CommonClass.strCodeType[k, 0, 1] + ",1");
                    }
                }
                CommonClass.SaveFile(CommonClass.Path, listData);
            }

            byte[] by = new byte[6];
            List<string> listCodeType = new List<string>();//得到上次保存的条码类型的数据
            CommonClass.ReadFile(CommonClass.Path, ref listCodeType);
            for (int k = 0; k < listCodeType.Count; k++)
            {
                byte[] data = BitConverter.GetBytes(Int32.Parse(listCodeType[k].Split(',')[1]));
                for (int t = 0; t < data.Length; t++)
                {
                    by[t] = data[t];
                }
                by[4] = 1;
                by[5] = 1;
                CommonClass.SoftDecoding_BarcodeType_OnOff(by, by.Length);
                System.Threading.Thread.Sleep(10);
            }
            byte[] data2 = BitConverter.GetBytes(716);
            byte[] by2 = new byte[] { 0, 0, 0, 0, 1, 0 };//402，2开启
            Array.Copy(data2, 0, by2, 0, 4);
            CommonClass.SoftDecoding_BarcodeType_OnOff(by2, by2.Length);

            //DataTable dt = FunPublic.GetDt("exec [dbo].[PDA_OSGoodsRecQuery] 'Vendor','','',''");
            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        cmbCardCode.Items.Add(dr["outsourcBP"].ToString() + "-" + dr["outsourcBPNM"].ToString());
            //    }
            //}

            btnScan.Focus();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtRFID.Text == "")
            {
                MessageBox.Show("请刷物料卡！");
                return;
            }
            if (cmbStatus.Text != "是")
            {
                MessageBox.Show("还有未完工工序，卡片不能回收！");
                return;
            }
            if (FunPublic.GetDt("select * from Tm_RFID where RFIDLUN = '" + txtRFID.Text + "' and [STATUS] <> 'F'").Rows.Count > 0)
            {
                MessageBox.Show("卡片状态异常，不能回收！");
                return;
            }

            //DataTable dt = FunPublic.GetDtSap("select firstName from OHEM where U_rfid = '" + txtRoumap.Text + "'");
            //string DeviceName = FunPublic.GetDeviceName();
            List<string> strSqls = new List<string>();
            strSqls.Add(@"
declare @MaxDoc as int
select @MaxDoc = isnull(max(DocEntry) + 1,1) from Doc_recRFID
INSERT INTO [Doc_recRFID]
           ([DocEntry]
           ,[CREATED]
           ,[DOC_DATE]
           ,[EBDOC]
           ,[SN]
           ,[PackageCode]
           ,[RFID_ID]
           ,[roumap])
     VALUES(@MaxDoc,'" + FunPublic.CurrentUser + "',getdate(),'" + txtEBNum.Text + "','" + txtSN.Text + "','" + txtPackageCode.Text + "','" + txtRFID.Text + "','" + txtRoumap.Text + "')");
            strSqls.Add("update Tm_RFID set STATUS = 'O' where RFIDLUN = '" + txtRFID.Text + "'");
            result = FunPublic.RunSqls(strSqls);
            if (result.Status == 1)
            {
                //MessageBox.Show("提交成功！");
                txtRoumap.Text = "";
                txtRFID.Text = "";
                //cmbCardCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                cmbStatus.Text = "";
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            byte[] pszData = new byte[25];
            byte[] data = new byte[1];
            try
            {
                if (RFID_15693.ScanSingleTag(ref data))
                {
                    //CommonClass.PlaySound();
                }
                else
                {
                    MessageBox.Show("扫描失败,请确认是否是15693标签,并确认标签是否处于RFID感应区");
                    return;
                }

                string RFID_ID = RFID_15693.RFID_ID();

                if (RFID_ID == "")
                {
                    txtRoumap.Text = "";
                    txtRFID.Text = "";
                    //cmbCardCode.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtSN.Text = "";
                    txtQuantity.Text = "";
                    cmbStatus.Text = "";
                    MessageBox.Show("刷卡失败！");
                }
                else
                {
                    int res = RFID_15693.RF_ISO15693_getSystemInformation(0, data, 0, pszData);

                    if (res == 0)
                    {
                        if (BitConverter.ToString(pszData, 11, 1) == "0B")
                        {//物料卡
                            txtRFID.Text = RFID_ID;
                            if (!CommonClass.ValidateRFID(RFID_ID))
                            {
                                throw new Exception("此卡已经被回收！");
                            }
                            string DeviceName = FunPublic.GetDeviceName();
                            if (FunPublic.GetDt("select * from Tm_RFID where STATUS = 'F' and RFIDLUN = '" + RFID_ID + "'").Rows.Count > 0)
                            {

                                DataTable dt = FunPublic.GetDt("exec [PDA_RecRFIDQuery] '" + RFID_ID + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        txtRoumap.Text = dr["roumap"].ToString();
                                        txtEBNum.Text = dr["EBDOC"].ToString();
                                        txtPackageCode.Text = dr["PackageCode"].ToString();
                                        txtSN.Text = dr["SN"].ToString();
                                        txtQuantity.Text = dr["QtyinPackage"].ToString();
                                        cmbStatus.Text = dr["Status"].ToString();
                                    }
                                }
                                else
                                {
                                    txtRoumap.Text = "";
                                    txtEBNum.Text = "";
                                    txtPackageCode.Text = "";
                                    txtSN.Text = "";
                                    txtQuantity.Text = "";
                                    cmbStatus.Text = "";
                                    MessageBox.Show("此车间上无此包收货！");
                                }
                            }
                            else
                            {
                                MessageBox.Show("卡片已回收！");
                                return;
                            }

                        }
                        else if (BitConverter.ToString(pszData, 11, 1) == "0D")
                        {//员工卡
                            MessageBox.Show("请刷物料卡！");
                            return;
                        }
                        CommonClass.PlaySoundBeep();
                    }
                    else
                    {
                        txtRoumap.Text = "";
                        txtRFID.Text = "";
                        //cmbCardCode.Text = "";
                        txtEBNum.Text = "";
                        txtPackageCode.Text = "";
                        txtSN.Text = "";
                        txtQuantity.Text = "";
                        cmbStatus.Text = "";
                        MessageBox.Show("刷卡失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                txtRoumap.Text = "";
                txtRFID.Text = "";
                //cmbCardCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                cmbStatus.Text = "";
                MessageBox.Show("刷卡失败！" + ex.Message);
            }

        }

        private void FrmRecRFID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 238)
            {
                BarCode_Scan();
            }
            else if (e.KeyValue == 8)
            {
                btnClear_Click(null, null);
            }
            else if (e.KeyValue == 120 || e.KeyValue == 121)
            {
                btnScan_Click(null, null);
            }
            else if (e.KeyValue == 13)
            {
                btnSubmit_Click(null, null);
            }
        }

        /// <summary>
        /// 扫描条码
        /// </summary>
        private void BarCode_Scan()
        {
            try
            {
                string RFID_ID = Barcode.scan();

                if (RFID_ID == "")
                {
                    txtRoumap.Text = "";
                    txtRFID.Text = "";
                    //cmbCardCode.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtSN.Text = "";
                    txtQuantity.Text = "";
                    cmbStatus.Text = "";
                    MessageBox.Show("扫描失败！");
                }
                else
                {
                    if (RFID_ID.Substring(0, 1) == "2")
                    {//物料卡
                        txtRFID.Text = RFID_ID;
                        string DeviceName = FunPublic.GetDeviceName();
                        DataTable dt = FunPublic.GetDt("exec [PDA_RecRFIDQuery] '" + RFID_ID + "'");
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                txtRoumap.Text = dr["roumap"].ToString();
                                txtEBNum.Text = dr["EBDOC"].ToString();
                                txtPackageCode.Text = dr["PackageCode"].ToString();
                                txtSN.Text = dr["SN"].ToString();
                                txtQuantity.Text = dr["QtyinPackage"].ToString();
                                cmbStatus.Text = dr["Status"].ToString();
                            }
                        }
                        else
                        {
                            txtRoumap.Text = "";
                            txtEBNum.Text = "";
                            txtPackageCode.Text = "";
                            txtSN.Text = "";
                            txtQuantity.Text = "";
                            cmbStatus.Text = "";
                            MessageBox.Show("此车间上无此包收货！");
                        }
                    }
                    else if (RFID_ID.Substring(0, 1) == "1")
                    {//员工卡
                        MessageBox.Show("请刷物料卡！");
                        return;
                    }
                    CommonClass.PlaySoundBeep();

                }
            }
            catch (Exception ex)
            {
                txtRoumap.Text = "";
                txtRFID.Text = "";
                //cmbCardCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                cmbStatus.Text = "";
                MessageBox.Show("扫描失败！" + ex.Message);
            }
        }


        private void FrmRecRFID_Closed(object sender, EventArgs e)
        {
            RFID_15693.freeMode();
            CommonClass.SoftDecoding_Deinit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRoumap.Text = "";
            txtRFID.Text = "";
            txtEBNum.Text = "";
            txtPackageCode.Text = "";
            txtSN.Text = "";
            txtQuantity.Text = "";
            cmbStatus.Text = "";
        }

    }
}