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
    public partial class FrmStopPDA : Form
    {
        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();
        string Type = ""; //类型
        string BaseEntry = ""; //13表DocEntry

        public FrmStopPDA()
        {
            InitializeComponent();
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹
        }

        private void FrmStopPDA_Load(object sender, EventArgs e)
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
            string sql;
            //判断是半成品还是产成品
            if (Type.Equals("S") || Type.Equals("RS"))//半成品
            {
                sql = "SELECT DocEntry,FACTORY_CODE,STATUS,CREATED,DOC_DATE,PONUM,EBDOC,BaseEntry,PRODOC FROM Doc_SeGoodspackage WHERE DocEntry ='" + BaseEntry + "'";
            }
            else if (Type.Equals("P") || Type.Equals("RP"))//产成品
            {
                sql = "SELECT DocEntry,FACTORY_CODE,STATUS,CREATED,DOC_DATE,PONUM,EBDOC,BaseEntry,PRODOC FROM Doc_Prodcutpackage WHERE DocEntry ='" + BaseEntry + "'";
            }
            else
            {
                return;
            }

            List<string> strSqls = new List<string>();

            DataTable dt = FunPublic.GetDt(sql);
            strSqls.Add(@"declare @maxDoc as int
select @maxDoc=isnull(MAX(DocEntry) +1,1) from [Doc_Stop]
INSERT INTO [Doc_Stop]
           ([DocEntry]
           ,[FACTORY_CODE]
           ,[CREATED]
           ,[DOC_DATE]
           ,[PONUM]
           ,[EBDOC]
           ,[BaseEntry]
           ,[Type]
           ,[PRODOC])
     VALUES(@maxDoc,'" + dt.Rows[0]["FACTORY_CODE"].ToString() + "','" + FunPublic.CurrentUser + "',getdate(),'" + dt.Rows[0]["PONUM"].ToString() + "','" + txtEBNum.Text + "','" + dt.Rows[0]["DocEntry"].ToString() + "','" + Type + "','" + dt.Rows[0]["PRODOC"].ToString() + "')");
            //包号
            sql = "SELECT DISTINCT NULL AS LINE_ID,RFID_ID,PackageCode,PackagesQty,QtyinPackage,SN,CSKU_CODE,CSKU_NAME,roumap,QtyofSize FROM Doc_Prodcutpackage13 WHERE RFID_ID='" + txtRFID.Text + "' and Type='" + Type + "' and DocEntry ='" + BaseEntry + "'";
            DataTable list1 = FunPublic.GetDt(sql);
            strSqls.Add(@"declare @maxDoc as int
select @maxDoc=isnull(MAX(DocEntry),1) from [Doc_Stop]
INSERT INTO [Doc_Stop1]
           ([DocEntry]
           ,[LINE_ID]
           ,[BaseLine]
           ,[SN]
           ,[CSKU_CODE]
           ,[CSKU_NAME]
           ,[TotalQty]
           ,[RFID_ID]
           ,[PackageCode]
           ,[QtyinPackage],roumap)
     VALUES(@maxDoc,1,'','" + list1.Rows[0]["SN"].ToString() + "','" + list1.Rows[0]["CSKU_CODE"].ToString() + "','" + list1.Rows[0]["CSKU_NAME"].ToString() + "',null,'" + txtRFID.Text + "','" + list1.Rows[0]["PackageCode"].ToString() + "'," + list1.Rows[0]["QtyinPackage"].ToString() + ",'" + list1.Rows[0]["roumap"].ToString() + "')");
            //工序
            sql = "SELECT DISTINCT NULL AS LINE_ID,SN,CSKU_CODE,PackageCode,STEPCode,STEPName,Quantity,WORKSHOP_CODE,STEPSEQ AS SNofStep,STEP_PRICE FROM Doc_Prodcutpackage13  WHERE RFID_ID='" + txtRFID.Text + "' and Type='" + Type + "' and DocEntry ='" + BaseEntry + "'";
            DataTable list2 = FunPublic.GetDt(sql);
            for (int i = 0; i < list2.Rows.Count; i++)
            {
                strSqls.Add(@"declare @maxDoc as int
select @maxDoc=isnull(MAX(DocEntry),1) from [Doc_Stop]
INSERT INTO [Doc_Stop2]
               ([DocEntry]
               ,[LINE_ID]
               ,[SN]
               ,[CSKU_CODE]
               ,[RFID_ID]
               ,[PackageCode]
               ,[STEPCode]
               ,[STEPName]
               ,[STEP_PRICE]
               ,[Quantity]
               ,[SNofStep]
               ,[WORKSHOP_CODE])
         VALUES(@maxDoc," + (i + 1).ToString() + ",'" + list2.Rows[i]["SN"].ToString() + "','" + list2.Rows[i]["CSKU_CODE"].ToString() + "','" + txtRFID.Text + "','" + list2.Rows[i]["PackageCode"].ToString() + "','" + list2.Rows[i]["STEPCode"].ToString() + "','" + list2.Rows[i]["STEPName"].ToString() + "'," + list2.Rows[i]["STEP_PRICE"].ToString() + "," + list2.Rows[i]["Quantity"].ToString() + ",'" + list2.Rows[i]["SNofStep"].ToString() + "','" + list2.Rows[i]["WORKSHOP_CODE"].ToString() + "')");
            }
            //删除13表中数据
            strSqls.Add("DELETE Doc_Prodcutpackage13 WHERE RFID_ID='" + txtRFID.Text + "'");
            //更新14表中的状态
            strSqls.Add("UPDATE Doc_Prodcutpackage14 SET STATUS ='C' WHERE RFID_ID='" + txtRFID.Text + "' and Type='" + Type + "' and DocEntry ='" + BaseEntry + "' and isnull(STATUS,'T') = 'T'");
            //更新RFID卡的状态为O-待分配
            strSqls.Add("update Tm_RFID set STATUS = 'O' where RFIDLUN = '" + txtRFID.Text + "'");

            result = FunPublic.RunSqls(strSqls);
            if (result.Status == 1)
            {
                //MessageBox.Show("提交成功！");
                txtRFID.Text = "";
                //cmbCardCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtCSKU_CODE.Text = "";
                txtQuantity.Text = "";
                txtWORKSHOP_CODE.Text = "";
                txtLastStep.Text = "";
                txtNextStep.Text = "";
                Type = "";
                BaseEntry = "";
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
                    //cmbCardCode.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtCSKU_CODE.Text = "";
                    txtQuantity.Text = "";
                    txtWORKSHOP_CODE.Text = "";
                    txtLastStep.Text = "";
                    txtNextStep.Text = "";
                    Type = "";
                    BaseEntry = "";
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
                            //查询13表
                            string sql = @"SELECT  DocEntry,Doc1_LINE_ID,Doc11_LINE_ID,Doc12_LINE_ID,EBDOC,RFID_ID,MappingCode,PackageCode,PackagesQty,QtyinPackage,ProRtEntry,ProRtLine,SN,CSKU_CODE,CSKU_NAME,STEPCode,STEPName,Quantity,WORKSHOP_CODE,WORKSHOPSEQ,STEPSEQ,roumap,QtyofSize,Type,STEP_PRICE
FROM Doc_Prodcutpackage13 T WHERE T.RFID_ID='" + RFID_ID + "' order by STEPSEQ";
                            DataTable dt = FunPublic.GetDt(sql);

                            if (dt.Rows.Count > 0)
                            {
                                Type = dt.Rows[0]["Type"].ToString();
                                BaseEntry = dt.Rows[0]["DocEntry"].ToString();

                                txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                                txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                                txtCSKU_CODE.Text = dt.Rows[0]["CSKU_CODE"].ToString();
                                txtQuantity.Text = dt.Rows[0]["QtyinPackage"].ToString();
                                txtWORKSHOP_CODE.Text = dt.Rows[0]["WORKSHOP_CODE"].ToString();
                                txtLastStep.Text = dt.Rows[0]["STEPCode"].ToString();
                                DataTable dt1 = FunPublic.GetDt("select STEPCode from Doc_Prodcutpackage14 where RFID_ID='" + RFID_ID + "' and EBDOC = '" + dt.Rows[0]["EBDOC"].ToString() + "' and SN = '" + dt.Rows[0]["SN"].ToString() + "' order by STEPSEQ desc");
                                if (dt1.Rows.Count > 0)
                                {
                                    txtNextStep.Text = dt1.Rows[0]["STEPCode"].ToString();
                                }
                            }
                            else
                            {
                                txtEBNum.Text = "";
                                txtPackageCode.Text = "";
                                txtCSKU_CODE.Text = "";
                                txtQuantity.Text = "";
                                txtWORKSHOP_CODE.Text = "";
                                txtLastStep.Text = "";
                                txtNextStep.Text = "";
                                Type = "";
                                BaseEntry = "";
                                MessageBox.Show("查不到明细！");
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
                        txtRFID.Text = "";
                        //cmbCardCode.Text = "";
                        txtEBNum.Text = "";
                        txtPackageCode.Text = "";
                        txtCSKU_CODE.Text = "";
                        txtQuantity.Text = "";
                        txtWORKSHOP_CODE.Text = "";
                        txtLastStep.Text = "";
                        txtNextStep.Text = "";
                        Type = "";
                        BaseEntry = "";
                        MessageBox.Show("刷卡失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                txtRFID.Text = "";
                //cmbCardCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtCSKU_CODE.Text = "";
                txtQuantity.Text = "";
                txtWORKSHOP_CODE.Text = "";
                txtLastStep.Text = "";
                txtNextStep.Text = "";
                Type = "";
                BaseEntry = "";
                MessageBox.Show("刷卡失败！" + ex.Message);
            }

        }

        private void FrmStopPDA_KeyDown(object sender, KeyEventArgs e)
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
                    //cmbCardCode.Text = "";
                    txtEBNum.Text = "";
                    txtPackageCode.Text = "";
                    txtCSKU_CODE.Text = "";
                    txtQuantity.Text = "";
                    txtWORKSHOP_CODE.Text = "";
                    txtLastStep.Text = "";
                    txtNextStep.Text = "";
                    Type = "";
                    BaseEntry = "";
                    MessageBox.Show("扫描失败！");
                }
                else
                {
                    if (RFID_ID.Substring(0, 1) == "2")
                    {//物料卡
                        txtRFID.Text = RFID_ID;
                        //查询13表
                        string sql = @"SELECT  DocEntry,Doc1_LINE_ID,Doc11_LINE_ID,Doc12_LINE_ID,EBDOC,RFID_ID,MappingCode,PackageCode,PackagesQty,QtyinPackage,ProRtEntry,ProRtLine,SN,CSKU_CODE,CSKU_NAME,STEPCode,STEPName,Quantity,WORKSHOP_CODE,WORKSHOPSEQ,STEPSEQ,roumap,QtyofSize,Type,STEP_PRICE
FROM Doc_Prodcutpackage13 T WHERE T.RFID_ID='" + RFID_ID + "' order by STEPSEQ";
                        DataTable dt = FunPublic.GetDt(sql);

                        if (dt.Rows.Count > 0)
                        {
                            Type = dt.Rows[0]["Type"].ToString();
                            BaseEntry = dt.Rows[0]["DocEntry"].ToString();

                            txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                            txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                            txtCSKU_CODE.Text = dt.Rows[0]["CSKU_CODE"].ToString();
                            txtQuantity.Text = dt.Rows[0]["QtyinPackage"].ToString();
                            txtWORKSHOP_CODE.Text = dt.Rows[0]["WORKSHOP_CODE"].ToString();
                            txtLastStep.Text = dt.Rows[0]["STEPCode"].ToString();
                            DataTable dt1 = FunPublic.GetDt("select STEPCode from Doc_Prodcutpackage14 where RFID_ID='" + RFID_ID + "' and EBDOC = '" + dt.Rows[0]["EBDOC"].ToString() + "' and SN = '" + dt.Rows[0]["SN"].ToString() + "' order by STEPSEQ desc");
                            if (dt1.Rows.Count > 0)
                            {
                                txtNextStep.Text = dt1.Rows[0]["STEPCode"].ToString();
                            }
                        }
                        else
                        {
                            txtEBNum.Text = "";
                            txtPackageCode.Text = "";
                            txtCSKU_CODE.Text = "";
                            txtQuantity.Text = "";
                            txtWORKSHOP_CODE.Text = "";
                            txtLastStep.Text = "";
                            txtNextStep.Text = "";
                            Type = "";
                            BaseEntry = "";
                            MessageBox.Show("查不到明细！");
                            return;
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
                txtRFID.Text = "";
                //cmbCardCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtCSKU_CODE.Text = "";
                txtQuantity.Text = "";
                txtWORKSHOP_CODE.Text = "";
                txtLastStep.Text = "";
                txtNextStep.Text = "";
                Type = "";
                BaseEntry = "";
                MessageBox.Show("扫描失败！" + ex.Message);
            }

        }

        private void FrmStopPDA_Closed(object sender, EventArgs e)
        {
            RFID_15693.freeMode();
            CommonClass.SoftDecoding_Deinit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRFID.Text = "";
            txtEBNum.Text = "";
            txtPackageCode.Text = "";
            txtCSKU_CODE.Text = "";
            txtQuantity.Text = "";
            txtWORKSHOP_CODE.Text = "";
            txtLastStep.Text = "";
            txtNextStep.Text = "";
            Type = "";
            BaseEntry = "";
        }

    }
}