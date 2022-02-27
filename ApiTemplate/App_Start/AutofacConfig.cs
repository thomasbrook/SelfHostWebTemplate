using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ApiTemplate.App_Start
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            //注册mvc的Controller
            builder.RegisterControllers(Assembly.GetCallingAssembly())
                .PropertiesAutowired();

            //注入BLL
            builder.RegisterAssemblyTypes(typeof(ApiTemplate.Bll.Anchor).Assembly)
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .PropertiesAutowired();

            // 注入DAL
            builder.RegisterAssemblyTypes(typeof(ApiTemplate.Sqlite.Dal.BaseDal).Assembly)
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .PropertiesAutowired();

            // 移除原本的mvc的容器，使用AutoFac的容器，将MVC的控制器对象实例交由autofac来创建
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}