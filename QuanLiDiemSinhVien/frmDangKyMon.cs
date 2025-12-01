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
    public partial class frmDangKyMon : Form
    {
        string maSV = "";
        public frmDangKyMon(string msv)
        {
            InitializeComponent();
            this.maSV = msv;
        }

        private void frmDangKyMon_Load(object sender, EventArgs e)
        {
            LoadMonChuaDangKy();
            LoadMonDaDangKy();
        }
        void LoadMonChuaDangKy()
        {
            try
            {
                // Logic SQL: Lấy tất cả môn trong bảng MonHoc
                // TRỪ ĐI (NOT IN) những môn đã có trong bảng Diem của sinh viên này
                string sql = @"SELECT MaMH, TenMH, SoTinChi, HocKy 
                               FROM MonHoc 
                               WHERE MaMH NOT IN (SELECT MaMH FROM Diem WHERE MaSV = @ma)";

                SqlParameter[] p = { new SqlParameter("@ma", maSV) };
                dgvDSMonHoc.DataSource = CoSoDuLieu.LayDuLieu(sql, p);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải DS môn: " + ex.Message); }
        }
        void LoadMonDaDangKy()
        {
            try
            {
                // Logic SQL: Kết nối bảng Diem và MonHoc để lấy tên môn
                string sql = @"SELECT d.MaMH, mh.TenMH, mh.SoTinChi, mh.HocKy 
                               FROM Diem d 
                               JOIN MonHoc mh ON d.MaMH = mh.MaMH 
                               WHERE d.MaSV = @ma";

                SqlParameter[] p = { new SqlParameter("@ma", maSV) };
                DataTable dt = CoSoDuLieu.LayDuLieu(sql, p);
                dgvDaDangKy.DataSource = dt;

                // Tính tổng tín chỉ và hiện lên Label
                TinhTongTinChi(dt);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải môn đã ĐK: " + ex.Message); }
        }
        void TinhTongTinChi(DataTable dt)
        {
            int tong = 0;
            foreach (DataRow row in dt.Rows)
            {
                // Kiểm tra null để tránh lỗi
                if (row["SoTinChi"] != DBNull.Value)
                    tong += int.Parse(row["SoTinChi"].ToString());
            }
            lblTongTinChi.Text = "Tổng số tín chỉ đã đăng ký: " + tong;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (dgvDSMonHoc.SelectedRows.Count > 0)
            {
                // Lấy Mã môn từ dòng đang chọn bên trái
                string maMH = dgvDSMonHoc.SelectedRows[0].Cells["MaMH"].Value.ToString();

                try
                {
                    // Thêm vào bảng Diem (Chỉ cần Mã SV và Mã Môn, điểm để trống)
                    string sql = "INSERT INTO Diem (MaSV, MaMH) VALUES (@sv, @mh)";

                    SqlParameter[] p = {
                        new SqlParameter("@sv", maSV),
                        new SqlParameter("@mh", maMH)
                    };

                    CoSoDuLieu.ThucThiLenh(sql, p);

                    MessageBox.Show("Đăng ký môn học thành công!");

                    // Tải lại cả 2 bảng để cập nhật danh sách
                    LoadMonChuaDangKy();
                    LoadMonDaDangKy();
                }
                catch { MessageBox.Show("Lỗi đăng ký! Có thể môn này đã tồn tại."); }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một môn bên danh sách trái để đăng ký!");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (dgvDaDangKy.SelectedRows.Count > 0)
            {
                string maMH = dgvDaDangKy.SelectedRows[0].Cells["MaMH"].Value.ToString();

                if (MessageBox.Show("Bạn muốn hủy môn học này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        // Xóa khỏi bảng Diem
                        string sql = "DELETE FROM Diem WHERE MaSV = @sv AND MaMH = @mh";

                        SqlParameter[] p = {
                            new SqlParameter("@sv", maSV),
                            new SqlParameter("@mh", maMH)
                        };

                        CoSoDuLieu.ThucThiLenh(sql, p);

                        MessageBox.Show("Đã hủy môn học!");

                        LoadMonChuaDangKy();
                        LoadMonDaDangKy();
                    }
                    catch { MessageBox.Show("Lỗi hủy môn! Có thể môn này đã có điểm số."); }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn môn bên phải để hủy!");
            }
        }
    }
}
