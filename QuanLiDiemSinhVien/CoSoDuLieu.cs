using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiDiemSinhVien
{
    internal class CoSoDuLieu
    {
        public static string chuoiKetNoi = @"Data Source=.;Initial Catalog=QuanLyDiemSinhVien;Integrated Security=True";

        public static DataTable LayDuLieu(string cauLenh)
        {
            SqlConnection ketNoi = new SqlConnection(chuoiKetNoi);
            SqlDataAdapter boThichUng = new SqlDataAdapter(cauLenh, ketNoi);
            DataTable bang = new DataTable();
            boThichUng.Fill(bang);
            return bang;
        }

        // Hàm thực thi lệnh (INSERT, UPDATE, DELETE)
        public static void ThucThiLenh(string cauLenh)
        {
            SqlConnection ketNoi = new SqlConnection(chuoiKetNoi);
            ketNoi.Open();
            SqlCommand lenh = new SqlCommand(cauLenh, ketNoi);
            lenh.ExecuteNonQuery();
            ketNoi.Close();
        }
    }
}
