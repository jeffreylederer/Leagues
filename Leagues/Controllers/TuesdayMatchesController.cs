using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Elmah;
using Leagues.Models;
using Leagues.Code;

namespace Leagues.Controllers
{
    [Authorize]
    public class TuesdayMatchesController : Controller
    {
        private LeaguesEntities db = new LeaguesEntities();

        // GET: TuesdayMatches
        public ActionResult Index(int? ScheduleID)
        {
            var tuesdayMatches = db.TuesdayMatches.Where(x => x.GameDate == ScheduleID.Value && x.Rink != -1); ;
            ViewBag.ScheduleID = new SelectList(db.TuesdaySchedules.ToList(), "id", "GameDateFormatted", ScheduleID ?? 0);
            ViewBag.Date = db.TuesdaySchedules.Find(ScheduleID.Value).GameDateFormatted;
            ViewBag.WeekID = ScheduleID;
            return View(tuesdayMatches.OrderBy(x=>x.Rink).ToList());
        }

        public ActionResult MoveUp(int id, int weekid)
        {
            var tuesdayMatches = db.TuesdayMatches.Where(x => x.GameDate == weekid).OrderBy(x => x.Rink);
            var match = tuesdayMatches.First(x => x.Rink == id);
            var match1 = tuesdayMatches.First(x => x.Rink == id+1);
            match1.Rink = id;
            match.Rink = id + 1;
            db.Entry(match).State = EntityState.Modified;
            db.Entry(match1).State = EntityState.Modified;
            try
            {

                db.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }
           
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
            var numOfWeeks = db.TuesdaySchedules.Count();
            var numofTeams = db.TuesdayTeams.Count();
            db.TuesdayMatches.RemoveRange(db.TuesdayMatches);
            db.SaveChanges();
            var cs = new CreateSchedule();
            List<Match> matches = null;
            if (numofTeams % 2 == 0)
                matches = cs.NoByes(numOfWeeks, numofTeams);
            else
                matches = cs.Byes(numOfWeeks, numofTeams);
           
            foreach (var match in matches)
            {
                db.TuesdayMatches.Add(new TuesdayMatch()
                {
                    GameDate = match.Week + 1,
                    Rink = match.Rink==-1? -1: match.Rink + 1,
                    Team1 = match.Team1 + 1,
                    Team2 = match.Team2 + 1,
                    Team1Score = 0,
                    Team2Score = 0
                });
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }
            
            return RedirectToAction("index", new {ScheduleID=1});
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
            db.TuesdaySchedules.RemoveRange(db.TuesdaySchedules);
            db.TuesdayTeams.RemoveRange(db.TuesdayTeams);
            db.TuesdayMatches.RemoveRange(db.TuesdayMatches);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                Console.WriteLine(e);
                throw;
            }
            
            return RedirectToAction("index","Home");
        }

        public ActionResult Scoring(int? id)
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
            var list = new List<ForfeitViewModel>(3)
            {
                new ForfeitViewModel() {value = 1, text = "no forfeit"},
                new ForfeitViewModel() {value = 2, text = $"{tuesdayMatch.Team1.ToString()} - ({tuesdayMatch.TuesdayTeam.Player.NickName}, {tuesdayMatch.TuesdayTeam.Player1.NickName})"},
                new ForfeitViewModel() {value = 3, text = $"{tuesdayMatch.Team2.ToString()} - ({tuesdayMatch.TuesdayTeam1.Player.NickName}, {tuesdayMatch.TuesdayTeam1.Player1.NickName})"}
            };
            ViewBag.Forfeit = new SelectList(list, "value", "text", tuesdayMatch.Forfeit);
            return View(tuesdayMatch);
        }

        // POST: TuesdayMatches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Scoring([Bind(Include = "id,GameDate,Rink,Team1,Team2,Team1Score,Team2Score,Forfeit")] TuesdayMatch tuesdayMatch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuesdayMatch).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", new { ScheduleID = tuesdayMatch.GameDate });
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
            var list = new List<ForfeitViewModel>(3)
            {
                new ForfeitViewModel() {value = 1, text = "no forfeit"},
                new ForfeitViewModel() {value = 2, text = $"{tuesdayMatch.Team1.ToString()} - ({tuesdayMatch.TuesdayTeam.Player.NickName}, {tuesdayMatch.TuesdayTeam.Player1.NickName})"},
                new ForfeitViewModel() {value = 3, text = $"{tuesdayMatch.Team2.ToString()} - ({tuesdayMatch.TuesdayTeam1.Player.NickName}, {tuesdayMatch.TuesdayTeam1.Player1.NickName})"}
            };
            ViewBag.Forfeit = new SelectList(list, "value", "text", tuesdayMatch.Forfeit);
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
