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
    public class TuesdayMatchesController : Controller
    {
        private LeagueEntities db = new LeagueEntities();

        // GET: TuesdayMatches
        public ActionResult Index(int? ScheduleID)
        {
            var tuesdayMatches = db.TuesdayMatches.Include(t => t.TuesdaySchedule).Include(t => t.TuesdayTeam).Include(t => t.TuesdayTeam1);
            if (ScheduleID.HasValue)
                tuesdayMatches = db.TuesdayMatches.Where(x => x.GameDate == ScheduleID.Value);
            ViewBag.ScheduleID = new SelectList(db.TuesdaySchedules, "id", "GameDateFormatted", ScheduleID.HasValue? ScheduleID.Value.ToString(): "");
            ViewBag.Date = ScheduleID.HasValue ? ScheduleID.Value.ToString() : "";
            return View(tuesdayMatches.ToList());
        }

        

        // GET: TuesdayMatches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuesdayMatch tuesdayMatch = db.TuesdayMatches.Find(id);
            if (tuesdayMatch == null)
            {
                return HttpNotFound();
            }
            return View(tuesdayMatch);
        }

        // GET: TuesdayMatches/Create
        public ActionResult Create(int? id)
        {
            ViewBag.GameDate = new SelectList(db.TuesdaySchedules, "id", "GameDateFormatted", id.HasValue? id.Value.ToString():"");
            ViewBag.Team1 = new SelectList(db.TuesdayTeams, "id", "id");
            ViewBag.Team2 = new SelectList(db.TuesdayTeams, "id", "id");
            ViewBag.Date = id.HasValue ? id.Value.ToString() : "";
            return View();
        }

        // POST: TuesdayMatches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,GameDate,Rink,Team1,Team2")] TuesdayMatch tuesdayMatch)
        {
            if (ModelState.IsValid)
            {
                db.TuesdayMatches.Add(tuesdayMatch);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", new { ScheduleID = tuesdayMatch.GameDate});
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

            ViewBag.GameDate = new SelectList(db.TuesdaySchedules, "id", "id", tuesdayMatch.GameDate);
            ViewBag.Team1 = new SelectList(db.TuesdayTeams, "id", "id", tuesdayMatch.Team1);
            ViewBag.Team2 = new SelectList(db.TuesdayTeams, "id", "id", tuesdayMatch.Team2);
            ViewBag.Date = tuesdayMatch.GameDate.ToString();
            return View(tuesdayMatch);
        }

        // GET: TuesdayMatches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuesdayMatch tuesdayMatch = db.TuesdayMatches.Find(id);
            if (tuesdayMatch == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameDate = new SelectList(db.TuesdaySchedules, "id", "id", tuesdayMatch.GameDate);
            ViewBag.Team1 = new SelectList(db.TuesdayTeams, "id", "id", tuesdayMatch.Team1);
            ViewBag.Team2 = new SelectList(db.TuesdayTeams, "id", "id", tuesdayMatch.Team2);
            return View(tuesdayMatch);
        }

        // POST: TuesdayMatches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,GameDate,Rink,Team1,Team2")] TuesdayMatch tuesdayMatch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuesdayMatch).State = EntityState.Modified;
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
            ViewBag.GameDate = new SelectList(db.TuesdaySchedules, "id", "id", tuesdayMatch.GameDate);
            ViewBag.Team1 = new SelectList(db.TuesdayTeams, "id", "id", tuesdayMatch.Team1);
            ViewBag.Team2 = new SelectList(db.TuesdayTeams, "id", "id", tuesdayMatch.Team2);
            return View(tuesdayMatch);
        }

        // GET: TuesdayMatches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuesdayMatch tuesdayMatch = db.TuesdayMatches.Find(id);
            if (tuesdayMatch == null)
            {
                return HttpNotFound();
            }
            return View(tuesdayMatch);
        }

        // POST: TuesdayMatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TuesdayMatch tuesdayMatch = db.TuesdayMatches.Find(id);
            db.TuesdayMatches.Remove(tuesdayMatch);
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
            return View(tuesdayMatch);
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
