namespace PDA
{
    partial class FrmPDALogin
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserCode = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(127, 189);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(72, 24);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "登录";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(22, 189);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 24);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.Text = "用户名：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.Text = "密码：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtUserCode
            // 
            this.txtUserCode.Location = new System.Drawing.Point(100, 97);
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.Size = new System.Drawing.Size(100, 23);
            this.txtUserCode.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(100, 128);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 23);
            this.txtPassword.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.Text = "选择工厂：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbServer
            // 
            this.cmbServer.Location = new System.Drawing.Point(99, 68);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(100, 23);
            this.cmbServer.TabIndex = 0;
            this.cmbServer.SelectedValueChanged += new System.EventHandler(this.cmbServer_SelectedValueChanged);
            // 
            // FrmPDALogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPDALogin";
            this.Text = "登录";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPDALogin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserCode;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbServer;
    }
}

