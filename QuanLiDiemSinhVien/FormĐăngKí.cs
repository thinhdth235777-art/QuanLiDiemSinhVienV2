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
    public partial class FormĐăngKí : Form
    {
        string chuoiKetNoi = @"Data Source=.;Initial Catalog=QuanLyDiemSinhVien;Integrated Security=True;TrustServerCertificate=True";
        public FormĐăngKí()
        {
            InitializeComponent();
        }

        private void btnĐK_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập thiếu
            if (txtTaiKhoanDK.Text == "" || txtMatKhauDK.Text == "" || txtHoTenDK.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Nhắc nhở");
                return;
            }

            // 2. Kiểm tra mật khẩu nhập lại
            if (txtMatKhauDK.Text != txtXacNhanMK.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi nhập liệu");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();

                    // 3. Kiểm tra xem tài khoản đã tồn tại chưa (Tránh trùng)
                    string sqlCheck = "SELECT COUNT(*) FROM DangNhap WHERE TaiKhoan = @acc";
                    SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
                    cmdCheck.Parameters.AddWithValue("@acc", txtTaiKhoanDK.Text);

                    int ketQua = (int)cmdCheck.ExecuteScalar(); // Trả về số lượng tìm thấy

                    if (ketQua > 0)
                    {
                        MessageBox.Show("Tài khoản này đã có người sử dụng!", "Trùng tài khoản");
                        return;
                    }

                    // 4. Nếu chưa trùng -> Thêm mới vào SQL
                    // Mặc định Quyền là 'GiaoVien' (Vì Sinh viên sẽ được tạo tự động bên form khác)
                    string sqlInsert = "INSERT INTO DangNhap (TaiKhoan, MatKhau, HoTen, Quyen) VALUES (@acc, @pass, @name, 'GiaoVien')";

                    SqlCommand cmd = new SqlCommand(sqlInsert, conn);
                    cmd.Parameters.AddWithValue("@acc", txtTaiKhoanDK.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txtMatKhauDK.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", txtHoTenDK.Text);

                    cmd.ExecuteNonQuery(); // Thực thi lệnh Insert

                    MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập ngay.", "Chúc mừng");

                    this.Close(); // Đóng form Đăng ký để quay về Đăng nhập
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn hủy đăng ký?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}

