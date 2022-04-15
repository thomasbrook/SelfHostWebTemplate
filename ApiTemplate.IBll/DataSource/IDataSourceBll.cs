using ApiTemplate.Model.Po;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplate.IBll.DataSource
{
    public interface IDataSourceBll
    {
        bool InsertDataSource(DataSourcePo model);

        bool UpdateDataSource(DataSourcePo model);

        bool DeleteDataSource(string Id); 
        bool InsertDataSourceTestWithSqlTransAttribute(DataSourcePo po);
        bool InsertDataSourceTestWithTransScopeAttribute(DataSourcePo po);
        bool InsertDataSourceTestWithSqlTrans(DataSourcePo po);
        DataSourcePo GetDataSource(string Id);
        List<DataSourcePo> ListDataSource();
    }
}
