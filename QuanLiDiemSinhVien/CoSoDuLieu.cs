using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiDiemSinhVien
{
    internal class CoSoDuLieu
    {
        public static string chuoiKetNoi = @"Data Source=.;Initial Catalog=QuanLyDiemSinhVien;Integrated Security=True";

        public static DataTable LayDuLieu(string cauLenh)
        {
            return LayDuLieu(cauLenh, null);
        }

        public static DataTable LayDuLieu(string cauLenh, SqlParameter[] parameters)
        {
            using (SqlConnection ketNoi = new SqlConnection(chuoiKetNoi))
            using (SqlCommand cmd = new SqlCommand(cauLenh, ketNoi))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable bang = new DataTable();
                    da.Fill(bang);
                    return bang;
                }
            }
        }

        // Hàm thực thi lệnh (INSERT, UPDATE, DELETE)
        public static void ThucThiLenh(string cauLenh)
        {
            ThucThiLenh(cauLenh, null);
        }

        public static void ThucThiLenh(string cauLenh, SqlParameter[] parameters)
        {
            using (SqlConnection ketNoi = new SqlConnection(chuoiKetNoi))
            {
                ketNoi.Open();
                using (SqlCommand lenh = new SqlCommand(cauLenh, ketNoi))
                {
                    if (parameters != null && parameters.Length > 0)
                        lenh.Parameters.AddRange(parameters);
                    lenh.ExecuteNonQuery();
                }
            }
        }
        public static DataTable LayHocBong()
        {
            return LayDuLieu("SELECT * FROM HocBong ORDER BY DiemGPA DESC");
        }

        public static DataTable LayHocBongTheoLop(string maLop)
        {
            string sql = "SELECT * FROM HocBong WHERE MaLop = @MaLop ORDER BY DiemGPA DESC";
            return LayDuLieu(sql, new SqlParameter[] { new SqlParameter("@MaLop", maLop) });
        }
    }
}
