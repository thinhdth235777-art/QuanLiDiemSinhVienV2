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
        string chuoiKetNoi = @"Data Source=MAY02\SQLEXPRESS;Initial Catalog=QuanLyDiemSinhVien;Integrated Security=True;TrustServerCertificate=True";
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text == "" || txtMatKhau.Text == "")
            {
                MessageBox.Show("Nhập thiếu thông tin!");
                return;
            }

            try
            {
                
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();
                    string sql = "SELECT * FROM DangNhap WHERE TaiKhoan = @acc AND MatKhau = @pass";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@acc", txtTaiKhoan.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txtMatKhau.Text.Trim());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Đăng nhập thành công!");

                        
                        string quyen = dt.Rows[0]["Quyen"].ToString();
                        string tk = dt.Rows[0]["TaiKhoan"].ToString();
                        string ten = dt.Rows[0]["HoTen"].ToString();

                        frmMain f = new frmMain(quyen, tk, ten);
                        this.Hide();
                        f.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                    }
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
    }
}

