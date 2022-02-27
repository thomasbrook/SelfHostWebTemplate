using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace ApiTemplate.Model.Po
{
    [Table("DatasourceConnection")]
    public class DataSourcePo
    {
        [ExplicitKey]
        public string IID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
        public string DatabaseType { get; set; }
        public string SystemName { get; set; }
        [Write(false)]
        public int Port { get; set; }
    }
}
