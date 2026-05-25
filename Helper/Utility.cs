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
//using IOCLWeb.Models;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
//using OfficeOpenXml;

namespace SchoolERP_System.Helper
{
    public class Utility
    {
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
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

        //public static string GetMenu(string userID)
        //{
        //    SqlParameter[] Prm = new SqlParameter[] { new SqlParameter("@UserID", userID) };
        //    DataTable dtAll = new SQLHelper().ExecuteDataTable("USP_IOCL_GetMenu", Prm, CommandType.StoredProcedure);
        //    List<MasterLevelMenuModel> MenuList = new List<MasterLevelMenuModel>();
        //    MenuList = Utility.ConvertDataTableToClassObjectList<MasterLevelMenuModel>(dtAll);
        //    StringBuilder SB = new StringBuilder();
        //    List<MasterLevelMenuModel> parentItems = (from a in MenuList where a.MenuParentID == "0" select a).ToList();

        //    SB.Append("<ul class='nav navbar-nav'>");
        //    foreach (var parentmenu in parentItems)
        //    {
        //        List<MasterLevelMenuModel> childItems = (from a in MenuList where a.MenuParentID == parentmenu.MenuID select a).ToList();
        //        if (childItems.Count > 0)
        //        {
        //            SB.Append("<li class='nav-item dropdown' id='" + Regex.Replace(parentmenu.Active_MenuName.Trim(), @"\s", "") + "'>");
        //            SB.Append("<a href='" + parentmenu.ActionLink + "' class='nav-link nav-toggle'><i class='" + parentmenu.MenuIcon + "'></i><span class='title'>" + parentmenu.MenuName + "</span><span class='selected'></span><span class='caret'></span></a>");
        //            AddChildItemForMenu(parentmenu, SB, MenuList);
        //            SB.Append("</li>");
        //        }
        //        else
        //        {
        //            SB.Append("<li class='nav-item' id='" + Regex.Replace(parentmenu.Active_MenuName.Trim(), @"\s", "") + "'>");
        //            SB.Append("<a href='" + parentmenu.ActionLink + "' class='nav-link nav-toggle'><i class='" + parentmenu.MenuIcon + "'></i><span class='title'>" + parentmenu.MenuName + "</span><span class='selected'></span></a>");
        //            SB.Append("</li>");
        //        }
        //    }
        //    SB.Append("</ul>");
        //    string HtmlMenu = Convert.ToString(SB);
        //    return HtmlMenu;
        //}

        //public static void AddChildItemForMenu(MasterLevelMenuModel childItem, StringBuilder childitem, List<MasterLevelMenuModel> MenuList)
        //{
        //    childitem.Append("<ul class='dropdown-menu'>");
        //    List<MasterLevelMenuModel> childItems = (from a in MenuList where a.MenuParentID == childItem.MenuID select a).ToList();
        //    foreach (MasterLevelMenuModel cItem in childItems)
        //    {
        //        childitem.Append("<li class='nav-item'>");
        //        List<MasterLevelMenuModel> subChilds = (from a in MenuList where a.MenuParentID == cItem.MenuID select a).ToList();
        //        if (subChilds.Count > 0)
        //        {
        //            childitem.Append("<a href='" + cItem.ActionLink + "' class='nav-link nav-toggle'><span class='title'>" + cItem.MenuName + "</span><span class='selected'></span><span class='arrow'></span></a>");
        //            AddChildItemForMenu(cItem, childitem, MenuList);
        //            childitem.Append("</li>");
        //        }
        //        else
        //        {
        //            childitem.Append("<a href='" + cItem.ActionLink + "' class='nav-link nav-toggle'><span class='title'>" + cItem.MenuName + "</span><span class='selected'></span></a></li>");
        //        }
        //    }
        //    childitem.Append("</ul>");
        //}

        //#region Get Menu Tree

        //public static string GetMenuTree(string roleID)
        //{
        //    SqlParameter[] Prm = new SqlParameter[] { new SqlParameter("@RoleID", roleID) };
        //    DataTable dtAll = new SQLHelper().ExecuteDataTable("USP_IOCL_GetMenuTree", Prm, CommandType.StoredProcedure);
        //    List<MasterLevelMenuModel> MenuList = new List<MasterLevelMenuModel>();
        //    MenuList = Utility.ConvertDataTableToClassObjectList<MasterLevelMenuModel>(dtAll);
        //    StringBuilder SB = new StringBuilder();
        //    List<MasterLevelMenuModel> parentItems = (from a in MenuList where a.MenuParentID == "0" select a).ToList();
        //    int count = 0;
        //    foreach (var parentmenu in parentItems)
        //    {
        //        List<MasterLevelMenuModel> childItems = (from a in MenuList where a.MenuParentID == parentmenu.MenuID select a).ToList();
        //        if (childItems.Count > 0)
        //        {
        //            SB.Append("<li data-value='" + parentmenu.MenuID + "' class='treeParent'><span class='treespan'>" + parentmenu.MenuName + "</span>");
        //            AddChildItemForTree(parentmenu, SB, MenuList);
        //            SB.Append("</li>");
        //        }
        //        else
        //        {
        //            SB.Append("<li data-value='" + parentmenu.MenuID + "'><span class='treespan'>" + parentmenu.MenuName + "</span>");
        //            SB.Append("</li>");

        //        }
        //        count++;
        //    }

        //    count = 0;
        //    return Convert.ToString(SB);
        //}

        //public static void AddChildItemForTree(MasterLevelMenuModel childItem, StringBuilder childitem, List<MasterLevelMenuModel> MenuList)
        //{
        //    childitem.Append("<ul>");
        //    List<MasterLevelMenuModel> childItems = (from a in MenuList where a.MenuParentID == childItem.MenuID select a).ToList();
        //    foreach (MasterLevelMenuModel cItem in childItems)
        //    {


        //        List<MasterLevelMenuModel> subChilds = (from a in MenuList where a.MenuParentID == cItem.MenuID select a).ToList();
        //        if (subChilds.Count > 0)
        //        {
        //            childitem.Append("<li data-value='" + cItem.MenuID + "' class='treeParent'><span class='treespan'>" + cItem.MenuName + "</span>");
        //            AddChildItemForTree(cItem, childitem, MenuList);
        //            childitem.Append("</li>");
        //        }
        //        else
        //        {
        //            childitem.Append("<li data-value='" + cItem.MenuID + "'><span class='treespan'>" + cItem.MenuName + "</span>");
        //            childitem.Append("</li>");
        //        }

        //    }
        //    childitem.Append("</ul>");

        //}

        //#endregion

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

        public static List<SelectListItem> GetDropDownListFromDatatable(DataTable dtlist, string valueFieldName, string textFieldName, SqlParameter[] prm = null, string selectedItem = "", string selectedOption = "", string defaultText = "")
        {
            List<SelectListItem> DropDownlist = new List<SelectListItem>();
            DataTable dtDropdown = dtlist;
            
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
            for (int i = 1; i >= 0; i--)
            {
                int CurrentYear = DateTime.Today.Year - i;
                int PreviousYear = (DateTime.Today.Year - 1) - i;
                string FinYear = null;
                FinYear = PreviousYear.ToString() + "-" + CurrentYear.ToString();
                FY_list.Add(new SelectListItem() { Text = Convert.ToString(FinYear), Value = Convert.ToString(FinYear) });
            }

            //FY_list.Add(new SelectListItem() { Text = Convert.ToString(CurrentFY), Value = Convert.ToString(CurrentFY) });

            for (int i = 0; i <= 2; i++)
            {
                int CurrentYear = DateTime.Today.Year + 1 + i;
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

        public static string GetIP()
        {
            bool GetLan = false;
            string visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
            {
                GetLan = true;
                visitorIPAddress = string.Empty;
            }

            if (GetLan)
            {
                if (string.IsNullOrEmpty(visitorIPAddress))
                {
                    //This is for Local(LAN) Connected ID Address
                    string stringHostName = Dns.GetHostName();
                    //Get Ip Host Entry
                    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                    //Get Ip Address From The Ip Host Entry Address List
                    IPAddress[] arrIpAddress = ipHostEntries.AddressList;
                    try
                    {
                        visitorIPAddress = arrIpAddress[1].ToString();
                    }
                    catch
                    {
                        try
                        {
                            visitorIPAddress = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                visitorIPAddress = arrIpAddress[0].ToString();
                            }
                            catch
                            {
                                visitorIPAddress = "127.0.0.1";
                            }
                        }
                    }
                }
            }
            return visitorIPAddress;
        }

        public static void SaveLoginLogoutActivity(string UserId, string Activity)
        {
            string IPAdd = Convert.ToString(Utility.GetIP());
            string MachineName = System.Environment.MachineName;
            string MachineUserName = System.Environment.UserName;

            SqlParameter[] prm = new SqlParameter[]
            {
                new SqlParameter("UserId", UserId),
                new SqlParameter("IPAdd", IPAdd),
                new SqlParameter("MachineName", MachineName),
                new SqlParameter("MachineUserName", MachineUserName),
                new SqlParameter("Activity", Activity)
            };
            int Result = new SQLHelper().ExecuteNonQuery("SPIPAM_SAVELOGINLOGOUTACTIVITY", prm, CommandType.StoredProcedure);
        }

        //public static DataTable ToDataTable(ExcelPackage package)
        //{
        //    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        //    DataTable table = new DataTable();
        //    foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        //    {
        //        table.Columns.Add(firstRowCell.Text);
        //    }

        //    for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //    {
        //        var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
        //        var newRow = table.NewRow();
        //        foreach (var cell in row)
        //        {
        //            newRow[cell.Start.Column - 1] = cell.Text;
        //        }
        //        table.Rows.Add(newRow);
        //    }
        //    return table;
        //}

        public static DataTable StripEmptyRows(DataTable dt)
        {
            List<int> rowIndexesToBeDeleted = new List<int>();
            int indexCount = 0;
            foreach (var row in dt.Rows)
            {
                var r = (DataRow)row;
                int emptyCount = 0;
                int itemArrayCount = r.ItemArray.Length;
                foreach (var i in r.ItemArray) if (string.IsNullOrWhiteSpace(i.ToString())) emptyCount++;

                if (emptyCount == itemArrayCount) rowIndexesToBeDeleted.Add(indexCount);

                indexCount++;
            }

            int count = 0;
            foreach (var i in rowIndexesToBeDeleted)
            {
                dt.Rows.RemoveAt(i - count);
                count++;
            }

            return dt;
        }
    }
}