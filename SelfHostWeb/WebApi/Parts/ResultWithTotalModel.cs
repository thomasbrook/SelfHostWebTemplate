using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostWeb.WebApi.Parts
{
    /// <summary>
    /// 带数据总量的数据模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultWithTotalModel<T>
    {
        /// <summary>
        /// 数据总条数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}
