using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bing.NetFramework.DbConnection
{
    public abstract class SqliteConnector
    {
        protected readonly string SqliteBAConn = "SqliteBAConn";
        protected readonly string SqliteMetaConn = "SqliteMetaConn";

        protected static volatile IDbConnection sqliteBAConn = null;
        protected static volatile IDbConnection sqliteMetaConn = null;

        protected IDbTransaction SqlTrans = null;
        protected static volatile IDbTransaction SqlGlobalTrans = null;
        protected static volatile IDbConnection TransactionScopeConn = null;

        private static object locker = new object();

        protected string GetConnectionStr(string key)
        {
            // 相对路径
            var connString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            if (string.IsNullOrWhiteSpace(connString)) throw new KeyNotFoundException($"{key} connection string is not found.");

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, connString);
            return filePath;
        }


        #region Connection
        protected IDbConnection GetConnectionForBA()
        {
            // 启用 TransactionScopeHandlerAttribute，需要打开连接，才会进入环境事务中。
            if (Transaction.Current != null)
            {
                if (TransactionScopeConn != null
                    && TransactionScopeConn.State == ConnectionState.Open)
                    return TransactionScopeConn;

                var filePath = GetConnectionStr(SqliteBAConn);
                TransactionScopeConn = new SQLiteConnection($"data source={filePath}");

                TransactionScopeConn.Open();
                return TransactionScopeConn;
            }

            // 启用 SqlTransactionHandlerAttribute
            if (SqlGlobalTrans != null)
            {
                return SqlGlobalTrans.Connection;
            }

            // 启用 Transaction
            if (SqlTrans != null)
            {
                return SqlTrans.Connection;
            }

            // 未开启事务
            //if (sqliteBAConn == null)
            //{
            //    lock (locker)
            //    {
            //        if (sqliteBAConn == null)
            //        {
            //            var filePath = GetConnectionStr(SqliteBAConn);
            //            sqliteBAConn = new SQLiteConnection($"data source={filePath}");
            //        }
            //    }
            //}

            sqliteBAConn = new SQLiteConnection($"data source={ GetConnectionStr(SqliteBAConn)}");

            return sqliteBAConn;
        }

        protected IDbConnection GetConnectionForMeta()
        {
            if (SqlTrans != null)
            {
                return SqlTrans.Connection;
            }

            if (sqliteMetaConn == null)
            {
                lock (locker)
                {
                    if (sqliteMetaConn == null)
                    {
                        var filePath = GetConnectionStr(SqliteMetaConn);
                        sqliteMetaConn = new SQLiteConnection($"data source={filePath}");
                    }
                }
            }

            return sqliteMetaConn;
        }

        #endregion
    }
}
