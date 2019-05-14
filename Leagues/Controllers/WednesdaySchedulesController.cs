using Leagues.Code;
using Leagues.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace Leagues.Controllers
{
    [Authorize]
    public class WednesdaySchedulesController : Controller
    {
        private LeaguesEntities db = new LeaguesEntities();

        // GET: WednesdaySchedules
        public ActionResult Index()
        {
            return View(db.WednesdaySchedules.ToList());
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
        public ActionResult Create([Bind(Include = "id,GameDate")] WednesdaySchedule WednesdaySchedule)
        {
            if (ModelState.IsValid)
            {
                db.WednesdaySchedules.Add(WednesdaySchedule);
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

            return View(WednesdaySchedule);
        }

        // GET: WednesdaySchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdaySchedule WednesdaySchedule = db.WednesdaySchedules.Find(id);
            if (WednesdaySchedule == null)
            {
                return HttpNotFound();
            }
            return View(WednesdaySchedule);
        }

        // POST: WednesdaySchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,GameDate")] WednesdaySchedule WednesdaySchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(WednesdaySchedule).State = EntityState.Modified;
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
            return View(WednesdaySchedule);
        }

        // GET: WednesdaySchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdaySchedule WednesdaySchedule = db.WednesdaySchedules.Find(id);
            if (WednesdaySchedule == null)
            {
                return HttpNotFound();
            }
            return View(WednesdaySchedule);
        }

        // POST: WednesdaySchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WednesdaySchedule WednesdaySchedule = db.WednesdaySchedules.Find(id);
            db.WednesdaySchedules.Remove(WednesdaySchedule);
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
            return View(WednesdaySchedule);
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

