using SchoolERP_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
//using Log4Exception;

namespace SchoolERP_System.Helper
{
    public class SQLHelperDynamic
    {


        public DataTable ExecuteDataTable(SqlConnection con, string sql, SqlParameter[] p, CommandType _CommandType)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                adp.SelectCommand.CommandType = _CommandType;
                adp.SelectCommand.CommandTimeout = 0;
                if (p != null)
                {
                    for (int i = 0; i <= p.Length - 1; i++)
                    {
                        adp.SelectCommand.Parameters.Add(p[i]);
                    }
                }

                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
            }
            return dt;
        }

        public DataTable ExecuteDataTable(SqlConnection con, string sql, CommandType _CommandType)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                adp.SelectCommand.CommandType = _CommandType;
                adp.SelectCommand.CommandTimeout = 0;

                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
            }
            return dt;
        }

        public object ExecuteScalar(SqlConnection con, string sql, SqlParameter[] p, CommandType _CommandType)
        {
            object retval = null;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = _CommandType;
                cmd.CommandTimeout = 0;
                if (p != null)
                {
                    for (int i = 0; i <= p.Length - 1; i++)
                    {
                        cmd.Parameters.Add(p[i]);
                    }
                }
                retval = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
            return retval;
        }

        public object ExecuteScalar(SqlConnection con, string sql, CommandType _CommandType)
        {
            object retval = null;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = _CommandType;
                cmd.CommandTimeout = 0;
                retval = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
            return retval;
        }

        public int ExecuteNonQuery(SqlConnection con, string sql, SqlParameter[] p, CommandType _CommandType)
        {
            int retval = 0;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = _CommandType;
                cmd.CommandTimeout = 0;
                if (p != null)
                {
                    for (int i = 0; i <= p.Length - 1; i++)
                    {
                        cmd.Parameters.Add(p[i]);
                    }
                }
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
            return retval;
        }

        public int ExecuteNonQuery(SqlConnection con, string sql, CommandType _CommandType)
        {
            int retval = 0;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = _CommandType;
                cmd.CommandTimeout = 0;
                retval = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
            return retval;
        }

        public DataSet ExecuteDataSet(SqlConnection con, string sql, CommandType _CommandType)
        {

            DataSet dt = new DataSet();
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                adp.SelectCommand.CommandType = _CommandType;
                adp.SelectCommand.CommandTimeout = 0;

                adp.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = new DataSet();
            }
            return dt;
        }

        public static List<T> ConvertDataTableToClassObjectList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, Convert.ToString(dr[column.ColumnName]), null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public static T ConvertDataTableToClassObject<T>(DataTable Dt)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataRow dr in Dt.Rows)
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                            pro.SetValue(obj, dr[column.ColumnName] == typeof(DBNull) ? "" : Convert.ToString(dr[column.ColumnName]), null);
                        else
                            continue;
                    }
                }
            }
            return obj;

        }
    }
}
