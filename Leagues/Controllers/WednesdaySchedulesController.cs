using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Leagues.Models;

namespace Leagues.Controllers
{
    public class WednesdaySchedulesController : Controller
    {
        private LeaguesEntities db = new LeaguesEntities();

        // GET: WednesdaySchedules
        public ActionResult Index()
        {
            return View(db.WednesdaySchedules.ToList());
        }

        // GET: WednesdaySchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdaySchedule wednesdaySchedule = db.WednesdaySchedules.Find(id);
            if (wednesdaySchedule == null)
            {
                return HttpNotFound();
            }
            return View(wednesdaySchedule);
        }

        // GET: WednesdaySchedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WednesdaySchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,GameDate")] WednesdaySchedule wednesdaySchedule)
        {
            if (ModelState.IsValid)
            {
                db.WednesdaySchedules.Add(wednesdaySchedule);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    Exception ex = e;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                    ModelState.AddModelError(string.Empty, ex.Message);

                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Insert failed");
                }
            }

            return View(wednesdaySchedule);
        }

        // GET: WednesdaySchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdaySchedule wednesdaySchedule = db.WednesdaySchedules.Find(id);
            if (wednesdaySchedule == null)
            {
                return HttpNotFound();
            }
            return View(wednesdaySchedule);
        }

        // POST: WednesdaySchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,GameDate")] WednesdaySchedule wednesdaySchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wednesdaySchedule).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    Exception ex = e;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                    ModelState.AddModelError(string.Empty, ex.Message);

                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Edit failed");
                }
            }
            return View(wednesdaySchedule);
        }

        // GET: WednesdaySchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdaySchedule wednesdaySchedule = db.WednesdaySchedules.Find(id);
            if (wednesdaySchedule == null)
            {
                return HttpNotFound();
            }
            return View(wednesdaySchedule);
        }

        // POST: WednesdaySchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WednesdaySchedule wednesdaySchedule = db.WednesdaySchedules.Find(id);
            db.WednesdaySchedules.Remove(wednesdaySchedule);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                Exception ex = e;
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                ViewBag.Error = ex.Message;
            }
            catch (Exception)
            {
                ViewBag.Error = "Delete failed";
            }
            return View(wednesdaySchedule);
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
