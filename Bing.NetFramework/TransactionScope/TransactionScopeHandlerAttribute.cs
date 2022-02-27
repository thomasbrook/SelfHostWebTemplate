using System;
using System.Transactions;

namespace Bing.NetFramework.TransactionScope
{
    /// <summary>
    /// 开始事务属性
    /// 定义属性，通过当前方法是否包含该属性进行判断开启事务，如果存在该属性则开启事务，否则忽略事务。
    /// 事务属性可以设置超时时间、事务范围以及事务隔离级别。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionScopeHandlerAttribute : Attribute
    {
        public TransactionScopeHandlerAttribute()
        {
            Timeout = 30;
            ScopeOption = TransactionScopeOption.Required;
            IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 事务范围
        /// </summary>
        public TransactionScopeOption ScopeOption { get; set; }
        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel IsolationLevel { get; set; }
    }
}
