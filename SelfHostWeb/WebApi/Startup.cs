using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac.Integration.WebApi;
using Swashbuckle.Application;
using Bing.NetFramework.TransactionScope;
using Bing.NetFramework.SqlTransaction;
using System.Net.Http;
using SelfHostWeb.SwaggerExtension;
using ApiTemplate.Sqlite.Dal;

namespace SelfHostWeb.WebApi
{
    class Startup
    {
        private static string _siteDir = ConfigurationManager.AppSettings.Get("SiteDir");

        public void Configuration(IAppBuilder app)
        {
            // web api 接口
            HttpConfiguration config = InitWebApiConfig();
            app.UseWebApi(config);

            //静态文件
            app.Use((context, fun) =>
            {
                return StaticWebFileHandel(context, fun);
            });

            ConfigureAutofac(config);
            ConfigureSwagger(config);
        }

        /// <summary>
        /// 路由初始化
        /// </summary>
        public HttpConfiguration InitWebApiConfig()
        {
            var config = new HttpConfiguration();

            //config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );

            // 加载外部程序集
            Assembly.Load("SelfHostWeb.Controller");

            // 配置 http 服务的路由
            var cors = new EnableCorsAttribute("*", "*", "*");//跨域允许设置
            config.EnableCors(cors);

            config.Formatters
               .XmlFormatter.SupportedMediaTypes.Clear();

            //默认返回 json
            config.Formatters
                .JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "json", "application/json"));

            //返回格式选择
            config.Formatters
                .XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "xml", "application/xml"));

            //json 序列化设置
            config.Formatters
                .JsonFormatter.SerializerSettings = new
                Newtonsoft.Json.JsonSerializerSettings()
                {
                    //NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    DateFormatString = "yyyy-MM-dd HH:mm:ss" //设置时间日期格式化
                };

            return config;
        }

        #region 静态文件
        /// <summary>
        /// 客户端请求静态文件处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task StaticWebFileHandel(IOwinContext context, Func<Task> next)
        {
            //获取物理文件路径
            var path = GetFilePath(context.Request.Path.Value);

            //验证路径是否存在
            if (!File.Exists(path) && !path.EndsWith("html"))
            {
                path += ".html";
            }

            if (File.Exists(path))
            {
                return SetResponse(context, path);
            }

            //不存在返回下一个请求
            return next();
        }
        public static string GetFilePath(string relPath)
        {
            if (relPath.IndexOf("index") > -1)
            {
                String temp = relPath;
            }

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = relPath.TrimStart('/').Replace('/', '\\');
            if (_siteDir == ".")
            {
                return Path.Combine(basePath, filePath);
            }
            else
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _siteDir == "." ? "" : _siteDir, relPath.Replace('/', '\\')).TrimStart('\\');
            }
        }
        public Task SetResponse(IOwinContext context, string path)
        {
            /*
                .txt text/plain
                RTF文本 .rtf application/rtf
                PDF文档 .pdf application/pdf
                Microsoft Word文件 .word application/msword
                PNG图像 .png image/png
                GIF图形 .gif image/gif
                JPEG图形 .jpeg,.jpg image/jpeg
                au声音文件 .au audio/basic
                MIDI音乐文件 mid,.midi audio/midi,audio/x-midi
                RealAudio音乐文件 .ra, .ram audio/x-pn-realaudio
                MPEG文件 .mpg,.mpeg video/mpeg
                AVI文件 .avi video/x-msvideo
                GZIP文件 .gz application/x-gzip
                TAR文件 .tar application/x-tar
                任意的二进制数据 application/octet-stream
             */

            var perfix = Path.GetExtension(path);
            if (perfix == ".html")
                context.Response.ContentType = "text/html; charset=utf-8";
            else if (perfix == ".txt")
                context.Response.ContentType = "text/plain";
            else if (perfix == ".js")
                context.Response.ContentType = "application/x-javascript";
            else if (perfix == ".css")
                context.Response.ContentType = "text/css";
            else
            {
                if (perfix == ".jpeg" || perfix == ".jpg")
                    context.Response.ContentType = "image/jpeg";
                else if (perfix == ".gif")
                    context.Response.ContentType = "image/gif";
                else if (perfix == ".png")
                    context.Response.ContentType = "image/png";
                else if (perfix == ".svg")
                    context.Response.ContentType = "image/svg+xml";
                else if (perfix == ".woff")
                    context.Response.ContentType = "application/font-woff";
                else if (perfix == ".woff2")
                    context.Response.ContentType = "application/font-woff2";
                else if (perfix == ".ttf")
                    context.Response.ContentType = "application/octet-stream";

                return context.Response.WriteAsync(File.ReadAllBytes(path));
            }

            //truetype
            return context.Response.WriteAsync(File.ReadAllText(path));
        }
        #endregion

        #region autofac

        private static void ConfigureAutofac(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            //注册 api Controller
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();

            //注入BLL 
            var bllAssembly = typeof(ApiTemplate.Bll.Anchor).Assembly;
            builder.RegisterAssemblyTypes(bllAssembly)
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired();

            // 注入DAL 
            var dalAssembly = typeof(ApiTemplate.Sqlite.Dal.BaseDal).Assembly;
            builder.RegisterAssemblyTypes(dalAssembly)
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired();

            // 注入通用dal
            builder.RegisterType(typeof(BaseDal))
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired();

            // 注入TransactionScope事务拦截器
            builder.RegisterType<TransactionScopeInterceptor>();
            builder.RegisterAssemblyTypes(bllAssembly)
                .Where(type => typeof(ITransactionScopeDependency).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionScopeInterceptor));

            // 注入SqlTransaction事务拦截器
            builder.RegisterType<SqlTransactionInterceptor>();
            builder.RegisterAssemblyTypes(bllAssembly)
                .Where(type => typeof(ISqlTransactionDependency).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(SqlTransactionInterceptor));

            // 移除原本的webApi的容器，使用AutoFac的容器，将 webApi 的控制器对象实例交由autofac来创建
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        #endregion

        #region swagger

        private static string GetRootUrlFromAppConfig(HttpRequestMessage message)
        {
            var virtualPathRoot = message.GetRequestContext().VirtualPathRoot;
            var schemeAndHost = $"http://{message.RequestUri.Host}:{ConfigurationManager.AppSettings.Get("Port")}";
            return new Uri(new Uri(schemeAndHost, UriKind.Absolute), virtualPathRoot).AbsoluteUri;
        }

        private static void ConfigureSwagger(HttpConfiguration config)
        {
            //var thisAssembly = typeof(Startup).Assembly;
            var parentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            config.EnableSwagger(c =>
            {
                // By default, the service root url is inferred from the request used to access the docs.
                // However, there may be situations (e.g. proxy and load-balanced environments) where this does not
                // resolve correctly. You can workaround this by providing your own code to determine the root URL.
                //
                c.RootUrl(req => GetRootUrlFromAppConfig(req));

                // If schemes are not explicitly provided in a Swagger 2.0 document, then the scheme used to access
                // the docs is taken as the default. If your API supports multiple schemes and you want to be explicit
                // about them, you can use the "Schemes" option as shown below.
                //
                c.Schemes(new[] { "http", "https" });

                // Use "SingleApiVersion" to describe a single version API. Swagger 2.0 includes an "Info" object to
                // hold additional metadata for an API. Version and title are required but you can also provide
                // additional fields by chaining methods off SingleApiVersion.
                //
                c.SingleApiVersion("v1", "SelfHostWeb")
                .Description("webApi 文档")
                //.Contact(build =>
                //{
                //    build.Email("xx@xx.com");
                //    build.Name("xx");
                //    build.Url("http://baidu.com");
                //})
                .License(build =>
                {
                    build.Name("xx r&d");
                });

                // If you want the output Swagger docs to be indented properly, enable the "PrettyPrint" option.
                //
                //c.PrettyPrint();

                // If your API has multiple versions, use "MultipleApiVersions" instead of "SingleApiVersion".
                // In this case, you must provide a lambda that tells Swashbuckle which actions should be
                // included in the docs for a given API version. Like "SingleApiVersion", each call to "Version"
                // returns an "Info" builder so you can provide additional metadata per API version.
                //
                //c.MultipleApiVersions(
                //    (apiDesc, targetApiVersion) => ResolveVersionSupportByRouteConstraint(apiDesc, targetApiVersion),
                //    (vc) =>
                //    {
                //        vc.Version("v2", "Swashbuckle Dummy API V2");
                //        vc.Version("v1", "Swashbuckle Dummy API V1");
                //    });

                // You can use "BasicAuth", "ApiKey" or "OAuth2" options to describe security schemes for the API.
                // See https://github.com/swagger-api/swagger-spec/blob/master/versions/2.0.md for more details.
                // NOTE: These only define the schemes and need to be coupled with a corresponding "security" property
                // at the document or operation level to indicate which schemes are required for an operation. To do this,
                // you'll need to implement a custom IDocumentFilter and/or IOperationFilter to set these properties
                // according to your specific authorization implementation
                //
                //c.BasicAuth("basic")
                //    .Description("Basic HTTP Authentication");
                //
                // NOTE: You must also configure 'EnableApiKeySupport' below in the SwaggerUI section
                //c.ApiKey("apiKey")
                //    .Description("API Key Authentication")
                //    .Name("apiKey")
                //    .In("header");
                //
                //c.OAuth2("oauth2")
                //    .Description("OAuth2 Implicit Grant")
                //    .Flow("implicit")
                //    .AuthorizationUrl("http://petstore.swagger.wordnik.com/api/oauth/dialog")
                //    //.TokenUrl("https://tempuri.org/token")
                //    .Scopes(scopes =>
                //    {
                //        scopes.Add("read", "Read access to protected resources");
                //        scopes.Add("write", "Write access to protected resources");
                //    });

                // Set this flag to omit descriptions for any actions decorated with the Obsolete attribute
                //c.IgnoreObsoleteActions();

                // Each operation be assigned one or more tags which are then used by consumers for various reasons.
                // For example, the swagger-ui groups operations according to the first tag of each operation.
                // By default, this will be controller name but you can use the "GroupActionsBy" option to
                // override with any value.
                //
                //c.GroupActionsBy(apiDesc => apiDesc.HttpMethod.ToString());

                // You can also specify a custom sort order for groups (as defined by "GroupActionsBy") to dictate
                // the order in which operations are listed. For example, if the default grouping is in place
                // (controller name) and you specify a descending alphabetic sort order, then actions from a
                // ProductsController will be listed before those from a CustomersController. This is typically
                // used to customize the order of groupings in the swagger-ui.
                //
                //c.OrderActionGroupsBy(new DescendingAlphabeticComparer());

                // If you annotate Controllers and API Types with
                // Xml comments (http://msdn.microsoft.com/en-us/library/b2s063f7(v=vs.110).aspx), you can incorporate
                // those comments into the generated docs and UI. You can enable this by providing the path to one or
                // more Xml comment files.
                //
                //c.IncludeXmlComments(GetXmlCommentsPath());

                c.IncludeXmlComments($@"{parentPath}\SelfHostWeb.xml");
                c.IncludeXmlComments($@"{parentPath}\ApiTemplate.Model.XML");

                // Swashbuckle makes a best attempt at generating Swagger compliant JSON schemas for the various types
                // exposed in your API. However, there may be occasions when more control of the output is needed.
                // This is supported through the "MapType" and "SchemaFilter" options:
                //
                // Use the "MapType" option to override the Schema generation for a specific type.
                // It should be noted that the resulting Schema will be placed "inline" for any applicable Operations.
                // While Swagger 2.0 supports inline definitions for "all" Schema types, the swagger-ui tool does not.
                // It expects "complex" Schemas to be defined separately and referenced. For this reason, you should only
                // use the "MapType" option when the resulting Schema is a primitive or array type. If you need to alter a
                // complex Schema, use a Schema filter.
                //
                //c.MapType<ProductType>(() => new Schema { type = "integer", format = "int32" });

                // If you want to post-modify "complex" Schemas once they've been generated, across the board or for a
                // specific type, you can wire up one or more Schema filters.
                //
                //c.SchemaFilter<ApplySchemaVendorExtensions>();

                // In a Swagger 2.0 document, complex types are typically declared globally and referenced by unique
                // Schema Id. By default, Swashbuckle does NOT use the full type name in Schema Ids. In most cases, this
                // works well because it prevents the "implementation detail" of type namespaces from leaking into your
                // Swagger docs and UI. However, if you have multiple types in your API with the same class name, you'll
                // need to opt out of this behavior to avoid Schema Id conflicts.
                //
                //c.UseFullTypeNameInSchemaIds();

                // Alternatively, you can provide your own custom strategy for inferring SchemaId's for
                // describing "complex" types in your API.
                //
                //c.SchemaId(t => t.FullName.Contains('`') ? t.FullName.Substring(0, t.FullName.IndexOf('`')) : t.FullName);

                // Set this flag to omit schema property descriptions for any type properties decorated with the
                // Obsolete attribute
                c.IgnoreObsoleteProperties();

                // In accordance with the built in JsonSerializer, Swashbuckle will, by default, describe enums as integers.
                // You can change the serializer behavior by configuring the StringToEnumConverter globally or for a given
                // enum type. Swashbuckle will honor this change out-of-the-box. However, if you use a different
                // approach to serialize enums as strings, you can also force Swashbuckle to describe them as strings.
                //
                c.DescribeAllEnumsAsStrings();

                // Similar to Schema filters, Swashbuckle also supports Operation and Document filters:
                //
                // Post-modify Operation descriptions once they've been generated by wiring up one or more
                // Operation filters.
                //
                c.OperationFilter<DefaultResponseOperationFilter>();
                //
                // If you've defined an OAuth2 flow as described above, you could use a custom filter
                // to inspect some attribute on each action and infer which (if any) OAuth2 scopes are required
                // to execute the operation
                //
                c.OperationFilter<OAuth2SecurityOperationFilter>();

                // Post-modify the entire Swagger document by wiring up one or more Document filters.
                // This gives full control to modify the final SwaggerDocument. You should have a good understanding of
                // the Swagger 2.0 spec. - https://github.com/swagger-api/swagger-spec/blob/master/versions/2.0.md
                // before using this option.
                //
                //c.DocumentFilter<ApplyDocumentVendorExtensions>();
                c.OperationFilter<GlobalHttpHeaderFilter>();

                // In contrast to WebApi, Swagger 2.0 does not include the query string component when mapping a URL
                // to an action. As a result, Swashbuckle will raise an exception if it encounters multiple actions
                // with the same path (sans query string) and HTTP method. You can workaround this by providing a
                // custom strategy to pick a winner or merge the descriptions for the purposes of the Swagger docs
                //
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // Wrap the default SwaggerGenerator with additional behavior (e.g. caching) or provide an
                // alternative implementation for ISwaggerProvider with the CustomProvider option.
                //
                c.CustomProvider((defaultProvider) => new CachingSwaggerProvider(defaultProvider));
            })
                .EnableSwaggerUi(c =>
                {
                    // Use the "DocumentTitle" option to change the Document title.
                    // Very helpful when you have multiple Swagger pages open, to tell them apart.
                    //
                    c.DocumentTitle("REST Api 在线文档");

                    // Use the "InjectStylesheet" option to enrich the UI with one or more additional CSS stylesheets.
                    // The file must be included in your project as an "Embedded Resource", and then the resource's
                    // "Logical Name" is passed to the method as shown below.
                    //
                    //c.InjectStylesheet(containingAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testStyles1.css");

                    // Use the "InjectJavaScript" option to invoke one or more custom JavaScripts after the swagger-ui
                    // has loaded. The file must be included in your project as an "Embedded Resource", and then the resource's
                    // "Logical Name" is passed to the method as shown above.
                    //
                    //c.InjectJavaScript(thisAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testScript1.js");

                    // The swagger-ui renders boolean data types as a dropdown. By default, it provides "true" and "false"
                    // strings as the possible choices. You can use this option to change these to something else,
                    // for example 0 and 1.
                    //
                    c.BooleanValues(new[] { "0", "1" });

                    // By default, swagger-ui will validate specs against swagger.io's online validator and display the result
                    // in a badge at the bottom of the page. Use these options to set a different validator URL or to disable the
                    // feature entirely.
                    //c.SetValidatorUrl("http://localhost/validator");
                    //c.DisableValidator();

                    // Use this option to control how the Operation listing is displayed.
                    // It can be set to "None" (default), "List" (shows operations for each resource),
                    // or "Full" (fully expanded: shows operations and their details).
                    //
                    c.DocExpansion(DocExpansion.List);

                    // Specify which HTTP operations will have the 'Try it out!' option. An empty paramter list disables
                    // it for all operations.
                    //
                    //c.SupportedSubmitMethods("GET", "HEAD");

                    // Use the CustomAsset option to provide your own version of assets used in the swagger-ui.
                    // It's typically used to instruct Swashbuckle to return your version instead of the default
                    // when a request is made for "index.html". As with all custom content, the file must be included
                    // in your project as an "Embedded Resource", and then the resource's "Logical Name" is passed to
                    // the method as shown below.
                    //
                    //c.CustomAsset("index", containingAssembly, "YourWebApiProject.SwaggerExtensions.index.html");

                    // If your API has multiple versions and you've applied the MultipleApiVersions setting
                    // as described above, you can also enable a select box in the swagger-ui, that displays
                    // a discovery URL for each version. This provides a convenient way for users to browse documentation
                    // for different API versions.
                    //
                    //c.EnableDiscoveryUrlSelector();

                    // If your API supports the OAuth2 Implicit flow, and you've described it correctly, according to
                    // the Swagger 2.0 specification, you can enable UI support as shown below.
                    //
                    //c.EnableOAuth2Support(
                    //    clientId: "test-client-id",
                    //    clientSecret: null,
                    //    realm: "test-realm",
                    //    appName: "Swagger UI"
                    //    //additionalQueryStringParams: new Dictionary<string, string>() { { "foo", "bar" } }
                    //);

                    // If your API supports ApiKey, you can override the default values.
                    // "apiKeyIn" can either be "query" or "header"
                    //
                    //c.EnableApiKeySupport("apiKey", "header");
                });
        }

        #endregion
    }
}
