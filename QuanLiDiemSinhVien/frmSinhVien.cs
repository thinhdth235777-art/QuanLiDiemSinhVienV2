using System;
using System.Data;
using System.Data.SqlClient; // Cần cái này để tạo tham số SqlParameter
using System.Windows.Forms;

namespace QuanLiDiemSinhVien // ⚠️ Kiểm tra tên Namespace cho khớp
{
    public partial class frmSinhVien : Form
    {
        bool dangThem = false; // Biến kiểm tra đang Thêm hay Sửa
        string chuoiKetNoi = CoSoDuLieu.chuoiKetNoi;
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboLop();
            ResetGiaoDien();
        }

        // ==========================================================
        // 1. CÁC HÀM LOAD DỮ LIỆU (Dùng CoSoDuLieu.LayDuLieu)
        // ==========================================================

        void LoadData()
        {
            try
            {
                // Lấy thông tin sinh viên + Tên lớp
                string sql = @"SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, 
                                      l.TenLop, sv.SoDienThoai, sv.DiaChi, sv.MaLop 
                               FROM SinhVien sv 
                               LEFT JOIN Lop l ON sv.MaLop = l.MaLop";

                // Gọi hàm từ class cũ của bạn
                DataTable dt = CoSoDuLieu.LayDuLieu(sql);
                dgvSinhVien.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
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
            catch { }
        }

        // ==========================================================
        // 2. XỬ LÝ GIAO DIỆN (Khóa/Mở nút)
        // ==========================================================

        void ResetGiaoDien()
        {
            txtMaSV.Text = ""; txtHoTen.Text = ""; txtDiaChi.Text = ""; txtSDT.Text = "";
            cboLop.SelectedIndex = -1;
            rdoNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Now;

            // Khóa nhập liệu
            txtMaSV.Enabled = false; txtHoTen.Enabled = false; dtpNgaySinh.Enabled = false;
            rdoNam.Enabled = false; rdoNu.Enabled = false; cboLop.Enabled = false;
            txtDiaChi.Enabled = false; txtSDT.Enabled = false;

            // Chỉnh nút
            btnThem.Enabled = true; btnSua.Enabled = true; btnXoa.Enabled = true;
            btnLuu.Enabled = false; btnHuy.Enabled = false;
        }

        void MoKhoaNhapLieu()
        {
            txtHoTen.Enabled = true; dtpNgaySinh.Enabled = true;
            rdoNam.Enabled = true; rdoNu.Enabled = true; cboLop.Enabled = true;
            txtDiaChi.Enabled = true; txtSDT.Enabled = true;

            btnThem.Enabled = false; btnSua.Enabled = false; btnXoa.Enabled = false;
            btnLuu.Enabled = true; btnHuy.Enabled = true;
        }

        // ==========================================================
        // 3. CÁC SỰ KIỆN NÚT BẤM (Dùng CoSoDuLieu.ThucThiLenh)
        // ==========================================================

        // Click vào bảng -> Hiện lên trên
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();

                if (row.Cells["NgaySinh"].Value != DBNull.Value)
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);

                string gt = row.Cells["GioiTinh"].Value.ToString();
                if (gt == "Nam") rdoNam.Checked = true; else rdoNu.Checked = true;

                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();

                // Kiểm tra tên cột trong SQL của bạn (SoDienThoai)
                if (row.Cells["SoDienThoai"].Value != null)
                    txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();

                if (row.Cells["MaLop"].Value != DBNull.Value)
                    cboLop.SelectedValue = row.Cells["MaLop"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            dangThem = true;
            ResetGiaoDien();
            MoKhoaNhapLieu();
            txtMaSV.Enabled = true; // Thêm thì mở khóa mã
            txtMaSV.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "") { MessageBox.Show("Chọn sinh viên cần sửa!"); return; }
            dangThem = false;
            MoKhoaNhapLieu();
            txtMaSV.Enabled = false; // Sửa thì khóa mã
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ResetGiaoDien();
        }

        // --- NÚT LƯU QUAN TRỌNG ---
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text.Trim() == "" || txtHoTen.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên sinh viên!", "Thông báo");
                return;
            }

            try
            {
                string gioitinh = rdoNam.Checked ? "Nam" : "Nữ";

                if (dangThem)
                {
                    // ================= TRƯỜNG HỢP THÊM MỚI =================

                    // 1. Kiểm tra trùng Mã SV
                    string sqlCheckSV = "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @ma";
                    // Tạo tham số riêng cho việc check
                    SqlParameter[] pCheck = { new SqlParameter("@ma", txtMaSV.Text.Trim()) };

                    DataTable dtCheck = CoSoDuLieu.LayDuLieu(sqlCheckSV, pCheck);
                    if (dtCheck.Rows.Count > 0 && int.Parse(dtCheck.Rows[0][0].ToString()) > 0)
                    {
                        MessageBox.Show("Mã sinh viên này đã tồn tại!", "Cảnh báo");
                        return;
                    }

                    // 2. Thêm vào bảng SINHVIEN
                    string sqlInsert = "INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, MaLop, DiaChi, SoDienThoai) VALUES (@ma, @ten, @ns, @gt, @lop, @dc, @sdt)";

                    // Tạo bộ tham số MỚI TINH cho lệnh Insert này
                    SqlParameter[] pInsert = new SqlParameter[]
                    {
                new SqlParameter("@ma", txtMaSV.Text.Trim()),
                new SqlParameter("@ten", txtHoTen.Text.Trim()),
                new SqlParameter("@ns", dtpNgaySinh.Value),
                new SqlParameter("@gt", gioitinh),
                new SqlParameter("@lop", cboLop.SelectedValue ?? DBNull.Value),
                new SqlParameter("@dc", txtDiaChi.Text.Trim()),
                new SqlParameter("@sdt", txtSDT.Text.Trim())
                    };
                    CoSoDuLieu.ThucThiLenh(sqlInsert, pInsert);

                    // 3. Xử lý Tài khoản (Kiểm tra và Thêm)
                    string sqlCheckTK = "SELECT COUNT(*) FROM DangNhap WHERE TaiKhoan = @ma";
                    // Lại tạo tham số mới cho lệnh check TK
                    SqlParameter[] pCheckTK = { new SqlParameter("@ma", txtMaSV.Text.Trim()) };
                    DataTable dtCheckTK = CoSoDuLieu.LayDuLieu(sqlCheckTK, pCheckTK);

                    if (int.Parse(dtCheckTK.Rows[0][0].ToString()) == 0)
                    {
                        string sqlTK = "INSERT INTO DangNhap (TaiKhoan, MatKhau, HoTen, Quyen) VALUES (@ma, '123', @ten, 'SinhVien')";

                        // Tạo tham số mới cho lệnh thêm TK
                        SqlParameter[] pTK = {
                    new SqlParameter("@ma", txtMaSV.Text.Trim()),
                    new SqlParameter("@ten", txtHoTen.Text.Trim())
                };
                        CoSoDuLieu.ThucThiLenh(sqlTK, pTK);
                    }
                }
                else
                {
                    // ================= TRƯỜNG HỢP CẬP NHẬT (SỬA) =================

                    string sqlUpdate = "UPDATE SinhVien SET HoTen=@ten, NgaySinh=@ns, GioiTinh=@gt, MaLop=@lop, DiaChi=@dc, SoDienThoai=@sdt WHERE MaSV=@ma";

                    // Tạo bộ tham số MỚI cho lệnh Update
                    SqlParameter[] pUpdate = new SqlParameter[]
                    {
                new SqlParameter("@ma", txtMaSV.Text.Trim()),
                new SqlParameter("@ten", txtHoTen.Text.Trim()),
                new SqlParameter("@ns", dtpNgaySinh.Value),
                new SqlParameter("@gt", gioitinh),
                new SqlParameter("@lop", cboLop.SelectedValue ?? DBNull.Value),
                new SqlParameter("@dc", txtDiaChi.Text.Trim()),
                new SqlParameter("@sdt", txtSDT.Text.Trim())
                    };
                    CoSoDuLieu.ThucThiLenh(sqlUpdate, pUpdate);

                    // Cập nhật tên bên Tài khoản
                    string sqlUpdateTK = "UPDATE DangNhap SET HoTen=@ten WHERE TaiKhoan=@ma";
                    SqlParameter[] pUpdateTK = {
                new SqlParameter("@ma", txtMaSV.Text.Trim()),
                new SqlParameter("@ten", txtHoTen.Text.Trim())
            };
                    CoSoDuLieu.ThucThiLenh(sqlUpdateTK, pUpdateTK);
                }

                MessageBox.Show("Lưu thành công!", "Thông báo");
                LoadData();
                ResetGiaoDien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // --- NÚT XÓA ---
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "") return;
            if (MessageBox.Show("Xóa sinh viên này? (Tài khoản cũng sẽ bị xóa)", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Tạo tham số mã SV
                    SqlParameter[] p = { new SqlParameter("@ma", txtMaSV.Text) };

                    // 1. Xóa Tài khoản trước
                    CoSoDuLieu.ThucThiLenh("DELETE FROM DangNhap WHERE TaiKhoan=@ma", p);

                    // 2. Xóa Sinh viên sau (Cần tạo lại mảng tham số mới để tránh lỗi đã dùng)
                    SqlParameter[] p2 = { new SqlParameter("@ma", txtMaSV.Text) };
                    CoSoDuLieu.ThucThiLenh("DELETE FROM SinhVien WHERE MaSV=@ma", p2);

                    MessageBox.Show("Đã xóa!");
                    LoadData();
                    ResetGiaoDien();
                }
                catch { MessageBox.Show("Không xóa được (Có thể do ràng buộc điểm)!"); }
            }
        }

        // --- NÚT TÌM KIẾM ---
        private void btnTim_Click(object sender, EventArgs e)
        {
            string tuKhoa = "%" + txtTimKiem.Text.Trim() + "%";

            // Tìm theo Mã hoặc Tên
            string sql = @"SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, l.TenLop, sv.SoDienThoai, sv.DiaChi, sv.MaLop 
                           FROM SinhVien sv LEFT JOIN Lop l ON sv.MaLop = l.MaLop
                           WHERE sv.HoTen LIKE @kw OR sv.MaSV LIKE @kw";

            SqlParameter[] p = { new SqlParameter("@kw", tuKhoa) };

            dgvSinhVien.DataSource = CoSoDuLieu.LayDuLieu(sql, p);
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
    
