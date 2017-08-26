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
    public partial class FrmMapping : Form
    {
        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();
        public string model = "菜单";//打开页面的方式

        public FrmMapping()
        {
            InitializeComponent();
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹
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
            try
            {
                string DeviceName = FunPublic.GetDeviceName();
                List<string> strSqls = new List<string>();
                //提交
                Submit(DeviceName.Substring(1, DeviceName.Length - 1), ref strSqls);

                //回收卡不包括主路线
                string rfid = string.Empty;
                string roumap = string.Empty;
                if (txtRoumap1.Text != "主路线")
                {
                    rfid = txtRFID1.Text;
                    roumap = txtRoumap1.Text;
                }
                else
                {
                    rfid = txtRFID2.Text;
                    roumap = txtRoumap2.Text;
                }


                string sql = @"if not exists(select DocEntry from Doc_Prodcutpackage13 where RFID_ID = '" + rfid + @"')
	begin
    update Tm_RFID set STATUS = 'O' where RFIDLUN = '" + rfid + @"'
    declare @MaxDoc as int
select @MaxDoc = isnull(max(DocEntry) + 1,1) from Doc_recRFID
INSERT INTO [Doc_recRFID]([DocEntry],[CREATED],[DOC_DATE],[EBDOC],[SN],[PackageCode],[RFID_ID],[roumap])
select top 1 @MaxDoc,'" + FunPublic.CurrentUser + "',getdate(),'" + txtEBNum.Text + "','" + txtSN.Text + "','" + txtPackageCode.Text + "','" + rfid + "',roumap from Doc_Prodcutpackage14 where RFID_ID='" + rfid + "' and EBDOC='" + txtEBNum.Text + @"'
    end";
                strSqls.Add(sql);
                result = FunPublic.RunSqls(strSqls);
                if (result.Status == 1)
                {
                    //如果此PDA属于完成线，判断主路线是否还有可以配货的卡片，并且将窗体标题改为对应路线
                    if (txtRoumap1.Text == "主路线" && (txtRoumap2.Text == "后带路线" || txtRoumap2.Text == "帽眉路线"))
                    {
                        string strsql = @"SELECT *,roumapName= (select name from Tm_roumap where DocEntry = roumap) FROM Doc_WorkshopGR 
                               WHERE MappingCode ='" + txtMappingCode.Text + @"' 
                               AND 
                               ((MmMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMM') AND roumap = 2) 
                                OR 
                               (HdMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingHD') AND roumap = 7))";
                        DataTable dt = FunPublic.GetDt(strsql);
                        if (dt.Rows.Count > 0)
                        {
                            this.Text = "配货 -" + dt.Rows[0]["roumapName"].ToString();
                            txtRFID2.Text = "";
                            txtRoumap2.Text = "";
                        }
                        else
                        {
                            txtRFID2.Text = "";
                            txtRoumap2.Text = "";
                            txtRFID1.Text = "";
                            txtRoumap1.Text = "";
                            txtMappingCode.Text = "";
                            txtEBNum.Text = "";
                            txtPackageCode.Text = "";
                            txtSN.Text = "";
                            txtQuantity.Text = "";
                        }
                    }
                    else
                    {
                        txtRFID2.Text = "";
                        txtRoumap2.Text = "";
                        txtRFID1.Text = "";
                        txtRoumap1.Text = "";
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
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
                this.panel1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                Scan();
            }
            catch (Exception ex)
            {
                txtRFID1.Text = "";
                txtRFID2.Text = "";
                txtRoumap1.Text = "";
                txtRoumap2.Text = "";
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

        private void FrmWorkshooGR_Closed(object sender, EventArgs e)
        {
            RFID_15693.freeMode();
            CommonClass.SoftDecoding_Deinit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRFID1.Text = "";
            txtRoumap1.Text = "";
            txtRFID2.Text = "";
            txtRoumap2.Text = "";
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
        //刷卡
        private void Scan()
        {
            byte[] pszData = new byte[25];
            byte[] data = new byte[1];

            if (RFID_15693.ScanSingleTag(ref data))
            {
                //CommonClass.PlaySound();
            }
            else
            {
                throw new Exception("扫描失败,请确认是否是15693标签,并确认标签是否处于RFID感应区！");
            }
            string RFID_ID = RFID_15693.RFID_ID();

            if (RFID_ID == "")
            {
                throw new Exception("扫描失败,RFID号为空！");
            }
            int res = RFID_15693.RF_ISO15693_getSystemInformation(0, data, 0, pszData);
            if (res != 0)
            {
                throw new Exception("扫描失败,获取卡类型时出错！");
            }

            if (BitConverter.ToString(pszData, 11, 1) == "0B")//物料卡
            {
                if (!CommonClass.ValidateRFID(RFID_ID))
                {
                    throw new Exception("此卡已经被回收！");
                }
                string DeviceName = FunPublic.GetDeviceName();
                //判断PDA的车间类型是否满足条件
                DataTable workshop = FunPublic.GetDt("select WORKSHOP_CODE from dbo.Tm_Station t0 where ReaderCode = '" + DeviceName.Substring(1, DeviceName.Length - 1) + @"' and  TYPE ='P' AND WORKSHOP_CODE in (select * from dbo.Fun_SplitStr((select value from Ts_Config where [key] = 'MapingWorkShop'),'#'))");
                if (workshop.Rows.Count == 0)
                {
                    throw new Exception("车间不符，无法匹配");
                }
                if (txtRFID1.Text == "")
                {
                    txtRFID1.Text = RFID_ID;
                    DataTable dt = FunPublic.GetDt("exec [PDA_MappingQuery] '" + RFID_ID + "','1','',''");

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            txtRoumap1.Text = dr["roumap"].ToString();
                            txtMappingCode.Text = dr["MappingCode"].ToString();
                            txtEBNum.Text = dr["EBDOC"].ToString();
                            txtPackageCode.Text = dr["PackageCode"].ToString();
                            txtSN.Text = dr["SN"].ToString();
                            txtQuantity.Text = dr["QtyinPackage"].ToString();
                        }
                    }
                    else
                    {
                        throw new Exception("卡片没有可匹配的工序！");
                    }
                }
                else
                {
                    txtRFID2.Text = RFID_ID;
                    if (txtRFID1.Text == txtRFID2.Text)
                    {
                        txtRFID2.Text = "";
                        MessageBox.Show("相同卡不能匹配！");
                        return;
                    }
                    Mapping(DeviceName.Substring(1, DeviceName.Length - 1));
                }
            }
            else if (BitConverter.ToString(pszData, 11, 1) == "0D")//员工卡
            {
                throw new Exception("请刷物料卡！");
            }
            CommonClass.PlaySoundBeep();
        }
        // 扫描条码
        private void BarCode_Scan()
        {
            try
            {
                string RFID_ID = Barcode.scan();

                if (RFID_ID == "")
                {
                    throw new Exception("扫描失败,RFID号为空！");
                }
                if (RFID_ID.Substring(0, 1) == "2")//物料卡
                {
                    string DeviceName = FunPublic.GetDeviceName();
                    //判断PDA的车间类型是否满足条件
                    DataTable workshop = FunPublic.GetDt("select WORKSHOP_CODE from dbo.Tm_Station t0 where ReaderCode = '" + DeviceName.Substring(1, DeviceName.Length - 1) + @"' and  TYPE ='P' AND WORKSHOP_CODE in (select * from dbo.Fun_SplitStr((select value from Ts_Config where [key] = 'MapingWorkShop'),'#'))");
                    if (workshop.Rows.Count == 0)
                    {
                        throw new Exception("车间不符，无法匹配");
                    }
                    if (txtRFID1.Text == "")
                    {
                        txtRFID1.Text = RFID_ID;
                        DataTable dt = FunPublic.GetDt("exec [PDA_MappingQuery] '" + RFID_ID + "','1','',''");

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                txtRoumap1.Text = dr["roumap"].ToString();
                                txtMappingCode.Text = dr["MappingCode"].ToString();
                                txtEBNum.Text = dr["EBDOC"].ToString();
                                txtPackageCode.Text = dr["PackageCode"].ToString();
                                txtSN.Text = dr["SN"].ToString();
                                txtQuantity.Text = dr["QtyinPackage"].ToString();
                            }
                        }
                        else
                        {
                            throw new Exception("卡片没有可匹配的工序！");
                        }
                    }
                    else
                    {
                        txtRFID2.Text = RFID_ID;
                        if (txtRFID1.Text == txtRFID2.Text)
                        {
                            txtRFID2.Text = "";
                            MessageBox.Show("相同卡不能匹配！");
                            return;
                        }
                        Mapping(DeviceName.Substring(1, DeviceName.Length - 1));
                    }
                }
                else if (RFID_ID.Substring(0, 1) == "1")//员工卡
                {
                    throw new Exception("请刷物料卡！");
                }
                CommonClass.PlaySoundBeep();
            }
            catch (Exception ex)
            {
                txtRFID1.Text = "";
                txtRFID2.Text = "";
                txtRoumap1.Text = "";
                txtRoumap2.Text = "";
                txtMappingCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";
                MessageBox.Show("刷卡失败！" + ex.Message);
            }
        }
        //匹配
        private void Mapping(string DeviceName)
        {
            string strsql = string.Empty;
            string rfid = txtRFID2.Text;
            string mappingcode = txtMappingCode.Text;
            string roumap = txtRoumap1.Text;

            //判断PDA的车间类型是否满足条件
            DataTable workshop = FunPublic.GetDt("select WORKSHOP_CODE from dbo.Tm_Station t0 where ReaderCode = '" + DeviceName + @"' and  TYPE ='P' AND WORKSHOP_CODE in
(select * from dbo.Fun_SplitStr((select value from Ts_Config where [key] = 'MapingWorkShop'),'#'))");
            if (workshop.Rows.Count == 0)
            {
                MessageBox.Show("车间类型不正确");
                txtRFID2.Text = "";
                return;
            }
            if ("1007".Equals(workshop.Rows[0]["WORKSHOP_CODE"]))//收发室 1:主路线  5:内衬
            {
                if (txtRoumap1.Text == "主路线")
                {
                    strsql = "select *,roumapName= (select name from Tm_roumap where DocEntry = roumap) from Doc_WorkshopGR where NcMappingStatus ='N' AND RFID_ID ='" + txtRFID2.Text + "' AND STEPCode =(select value from Ts_Config where [KEY]='FMappinNC') AND MappingCode ='" + mappingcode + "' AND roumap ='5'";
                }
                else if (txtRoumap1.Text == "内衬路线")
                {
                    strsql = "select *,roumapName= (select name from Tm_roumap where DocEntry = roumap) from Doc_WorkshopGR where NcMappingStatus ='N' AND RFID_ID ='" + txtRFID2.Text + "' AND STEPCode =(select value from Ts_Config where [KEY]='FMappingMain') AND MappingCode ='" + mappingcode + "' AND roumap ='1'";
                }
                else
                {
                    MessageBox.Show("路线与收发室不匹配");
                    txtRFID2.Text = "";
                    return;
                }
            }
            else if ("1005".Equals(workshop.Rows[0]["WORKSHOP_CODE"]))//完成线 1:主路线 2:帽眉  7:后带
            {
                if (txtRoumap1.Text == "主路线")
                {
                    strsql = @"SELECT *,roumapName= (select name from Tm_roumap where DocEntry = roumap) FROM Doc_WorkshopGR 
                               WHERE RFID_ID ='" + txtRFID2.Text + "' AND MappingCode ='" + mappingcode + @"' 
                               AND 
                               ((MmMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMM') AND roumap = 2) 
                                OR 
                               (HdMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingHD') AND roumap = 7))";
                }
                else if (txtRoumap1.Text == "后带路线" || txtRoumap1.Text == "帽眉路线")
                {
                    strsql = @"SELECT *,roumapName= (select name from Tm_roumap where DocEntry = roumap) FROM Doc_WorkshopGR 
                               WHERE RFID_ID ='" + txtRFID2.Text + "' AND MappingCode ='" + mappingcode + @"' 
                               AND roumap = 1 AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMain') 
                               AND (MmMappingStatus ='N' OR HdMappingStatus ='N')";
                }
                else
                {
                    MessageBox.Show("路线与完成线不匹配");
                    txtRFID2.Text = "";
                    return;
                }
            }

            DataTable dt = FunPublic.GetDt(strsql);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("没有找到对应工序");
                txtRFID2.Text = "";
                return;
            }
            txtRoumap2.Text = dt.Rows[0]["roumapName"].ToString();
            this.panel1.Visible = true;
        }
        //提交
        private void Submit(string DeviceName, ref List<string> strSqls)
        {
            //获取车间类型
            DataTable workshop = FunPublic.GetDt("select WORKSHOP_CODE from dbo.Tm_Station t0 where ReaderCode = '" + DeviceName + @"' and  TYPE ='P' AND WORKSHOP_CODE in
(select * from dbo.Fun_SplitStr((select value from Ts_Config where [key] = 'MapingWorkShop'),'#'))");
            if (workshop.Rows.Count == 0)
            {
                throw new Exception("车间不符，无法匹配");
            }

            if ("1007".Equals(workshop.Rows[0]["WORKSHOP_CODE"]))//收发室 1:主路线  5:内衬
            {
                if (txtRoumap1.Text == "主路线")
                {
                    strSqls.Add("update Doc_WorkshopGR set NcMappingStatus ='Y' where NcMappingStatus ='N' AND RFID_ID ='" + txtRFID2.Text + "' AND STEPCode =(select value from Ts_Config where [KEY]='FMappinNC') AND MappingCode ='" + txtMappingCode.Text + "' AND roumap ='5'");
                    strSqls.Add("update Doc_WorkshopGR set NcMappingStatus ='Y' where NcMappingStatus ='N' AND RFID_ID ='" + txtRFID1.Text + "' AND STEPCode =(select value from Ts_Config where [KEY]='FMappingMain') AND MappingCode ='" + txtMappingCode.Text + "' AND roumap ='1'");
                }
                else if (txtRoumap1.Text == "内衬路线")
                {
                    strSqls.Add("update Doc_WorkshopGR set NcMappingStatus ='Y' where NcMappingStatus ='N' AND RFID_ID ='" + txtRFID2.Text + "' AND STEPCode =(select value from Ts_Config where [KEY]='FMappingMain') AND MappingCode ='" + txtMappingCode.Text + "' AND roumap ='1'");
                    strSqls.Add("update Doc_WorkshopGR set NcMappingStatus ='Y' where NcMappingStatus ='N' AND RFID_ID ='" + txtRFID1.Text + "' AND STEPCode =(select value from Ts_Config where [KEY]='FMappinNC') AND MappingCode ='" + txtMappingCode.Text + "' AND roumap ='5'");
                }
            }
            else if ("1005".Equals(workshop.Rows[0]["WORKSHOP_CODE"]))//完成线 1:主路线 2:帽眉  7:后带
            {
                if (txtRoumap1.Text == "主路线" && txtRoumap2.Text == "后带路线")
                {
                    strSqls.Add(@"update Doc_WorkshopGR set HdMappingStatus ='Y'  
                               WHERE RFID_ID ='" + txtRFID2.Text + "' AND MappingCode ='" + txtMappingCode.Text + @"' 
                               AND HdMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingHD') AND roumap = 7");
                    strSqls.Add(@"update Doc_WorkshopGR set HdMappingStatus ='Y'  
                               WHERE RFID_ID ='" + txtRFID1.Text + "' AND MappingCode ='" + txtMappingCode.Text + @"' 
                               AND roumap = 1 AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMain') 
                               AND HdMappingStatus ='N'");
                }
                else if (txtRoumap1.Text == "主路线" && txtRoumap2.Text == "帽眉路线")
                {
                    strSqls.Add(@"update Doc_WorkshopGR set MmMappingStatus ='Y'  
                               WHERE RFID_ID ='" + txtRFID2.Text + "' AND MappingCode ='" + txtMappingCode.Text + @"' 
                               AND MmMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMM') AND roumap = 2");
                    strSqls.Add(@"update Doc_WorkshopGR set MmMappingStatus ='Y'  
                               WHERE RFID_ID ='" + txtRFID1.Text + "' AND MappingCode ='" + txtMappingCode.Text + @"' 
                               AND roumap = 1 AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMain') 
                               AND MmMappingStatus ='N'");
                }
                else if (txtRoumap1.Text == "帽眉路线")
                {
                    strSqls.Add(@"update Doc_WorkshopGR set MmMappingStatus ='Y'  
                               WHERE RFID_ID ='" + txtRFID2.Text + "' AND MappingCode ='" + txtMappingCode.Text + @"' 
                               AND roumap = 1 AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMain') 
                               AND MmMappingStatus ='N'");
                    strSqls.Add(@"update Doc_WorkshopGR set MmMappingStatus ='Y'  
                               WHERE RFID_ID ='" + txtRFID1.Text + "' AND MappingCode ='" + txtMappingCode.Text + @"' 
                               AND MmMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMM') AND roumap = 2");
                }
                else if (txtRoumap1.Text == "后带路线")
                {
                    strSqls.Add(@"update Doc_WorkshopGR set HdMappingStatus ='Y'  
                               WHERE RFID_ID ='" + txtRFID2.Text + "' AND MappingCode ='" + txtMappingCode.Text + @"' 
                               AND roumap = 1 AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMain') 
                               AND HdMappingStatus ='N'");
                    strSqls.Add(@"update Doc_WorkshopGR set HdMappingStatus ='Y'  
                               WHERE RFID_ID ='" + txtRFID1.Text + "' AND MappingCode ='" + txtMappingCode.Text + @"' 
                               AND HdMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingHD') AND roumap = 7");
                }
            }
        }

    }
}