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
    public partial class FormMonHocSV : Form
    {
        string chuoiKetNoi = @"Data Source=MAY-59;Initial Catalog=QuanLyDiemSinhVien;Integrated Security=True;TrustServerCertificate=True";
        public FormMonHocSV(string maSV)
        {
            InitializeComponent();
        }

        private void FormMonHocSV_Load(object sender, EventArgs e)
        {
            KhoiTaoCboLoc();
            LoadComboKhoa();
            LoadDanhSachMonHoc();
        }

        void LoadComboKhoa()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MaKhoa, TenKhoa FROM Khoa", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboKhoa.DataSource = dt;
                    cboKhoa.DisplayMember = "TenKhoa";
                    cboKhoa.ValueMember = "MaKhoa";
                    cboKhoa.SelectedIndex = -1;
                }
            }
            catch { }
        }
        void KhoiTaoCboLoc()
        {
            cboLocHocKy.Items.Add("Tất cả");
            for (int i = 1; i <= 8; i++) cboLocHocKy.Items.Add(i.ToString());
            cboLocHocKy.SelectedIndex = 0; // Mặc định chọn "Tất cả"
        }

        // Hàm tải danh sách Môn học (Có hỗ trợ Lọc)
        void LoadDanhSachMonHoc(string hocKyLoc = "Tất cả")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();
                    string sql = "";
                    if (hocKyLoc == "Tất cả")
                    {
                        sql = @"SELECT mh.MaMH, mh.TenMH, mh.SoTinChi, mh.HocKy, k.TenKhoa, mh.MaKhoa
                                FROM MonHoc mh LEFT JOIN Khoa k ON mh.MaKhoa = k.MaKhoa";
                    }
                    else
                    {
                        // Nếu chọn học kỳ cụ thể thì lọc thêm WHERE
                        sql = @"SELECT mh.MaMH, mh.TenMH, mh.SoTinChi, mh.HocKy, k.TenKhoa, mh.MaKhoa
                                FROM MonHoc mh LEFT JOIN Khoa k ON mh.MaKhoa = k.MaKhoa
                                WHERE mh.HocKy = " + hocKyLoc;
                    }

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMonHoc.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMonHoc.Rows[e.RowIndex];
                txtMaMH.Text = row.Cells["MaMH"].Value.ToString();
                txtTenMH.Text = row.Cells["TenMH"].Value.ToString();
                txtSTC.Text = row.Cells["SoTinChi"].Value.ToString();
                cboHocKy.Text = row.Cells["HocKy"].Value.ToString();

                // Chọn đúng Khoa
                if (row.Cells["MaKhoa"].Value != DBNull.Value)
                    cboKhoa.SelectedValue = row.Cells["MaKhoa"].Value.ToString();
            }
        }

        private void cboLocHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string giatri = cboLocHocKy.SelectedItem.ToString();
            LoadDanhSachMonHoc(giatri);
        }
    }
}
