using ApiTemplate.Bll.IDal.DataSource;
using ApiTemplate.Model.Po;
using Bing.NetFramework.DbConnection;
using Dapper.Contrib.Extensions;
using System.Data;

namespace ApiTemplate.Sqlite.Dal.DataSource
{
    public class DataSourceTestDal : BaSQLiteConnection, IDataSourceTestDal
    {
        /// <summary>
        /// 开启事务，数据处理层、数据库连接，都是创建新实例
        /// </summary>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public IDataSourceTestDal NewInstanceWithTransaction(ref IDbTransaction dbTransaction)
        {
            var dal = new DataSourceTestDal();
            dal.BeginDbTransaction(ref dbTransaction);
            return dal;
        }

        public bool Insert(DataSourcePo po)
        {
            var conn = GetConnectionForBA();
            var count = conn.Insert<DataSourcePo>(po);
            return count > 0;
        }
    }
}
