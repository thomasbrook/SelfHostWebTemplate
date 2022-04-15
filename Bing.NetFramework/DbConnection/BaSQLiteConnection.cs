﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Bing.NetFramework.DbConnection
{
    public class BaSQLiteConnection : SqliteConnector, IDisposable
    {
        #region Transaction
        protected IDbTransaction BeginDbTransaction(ref IDbTransaction dbTransaction, bool isSqlTransactionHandlerAttribute = false)
        {
            if (dbTransaction == null)
            {
                var filePath = GetConnectionString(ConnectionString);
                var sqliteConn = new SQLiteConnection($"data source={filePath}");

                sqliteConn.Open();

                dbTransaction = sqliteConn.BeginTransaction();
            }

            if (isSqlTransactionHandlerAttribute) SqlGlobalTrans = dbTransaction;
            else SqlTrans = dbTransaction;

            return SqlTrans;
        }

        public void CommitDbTransaction()
        {
            if (SqlTrans != null)
            {
                SqlTrans.Commit();
                SqlTrans = null;
            }
        }

        public void RollbackDbTransaction()
        {
            if (SqlTrans != null)
            {
                SqlTrans.Rollback();
                SqlTrans = null;
            }
        }
        public void Dispose()
        {
            if (Transaction.Current != null)
            {
                return;
            }

            if (SqlGlobalTrans != null)
            {
                return;
            }

            if (SqlTrans != null)
            {
                return;
            }
           
        }

        #endregion


    }
}
