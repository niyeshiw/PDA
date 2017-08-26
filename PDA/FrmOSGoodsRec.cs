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
    public partial class FrmOSGoodsRec : Form
    {
        RFID_15693.Tag_15693 tag = new RFID_15693.Tag_15693();
        Result result = new Result();

        public FrmOSGoodsRec()
        {
            InitializeComponent();
            CommonClass.SoftDecoding_Init();
            CommonClass.SoftDecoding_Select_ScanMode();

            CommonClass.CreaterDirectory(CommonClass.Path);
            CommonClass.Path += "\\2D_S_CodeType.txt";//把保存的文件存放在当前目录的前目录的File文件夹
        }

        private void FrmOSGoodsRec_Load(object sender, EventArgs e)
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

            DataTable dt = FunPublic.GetDt("exec [dbo].[PDA_OSGoodsRecQuery] 'Vendor','','',''");
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

            string DeviceName = FunPublic.GetDeviceName();
            List<string> strSqls = new List<string>();
            strSqls.Add(@"declare @MaxDoc as int
declare @DocEntry as int
select @MaxDoc = isnull(MAX(DocEntry),0) + 1 from Doc_OSGoodsRec
select @DocEntry = MIN(t0.DocEntry) from Doc_OSGoodsIssu t0 inner join Doc_OSGoodsIssu1 t1 on t0.DocEntry = t1.DocEntry and t0.STATUS = 'O' and t1.RFID_ID = '" + txtRFID.Text + @"' and t0.BPCode = '" + cmbCardCode.Text.Substring(0, cmbCardCode.Text.IndexOf('-')) + @"' and t1.LINESTATUS not in ('C','F')
insert into Doc_OSGoodsRec(DocEntry,DOC_DATE,BPCode,BPName,CREATED,STATUS,FACTORY_CODE,BaseEntry) select @MaxDoc,getdate(),BPCode,BPName,CREATED,'O',FACTORY_CODE,@DocEntry from Doc_OSGoodsIssu where DocEntry = @DocEntry
insert into Doc_OSGoodsRec1(DocEntry,LINE_ID,PONUM,EBDOC,PRODOC,CSKU_CODE,SN,CSKU_NAME,PackageCode,RFID_ID,Quantity,BaseEntry,BaseDoc1_Line,BaseDoc11_Line,BaseDoc12_Line,Type,STEPCode,STEPName,STEPSEQ,EXPLAIN1,GRQtyp,QtyofReject,QtyofRepair,MappingCode)
select @MaxDoc,LINE_ID,PONUM,EBDOC,PRODOC,CSKU_CODE,SN,CSKU_NAME,PackageCode,RFID_ID,Quantity,BaseEntry,BaseDoc1_Line,BaseDoc11_Line,BaseDoc12_Line,Type,STEPCode,STEPName,STEPSEQ,EXPLAIN1,GRQtyp,QtyofReject,QtyofRepair,MappingCode from Doc_OSGoodsIssu1 where DocEntry = @DocEntry
update Doc_OSGoodsIssu set STATUS = 'F' where DocEntry = @DocEntry
update Doc_OSGoodsIssu1 set LINESTATUS = 'F' where DocEntry = @DocEntry
if not exists(select * from Ts_Config where [Key] = 'BPCode' and Value = '" + cmbCardCode.Text.Substring(0, cmbCardCode.Text.IndexOf('-')) + @"')
begin
    update Doc_Prodcutpackage14 set STATUS = 'F' where cast(DocEntry as nvarchar)+'#'+cast(Doc1_LINE_ID as nvarchar)+'#'+cast(Doc11_LINE_ID as nvarchar)+'#'+cast(Doc12_LINE_ID as nvarchar)+'#'+cast(Type as nvarchar) in (select cast(BaseEntry as nvarchar)+'#'+cast(BaseDoc1_Line as nvarchar)+'#'+cast(BaseDoc11_Line as nvarchar)+'#'+cast(BaseDoc12_Line as nvarchar)+'#'+cast(Type as nvarchar) from Doc_OSGoodsIssu1 where DocEntry = @DocEntry ) and RFID_ID = '" + txtRFID.Text + @"'
end
            ");

            result = FunPublic.RunSqls(strSqls);
            if (result.Status == 1)
            {
                string RFID_ID = txtRFID.Text;

                //MessageBox.Show("提交成功！");
                txtRFID.Text = "";
                //cmbCardCode.Text = "";
                txtEBNum.Text = "";
                txtPackageCode.Text = "";
                txtSN.Text = "";
                txtQuantity.Text = "";


                RFID_15693.freeMode();
                CommonClass.SoftDecoding_Deinit();

                DataTable dt = FunPublic.GetDt("exec [PDA_MappingQuery] '" + RFID_ID + "','4','',''");

                if (dt.Rows.Count > 0)
                {
                    FrmOSMapping FrmMapping = new FrmOSMapping();
                    FrmMapping.txtRFID1.Text = RFID_ID;
                    FrmMapping.txtMappingCode.Text = dt.Rows[0]["MappingCode"].ToString();
                    FrmMapping.txtEBNum.Text = dt.Rows[0]["EBDOC"].ToString();
                    FrmMapping.txtPackageCode.Text = dt.Rows[0]["PackageCode"].ToString();
                    FrmMapping.txtSN.Text = dt.Rows[0]["SN"].ToString();
                    FrmMapping.txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                    FrmMapping.model = "弹窗";
                    FrmMapping.ShowDialog();
                    FrmMapping.txtRFID2.Focus();

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
                }
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
                            DataTable dt = FunPublic.GetDt("exec [PDA_OSGoodsRecQuery] 'RFID','" + RFID_ID + "','" + cmbCardCode.Text.Substring(0, cmbCardCode.Text.IndexOf('-')) + "',''");
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    txtEBNum.Text = dr["EBDOC"].ToString();
                                    txtPackageCode.Text = dr["PackageCode"].ToString();
                                    txtSN.Text = dr["SN"].ToString();
                                    txtQuantity.Text = dr["Quantity"].ToString();
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

        private void FrmOSGoodsRec_KeyDown(object sender, KeyEventArgs e)
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
                        DataTable dt = FunPublic.GetDt("exec [PDA_OSGoodsRecQuery] 'RFID','" + RFID_ID + "','" + cmbCardCode.Text.Substring(0, cmbCardCode.Text.IndexOf('-')) + "',''");
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                txtEBNum.Text = dr["EBDOC"].ToString();
                                txtPackageCode.Text = dr["PackageCode"].ToString();
                                txtSN.Text = dr["SN"].ToString();
                                txtQuantity.Text = dr["Quantity"].ToString();
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

        private void FrmOSGoodsRec_Closed(object sender, EventArgs e)
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