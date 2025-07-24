using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SchoolERP_System.Areas.LearningAdmin.Helper
{
    public class SQLHelper
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConStrLA"]);
        //public static string strConnectionString = "Password=P^YygR!12Xit&aP4LJ;Persist Security Info=True;User ID=sa;Initial Catalog=RS_Attendance_DB_6_May;Data Source=103.48.50.181,1232";
        //   public static string strConnectionString = "Password=Oxtj3_83;Persist Security Info=True;User ID=RS_Attendance;Initial Catalog=RS_Attendance_DB;Data Source=64.71.180.27";

        //public static string strConnectionString = "Password=sa@123;Persist Security Info=True;User ID=sa;Initial Catalog=RS_Attendance_DB;Data Source=HP";
        //public static string strConnectionString = "Password=P@55w0rd;Persist Security Info=True;User ID=sa;Initial Catalog=RS_Attendance_DB;Data Source=DESKTOP-M549I97\\SQLEXPRESS";
        //SqlConnection con = new SqlConnection(strConnectionString);
        public DataTable ExecuteDataTable(string sql, SqlParameter[] p, CommandType _CommandType)
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
                dt = Utility.RemoveBlankRow(dt);
            }
            catch (Exception ex)
            {

                dt = new DataTable();
            }
            return dt;
        }

        public DataTable ExecuteDataTable(string sql, CommandType _CommandType)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                adp.SelectCommand.CommandType = _CommandType;
                adp.SelectCommand.CommandTimeout = 0;

                adp.Fill(dt);
                dt = Utility.RemoveBlankRow(dt);
            }
            catch (Exception ex)
            {

                dt = new DataTable();
            }
            return dt;
        }

        public object ExecuteScalar(string sql, SqlParameter[] p, CommandType _CommandType)
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

        public object ExecuteScalar(string sql, CommandType _CommandType)
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

        public int ExecuteNonQuery(string sql, SqlParameter[] p, CommandType _CommandType)
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

        public int ExecuteNonQuery(string sql, CommandType _CommandType)
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

        public DataSet ExecuteDataSet(string sql, SqlParameter[] p, CommandType _CommandType)
        {

            DataSet ds = new DataSet();
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

                adp.Fill(ds);
                ds = Utility.RemoveBlankRow(ds);
            }
            catch (Exception ex)
            {

                ds = new DataSet();
            }
            return ds;
        }

        public DataSet ExecuteDataSet(string sql, CommandType _CommandType)
        {

            DataSet ds = new DataSet();
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                adp.SelectCommand.CommandType = _CommandType;
                adp.SelectCommand.CommandTimeout = 0;

                adp.Fill(ds);
                ds = Utility.RemoveBlankRow(ds);
            }
            catch (Exception ex)
            {

                ds = new DataSet();
            }
            return ds;
        }

    }
}