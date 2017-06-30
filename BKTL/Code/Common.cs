//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;

namespace BKTL
{
    /// <summary>
    /// Các hàm dùng chung
    /// </summary>
    static public class Common
    {
        const int defaultWidth = 800;

        /// <summary> Trả về setting trong app.config hay web.config
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value"></param>
        /// <returns>Giá trị của Key trong app.config hoặc web.config. Trả về rỗng nếu không có Key hoặc lỗi</returns>
        static public string GetSetting(string key, out string value)
        {
            string msg = "";
            value = null;
            try
            {
                value = ConfigurationManager.AppSettings[key].ToString();
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }
        static public string GetSettingWithDefault(string key, string defaultValue)
        {
            string value = null;
            string msg = GetSetting(key, out value);
            if (msg.Length > 0) value = defaultValue;

            return value;
        }

        static public string GetSession(HttpSessionState Session, string key, out int value)
        {
            string msg = "";
            value = 0;
            try
            {
                value = int.Parse(Session[key].ToString());
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }
        static public string GetSession(HttpSessionState Session, string key, out long value)
        {
            string msg = "";
            value = 0;
            try
            {
                value = long.Parse(Session[key].ToString());
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }
        static public string GetSession(HttpSessionState Session, string key, out string value)
        {
            string msg = "";
            value = null;
            try
            {
                value = Session[key].ToString();
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }
        static public string GetSession(HttpSessionState Session, string key, out Guid value)
        {
            string msg = "";
            value = Guid.Empty;
            try
            {
                value = Guid.Parse(Session[key].ToString());
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }
        static public int GetSessionWithDefault(HttpSessionState Session, string key, int defaultValue)
        {
            try
            {
                return int.Parse(Session[key].ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        static public long GetSessionWithDefault(HttpSessionState Session, string key, long defaultValue)
        {
            try
            {
                return long.Parse(Session[key].ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        static public string GetSessionWithDefault(HttpSessionState Session, string key, string defaultValue)
        {
            try
            {
                return Session[key].ToString();
            }
            catch
            {
                return defaultValue;
            }
        }

        static public string ProcessRequest<T>(HttpRequest Request, out T value) where T : class
        {
            value = default(T);

            try
            {
                System.IO.Stream body = Request.InputStream;
                System.Text.Encoding encoding = Request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                string json = reader.ReadToEnd();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                object o = serializer.Deserialize(json, typeof(T));
                value = o as T;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "";
        }

        static public string SaveToViewState<T>(StateBag ViewState, T parameters) where T : class
        {
            try
            {
                var ps = typeof(T).GetProperties();
                foreach (var p in ps)
                    ViewState[p.Name] = p.GetValue(parameters, null);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }

        static public string LoadFromViewState<T>(StateBag ViewState, string key, out T value) where T : class
        {
            value = null;
            try
            {
                value = ViewState[key] as T;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }
        static public string LoadFromViewState(StateBag ViewState, string key, out int value)
        {
            value = 0;
            try
            {
                value = int.Parse(ViewState[key].ToString());
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }
        static public string LoadFromViewState(StateBag ViewState, string key, out string value)
        {
            value = null;
            try
            {
                value = ViewState[key].ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }


        static public bool HasPublicConstField(Type type, string field)
        {
            try
            {
                field = field.ToLower();

                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (var f in fieldInfos)
                    if (f != null && field == f.Name.ToLower()) return true;
            }
            catch
            {
                return false;
            }
            return false;
        }
        static public bool HasPublicConstValue(Type type, int value)
        {
            List<string> values = null;
            string msg = GetPublicConstValues(type, out values);
            if (msg.Length > 0) return false;

            return values.Contains(value.ToString());
        }
        static public string GetPublicConstValues(Type type, out List<string> values, bool getLowercase = false)
        {
            string msg = "";
            values = new List<string>();

            try
            {
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (var f in fieldInfos)
                {
                    var v = f.GetValue(null);
                    if (getLowercase) values.Add(v.ToString().ToLower());
                    else values.Add(v.ToString());
                }
            }
            catch (Exception ex)
            {
                values.Clear();
                values = null;
                msg = ex.ToString();
            }
            return msg;
        }

        static public string CopyObjectPropertyData<T1, T2>(T1 obj1, T2 obj2, string exceptProperties = null) where T1 : class
        {
            string msg = "";
            string pName = "";

            string[] arrExceptProperties = null;
            if (!string.IsNullOrWhiteSpace(exceptProperties)) arrExceptProperties = exceptProperties.Split('|');

            try
            {
                var ps1 = typeof(T1).GetProperties();
                foreach (var p1 in ps1)
                {
                    pName = p1.Name;
                    if (arrExceptProperties != null && arrExceptProperties.Contains(pName)) continue;

                    var p2 = typeof(T2).GetProperty(pName);
                    if (p2 != null) p2.SetValue(obj2, p1.GetValue(obj1, null), null);
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString() + " @" + pName;
            }
            return msg;
        }

        //string msg = Common.DoEx(i => DBM.ExecStore("sp_InvoiceRelease_UpdateStatus", new { IP }));
        //if (msg.Length > 0)
        //    Log.WriteErrorLog(msg).ToGeneralSystemErrorMessage();

        /// <summary> <para>Hàm chạy 1 Expression (biểu thức/câu lệnh). Nếu Expression chạy có lỗi (return string khác rỗng) thì sẽ ghép nội dung của Expression vào trước lỗi.
        /// Nội dung của Expression sẽ bao gồm tên hàm được gọi (câu lệnh) và các tham số của hàm, cùng giá trị của các tham số này.</para>
        /// <para>Ví dụ: string msg = Common.DoEx(i => DBM.ExecStore("sp_InvoiceRelease_Insert", o).ToGeneralSystemErrorMessage());</para>
        /// nếu có lỗi sẽ trả về: ERROR AT [BSS.DBM.ExecStore(sp_InvoiceRelease_Insert; o = { IP = 1.2.3.4, UserName = ABC }).BSS.Log.ToGeneralSystemErrorMessage()]. ERROR CONTENT: Có lỗi xảy ra. Xin vui lòng thử lại sau hoặc thông báo với quản trị (#Error: Connection string is null or empty)
        /// <para>Ví dụ khác: lấy 20 ký tự đầu của expression khi có lỗi: string msg = Common.DoEx(s20 => DBM.ExecStore("sp_InvoiceRelease_UpdateStatus", new { IP }));</para>
        /// <para>Lấy đễn vị trí của "IP" trong expression khi có lỗi: string msg = Common.DoEx(fIP => DBM.ExecStore("sp_InvoiceRelease_UpdateStatus", new { IP }));</para>
        /// </summary>
        static public string DoEx(Expression<Func<object, string>> expression)
        {
            string msg = "";
            try
            {
                Func<object, string> compiledExpression = expression.Compile();
                msg = compiledExpression(null);
                if (msg.Length > 0)
                {
                    var pName = expression.Parameters[0].Name;

                    string s = expression.Body.ToString();
                    int i;
                    switch (pName[0])
                    {
                        case 's':
                            i = int.Parse(pName.Substring(1));
                            s = s.Substring(0, i < s.Length ? i : s.Length - 1) + "...";
                            break;
                        case 'f':
                            i = s.IndexOf(pName.ToString().Substring(1));
                            if (i >= 0) s = s.Substring(0, i >= 0 ? i : s.Length - 1) + "...";
                            break;
                        default:
                            s = DoExGetValue(expression.Body).ToString();
                            break;
                    }
                    msg = "ERROR AT [" + s + "]. ERROR CONTENT: " + msg;
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return msg;
        }

        // Hàm tính giá trị của các Expression: MemberExpression (ConstantExpression, FieldExpression), MethodCallExpression...
        private static string DoExGetValue(object o)
        {
            try
            {
                if (o is ConstantExpression) return ((ConstantExpression)o).Value.ToString();

                if (o is MethodCallExpression)
                {
                    var mce = o as MethodCallExpression;
                    var method = mce.Method;
                    var methodName = method.DeclaringType.FullName + "." + method.Name;

                    string s = "";
                    foreach (var a in mce.Arguments)
                        s += DoExGetValue(a) + "; ";

                    if (s.Length == 0) s = "?";
                    else s = s.TrimEnd(';', ' ');

                    if (method.IsDefined(typeof(ExtensionAttribute), false)) return s + "." + methodName + "()";
                    return methodName + "(" + s + ")";
                }

                if (o is NewExpression)
                {
                    var Args = ((NewExpression)o).Arguments;
                    string s = "";
                    for (int i = 0; i < Args.Count; i++)
                    {
                        var p = ((NewExpression)o).Members[i];
                        var v = DoExGetValue(Args[i]);
                        if (v.IndexOf("=") < 0) s += p.Name + " = " + v + ", ";
                        else s += v + ", ";
                    }
                    return "new { " + s.TrimEnd(',', ' ') + " }";
                }

                if (o is MemberExpression)
                {
                    var me = o as MemberExpression;
                    var objectMember = Expression.Convert(me, typeof(object));
                    var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                    var getter = getterLambda.Compile();
                    var v = getter() ?? "null";
                    if (me.Member is FieldInfo) return me.Member.Name + " = " + v.ToString();
                    return v.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "?";
        }



        /// <summary>
        /// Đăng ký 1 script ở page hoặc UserControl để hiển thị Popup thông báo lỗi (gọi hàm javascript bkav_alert_error)
        /// </summary>
        /// <param name="msg">Nội dung trong Popup thông báo lỗi</param>
        /// <param name="userControl">Là UserControl chứa PU thông báo lỗi</param>
        /// <returns></returns>
        static public string ShowErrorPopup(this string msg, UserControl userControl, int width = defaultWidth)
        {
            return ShowErrorPopup(msg, (object)userControl, width);
        }
        static public string ShowErrorPopup(this string msg, Page page, int width = defaultWidth)
        {
            return ShowErrorPopup(msg, (object)page, width);
        }
        static public string ShowErrorPopup(string msg, object oPageOrUserControl, int width = defaultWidth)
        {
            string message = msg.Replace("\r\n", "</br>");
            return RegisterScript("bkav_alert_error('" + message + "', " + width + ");", oPageOrUserControl, "bkav_alert_error");
        }

        /// <summary>
        /// Đăng ký 1 script ở page hoặc UserControl để chạy 1 đoạn code javascript
        /// </summary>
        /// <param name="script">Đoạn code javascript</param>
        /// <param name="id">Id của script</param>
        /// <param name="o">Là page hay UserControl</param>
        /// <returns></returns>
        static public string RegisterScript(this string script, object o, string id = null)
        {
            if (!(o is UserControl) && !(o is Page)) return "";
            try
            {
                Page page = o as Page;
                if (page == null) // o is UserControl
                    page = ((UserControl)o).Page;

                if (id == null) id = Guid.NewGuid().ToString();
                page.ClientScript.RegisterStartupScript(o.GetType(), id, script, true);
                return script;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fullPath"></param>
        static public string ObjectToFile<T>(T obj, string fullPath)
        {
            string msg = "";

            if (obj == null) return "Error: obj is null @Common.ObjectToFile";

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, obj);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fullPath);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullPath"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static public string FileToObject<T>(string fullPath, out T value)
        {
            string msg = "";
            value = default(T);

            if (string.IsNullOrEmpty(fullPath)) return "Error: fullPath is null or empty @Common.FileToObject";

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fullPath);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        value = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return msg;
        }


        //static public string GetDataTableFromExcelFile(string fullPath, out DataTable dt)
        //{
        //    string msg = "";
        //    dt = null;

        //    string extension = Path.GetExtension(fullPath).ToLower();
        //    if (extension == ".xls") msg = GetDataTableFromXls(fullPath, out dt);
        //    else if (extension == ".xlsx") msg = GetDataTableFromXlsx(fullPath, out dt);
        //    else return "Phần mở rộng của File không phải là Excel (.xls hoặc .xlsx)";
        //    if (msg.Length > 0) return msg;

        //    return msg;
        //}

        //static public string GetDataTableFromXls(string fullPath, out DataTable dt)
        //{
        //    string msg = "";
        //    dt = null;
        //    int rowIndex = 0;
        //    try
        //    {
        //        FileInfo fi = new FileInfo(fullPath);
        //        if (!fi.Exists) return "File không tồn tại: " + fullPath;

        //        HSSFWorkbook hssfwb;
        //        using (FileStream file = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        //        {
        //            hssfwb = new HSSFWorkbook(file);
        //        }

        //        ISheet sheet = hssfwb.GetSheetAt(0);
        //        dt = new DataTable(sheet.SheetName);

        //        int maxCols = 0;
        //        foreach (IRow row in sheet)
        //            if (row.LastCellNum > maxCols) maxCols = row.LastCellNum;

        //        for (int i = 0; i < maxCols; i++)
        //            dt.Columns.Add(string.Format("Column {0}", i));

        //        foreach (IRow row in sheet)
        //        {
        //            rowIndex++;
        //            DataRow dataRow = dt.NewRow();

        //            foreach (var c in row.Cells)
        //            {
        //                switch (c.CellType)
        //                {
        //                    case CellType.Blank: dataRow[c.ColumnIndex] = ""; break;
        //                    case CellType.Boolean: dataRow[c.ColumnIndex] = c.BooleanCellValue; break;
        //                    case CellType.Error: dataRow[c.ColumnIndex] = c.ErrorCellValue; break;
        //                    case CellType.Formula: dataRow[c.ColumnIndex] = c; break;
        //                    case CellType.String: dataRow[c.ColumnIndex] = c.StringCellValue; break;
        //                    case CellType.Unknown: dataRow[c.ColumnIndex] = c; break;
        //                    case CellType.Numeric:
        //                        if (DateUtil.IsCellDateFormatted(c)) dataRow[c.ColumnIndex] = c.DateCellValue.ToString("dd/MM/yyyy");
        //                        else dataRow[c.ColumnIndex] = c.NumericCellValue.ToString();

        //                        break;
        //                }
        //            }

        //            dt.Rows.Add(dataRow);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dt = null;
        //        msg = ex.ToString() + " @Row: #" + rowIndex;
        //    }
        //    return msg;
        //}
        //static public string GetDataTableFromXlsx(string fullPath, out DataTable dt)
        //{
        //    string msg = "";
        //    dt = null;
        //    int rowNum = 1;
        //    int colNum = 1;
        //    try
        //    {
        //        FileInfo fi = new FileInfo(fullPath);
        //        if (!fi.Exists) return "File không tồn tại: " + fullPath;

        //        using (var pck = new ExcelPackage())
        //        {
        //            using (var stream = File.OpenRead(fullPath))
        //            {
        //                pck.Load(stream);
        //            }
        //            var ws = pck.Workbook.Worksheets.First();

        //            dt = new DataTable();
        //            for (colNum = 1; colNum <= ws.Dimension.End.Column; colNum++)
        //                dt.Columns.Add(string.Format("Column {0}", colNum));

        //            for (; rowNum <= ws.Dimension.End.Row; rowNum++)
        //            {
        //                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
        //                DataRow row = dt.Rows.Add();
        //                foreach (var cell in wsRow)
        //                {
        //                    colNum = 1;

        //                    string format = cell.Style.Numberformat.Format;
        //                    object value = cell.Value;
        //                    if (format.Contains("yyyy") || format.Contains("yy"))
        //                    {
        //                        var v = cell.Value;
        //                        DateTime date = DateTime.MinValue;
        //                        if (v is double) date = DateTime.FromOADate((double)v);
        //                        else if (v is DateTime) date = (DateTime)v;
        //                        else
        //                        {
        //                            try
        //                            {
        //                                date = DateTime.ParseExact(v.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                            }
        //                            catch { }
        //                        }
        //                        if (date != DateTime.MinValue) value = date.ToString("dd/MM/yyyy");
        //                    }
        //                    row[cell.Start.Column - 1] = value;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dt = null;
        //        msg = ex.ToString() + " @Row-Col: #" + rowNum + "-" + colNum;
        //    }
        //    return msg;
        //}

        //static public string SaveDataTableToExcelFile(DataTable dt, string fileName, HttpResponse Response)
        //{
        //    string msg;
        //    byte[] bytes = null;
        //    msg = SaveDataTableToExcelFile(dt, out bytes);
        //    if (msg.Length > 0) return msg;

        //    try
        //    {
        //        string attachment = "attachment; filename=" + fileName + ".xlsx";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", attachment);
        //        Response.BinaryWrite(bytes);
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.ToString();
        //    }
        //    return msg;
        //}
        //static public string SaveDataTableToExcelFile(DataTable dt, string fullPath)
        //{
        //    byte[] excelData = null;
        //    string msg = SaveDataTableToExcelFile(dt, out excelData);
        //    if (msg.Length > 0) return msg;

        //    try
        //    {
        //        File.WriteAllBytes(fullPath, excelData);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.ToString();
        //    }
        //    return msg;
        //}
        //static public string SaveDataTableToExcelFile(DataTable dt, out byte[] excelData)
        //{
        //    string msg = "";

        //    excelData = null;
        //    if (dt == null || dt.Rows.Count == 0) return "DataTable rỗng hoặc không có dữ liệu";

        //    int ExcelExport_HeaderRow = 1;
        //    try
        //    {
        //        ExcelPackage pck = new ExcelPackage();

        //        int totalCols = dt.Columns.Count;
        //        var ws = pck.Workbook.Worksheets.Add(dt.TableName);

        //        //SetHeader(ws, out indexes);
        //        int[] maxWidthes = new int[totalCols];
        //        for (int iCol = 0; iCol < totalCols; iCol++)
        //        {
        //            var cell = dt.Columns[iCol];
        //            ws.Cells[ExcelExport_HeaderRow, iCol + 1].Value = cell.ColumnName;

        //            maxWidthes[iCol] = cell.ColumnName.Length + 1;
        //        }

        //        int iRow = ExcelExport_HeaderRow + 1;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            for (int iCol = 0; iCol < totalCols; iCol++)
        //            {
        //                var cell = row[iCol];

        //                ws.Cells[iRow, iCol + 1].Value = cell;

        //                if (cell != null && cell.ToString().Length > maxWidthes[iCol]) maxWidthes[iCol] = cell.ToString().Length + 1;
        //            }
        //            iRow++;
        //        }
        //        for (int iCol = 0; iCol < totalCols; iCol++) ws.Column(iCol + 1).Width = maxWidthes[iCol];

        //        //FormatSheet(ws, indexes);

        //        excelData = pck.GetAsByteArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.ToString();
        //    }

        //    return msg;
        //}
    }
}
