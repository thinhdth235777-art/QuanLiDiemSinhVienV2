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
    public partial class FormMonHoc : Form
    {
        string chuoiKetNoi = @"Data Source=MAY-60;Initial Catalog=QuanLyDiemSinhVien;Integrated Security=True;TrustServerCertificate=True";
        bool dangThem = false;
        public FormMonHoc()
        {
            InitializeComponent();
        }

        private void FormMonHoc_Load(object sender, EventArgs e)
        {
            KhoiTaoCboLoc();
            LoadComboKhoa();
            LoadDanhSachMonHoc();
            ResetGiaoDien();

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

        void ResetGiaoDien()
        {
            txtMaMH.Text = ""; txtTenMH.Text = ""; txtSTC.Text = "";
            cboHocKy.SelectedIndex = -1; cboKhoa.SelectedIndex = -1;

            txtMaMH.Enabled = false; txtTenMH.Enabled = false;
            txtSTC.Enabled = false; cboHocKy.Enabled = false; cboKhoa.Enabled = false;

            btnThem.Enabled = true; btnSua.Enabled = true; btnXoa.Enabled = true;
            btnLuu.Enabled = false; btnHuy.Enabled = false;

        }
        void MoKhoaDeNhap()
        {
            txtTenMH.Enabled = true; txtSTC.Enabled = true;
            cboHocKy.Enabled = true; cboKhoa.Enabled = true;

            btnThem.Enabled = false; btnSua.Enabled = false; btnXoa.Enabled = false;
            btnLuu.Enabled = true; btnHuy.Enabled = true;
        }

        private void cboLocHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string giatri = cboLocHocKy.SelectedItem.ToString();
            LoadDanhSachMonHoc(giatri);

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

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetGiaoDien();
            dangThem = true;
            MoKhoaDeNhap();
            txtMaMH.Enabled = true;
            txtMaMH.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaMH.Text == "") { MessageBox.Show("Chọn môn cần sửa!"); return; }
            dangThem = false;
            MoKhoaDeNhap();
            txtMaMH.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaMH.Text == "") return;
            if (MessageBox.Show("Xóa môn này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    SqlParameter[] p = { new SqlParameter("@ma", txtMaMH.Text) };
                    CoSoDuLieu.ThucThiLenh("DELETE FROM MonHoc WHERE MaMH = @ma", p);

                    MessageBox.Show("Đã xóa!");
                    LoadDanhSachMonHoc(cboLocHocKy.Text);
                    ResetGiaoDien();
                }
                catch { MessageBox.Show("Không thể xóa môn này (Đã có điểm)!"); }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaMH.Text == "" || txtTenMH.Text == "")
            {
                MessageBox.Show("Thiếu thông tin!"); return;
            }

            try
            {
                // Xử lý số liệu an toàn
                int tinChi = 0; int.TryParse(txtSTC.Text, out tinChi);
                int hocKy = 1; int.TryParse(cboHocKy.Text, out hocKy);

                // Tạo bộ tham số
                SqlParameter[] p = {
                    new SqlParameter("@ma", txtMaMH.Text),
                    new SqlParameter("@ten", txtTenMH.Text),
                    new SqlParameter("@tc", tinChi),
                    new SqlParameter("@hk", hocKy),
                    new SqlParameter("@khoa", cboKhoa.SelectedValue ?? DBNull.Value)
                };

                if (dangThem)
                {
                    // Check trùng mã
                    string sqlCheck = "SELECT * FROM MonHoc WHERE MaMH=@ma";
                    SqlParameter[] pCheck = { new SqlParameter("@ma", txtMaMH.Text) };
                    if (CoSoDuLieu.LayDuLieu(sqlCheck, pCheck).Rows.Count > 0)
                    {
                        MessageBox.Show("Mã môn học đã tồn tại!"); return;
                    }

                    // Thêm mới
                    string sqlInsert = "INSERT INTO MonHoc VALUES(@ma, @ten, @tc, @hk, @khoa)";
                    CoSoDuLieu.ThucThiLenh(sqlInsert, p);
                }
                else
                {
                    // Cập nhật
                    string sqlUpdate = "UPDATE MonHoc SET TenMH=@ten, SoTinChi=@tc, HocKy=@hk, MaKhoa=@khoa WHERE MaMH=@ma";
                    CoSoDuLieu.ThucThiLenh(sqlUpdate, p);
                }

                MessageBox.Show("Lưu thành công!");
                LoadDanhSachMonHoc(cboLocHocKy.Text); // Load lại theo lọc
                ResetGiaoDien();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ResetGiaoDien();
        }
    }
}

