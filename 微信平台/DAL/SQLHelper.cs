using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

/// <summary>
/// The SqlHelper class is intended to encapsulate high performance, 
/// scalable best practices for common uses of SqlClient.
/// </summary>
public abstract class SqlHelper
{
    public static void updatelog(string name, string content)
    {
        SqlParameter[] spt = SqlHelper.GetCachedParameters("pap_tb_log");
        if (spt == null)
        {
            spt = new SqlParameter[]{
        new SqlParameter("@_name",SqlDbType.VarChar,50),
        new SqlParameter("@_content",SqlDbType.Text)
      
        };
            SqlHelper.CacheParameters("pap_tb_log", spt);
        }
        spt[0].Value = name;
        spt[1].Value = content;

        SqlHelper.ExecuteNonQuery(SqlHelper.SqlConnectionString, CommandType.StoredProcedure, "pap_tb_log", spt);

    }



    //Database connection strings
    public static readonly string SqlConnectionString = ConfigurationManager.ConnectionStrings["CommonConnnection"].ConnectionString;

    // Hashtable to store cached parameters
    private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());
    /// <summary>
    /// 通用分页存储过程，有条件查询，有排序字段，按照排序字段的降序排列
    /// </summary>
    /// <param name="PageSize">每页记录数</param>
    /// <param name="CurrentCount">当前记录数量（页码*每页记录数）</param>
    /// <param name="TableName">表名称</param>
    /// <param name="Where">查询条件，例："ID>1000 AND Name like '%LiLinFeng%'" 排序条件，直接在后面加，例：" ORDER BY ID DESC,NAME ASC"</param>
    /// <param name="TotalCount">记录总数</param>
    /// <returns></returns>
    public static DataSet GetList(string connectionString, string Order, int PageSize, int CurrentCount, string TableName, string Where, out int TotalCount, out int totalItemCount)
    {
        SqlParameter[] parmList =
                {
                    new SqlParameter("@pageSize",PageSize),
                    new SqlParameter("@pageIndex",CurrentCount),
                    new SqlParameter("@TableName",TableName),
                    new SqlParameter("@sqlWhere",Where),
                    new SqlParameter("@OrderField",Order),
                    new SqlParameter("@TotalPage",SqlDbType.Int,4),
                    new SqlParameter("@return",SqlDbType.Int,4)
                };
        parmList[5].Direction = ParameterDirection.Output;
        parmList[6].Direction = ParameterDirection.ReturnValue;
        DataSet ds = ExecuteDataSet(connectionString, CommandType.StoredProcedure, "GetDataList", parmList);
        TotalCount = Convert.ToInt32(parmList[5].Value);
        totalItemCount = Convert.ToInt32(parmList[6].Value);
        return ds;
    }
    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">a valid connection string for a SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {

        SqlCommand cmd = new SqlCommand();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
           
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
    }

    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="conn">an existing database connection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {

        SqlCommand cmd = new SqlCommand();

        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="trans">an existing sql transaction</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>an int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>
    /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">a valid connection string for a SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>A SqlDataReader containing the results</returns>
    public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(connectionString);

        // we use a try/catch here because if the method throws an exception we want to 
        // close the connection throw code, because no datareader will exist, hence the 
        // commandBehaviour.CloseConnection will not work
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }

    /// <summary>
    /// return a dataset
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="cmdType"></param>
    /// <param name="cmdText"></param>
    /// <param name="commandParameters"></param>
    /// <returns>return a dataset</returns>
    public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            da.SelectCommand = cmd;
            da.Fill(ds);

            return ds;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }

    /// <summary>
    /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">a valid connection string for a SqlConnection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
    public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
        SqlCommand cmd = new SqlCommand();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
    }

    /// <summary>
    /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="conn">an existing database connection</param>
    /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">the stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
    /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
    public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {

        SqlCommand cmd = new SqlCommand();

        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        object val = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return val;
    }

    /// <summary>
    /// add parameter array to the cache
    /// </summary>
    /// <param name="cacheKey">Key to the parameter cache</param>
    /// <param name="cmdParms">an array of SqlParamters to be cached</param>
    public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
    {
        parmCache[cacheKey] = commandParameters;
    }

    /// <summary>
    /// Retrieve cached parameters
    /// </summary>
    /// <param name="cacheKey">key used to lookup parameters</param>
    /// <returns>Cached SqlParamters array</returns>
    public static SqlParameter[] GetCachedParameters(string cacheKey)
    {
        SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

        if (cachedParms == null)
            return null;

        SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

        for (int i = 0, j = cachedParms.Length; i < j; i++)
            clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

        return clonedParms;
    }

    /// <summary>
    /// Prepare a command for execution
    /// </summary>
    /// <param name="cmd">SqlCommand object</param>
    /// <param name="conn">SqlConnection object</param>
    /// <param name="trans">SqlTransaction object</param>
    /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
    /// <param name="cmdText">Command text, e.g. Select * from Products</param>
    /// <param name="cmdParms">SqlParameters to use in the command</param>
    private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
    {

        if (conn.State != ConnectionState.Open)
            conn.Open();

        cmd.Connection = conn;
        cmd.CommandText = cmdText;

        if (trans != null)
            cmd.Transaction = trans;

        cmd.CommandType = cmdType;

        if (cmdParms != null)
        {
            foreach (SqlParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }
}

