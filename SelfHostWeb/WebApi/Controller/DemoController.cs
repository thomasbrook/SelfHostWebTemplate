using ApiTemplate.Model.Po;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SelfHostWeb.IBll.DataSource;
using SelfHostWeb.Model;
using SelfHostWeb.WebApi.Parts;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace SelfHostWeb.WebApi.Controller
{
    [WebApiExceptionFilter]
    public class DemoController : ApiController
    {
        private static readonly ILog _log = LogManager.GetLogger("ProgramLog");
        private readonly IDataSourceBll _dataSourceBll;
        public DemoController(IDataSourceBll dataSourceBll)
        {
            _dataSourceBll = dataSourceBll;
        }

        [HttpPost, WebApiAuth]
        public dynamic allStudents()
        {
            return new
            {
                result = true,
                desc = "请求成功",
                data = new List<StudentInfo>()
                {
                    new StudentInfo
                    {
                        ClassId="ClassID982712311231",
                        Sex="男",
                        StudentId=Guid.NewGuid().ToString(),
                        StudentName="男学生一枚",
                        StudentNumber="XH2018090001"
                    },
                    new StudentInfo
                    {
                        ClassId="ClassID982712311232",
                        Sex="女",
                        StudentId=Guid.NewGuid().ToString(),
                        StudentName="女学生一枚",
                        StudentNumber="XH2018090002"
                    }
                }
            };
        }

        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, WebApiAuth]
        [SwaggerResponse(HttpStatusCode.OK, "请求成功", typeof(ResponseModel<ResultWithTotalModel<List<DataSourcePo>>>))]
        public dynamic ListDataSource()
        {
            var result = _dataSourceBll.ListDataSource();

            var resp = new ResponseModel<dynamic>();
            resp.Data = new { Count = result.Count, Data = result };
            resp.ExMessage = null;
            resp.StatusCode = (int)HttpStatusCode.OK; // HttpStatusCode.BadRequest;

            return this.SerialResponseMessage(resp, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        public class Test
        {
            public string id { get; set; }
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpPost, WebApiAuth]
        [SwaggerResponse(HttpStatusCode.OK, "请求成功", typeof(ResponseModel<DataSourcePo>))]
        public dynamic GetDataSource([FromBody] Test t)
        {

            var result = _dataSourceBll.GetDataSource(t.id);

            var resp = new ResponseModel<DataSourcePo>();
            resp.Data = result;
            resp.ExMessage = null;
            resp.StatusCode = (int)HttpStatusCode.OK; // HttpStatusCode.BadRequest;

            return this.SerialResponseMessage(resp, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        /// <summary>
        /// 批量插入两条主键重复数据，通过 Transaction、Attribute、Interceptor 实现事务
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpPost, WebApiAuth]
        [SwaggerResponse(HttpStatusCode.OK, "请求成功", typeof(ResponseModel<string>))]
        public dynamic InsertDataSourceWithSqlTrans([FromBody] DataSourcePo t)
        {
            _log.Debug($"/api/demo/InsertDataSourceWithSqlTrans,param:{JsonConvert.SerializeObject(t)}");
            var resp = new ResponseModel<string>();
            try
            {
                var result = _dataSourceBll.InsertDataSourceTestWithSqlTransAttribute(t);
                resp.Data = result ? "事务执行成功" : "事务执行失败";
                resp.ExMessage = null;
                resp.StatusCode = (int)HttpStatusCode.OK; // HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                resp.ExMessage = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return this.SerialResponseMessage(resp, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        /// <summary>
        /// 批量插入两条主键重复数据，通过 TransactionScope、Attribute、Interceptor 实现事务
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpPost, WebApiAuth]
        [SwaggerResponse(HttpStatusCode.OK, "请求成功", typeof(ResponseModel<string>))]
        public dynamic InsertDataSourceWithTransScope([FromBody] DataSourcePo t)
        {
            _log.Debug($"/api/demo/GetDataSource,param:{JsonConvert.SerializeObject(t)}");
            var resp = new ResponseModel<string>();
            try
            {
                var result = _dataSourceBll.InsertDataSourceTestWithTransScopeAttribute(t);

                resp.Data = result ? "事务执行成功" : "事务执行失败";
                resp.ExMessage = null;
                resp.StatusCode = (int)HttpStatusCode.OK; // HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                resp.ExMessage = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return this.SerialResponseMessage(resp, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        /// <summary>
        /// 批量插入两条主键重复数据，通过传递Transaction参数实现事务
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpPost, WebApiAuth]
        [SwaggerResponse(HttpStatusCode.OK, "请求成功", typeof(ResponseModel<string>))]
        public dynamic InsertDataSourceWithTrans([FromBody] DataSourcePo t)
        {
            _log.Debug($"/api/demo/GetDataSource,param:{JsonConvert.SerializeObject(t)}");

            var resp = new ResponseModel<string>();
            ;
            try
            {
                var result = _dataSourceBll.InsertDataSourceTestWithSqlTrans(t);

                resp.Data = result ? "事务执行成功" : "事务执行失败";
                resp.ExMessage = null;
                resp.StatusCode = (int)HttpStatusCode.OK; // HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                resp.ExMessage = ex.Message;
                resp.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return this.SerialResponseMessage(resp, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }
    }
}
