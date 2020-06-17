﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace QLKTX.DB
{
    public class DB_Main
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;

        // Đối tượng kết nối
        readonly SqlConnection conn = null;
        readonly SqlCommand comm = null;
        SqlDataAdapter da = null;

        public DB_Main()
        {
            conn = new SqlConnection(connectionString);
            comm = conn.CreateCommand();
        }

        public DataTable ExecuteQuery(string strSQL, SqlParameter[] sqlParameters, CommandType ct)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.Parameters.AddRange(sqlParameters);
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            da = new SqlDataAdapter(comm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public bool ExecuteNonQuery(string strSQL, SqlParameter[] sqlParameters, CommandType ct, ref string error)
        {
            bool f = false;
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            comm.Parameters.AddRange(sqlParameters);
            try
            {
                comm.ExecuteNonQuery();
                f = true;
                error = "";
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return f;
        }
    }
}
