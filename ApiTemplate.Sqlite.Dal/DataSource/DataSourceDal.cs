using ApiTemplate.Model.Po;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Bing.NetFramework.DbConnection;
using ApiTemplate.Bll.IDal.DataSource;

namespace ApiTemplate.Dal.Sqlite.DataSource
{
    public class DataSourceDal : BaSQLiteConnection, IDataSourceDal
    {
        /// <summary>
        /// 开启事务，数据处理层、数据库连接，都是创建新实例
        /// </summary>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public IDataSourceDal NewInstanceWithTransaction(ref IDbTransaction dbTransaction)
        {
            var dal = new DataSourceDal();
            dal.BeginDbTransaction(ref dbTransaction);
            return dal;
        }

        public bool Delete(string Id)
        {
            throw new NotImplementedException();
        }

        public DataSourcePo Get(string Id)
        {
            using (this)
            {
                var conn = GetConnection();
                DataSourcePo model = conn.Get<DataSourcePo>(Id);
                //model = conn.QuerySingle<DataSourcePo>("SELECT * FROM DatasourceConnection WHERE IID = @IID", new { IID = Id });
                return model;
            }
        }

        public bool Insert(DataSourcePo model)
        {
            using (this)
            {
                var conn = GetConnection();
                var count = conn.Insert<DataSourcePo>(model);

                return count > 0;
            }
        }

        public List<DataSourcePo> List()
        {
            using (this)
            {
                var conn = GetConnection();
                var dataSources = conn.Query<DataSourcePo>("SELECT * FROM DatasourceConnection").ToList();
                return dataSources;
            }
        }

        public bool Update(DataSourcePo model)
        {
            throw new NotImplementedException();
        }

    }
}
