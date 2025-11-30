using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiDiemSinhVien
{
    public partial class frmMain : Form
    {
        private Form currentChildForm;
        private string quyenHan;
        private string taiKhoan;
        private string hoTen;
        public frmMain(string quyen, string tk, string ten)
        {
           
            InitializeComponent();
            this.quyenHan = quyen;
            this.taiKhoan = tk;
            this.hoTen = ten;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;

            // Chỉ clear panelDesktop, giữ menu và title bar
            panelDesktop.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelDesktop.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();

            lblTitle.Text = childForm.Text;
        }
        private void btnSinhVien_Click(object sender, EventArgs e)
        {
            if (this.quyenHan == "GiaoVien")
            {
                OpenChildForm(new frmSinhVien());
            }
            else if (this.quyenHan == "SinhVien")
            {
                OpenChildForm(new frmThongTinCaNhan(this.taiKhoan));
            }
            else
            {
                // Trường hợp quyền bị sai hoặc rỗng
                MessageBox.Show("Lỗi phân quyền! Quyền hiện tại: " + this.quyenHan);
            }
            }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }

            // Reset lại tiêu đề
            lblTitle.Text = "TRANG CHỦ";

            // Nạp lại Dashboard (Bạn phải viết hàm tạo lại 4 ô màu, hoặc đơn giản là mở lại FormMain mới)
            // Cách đơn giản nhất cho người mới:
            // Ẩn form hiện tại đi và mở form mới
            
            /*this.Hide();
            frmMain moi = new frmMain(this.quyenHan, this.taiKhoan, this.hoTen);
            moi.ShowDialog();
            this.Close();*/
        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelDesktop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    }
