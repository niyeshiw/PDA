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
    public partial class FrmOSMapping : Form
    {
        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();
        public string model = "菜单";

        public FrmOSMapping()
        {
            InitializeComponent();
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹
            txtRFID1.Focus();
        }

        private void FrmWorkshooGR_Load(object sender, EventArgs e)
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
            if (model == "弹窗")
            {
                txtRFID2.Focus();
            }
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
            if (txtRFID1.Text == "" || txtRFID2.Text == "")
            {
                MessageBox.Show("请刷物料卡！");
                return;
            }
            List<string> strSqls = new List<string>();
            strSqls.Add(@"update t0 set t0.OsMappingStatus='Y' from Doc_WorkshopGR t0 inner join (
		select Result as STEPCode from dbo.Fun_SplitStr((select Value from Ts_Config where [Key] = 'MappingOs'),'#')
		) t1 on t0.STEPCode = t1.STEPCode where t0.MappingCode = '" + txtMappingCode.Text + "' and t0.RFID_ID='" + txtRFID1.Text + "'");
            strSqls.Add(@"update t0 set t0.OsMappingStatus='Y' from Doc_WorkshopGR t0 inner join (
		select Result as STEPCode from dbo.Fun_SplitStr((select Value from Ts_Config where [Key] = 'MappingOs'),'#')
		) t1 on t0.STEPCode = t1.STEPCode where t0.MappingCode = '" + txtMappingCode.Text + "' and t0.RFID_ID='" + txtRFID2.Text + "'");
            strSqls.Add(@"update t0 set MappingStatus = 'Y' from Doc_OSGoodsIssu1 t0 where DocEntry =(SELECT MAX(DocEntry) FROM Doc_OSGoodsIssu1 WHERE LINESTATUS='F' AND RFID_ID='" + txtRFID1.Text + "')");
            strSqls.Add(@"update t0 set MappingStatus = 'Y' from Doc_OSGoodsIssu1 t0 where DocEntry =(SELECT MAX(DocEntry) FROM Doc_OSGoodsIssu1 WHERE LINESTATUS='F' AND RFID_ID='" + txtRFID2.Text + "')");
            //回收卡

            string sql = @"if not exists(select count(1) from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID1.Text + @"'
and not exists(select count(1) from Doc_OSGoodsIssu1 where RFID_ID='" + txtRFID1.Text + @"' and LINESTATUS ='O') and NOT EXISTS(
select count(1) from Doc_OSGoodsIssu1 where LINESTATUS = 'F' AND ISNULL(MappingStatus,'N')='N' AND RFID_ID ='" + txtRFID1.Text + @"'))
	begin
    update Tm_RFID set STATUS = 'O' where RFIDLUN = '" + txtRFID1.Text + @"'
    declare @MaxDoc as int
select @MaxDoc = isnull(max(DocEntry) + 1,1) from Doc_recRFID
INSERT INTO [Doc_recRFID]([DocEntry],[CREATED],[DOC_DATE],[EBDOC],[SN],[PackageCode],[RFID_ID],[roumap])
select top 1 @MaxDoc,'" + FunPublic.CurrentUser + "',getdate(),'" + txtEBNum.Text + "','" + txtSN.Text + "','" + txtPackageCode.Text + "','" + txtRFID1.Text + "',roumap from Doc_Prodcutpackage14 where RFID_ID='" + txtRFID1.Text + "' and EBDOC='" + txtEBNum.Text + @"' and MappingCode = '" + txtMappingCode.Text + @"'
    end";
            strSqls.Add(sql);


            result = FunPublic.RunSqls(strSqls);
            if (result.Status == 1)
            {
                txtRFID1.Text = "";
                txtRFID2.Text = "";
                txtMappingCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                if ("弹窗".Equals(model))
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(result.Message);
            }
            this.panel1.Visible = false;
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
                    txtMappingCode.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtSN.Text = "";
                    txtQuantity.Text = "";
                    MessageBox.Show("刷卡失败！");
                }
                else
                {
                    int res = RFID_15693.RF_ISO15693_getSystemInformation(0, data, 0, pszData);

                    if (res == 0)
                    {
                        if (BitConverter.ToString(pszData, 11, 1) == "0B")
                        {//物料卡
                            if (!CommonClass.ValidateRFID(RFID_ID))
                            {
                                throw new Exception("此卡已经被回收！");
                            }
                            if (txtRFID1.Focused)
                            {
                                DataTable dt = FunPublic.GetDt("exec [PDA_MappingQuery] '" + RFID_ID + "','4','',''");
                                if (dt.Rows.Count > 0)
                                {
                                    DataRow[] dr = dt.Select("MappingCode='" + txtMappingCode.Text + "'");
                                    if (dr.Length > 0)
                                    {
                                        txtRFID1.Text = RFID_ID;
                                        if (txtRFID2.Text != "")
                                        {
                                            this.panel1.Visible = true;
                                        }
                                        else
                                        {
                                            txtRFID2.Focus();
                                            return;
                                        }
                                    }
                                    else if (dr.Length == 0 && txtMappingCode.Text == "")
                                    {
                                        txtRFID1.Text = RFID_ID;
                                        txtMappingCode.Text = dt.Rows[0]["MappingCode"].ToString();
                                        txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                                        txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                                        txtSN.Text = dt.Rows[0]["SN"].ToString();
                                        txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                                        if (txtRFID2.Text != "")
                                        {
                                            this.panel1.Visible = true;
                                        }
                                        else
                                        {
                                            txtRFID2.Focus();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("无匹配！");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("未做收货！");
                                    return;
                                }
                            }
                            if (txtRFID2.Focused)
                            {
                                DataTable dt = FunPublic.GetDt("exec [PDA_MappingQuery] '" + RFID_ID + "','5','',''");
                                if (dt.Rows.Count > 0)
                                {
                                    DataRow[] dr = dt.Select("MappingCode='" + txtMappingCode.Text + "'");
                                    if (dr.Length > 0)
                                    {
                                        txtRFID2.Text = RFID_ID;
                                        if (txtRFID1.Text != "")
                                        {
                                            this.panel1.Visible = true;
                                        }
                                        else
                                        {
                                            txtRFID1.Focus();
                                        }
                                    }
                                    else if (dr.Length == 0 && txtMappingCode.Text == "")
                                    {
                                        txtRFID2.Text = RFID_ID;
                                        txtMappingCode.Text = dt.Rows[0]["MappingCode"].ToString();
                                        txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                                        txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                                        txtSN.Text = dt.Rows[0]["SN"].ToString();
                                        txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                                        if (txtRFID1.Text != "")
                                        {
                                            this.panel1.Visible = true;
                                        }
                                        else
                                        {
                                            txtRFID1.Focus();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("无匹配！");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("未做收货！");
                                    return;
                                }
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
                        txtRFID2.Text = "";
                        txtMappingCode.Text = "";
                        txtEBNum.Text = "";
                        txtPackageCode.Text = "";
                        txtSN.Text = "";
                        txtQuantity.Text = "";
                        MessageBox.Show("刷卡失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                txtRFID2.Text = "";
                txtMappingCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                MessageBox.Show("刷卡失败！" + ex.Message);
            }

        }

        private void FrmWorkshooGR_KeyDown(object sender, KeyEventArgs e)
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
                    txtMappingCode.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtSN.Text = "";
                    txtQuantity.Text = "";
                    MessageBox.Show("扫描失败！");
                }
                else
                {
                    if (RFID_ID.Substring(0, 1) == "2")
                    {//物料卡
                        if (txtRFID1.Focused)
                        {
                            DataTable dt = FunPublic.GetDt("exec [PDA_MappingQuery] '" + RFID_ID + "','4','',''");
                            if (dt.Rows.Count > 0)
                            {
                                DataRow[] dr = dt.Select("MappingCode='" + txtMappingCode.Text + "'");
                                if (dr.Length > 0)
                                {
                                    txtRFID1.Text = RFID_ID;
                                    if (txtRFID2.Text != "")
                                    {
                                        this.panel1.Visible = true;
                                    }
                                    else
                                    {
                                        txtRFID2.Focus();
                                        return;
                                    }
                                }
                                else if (dr.Length == 0 && txtMappingCode.Text == "")
                                {
                                    txtRFID1.Text = RFID_ID;
                                    txtMappingCode.Text = dt.Rows[0]["MappingCode"].ToString();
                                    txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                                    txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                                    txtSN.Text = dt.Rows[0]["SN"].ToString();
                                    txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                                    if (txtRFID2.Text != "")
                                    {
                                        this.panel1.Visible = true;
                                    }
                                    else
                                    {
                                        txtRFID2.Focus();
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("无匹配！");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("未做收货！");
                                return;
                            }
                        }
                        if (txtRFID2.Focused)
                        {
                            DataTable dt = FunPublic.GetDt("exec [PDA_MappingQuery] '" + RFID_ID + "','5','',''");
                            if (dt.Rows.Count > 0)
                            {
                                DataRow[] dr = dt.Select("MappingCode='" + txtMappingCode.Text + "'");
                                if (dr.Length > 0)
                                {
                                    txtRFID2.Text = RFID_ID;
                                    if (txtRFID1.Text != "")
                                    {
                                        this.panel1.Visible = true;
                                    }
                                    else
                                    {
                                        txtRFID1.Focus();
                                        return;
                                    }
                                }
                                else if (dr.Length == 0 && txtMappingCode.Text == "")
                                {
                                    txtRFID2.Text = RFID_ID;
                                    txtMappingCode.Text = dt.Rows[0]["MappingCode"].ToString();
                                    txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                                    txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                                    txtSN.Text = dt.Rows[0]["SN"].ToString();
                                    txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                                    if (txtRFID1.Text != "")
                                    {
                                        this.panel1.Visible = true;
                                    }
                                    else
                                    {
                                        txtRFID1.Focus();
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("无匹配！");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("未做收货！");
                                return;
                            }
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
                txtRFID2.Text = "";
                txtMappingCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                MessageBox.Show("扫描失败！" + ex.Message);
            }

        }

        private void FrmWorkshooGR_Closed(object sender, EventArgs e)
        {
            RFID_15693.freeMode();
            CommonClass.SoftDecoding_Deinit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRFID1.Text = "";
            txtRFID2.Text = "";
            txtMappingCode.Text = "";
            txtEBNum.Text = "";
            txtPackageCode.Text = "";
            txtSN.Text = "";
            txtQuantity.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
        }

    }
}