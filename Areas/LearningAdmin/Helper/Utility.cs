using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace SchoolERP_System.Areas.LearningAdmin.Helper
{
    public class Utility
    {
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
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static bool IsDate(string input)
        {
            DateTime temp;
            return DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out temp);
        }
        public static bool IsValidTime(string input)
        {
            TimeSpan temp;
            return TimeSpan.TryParse(input, out temp);
        }
        public static DateTime ConvertDate(string input)
        {
            DateTime temp;
            DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out temp);
            return temp;
        }
        public static string ChangeDateFormat(DateTime Date)
        {
            string ChangedDate = "";
            string Day = Convert.ToString(Date.Day);
            string Month = Convert.ToString(Date.Month);
            string Year = Convert.ToString(Date.Year);
            ChangedDate = Year + "-" + Month + "-" + Day;
            return ChangedDate;
        }

        public static bool IsValidDateRange(string FromDate, string ToDate)
        {
            bool result = false;

            if (FromDate != null && ToDate != null)
            {
                int FromDateDateValues = Convert.ToInt32(FromDate.Replace("-", ""));
                int ToDateDateValues = Convert.ToInt32(ToDate.Replace("-", ""));
                if (FromDateDateValues <= ToDateDateValues)
                    result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static string ChangeDateTimeFormat(DateTime Date)
        {
            string ChangedDate = "";
            string Day = Convert.ToString(Date.Day);
            string Month = Convert.ToString(Date.Month);
            string Year = Convert.ToString(Date.Year);
            string Time = Convert.ToString(Date.ToString("HH:mm"));
            string minute = Convert.ToString(Date.Minute.ToString("mm"));
            string second = Convert.ToString(Date.Second.ToString("ss"));
            ChangedDate = Year + "-" + Month + "-" + Day + " " + Time;
            return ChangedDate;
        }
        public static string CreateTableDataWithHeader(DataTable dt)
        {
            string data = "";
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (i == 0)
                    {
                        data += "[";
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (k == dt.Columns.Count - 1)
                            {
                                data += "\"" + Convert.ToString(dt.Columns[k].ColumnName).Replace(" ", "").Replace("(DD/MM/YYYY)", "").Replace("(24Hrs#)", "").Replace("(Y/N)", "") + "\"";
                            }
                            else
                            {
                                data += "\"" + Convert.ToString(dt.Columns[k].ColumnName).Replace(" ", "").Replace("(DD/MM/YYYY)", "").Replace("(24Hrs#)", "").Replace("(Y/N)", "") + "\",";
                            }
                        }
                        data += "],";
                    }
                    data += "[";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == dt.Columns.Count - 1)
                        {
                            data += "\"" + Convert.ToString(dt.Rows[i][j]) + "\"";
                        }
                        else
                        {
                            data += "\"" + Convert.ToString(dt.Rows[i][j]) + "\",";
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        data += "]";
                    }
                    else
                    {
                        data += "],";
                    }
                }
            }
            return data;
        }
        public static string CreateTableDataWithOutHeader(DataTable dt)
        {
            string data = "";
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data += "[";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == dt.Columns.Count - 1)
                        {
                            data += "\"" + Convert.ToString(dt.Rows[i][j]) + "\"";
                        }
                        else
                        {
                            data += "\"" + Convert.ToString(dt.Rows[i][j]) + "\",";
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        data += "]";
                    }
                    else
                    {
                        data += "],";
                    }
                }
            }
            return data;
        }
        public string encryptText(string text)
        {
            string encryptedText = "";
            char val;
            foreach (char c in text)
            {
                try
                {
                    val = Convert.ToChar((int)c + 10);

                }
                catch
                {
                    val = 'A';
                }
                encryptedText = encryptedText + val;

            }

            return encryptedText;
        }
        public static DataTable RemoveBlankRow(DataTable dtRomoveBlank)
        {

            for (int h = 0; h < dtRomoveBlank.Rows.Count; h++)
            {
                if (dtRomoveBlank.Rows[h].IsNull(0) == true)
                {

                    dtRomoveBlank.Rows.RemoveAt(h);
                    h = 0;
                }
            }
            return dtRomoveBlank;
        }
        public static DataSet RemoveBlankRow(DataSet dtRomoveBlank)
        {
            for (int j = 0; j < dtRomoveBlank.Tables.Count; j++)
            {
                for (int h = 0; h < dtRomoveBlank.Tables[j].Rows.Count; h++)
                {
                    if (dtRomoveBlank.Tables[j].Rows[h].IsNull(0) == true)
                    {

                        dtRomoveBlank.Tables[j].Rows.RemoveAt(h);
                        h = 0;
                    }
                }
            }
            return dtRomoveBlank;
        }
        public static bool IsPastDate(DateTime input)
        {
            DateTime temp;
            bool IsPast;
            string Inputdate = Utility.ChangeDateFormat(input);
            temp = Convert.ToDateTime(Inputdate);
            string Current = Utility.ChangeDateFormat(DateTime.Now);
            DateTime CurrentDate = Convert.ToDateTime(Current);
            if (temp < CurrentDate)
            {
                IsPast = true;
                return IsPast;
            }
            else
            {
                IsPast = false;
                return IsPast;
            }
            //IsPast = DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out temp);

        }
        public static DataTable ChangeColumnDataType(DataTable table)
        {
            List<string> columnName = new List<string>();
            try
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    columnName.Add(table.Columns[i].ColumnName);
                }
                foreach (string ColName in columnName)
                {
                    DataColumn newcolumn = new DataColumn("temporary", typeof(string));
                    table.Columns.Add(newcolumn);
                    foreach (DataRow row in table.Rows)
                    {
                        try
                        {
                            row["temporary"] = Convert.ChangeType(row[ColName], typeof(string));
                        }
                        catch
                        {
                        }
                    }
                    table.Columns.Remove(ColName);
                    newcolumn.ColumnName = ColName;
                }
            }
            catch (Exception)
            {
                return new DataTable();
            }

            return table;
        }

        public static string CheckColumnDataType(DataTable table)
        {
            string ReturnString = "Success";
            try
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (table.Columns[i].DataType == typeof(DateTime))
                    {
                        ReturnString = "Error";
                    }
                }
            }
            catch (Exception)
            {
                ReturnString = "Error";
            }

            return ReturnString;

        }
        public static string CheckDateFormatForExcel(DataTable table, int columnNo)
        {
            string ReturnString = "";
            try
            {
                string[] monthlist = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames;//CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    bool check = false;
                    string date = Convert.ToString(table.Rows[i][columnNo]);
                    string month = date.Split('-')[1];
                    foreach (var monthabbr in monthlist)
                    {
                        if (monthabbr == month)
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        ReturnString = "Wrong date format. Line No: " + (i + 1) + ", Column: " + columnNo + ". (ex - (12-Jul-2017) DD-MMM-YYYY )";
                        return ReturnString;
                    }
                }
            }
            catch (Exception)
            {
                ReturnString = "Error";
                return ReturnString;
            }

            return ReturnString;

        }

        /// <summary>
        /// Application Server date format must be either DD/MM/YYYY or MM/DD/YYYY
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnNo"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static DataTable ChangeDateFormatForExcel(DataTable table, int columnNo, out string check)
        {
            check = "";

            try
            {
                string[] monthlist = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames;//CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    bool CorrectFormat = true;
                    string date = Convert.ToString(table.Rows[i][columnNo]).Remove(Convert.ToString(table.Rows[i][columnNo]).IndexOf(' '));
                    string sysdateformat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                    string[] splitDateFormat = null;
                    if (date.Contains("-"))
                    {
                        splitDateFormat = date.Split('-');
                    }
                    else if (date.Contains("/"))
                    {
                        splitDateFormat = date.Split('/');
                    }
                    else
                    {
                        CorrectFormat = false;
                        // table = null;
                        check += "Wrong date format. Line No: " + (i + 1) + ", Column: " + columnNo + ". (ex - DD-MM-YYYY or DD/MM/YYYY ). /";
                    }

                    if (splitDateFormat != null && splitDateFormat.Length > 0)
                    {
                        if (sysdateformat.Substring(0, 1).ToUpper() == "M")
                        {
                            if (Convert.ToInt32(splitDateFormat[0]) > 12)
                            {
                                check += "Wrong date format. Line No: " + (i + 1) + ", Column: " + columnNo + ". (ex - DD-MM-YYYY or DD/MM/YYYY ). /";
                            }
                        }
                        else if (sysdateformat.Substring(0, 1).ToUpper() == "D")
                        {
                            if (Convert.ToInt32(splitDateFormat[1]) > 12)
                            {
                                check += "Wrong date format. Line No: " + (i + 1) + ", Column: " + columnNo + ". (ex - DD-MM-YYYY or DD/MM/YYYY ). /";
                            }
                        }
                    }

                    if (CorrectFormat == true)
                    {
                        if (sysdateformat.Substring(0, 1).ToUpper() == "M")
                        {
                            table.Rows[i][columnNo] = splitDateFormat[1] + "-" + monthlist[Convert.ToInt32(splitDateFormat[0]) - 1] + "-" + splitDateFormat[2];
                        }
                        else if (sysdateformat.Substring(0, 1).ToUpper() == "D")
                        {
                            table.Rows[i][columnNo] = splitDateFormat[0] + "-" + monthlist[Convert.ToInt32(splitDateFormat[1]) - 1] + "-" + splitDateFormat[2];
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                check = ex.Message + " /";
            }

            return table;

        }

        public static DataTable ConvertReaderToDatatable(string connectionString)
        {
            DataTable dt = new DataTable();
            OleDbConnection ConExcel = new OleDbConnection();
            OleDbDataReader dr = null;
            try
            {
                ConExcel.ConnectionString = connectionString;
                OleDbCommand CmdExcel = new OleDbCommand();
                OleDbDataAdapter AdpExcel = new OleDbDataAdapter();
                ConExcel.Open();
                DataTable DtExcelSchema = new System.Data.DataTable();
                DtExcelSchema = ConExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = Convert.ToString(DtExcelSchema.Rows[0]["TABLE_NAME"]);
                ConExcel.Close();
                ConExcel.Open();
                CmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                //AdpExcel.SelectCommand = CmdExcel;
                //AdpExcel.SelectCommand.Connection = ConExcel;
                CmdExcel.Connection = ConExcel;
                CmdExcel.CommandTimeout = 0;
                dr = CmdExcel.ExecuteReader();

                if (dr.HasRows)
                {

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        dt.Columns.Add(dr.GetName(i), typeof(string));
                    }
                    while (dr.Read())
                    {
                        DataRow dataRow = dt.NewRow();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            dataRow[j] = Convert.ToString(dr[j]);
                        }
                        dt.Rows.Add(dataRow);
                    }
                }
                dr.Close();
                ConExcel.Close();
            }
            catch (Exception ex)
            {
                dt = new DataTable();
            }
            finally
            {
                dr.Close();
                ConExcel.Close();
            }

            return dt;

        }

    

        /// <summary>
        /// Get List of SelectListItem 
        /// </summary>
        /// <param name="spName">Set Stored Procedure Name</param>
        /// <param name="valueFieldName">Set ValueField Name which bind the seletlistitem Value from the Stored Procedure</param>
        /// <param name="textFieldName">Set TextField Name which bind the seletlistitem Text from the Stored Procedure</param>
        /// <param name="prm">Set the Parameters of Stored Procedure if any (Optional).</param>
        /// <param name="selectedItem">Set the SelectedItem Value if any (Optional).</param>
        /// <param name="selectedOption">Set the SelectedItem Option Text or Value if any (Optional).</param>
        /// <param name="defaultText">Set the default text if any (Optional).</param>
        /// <returns></returns>
        public static List<SelectListItem> GetDropDownList(string spName, string valueFieldName, string textFieldName, SqlParameter[] prm = null, string selectedItem = "", string selectedOption = "", string defaultText = "")
        {
            List<SelectListItem> DropDownlist = new List<SelectListItem>();
            DataTable dtDropdown = new DataTable();
            if (prm != null)
            {
                dtDropdown = new SQLHelper().ExecuteDataTable(spName, prm, CommandType.StoredProcedure);
            }
            else
            {
                dtDropdown = new SQLHelper().ExecuteDataTable(spName, CommandType.StoredProcedure);
            }
            if (!string.IsNullOrWhiteSpace(defaultText))
            {
                if (string.IsNullOrWhiteSpace(selectedItem))
                {
                    DropDownlist.Add(new SelectListItem() { Text = defaultText, Value = "-1", Selected = true });
                }
                else
                {
                    DropDownlist.Add(new SelectListItem() { Text = defaultText, Value = "-1" });
                }
            }
            if (dtDropdown != null && dtDropdown.Rows.Count > 0)
            {
                for (int i = 0; i < dtDropdown.Rows.Count; i++)
                {
                    DropDownlist.Add(new SelectListItem() { Text = Convert.ToString(dtDropdown.Rows[i][textFieldName]), Value = Convert.ToString(dtDropdown.Rows[i][valueFieldName]) });
                }
            }
            if (!string.IsNullOrWhiteSpace(selectedItem))
            {
                if (!string.IsNullOrWhiteSpace(selectedOption))
                {
                    if (selectedOption.ToUpper() == "TEXT")
                    {
                        foreach (var listItem in DropDownlist)
                        {
                            if (listItem.Text == selectedItem)
                            {
                                listItem.Selected = true;
                            }
                        }
                    }
                    else if (selectedOption.ToUpper() == "VALUE")
                    {
                        foreach (var listItem in DropDownlist)
                        {
                            if (listItem.Value == selectedItem)
                            {
                                listItem.Selected = true;
                            }
                        }
                    }
                }
            }
            return DropDownlist;
        }
        /// <summary>
        /// Check, either logged in user has access for the action or not 
        /// </summary>
        /// <param name="actionName">Set action name</param>
        /// <param name="controllerName">Set controller name</param>
        /// <param name="roleID">Set loggedin user role id</param>
        /// <returns>Yes/No (string)</returns>
        public static string CheckMenuAccess(string actionName, string controllerName, string roleID)
        {
            string returnValue = "";
            if (string.IsNullOrWhiteSpace(actionName) && string.IsNullOrWhiteSpace(controllerName) && string.IsNullOrWhiteSpace(roleID))
            {
                returnValue = "No";
            }
            else
            {
                SqlParameter[] prm = new SqlParameter[] { 
                    new SqlParameter("@RoleID",roleID),
                    new SqlParameter("@Action",actionName),
                    new SqlParameter("@Controller",controllerName)
                };
                returnValue = Convert.ToString(new SQLHelper().ExecuteScalar("SPIPAM_CheckPageAccess", prm, CommandType.StoredProcedure));
            }
            return returnValue;
        }

        // Get current financial year
        public static string GetCurrentFinancialYear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (DateTime.Today.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
        }

        // Get financial year List
        public static List<SelectListItem> GetFinancialYearList()
        {
            string CurrentFY = GetCurrentFinancialYear();
            List<SelectListItem> FY_list = new List<SelectListItem>();
            for (int i = 9; i >= 0; i--)
            {
                int CurrentYear = DateTime.Today.Year - i;
                int PreviousYear = (DateTime.Today.Year - 1) - i;
                string FinYear = null;
                FinYear = PreviousYear.ToString() + "-" + CurrentYear.ToString();
                FY_list.Add(new SelectListItem() { Text = Convert.ToString(FinYear), Value = Convert.ToString(FinYear) });
            }

            FY_list.Add(new SelectListItem() { Text = Convert.ToString(CurrentFY), Value = Convert.ToString(CurrentFY) });

            for (int i = 1; i <= 2; i++)
            {
                int CurrentYear = DateTime.Today.Year+1 + i;
                int PreviousYear = (DateTime.Today.Year) + i;
                string FinYear = null;
                FinYear = PreviousYear.ToString() + "-" + CurrentYear.ToString();
                FY_list.Add(new SelectListItem() { Text = Convert.ToString(FinYear), Value = Convert.ToString(FinYear) });
            }
            return FY_list;
        }
        // Convert CSV to Datatable
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Replace(".", "").Replace("#", ""));
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }
       
    }
}