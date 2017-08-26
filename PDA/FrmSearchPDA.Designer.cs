namespace PDA
{
    partial class FrmSearchPDA
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
            this.txtWORKSHOP_CODE = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLastStep = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNextStep = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
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
            this.label2.Location = new System.Drawing.Point(2, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.Text = "EB单号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(2, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 20);
            this.label3.Text = "包号";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(2, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 20);
            this.label4.Text = "款号";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(2, 122);
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
            this.txtRFID.Location = new System.Drawing.Point(66, 3);
            this.txtRFID.Name = "txtRFID";
            this.txtRFID.Size = new System.Drawing.Size(159, 23);
            this.txtRFID.TabIndex = 15;
            // 
            // txtEBNum
            // 
            this.txtEBNum.Enabled = false;
            this.txtEBNum.Location = new System.Drawing.Point(66, 32);
            this.txtEBNum.Name = "txtEBNum";
            this.txtEBNum.Size = new System.Drawing.Size(159, 23);
            this.txtEBNum.TabIndex = 16;
            // 
            // txtPackageCode
            // 
            this.txtPackageCode.Enabled = false;
            this.txtPackageCode.Location = new System.Drawing.Point(66, 61);
            this.txtPackageCode.Name = "txtPackageCode";
            this.txtPackageCode.Size = new System.Drawing.Size(159, 23);
            this.txtPackageCode.TabIndex = 17;
            // 
            // txtCSKU_CODE
            // 
            this.txtCSKU_CODE.Enabled = false;
            this.txtCSKU_CODE.Location = new System.Drawing.Point(66, 90);
            this.txtCSKU_CODE.Name = "txtCSKU_CODE";
            this.txtCSKU_CODE.Size = new System.Drawing.Size(159, 23);
            this.txtCSKU_CODE.TabIndex = 18;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Enabled = false;
            this.txtQuantity.Location = new System.Drawing.Point(66, 119);
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
            // txtWORKSHOP_CODE
            // 
            this.txtWORKSHOP_CODE.Enabled = false;
            this.txtWORKSHOP_CODE.Location = new System.Drawing.Point(66, 148);
            this.txtWORKSHOP_CODE.Name = "txtWORKSHOP_CODE";
            this.txtWORKSHOP_CODE.Size = new System.Drawing.Size(159, 23);
            this.txtWORKSHOP_CODE.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(2, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 20);
            this.label7.Text = "车间";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtLastStep
            // 
            this.txtLastStep.Enabled = false;
            this.txtLastStep.Location = new System.Drawing.Point(114, 177);
            this.txtLastStep.Name = "txtLastStep";
            this.txtLastStep.Size = new System.Drawing.Size(111, 23);
            this.txtLastStep.TabIndex = 50;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(2, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 20);
            this.label6.Text = "最后完工工序";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNextStep
            // 
            this.txtNextStep.Enabled = false;
            this.txtNextStep.Location = new System.Drawing.Point(114, 203);
            this.txtNextStep.Name = "txtNextStep";
            this.txtNextStep.Size = new System.Drawing.Size(111, 23);
            this.txtNextStep.TabIndex = 53;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(2, 206);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 20);
            this.label8.Text = "下一工序";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FrmSearchPDA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.txtNextStep);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtLastStep);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtWORKSHOP_CODE);
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
            this.Name = "FrmSearchPDA";
            this.Text = "RFID卡查询";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmSearchPDA_Load);
            this.Closed += new System.EventHandler(this.FrmSearchPDA_Closed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearchPDA_KeyDown);
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
        private System.Windows.Forms.TextBox txtWORKSHOP_CODE;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLastStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNextStep;
        private System.Windows.Forms.Label label8;
    }
}