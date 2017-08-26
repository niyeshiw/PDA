namespace PDA
{
    partial class FrmRecRFID
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
            this.txtSN = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtRoumap = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.Text = "RFID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.Text = "EB单号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 20);
            this.label3.Text = "包号";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 20);
            this.label4.Text = "流水号";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 20);
            this.label5.Text = "数量";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(66, 229);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(42, 24);
            this.btnSubmit.TabIndex = 13;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(193, 229);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(42, 24);
            this.btnReturn.TabIndex = 14;
            this.btnReturn.Text = "返回";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // txtRFID
            // 
            this.txtRFID.Location = new System.Drawing.Point(67, 34);
            this.txtRFID.Name = "txtRFID";
            this.txtRFID.Size = new System.Drawing.Size(159, 23);
            this.txtRFID.TabIndex = 15;
            // 
            // txtEBNum
            // 
            this.txtEBNum.Enabled = false;
            this.txtEBNum.Location = new System.Drawing.Point(67, 63);
            this.txtEBNum.Name = "txtEBNum";
            this.txtEBNum.Size = new System.Drawing.Size(159, 23);
            this.txtEBNum.TabIndex = 16;
            // 
            // txtPackageCode
            // 
            this.txtPackageCode.Enabled = false;
            this.txtPackageCode.Location = new System.Drawing.Point(67, 92);
            this.txtPackageCode.Name = "txtPackageCode";
            this.txtPackageCode.Size = new System.Drawing.Size(159, 23);
            this.txtPackageCode.TabIndex = 17;
            // 
            // txtSN
            // 
            this.txtSN.Enabled = false;
            this.txtSN.Location = new System.Drawing.Point(67, 121);
            this.txtSN.Name = "txtSN";
            this.txtSN.Size = new System.Drawing.Size(159, 23);
            this.txtSN.TabIndex = 18;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Enabled = false;
            this.txtQuantity.Location = new System.Drawing.Point(67, 150);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(159, 23);
            this.txtQuantity.TabIndex = 19;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(3, 229);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(42, 24);
            this.btnScan.TabIndex = 23;
            this.btnScan.Text = "刷卡";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(130, 229);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(42, 24);
            this.btnClear.TabIndex = 31;
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtRoumap
            // 
            this.txtRoumap.Enabled = false;
            this.txtRoumap.Location = new System.Drawing.Point(67, 5);
            this.txtRoumap.Name = "txtRoumap";
            this.txtRoumap.Size = new System.Drawing.Size(159, 23);
            this.txtRoumap.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(3, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 20);
            this.label7.Text = "路线";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 20);
            this.label6.Text = "状态";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Enabled = false;
            this.cmbStatus.Items.Add("");
            this.cmbStatus.Items.Add("是");
            this.cmbStatus.Items.Add("否");
            this.cmbStatus.Location = new System.Drawing.Point(67, 179);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(159, 23);
            this.cmbStatus.TabIndex = 41;
            // 
            // FrmRecRFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRoumap);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.txtSN);
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
            this.Name = "FrmRecRFID";
            this.Text = "RFID卡回收";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmRecRFID_Load);
            this.Closed += new System.EventHandler(this.FrmRecRFID_Closed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmRecRFID_KeyDown);
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
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtRoumap;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbStatus;
    }
}