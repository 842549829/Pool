namespace ChinaPay.B3B.Tool.Unsubscribe
{
    partial class FrmOption
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tvwOption = new System.Windows.Forms.TreeView();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.txtDataSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.lblRange = new System.Windows.Forms.Label();
            this.nudRange = new System.Windows.Forms.NumericUpDown();
            this.chkAutoUnsubscribe = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRange)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(563, 528);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(703, 528);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tvwOption
            // 
            this.tvwOption.Location = new System.Drawing.Point(12, 12);
            this.tvwOption.Name = "tvwOption";
            this.tvwOption.Size = new System.Drawing.Size(218, 495);
            this.tvwOption.TabIndex = 0;
            // 
            // lblDataSource
            // 
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(32, 36);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(53, 12);
            this.lblDataSource.TabIndex = 3;
            this.lblDataSource.Text = "数据源：";
            // 
            // txtDataSource
            // 
            this.txtDataSource.Location = new System.Drawing.Point(34, 51);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(424, 21);
            this.txtDataSource.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "时间间隔：（分）";
            // 
            // nudInterval
            // 
            this.nudInterval.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudInterval.Location = new System.Drawing.Point(34, 117);
            this.nudInterval.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nudInterval.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(120, 21);
            this.nudInterval.TabIndex = 6;
            this.nudInterval.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lblRange
            // 
            this.lblRange.AutoSize = true;
            this.lblRange.Location = new System.Drawing.Point(32, 161);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(101, 12);
            this.lblRange.TabIndex = 7;
            this.lblRange.Text = "时间区间：（时）";
            // 
            // nudRange
            // 
            this.nudRange.Location = new System.Drawing.Point(34, 187);
            this.nudRange.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nudRange.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRange.Name = "nudRange";
            this.nudRange.Size = new System.Drawing.Size(120, 21);
            this.nudRange.TabIndex = 8;
            this.nudRange.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // chkAutoUnsubscribe
            // 
            this.chkAutoUnsubscribe.AutoSize = true;
            this.chkAutoUnsubscribe.Checked = true;
            this.chkAutoUnsubscribe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoUnsubscribe.Location = new System.Drawing.Point(34, 464);
            this.chkAutoUnsubscribe.Name = "chkAutoUnsubscribe";
            this.chkAutoUnsubscribe.Size = new System.Drawing.Size(72, 16);
            this.chkAutoUnsubscribe.TabIndex = 9;
            this.chkAutoUnsubscribe.Text = "自动取消";
            this.chkAutoUnsubscribe.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.nudInterval);
            this.panel1.Controls.Add(this.chkAutoUnsubscribe);
            this.panel1.Controls.Add(this.lblDataSource);
            this.panel1.Controls.Add(this.nudRange);
            this.panel1.Controls.Add(this.txtDataSource);
            this.panel1.Controls.Add(this.lblRange);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(236, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 495);
            this.panel1.TabIndex = 10;
            // 
            // FrmOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 568);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tvwOption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOption";
            this.Text = " ";
            this.Load += new System.EventHandler(this.FrmOption_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRange)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TreeView tvwOption;
        private System.Windows.Forms.Label lblDataSource;
        private System.Windows.Forms.TextBox txtDataSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudInterval;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.NumericUpDown nudRange;
        private System.Windows.Forms.CheckBox chkAutoUnsubscribe;
        private System.Windows.Forms.Panel panel1;
    }
}