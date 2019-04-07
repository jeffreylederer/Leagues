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
    public class WednesdayMatchesController : Controller
    {
        private LeagueEntities db = new LeagueEntities();

        // GET: WednesdayMatches
        public ActionResult Index(int? ScheduleID)
        {
            var wednesdayMatches = db.WednesdayMatches.Include(w => w.WednesdaySchedule).Include(w => w.WednesdayTeam).Include(w => w.WednesdayTeam1);
            if (ScheduleID.HasValue)
                wednesdayMatches = db.WednesdayMatches.Where(x => x.GameDate == ScheduleID.Value);
            return View(wednesdayMatches.ToList());
        }

        // GET: WednesdayMatches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayMatch wednesdayMatch = db.WednesdayMatches.Find(id);
            if (wednesdayMatch == null)
            {
                return HttpNotFound();
            }
            return View(wednesdayMatch);
        }

        // GET: WednesdayMatches/Create
        public ActionResult Create()
        {
            ViewBag.GameDate = new SelectList(db.WednesdaySchedules, "id", "id");
            ViewBag.Team1 = new SelectList(db.WednesdayTeams, "id", "id");
            ViewBag.Team2 = new SelectList(db.WednesdayTeams, "id", "id");
            return View();
        }

        // POST: WednesdayMatches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,GameDate,Rink,Team1,Team2")] WednesdayMatch wednesdayMatch)
        {
            if (ModelState.IsValid)
            {
                db.WednesdayMatches.Add(wednesdayMatch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GameDate = new SelectList(db.WednesdaySchedules, "id", "id", wednesdayMatch.GameDate);
            ViewBag.Team1 = new SelectList(db.WednesdayTeams, "id", "id", wednesdayMatch.Team1);
            ViewBag.Team2 = new SelectList(db.WednesdayTeams, "id", "id", wednesdayMatch.Team2);
            return View(wednesdayMatch);
        }

        // GET: WednesdayMatches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayMatch wednesdayMatch = db.WednesdayMatches.Find(id);
            if (wednesdayMatch == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameDate = new SelectList(db.WednesdaySchedules, "id", "id", wednesdayMatch.GameDate);
            ViewBag.Team1 = new SelectList(db.WednesdayTeams, "id", "id", wednesdayMatch.Team1);
            ViewBag.Team2 = new SelectList(db.WednesdayTeams, "id", "id", wednesdayMatch.Team2);
            return View(wednesdayMatch);
        }

        // POST: WednesdayMatches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,GameDate,Rink,Team1,Team2")] WednesdayMatch wednesdayMatch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wednesdayMatch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GameDate = new SelectList(db.WednesdaySchedules, "id", "id", wednesdayMatch.GameDate);
            ViewBag.Team1 = new SelectList(db.WednesdayTeams, "id", "id", wednesdayMatch.Team1);
            ViewBag.Team2 = new SelectList(db.WednesdayTeams, "id", "id", wednesdayMatch.Team2);
            return View(wednesdayMatch);
        }

        // GET: WednesdayMatches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayMatch wednesdayMatch = db.WednesdayMatches.Find(id);
            if (wednesdayMatch == null)
            {
                return HttpNotFound();
            }
            return View(wednesdayMatch);
        }

        // POST: WednesdayMatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WednesdayMatch wednesdayMatch = db.WednesdayMatches.Find(id);
            db.WednesdayMatches.Remove(wednesdayMatch);
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
