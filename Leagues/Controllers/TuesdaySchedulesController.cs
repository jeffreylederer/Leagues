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
            return View();
        }

        // POST: TuesdaySchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,GameDate")] TuesdaySchedule tuesdaySchedule)
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
        public ActionResult Edit([Bind(Include = "id,GameDate")] TuesdaySchedule tuesdaySchedule)
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
                Exception ex = e;
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                ViewBag.Error = ex.Message;
            }
            catch (Exception)
            {
                ViewBag.Error = "Delete failed";
            }
            return View(tuesdaySchedule);
        }

        public ActionResult CreateSchedule()
        {
            const int numberofWeeks = 9;
            int numberofTeams = db.TuesdayTeams.Count();
            

            var cs = new CreateSchedule();
            List<Match> matches;
            if (numberofTeams % 2 == 0)
                matches = cs.NoByes(numberofWeeks, numberofTeams);
            else
                matches = cs.Byes(numberofWeeks, numberofTeams);

            foreach (var item in db.TuesdayMatches)
            {
                db.TuesdayMatches.Remove(item);
            }
            db.SaveChanges();

            var teamCount = numberofTeams + numberofTeams % 2;
            int numberOfRinks = teamCount / 2;
            
            for (int w = 0; w < numberofWeeks; w++)
            {
                for (int r = 0; r < numberOfRinks; r++)
                {
                    var match = matches.Find(x => x.Rink == r && x.Week == w);
                    db.TuesdayMatches.Add(new TuesdayMatch()
                    {
                        GameDate = w,
                        Rink = r + 1,
                        Team1 = match.Team1,
                        Team2 = match.Team2
                    });
                }
            }
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

