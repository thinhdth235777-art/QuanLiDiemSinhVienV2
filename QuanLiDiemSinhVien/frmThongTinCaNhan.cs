using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiDiemSinhVien
{
    public partial class frmThongTinCaNhan : Form
    {
        string maSVHienTai = "";
        string chuoiKetNoi = @"Data Source=MAY02\SQLEXPRESS;Initial Catalog=QuanLyDiemSinhVien;Integrated Security=True;TrustServerCertificate=True";
        public frmThongTinCaNhan(string maSV)
        {
            InitializeComponent();
            this.maSVHienTai = maSV;
        }

        private void frmThongTinCaNhan_Load(object sender, EventArgs e)
        {
            TaiThongTinSinhVien();
        }
        void TaiThongTinSinhVien()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();
                    string sql = @"SELECT sv.*, l.TenLop 
                                   FROM SinhVien sv 
                                   LEFT JOIN Lop l ON sv.MaLop = l.MaLop
                                   WHERE sv.MaSV = @ma";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maSVHienTai);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read()) 
                    {
                        txtMaSV.Text = dr["MaSV"].ToString();
                        txtHoTen.Text = dr["HoTen"].ToString();
                    
                        if (dr["NgaySinh"] != DBNull.Value)
                            dtpNgaySinh.Value = Convert.ToDateTime(dr["NgaySinh"]);

                        string gioitinh = dr["GioiTinh"].ToString();
                        if (gioitinh == "Nam")
                        {
                            rdoNam.Checked = true;
                        }
                        else
                        {
                            rdoNu.Checked = true;
                        }
                        txtLop.Text = dr["TenLop"].ToString(); 

                        txtSDT.Text = dr["SoDienThoai"].ToString();
                        txtEmail.Text = dr["Email"].ToString();
                        txtDiaChi.Text = dr["DiaChi"].ToString();

                    }
                    if (dr["AnhThe"] != DBNull.Value) 
                    {
                        byte[] imgData = (byte[])dr["AnhThe"];

                        using (MemoryStream ms = new MemoryStream(imgData))
                        {
                            picAvatar.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        picAvatar.Image = null; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin: " + ex.Message);
            }
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();

                    string sql = @"UPDATE SinhVien 
                           SET SoDienThoai = @sdt, 
                               Email = @email, 
                               DiaChi = @dc, 
                               AnhThe = @anh 
                           WHERE MaSV = @ma";

                    SqlCommand cmd = new SqlCommand(sql, conn);
               
                    cmd.Parameters.AddWithValue("@sdt", txtSDT.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@dc", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@ma", maSVHienTai);

                    if (picAvatar.Image != null)
                    {
                        cmd.Parameters.AddWithValue("@anh", ChuyenAnhSangByte(picAvatar.Image));
                    }
                    else
                    {
                        cmd.Parameters.Add("@anh", SqlDbType.VarBinary).Value = DBNull.Value;
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật hồ sơ và ảnh đại diện thành công!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Tệp hình ảnh|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            od.Title = "Chọn ảnh đại diện";

            if (od.ShowDialog() == DialogResult.OK)
            {
                string fileAnh = od.FileName;
                picAvatar.Image = Image.FromFile(fileAnh);
                picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            }    
        }
        private byte[] ChuyenAnhSangByte(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {       
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
