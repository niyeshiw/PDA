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
    public partial class FrmOSGoodsIssu : Form
    {
        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();

        public FrmOSGoodsIssu()
        {
            InitializeComponent();
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹
        }

        private void FrmOSGoodsIssu_Load(object sender, EventArgs e)
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


            DataTable dt = FunPublic.GetDt("exec [dbo].[PDA_OSGoodsIssuQuery] 'Vendor','','',''");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    cmbCardCode.Items.Add(dr["outsourcBP"].ToString() + "-" + dr["outsourcBPNM"].ToString());
                }
            }
            ////校正宽度
            //int nWidth = 0, nTemp = 0;
            //Graphics g = cmbCardCode.CreateGraphics();
            //for (int i = 0; i < cmbCardCode.Items.Count; i++)
            //{
            //    nTemp = (int)g.MeasureString(cmbCardCode.Items[i].ToString(), cmbCardCode.Font).Width;
            //    if (nTemp > nWidth)
            //        nWidth = nTemp;
            //}
            //g.Dispose();
            //SendMessage(cmbCardCode.Handle, CB_SETDROPPEDWIDTH, nWidth, 0);

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
            if (cmbCardCode.Text == "")
            {
                MessageBox.Show("请选择供应商！");
                return;
            }
            if (txtRFID.Text == "")
            {
                MessageBox.Show("请刷物料卡！");
                return;
            }
            if (txtEBNum.Text == "")
            {
                MessageBox.Show("此供应商下无此包外发！");
                return;
            }

            string DeviceName = FunPublic.GetDeviceName();
            List<string> strSqls = new List<string>();
            strSqls.Add(@"declare @MaxDoc as int
select @MaxDoc = isnull(MAX(DocEntry),0) + 1 from Doc_OSGoodsIssu

select t1.FACTORY_CODE,t1.PONUM,t1.EBDOC,t1.PRODOC,t0.CSKU_CODE,t0.SN,t0.CSKU_NAME,t0.PackageCode,t0.RFID_ID,t0.QtyinPackage,t0.DocEntry,t0.Doc1_LINE_ID,t0.Doc11_LINE_ID,t0.Doc12_LINE_ID,t0.STEPCode,t0.STEPName,t0.STEPSEQ,t4.EXPLAIN1,t0.MappingCode,t0.C_COLOUR,t0.B_COLOUR,t0.STEP_PRICE,t0.PackagesQty,t0.Type,t0.MODEL 
into #t1
from Doc_Prodcutpackage13 t0 inner join Doc_Prodcutpackage t1 on t0.DocEntry = t1.DocEntry
inner join Doc_Osplanning t2 on t0.PLEntry = t2.BaseEntry inner join Doc_Osplanning1 t3 on t2.DocEntry = t3.DocEntry and t0.CSKU_CODE = t3.CSKU_CODE inner join Doc_Osplanning11 t4 on t3.DocEntry = t4.DocEntry and t3.LINE_ID = t4.Doc1_LINE_ID and t4.STEPCode = t0.STEPCode
where t0.RFID_ID = '" + txtRFID.Text + @"' and t4.outsourcBP = '" + cmbCardCode.Text.Substring(0, cmbCardCode.Text.IndexOf('-')) + @"'
insert into Doc_OSGoodsIssu(DocEntry,DOC_DATE,BPCode,BPName,CREATED,STATUS,FACTORY_CODE) values(@MaxDoc,GETDATE(),'" + cmbCardCode.Text.Substring(0, cmbCardCode.Text.IndexOf('-')) + "','" + cmbCardCode.Text.Substring(cmbCardCode.Text.LastIndexOf("-") + 1) + "'," + FunPublic.CurrentUser + @",'O',(select distinct FACTORY_CODE from #t1))
insert into Doc_OSGoodsIssu1(DocEntry,LINE_ID,PONUM,EBDOC,PRODOC,CSKU_CODE,SN,CSKU_NAME,PackageCode,RFID_ID,Quantity,BaseEntry,BaseDoc1_Line,BaseDoc11_Line,BaseDoc12_Line,Type,STEPCode,STEPName,STEPSEQ,EXPLAIN1,GRQtyp,QtyofReject,QtyofRepair,MappingCode,C_COLOUR,B_COLOUR,STEP_PRICE,PackagesQty,MODEL)
select @MaxDoc,ROW_NUMBER() over(order by SN),PONUM,EBDOC,PRODOC,CSKU_CODE,SN,CSKU_NAME,PackageCode,RFID_ID,QtyinPackage,DocEntry,Doc1_LINE_ID,Doc11_LINE_ID,Doc12_LINE_ID,Type,STEPCode,STEPName,STEPSEQ,EXPLAIN1,0,0,0,MappingCode,C_COLOUR,B_COLOUR,STEP_PRICE,PackagesQty,MODEL from #t1 where STEPSEQ = (select MIN(STEPSEQ) from #t1)

delete from Doc_Prodcutpackage13 where RFID_ID = '" + txtRFID.Text + @"' and cast(DocEntry as nvarchar(30))+'#'+cast(Doc1_LINE_ID as nvarchar(30))+'#'+cast(Doc11_LINE_ID as nvarchar(30))+cast(Doc12_LINE_ID as nvarchar(30))
in (select cast(DocEntry as nvarchar(30))+'#'+cast(Doc1_LINE_ID as nvarchar(30))+'#'+cast(Doc11_LINE_ID as nvarchar(30))+cast(Doc12_LINE_ID as nvarchar(30)) from #t1)

drop table #t1
            ");
            strSqls.Add(@"declare @MaxDoc as int
select @MaxDoc = isnull(MAX(DocEntry),0) from Doc_OSGoodsIssu
declare @PlanningDoc as int
select @PlanningDoc = BaseEntry from Doc_Prodcutpackage where DocEntry = (select top 1 BaseEntry from Doc_OSGoodsIssu1 where DocEntry = @MaxDoc)
update Doc_Osplanning1 set FHQty = FHQty + " + txtQuantity.Text + " where DocEntry in (select DocEntry from Doc_Osplanning where BaseEntry = @PlanningDoc) and SN = '" + txtSN.Text + "'");
            result = FunPublic.RunSqls(strSqls);
            if (result.Status == 1)
            {
                //MessageBox.Show("提交成功！");
                txtRFID.Text = "";
                //cmbCardCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";

            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (cmbCardCode.Text == "")
            {
                MessageBox.Show("请先选择供应商！");
                return;
            }
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
                            txtRFID.Text = RFID_ID;
                            if (!CommonClass.ValidateRFID(RFID_ID))
                            {
                                throw new Exception("此卡已经被回收！");
                            }
                            string DeviceName = FunPublic.GetDeviceName();
                            DataTable dt = FunPublic.GetDt("exec [PDA_OSGoodsIssuQuery] 'RFID','" + RFID_ID + "','" + cmbCardCode.Text.Substring(0, cmbCardCode.Text.IndexOf('-')) + "',''");
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    txtEBNum.Text = dr["EBDOC"].ToString();
                                    txtPackageCode.Text = dr["PackageCode"].ToString();
                                    txtSN.Text = dr["SN"].ToString();
                                    txtQuantity.Text = dr["QtyinPackage"].ToString();

                                }
                            }
                            else
                            {
                                txtEBNum.Text = "";
                                txtPackageCode.Text = "";
                                txtSN.Text = "";
                                txtQuantity.Text = "";
                                MessageBox.Show("此供应商下无此包外发！");
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
                        txtSN.Text = "";
                        txtQuantity.Text = "";
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
                txtSN.Text = "";
                txtQuantity.Text = "";
                MessageBox.Show("刷卡失败！" + ex.Message);
            }

        }

        private void FrmOSGoodsIssu_KeyDown(object sender, KeyEventArgs e)
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
            if (cmbCardCode.Text == "")
            {
                MessageBox.Show("请先选择供应商！");
                return;
            }
            try
            {

                string RFID_ID = Barcode.scan();

                if (RFID_ID == "")
                {
                    txtRFID.Text = "";
                    //cmbCardCode.Text = "";
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
                        txtRFID.Text = RFID_ID;
                        string DeviceName = FunPublic.GetDeviceName();
                        DataTable dt = FunPublic.GetDt("exec [PDA_OSGoodsIssuQuery] 'RFID','" + RFID_ID + "','" + cmbCardCode.Text.Substring(0, cmbCardCode.Text.IndexOf('-')) + "',''");
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                txtEBNum.Text = dr["EBDOC"].ToString();
                                txtPackageCode.Text = dr["PackageCode"].ToString();
                                txtSN.Text = dr["SN"].ToString();
                                txtQuantity.Text = dr["QtyinPackage"].ToString();

                            }
                        }
                        else
                        {
                            txtEBNum.Text = "";
                            txtPackageCode.Text = "";
                            txtSN.Text = "";
                            txtQuantity.Text = "";
                            MessageBox.Show("此供应商下无此包外发！");
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
                txtSN.Text = "";
                txtQuantity.Text = "";
                MessageBox.Show("扫描失败！" + ex.Message);
            }
        }

        private void FrmOSGoodsIssu_Closed(object sender, EventArgs e)
        {
            RFID_15693.freeMode();
            CommonClass.SoftDecoding_Deinit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbCardCode.Text = "";
            txtRFID.Text = "";
            txtEBNum.Text = "";
            txtPackageCode.Text = "";
            txtSN.Text = "";
            txtQuantity.Text = "";
        }

    }
}