using CsvHelper;
using DAL;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebCase.Areas.Basic.ViewModels;
using WebCase.Controllers;

namespace WebCase.Areas.Basic.Controllers
{
    public class CustomersController : BaseController
    {
        private CustomersRep repository;
        public CustomersController()
        {
            this.repository = new CustomersRep();

        }
        // GET: Basic/Customers
        public ActionResult Index()
        {

            var data = repository.GetAll().ToList();
            var result = mapper.Map<IEnumerable<CustomersVM>>(data); // 轉換型別

            return View(result);

        }
        public ActionResult ExportCSV()
        {
            string docPath =Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var list = repository.GetAll().ToList();//去資料庫取得list資料
            //string docPath2= @"c:\WriteLines.csv.";
            
            using (var writer = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(list);
                }
            }
            return SuccessResult();
            //return View();
        }

            public ActionResult ShowText()
        {
            //var pp = AppDomain.CurrentDomain.GetAssemblies();

            //return Json(ret, JsonRequestBehavior.AllowGet);

            return View(db.Customers.ToList());
        }
        // GET: Basic/Customers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // GET: Basic/Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Basic/Customers/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customers customers)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customers);
        }

        // GET: Basic/Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            var customersVM = mapper.Map<CustomersVM>(customers); // 轉換型別
            return View(customersVM);
        }

        // POST: Basic/Customers/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomersVM customersVM)
        {

            var customers = mapper.Map<Customers>(customersVM); // 轉換型別

            if (ModelState.IsValid)
            {

                //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                db.Entry(customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customers);
        }

        // GET: Basic/Customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = db.Customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Basic/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            Customers customers = db.Customers.Find(id);
            db.Customers.Remove(customers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
