using ApiTemplate.Model.Po;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplate.Bll.IDal.DataSource
{
    public interface IDataSourceDal
    {
        #region 事务处理
        IDataSourceDal NewInstanceWithTransaction(ref IDbTransaction dbTransaction);
        #endregion

        bool Insert(DataSourcePo model);
        bool Update(DataSourcePo model);

        bool Delete(string Id);

        DataSourcePo Get(string Id);
        List<DataSourcePo> List();
    }
}
