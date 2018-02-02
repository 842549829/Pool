namespace ChinaPay.B3B.Tool.Unsubscribe
{
    partial class FrmPNRQuery
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnQueryAll = new System.Windows.Forms.Button();
            this.dgvPNRHistories = new System.Windows.Forms.DataGridView();
            this.btnCancelAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPNRHistories)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(697, 536);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnQueryAll
            // 
            this.btnQueryAll.Location = new System.Drawing.Point(535, 536);
            this.btnQueryAll.Name = "btnQueryAll";
            this.btnQueryAll.Size = new System.Drawing.Size(75, 23);
            this.btnQueryAll.TabIndex = 8;
            this.btnQueryAll.Text = "查询全部";
            this.btnQueryAll.UseVisualStyleBackColor = true;
            this.btnQueryAll.Click += new System.EventHandler(this.btnQueryAll_Click);
            // 
            // dgvPNRHistories
            // 
            this.dgvPNRHistories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPNRHistories.Location = new System.Drawing.Point(2, 1);
            this.dgvPNRHistories.Name = "dgvPNRHistories";
            this.dgvPNRHistories.RowTemplate.Height = 23;
            this.dgvPNRHistories.Size = new System.Drawing.Size(779, 529);
            this.dgvPNRHistories.TabIndex = 9;
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Location = new System.Drawing.Point(616, 536);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(75, 23);
            this.btnCancelAll.TabIndex = 12;
            this.btnCancelAll.Text = "取消全部";
            this.btnCancelAll.UseVisualStyleBackColor = true;
            this.btnCancelAll.Click += new System.EventHandler(this.btnCancelAll_Click);
            // 
            // FrmPNRQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.btnCancelAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnQueryAll);
            this.Controls.Add(this.dgvPNRHistories);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPNRQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编码查询";
            this.Load += new System.EventHandler(this.FrmPNRQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPNRHistories)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnQueryAll;
        private System.Windows.Forms.DataGridView dgvPNRHistories;
        private System.Windows.Forms.Button btnCancelAll;
    }
}