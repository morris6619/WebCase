using System;
using System.Web.Mvc;

namespace WebCase.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {

            Logger.Trace("這是TRACE的訊息");
            Logger.Debug("這是Debug的訊息");
            Logger.Info("這是Info的訊息");
            Logger.Warn("這是Warn的訊息");
            Logger.Error("這是Error的訊息");
            Logger.Fatal("這是Fatal的訊息");
            return View();
        }

        public ActionResult cal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult cal(FormCollection collection)
        {
            try
            {
                int a = int.Parse(collection["a"]);
                int b = int.Parse(collection["b"]);
                int c = a / b;
            

            }
            catch (Exception ex)
            {
                Logger.Error(ex.GetExceptionDetails);
            }

            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}