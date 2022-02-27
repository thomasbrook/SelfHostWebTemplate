using ApiTemplate.Bll.IDal;
using ApiTemplate.Bll.IDal.DataSource;
using ApiTemplate.Model.Po;
using Bing.NetFramework.SqlTransaction;
using Bing.NetFramework.TransactionScope;
using SelfHostWeb.IBll.DataSource;
using System;
using System.Collections.Generic;
using System.Data;

namespace ApiTemplate.Bll.DataSource
{

    public class DataSourceBll : IDataSourceBll, ITransactionScopeDependency, ISqlTransactionDependency
    {
        public IDataSourceDal dataSourceDal { get; set; }
        public IDataSourceTestDal dataSourceTestDal { get; set; }
        public IBaseDal baseDal { get; set; }

        public DataSourceBll(IDataSourceDal dataSourceDal, IDataSourceTestDal dataSourceTestDal, IBaseDal baseDal)
        {
            this.dataSourceDal = dataSourceDal;
            this.dataSourceTestDal = dataSourceTestDal;
            this.baseDal = baseDal;
        }

        public bool InsertDataSource(DataSourcePo model)
        {
            return dataSourceDal.Insert(model);
        }

        public bool UpdateDataSource(DataSourcePo model)
        {
            return dataSourceDal.Update(model);
        }

        public bool DeleteDataSource(string Id)
        {
            return dataSourceDal.Delete(Id);
        }

        public DataSourcePo GetDataSource(string id)
        {
            //var result = dataSourceDal.Get(id);
            var result = baseDal.Get<DataSourcePo>(id);
            return result;
        }

        public List<DataSourcePo> ListDataSource()
        {
            return dataSourceDal.List();
        }

        [SqlTransactionHandler]
        public bool InsertDataSourceTestWithSqlTransAttribute(DataSourcePo po)
        {
            dataSourceDal.Insert(po);

            dataSourceTestDal.Insert(po);
            return true;
        }

        [TransactionScopeHandler]
        public bool InsertDataSourceTestWithTransScopeAttribute(DataSourcePo po)
        {
            dataSourceDal.Insert(po);

            dataSourceTestDal.Insert(po);

            return true;
        }

        public bool InsertDataSourceTestWithSqlTrans(DataSourcePo po)
        {
            IDbTransaction dbTrans = null;
            var dal = dataSourceDal.NewInstanceWithTransaction(ref dbTrans);
            DataSourcePo result = null;
            try
            {

                dal.Insert(po);

                var testDal = dataSourceTestDal.NewInstanceWithTransaction(ref dbTrans);
                testDal.Insert(po);

                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                throw ex;
            }
            finally
            {
                dbTrans = null;
            }

            return true;
        }
    }
}
