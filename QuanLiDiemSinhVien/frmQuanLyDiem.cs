using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace QuanLiDiemSinhVien
{
    public partial class frmQuanLyDiem : Form
    {
        private string hienThiChoTaiKhoan = null;
        private bool dangChinhSua = false;
        private bool dangTaoMoi = false;

        public frmQuanLyDiem() { InitializeComponent(); }

        public frmQuanLyDiem(string maSV) : this()
        {
            hienThiChoTaiKhoan = maSV;
        }

        private void frmQuanLyDiem_Load(object sender, EventArgs e)
        {
            LoadComboMonHoc();
            LoadComboMaSV();
            LoadComboTimKiem();
            LoadData();
            ChinhSua(false);

            if (!string.IsNullOrEmpty(hienThiChoTaiKhoan))
            {
                btnThem.Visible = false;
                btnSua.Visible = false;
                btnXoa.Visible = false;
                btnLuu.Visible = false;
                btnLamMoi.Visible = false;
                btnHuy.Visible = false;
                cbMaSV.Text = hienThiChoTaiKhoan;
                cbMaSV.Enabled = false;
            }
        }
        private void LoadData()
        {
            try
            {
                string sql = @"SELECT d.MaSV, sv.HoTen, d.MaMH, mh.TenMH,
                              d.DiemQuaTrinh, d.DiemThi,
                              (ISNULL(d.DiemQuaTrinh,0) + ISNULL(d.DiemThi,0)) / 2.0 AS DiemTongKet,
                              d.HocKy, d.NamHoc
                       FROM Diem d
                       LEFT JOIN SinhVien sv ON d.MaSV = sv.MaSV
                       LEFT JOIN MonHoc mh ON d.MaMH = mh.MaMH";

                DataTable dt;
                if (!string.IsNullOrEmpty(hienThiChoTaiKhoan))
                {
                    sql += " WHERE d.MaSV = @MaSV";
                    var p = new SqlParameter[] { new SqlParameter("@MaSV", hienThiChoTaiKhoan) };
                    dt = CoSoDuLieu.LayDuLieu(sql, p);
                }
                else
                {
                    dt = CoSoDuLieu.LayDuLieu(sql);
                }

                dgvDiem.DataSource = dt;
                dgvDiem.AutoResizeColumns();

                dgvDiem.Columns["MaSV"].HeaderText = "Mã SV";
                dgvDiem.Columns["HoTen"].HeaderText = "Họ và tên";
                dgvDiem.Columns["MaMH"].HeaderText = "Mã MH";
                dgvDiem.Columns["TenMH"].HeaderText = "Tên môn học";
                dgvDiem.Columns["DiemQuaTrinh"].HeaderText = "Điểm quá trình";
                dgvDiem.Columns["DiemThi"].HeaderText = "Điểm thi";
                dgvDiem.Columns["DiemTongKet"].HeaderText = "Điểm tổng kết";
                dgvDiem.Columns["HocKy"].HeaderText = "Học kỳ";
                dgvDiem.Columns["NamHoc"].HeaderText = "Năm học";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi LoadData: " + ex.Message);
            }
        }
        private void LoadComboMonHoc()
        {
            try
            {
                string sql = "SELECT MaMH, TenMH FROM MonHoc";
                cboMonHoc.DataSource = CoSoDuLieu.LayDuLieu(sql);
                cboMonHoc.DisplayMember = "TenMH";
                cboMonHoc.ValueMember = "MaMH";
                cboMonHoc.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi LoadComboMonHoc: " + ex.Message);
            }
        }
        private void ChinhSua(bool kichHoat)
        {
            dangChinhSua = kichHoat;

            cbMaSV.Enabled = kichHoat && string.IsNullOrEmpty(hienThiChoTaiKhoan);
            cboMonHoc.Enabled = kichHoat;
            txtDiemQuaTrinh.Enabled = kichHoat;
            txtDiemThi.Enabled = kichHoat;
            txtHocKy.Enabled = kichHoat;
            txtNamHoc.Enabled = kichHoat;

            txtDiemTongKet.Enabled = false;

            bool laGV = string.IsNullOrEmpty(hienThiChoTaiKhoan);
            btnThem.Enabled = !kichHoat && laGV;
            btnSua.Enabled = !kichHoat && dgvDiem.CurrentRow != null && laGV;
            btnXoa.Enabled = !kichHoat && dgvDiem.CurrentRow != null && laGV;
            btnLuu.Enabled = kichHoat;
            btnHuy.Enabled = kichHoat;
            btnLamMoi.Enabled = !kichHoat;
        }
        private void LoadComboMaSV()
        {
            var sql = "SELECT MaSV, HoTen FROM SinhVien";
            cbMaSV.DataSource = CoSoDuLieu.LayDuLieu(sql);
            cbMaSV.DisplayMember = "MaSV";
            cbMaSV.ValueMember = "MaSV";
            cbMaSV.SelectedIndex = -1;
        }
        private void TinhDiemTongKet()
        {
            try
            {
                double dq = 0, dt = 0;
                double.TryParse(txtDiemQuaTrinh.Text.Trim().Replace(',', '.'), out dq);
                double.TryParse(txtDiemThi.Text.Trim().Replace(',', '.'), out dt);
                double tong = (dq + dt) / 2.0;
                txtDiemTongKet.Text = tong % 1 == 0 ? ((int)tong).ToString() : tong.ToString("0.##");
            }
            catch { }
        }

        private void dgvDiem_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDiem.CurrentRow == null) return;
            cbMaSV.SelectedValue = dgvDiem.CurrentRow.Cells["MaSV"].Value?.ToString();
            cboMonHoc.SelectedValue = dgvDiem.CurrentRow.Cells["MaMH"].Value?.ToString();
            txtDiemQuaTrinh.Text = dgvDiem.CurrentRow.Cells["DiemQuaTrinh"].Value?.ToString();
            txtDiemThi.Text = dgvDiem.CurrentRow.Cells["DiemThi"].Value?.ToString();
            txtDiemTongKet.Text = dgvDiem.CurrentRow.Cells["DiemTongKet"].Value?.ToString();
            txtHocKy.Text = dgvDiem.CurrentRow.Cells["HocKy"].Value?.ToString();
            txtNamHoc.Text = dgvDiem.CurrentRow.Cells["NamHoc"].Value?.ToString();
            if (dgvDiem.CurrentRow == null) return;
            var row = dgvDiem.CurrentRow;
            txtDiemQuaTrinh.Text = row.Cells["DiemQuaTrinh"].Value?.ToString() ?? "";
            txtDiemThi.Text = row.Cells["DiemThi"].Value?.ToString() ?? "";
            TinhDiemTongKet();
        }
        
        private void btnThem_Click(object sender, EventArgs e)
        {
            dangTaoMoi = true;
            cbMaSV.SelectedIndex = -1;
            cboMonHoc.SelectedIndex = -1;
            txtDiemQuaTrinh.Clear();
            txtDiemThi.Clear();
            txtDiemTongKet.Clear();
            txtHocKy.Clear();
            txtNamHoc.Clear();
            ChinhSua(true);
            cbMaSV.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDiem.CurrentRow == null)
            {
                MessageBox.Show("Chọn dòng để sửa.");
                return;
            }
            dangTaoMoi = false;
            // không cho thay MaSV/MaMH khi sửa
            cbMaSV.Enabled = false;
            cboMonHoc.Enabled = false;
            ChinhSua(true);
            txtDiemQuaTrinh.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                string maSV = cbMaSV.SelectedValue?.ToString();
                if (cbMaSV.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn sinh viên.");
                    return;
                }
                string maMH = cboMonHoc.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maMH))
                {
                    MessageBox.Show("Vui lòng nhập Mã SV và chọn Môn học.");
                    return;
                }

                if (!float.TryParse(txtDiemQuaTrinh.Text.Trim(), out float dq) ||
                    !float.TryParse(txtDiemThi.Text.Trim(), out float dt))
                {
                    MessageBox.Show("Điểm phải là số hợp lệ.");
                    return;
                }

                if (!int.TryParse(txtHocKy.Text.Trim(), out int hk))
                {
                    MessageBox.Show("Học Kỳ phải là số nguyên.");
                    return;
                }

                string namHoc = txtNamHoc.Text.Trim();
                string sql;
                SqlParameter[] pars;

                if (dangTaoMoi)
                {
                    sql = @"INSERT INTO Diem (MaSV, MaMH, DiemQuaTrinh, DiemThi, HocKy, NamHoc)
                    VALUES (@MaSV, @MaMH, @DQ, @DT, @HK, @NH)";
                }
                else
                {
                    sql = @"UPDATE Diem SET DiemQuaTrinh = @DQ, DiemThi = @DT, HocKy = @HK, NamHoc = @NH
                    WHERE MaSV = @MaSV AND MaMH = @MaMH";
                }

                pars = new SqlParameter[]
                {
            new SqlParameter("@MaSV", maSV),
            new SqlParameter("@MaMH", maMH),
            new SqlParameter("@DQ", dq),
            new SqlParameter("@DT", dt),
            new SqlParameter("@HK", hk),
            new SqlParameter("@NH", namHoc)
                };

                CoSoDuLieu.ThucThiLenh(sql, pars);
                LoadData();
                ChinhSua(false);
                MessageBox.Show("Lưu thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Lưu: " + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadComboMaSV();
            LoadComboMonHoc();
            LoadData();
            ChinhSua(false);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            cbMaSV.SelectedIndex = -1;
            cboMonHoc.SelectedIndex = -1;
            txtDiemQuaTrinh.Clear();
            txtDiemThi.Clear();
            txtDiemTongKet.Clear();
            txtHocKy.Clear();
            txtNamHoc.Clear();

            ChinhSua(false);
        }
        void LoadComboTimKiem()
        {
            try
            {
                cbTimKiem.Items.Clear();
                cbTimKiem.Items.Add("Mã SV");      
                cbTimKiem.Items.Add("Họ tên");     
                cbTimKiem.Items.Add("Tên môn");    
                cbTimKiem.Items.Add("Học kỳ");     
                cbTimKiem.Items.Add("Năm học"); 
                cbTimKiem.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi LoadComboTimKiem: " + ex.Message);
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string timTheo = cbTimKiem.SelectedItem?.ToString();
                string tuKhoa = txtTimKiem.Text.Trim();

                if (string.IsNullOrEmpty(timTheo) || string.IsNullOrEmpty(tuKhoa))
                {
                    MessageBox.Show("Chọn điều kiện lọc và nhập từ khóa.");
                    return;
                }

                var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Mã SV", "d.MaSV" },
                    { "Họ tên", "sv.HoTen" },
                    { "Tên môn", "mh.TenMH" },
                    { "Học kỳ", "d.HocKy" },
                    { "Năm học", "d.NamHoc" }
                };

                if (!map.ContainsKey(timTheo))
                {
                    MessageBox.Show("Trường tìm không hợp lệ.");
                    return;
                }

                string cot = map[timTheo];
                string sqlBase = @"SELECT d. MaSV, sv.HoTen, d.MaMH, mh.TenMH,
                 d.DiemQuaTrinh, d.DiemThi, 
                 (ISNULL(d.DiemQuaTrinh,0) + ISNULL(d.DiemThi,0)) / 2.0 AS DiemTongKet,
                 d.HocKy, d.NamHoc
                 FROM Diem d
                 LEFT JOIN SinhVien sv ON d.MaSV = sv.MaSV
                 LEFT JOIN MonHoc mh ON d.MaMH = mh. MaMH
                 WHERE ";
                DataTable dt;
                if (timTheo == "Học kỳ")
                {
                    if (!int.TryParse(tuKhoa, out int hk))
                    {
                        MessageBox.Show("Học kỳ phải là số nguyên.");
                        return;
                    }
                    string sql = sqlBase + cot + " = @val";
                    var pars = new SqlParameter[] { new SqlParameter("@val", hk) };
                    dt = CoSoDuLieu.LayDuLieu(sql, pars);
                }
                else
                {
                    string sql = sqlBase + cot + " LIKE @val";
                    var pars = new SqlParameter[] { new SqlParameter("@val", "%" + tuKhoa + "%") };
                    dt = CoSoDuLieu.LayDuLieu(sql, pars);
                }

                dgvDiem.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Tìm kiếm: " + ex.Message);
            }
        }

        private void btnHienTatCa_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDiem.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.");
                return;
            }

            string maSV = dgvDiem.CurrentRow.Cells["MaSV"].Value.ToString();
            string maMH = dgvDiem.CurrentRow.Cells["MaMH"].Value.ToString();

            DialogResult d = MessageBox.Show(
                $"Bạn có chắc muốn xóa điểm của SV {maSV} – Môn {maMH}?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (d == DialogResult.No) return;

            string sql = "DELETE FROM Diem WHERE MaSV = @MaSV AND MaMH = @MaMH";
            SqlParameter[] pars =
            {
        new SqlParameter("@MaSV", maSV),
        new SqlParameter("@MaMH", maMH)
    };

            CoSoDuLieu.ThucThiLenh(sql, pars);
            LoadData();
            MessageBox.Show("Xóa thành công!");
        }

        private void cbMaSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaSV.SelectedIndex != -1)
            {
                // Lấy dòng dữ liệu được chọn
                DataRowView drv = cbMaSV.SelectedItem as DataRowView;

                if (drv != null)
                {
                    // Gán tên sinh viên vào txtHoTen
                    txtHoTen.Text = drv["HoTen"].ToString();
                }
            }
            else
            {
                txtHoTen.Text = "";
            }
        }
    }
}
