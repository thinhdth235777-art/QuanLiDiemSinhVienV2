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
    public partial class frmDoiMatKhau : Form
    {
        string taiKhoanHienTai = "";
        public frmDoiMatKhau(string taiKhoan)
        {
            InitializeComponent();
            this.taiKhoanHienTai = taiKhoan;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMatKhauCu.Text == "" || txtMatKhauMoi.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            if (txtMatKhauMoi.Text != txtXacNhan.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi");
                return;
            }

            try
            {
                string sqlCheck = "SELECT COUNT(*) FROM DangNhap WHERE TaiKhoan = @tk AND MatKhau = @mkCu";
                SqlParameter[] pCheck = {
                    new SqlParameter("@tk", taiKhoanHienTai),
                    new SqlParameter("@mkCu", txtMatKhauCu.Text)
                };

                DataTable dt = CoSoDuLieu.LayDuLieu(sqlCheck, pCheck);

                if (dt.Rows[0][0].ToString() == "0")
                {
                    MessageBox.Show("Mật khẩu cũ không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string sqlUpdate = "UPDATE DangNhap SET MatKhau = @mkMoi WHERE TaiKhoan = @tk";
                SqlParameter[] pUpdate = {
                    new SqlParameter("@mkMoi", txtMatKhauMoi.Text),
                    new SqlParameter("@tk", taiKhoanHienTai)
                };

                CoSoDuLieu.ThucThiLenh(sqlUpdate, pUpdate);

                MessageBox.Show("Đổi mật khẩu thành công! Vui lòng ghi nhớ mật khẩu mới.");
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHienMatKhau.Checked)
            {
                // Hiện chữ bình thường (Dùng ký tự rỗng '\0')
                txtMatKhauCu.PasswordChar = '\0';
                txtMatKhauMoi.PasswordChar = '\0';
                txtXacNhan.PasswordChar = '\0';
            }
            else
            {
                // Ẩn thành dấu sao
                txtMatKhauCu.PasswordChar = '*';
                txtMatKhauMoi.PasswordChar = '*';
                txtXacNhan.PasswordChar = '*';
            }
        }
    }
}
