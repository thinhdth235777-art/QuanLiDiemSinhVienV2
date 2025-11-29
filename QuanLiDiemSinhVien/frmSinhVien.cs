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
    public partial class frmSinhVien : Form
    {
        string chuoiKetNoi = @"Data Source=MAY02\SQLEXPRESS;Initial Catalog=QuanLyDiemSinhVien;Integrated Security=True;TrustServerCertificate=True";
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboLop();
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }
        void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();

                    string sql = @"SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, l.TenLop, sv.SoDienThoai, sv.DiaChi 
                                   FROM SinhVien sv 
                                   LEFT JOIN Lop l ON sv.MaLop = l.MaLop";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvSinhVien.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        void LoadComboLop()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MaLop, TenLop FROM Lop", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboLop.DataSource = dt;
                    cboLop.DisplayMember = "TenLop"; 
                    cboLop.ValueMember = "MaLop";    
                    cboLop.SelectedIndex = -1;       
                }
            }
            catch { }
        }
    }
}
