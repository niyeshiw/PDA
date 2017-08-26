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
    public partial class FrmDailyReport : Form
    {

        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();

        public FrmDailyReport()
        {
            InitializeComponent();
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹
        }

        private void FrmDailyReport_Load(object sender, EventArgs e)
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
            if (txtStaff.Text == "")
            {
                MessageBox.Show("请刷员工卡！");
                return;
            }
            if (txtRFID.Text == "")
            {
                MessageBox.Show("请刷物料卡！");
                return;
            }
            if (cmbStep.Text == "")
            {
                MessageBox.Show("请选择工序！");
                return;
            }
            DataTable dt = FunPublic.GetDt("select firstName from OHEM where U_rfid = '" + txtStaff.Text + "'");
            string DeviceName = FunPublic.GetDeviceName();
            List<string> strSqls = new List<string>();
            strSqls.Add("insert into Doc_DailyReport([DocEntry],[Doc1_LINE_ID],[Doc11_LINE_ID],[Doc12_LINE_ID],[EBDOC],[RFID_ID],[MappingCode],[PackageCode],[PackagesQty],[QtyinPackage],[ProRtEntry],[ProRtLine],[SN],[CSKU_CODE],[CSKU_NAME],[STEPCode],[STEPName],[Quantity],[WORKSHOP_CODE],[WORKSHOPSEQ],[STEPSEQ],[roumap],[QtyofSize],[StaffCode],[StationCode],[DOC_DATE],[QtyofReject],[QtyofRepair],[StaffName],[Type],STEP_PRICE,MODEL,C_COLOUR,B_COLOUR,ISFREESTEP,ISOPTION) select [DocEntry],[Doc1_LINE_ID],[Doc11_LINE_ID],[Doc12_LINE_ID],[EBDOC],[RFID_ID],[MappingCode],[PackageCode],[PackagesQty],[QtyinPackage],[ProRtEntry],[ProRtLine],[SN],[CSKU_CODE],[CSKU_NAME],[STEPCode],[STEPName],[Quantity],[WORKSHOP_CODE],[WORKSHOPSEQ],[STEPSEQ],[roumap],[QtyofSize],'" + txtStaff.Text.Substring(6, 4) + "','" + DeviceName.Substring(1, DeviceName.Length - 1) + "',getdate(),0,0,'" + dt.Rows[0][0].ToString() + "',[Type],STEP_PRICE,MODEL,C_COLOUR,B_COLOUR,ISFREESTEP,ISOPTION from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + "' and STEPCode = '" + cmbStep.Text.Substring(0, cmbStep.Text.IndexOf('-')) + "'");
            strSqls.Add("delete from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + "' and STEPCode = '" + cmbStep.Text.Substring(0, cmbStep.Text.IndexOf('-')) + "'");
            strSqls.Add("update Doc_Prodcutpackage14 set STATUS = 'F' where RFID_ID = '" + txtRFID.Text + "' and STEPCode = '" + cmbStep.Text.Substring(0, cmbStep.Text.IndexOf('-')) + "'");
            result = FunPublic.RunSqls(strSqls);
            if (result.Status == 1)
            {
                //MessageBox.Show("提交成功！");
                txtRFID.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                cmbStep.Items.Clear();
                cmbStep.Text = "";
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
                    txtRFID.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtSN.Text = "";
                    txtQuantity.Text = "";
                    cmbStep.Items.Clear();
                    cmbStep.Text = "";
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
                            if (Convert.ToInt32(FunPublic.GetDt("select COUNT(distinct EBDOC) from Doc_Prodcutpackage13 where RFID_ID='" + RFID_ID + "'").Rows[0][0]) > 1)
                            {
                                MessageBox.Show("此卡存在多个EB单号，不能刷卡！");
                                return;
                            }
                            string DeviceName = FunPublic.GetDeviceName();
                            DataTable dt = FunPublic.GetDt("exec [PDA_DailyReportQuery] '" + RFID_ID + "','" + DeviceName.Substring(1, DeviceName.Length - 1) + "'");
                            if (dt.Rows.Count > 0)
                            {
                                cmbStep.Items.Clear();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    txtEBNum.Text = dr["EBDOC"].ToString();
                                    txtPackageCode.Text = dr["PackageCode"].ToString();
                                    txtSN.Text = dr["SN"].ToString();
                                    txtQuantity.Text = dr["QtyinPackage"].ToString();
                                    cmbStep.Items.Add(dr["STEPCode"].ToString() + "-" + dr["STEPName"].ToString());
                                    cmbStep.Text = dr["STEPCode"].ToString() + "-" + dr["STEPName"].ToString();
                                }
                            }
                            else
                            {
                                MessageBox.Show("此设备上无可选工序！");
                            }
                        }
                        else if (BitConverter.ToString(pszData, 11, 1) == "0D")
                        {//员工卡
                            txtStaff.Text = RFID_ID;
                        }
                        CommonClass.PlaySoundBeep();
                    }
                    else
                    {
                        txtRFID.Text = "";
                        txtEBNum.Text = "";
                        txtPackageCode.Text = "";
                        txtSN.Text = "";
                        txtQuantity.Text = "";
                        cmbStep.Items.Clear();
                        cmbStep.Text = "";
                        MessageBox.Show("刷卡失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                txtRFID.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                cmbStep.Items.Clear();
                cmbStep.Text = "";
                MessageBox.Show("刷卡失败！" + ex.Message);
            }

        }

        private void FrmDailyReport_KeyDown(object sender, KeyEventArgs e)
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
                    txtRFID.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtSN.Text = "";
                    txtQuantity.Text = "";
                    cmbStep.Items.Clear();
                    cmbStep.Text = "";
                    MessageBox.Show("扫描失败！");
                }
                else
                {
                    if (RFID_ID.Substring(0, 1) == "2")
                    {//物料卡
                        txtRFID.Text = RFID_ID;
                        string DeviceName = FunPublic.GetDeviceName();
                        DataTable dt = FunPublic.GetDt("exec [PDA_DailyReportQuery] '" + RFID_ID + "','" + DeviceName.Substring(1, DeviceName.Length - 1) + "'");
                        if (dt.Rows.Count > 0)
                        {
                            cmbStep.Items.Clear();
                            foreach (DataRow dr in dt.Rows)
                            {
                                txtEBNum.Text = dr["EBDOC"].ToString();
                                txtPackageCode.Text = dr["PackageCode"].ToString();
                                txtSN.Text = dr["SN"].ToString();
                                txtQuantity.Text = dr["QtyinPackage"].ToString();
                                cmbStep.Items.Add(dr["STEPCode"].ToString() + "-" + dr["STEPName"].ToString());
                                cmbStep.Text = dr["STEPCode"].ToString() + "-" + dr["STEPName"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("此设备上无可选工序！");
                        }
                    }
                    else if (RFID_ID.Substring(0, 1) == "1")
                    {//员工卡
                        txtStaff.Text = RFID_ID;
                    }
                    CommonClass.PlaySoundBeep();

                }
            }
            catch (Exception ex)
            {
                txtRFID.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                cmbStep.Items.Clear();
                cmbStep.Text = "";
                MessageBox.Show("扫描失败！" + ex.Message);
            }
        }

        private void FrmDailyReport_Closed(object sender, EventArgs e)
        {
            RFID_15693.freeMode();
            CommonClass.SoftDecoding_Deinit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStaff.Text = "";
            txtRFID.Text = "";
            txtEBNum.Text = "";
            txtPackageCode.Text = "";
            txtSN.Text = "";
            txtQuantity.Text = "";
            cmbStep.Items.Clear();
            cmbStep.Text = "";
        }

    }
}