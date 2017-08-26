namespace PDA
{
    partial class FrmOSMapping
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
            this.txtRFID1 = new System.Windows.Forms.TextBox();
            this.txtEBNum = new System.Windows.Forms.TextBox();
            this.txtPackageCode = new System.Windows.Forms.TextBox();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtRFID2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMappingCode = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.Text = "车间配货";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.Text = "EB单号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 20);
            this.label3.Text = "包号";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 20);
            this.label4.Text = "流水号";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 20);
            this.label5.Text = "数量";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(36, 99);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(42, 24);
            this.btnSubmit.TabIndex = 13;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(124, 204);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(42, 24);
            this.btnReturn.TabIndex = 14;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // txtRFID1
            // 
            this.txtRFID1.Location = new System.Drawing.Point(77, 3);
            this.txtRFID1.Name = "txtRFID1";
            this.txtRFID1.Size = new System.Drawing.Size(155, 23);
            this.txtRFID1.TabIndex = 15;
            // 
            // txtEBNum
            // 
            this.txtEBNum.Enabled = false;
            this.txtEBNum.Location = new System.Drawing.Point(77, 87);
            this.txtEBNum.Name = "txtEBNum";
            this.txtEBNum.Size = new System.Drawing.Size(155, 23);
            this.txtEBNum.TabIndex = 16;
            // 
            // txtPackageCode
            // 
            this.txtPackageCode.Enabled = false;
            this.txtPackageCode.Location = new System.Drawing.Point(77, 115);
            this.txtPackageCode.Name = "txtPackageCode";
            this.txtPackageCode.Size = new System.Drawing.Size(155, 23);
            this.txtPackageCode.TabIndex = 17;
            // 
            // txtSN
            // 
            this.txtSN.Enabled = false;
            this.txtSN.Location = new System.Drawing.Point(77, 143);
            this.txtSN.Name = "txtSN";
            this.txtSN.Size = new System.Drawing.Size(155, 23);
            this.txtSN.TabIndex = 18;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Enabled = false;
            this.txtQuantity.Location = new System.Drawing.Point(77, 171);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(155, 23);
            this.txtQuantity.TabIndex = 19;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(3, 204);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(42, 24);
            this.btnScan.TabIndex = 23;
            this.btnScan.Text = "刷卡";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(60, 204);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(42, 24);
            this.btnClear.TabIndex = 31;
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtRFID2
            // 
            this.txtRFID2.Location = new System.Drawing.Point(77, 31);
            this.txtRFID2.Name = "txtRFID2";
            this.txtRFID2.Size = new System.Drawing.Size(155, 23);
            this.txtRFID2.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 20);
            this.label7.Text = "外协配货";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtMappingCode
            // 
            this.txtMappingCode.Enabled = false;
            this.txtMappingCode.Location = new System.Drawing.Point(77, 59);
            this.txtMappingCode.Name = "txtMappingCode";
            this.txtMappingCode.Size = new System.Drawing.Size(155, 23);
            this.txtMappingCode.TabIndex = 56;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(14, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 20);
            this.label9.Text = "匹配号";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSubmit);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(28, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 140);
            this.panel1.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(100, 99);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(42, 24);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
            this.label6.Location = new System.Drawing.Point(36, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 46);
            this.label6.Text = "配货成功";
            // 
            // FrmOSMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtMappingCode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtRFID2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.txtSN);
            this.Controls.Add(this.txtPackageCode);
            this.Controls.Add(this.txtEBNum);
            this.Controls.Add(this.txtRFID1);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOSMapping";
            this.Text = "外协匹配";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmWorkshooGR_Load);
            this.Closed += new System.EventHandler(this.FrmWorkshooGR_Closed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmWorkshooGR_KeyDown);
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox txtRFID1;
        public System.Windows.Forms.TextBox txtEBNum;
        public System.Windows.Forms.TextBox txtPackageCode;
        public System.Windows.Forms.TextBox txtSN;
        public System.Windows.Forms.TextBox txtQuantity;
        public System.Windows.Forms.TextBox txtRFID2;
        public System.Windows.Forms.TextBox txtMappingCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
    }
}