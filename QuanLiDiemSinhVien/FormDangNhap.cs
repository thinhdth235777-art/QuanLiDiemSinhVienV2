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
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập liệu
            if (txtTaiKhoan.Text == "" || txtMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. Kiểm tra trong CSDL (Dùng tham số @ để an toàn)
                string sql = "SELECT * FROM DangNhap WHERE TaiKhoan = @tk AND MatKhau = @mk";

                SqlParameter[] p = new SqlParameter[] {
            new SqlParameter("@tk", txtTaiKhoan.Text.Trim()),
            new SqlParameter("@mk", txtMatKhau.Text.Trim())
        };

                DataTable dt = CoSoDuLieu.LayDuLieu(sql, p);

                if (dt.Rows.Count > 0)
                {
                    string quyen = dt.Rows[0]["Quyen"].ToString();
                    string tk = dt.Rows[0]["TaiKhoan"].ToString();
                    string ten = dt.Rows[0]["HoTen"].ToString();

                    MessageBox.Show("Đăng nhập thành công! Xin chào " + ten, "Thông báo");
                    frmMain f = new frmMain(quyen, tk, ten);
                    this.Hide(); 

                    DialogResult result = f.ShowDialog();

                    if (result == DialogResult.OK)
                    {    
                        this.Show();          
                        txtMatKhau.Text = ""; 
                        txtTaiKhoan.Focus();  
                    }
                    else
                    {
                        Application.Exit();   
                    }
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {    
            if (chkHienMatKhau.Checked)
            {
                txtMatKhau.PasswordChar = '\0'; 
            }
            else
            {
                txtMatKhau.PasswordChar = '*';  
            }
        }

        private void lnkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormĐăngKí f = new FormĐăngKí();           
            this.Hide();            
            f.ShowDialog();
            this.Show();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }
    }
}

