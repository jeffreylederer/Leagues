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
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                throw;
            }
            
            return RedirectToAction("Index", new { ScheduleID = weekid });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateMatches()
        {
            return View();
        }

        public ActionResult DownloadFile()
        {
            string path = Server.MapPath("/App_Data/");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "WednesdayLabels.docx");
            string fileName = "WednesdayLabels.docx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }
           
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
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }
            
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
            var list = new List<ForfeitViewModel>(3)
            {
                new ForfeitViewModel() {value = 1, text = "no forfeit"},
                new ForfeitViewModel() {value = 2, text = $"{WednesdayMatch.Team1.ToString()} - ({WednesdayMatch.WednesdayTeam.Player.NickName}, {WednesdayMatch.WednesdayTeam.Player1.NickName}, {WednesdayMatch.WednesdayTeam.Player2.NickName})"},
                new ForfeitViewModel() {value = 3, text = $"{WednesdayMatch.Team2.ToString()} - ({WednesdayMatch.WednesdayTeam1.Player.NickName}, {WednesdayMatch.WednesdayTeam1.Player1.NickName}, {WednesdayMatch.WednesdayTeam1.Player2.NickName})"}
            };
            ViewBag.Forfeit = new SelectList(list, "value","text", WednesdayMatch.Forfeit);
            return View(WednesdayMatch);
        }

        // POST: WednesdayMatches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Scoring([Bind(Include = "id,GameDate,Rink,Team1,Team2,Team1Score,Team2Score, Forfeit")] WednesdayMatch WednesdayMatch)
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
                new ForfeitViewModel() {value = 2, text = $"{WednesdayMatch.Team1.ToString()} - ({WednesdayMatch.WednesdayTeam.Player.NickName}, {WednesdayMatch.WednesdayTeam.Player1.NickName}, {WednesdayMatch.WednesdayTeam.Player2.NickName})"},
                new ForfeitViewModel() {value = 3, text = $"{WednesdayMatch.Team2.ToString()} - ({WednesdayMatch.WednesdayTeam1.Player.NickName}, {WednesdayMatch.WednesdayTeam1.Player1.NickName}, {WednesdayMatch.WednesdayTeam1.Player2.NickName})"}
            };
            ViewBag.Forfeit = new SelectList(list, "value", "text", WednesdayMatch.Forfeit);
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
