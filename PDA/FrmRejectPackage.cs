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
    public partial class FrmRejectPackage : Form
    {
        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();

        public FrmRejectPackage()
        {
            InitializeComponent();
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹
        }

        private void FrmRejectPackage_Load(object sender, EventArgs e)
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


            DataTable dt = FunPublic.GetDt("exec [dbo].[PDA_RejectPackageQuery] 'RejectReas','','',''");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    cmbReason.Items.Add(dr["Code"].ToString() + "-" + dr["Name"].ToString());
                }
            }

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

                if (txtRFID_New.Focused == true)
                {//刷新卡
                    if (RFID_ID == "")
                    {
                        txtRFID_New.Text = "";
                        MessageBox.Show("刷卡失败！");
                    }
                    else
                    {
                        int res = RFID_15693.RF_ISO15693_getSystemInformation(0, data, 0, pszData);

                        if (res == 0)
                        {
                            if (BitConverter.ToString(pszData, 11, 1) == "0B")
                            {//物料卡
                                //txtRFID_New.Text = RFID_ID;

                                DataTable dt = FunPublic.GetDt("select * from Tm_RFID WHERE RFIDLUN = '" + RFID_ID + "' and STATUS = 'O'");
                                if (dt.Rows.Count == 0)
                                {
                                    MessageBox.Show("物料卡" + RFID_ID + "状态错误，请换一张卡！", "提示");
                                    return;
                                }

                                string DocEntry = FunPublic.GetDt("select isnull(max(DocEntry)+1,1) DocEntry from Doc_RejectPackage").Rows[0][0].ToString();

                                DataTable dt1 = new DataTable();
                                if ("B".Equals(cmbType.Text.Substring(0, 1)))
                                {
                                    dt1 = FunPublic.GetDt("select * from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "'");
                                }
                                else if ("A".Equals(cmbType.Text.Substring(0, 1)))
                                {
                                    dt1 = FunPublic.GetDt("select * from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + @"' and STEPSEQ >= (
select STEPSEQ from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + @"' and STEPCode = '" + cmbRejectStationf.Text.Substring(0, cmbRejectStationf.Text.IndexOf('-')) + "')");
                                }

                                List<string> sqlstrs = new List<string>();
                                sqlstrs.Add(@"INSERT INTO [Doc_RejectPackage]
           ([DocEntry],[DOC_DATE],[CREATED],[RFID_ID],[PackageCode],[CSKU_CODE],[CSKU_NAME],[QtyofReject],[RejectStation],[free],[RejectStationf],[Type],[RejectStaffCo],[RejectReson],[FACTORY_CODE],[EBDOC],[BaseEntry],[RFID_ID1],[STATUS]) VALUES(" + DocEntry + ",getdate(),'" + FunPublic.CurrentUser + "','" + txtRFID.Text + "','" + txtPackageCode.Text + "','" + txtCSKU_CODE.Text + "',''," + txtQuantity.Text + ",'" + cmbRejectStation.Text.Substring(0, cmbRejectStation.Text.IndexOf('-')) + "','" + cmbfree.Text.Substring(0, cmbfree.Text.IndexOf('-')) + "','" + cmbRejectStationf.Text.Substring(0, cmbRejectStationf.Text.IndexOf('-')) + "','" + cmbType.Text.Substring(0, cmbType.Text.IndexOf('-')) + "','','" + cmbReason.Text.Substring(0, cmbReason.Text.IndexOf('-')) + "',(select FACTORY_CODE from Tm_Station where ReaderCode = '" + FunPublic.GetDeviceName() + "'),'" + txtEBNum.Text + "',null,'" + RFID_ID + "','O')");

                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    ListViewItem ListViewItem = listView1.Items[i];

                                    sqlstrs.Add(@"INSERT INTO [Doc_RejectPackage1]([DocEntry],[LINE_ID],[STEPCode],[STEPName],[STEP_PRICE],[STEP_PRICE_H],[Quantity],[WORKSHOP_CODE],[STEPSEQ],[IsFinish]) VALUES(" + DocEntry + "," + i + ",'" + ListViewItem.SubItems[2].Text + "','" + ListViewItem.SubItems[3].Text + "','" + ListViewItem.SubItems[7].Text + "','" + ListViewItem.SubItems[8].Text + "','" + ListViewItem.SubItems[4].Text + "','" + ListViewItem.SubItems[5].Text + "','" + ListViewItem.SubItems[6].Text + "','" + ListViewItem.SubItems[9].Text + "')");

                                    sqlstrs.Add(@"INSERT INTO [Doc_Prodcutpackage13]
                                                   ([DocEntry]
                                                   ,[Doc1_LINE_ID]
                                                   ,[Doc11_LINE_ID]
                                                   ,[Doc12_LINE_ID]
                                                   ,[RFID_ID]
                                                   ,[MappingCode]
                                                   ,[PackageCode]
                                                   ,[PackagesQty]
                                                   ,[QtyinPackage]
                                                   ,[ProRtEntry]
                                                   ,[ProRtLine]
                                                   ,[SN]
                                                   ,[CSKU_CODE]
                                                   ,[CSKU_NAME]
                                                   ,[STEPCode]
                                                   ,[STEPName]
                                                   ,[Quantity]
                                                   ,[WORKSHOP_CODE]
                                                   ,[WORKSHOPSEQ]
                                                   ,[STEPSEQ]
                                                   ,[roumap]
                                                   ,[EBDOC]
                                                   ,[QtyofSize],STEP_PRICE,Type,PLEntry,FACTORY_CODE,MODEL,C_COLOUR,B_COLOUR,QtyofSizePackage,ISFREESTEP,ISOPTION,KEYVALUE,EXPLAIN1,EXPLAIN)
                 VALUES('" + DocEntry + "','" + (i + 1) + "','','','" + RFID_ID + "','" + dt1.Rows[i]["MappingCode"].ToString() + "','" + dt1.Rows[i]["PackageCode"].ToString() + "'," + dt1.Rows[i]["PackagesQty"].ToString() + "," + dt1.Rows[i]["QtyinPackage"].ToString() + ",'" + dt1.Rows[i]["ProRtEntry"].ToString() + "','" + dt1.Rows[i]["ProRtLine"].ToString() + "'," + dt1.Rows[i]["SN"].ToString() + ",'" + dt1.Rows[i]["CSKU_CODE"].ToString() + "','" + dt1.Rows[i]["CSKU_NAME"].ToString() + "','" + dt1.Rows[i]["STEPCode"].ToString() + "','" + dt1.Rows[i]["STEPName"].ToString() + "'," + txtQuantity.Text + ",'" + dt1.Rows[i]["WORKSHOP_CODE"].ToString() + "'," + dt1.Rows[i]["WORKSHOPSEQ"].ToString() + "," + dt1.Rows[i]["STEPSEQ"].ToString() + ",'" + dt1.Rows[i]["roumap"].ToString() + "','" + dt1.Rows[i]["EBDOC"].ToString() + "','" + dt1.Rows[i]["QtyofSize"].ToString() + "'," + ListViewItem.SubItems[7].Text + ",(case when '" + dt1.Rows[i]["Type"].ToString() + "' = 'P' then 'RP' when '" + dt1.Rows[i]["Type"].ToString() + "' = 'S' then 'RS' end),'" + dt1.Rows[i]["PLEntry"].ToString() + "','" + dt1.Rows[i]["FACTORY_CODE"].ToString() + "','" + dt1.Rows[i]["MODEL"].ToString() + "','" + dt1.Rows[i]["C_COLOUR"].ToString() + "','" + dt1.Rows[i]["B_COLOUR"].ToString() + "','" + dt1.Rows[i]["QtyofSizePackage"].ToString() + "','" + dt1.Rows[i]["ISFREESTEP"].ToString() + "','" + dt1.Rows[i]["ISOPTION"].ToString() + "','" + dt1.Rows[i]["KEYVALUE"].ToString() + "','" + dt1.Rows[i]["EXPLAIN1"].ToString() + "','" + dt1.Rows[i]["EXPLAIN"].ToString() + "')");
                                    sqlstrs.Add(@"INSERT INTO [Doc_Prodcutpackage14]
                                                   ([DocEntry]
                                                   ,[Doc1_LINE_ID]
                                                   ,[Doc11_LINE_ID]
                                                   ,[Doc12_LINE_ID]
                                                   ,[RFID_ID]
                                                   ,[MappingCode]
                                                   ,[PackageCode]
                                                   ,[PackagesQty]
                                                   ,[QtyinPackage]
                                                   ,[ProRtEntry]
                                                   ,[ProRtLine]
                                                   ,[SN]
                                                   ,[CSKU_CODE]
                                                   ,[CSKU_NAME]
                                                   ,[STEPCode]
                                                   ,[STEPName]
                                                   ,[Quantity]
                                                   ,[WORKSHOP_CODE]
                                                   ,[WORKSHOPSEQ]
                                                   ,[STEPSEQ]
                                                   ,[roumap]
                                                   ,[EBDOC]
                                                   ,[QtyofSize],STEP_PRICE,Type,PLEntry,FACTORY_CODE,MODEL,C_COLOUR,B_COLOUR,QtyofSizePackage,ISFREESTEP,ISOPTION,KEYVALUE,EXPLAIN1,EXPLAIN)
                VALUES('" + DocEntry + "','" + (i + 1) + "','','','" + RFID_ID + "','" + dt1.Rows[i]["MappingCode"].ToString() + "','" + dt1.Rows[i]["PackageCode"].ToString() + "'," + dt1.Rows[i]["PackagesQty"].ToString() + "," + dt1.Rows[i]["QtyinPackage"].ToString() + ",'" + dt1.Rows[i]["ProRtEntry"].ToString() + "','" + dt1.Rows[i]["ProRtLine"].ToString() + "'," + dt1.Rows[i]["SN"].ToString() + ",'" + dt1.Rows[i]["CSKU_CODE"].ToString() + "','" + dt1.Rows[i]["CSKU_NAME"].ToString() + "','" + dt1.Rows[i]["STEPCode"].ToString() + "','" + dt1.Rows[i]["STEPName"].ToString() + "'," + txtQuantity.Text + ",'" + dt1.Rows[i]["WORKSHOP_CODE"].ToString() + "'," + dt1.Rows[i]["WORKSHOPSEQ"].ToString() + "," + dt1.Rows[i]["STEPSEQ"].ToString() + ",'" + dt1.Rows[i]["roumap"].ToString() + "','" + dt1.Rows[i]["EBDOC"].ToString() + "','" + dt1.Rows[i]["QtyofSize"].ToString() + "'," + ListViewItem.SubItems[7].Text + ",(case when '" + dt1.Rows[i]["Type"].ToString() + "' = 'P' then 'RP' when '" + dt1.Rows[i]["Type"].ToString() + "' = 'S' then 'RS' end),'" + dt1.Rows[i]["PLEntry"].ToString() + "','" + dt1.Rows[i]["FACTORY_CODE"].ToString() + "','" + dt1.Rows[i]["MODEL"].ToString() + "','" + dt1.Rows[i]["C_COLOUR"].ToString() + "','" + dt1.Rows[i]["B_COLOUR"].ToString() + "','" + dt1.Rows[i]["QtyofSizePackage"].ToString() + "','" + dt1.Rows[i]["ISFREESTEP"].ToString() + "','" + dt1.Rows[i]["ISOPTION"].ToString() + "','" + dt1.Rows[i]["KEYVALUE"].ToString() + "','" + dt1.Rows[i]["EXPLAIN1"].ToString() + "','" + dt1.Rows[i]["EXPLAIN"].ToString() + "')");
                                }
                                //更新RFID卡状态
                                sqlstrs.Add("update Tm_RFID set STATUS = 'F' where RFIDLUN = '" + RFID_ID + "' ");
                                sqlstrs.Add("update dbo.Doc_RejectPackage set RFID_ID1 = '" + RFID_ID + "',STATUS = 'T' where DocEntry = '" + DocEntry + "'");

                                //更新原来卡数量
                                sqlstrs.Add("update Doc_Prodcutpackage13 set QtyinPackage = QtyinPackage - " + txtQuantity.Text + " where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "'");
                                sqlstrs.Add("delete from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "' and QtyinPackage = 0");
                                sqlstrs.Add("update Doc_Prodcutpackage14 set QtyinPackage = QtyinPackage - " + txtQuantity.Text + " where cast(DocEntry as nvarchar)+'#'+cast(Doc1_LINE_ID as nvarchar)+'#'+cast(Doc11_LINE_ID as nvarchar)+'#'+cast(Doc12_LINE_ID as nvarchar)+'#'+Type in (select cast(DocEntry as nvarchar)+'#'+cast(Doc1_LINE_ID as nvarchar)+'#'+cast(Doc11_LINE_ID as nvarchar)+'#'+cast(Doc12_LINE_ID as nvarchar)+'#'+Type from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "')");

                                result = FunPublic.RunSqls(sqlstrs);
                                if (result.Status == 1)
                                {

                                    //lueSTATUS.EditValue = "T";
                                    txtRFID_New.Text = RFID_ID;

                                }
                                else
                                {
                                    MessageBox.Show(result.Message, "提示");
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
                            txtRFID_New.Text = "";
                            MessageBox.Show("刷卡失败！");
                        }
                    }
                }
                else
                {//刷旧卡
                    if (RFID_ID == "")
                    {
                        txtRFID.Text = "";
                        cmbType.Text = "";
                        txtQuantity.Text = "";
                        cmbfree.Text = "";
                        cmbRejectStation.Items.Clear();
                        cmbRejectStation.Text = "";
                        cmbRejectStationf.Items.Clear();
                        cmbRejectStationf.Text = "";
                        cmbReason.Text = "";
                        txtEBNum.Text = "";
                        txtCSKU_CODE.Text = "";
                        txtPackageCode.Text = "";
                        listView1.Items.Clear();
                        MessageBox.Show("刷卡失败！");
                    }
                    else
                    {
                        txtRFID.Text = "";
                        cmbType.Text = "";
                        txtQuantity.Text = "";
                        cmbfree.Text = "";
                        cmbRejectStation.Items.Clear();
                        cmbRejectStation.Text = "";
                        cmbRejectStationf.Items.Clear();
                        cmbRejectStationf.Text = "";
                        cmbReason.Text = "";
                        txtEBNum.Text = "";
                        txtCSKU_CODE.Text = "";
                        txtPackageCode.Text = "";
                        listView1.Items.Clear();

                        int res = RFID_15693.RF_ISO15693_getSystemInformation(0, data, 0, pszData);

                        if (res == 0)
                        {
                            if (BitConverter.ToString(pszData, 11, 1) == "0B")
                            {//物料卡
                                txtRFID.Text = RFID_ID;
                                string DeviceName = FunPublic.GetDeviceName();
                                DataTable dt = FunPublic.GetDt("select top 1 * from Doc_DailyReport where RFID_ID = '" + RFID_ID + "' and DOC_DATE >= DATEADD(YEAR,-1,GETDATE()) order by DOC_DATE desc");
                                if (dt.Rows.Count > 0)
                                {
                                    DataTable dt1 = FunPublic.GetDt("select STEPCode cd,STEPName nm,ID,StaffCode from Doc_DailyReport where RFID_ID = '" + RFID_ID + "' and EBDOC = '" + dt.Rows[0]["EBDOC"].ToString() + "' and SN = '" + dt.Rows[0]["SN"].ToString() + "' and PackageCode = '" + dt.Rows[0]["PackageCode"].ToString() + "'");

                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        cmbRejectStation.Items.Add(dt1.Rows[i]["cd"].ToString() + "-" + dt1.Rows[i]["nm"].ToString());
                                        cmbRejectStationf.Items.Add(dt1.Rows[i]["cd"].ToString() + "-" + dt1.Rows[i]["nm"].ToString());
                                    }

                                    txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                                    txtCSKU_CODE.Text = dt.Rows[0]["CSKU_CODE"].ToString();
                                    txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                                }
                                else
                                {
                                    MessageBox.Show("此包还没有完工工序！", "提示");
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
                            txtRFID.Text = "";
                            cmbType.Text = "";
                            txtQuantity.Text = "";
                            cmbfree.Text = "";
                            cmbRejectStation.Items.Clear();
                            cmbRejectStation.Text = "";
                            cmbRejectStationf.Items.Clear();
                            cmbRejectStationf.Text = "";
                            cmbReason.Text = "";
                            txtEBNum.Text = "";
                            txtCSKU_CODE.Text = "";
                            txtPackageCode.Text = "";
                            listView1.Items.Clear();
                            MessageBox.Show("刷卡失败！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtRFID.Text = "";
                cmbType.Text = "";
                txtQuantity.Text = "";
                cmbfree.Text = "";
                cmbRejectStation.Items.Clear();
                cmbRejectStation.Text = "";
                cmbRejectStationf.Items.Clear();
                cmbRejectStationf.Text = "";
                cmbReason.Text = "";
                txtEBNum.Text = "";
                txtCSKU_CODE.Text = "";
                txtPackageCode.Text = "";
                listView1.Items.Clear();
                MessageBox.Show("刷卡失败！" + ex.Message);
            }

        }

        private void FrmRejectPackage_KeyDown(object sender, KeyEventArgs e)
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

                if (txtRFID_New.Focused == true)
                {//刷新卡
                    if (RFID_ID == "")
                    {
                        txtRFID_New.Text = "";
                        MessageBox.Show("扫描失败！");
                    }
                    else
                    {
                        if (RFID_ID.Substring(0, 1) == "2")
                        {//物料卡
                            //txtRFID_New.Text = RFID_ID;

                            DataTable dt = FunPublic.GetDt("select * from Tm_RFID WHERE RFIDLUN = '" + RFID_ID + "' and STATUS = 'O'");
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("物料卡" + RFID_ID + "状态错误，请换一张卡！", "提示");
                                return;
                            }

                            string DocEntry = FunPublic.GetDt("select isnull(max(DocEntry)+1,1) DocEntry from Doc_RejectPackage").Rows[0][0].ToString();

                            DataTable dt1 = new DataTable();
                            if ("B".Equals(cmbType.Text.Substring(0, 1)))
                            {
                                dt1 = FunPublic.GetDt("select * from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "'");
                            }
                            else if ("A".Equals(cmbType.Text.Substring(0, 1)))
                            {
                                dt1 = FunPublic.GetDt("select * from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + @"' and STEPSEQ >= (
select STEPSEQ from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + @"' and STEPCode = '" + cmbRejectStationf.Text.Substring(0, cmbRejectStationf.Text.IndexOf('-')) + "')");
                            }

                            List<string> sqlstrs = new List<string>();
                            sqlstrs.Add(@"INSERT INTO [Doc_RejectPackage]
           ([DocEntry],[DOC_DATE],[CREATED],[RFID_ID],[PackageCode],[CSKU_CODE],[CSKU_NAME],[QtyofReject],[RejectStation],[free],[RejectStationf],[Type],[RejectStaffCo],[RejectReson],[FACTORY_CODE],[EBDOC],[BaseEntry],[RFID_ID1],[STATUS]) VALUES(" + DocEntry + ",getdate(),'" + FunPublic.CurrentUser + "','" + txtRFID.Text + "','" + txtPackageCode.Text + "','" + txtCSKU_CODE.Text + "',''," + txtQuantity.Text + ",'" + cmbRejectStation.Text.Substring(0, cmbRejectStation.Text.IndexOf('-')) + "','" + cmbfree.Text.Substring(0, cmbfree.Text.IndexOf('-')) + "','" + cmbRejectStationf.Text.Substring(0, cmbRejectStationf.Text.IndexOf('-')) + "','" + cmbType.Text.Substring(0, cmbType.Text.IndexOf('-')) + "','','" + cmbReason.Text.Substring(0, cmbReason.Text.IndexOf('-')) + "',(select FACTORY_CODE from Tm_Station where ReaderCode = '" + FunPublic.GetDeviceName() + "'),'" + txtEBNum.Text + "',null,'" + RFID_ID + "','O')");

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                ListViewItem ListViewItem = listView1.Items[i];

                                sqlstrs.Add(@"INSERT INTO [Doc_RejectPackage1]([DocEntry],[LINE_ID],[STEPCode],[STEPName],[STEP_PRICE],[STEP_PRICE_H],[Quantity],[WORKSHOP_CODE],[STEPSEQ],[IsFinish]) VALUES(" + DocEntry + "," + i + ",'" + ListViewItem.SubItems[2].Text + "','" + ListViewItem.SubItems[3].Text + "','" + ListViewItem.SubItems[7].Text + "','" + ListViewItem.SubItems[8].Text + "','" + ListViewItem.SubItems[4].Text + "','" + ListViewItem.SubItems[5].Text + "','" + ListViewItem.SubItems[6].Text + "','" + ListViewItem.SubItems[9].Text + "')");

                                sqlstrs.Add(@"INSERT INTO [Doc_Prodcutpackage13]
                                                   ([DocEntry]
                                                   ,[Doc1_LINE_ID]
                                                   ,[Doc11_LINE_ID]
                                                   ,[Doc12_LINE_ID]
                                                   ,[RFID_ID]
                                                   ,[MappingCode]
                                                   ,[PackageCode]
                                                   ,[PackagesQty]
                                                   ,[QtyinPackage]
                                                   ,[ProRtEntry]
                                                   ,[ProRtLine]
                                                   ,[SN]
                                                   ,[CSKU_CODE]
                                                   ,[CSKU_NAME]
                                                   ,[STEPCode]
                                                   ,[STEPName]
                                                   ,[Quantity]
                                                   ,[WORKSHOP_CODE]
                                                   ,[WORKSHOPSEQ]
                                                   ,[STEPSEQ]
                                                   ,[roumap]
                                                   ,[EBDOC]
                                                   ,[QtyofSize],STEP_PRICE,Type,PLEntry,FACTORY_CODE)
                 VALUES('" + DocEntry + "','" + (i + 1) + "','','','" + RFID_ID + "','" + dt1.Rows[i]["MappingCode"].ToString() + "','" + dt1.Rows[i]["PackageCode"].ToString() + "'," + dt1.Rows[i]["PackagesQty"].ToString() + "," + dt1.Rows[i]["QtyinPackage"].ToString() + ",'" + dt1.Rows[i]["ProRtEntry"].ToString() + "','" + dt1.Rows[i]["ProRtLine"].ToString() + "'," + dt1.Rows[i]["SN"].ToString() + ",'" + dt1.Rows[i]["CSKU_CODE"].ToString() + "','" + dt1.Rows[i]["CSKU_NAME"].ToString() + "','" + dt1.Rows[i]["STEPCode"].ToString() + "','" + dt1.Rows[i]["STEPName"].ToString() + "'," + txtQuantity.Text + ",'" + dt1.Rows[i]["WORKSHOP_CODE"].ToString() + "'," + dt1.Rows[i]["WORKSHOPSEQ"].ToString() + "," + dt1.Rows[i]["STEPSEQ"].ToString() + ",'" + dt1.Rows[i]["roumap"].ToString() + "','" + dt1.Rows[i]["EBDOC"].ToString() + "','" + dt1.Rows[i]["QtyofSize"].ToString() + "'," + ListViewItem.SubItems[7].Text + ",(case when '" + dt1.Rows[i]["Type"].ToString() + "' = 'P' then 'RP' when '" + dt1.Rows[i]["Type"].ToString() + "' = 'S' then 'RS' end),'" + dt1.Rows[i]["PLEntry"].ToString() + "','" + dt1.Rows[i]["FACTORY_CODE"].ToString() + "')");
                                sqlstrs.Add(@"INSERT INTO [Doc_Prodcutpackage14]
                                                   ([DocEntry]
                                                   ,[Doc1_LINE_ID]
                                                   ,[Doc11_LINE_ID]
                                                   ,[Doc12_LINE_ID]
                                                   ,[RFID_ID]
                                                   ,[MappingCode]
                                                   ,[PackageCode]
                                                   ,[PackagesQty]
                                                   ,[QtyinPackage]
                                                   ,[ProRtEntry]
                                                   ,[ProRtLine]
                                                   ,[SN]
                                                   ,[CSKU_CODE]
                                                   ,[CSKU_NAME]
                                                   ,[STEPCode]
                                                   ,[STEPName]
                                                   ,[Quantity]
                                                   ,[WORKSHOP_CODE]
                                                   ,[WORKSHOPSEQ]
                                                   ,[STEPSEQ]
                                                   ,[roumap]
                                                   ,[EBDOC]
                                                   ,[QtyofSize],STEP_PRICE,Type,PLEntry,FACTORY_CODE)
                VALUES('" + DocEntry + "','" + (i + 1) + "','','','" + RFID_ID + "','" + dt1.Rows[i]["MappingCode"].ToString() + "','" + dt1.Rows[i]["PackageCode"].ToString() + "'," + dt1.Rows[i]["PackagesQty"].ToString() + "," + dt1.Rows[i]["QtyinPackage"].ToString() + ",'" + dt1.Rows[i]["ProRtEntry"].ToString() + "','" + dt1.Rows[i]["ProRtLine"].ToString() + "'," + dt1.Rows[i]["SN"].ToString() + ",'" + dt1.Rows[i]["CSKU_CODE"].ToString() + "','" + dt1.Rows[i]["CSKU_NAME"].ToString() + "','" + dt1.Rows[i]["STEPCode"].ToString() + "','" + dt1.Rows[i]["STEPName"].ToString() + "'," + txtQuantity.Text + ",'" + dt1.Rows[i]["WORKSHOP_CODE"].ToString() + "'," + dt1.Rows[i]["WORKSHOPSEQ"].ToString() + "," + dt1.Rows[i]["STEPSEQ"].ToString() + ",'" + dt1.Rows[i]["roumap"].ToString() + "','" + dt1.Rows[i]["EBDOC"].ToString() + "','" + dt1.Rows[i]["QtyofSize"].ToString() + "'," + ListViewItem.SubItems[7].Text + ",(case when '" + dt1.Rows[i]["Type"].ToString() + "' = 'P' then 'RP' when '" + dt1.Rows[i]["Type"].ToString() + "' = 'S' then 'RS' end),'" + dt1.Rows[i]["PLEntry"].ToString() + "','" + dt1.Rows[i]["FACTORY_CODE"].ToString() + "')");
                            }
                            //更新RFID卡状态
                            sqlstrs.Add("update Tm_RFID set STATUS = 'F' where RFIDLUN = '" + RFID_ID + "' ");
                            sqlstrs.Add("update dbo.Doc_RejectPackage set RFID_ID1 = '" + RFID_ID + "',STATUS = 'T' where DocEntry = '" + DocEntry + "'");

                            //更新原来卡数量
                            sqlstrs.Add("update Doc_Prodcutpackage13 set QtyinPackage = QtyinPackage - " + txtQuantity.Text + " where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "'");
                            sqlstrs.Add("delete from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "' and QtyinPackage = 0");
                            sqlstrs.Add("update Doc_Prodcutpackage14 set QtyinPackage = QtyinPackage - " + txtQuantity.Text + " where cast(DocEntry as nvarchar)+'#'+cast(Doc1_LINE_ID as nvarchar)+'#'+cast(Doc11_LINE_ID as nvarchar)+'#'+cast(Doc12_LINE_ID as nvarchar)+'#'+Type in (select cast(DocEntry as nvarchar)+'#'+cast(Doc1_LINE_ID as nvarchar)+'#'+cast(Doc11_LINE_ID as nvarchar)+'#'+cast(Doc12_LINE_ID as nvarchar)+'#'+Type from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "')");

                            result = FunPublic.RunSqls(sqlstrs);
                            if (result.Status == 1)
                            {

                                //lueSTATUS.EditValue = "T";
                                txtRFID_New.Text = RFID_ID;

                            }
                            else
                            {
                                MessageBox.Show(result.Message, "提示");
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
                else
                {//刷旧卡
                    if (RFID_ID == "")
                    {
                        txtRFID.Text = "";
                        cmbType.Text = "";
                        txtQuantity.Text = "";
                        cmbfree.Text = "";
                        cmbRejectStation.Items.Clear();
                        cmbRejectStation.Text = "";
                        cmbRejectStationf.Items.Clear();
                        cmbRejectStationf.Text = "";
                        cmbReason.Text = "";
                        txtEBNum.Text = "";
                        txtCSKU_CODE.Text = "";
                        txtPackageCode.Text = "";
                        listView1.Items.Clear();
                        MessageBox.Show("扫描失败！");
                    }
                    else
                    {
                        txtRFID.Text = "";
                        cmbType.Text = "";
                        txtQuantity.Text = "";
                        cmbfree.Text = "";
                        cmbRejectStation.Items.Clear();
                        cmbRejectStation.Text = "";
                        cmbRejectStationf.Items.Clear();
                        cmbRejectStationf.Text = "";
                        cmbReason.Text = "";
                        txtEBNum.Text = "";
                        txtCSKU_CODE.Text = "";
                        txtPackageCode.Text = "";
                        listView1.Items.Clear();

                        if (RFID_ID.Substring(0, 1) == "2")
                        {//物料卡
                            txtRFID.Text = RFID_ID;
                            string DeviceName = FunPublic.GetDeviceName();
                            DataTable dt = FunPublic.GetDt("select top 1 * from Doc_DailyReport where RFID_ID = '" + RFID_ID + "' and DOC_DATE >= DATEADD(YEAR,-1,GETDATE()) order by DOC_DATE desc");
                            if (dt.Rows.Count > 0)
                            {
                                DataTable dt1 = FunPublic.GetDt("select STEPCode cd,STEPName nm,ID,StaffCode from Doc_DailyReport where RFID_ID = '" + RFID_ID + "' and EBDOC = '" + dt.Rows[0]["EBDOC"].ToString() + "' and SN = '" + dt.Rows[0]["SN"].ToString() + "' and PackageCode = '" + dt.Rows[0]["PackageCode"].ToString() + "'");

                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    cmbRejectStation.Items.Add(dt1.Rows[i]["cd"].ToString() + "-" + dt1.Rows[i]["nm"].ToString());
                                    cmbRejectStationf.Items.Add(dt1.Rows[i]["cd"].ToString() + "-" + dt1.Rows[i]["nm"].ToString());
                                }

                                txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                                txtCSKU_CODE.Text = dt.Rows[0]["CSKU_CODE"].ToString();
                                txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("此包还没有完工工序！", "提示");
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
            }
            catch (Exception ex)
            {
                txtRFID.Text = "";
                cmbType.Text = "";
                txtQuantity.Text = "";
                cmbfree.Text = "";
                cmbRejectStation.Items.Clear();
                cmbRejectStation.Text = "";
                cmbRejectStationf.Items.Clear();
                cmbRejectStationf.Text = "";
                cmbReason.Text = "";
                txtEBNum.Text = "";
                txtCSKU_CODE.Text = "";
                txtPackageCode.Text = "";
                listView1.Items.Clear();
                MessageBox.Show("扫描失败！" + ex.Message + System.Environment.NewLine + ex.StackTrace );
            }
        }

        private void FrmRejectPackage_Closed(object sender, EventArgs e)
        {
            RFID_15693.freeMode();
            CommonClass.SoftDecoding_Deinit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRFID.Text = "";
            cmbType.Text = "";
            txtQuantity.Text = "";
            cmbfree.Text = "";
            cmbRejectStation.Items.Clear();
            cmbRejectStation.Text = "";
            cmbRejectStationf.Items.Clear();
            cmbRejectStationf.Text = "";
            cmbReason.Text = "";
            txtEBNum.Text = "";
            txtCSKU_CODE.Text = "";
            txtPackageCode.Text = "";
            txtRFID_New.Text = "";
            listView1.Items.Clear();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string STEP_PRICE = "0";
            if (cmbfree.Text != "" && cmbType.Text != "")
            {
                if ("Y".Equals(cmbfree.Text.Substring(0, 1)))
                {
                    STEP_PRICE = "isnull(STEP_PRICE,0)";
                }
                else if ("N".Equals(cmbfree.Text.Substring(0, 1)))
                {
                    STEP_PRICE = "0";
                }

                if ("B".Equals(cmbType.Text.Substring(0, 1)))
                {
                    DataTable dt = FunPublic.GetDt("select null LINE_ID,STEPCode,STEPName,Quantity,WORKSHOP_CODE,STEPSEQ,case STATUS when 'T' then isnull(STEP_PRICE,0) else " + STEP_PRICE + " end as STEP_PRICE,isnull(STEP_PRICE,0) as STEP_PRICE_H,case STATUS when 'T' then 'N' else 'Y' end IsFinish from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + "' order by STEPSEQ");

                    FunPublic.View_ListView(dt, listView1);

                    cmbRejectStationf.Enabled = false;
                }
                else if ("A".Equals(cmbType.Text.Substring(0, 1)))
                {
                    cmbRejectStationf.Enabled = true;
                    listView1.Items.Clear();
                }
            }

        }

        private void cmbRejectStationf_SelectedIndexChanged(object sender, EventArgs e)
        {
            string STEP_PRICE = "0";
            if (cmbfree.Text != "")
            {
                if ("Y".Equals(cmbfree.Text.Substring(0, 1)))
                {
                    STEP_PRICE = "isnull(STEP_PRICE,0)";
                }
                else if ("N".Equals(cmbfree.Text.Substring(0, 1)))
                {
                    STEP_PRICE = "0";
                }
                DataTable dt = FunPublic.GetDt("select null LINE_ID,STEPCode,STEPName,Quantity,WORKSHOP_CODE,STEPSEQ,case STATUS when 'T' then isnull(STEP_PRICE,0) else " + STEP_PRICE + " end as STEP_PRICE,isnull(STEP_PRICE,0) as STEP_PRICE_H,case STATUS when 'T' then 'N' else 'Y' end IsFinish from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + @"' and STEPSEQ >= (
select STEPSEQ from Doc_Prodcutpackage14 where RFID_ID = '" + txtRFID.Text + "' and EBDOC = '" + txtEBNum.Text + "' and CSKU_CODE = '" + txtCSKU_CODE.Text + @"' and STEPCode = '" + cmbRejectStationf.Text.Substring(0, cmbRejectStationf.Text.IndexOf('-')) + "')  order by STEPSEQ");

                FunPublic.View_ListView(dt, listView1);
            }

        }

        private void cmbfree_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbfree.Text != "")
            {
                if ("Y".Equals(cmbfree.Text.Substring(0, 1)))
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        ListViewItem listItem = listView1.Items[i];
                        listItem.SubItems[7].Text = listItem.SubItems[8].Text;
                    }
                }
                else if ("N".Equals(cmbfree.Text.Substring(0, 1)))
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        ListViewItem listItem = listView1.Items[i];
                        if (listItem.SubItems[9].Text == "Y")
                        {
                            listItem.SubItems[7].Text = "0";
                        }
                        else
                        {
                            listItem.SubItems[7].Text = listItem.SubItems[8].Text;
                        }
                    }
                }
            }

        }

    }
}