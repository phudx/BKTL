using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace BKTL
{
    public class DBM : IDisposable
    {
        const int QueryTypeNoOutput = 1;
        const int QueryTypeWithOutputs = 2;
        const int QueryTypeGetDataTable = 3;

        const string PREFIX_VARIABLE_SQL = "@";

        static public string ConnectionString { get; set; }

        static public string AppSettingsDBKey = "MSSQL";
        public SqlConnection SqlConn { get; set; }
        public SqlTransaction SqlTransac { get; set; }

        private string storeName = "";
        private Hashtable htParams = new Hashtable();

        private string queryString = "";

        /// <summary>
        /// Constructor: khởi tạo ConnectionString default (theo key AppSettingsDBKey = "MSSQL") để sử dụng cho các hàm tĩnh. Các hàm tĩnh khi được gọi không cần truyền ConnectionString
        /// </summary>
        static DBM()
        {
            ConnectionString = Common.GetSettingWithDefault(AppSettingsDBKey, "");
        }
        /// <summary>
        /// Constructor: tạo SqlConnection cho object DBM với ConnectionString được truyền vào
        /// </summary>
        public DBM()
        {
            SqlConn = new SqlConnection(ConnectionString);
        }
        /// <summary>
        /// Constructor: gán SqlConnection cho object DBM với SqlConnection được truyền vào
        /// </summary>
        /// <param name="SqlConn"></param>
        public DBM(SqlConnection SqlConn)
        {
            this.SqlConn = SqlConn;
        }



        #region No output: Các hàm tĩnh, cho phép execute store và không có gì trả về (no output)
        // Example: BSS.DBM.ExecStore("sp_test", new { var1, var2, var3 });

        /// <summary>
        /// Hàm tĩnh, cho phép execute store storeName trong DB mà không cần khởi tạo object DBM, dùng default ConnectionString (được set trong key AppSettingsDBKey = "MSSQL" ở file app.config hoặc web.config)
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName)
        {
            List<object> outValue = null;
            return ExecStore(ConnectionString, storeName, new { }, QueryTypeNoOutput, null, out outValue);
        }
        /// <summary>
        /// Hàm tĩnh, cho phép execute store storeName trong DB mà không cần khởi tạo object DBM, với connection string strConnect được truyền vào
        /// </summary>
        /// <param name="strConnect"></param>
        /// <param name="storeName"></param>
        static public string ExecStore(string strConnect, string storeName)
        {
            List<object> outValue = null;
            return ExecStore(strConnect, storeName, new { }, QueryTypeNoOutput, null, out outValue);
        }
        /// <summary>
        /// Hàm tĩnh, cho phép execute store storeName trong DB, 
        /// với tham số parameters mà không cần khởi tạo object DBM, 
        /// dùng default ConnectionString (được set trong key AppSettingsDBKey = "MSSQL" ở file app.config hoặc web.config)
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="parameters"></param>
        static public string ExecStore<T>(string storeName, T parameters) where T : class
        {
            List<object> outValue = null;
            return ExecStore(ConnectionString, storeName, parameters, QueryTypeNoOutput, null, out outValue);
        }
        /// <summary>
        /// Hàm tĩnh, cho phép execute store storeName trong DB, 
        /// với tham số parameters mà không cần khởi tạo object DBM,
        /// với connection string strConnect được truyền vào
        /// </summary>
        /// <param name="strConnect"></param>
        /// <param name="storeName"></param>
        /// <param name="parameters"></param>
        static public string ExecStore<T>(string strConnect, string storeName, T parameters) where T : class
        {
            List<object> outValue = null;
            return ExecStore(strConnect, storeName, parameters, QueryTypeNoOutput, null, out outValue);
        }
        #endregion


        #region One output: Các hàm tĩnh, cho phép execute store và trả về 1 giá trị mà không cần chỉ rõ tên tham số trả về (Trả về 1 giá trị có kiểu là 1 trong các kiểu: string, int, long, object)
        // GetField by Scalar

        #region One output: type string
        /// <summary>
        /// ExecStore and return one output with type of string
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, out string v)
        {
            return ExecStore(ConnectionString, storeName, new { }, null, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, out string v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, null, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, out string v) where T : class
        {
            return ExecStore(strConnect, storeName, parameters, null, out v);
        }
        #endregion

        #region One output: type int
        /// <summary>
        /// ExecStore and return one output with type of int
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, out int v)
        {
            return ExecStore(ConnectionString, storeName, new { }, null, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, out int v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, null, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, out int v) where T : class
        {
            return ExecStore(strConnect, storeName, parameters, null, out v);
        }
        #endregion

        #region One output: type long
        /// <summary>
        /// ExecStore and return one output with type of long
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, out long v)
        {
            return ExecStore(ConnectionString, storeName, new { }, null, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, out long v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, null, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, out long v) where T : class
        {
            return ExecStore(strConnect, storeName, parameters, null, out v);
        }
        #endregion

        #region One output: type bool
        /// <summary>
        /// ExecStore and return one output with type of bool
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, out bool v)
        {
            return ExecStore(ConnectionString, storeName, new { }, null, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, out bool v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, null, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, out bool v) where T : class
        {
            return ExecStore(strConnect, storeName, parameters, null, out v);
        }
        #endregion

        #region One output: type Datatable
        /// <summary>
        /// ExecStore and return one output with type of DataTable
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, out DataTable dt)
        {
            return ExecStore(ConnectionString, storeName, null, out dt);
        }
        static public string ExecStore(string strConnect, string storeName, out DataTable dt)
        {
            return ExecStore(strConnect, storeName, null, out dt);
        }
        static public string ExecStore<T>(string storeName, T parameters, out DataTable dt) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, out dt);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, out DataTable dt) where T : class
        {
            Hashtable htParams = new Hashtable();
            AddParams(ref htParams, parameters);

            return ExecStore(strConnect, storeName, htParams, out dt);
        }
        static public string ExecStore(string strConnect, string storeName, Hashtable htParams, out DataTable dt)
        {
            dt = null;

            List<object> outValue = null;
            string msg = ExecStore(strConnect, storeName, htParams, QueryTypeGetDataTable, null, out outValue);
            if (msg.Length > 0) return msg;

            if (outValue == null) return "Error: outValue is null @ExecStore";
            if (outValue.Count == 0) return "Error: outValue has no value @ExecStore";

            dt = outValue[0] as DataTable; // cast về DataTable. Trả về null nếu không cast được

            return msg;
        }
        #endregion

        #region One output: type object
        /// <summary>
        /// ExecStore and return one output with type of object
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, out object v)
        {
            return ExecStore(ConnectionString, storeName, new { }, null, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, out object v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, null, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, out object v) where T : class
        {
            return ExecStore(strConnect, storeName, parameters, null, out v);
        }
        #endregion
        #endregion


        #region One output: Các hàm tĩnh, cho phép execute store và trả về 1 giá trị của tham số có tên được chỉ định (Trả về 1 giá trị có kiểu là 1 trong các kiểu: string, int, long, object)
        // GetField by Output Var      

        #region One output: type Guid
        /// <summary>
        /// ExecStore with storeName, params, out param outVarName and return one output with type of Guid
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="outVarName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, string outVarName, out Guid v)
        {
            return ExecStore(ConnectionString, storeName, new { }, outVarName, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, string outVarName, out Guid v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, outVarName, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, string outVarName, out Guid v) where T : class
        {
            v = Guid.Empty;

            object outValue = null;
            string msg = ExecStore(strConnect, storeName, parameters, outVarName, out outValue);
            if (msg.Length > 0) return msg;

            if (outValue == null) return "Error: Out value is null @ExecStore";

            msg = Convertor.StringToGuid(outValue.ToString(), out v);

            return msg;
        }
        #endregion

        #region One output: type string
        /// <summary>
        /// ExecStore with storeName, params, out param outVarName and return one output with type of string
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="outVarName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, string outVarName, out string v)
        {
            return ExecStore(ConnectionString, storeName, new { }, outVarName, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, string outVarName, out string v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, outVarName, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, string outVarName, out string v) where T : class
        {
            v = null;

            object outValue = null;
            string msg = ExecStore(strConnect, storeName, parameters, outVarName, out outValue);
            if (msg.Length > 0) return msg;

            if (outValue == null) return "Error: Out value is null @ExecStore";

            v = Convert.ToString(outValue);

            return msg;
        }
        #endregion

        #region One output: type int
        /// <summary>
        /// ExecStore with storeName, params, out param outVarName and return one output with type of int
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="outVarName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, string outVarName, out int v)
        {
            return ExecStore(ConnectionString, storeName, new { }, outVarName, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, string outVarName, out int v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, outVarName, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, string outVarName, out int v) where T : class
        {
            v = 0;

            object outValue = null;
            string msg = ExecStore(strConnect, storeName, parameters, outVarName, out outValue);
            if (msg.Length > 0) return msg;

            if (outValue == null) return "Error: Out value is null @ExecStore";

            if (!int.TryParse(outValue.ToString(), out v))
                msg = "Error: Cannot convert out value to int @ExecStore";

            return msg;
        }
        #endregion

        #region One output: type long
        /// <summary>
        /// ExecStore with storeName, params, out param outVarName and return one output with type of long
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="outVarName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, string outVarName, out long v)
        {
            return ExecStore(ConnectionString, storeName, new { }, outVarName, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, string outVarName, out long v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, outVarName, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, string outVarName, out long v) where T : class
        {
            v = 0;

            object outValue = null;
            string msg = ExecStore(strConnect, storeName, parameters, outVarName, out outValue);
            if (msg.Length > 0) return msg;

            if (outValue == null) return "Error: Out value is null @ExecStore";

            if (!long.TryParse(outValue.ToString(), out v))
                msg = "Error: Cannot convert out value to long @ExecStore";

            return msg;
        }
        #endregion

        #region One output: type bool
        /// <summary>
        /// ExecStore with storeName, params, out param outVarName and return one output with type of bool
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="outVarName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, string outVarName, out bool v)
        {
            return ExecStore(ConnectionString, storeName, new { }, outVarName, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, string outVarName, out bool v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, outVarName, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, string outVarName, out bool v) where T : class
        {
            v = false;

            object outValue = null;
            string msg = ExecStore(strConnect, storeName, parameters, outVarName, out outValue);
            if (msg.Length > 0) return msg;

            if (outValue == null) return "Error: Out value is null @ExecStore";

            if (!bool.TryParse(outValue.ToString(), out v))
                msg = "Error: Cannot convert out value to bool @ExecStore";

            return msg;
        }
        #endregion

        #region One output: type object
        /// <summary>
        /// ExecStore with storeName, params, out param outVarName and return one output with type of object
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="outVarName"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, string outVarName, out object v)
        {
            return ExecStore(ConnectionString, storeName, new { }, outVarName, out v);
        }
        static public string ExecStore<T>(string storeName, T parameters, string outVarName, out object v) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, outVarName, out v);
        }
        static public string ExecStore(string strConnect, string storeName, string outVarName, out object v)
        {
            return ExecStore(strConnect, storeName, new { }, outVarName, out v);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, string outVarName, out object v) where T : class
        {
            v = null;
            List<string> outVarNames = null;
            List<object> vs = null;

            if (!string.IsNullOrWhiteSpace(outVarName)) outVarNames = new List<string> { outVarName };
            string msg = ExecStore(strConnect, storeName, parameters, QueryTypeWithOutputs, outVarNames, out vs);
            if (msg.Length > 0) return msg;

            if (vs == null) return "Error: outValue is null @ExecStore";
            if (vs.Count == 0) return "Error: outValue has no value @ExecStore";

            v = vs[0];
            return "";
        }
        #endregion

        #region Many outputs: type List<object>
        /// <summary>
        /// ExecStore with storeName, params, out param outVarName and return one output with type of object
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="outVarNames"></param>
        /// <param name="vs"></param>
        /// <returns></returns>
        static public string ExecStore(string storeName, List<string> outVarNames, out List<object> vs)
        {
            return ExecStore(ConnectionString, storeName, new { }, QueryTypeWithOutputs, outVarNames, out vs);
        }
        static public string ExecStore<T>(string storeName, T parameters, List<string> outVarNames, out List<object> vs) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, QueryTypeWithOutputs, outVarNames, out vs);
        }
        static public string ExecStore<T>(string strConnect, string storeName, T parameters, List<string> outVarNames, out List<object> vs) where T : class
        {
            return ExecStore(strConnect, storeName, parameters, QueryTypeWithOutputs, outVarNames, out vs);
        }
        #endregion
        #endregion


        #region Hàm private, phục vụ các hàm tĩnh execute store ở trên
        // Hàm tĩnh, cho phép execute store trong DB với SqlConn, SqlTransac, storeName, và DS tham số htParam được truyền vào
        // Đây là hàm "nguyên tố", được nạp chồng bởi các hàm khác, nên có đầy đủ các tham số để có thể "tự chạy"
        // Hàm này tự tạo SqlConn, dùng để chạy các câu query đơn, không có transaction
        // Example: BSS.DBM.ExecStore("sp_test", new { var1, var2, var3 }, "Name", out sName);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeName"></param>
        /// <param name="parameters"></param>
        /// <param name="queryType"></param>
        /// <param name="outVarNames"></param>
        /// <param name="outValues"></param>
        /// <returns></returns>
        static private string ExecStore<T>(string storeName, T parameters, int queryType, List<string> outVarNames, out List<object> outValues) where T : class
        {
            return ExecStore(ConnectionString, storeName, parameters, queryType, outVarNames, out outValues);
        }
        static private string ExecStore<T>(string strConnect, string storeName, T parameters, int queryType, List<string> outVarNames, out List<object> outValues) where T : class
        {
            Hashtable htParams = new Hashtable();
            AddParams(ref htParams, parameters);

            return ExecStore(strConnect, storeName, htParams, queryType, outVarNames, out outValues);

        }
        static private string ExecStore(string strConnect, string storeName, Hashtable htParams, int queryType, List<string> outVarNames, out List<object> outValues)
        {
            outValues = null;
            if (string.IsNullOrWhiteSpace(strConnect)) return "Error: Connection string is null or empty @ExecStore";

            using (SqlConnection SqlConn = new SqlConnection(strConnect))
            {
                return ExecStore(SqlConn, null, storeName, htParams, queryType, outVarNames, out outValues);
            }
        }
        #endregion


        #region Hàm nguyên tố tĩnh: cho phép execute store trong DB với SqlConn, SqlTransac, storeName, và DS tham số htParam được truyền vào
        // Đây là hàm "nguyên tố", được nạp chồng bởi các hàm khác, nên có đầy đủ các tham số để có thể "tự chạy"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlConn"></param>
        /// <param name="sqlTransac"></param>
        /// <param name="storeName"></param>
        /// <param name="parameters"></param>
        /// <param name="queryType"></param>
        /// <param name="outVarNames"></param>
        /// <param name="outValues"></param>
        /// <returns></returns>
        static private string ExecStore<T>(SqlConnection sqlConn, SqlTransaction sqlTransac, string storeName, T parameters, int queryType, List<string> outVarNames, out List<object> outValues) where T : class
        {
            Hashtable htParams = new Hashtable();
            AddParams(ref htParams, parameters);

            return ExecStore(sqlConn, sqlTransac, storeName, htParams, queryType, outVarNames, out outValues);
        }

        static private string ExecStore(SqlConnection sqlConn, SqlTransaction sqlTransac, string storeName, Hashtable htParams, int queryType, List<string> outVarNames, out List<object> outValues)
        {
            outValues = null;

            if (sqlConn == null) return "Error: SqlConn is null @ExecStore";

            string msg = "";
            using (SqlCommand scmCmdToExecute = new SqlCommand(storeName, sqlConn))
            {
                if (sqlTransac != null) scmCmdToExecute.Transaction = sqlTransac;

                scmCmdToExecute.CommandType = CommandType.StoredProcedure;

                if (htParams != null)
                {
                    foreach (DictionaryEntry p in htParams)
                    {
                        SqlParameter sp = null;

                        if (p.Value == null) sp = new SqlParameter(p.Key.ToString(), System.DBNull.Value);
                        else if (p.Value.GetType().IsArray)
                        {
                            sp = new SqlParameter(p.Key.ToString(), SqlDbType.Binary);
                            sp.Value = p.Value;
                        }
                        else sp = new SqlParameter(p.Key.ToString(), p.Value);

                        scmCmdToExecute.Parameters.Add(sp);
                    }
                }

                List<SqlParameter> spOuts = null;
                if (outVarNames != null)
                {
                    spOuts = new List<SqlParameter>();
                    foreach (string outVarName in outVarNames)
                    {
                        SqlParameter spOut = new SqlParameter();

                        spOut = new SqlParameter(outVarName, System.DBNull.Value);
                        spOut.Direction = ParameterDirection.Output;
                        spOut.Size = 1000;
                        scmCmdToExecute.Parameters.Add(spOut);

                        spOuts.Add(spOut);
                    }
                }
                try
                {
                    if (sqlConn.State != ConnectionState.Open) sqlConn.Open();
                    switch (queryType)
                    {
                        case QueryTypeNoOutput: scmCmdToExecute.ExecuteNonQuery(); break;
                        case QueryTypeWithOutputs:
                            outValues = new List<object>();

                            if (outVarNames != null)
                            {
                                scmCmdToExecute.ExecuteNonQuery();

                                foreach (SqlParameter spOut in spOuts) outValues.Add(spOut.Value);
                            }
                            else outValues.Add(scmCmdToExecute.ExecuteScalar());
                            break;
                        case QueryTypeGetDataTable:
                            using (var ds = new DataSet())
                            {
                                using (var dad = new SqlDataAdapter(scmCmdToExecute))
                                {
                                    dad.Fill(ds);
                                    outValues = new List<object>();
                                    outValues.Add(ds.Tables[0]);
                                }
                            }
                            break;
                        default: msg = "Error: QueryType is unknown @ExecStore"; break;
                    }
                }
                catch (Exception ex)
                {
                    msg = "SQL query error at " + storeName + ": " + ex.ToString();
                }
                finally
                {
                    if (sqlTransac == null)
                        if (sqlConn.State != ConnectionState.Closed) sqlConn.Close();
                }
            }
            return msg;
        }
        #endregion


        #region Các hàm động, cho phép execute store với SqlConn, SqlTransac, storeName, htParam đã được set với object DBM. Dùng khi cần có transaction với các câu query khác
        // Ví dụ: BSS.DBM dbm = new BSS.DBM();
        //        dbm.BeginTransac()
        //        dbm.SetStoreNameAndParams("sp_test", new { var1, var2, var3 });
        //        dbm.ExecStore();
        //        dbm.CommitTransac();

        /// <summary> Hàm thực thi các store sau khi chạy lệnh SetStoreNameAndParams để điền các tham số đầu vào và tên store cần thực thi
        /// <para>BSS.DBM dbm = new BSS.DBM();</para>
        /// <para>string err = dbm.SetStoreNameAndParams("dbo.sp_LogActivity_Insert", new { LogContent = "Enter log", UserID = 20, ObjectGUID = Guid.NewGuid(), IP = "10.3.4.43" }).ExecStore();</para>
        /// </summary>
        /// <returns>Giá trị trả về rỗng là thành công, còn khác rỗng là lỗi</returns>
        public string ExecStore()
        {
            List<object> outValue = null;
            return ExecStore(SqlConn, SqlTransac, storeName, htParams, QueryTypeNoOutput, null, out outValue);
        }
        /// <summary>
        /// Hàm thực thi các store sau khi chạy lệnh SetStoreNameAndParams để điền các tham số đầu vào và tên store cần thực thi và có giá trị trả ra 
        /// </summary>
        /// <param name="outVarNames">Tên biến quy định là biến output trong store</param>
        /// <param name="outValues">Biến sẽ được gán giá trị sau khi chạy</param>
        /// <returns>Giá trị trả về rỗng là thành công, còn khác rỗng là lỗi</returns>
        public string ExecStore(List<string> outVarNames, out List<object> outValues)
        {
            outValues = null;
            return ExecStore(SqlConn, SqlTransac, storeName, htParams, QueryTypeWithOutputs, outVarNames, out outValues);
        }
        /// <summary>
        /// Hàm thực thi các store sau khi chạy lệnh SetStoreNameAndParams để điền các tham số đầu vào và tên store cần thực thi và có giá trị trả ra
        /// BSS.DBM dbm = new BSS.DBM();
        /// DataTable dt = null;
        /// string err = obj.SetStoreNameAndParams("dbo.sp_LogActivity_SelectOne", new { ID = 1 });
        /// err = ExecStore(out dt);
        /// </summary>
        /// <param name="dt">Table chứa giá trị sau khi chạy</param>
        /// <returns>Giá trị trả về rỗng là thành công, còn khác rỗng là lỗi</returns>
        public string ExecStore(out DataTable dt)
        {
            dt = null;

            List<object> outValues = null;
            string msg = ExecStore(SqlConn, SqlTransac, storeName, htParams, QueryTypeGetDataTable, null, out outValues);
            if (msg.Length > 0) return msg;

            if (outValues.Count == 0) return "Error: No out value @ExecStore";

            dt = outValues[0] as DataTable; // cast về DataTable. Trả về null nếu không cast được

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string SetStoreNameAndParams<T>(string storeName, T parameters) where T : class
        {
            this.storeName = storeName;
            htParams.Clear();

            return AddParams(parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DBM ClearParams()
        {
            htParams.Clear();
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string AddParams<T>(T parameters) where T : class
        {
            return AddParams(ref htParams, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htParams"></param>
        /// <param name="parameters"></param>
        static public string AddParams<T>(ref Hashtable htParams, T parameters) where T : class
        {
            if (parameters == null) return "Error: parameters is null @DBM.AddParams";

            string msg = "";
            try
            {
                var ps = typeof(T).GetProperties();
                foreach (var p in ps)
                    htParams.Add(PREFIX_VARIABLE_SQL + p.Name, p.GetValue(parameters, null));
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenConnection()
        {
            if (SqlConn.State != ConnectionState.Open)
                SqlConn.Open();
        }
        /// <summary>
        /// 
        /// </summary>
        public void CloseConnection()
        {
            if (SqlTransac == null)
            {
                if (SqlConn.State != ConnectionState.Closed)
                    SqlConn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void BeginTransac()
        {
            if (SqlConn.State != ConnectionState.Open)
                SqlConn.Open();
            SqlTransac = SqlConn.BeginTransaction();
        }
        /// <summary>
        /// 
        /// </summary>
        public void CommitTransac()
        {
            if (SqlTransac != null)
            {
                SqlTransac.Commit();
                SqlTransac = null;
            }
            if (SqlConn.State != ConnectionState.Closed)
                SqlConn.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        public void RollBackTransac()
        {
            if (SqlTransac != null)
            {
                SqlTransac.Rollback();
                SqlTransac = null;
            }
            if (SqlConn.State != ConnectionState.Closed)
                SqlConn.Close();
        }
        /// <summary>
        /// Dispose object DBM
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            CloseConnection();
            if (null != SqlConn) SqlConn.Dispose();
        }
        #endregion


        #region Các hàm ExecQuery bằng câu lênh trực tiếp
        /// <summary>
        /// Thực thi bằng câu lênh trực tiếp
        /// </summary>       
        /// <param name="_QueryString"></param>
        /// <returns></returns>
        public string ExecQueryString()
        {
            return ExecQueryString(SqlConn, SqlTransac, queryString);
        }

        public DBM SetQueryString(string queryString)
        {
            this.queryString = queryString;
            return this;
        }

        /// <summary>
        /// Thực thi bằng câu lênh trực tiếp
        /// </summary>       
        /// <param name="_QueryString"></param>
        /// <returns></returns>
        static public string ExecQueryString(string _QueryString)
        {
            return ExecQueryString(ConnectionString, _QueryString);
        }
        static public string ExecQueryString(string strConnect, string _QueryString)
        {
            using (SqlConnection SqlConn = new SqlConnection(strConnect))
            {
                return ExecQueryString(SqlConn, null, _QueryString);
            }
        }
        /// <summary>
        /// Thực thi bằng câu lênh trực tiếp
        /// </summary>
        /// <param name="_SqlConn"></param>
        /// <param name="_SqlTransac"></param>
        /// <param name="_QueryString"></param>
        /// <returns></returns>
        static private string ExecQueryString(SqlConnection _SqlConn, SqlTransaction _SqlTransac, string _QueryString)
        {
            if (_SqlConn == null) return "Error: SqlConn is null @DBM.ExecQueryString";
            if (string.IsNullOrEmpty(_QueryString)) return "Error: QueryString is null @DBM.ExecQueryString";
            string msg = "";
            using (SqlCommand scmCmdToExecute = new SqlCommand(_QueryString, _SqlConn))
            {
                if (_SqlTransac != null) scmCmdToExecute.Transaction = _SqlTransac;
                scmCmdToExecute.CommandType = CommandType.Text;
                try
                {
                    if (_SqlConn.State != ConnectionState.Open) _SqlConn.Open();
                    scmCmdToExecute.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    msg = "SQL query error : " + ex.ToString();
                }
                finally
                {
                    if (_SqlTransac == null)
                        if (_SqlConn.State != ConnectionState.Closed) _SqlConn.Close();
                }
            }
            return msg;
        }
        #endregion


        #region Các hàm convert từ 1 Row trong DataTable về 1 Object; từ 1 DataTable về 1 list các Object
        /// <summary>
        /// Hàm để lấy một đối tượng sau khi chạy lệnh SetStoreNameAndParams để điền các tham số đầu vào và tên store cần thực thi và có giá trị trả ra
        /// BSS.DBM dbm = new BSS.DBM();
        /// T obj = null;
        /// string err = obj.SetStoreNameAndParams("dbo.sp_LogActivity_SelectOne", new { ID = 1 }).ExecStore(out obj);
        /// </summary>
        /// <param name="oneItem">Đối tượng chứa giá trị sau khi chạy</param>
        /// <returns>Giá trị trả về rỗng là thành công, còn khác rỗng là lỗi</returns>
        public string GetOne<T>(out T oneItem) where T : class
        {
            return GetOne(this, storeName, htParams, out oneItem);
        }
        /// <summary>
        /// Hàm để lấy danh sách đối tượng sau khi chạy lệnh SetStoreNameAndParams để điền các tham số đầu vào và tên store cần thực thi và có giá trị trả ra
        /// BSS.DBM dbm = new BSS.DBM();
        /// List lt = null;
        /// string err = obj.SetStoreNameAndParams("dbo.sp_LogActivity_SelectOne", new { ID = 1 }).ExecStore(out lt);
        /// </summary>
        /// <param name="list">List chứa giá trị sau khi chạy</param>
        /// <returns>Giá trị trả về rỗng là thành công, còn khác rỗng là lỗi</returns>
        public string GetList<T>(out List<T> list) where T : class
        {
            return GetList(this, storeName, htParams, out list);
        }

        /// <summary>
        /// Hàm tĩnh để lấy 1 đối tượng
        /// LogActivity objLogActivity = null;
        /// DataTable dt = null;
        /// string err = DBM.GetOne("dbo.sp_LogActivity_SelectOne", new { ID = 5 }, out objLogActivity);
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng chứa biến</typeparam>
        /// <typeparam name="TL">Kiểu đối tượng cần lấy</typeparam>
        /// <param name="storeName">Tên store</param>
        /// <param name="parameters">Đối tượng chứa biến chuyền vào</param>
        /// <param name="oneItem">Đối tượng trả ra</param>
        /// <returns>Giá trị trả về rỗng là thành công, còn khác rỗng là lỗi</returns>
        static public string GetOne<T, TL>(string storeName, T parameters, out TL oneItem) where T : class
        {
            Hashtable htParams = new Hashtable();
            AddParams(ref htParams, parameters);

            return GetOne(null, storeName, htParams, out oneItem);
        }
        /// <summary> 
        /// Hàm tĩnh để lấy danh sách đối tượng
        /// List clsLogActivity objListLogActivity = null;
        /// DataTable dt = null;
        /// string err = DBM.GetList("dbo.sp_LogActivity_SelectOne", new { ID = 5 }, out objListLogActivity); 
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng chứa biến</typeparam>
        /// <typeparam name="TL">Kiểu đối tượng cần lấy</typeparam>
        /// <param name="storeName">Tên store</param>
        /// <param name="parameters">Đối tượng chứa biến chuyền vào</param>
        /// <param name="list">Đối tượng trả ra</param>
        /// <returns>Giá trị trả về rỗng là thành công, còn khác rỗng là lỗi</returns>
        static public string GetList<T, TL>(string storeName, T parameters, out List<TL> list) where T : class
        {
            Hashtable htParams = new Hashtable();
            AddParams(ref htParams, parameters);

            return GetList(null, storeName, htParams, out list);
        }

        static private string GetOne<T>(DBM dbm, string storeName, Hashtable htParams, out T oneItem)
        {
            oneItem = default(T);

            List<T> list = null;
            string msg = GetList(dbm, storeName, htParams, out list);
            if (msg.Length > 0) return msg;

            if (list == null) return "Error: list is null @DBM.GetOne";
            if (list.Count == 0) return "";

            oneItem = list[0];

            return msg;
        }
        static private string GetList<T>(DBM dbm, string storeName, Hashtable htParams, out List<T> list)
        {
            string msg = "";
            list = null;

            DataTable dt = null;
            if (dbm == null) msg = ExecStore(ConnectionString, storeName, htParams, out dt);
            else msg = dbm.ExecStore(out dt);

            if (msg.Length > 0) return msg;

            msg = DataTableToList(dt, out list);

            return msg;
        }

        /// <summary>
        /// Convert 1 DataTable to 1 list of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string DataTableToList<T>(DataTable table, out List<T> list)
        {
            list = null;
            if (table == null) return "Error: table is null @DataTableToList";

            list = new List<T>();

            string msg = "";
            foreach (DataRow row in table.Rows)
            {
                T item = default(T);
                msg = RowToObject<T>(row, out item);
                if (msg.Length == 0 && item == null) msg = "Error: item is null @DataTableToList";

                if (msg.Length > 0)
                {
                    list.Clear();
                    list = null;
                    return msg;
                }

                list.Add(item);
            }

            return msg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string RowToObject<T>(DataRow row, out T value)
        {
            value = default(T);

            if (row == null) return "Error: row is null @RowToObject";

            string columnName = "";
            try
            {
                value = Activator.CreateInstance<T>();
                Type type = value.GetType();
                foreach (DataColumn column in row.Table.Columns)
                {
                    columnName = column.ColumnName;
                    PropertyInfo prop = type.GetProperty(columnName);
                    if (prop == null) continue;

                    object cell = row[columnName];
                    if (cell != DBNull.Value) prop.SetValue(value, cell, null);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString() + " @" + columnName;
            }
            return "";
        }
        #endregion
    }
}