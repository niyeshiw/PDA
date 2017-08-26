namespace PDA
{
    partial class FrmRejectPackage
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.txtRFID = new System.Windows.Forms.TextBox();
            this.txtEBNum = new System.Windows.Forms.TextBox();
            this.txtPackageCode = new System.Windows.Forms.TextBox();
            this.txtCSKU_CODE = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbfree = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbRejectStationf = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbReason = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRFID_New = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.cmbRejectStation = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.Text = "RFID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.Text = "EB单号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(2, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 20);
            this.label3.Text = "包号";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(2, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 20);
            this.label4.Text = "款号";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 20);
            this.label5.Text = "数量";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(59, 463);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(42, 24);
            this.btnSubmit.TabIndex = 13;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.Visible = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(176, 463);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(42, 24);
            this.btnReturn.TabIndex = 14;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // txtRFID
            // 
            this.txtRFID.Location = new System.Drawing.Point(66, 3);
            this.txtRFID.Name = "txtRFID";
            this.txtRFID.Size = new System.Drawing.Size(152, 23);
            this.txtRFID.TabIndex = 15;
            // 
            // txtEBNum
            // 
            this.txtEBNum.Enabled = false;
            this.txtEBNum.Location = new System.Drawing.Point(67, 206);
            this.txtEBNum.Name = "txtEBNum";
            this.txtEBNum.Size = new System.Drawing.Size(152, 23);
            this.txtEBNum.TabIndex = 16;
            // 
            // txtPackageCode
            // 
            this.txtPackageCode.Enabled = false;
            this.txtPackageCode.Location = new System.Drawing.Point(66, 264);
            this.txtPackageCode.Name = "txtPackageCode";
            this.txtPackageCode.Size = new System.Drawing.Size(153, 23);
            this.txtPackageCode.TabIndex = 17;
            // 
            // txtCSKU_CODE
            // 
            this.txtCSKU_CODE.Enabled = false;
            this.txtCSKU_CODE.Location = new System.Drawing.Point(66, 235);
            this.txtCSKU_CODE.Name = "txtCSKU_CODE";
            this.txtCSKU_CODE.Size = new System.Drawing.Size(153, 23);
            this.txtCSKU_CODE.TabIndex = 18;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(67, 90);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(152, 23);
            this.txtQuantity.TabIndex = 19;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(2, 463);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(42, 24);
            this.btnScan.TabIndex = 23;
            this.btnScan.Text = "刷卡";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(118, 463);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(42, 24);
            this.btnClear.TabIndex = 31;
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cmbType
            // 
            this.cmbType.Items.Add("");
            this.cmbType.Items.Add("A-返工");
            this.cmbType.Items.Add("B-报废");
            this.cmbType.Location = new System.Drawing.Point(67, 119);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(152, 23);
            this.cmbType.TabIndex = 50;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 20);
            this.label7.Text = "类型";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbfree
            // 
            this.cmbfree.Items.Add("");
            this.cmbfree.Items.Add("Y-是");
            this.cmbfree.Items.Add("N-否");
            this.cmbfree.Location = new System.Drawing.Point(66, 32);
            this.cmbfree.Name = "cmbfree";
            this.cmbfree.Size = new System.Drawing.Size(152, 23);
            this.cmbfree.TabIndex = 53;
            this.cmbfree.SelectedIndexChanged += new System.EventHandler(this.cmbfree_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(1, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 20);
            this.label8.Text = "工资";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbRejectStationf
            // 
            this.cmbRejectStationf.Location = new System.Drawing.Point(67, 148);
            this.cmbRejectStationf.Name = "cmbRejectStationf";
            this.cmbRejectStationf.Size = new System.Drawing.Size(152, 23);
            this.cmbRejectStationf.TabIndex = 56;
            this.cmbRejectStationf.SelectedIndexChanged += new System.EventHandler(this.cmbRejectStationf_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 151);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 20);
            this.label9.Text = "返工从";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbReason
            // 
            this.cmbReason.Location = new System.Drawing.Point(67, 177);
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.Size = new System.Drawing.Size(152, 23);
            this.cmbReason.TabIndex = 59;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 20);
            this.label10.Text = "原因";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtRFID_New
            // 
            this.txtRFID_New.Location = new System.Drawing.Point(65, 434);
            this.txtRFID_New.Name = "txtRFID_New";
            this.txtRFID_New.Size = new System.Drawing.Size(153, 23);
            this.txtRFID_New.TabIndex = 62;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(1, 437);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 20);
            this.label6.Text = "新RFID号";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // listView1
            // 
            this.listView1.Columns.Add(this.columnHeader9);
            this.listView1.Columns.Add(this.columnHeader8);
            this.listView1.Columns.Add(this.columnHeader1);
            this.listView1.Columns.Add(this.columnHeader2);
            this.listView1.Columns.Add(this.columnHeader3);
            this.listView1.Columns.Add(this.columnHeader4);
            this.listView1.Columns.Add(this.columnHeader5);
            this.listView1.Columns.Add(this.columnHeader6);
            this.listView1.Columns.Add(this.columnHeader7);
            this.listView1.Columns.Add(this.columnHeader10);
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(4, 297);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(215, 131);
            this.listView1.TabIndex = 64;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "选择";
            this.columnHeader9.Width = 0;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "行号";
            this.columnHeader8.Width = 0;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "工序编号";
            this.columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "工序名称";
            this.columnHeader2.Width = 70;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "工艺数量";
            this.columnHeader3.Width = 70;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "车间编号";
            this.columnHeader4.Width = 70;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "工序排序";
            this.columnHeader5.Width = 70;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "工序单价";
            this.columnHeader6.Width = 70;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "历史单价";
            this.columnHeader7.Width = 70;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "已完工";
            this.columnHeader10.Width = 60;
            // 
            // cmbRejectStation
            // 
            this.cmbRejectStation.Location = new System.Drawing.Point(66, 61);
            this.cmbRejectStation.Name = "cmbRejectStation";
            this.cmbRejectStation.Size = new System.Drawing.Size(152, 23);
            this.cmbRejectStation.TabIndex = 66;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(0, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 20);
            this.label11.Text = "报废工序";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FrmRejectPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.cmbRejectStation);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.txtRFID_New);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbReason);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbRejectStationf);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbfree);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.txtCSKU_CODE);
            this.Controls.Add(this.txtPackageCode);
            this.Controls.Add(this.txtEBNum);
            this.Controls.Add(this.txtRFID);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRejectPackage";
            this.Text = "报废或返工";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmRejectPackage_Load);
            this.Closed += new System.EventHandler(this.FrmRejectPackage_Closed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmRejectPackage_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.TextBox txtRFID;
        private System.Windows.Forms.TextBox txtEBNum;
        private System.Windows.Forms.TextBox txtPackageCode;
        private System.Windows.Forms.TextBox txtCSKU_CODE;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbfree;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbRejectStationf;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbReason;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtRFID_New;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ComboBox cmbRejectStation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
    }
}