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
    public class TuesdayTeamsController : Controller
    {
        private LeagueEntities db = new LeagueEntities();

        // GET: TuesdayTeams
        public ActionResult Index()
        {
            var tuesdayTeams = db.TuesdayTeams.Include(t => t.Player).Include(t => t.Player1);
            return View(tuesdayTeams.ToList());
        }

        // GET: TuesdayTeams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuesdayTeam tuesdayTeam = db.TuesdayTeams.Find(id);
            if (tuesdayTeam == null)
            {
                return HttpNotFound();
            }
            return View(tuesdayTeam);
        }

        // GET: TuesdayTeams/Create
        public ActionResult Create()
        {
            ViewBag.Skip = new SelectList(db.Players.Where(x=>x.TuesdayLeague), "id", "FullName"," ");
            ViewBag.Lead = new SelectList(db.Players.Where(x => x.TuesdayLeague), "id", "FullName"," ");
            return View();
        }

        // POST: TuesdayTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Skip,Lead")] TuesdayTeam tuesdayTeam)
        {
            if (ModelState.IsValid)
            {
                db.TuesdayTeams.Add(tuesdayTeam);
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

            ViewBag.Skip = new SelectList(db.Players.Where(x => x.TuesdayLeague), "id", "FullName", tuesdayTeam.Skip);
            ViewBag.Lead = new SelectList(db.Players.Where(x => x.TuesdayLeague), "id", "FullName", tuesdayTeam.Lead);
            return View(tuesdayTeam);
        }

        // GET: TuesdayTeams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuesdayTeam tuesdayTeam = db.TuesdayTeams.Find(id);
            if (tuesdayTeam == null)
            {
                return HttpNotFound();
            }
            ViewBag.Skip = new SelectList(db.Players.Where(x => x.TuesdayLeague), "id", "FullName", tuesdayTeam.Skip);
            ViewBag.Lead = new SelectList(db.Players.Where(x => x.TuesdayLeague), "id", "FullName", tuesdayTeam.Lead);
            return View(tuesdayTeam);
        }

        // POST: TuesdayTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Skip,Lead")] TuesdayTeam tuesdayTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuesdayTeam).State = EntityState.Modified;
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
                    ModelState.AddModelError(string.Empty, e.Message);

                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Edit failed");
                }
            }
            ViewBag.Skip = new SelectList(db.Players.Where(x => x.TuesdayLeague), "id", "FullName", tuesdayTeam.Skip);
            ViewBag.Lead = new SelectList(db.Players.Where(x => x.TuesdayLeague), "id", "FullName", tuesdayTeam.Lead);
            return View(tuesdayTeam);
        }

        // GET: TuesdayTeams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuesdayTeam tuesdayTeam = db.TuesdayTeams.Find(id);
            if (tuesdayTeam == null)
            {
                return HttpNotFound();
            }
            return View(tuesdayTeam);
        }

        // POST: TuesdayTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TuesdayTeam tuesdayTeam = db.TuesdayTeams.Find(id);
            db.TuesdayTeams.Remove(tuesdayTeam);
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
            return View(tuesdayTeam);
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
