namespace QuanLiDiemSinhVien
{
    partial class frmXetHocBong
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
            this.lblThongKe = new System.Windows.Forms.Label();
            this.cbLop = new System.Windows.Forms.ComboBox();
            this.dgvHocBong = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHocBong)).BeginInit();
            this.SuspendLayout();
            // 
            // lblThongKe
            // 
            this.lblThongKe.AutoSize = true;
            this.lblThongKe.Location = new System.Drawing.Point(376, 39);
            this.lblThongKe.Name = "lblThongKe";
            this.lblThongKe.Size = new System.Drawing.Size(147, 25);
            this.lblThongKe.TabIndex = 0;
            this.lblThongKe.Text = "Tổng: 0 | Đạt: 0";
            // 
            // cbLop
            // 
            this.cbLop.FormattingEnabled = true;
            this.cbLop.Location = new System.Drawing.Point(161, 36);
            this.cbLop.Name = "cbLop";
            this.cbLop.Size = new System.Drawing.Size(209, 33);
            this.cbLop.TabIndex = 1;
            this.cbLop.SelectedIndexChanged += new System.EventHandler(this.cbLop_SelectedIndexChanged);
            // 
            // dgvHocBong
            // 
            this.dgvHocBong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHocBong.Location = new System.Drawing.Point(24, 134);
            this.dgvHocBong.Name = "dgvHocBong";
            this.dgvHocBong.RowHeadersWidth = 51;
            this.dgvHocBong.RowTemplate.Height = 24;
            this.dgvHocBong.Size = new System.Drawing.Size(1115, 277);
            this.dgvHocBong.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Lớp:";
            // 
            // frmXetHocBong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 735);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvHocBong);
            this.Controls.Add(this.cbLop);
            this.Controls.Add(this.lblThongKe);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmXetHocBong";
            this.Text = "frmXetHocBong";
            this.Load += new System.EventHandler(this.frmXetHocBong_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHocBong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblThongKe;
        private System.Windows.Forms.ComboBox cbLop;
        private System.Windows.Forms.DataGridView dgvHocBong;
        private System.Windows.Forms.Label label1;
    }
}