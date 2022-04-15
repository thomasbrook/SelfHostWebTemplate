using log4net;
using SelfHostWeb.IBll.DataSource;
using System.Web.Mvc;

namespace ApiTemplate.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog _log = LogManager.GetLogger("WebLog");


        private readonly IDataSourceBll _dataSourceBll;
        public HomeController(IDataSourceBll dataSourceBll)
        {
            _dataSourceBll = dataSourceBll;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            _log.Info("Home Page");
            return View();
        }

        public JsonResult ListDataSource()
        {
            var result = _dataSourceBll.ListDataSource();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataSource(string id)
        {
            var result = _dataSourceBll.GetDataSource(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
