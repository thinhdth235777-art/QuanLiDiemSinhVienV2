using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiDiemSinhVien
{
    public partial class frmXetHocBong : Form
    {
        public frmXetHocBong()
        {
            InitializeComponent();
        }

        private void frmXetHocBong_Load(object sender, EventArgs e)
        {
            LoadComboLop();
            LoadData();
        }
        private void LoadComboLop()
        {
            DataTable dt = CoSoDuLieu.LayDuLieu("SELECT MaLop, TenLop FROM Lop ORDER BY TenLop");

            DataRow row = dt.NewRow();
            row["MaLop"] = "";
            row["TenLop"] = "-- Tất cả --";
            dt.Rows.InsertAt(row, 0);

            cbLop.DataSource = dt;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "MaLop";
        }
        private void ChinhMau()
        {
            int col = dgvHocBong.Columns["XetHocBong"].Index;
            foreach (DataGridViewRow row in dgvHocBong.Rows)
            {
                if (row.Cells[col].Value != null && row.Cells[col].Value.ToString() == "Đạt")
                    row.Cells[col].Style.BackColor = Color.LightGreen;
                else
                    row.Cells[col].Style.BackColor = Color.LightCoral;
            }
        }
        private void LoadData()
        {
            DataTable dt = CoSoDuLieu.LayHocBong();
            dgvHocBong.DataSource = dt;
            dgvHocBong.AutoResizeColumns();

            // Tô màu
            ChinhMau();

            // Thống kê
            int dat = dt.AsEnumerable().Count(r => r["XetHocBong"].ToString() == "Đạt");
            lblThongKe.Text = $"Tổng: {dt.Rows.Count} | Đạt: {dat}";
        }

        private void cbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maLop = cbLop.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(maLop))
                LoadData();
            else
            {
                DataTable dt = CoSoDuLieu.LayHocBongTheoLop(maLop);
                dgvHocBong.DataSource = dt;
                ChinhMau();

                int dat = dt.AsEnumerable().Count(r => r["XetHocBong"].ToString() == "Đạt");
                lblThongKe.Text = $"Tổng: {dt.Rows.Count} | Đạt: {dat}";
            }
        }
    }
}
