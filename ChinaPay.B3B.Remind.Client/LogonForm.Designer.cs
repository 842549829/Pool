namespace ChinaPay.B3B.Remind.Client {
    partial class LogonForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogonForm));
            this.tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCarrier = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeUser = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLogon = new System.Windows.Forms.Button();
            this.lblFooter = new System.Windows.Forms.Label();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPwd = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.chbSavePassword = new System.Windows.Forms.CheckBox();
            this.cmsTray.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tray
            // 
            this.tray.ContextMenuStrip = this.cmsTray;
            this.tray.Icon = ((System.Drawing.Icon)(resources.GetObject("tray.Icon")));
            this.tray.Text = "B3B订单提醒";
            // 
            // cmsTray
            // 
            this.cmsTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCarrier,
            this.tsmiStatus,
            this.tsmiChangeUser,
            this.tsmiExit});
            this.cmsTray.Name = "cmsTray";
            this.cmsTray.Size = new System.Drawing.Size(153, 114);
            // 
            // tsmiCarrier
            // 
            this.tsmiCarrier.Image = ((System.Drawing.Image)(resources.GetObject("tsmiCarrier.Image")));
            this.tsmiCarrier.Name = "tsmiCarrier";
            this.tsmiCarrier.Size = new System.Drawing.Size(140, 22);
            this.tsmiCarrier.Text = "航空公司(&C)";
            this.tsmiCarrier.Click += new System.EventHandler(this.tsmiCarrier_Click);
            // 
            // tsmiStatus
            // 
            this.tsmiStatus.Image = ((System.Drawing.Image)(resources.GetObject("tsmiStatus.Image")));
            this.tsmiStatus.Name = "tsmiStatus";
            this.tsmiStatus.Size = new System.Drawing.Size(152, 22);
            this.tsmiStatus.Text = "提醒状态(&S)";
            this.tsmiStatus.Click += new System.EventHandler(this.tsmiStatus_Click);
            // 
            // tsmiChangeUser
            // 
            this.tsmiChangeUser.Image = ((System.Drawing.Image)(resources.GetObject("tsmiChangeUser.Image")));
            this.tsmiChangeUser.Name = "tsmiChangeUser";
            this.tsmiChangeUser.Size = new System.Drawing.Size(152, 22);
            this.tsmiChangeUser.Text = "切换用户(&E)";
            this.tsmiChangeUser.Click += new System.EventHandler(this.tsmiChangeUser_Click);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExit.Image")));
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(152, 22);
            this.tsmiExit.Text = "退出(&X)";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(236)))), ((int)(((byte)(229)))));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(230, 210);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 33);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取  消(&C)";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLogon
            // 
            this.btnLogon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(236)))), ((int)(((byte)(229)))));
            this.btnLogon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLogon.BackgroundImage")));
            this.btnLogon.FlatAppearance.BorderSize = 0;
            this.btnLogon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogon.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.btnLogon.ForeColor = System.Drawing.Color.White;
            this.btnLogon.Location = new System.Drawing.Point(89, 210);
            this.btnLogon.Name = "btnLogon";
            this.btnLogon.Size = new System.Drawing.Size(114, 33);
            this.btnLogon.TabIndex = 3;
            this.btnLogon.Text = "登  录(&L)";
            this.btnLogon.UseVisualStyleBackColor = false;
            this.btnLogon.Click += new System.EventHandler(this.btnLogon_Click);
            // 
            // lblFooter
            // 
            this.lblFooter.AutoSize = true;
            this.lblFooter.Font = new System.Drawing.Font("微软雅黑", 11.5F);
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblFooter.Location = new System.Drawing.Point(61, 254);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(312, 21);
            this.lblFooter.TabIndex = 100;
            this.lblFooter.Text = "昆明国付通科技有限公司    0871-8883388";
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTitle.BackgroundImage")));
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(435, 50);
            this.pnlTitle.TabIndex = 102;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(127)))), ((int)(((byte)(222)))));
            this.lblUserName.Location = new System.Drawing.Point(3, 12);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(83, 20);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "用户名:";
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.lblPwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(127)))), ((int)(((byte)(222)))));
            this.lblPwd.Location = new System.Drawing.Point(3, 50);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(84, 20);
            this.lblPwd.TabIndex = 3;
            this.lblPwd.Text = "密  码:";
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Location = new System.Drawing.Point(147, 91);
            this.txtUserName.MaxLength = 50;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(210, 14);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // txtPwd
            // 
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPwd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtPwd.Location = new System.Drawing.Point(92, 54);
            this.txtPwd.MaxLength = 50;
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(210, 14);
            this.txtPwd.TabIndex = 1;
            this.txtPwd.UseSystemPasswordChar = true;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlContent.BackgroundImage")));
            this.pnlContent.Controls.Add(this.lblPwd);
            this.pnlContent.Controls.Add(this.txtPwd);
            this.pnlContent.Controls.Add(this.lblUserName);
            this.pnlContent.Location = new System.Drawing.Point(55, 75);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(325, 82);
            this.pnlContent.TabIndex = 0;
            // 
            // chbSavePassword
            // 
            this.chbSavePassword.AutoSize = true;
            this.chbSavePassword.Location = new System.Drawing.Point(147, 174);
            this.chbSavePassword.Name = "chbSavePassword";
            this.chbSavePassword.Size = new System.Drawing.Size(72, 16);
            this.chbSavePassword.TabIndex = 2;
            this.chbSavePassword.Text = "保存密码";
            this.chbSavePassword.UseVisualStyleBackColor = true;
            // 
            // LogonForm
            // 
            this.AcceptButton = this.btnLogon;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(230)))), ((int)(((byte)(251)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(435, 300);
            this.Controls.Add(this.chbSavePassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.btnLogon);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.cmsTray.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon tray;
        private System.Windows.Forms.ContextMenuStrip cmsTray;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiCarrier;
        private System.Windows.Forms.ToolStripMenuItem tsmiStatus;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeUser;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLogon;
        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.CheckBox chbSavePassword;


    }
}