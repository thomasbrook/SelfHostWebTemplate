using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Threading;
using System.Transactions;

namespace Bing.NetFramework.TransactionScope
{
    public class TransactionScopeInterceptor : IInterceptor
    {
        private readonly static object _TransScopeLock = new object();
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

                var transaction = methodInfo.GetCustomAttributes(typeof(TransactionScopeHandlerAttribute), true).FirstOrDefault();
                if (transaction != null)
                {
                    var trans = transaction as TransactionScopeHandlerAttribute;
                    if (trans == null)
                    {
                        invocation.Proceed();
                        return;
                    }

                    var transOption = new TransactionOptions();
                    transOption.IsolationLevel = trans.IsolationLevel;
                    transOption.Timeout = new TimeSpan(0, 0, trans.Timeout);

                    // 线程级别锁
                    lock (_TransScopeLock)
                    {
                        using (var scope = new System.Transactions.TransactionScope(trans.ScopeOption, transOption))
                        {
                            try
                            {
                                invocation.Proceed();
                                scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
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
