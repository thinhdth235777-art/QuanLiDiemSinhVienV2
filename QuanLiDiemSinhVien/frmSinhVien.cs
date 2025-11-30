using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiDiemSinhVien
{
    public partial class frmSinhVien : Form

    {
        public frmSinhVien()
        {
            InitializeComponent();
        }
        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboLop();
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }

        void LoadData()
        {
            try
            {
                string sql = @"SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, 
                                      l.TenLop, sv.SoDienThoai, sv.DiaChi 
                               FROM SinhVien sv
                               LEFT JOIN Lop l ON sv.MaLop = l.MaLop";

                DataTable dt = CoSoDuLieu.LayDuLieu(sql);
                dgvSinhVien.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi LoadData: " + ex.Message);
            }
        }

        void LoadComboLop()
        {
            try
            {
                string sql = "SELECT MaLop, TenLop FROM Lop";
                DataTable dt = CoSoDuLieu.LayDuLieu(sql);

                cboLop.DataSource = dt;
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";
                cboLop.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi LoadComboLop: " + ex.Message);
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
        }
    }
}
