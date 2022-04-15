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
        protected readonly string ConnectionString = "SqliteBAConn";

        protected IDbTransaction SqlTrans = null;
        protected static volatile IDbTransaction SqlGlobalTrans = null;
        protected static volatile IDbConnection TransactionScopeConn = null;

        protected string GetConnectionString(string key)
        {
            // 相对路径
            var connString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            if (string.IsNullOrWhiteSpace(connString)) throw new KeyNotFoundException($"{key} connection string is not found.");

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, connString);
            return filePath;
        }

        protected IDbConnection GetConnection()
        {
            // 启用 TransactionScopeHandlerAttribute，需要打开连接，才会进入环境事务中。
            if (Transaction.Current != null)
            {
                if (TransactionScopeConn != null
                    && TransactionScopeConn.State == ConnectionState.Open)
                    return TransactionScopeConn;

                var filePath = GetConnectionString(ConnectionString);
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

            return new SQLiteConnection($"data source={ GetConnectionString(ConnectionString)}");
        }
    }
}
