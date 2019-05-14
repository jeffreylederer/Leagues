using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Leagues.Models;
using Leagues.Code;

namespace Leagues.Controllers
{
    [Authorize]
    public class WednesdayMatchesController : Controller
    {
        private LeaguesEntities db = new LeaguesEntities();

        // GET: WednesdayMatches
        public ActionResult Index(int? ScheduleID)
        {
            var WednesdayMatches = db.WednesdayMatches.Where(x => x.GameDate == ScheduleID.Value && x.Rink != -1); ;
            ViewBag.ScheduleID = new SelectList(db.WednesdaySchedules.ToList(), "id", "GameDateFormatted", ScheduleID ?? 0);
            ViewBag.Date = db.WednesdaySchedules.Find(ScheduleID.Value).GameDateFormatted;
            ViewBag.WeekID = ScheduleID;
            return View(WednesdayMatches.OrderBy(x => x.Rink).ToList());
        }

        public ActionResult MoveUp(int id, int weekid)
        {
            var WednesdayMatches = db.WednesdayMatches.Where(x => x.GameDate == weekid).OrderBy(x => x.Rink);
            var match = WednesdayMatches.First(x => x.Rink == id);
            var match1 = WednesdayMatches.First(x => x.Rink == id + 1);
            match1.Rink = id;
            match.Rink = id + 1;
            db.Entry(match).State = EntityState.Modified;
            db.Entry(match1).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { ScheduleID = weekid });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateMatches()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMatches(string DeleteIT)
        {
            var numOfWeeks = db.WednesdaySchedules.Count();
            var numofTeams = db.WednesdayTeams.Count();
            db.WednesdayMatches.RemoveRange(db.WednesdayMatches);
            db.SaveChanges();
            var cs = new CreateSchedule();

            var matches = numofTeams % 2 == 0 ? cs.NoByes(numOfWeeks, numofTeams) : cs.Byes(numOfWeeks, numofTeams);

            foreach (var match in matches)
            {
                db.WednesdayMatches.Add(new WednesdayMatch()
                {
                    id=0,
                    GameDate = match.Week + 1,
                    Rink = match.Rink == -1 ? -1 : match.Rink + 1,
                    Team1 = match.Team1 + 1,
                    Team2 = match.Team2 + 1,
                    Team1Score = 0,
                    Team2Score = 0
                });
               
            }
            db.SaveChanges();
            return RedirectToAction("index", new { ScheduleID = 1 });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ClearSchedule()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClearSchedule(string DeleteIT)
        {
            db.WednesdaySchedules.RemoveRange(db.WednesdaySchedules);
            db.WednesdayTeams.RemoveRange(db.WednesdayTeams);
            db.WednesdayMatches.RemoveRange(db.WednesdayMatches);
            db.SaveChanges();
            return RedirectToAction("index", "Home");
        }

        public ActionResult Scoring(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayMatch WednesdayMatch = db.WednesdayMatches.Find(id);
            if (WednesdayMatch == null)
            {
                return HttpNotFound();
            }
            return View(WednesdayMatch);
        }

        // POST: WednesdayMatches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Scoring([Bind(Include = "id,GameDate,Rink,Team1,Team2,Team1Score,Team2Score")] WednesdayMatch WednesdayMatch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(WednesdayMatch).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", new { ScheduleID = WednesdayMatch.GameDate });
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
            return View(WednesdayMatch);
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
