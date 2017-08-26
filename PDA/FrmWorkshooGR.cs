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
    public partial class FrmWorkshooGR : Form
    {
        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();

        public FrmWorkshooGR()
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
            try
            {
                //获取设备名称
                string DeviceName = FunPublic.GetDeviceName();
                //提交
                bool result = Submit(DeviceName);
                //提交成功尝试匹配
                if (result)
                {
                    result = Mapping(DeviceName.Substring(1, DeviceName.Length - 1));
                    //清空页面
                    txtRFID.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtSN.Text = "";
                    txtQuantity.Text = "";
                    cmbStep.Items.Clear();
                    cmbStep.Text = "";
                }
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
                Scan();//扫描
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
                MessageBox.Show(ex.Message);
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
            txtRFID.Text = "";
            txtEBNum.Text = "";
            txtPackageCode.Text = "";
            txtSN.Text = "";
            txtQuantity.Text = "";
            cmbStep.Items.Clear();
            cmbStep.Text = "";
        }
        //扫描条码
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
                    if (!CommonClass.ValidateRFID(RFID_ID))
                    {
                        throw new Exception("此卡已经被回收！");
                    }
                    txtRFID.Text = RFID_ID;
                    string DeviceName = FunPublic.GetDeviceName();
                    DataTable dt = FunPublic.GetDt("exec [PDA_WorkshopGRQuery] '" + RFID_ID + "','" + DeviceName.Substring(1, DeviceName.Length - 1) + "'");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            txtEBNum.Text = dr["EBDOC"].ToString();
                            txtPackageCode.Text = dr["PackageCode"].ToString();
                            txtSN.Text = dr["SN"].ToString();
                            txtQuantity.Text = dr["QtyinPackage"].ToString();
                            cmbStep.Items.Add(dr["STEPCode"].ToString() + "-" + dr["STEPName"].ToString());
                            cmbStep.Text = dr["STEPCode"].ToString() + "-" + dr["STEPName"].ToString();
                            txtMappingCode.Text = dr["MappingCode"].ToString();
                            txtRouMap.Text = dr["roumap"].ToString();
                            txtRouMapName.Text = dr["roumapName"].ToString();
                        }
                    }
                    else
                    {
                        throw new Exception("此车间已收货！");
                    }
                }
                else if (RFID_ID.Substring(0, 1) == "1")
                {//员工卡
                    txtStaff.Text = RFID_ID;
                }
                CommonClass.PlaySoundBeep();

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
                MessageBox.Show(ex.Message);
            }

        }
        //刷卡
        private void Scan()
        {
            byte[] pszData = new byte[25];
            byte[] data = new byte[1];

            if (RFID_15693.ScanSingleTag(ref data))
            {

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
                txtRFID.Text = RFID_ID;
                if (!CommonClass.ValidateRFID(RFID_ID))
                {
                    throw new Exception("此卡已经被回收！");
                }
                string DeviceName = FunPublic.GetDeviceName();
                DataTable dt = FunPublic.GetDt("exec [PDA_WorkshopGRQuery] '" + RFID_ID + "','" + DeviceName.Substring(1, DeviceName.Length - 1) + "'");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (FunPublic.GetDt(string.Format("select STEPCODE from Doc_prodcutPackage13 where EBDOC='{0}' and RFID_ID = '{1}' AND STEPSEQ <{2} ", dr["EBDOC"], RFID_ID, dr["STEPSEQ"])).Rows.Count > 1)
                            throw new Exception("此卡片还有没有完工的工序，不能收货！");
                        txtEBNum.Text = dr["EBDOC"].ToString();
                        txtPackageCode.Text = dr["PackageCode"].ToString();
                        txtSN.Text = dr["SN"].ToString();
                        txtQuantity.Text = dr["QtyinPackage"].ToString();
                        cmbStep.Items.Add(dr["STEPCode"].ToString() + "-" + dr["STEPName"].ToString());
                        cmbStep.Text = dr["STEPCode"].ToString() + "-" + dr["STEPName"].ToString();
                        txtMappingCode.Text = dr["MappingCode"].ToString();
                        txtRouMap.Text = dr["roumap"].ToString();
                        txtRouMapName.Text = dr["roumapName"].ToString();
                    }
                }
                else
                {
                    throw new Exception("此车间已收货！");
                }
            }
            else if (BitConverter.ToString(pszData, 11, 1) == "0D")//员工卡
            {
                txtStaff.Text = RFID_ID;
            }
            CommonClass.PlaySoundBeep();
        }
        //提交
        private bool Submit(string DeviceName)
        {
            //获取员工名称
            DataTable dt = FunPublic.GetDt("select firstName from OHEM where U_rfid = '" + txtStaff.Text + "'");
            //将信息写入车间收货表
            List<string> strSqls = new List<string>();
            strSqls.Add(@"insert into Doc_WorkshopGR([DocEntry],[Doc1_LINE_ID],[Doc11_LINE_ID],[Doc12_LINE_ID],[EBDOC],[RFID_ID],[MappingCode],[PackageCode],[PackagesQty],[QtyinPackage],[ProRtEntry],[ProRtLine],[SN],[CSKU_CODE],[CSKU_NAME],[STEPCode],[STEPName],[Quantity],[WORKSHOP_CODE],[WORKSHOPSEQ],[STEPSEQ],[roumap],[QtyofSize],[StaffCode],[StationCode],[DOC_DATE],[QtyofReject],[QtyofRepair],[StaffName],FACTORY_CODE,Type,C_COLOUR,B_COLOUR,QtyofSizePackage) 
            select [DocEntry],[Doc1_LINE_ID],[Doc11_LINE_ID],[Doc12_LINE_ID],[EBDOC],[RFID_ID],[MappingCode],[PackageCode],[PackagesQty],[QtyinPackage],[ProRtEntry],[ProRtLine],[SN],[CSKU_CODE],[CSKU_NAME],[STEPCode],[STEPName],[Quantity],[WORKSHOP_CODE],[WORKSHOPSEQ],[STEPSEQ],[roumap],[QtyofSize],'" + txtStaff.Text.Substring(6, 4) + "','" + DeviceName.Substring(1, DeviceName.Length - 1) + "',getdate(),0,0,'" + dt.Rows[0][0].ToString() + "',FACTORY_CODE,Type,C_COLOUR,B_COLOUR,QtyofSizePackage from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + "' and STEPCode = '" + cmbStep.Text.Substring(0, cmbStep.Text.IndexOf('-')) + "'");
            strSqls.Add("delete from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + "' and STEPCode = '" + cmbStep.Text.Substring(0, cmbStep.Text.IndexOf('-')) + "'");
            strSqls.Add("update Doc_Prodcutpackage14 set STATUS = 'F' where RFID_ID = '" + txtRFID.Text + "' and STEPCode = '" + cmbStep.Text.Substring(0, cmbStep.Text.IndexOf('-')) + "'");

            //回收卡
            if (FunPublic.GetDt("select Value from Ts_Config where [KEY] = 'WorkShopGRHS' and Value like '%" + txtRouMap.Text + "%'").Rows.Count > 0)
            {
                string sql = @"if not exists(select DocEntry from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + @"')
                begin
                update Tm_RFID set STATUS = 'O' where RFIDLUN = '" + txtRFID.Text + @"'
                declare @MaxDoc as int
                select @MaxDoc = isnull(max(DocEntry) + 1,1) from Doc_recRFID
                INSERT INTO [Doc_recRFID]([DocEntry],[CREATED],[DOC_DATE],[EBDOC],[SN],[PackageCode],[RFID_ID],[roumap])
                select top 1 @MaxDoc,'" + FunPublic.CurrentUser + "',getdate(),'" + txtEBNum.Text + "','" + txtSN.Text + "','" + txtPackageCode.Text + "','" + txtRFID.Text + "',roumap from Doc_Prodcutpackage14 where RFID_ID='" + txtRFID.Text + "' and EBDOC='" + txtEBNum.Text + @"'
                end ";
                strSqls.Add(sql);
            }


            result = FunPublic.RunSqls(strSqls);
            //提交成功
            if (result.Status == 1)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        //匹配
        private bool Mapping(string DeviceName)
        {
            string strsql = string.Empty;
            string rfid = txtRFID.Text;
            string mappingcode = txtMappingCode.Text;
            string roumap = txtRouMap.Text;

            //判断PDA的车间类型是否满足条件
            DataTable workshop = FunPublic.GetDt("select WORKSHOP_CODE from dbo.Tm_Station t0 where ReaderCode = '" + DeviceName + @"' and  TYPE ='P' AND WORKSHOP_CODE in
(select * from dbo.Fun_SplitStr((select value from Ts_Config where [key] = 'MapingWorkShop'),'#'))");
            if (workshop.Rows.Count == 0)
            {
                return true;
            }
            if ("1007".Equals(workshop.Rows[0]["WORKSHOP_CODE"]))//收发室 1:主路线  5:内衬
            {
                if (txtRouMapName.Text == "主路线")
                {
                    strsql = "select *,roumapName= (select name from Tm_roumap where DocEntry = roumap) from Doc_WorkshopGR where NcMappingStatus ='N' AND STEPCode =(select value from Ts_Config where [KEY]='FMappinNC') AND MappingCode ='" + mappingcode + "' AND roumap ='5'";
                }
                else if (txtRouMapName.Text == "内衬路线")
                {
                    strsql = "select *,roumapName= (select name from Tm_roumap where DocEntry = roumap) from Doc_WorkshopGR where NcMappingStatus ='N' AND STEPCode =(select value from Ts_Config where [KEY]='FMappingMain') AND MappingCode ='" + mappingcode + "' AND roumap ='1'";
                }
                else
                {
                    return false;
                }
            }
            else if ("1005".Equals(workshop.Rows[0]["WORKSHOP_CODE"]))//完成线
            {
                if (txtRouMapName.Text == "主路线")
                {
                    strsql = @"SELECT *,roumapName= (select name from Tm_roumap where DocEntry = roumap) FROM Doc_WorkshopGR 
                               WHERE MappingCode ='" + mappingcode + @"' 
                               AND 
                               ((MmMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMM') AND roumap = 2) 
                                OR 
                               (HdMappingStatus ='N' AND STEPCode in(select value from Ts_Config where [KEY]='SMappingHD') AND roumap = 7))";
                }
                else if (txtRouMapName.Text == "后带路线" || txtRouMapName.Text == "帽眉路线")
                {
                    strsql = @"SELECT *,roumapName= (select name from Tm_roumap where DocEntry = roumap) FROM Doc_WorkshopGR 
                               WHERE MappingCode ='" + mappingcode + @"' 
                               AND roumap = 1 AND STEPCode in(select value from Ts_Config where [KEY]='SMappingMain') 
                               AND (MmMappingStatus ='N' OR HdMappingStatus ='N')";
                }
                else
                {
                    return false;
                }
            }
            DataTable dt = FunPublic.GetDt(strsql);
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            #region 关闭扫描
            RFID_15693.freeMode();
            CommonClass.SoftDecoding_Deinit();
            #endregion

            FrmMapping FrmMapping = new FrmMapping();
            FrmMapping.txtRFID1.Text = rfid;
            FrmMapping.txtRoumap1.Text = txtRouMapName.Text;
            FrmMapping.txtMappingCode.Text = mappingcode;
            FrmMapping.txtEBNum.Text = txtEBNum.Text;
            FrmMapping.txtPackageCode.Text = txtPackageCode.Text;
            FrmMapping.txtSN.Text = txtSN.Text;
            FrmMapping.txtQuantity.Text = txtQuantity.Text;
            FrmMapping.Text = "配货 -" + dt.Rows[0]["roumapName"].ToString();
            FrmMapping.model = "弹窗";
            FrmMapping.ShowDialog();

            #region 开启扫描
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹

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
            #endregion
            return true;
        }
        //回收卡(暂停使用)
        private void RecRFID()
        {
            DataTable dt2 = FunPublic.GetDt("exec [PDA_RecRFIDQuery] '" + txtRFID.Text + "'");
            if (dt2.Rows.Count > 0 && dt2.Rows[0]["Status"] == "是" && (dt2.Rows[0]["name"].ToString().Contains("帽眉") || dt2.Rows[0]["name"].ToString().Contains("后带")))
            {
                //回收卡
                List<string> strSqls = new List<string>();
                strSqls.Add(@"
                declare @MaxDoc as int
                select @MaxDoc = isnull(max(DocEntry) + 1,1) from Doc_recRFID
                INSERT INTO [Doc_recRFID]([DocEntry],[CREATED],[DOC_DATE],[EBDOC],[SN],[PackageCode],[RFID_ID],[roumap])
                     VALUES(@MaxDoc,'" + FunPublic.CurrentUser + "',getdate(),'" + txtEBNum.Text + "','" + txtSN.Text + "','" + txtPackageCode.Text + "','" + txtRFID.Text + "','" + dt2.Rows[0]["roumap"] + "')");
                strSqls.Add("update Tm_RFID set STATUS = 'O' where RFIDLUN = '" + txtRFID.Text + "'");
                result = FunPublic.RunSqls(strSqls);
                if (result.Status != 1)
                {
                    throw new Exception("回收卡出错:" + result.Message);
                }
            }
        }

    }
}