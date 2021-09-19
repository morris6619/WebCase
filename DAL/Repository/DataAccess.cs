﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Repository
{
    public class DataAccess : IDisposable
    {
        // TODO: use connection string ExamEntities
        // http://stackoverflow.com/questions/4805094/pass-connection-string-to-code-first-dbcontext
        public NLog.Logger Logger;
        private bool disposed = false;

        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataAdapter da;

        public DataAccess()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            da = new SqlDataAdapter(cmd);
        }

        public SqlConnection getConn()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            return conn;
        }

        public DataSet ExecuteDataSet()
        {
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        /// <summary>
        /// 轉換DateTime?為民國字串 (民國xxx年xx月xx日)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string ConvertTWDate(DateTime? dt)
        {
            return (dt == null) ? "" : "民國" + (dt.Value.Year - 1911).ToString() + "年" + dt.Value.ToString("MM月dd日");
        }


        public int SelectIDENT_CURRENT(string TableName)
        {
            string sql = string.Format("SELECT IDENT_CURRENT('{0}')", TableName);
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// 執行T-SQL 查詢，並將查詢結果以DataSet回傳
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sqlCommand)
        {
            if (string.IsNullOrWhiteSpace(sqlCommand)) throw new ArgumentException("參數為空值", "sqlCommand");

            DataSet ds = new DataSet();
            cmd.Parameters.Clear();

            cmd.CommandText = sqlCommand;
            cmd.CommandType = CommandType.Text;

            da.Fill(ds);

            return ds;
        }

        /// <summary>
        /// 執行T-SQL 查詢, 並將查詢結果以DataSet回傳
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="p">查詢參數</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sqlCommand, SqlParameter[] p)
        {
            if (string.IsNullOrWhiteSpace(sqlCommand)) throw new ArgumentException("參數為空值", "sqlCommand");
            DataSet ds = new DataSet();
            cmd.Parameters.Clear();
            foreach (var sqlParameter in p)
            {
                cmd.Parameters.Add(sqlParameter);
            }

            cmd.CommandText = sqlCommand;
            cmd.CommandType = CommandType.Text;

            da.Fill(ds);

            return ds;
        }

        public DataTable ExecuteDataTable(string sqlCommand, SqlParameter[] p)
        {
            DataTable ds = new DataTable();

            if (string.IsNullOrWhiteSpace(sqlCommand)) throw new ArgumentException("參數為空值", "sqlCommand");
            cmd.Parameters.Clear();
            foreach (var sqlParameter in p)
            {
                cmd.Parameters.Add(sqlParameter);
            }

            cmd.CommandText = sqlCommand;
            cmd.CommandType = CommandType.Text;

            da.Fill(ds);

            return ds;
        }
        public DataTable ExecuteDataTable(string sqlCommand)
        {
            DataTable ds = new DataTable();

            if (string.IsNullOrWhiteSpace(sqlCommand)) throw new ArgumentException("參數為空值", "sqlCommand");
            cmd.Parameters.Clear();
            cmd.CommandText = sqlCommand;
            cmd.CommandType = CommandType.Text;

            da.Fill(ds);

            return ds;
        }

        public Tuple<bool, DataTable, string> TryExecuteDataTable(string sqlCommand)
        {
            string msg = string.Empty;
            DataTable ds = new DataTable();
            try
            {
                if (string.IsNullOrWhiteSpace(sqlCommand))
                    msg = "參數為空值";
                else
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = sqlCommand;
                    cmd.CommandType = CommandType.Text;
                    da.Fill(ds);
                    msg = "Success";
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ds, ex.Message);
            }

            return Tuple.Create(true, ds, msg);
        }

        public object ExecuteExecuteScalar(string sqlText, SqlParameter[] p)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            cmd.CommandText = sqlText;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            foreach (var sqlParameter in p)
            {
                cmd.Parameters.Add(sqlParameter);
            }
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
        }

        public object ExecuteExecuteScalar(string sqlText)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            cmd.CommandText = sqlText;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                return null;
            }
        }

        public bool ExecuteNonQuery(string sqlText, SqlParameter[] p)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            bool ok = false;

            cmd.CommandText = sqlText;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            foreach (var sqlParameter in p)
            {
                cmd.Parameters.Add(sqlParameter);
            }
            try
            {
                cmd.ExecuteNonQuery();
                ok = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

            }
            return ok;
        }

        public bool ExecuteNonQuery(string sqlText, SqlParameter[] p, out string ErrMsg)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            bool ok = false;

            cmd.CommandText = sqlText;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            foreach (var sqlParameter in p)
            {
                cmd.Parameters.Add(sqlParameter);
            }
            try
            {
                cmd.ExecuteNonQuery();
                ok = true;
                ErrMsg = "";
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString().Replace("\r\n", "");
            }
            return ok;
        }

        public bool ExecuteNonQuery(string sqlText, out string ErrMsg)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            bool ok = false;
            cmd.CommandText = sqlText;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            try
            {
                cmd.ExecuteNonQuery();
                ok = true;
                ErrMsg = "";
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString().Replace("\r\n", "");
            }
            return ok;
        }
        // reference: http://blog.miniasp.com/post/2008/12/09/How-to-get-Stored-Procedure-return-value-using-ADONET.aspx



        public DataTable SP_ExecuteDataTable(string sqlCommand, Dictionary<string, object> in_parameters)
        {
            DataTable ds = new DataTable();

            if (string.IsNullOrWhiteSpace(sqlCommand)) throw new ArgumentException("參數為空值", "sqlCommand");
            cmd.Parameters.Clear();
            cmd.CommandText = sqlCommand;
            cmd.CommandType = CommandType.StoredProcedure;
            if (in_parameters != null)
            {
                foreach (string key in in_parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(key, in_parameters[key]);
                }
            }
            da.Fill(ds);

            return ds;
        }

        /// <summary>
        /// run sp, no return
        /// </summary>
        public string SP_ExecuteNonQuery(string procedureName, Dictionary<string, object> in_parameters)
        {
            using (SqlCommand cmd = new SqlCommand(procedureName, conn))
            {
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                if (in_parameters != null)
                {
                    foreach (string key in in_parameters.Keys)
                    {
                        cmd.Parameters.AddWithValue(key, in_parameters[key]);
                    }
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    return "";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        /// <summary>
        /// run sp, return scalar 
        /// </summary>
        public object SP_ExecuteScalar(string procedureName, Dictionary<string, object> in_parameters)
        {
            using (SqlCommand cmd = new SqlCommand(procedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (in_parameters != null)
                {
                    foreach (string key in in_parameters.Keys)
                    {
                        cmd.Parameters.AddWithValue(key, in_parameters[key]);
                    }
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// run sp, get retVal
        /// </summary>
        public object SP_ExecuteNonQuery(string procedureName, Dictionary<string, object> in_parameters, string retValName)
        {
            using (SqlCommand cmd = new SqlCommand(procedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (in_parameters != null)
                {
                    foreach (string key in in_parameters.Keys)
                    {
                        cmd.Parameters.AddWithValue(key, in_parameters[key]);
                    }
                }

                SqlParameter retValParam = null;

                if (!string.IsNullOrEmpty(retValName))
                {
                    retValParam = cmd.Parameters.Add(retValName, SqlDbType.VarChar, 250);
                    retValParam.Direction = ParameterDirection.ReturnValue;
                }

                if (retValParam == null)
                    return null;
                else
                    return retValParam.Value;
            }
        }

        //--------------------------------------------------------------------------------
        // http://msdn.microsoft.com/zh-tw/library/system.idisposable.dispose(v=vs.110).aspx
        //--------------------------------------------------------------------------------

        ~DataAccess()
        {
            Dispose(false);
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        if (conn.State != ConnectionState.Closed) conn.Close();
                        da.Dispose();
                        cmd.Dispose();
                        conn.Dispose();
                    }
                    catch { }
                }

                disposed = true;
            }
        }

    }
}
