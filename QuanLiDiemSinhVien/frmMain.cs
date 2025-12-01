using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
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
            //panelDesktop.Controls.Clear();

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

        private void btnDiem_Click(object sender, EventArgs e)
        {
            if (this.quyenHan == "GiaoVien")
            {
                OpenChildForm(new frmQuanLyDiem());
            }
            else if (this.quyenHan == "SinhVien")
            {
                // truyền tài khoản (MaSV) để chỉ hiển thị điểm của chính sinh viên đó
                OpenChildForm(new frmQuanLyDiem(this.taiKhoan));
            }
            else
            {
                MessageBox.Show("Lỗi phân quyền! Quyền hiện tại: " + this.quyenHan);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (traloi == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close(); 
            }
        }

        private void btnMonHoc_Click(object sender, EventArgs e)
        {
            if (this.quyenHan == "GiaoVien")
            {
                OpenChildForm(new FormMonHoc());
            }
            else if (this.quyenHan == "SinhVien")
            {
                // truyền tài khoản (MaSV) để chỉ hiển thị điểm của chính sinh viên đó
                OpenChildForm(new FormMonHocSV(this.taiKhoan));
            }
            else
            {
                MessageBox.Show("Lỗi phân quyền! Quyền hiện tại: " + this.quyenHan);
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (this.quyenHan == "GiaoVien")
            {
                MessageBox.Show("Chức năng này chỉ dành cho Sinh viên!");
                return;
            }
            OpenChildForm(new frmDangKyMon(this.taiKhoan));
        }
        

        private void frmMain_Load(object sender, EventArgs e)
        {
            VeBieuDo();
            LoadDashboard();
        }
        void VeBieuDo()
        {
            // --- BIỂU ĐỒ 1: CỘT (Thống kê theo Khoa) ---
            chartCot.Series.Clear(); 
            Series s1 = new Series("Sinh Viên");
            s1.ChartType = SeriesChartType.Column; 

            // Thêm dữ liệu mẫu (Sau này thay bằng SQL)
            s1.Points.AddXY("CNTT", 45);
            s1.Points.AddXY("Kinh Tế", 30);
            s1.Points.AddXY("Ngoại Ngữ", 25);
            s1.Points.AddXY("Sư Phạm", 15);

            // Hiển thị số liệu trên đầu cột
            s1.IsValueShownAsLabel = true;

            chartCot.Series.Add(s1);

            // --- BIỂU ĐỒ 2: TRÒN (Tỷ lệ Nam/Nữ) ---
            chartTron.Series.Clear();
            Series s2 = new Series("GioiTinh");
            s2.ChartType = SeriesChartType.Doughnut; 

            s2.Points.AddXY("Nam", 60);
            s2.Points.AddXY("Nữ", 40);

            s2.IsValueShownAsLabel = true; 
            s2.Label = "#PERCENT";         
            s2.LegendText = "#AXISLABEL";  

            chartTron.Series.Add(s2);
        }
        void LoadDashboard()
        {
            try
            {
                // =============================================================
                // PHẦN 1: CẬP NHẬT CÁC Ô SỐ LIỆU (CARDS)
                // =============================================================

                // 1. Tổng Sinh Viên
                DataTable dtSV = CoSoDuLieu.LayDuLieu("SELECT COUNT(*) FROM SinhVien");
                if (dtSV.Rows.Count > 0)
                    lblTongSV.Text = dtSV.Rows[0][0].ToString();

                // 2. Tổng Số Lớp
                DataTable dtLop = CoSoDuLieu.LayDuLieu("SELECT COUNT(*) FROM Lop");
                if (dtLop.Rows.Count > 0)
                    lblTongLop.Text = dtLop.Rows[0][0].ToString();

                // 3. Tổng Môn Học
                DataTable dtMon = CoSoDuLieu.LayDuLieu("SELECT COUNT(*) FROM MonHoc");
                if (dtMon.Rows.Count > 0)
                    lblTongMon.Text = dtMon.Rows[0][0].ToString();

                // 4. Sinh viên có học bổng (Ví dụ: Có ít nhất 1 môn >= 8.5)
                string sqlHB = "SELECT COUNT(DISTINCT MaSV) FROM Diem WHERE DiemThi >= 8.5";
                DataTable dtHB = CoSoDuLieu.LayDuLieu(sqlHB);
                if (dtHB.Rows.Count > 0)
                    lblHocBong.Text = dtHB.Rows[0][0].ToString();


                // =============================================================
                // PHẦN 2: CẬP NHẬT BIỂU ĐỒ (CHARTS)
                // =============================================================

                // --- A. BIỂU ĐỒ CỘT: Thống kê Sinh viên theo Khoa ---
                if (chartCot.Series.IndexOf("Sinh Viên") != -1)
                    chartCot.Series.Clear(); // Xóa cũ để vẽ mới

                Series s1 = new Series("Sinh Viên");
                s1.ChartType = SeriesChartType.Column; // Dạng cột

                // SQL: Đếm SV theo Khoa (Dựa vào bảng Lop)
                string sqlCot = @"SELECT l.Khoa, COUNT(sv.MaSV) as SoLuong 
                          FROM SinhVien sv 
                          JOIN Lop l ON sv.MaLop = l.MaLop 
                          GROUP BY l.Khoa";

                DataTable dtCot = CoSoDuLieu.LayDuLieu(sqlCot);

                foreach (DataRow row in dtCot.Rows)
                {
                    string tenKhoa = row["Khoa"].ToString();
                    int soLuong = int.Parse(row["SoLuong"].ToString());

                    s1.Points.AddXY(tenKhoa, soLuong);
                }

                s1.IsValueShownAsLabel = true; // Hiện số trên đầu cột
                chartCot.Series.Add(s1);


                // --- B. BIỂU ĐỒ TRÒN: Tỷ lệ Nam/Nữ ---
                if (chartTron.Series.IndexOf("GioiTinh") != -1)
                    chartTron.Series.Clear();

                Series s2 = new Series("GioiTinh");
                s2.ChartType = SeriesChartType.Doughnut; // Dạng bánh Donut

                // SQL: Đếm theo Giới tính
                string sqlTron = "SELECT GioiTinh, COUNT(*) as SoLuong FROM SinhVien GROUP BY GioiTinh";
                DataTable dtTron = CoSoDuLieu.LayDuLieu(sqlTron);

                foreach (DataRow row in dtTron.Rows)
                {
                    string gioiTinh = row["GioiTinh"].ToString();
                    int soLuong = int.Parse(row["SoLuong"].ToString());

                    int i = s2.Points.AddXY(gioiTinh, soLuong);
                    s2.Points[i].Label = "#PERCENT"; // Hiện phần trăm
                    s2.Points[i].LegendText = gioiTinh; // Hiện chú thích
                }

                chartTron.Series.Add(s2);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                currentChildForm = null; // Reset biến này
            }

            // 2. Đặt lại tiêu đề
            lblTitle.Text = "TRANG CHỦ";

            // 3. Cập nhật lại số liệu mới nhất (Load lại dữ liệu cho nóng hổi)
            LoadDashboard();
        }
    }
}