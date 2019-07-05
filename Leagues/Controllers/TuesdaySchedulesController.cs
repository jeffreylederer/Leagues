using Leagues.Code;
using Leagues.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Elmah;


namespace Leagues.Controllers
{
    [Authorize]
    public class TuesdaySchedulesController : Controller
    {
        private LeaguesEntities db = new LeaguesEntities();

        // GET: TuesdaySchedules
        public ActionResult Index()
        {
            return View(db.TuesdaySchedules.ToList());
        }

        
        // GET: TuesdaySchedules/Create
        public ActionResult Create()
        {
            var id = 1;
            DateTime date = DateTime.Now;
            var items = db.TuesdaySchedules.OrderByDescending(x => x.id);
            if (items.Count() > 0)
            {
                date = items.First().GameDate.AddDays(7);
                id = items.First().id + 1;
            }
            var item = new TuesdaySchedule()
            {
                IsCancelled = false,
                id = id,
                GameDate = date
            };
            return View(item);
        }

        
        // POST: TuesdaySchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,GameDate,IsCancelled")] TuesdaySchedule tuesdaySchedule)
        {
            if (ModelState.IsValid)
            {
                db.TuesdaySchedules.Add(tuesdaySchedule);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    ErrorSignal.FromCurrentContext().Raise(e);
                    Exception ex = e;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception e)
                {
                    ErrorSignal.FromCurrentContext().Raise(e);
                    ModelState.AddModelError(string.Empty, "Insert failed");
                }
            }

            return View(tuesdaySchedule);
        }

        // GET: TuesdaySchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuesdaySchedule tuesdaySchedule = db.TuesdaySchedules.Find(id);
            if (tuesdaySchedule == null)
            {
                return HttpNotFound();
            }
            return View(tuesdaySchedule);
        }

        // POST: TuesdaySchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,GameDate,IsCancelled")] TuesdaySchedule tuesdaySchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuesdaySchedule).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    ErrorSignal.FromCurrentContext().Raise(e);
                    Exception ex = e;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception e)
                {
                    ErrorSignal.FromCurrentContext().Raise(e);
                    ModelState.AddModelError(string.Empty, "Edit failed");
                }
            }
            return View(tuesdaySchedule);
        }

        // GET: TuesdaySchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuesdaySchedule tuesdaySchedule = db.TuesdaySchedules.Find(id);
            if (tuesdaySchedule == null)
            {
                return HttpNotFound();
            }
            return View(tuesdaySchedule);
        }

        // POST: TuesdaySchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TuesdaySchedule tuesdaySchedule = db.TuesdaySchedules.Find(id);
            db.TuesdaySchedules.Remove(tuesdaySchedule);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                Exception ex = e;
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                ViewBag.Error = ex.Message;
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                ViewBag.Error = "Delete failed";
            }
            return View(tuesdaySchedule);
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

