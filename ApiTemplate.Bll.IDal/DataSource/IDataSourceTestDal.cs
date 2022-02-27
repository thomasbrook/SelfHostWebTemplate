using ApiTemplate.Model.Po;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplate.Bll.IDal.DataSource
{
    public interface IDataSourceTestDal
    {
        #region 事务处理
        IDataSourceTestDal NewInstanceWithTransaction(ref IDbTransaction dbTransaction);
        #endregion

        bool Insert(DataSourcePo po);
    }
}
