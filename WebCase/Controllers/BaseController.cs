using AutoMapper;
using DAL;
using System;
using System.Web.Mvc;
using WebCase.Mappings;

namespace WebCase.Controllers
{
    public class BaseController : Controller
    {
        public DBEntities db;
        public NLog.Logger Logger;
        public MapperConfiguration config;
        public IMapper mapper;
        public BaseController()
        {
            try{
                db = new DBEntities();
                Logger = NLog.LogManager.GetCurrentClassLogger();
                //config = new MapperConfiguration(cfg => cfg.AddProfile<ServiceMappings>());//手動加入Profile
                config = new MapperConfiguration(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies())); //掃描有Profile的都自動加入
                mapper = config.CreateMapper(); // 用設定檔建立 Mapper

            }
            catch(Exception ex)
            {
                Logger.Error(ex.GetExceptionDetails());
            }            
        }


        public ActionResult SuccessResult(object Data = null, String Msg = "編輯成功!")
        {
            if (Data == null)
            {
                Data = new { success = true, responseText = Msg };
            };
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FailResult(String Msg = "操作失敗!")
        {
            return Json(new { success = false, responseText = Msg }, JsonRequestBehavior.AllowGet);
        }
    }
}