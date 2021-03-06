﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX.BS
{
    public partial class BS_layer
    {
        public bool Insert(string MSSV, string MaNV, string Khu, string MaPhong, string HocKi, string NamHoc,
            DateTime NgayGioDK, int ThoiHan, string NgayBD, ref int identity, ref string error)
        {
            string sql = "INSERT INTO PHIEUDK VALUES(@MSSV, @MaNV, @Khu, @MaPhong, @HocKi, @NamHoc, @NgayGioDK, @ThoiHan, @NgayBD) " +
                            "SELECT SCOPE_IDENTITY();";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("MSSV", MSSV),
                new SqlParameter("MaNV", MaNV),
                new SqlParameter("Khu", Khu),
                new SqlParameter("MaPhong", MaPhong),
                new SqlParameter("HocKi", HocKi),
                new SqlParameter("NamHoc", NamHoc),
                new SqlParameter("NgayGioDK", NgayGioDK),
                new SqlParameter("ThoiHan", ThoiHan),
                new SqlParameter("NgayBD", NgayBD)
            };
            return db.SelectScopeIdentity(sql, sqlParameters, CommandType.Text, ref identity, ref error);
        }

        public bool Update(string MaPDK, string MSSV, string MaNV, string Khu, string MaPhong, string HocKi, string NamHoc, 
            DateTime NgayGioDK, int ThoiHan, string NgayBD, ref string error)
        {
            string sql = "UPDATE PHIEUDK SET " +
                            "MSSV = @MSSV, " +
                            "MaNV = @MaNV, " +
                            "Khu = @Khu, " +
                            "MaPhong = @MaPhong, " +
                            "HocKi = @HocKi, " +
                            "NamHoc = @NamHoc, " +
                            "NgayGioDK = @NgayGioDK, " +
                            "ThoiHan = @ThoiHan, " +
                            "NgayBD = @NgayBD" +
                        "WHERE MaPDK = @MaPDK";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("MaPDK", MaPDK),
                new SqlParameter("MSSV", MSSV),
                new SqlParameter("MaNV", MaNV),
                new SqlParameter("Khu", Khu),
                new SqlParameter("MaPhong", MaPhong),
                new SqlParameter("HocKi", HocKi),
                new SqlParameter("NamHoc", NamHoc),
                new SqlParameter("NgayGioDK", NgayGioDK),
                new SqlParameter("ThoiHan", ThoiHan),
                new SqlParameter("NgayBD", NgayBD)
            };
            return db.ExecuteNonQuery(sql, sqlParameters, System.Data.CommandType.Text, ref error);
        }

        public DataTable LayDSPhongTrong(ref string error)
        {
            string sql =
            " SELECT PHONG.Khu AS Khu, PHONG.MaPhong AS MaPhong " +
            " FROM PHONG, LOAIPHONG " +
            " EXCEPT " +
            " SELECT TMP.Khu, TMP.MaPhong FROM " +
            "    (SELECT STAY.Khu, STAY.MaPhong, COUNT(STAY.MSSV) AS SV, SoSV " +
            "        FROM " +
            "            STAY INNER JOIN PHONG ON STAY.Khu= PHONG.Khu AND STAY.MaPhong= PHONG.MaPhong " +
            "                 INNER JOIN LOAIPHONG ON LOAIPHONG.MaLoaiPhong= PHONG.LoaiPhong " +
            "        GROUP BY STAY.Khu, STAY.MaPhong, SoSV " +
            "     ) AS TMP " +
            " WHERE TMP.SV = TMP.SoSV;";
            SqlParameter[] sqlParameters= new SqlParameter[] { };
            return db.ExecuteQuery(sql, sqlParameters, CommandType.Text, ref error);

            //return db.ExecuteStoredProcedure("DS_PHONG_TRONG", ref error);
        }
    }
}
