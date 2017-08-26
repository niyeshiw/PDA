namespace PDA
{
    partial class FrmMenu
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
            this.btnDailyReport = new System.Windows.Forms.Button();
            this.btnOSGoodsIssu = new System.Windows.Forms.Button();
            this.btnOSGoodsRec = new System.Windows.Forms.Button();
            this.btnRejectPackage = new System.Windows.Forms.Button();
            this.btnSearchPDA = new System.Windows.Forms.Button();
            this.btnWorkshooGR = new System.Windows.Forms.Button();
            this.btnRecRFID = new System.Windows.Forms.Button();
            this.btnStopPDA = new System.Windows.Forms.Button();
            this.btnMapping = new System.Windows.Forms.Button();
            this.btnOsMapping = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDailyReport
            // 
            this.btnDailyReport.Location = new System.Drawing.Point(3, 3);
            this.btnDailyReport.Name = "btnDailyReport";
            this.btnDailyReport.Size = new System.Drawing.Size(110, 30);
            this.btnDailyReport.TabIndex = 4;
            this.btnDailyReport.Text = "完工入库";
            this.btnDailyReport.Click += new System.EventHandler(this.btnDailyReport_Click);
            // 
            // btnOSGoodsIssu
            // 
            this.btnOSGoodsIssu.Location = new System.Drawing.Point(125, 3);
            this.btnOSGoodsIssu.Name = "btnOSGoodsIssu";
            this.btnOSGoodsIssu.Size = new System.Drawing.Size(110, 30);
            this.btnOSGoodsIssu.TabIndex = 5;
            this.btnOSGoodsIssu.Text = "外协发货";
            this.btnOSGoodsIssu.Click += new System.EventHandler(this.btnOSGoodsIssu_Click);
            // 
            // btnOSGoodsRec
            // 
            this.btnOSGoodsRec.Location = new System.Drawing.Point(3, 39);
            this.btnOSGoodsRec.Name = "btnOSGoodsRec";
            this.btnOSGoodsRec.Size = new System.Drawing.Size(110, 30);
            this.btnOSGoodsRec.TabIndex = 6;
            this.btnOSGoodsRec.Text = "外协收货";
            this.btnOSGoodsRec.Click += new System.EventHandler(this.btnOSGoodsRec_Click);
            // 
            // btnRejectPackage
            // 
            this.btnRejectPackage.Location = new System.Drawing.Point(3, 75);
            this.btnRejectPackage.Name = "btnRejectPackage";
            this.btnRejectPackage.Size = new System.Drawing.Size(110, 30);
            this.btnRejectPackage.TabIndex = 7;
            this.btnRejectPackage.Text = "报废/返工";
            this.btnRejectPackage.Click += new System.EventHandler(this.btnRejectPackage_Click);
            // 
            // btnSearchPDA
            // 
            this.btnSearchPDA.Location = new System.Drawing.Point(125, 111);
            this.btnSearchPDA.Name = "btnSearchPDA";
            this.btnSearchPDA.Size = new System.Drawing.Size(110, 30);
            this.btnSearchPDA.TabIndex = 8;
            this.btnSearchPDA.Text = "RFID卡查询";
            this.btnSearchPDA.Click += new System.EventHandler(this.btnSearchPDA_Click);
            // 
            // btnWorkshooGR
            // 
            this.btnWorkshooGR.Location = new System.Drawing.Point(125, 39);
            this.btnWorkshooGR.Name = "btnWorkshooGR";
            this.btnWorkshooGR.Size = new System.Drawing.Size(110, 30);
            this.btnWorkshooGR.TabIndex = 9;
            this.btnWorkshooGR.Text = "车间收货";
            this.btnWorkshooGR.Click += new System.EventHandler(this.btnWorkshooGR_Click);
            // 
            // btnRecRFID
            // 
            this.btnRecRFID.Location = new System.Drawing.Point(3, 111);
            this.btnRecRFID.Name = "btnRecRFID";
            this.btnRecRFID.Size = new System.Drawing.Size(110, 30);
            this.btnRecRFID.TabIndex = 10;
            this.btnRecRFID.Text = "RFID卡回收";
            this.btnRecRFID.Click += new System.EventHandler(this.btnRecRFID_Click);
            // 
            // btnStopPDA
            // 
            this.btnStopPDA.Location = new System.Drawing.Point(125, 75);
            this.btnStopPDA.Name = "btnStopPDA";
            this.btnStopPDA.Size = new System.Drawing.Size(110, 30);
            this.btnStopPDA.TabIndex = 11;
            this.btnStopPDA.Text = "终止生产";
            this.btnStopPDA.Click += new System.EventHandler(this.btnStopPDA_Click);
            // 
            // btnMapping
            // 
            this.btnMapping.Location = new System.Drawing.Point(3, 147);
            this.btnMapping.Name = "btnMapping";
            this.btnMapping.Size = new System.Drawing.Size(110, 30);
            this.btnMapping.TabIndex = 12;
            this.btnMapping.Text = "配货";
            this.btnMapping.Click += new System.EventHandler(this.btnMapping_Click);
            // 
            // btnOsMapping
            // 
            this.btnOsMapping.Location = new System.Drawing.Point(125, 147);
            this.btnOsMapping.Name = "btnOsMapping";
            this.btnOsMapping.Size = new System.Drawing.Size(110, 30);
            this.btnOsMapping.TabIndex = 12;
            this.btnOsMapping.Text = "外协配货";
            this.btnOsMapping.Click += new System.EventHandler(this.btnOsMapping_Click);
            // 
            // FrmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.btnOsMapping);
            this.Controls.Add(this.btnMapping);
            this.Controls.Add(this.btnStopPDA);
            this.Controls.Add(this.btnRecRFID);
            this.Controls.Add(this.btnWorkshooGR);
            this.Controls.Add(this.btnSearchPDA);
            this.Controls.Add(this.btnRejectPackage);
            this.Controls.Add(this.btnOSGoodsRec);
            this.Controls.Add(this.btnOSGoodsIssu);
            this.Controls.Add(this.btnDailyReport);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMenu";
            this.Text = "主菜单";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDailyReport;
        private System.Windows.Forms.Button btnOSGoodsIssu;
        private System.Windows.Forms.Button btnOSGoodsRec;
        private System.Windows.Forms.Button btnRejectPackage;
        private System.Windows.Forms.Button btnSearchPDA;
        private System.Windows.Forms.Button btnWorkshooGR;
        private System.Windows.Forms.Button btnRecRFID;
        private System.Windows.Forms.Button btnStopPDA;
        private System.Windows.Forms.Button btnMapping;
        private System.Windows.Forms.Button btnOsMapping;

    }
}