using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bing.NetFramework.SqlTransaction
{
    public class SqlTransactionInterceptor : IInterceptor
    {
        private readonly static object _SqlTransLock = new object();
        private bool isDevMode = false;
        public void Intercept(IInvocation invocation)
        {
            if (!isDevMode)
            {
                var methodInfo = invocation.MethodInvocationTarget;
                if (methodInfo == null)
                {
                    methodInfo = invocation.Method;
                }

                var transaction = methodInfo.GetCustomAttributes(typeof(SqlTransactionHandlerAttribute), true).FirstOrDefault();
                if (transaction != null)
                {
                    var trans = transaction as SqlTransactionHandlerAttribute;
                    if (trans == null)
                    {
                        invocation.Proceed();
                        return;
                    }

                    // 线程级别锁
                    lock (_SqlTransLock)
                    {
                        try
                        {
                            invocation.Proceed();
                            trans.DbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.DbTransaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            trans.DbTransaction = null;
                        }
                    }
                    return;
                }

                invocation.Proceed();
                return;
            }

            invocation.Proceed();
            return;
        }
    }
}
