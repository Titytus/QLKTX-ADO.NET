﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLKTX.DB;

namespace QLKTX.BS
{
    public partial class BS_layer
    {
        public DB_Main db = null;

        public BS_layer()
        {
            db = new DB_Main();
        }

        public enum TableName
        {
            SinhVien,
            NhanVien,
            PhieuDK,
            HoaDon,
            Phong,
            DichVu,
            LoaiPhong,
            SDDV,
            Stay
        }

        public DataTable Select(ref string error, TableName table, object selectType, string strValue = "")
        {
            string strTableName = table.ToString();
            string strType = selectType.ToString();
            string sql = "";
            SqlParameter[] sqlParameters = new SqlParameter[] { };
            switch (selectType)
            {
                case EnumConst.NhanVien.All:
                case EnumConst.SinhVien.All:
                case EnumConst.Phong.All:
                case EnumConst.PhieuDK.All:
                case EnumConst.HoaDon.All:
                case EnumConst.DichVu.All:
                case EnumConst.LoaiPhong.All:
                    sql = $"SELECT * FROM {strTableName}";
                    break;
                default:
                    strValue = "%" + strValue + "%";
                    sql = $"SELECT * FROM {strTableName} WHERE {strType} LIKE @Value";
                    sqlParameters = new SqlParameter[]
                    {
                        new SqlParameter("Value", strValue)
                    };
                    break;
            }
            return db.ExecuteQuery(sql, sqlParameters, CommandType.Text, ref error);
        }

        public bool Delete(TableName tableName, string strKey, ref string error)
        {
            string strTableName = tableName.ToString();
            string key = "";
            switch (tableName)
            {
                case TableName.SinhVien:    key = "MSSV"; break;
                case TableName.NhanVien:    key = "MaNV"; break;
                case TableName.HoaDon:      key = "MaHD"; break;
                case TableName.PhieuDK:     key = "MaPDK";break;
                case TableName.LoaiPhong:   key = "MaLoaiPhong"; break;
                case TableName.DichVu:      key = "MaDV"; break;
                case TableName.SDDV:        key = "MaHD"; break;
                case TableName.Stay:        key = "MSSV"; break;
            }    
            string sql = $"DELETE FROM {strTableName} WHERE {key} = @Key";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("Key", strKey)
            };
            return db.ExecuteNonQuery(sql, sqlParameters, CommandType.Text, ref error);
        }
    }
}
