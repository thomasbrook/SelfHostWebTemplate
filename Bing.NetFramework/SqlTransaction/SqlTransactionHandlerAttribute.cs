using Bing.NetFramework.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bing.NetFramework.SqlTransaction
{
    /// <summary>
    /// 数据库事务桥接器
    /// </summary>
    public class BaSQLiteBridge : BaSQLiteConnection
    {
        public IDbTransaction DbTransaction { get; set; }

        public BaSQLiteBridge()
        {
            IDbTransaction dbTrans = null;
            this.BeginDbTransaction(ref dbTrans, isSqlTransactionHandlerAttribute: true);
            DbTransaction = dbTrans;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class SqlTransactionHandlerAttribute : Attribute
    {
        public SqlTransactionHandlerAttribute()
        {
            DbTransaction = new BaSQLiteBridge().DbTransaction;
        }

        public IDbTransaction DbTransaction { get; set; }
    }
}
