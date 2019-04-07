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
    public class WednesdayTeamsController : Controller
    {
        private LeagueEntities db = new LeagueEntities();

        // GET: WednesdayTeams
        public ActionResult Index()
        {
            var wednesdayTeams = db.WednesdayTeams.Include(w => w.Player).Include(w => w.Player1).Include(w => w.Player2);
            return View(wednesdayTeams.ToList());
        }

        // GET: WednesdayTeams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayTeam wednesdayTeam = db.WednesdayTeams.Find(id);
            if (wednesdayTeam == null)
            {
                return HttpNotFound();
            }
            return View(wednesdayTeam);
        }

        // GET: WednesdayTeams/Create
        public ActionResult Create()
        {
            ViewBag.Skip = new SelectList(db.Players.Where(x=>x.WednesdayLeague), "id", "FullName");
            ViewBag.ViceSkip = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName");
            ViewBag.Lead = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName");
            return View();
        }

        // POST: WednesdayTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Skip,ViceSkip,Lead")] WednesdayTeam wednesdayTeam)
        {
            if (ModelState.IsValid)
            {
                db.WednesdayTeams.Add(wednesdayTeam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Skip = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.Skip);
            ViewBag.ViceSkip = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.ViceSkip);
            ViewBag.Lead = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.Lead);
            return View(wednesdayTeam);
        }

        // GET: WednesdayTeams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayTeam wednesdayTeam = db.WednesdayTeams.Find(id);
            if (wednesdayTeam == null)
            {
                return HttpNotFound();
            }
            ViewBag.Skip = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.Skip);
            ViewBag.ViceSkip = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.ViceSkip);
            ViewBag.Lead = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.Lead);
            return View(wednesdayTeam);
        }

        // POST: WednesdayTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Skip,ViceSkip,Lead")] WednesdayTeam wednesdayTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wednesdayTeam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Skip = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.Skip);
            ViewBag.ViceSkip = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.ViceSkip);
            ViewBag.Lead = new SelectList(db.Players.Where(x => x.WednesdayLeague), "id", "FullName", wednesdayTeam.Lead);
            return View(wednesdayTeam);
        }

        // GET: WednesdayTeams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayTeam wednesdayTeam = db.WednesdayTeams.Find(id);
            if (wednesdayTeam == null)
            {
                return HttpNotFound();
            }
            return View(wednesdayTeam);
        }

        // POST: WednesdayTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WednesdayTeam wednesdayTeam = db.WednesdayTeams.Find(id);
            db.WednesdayTeams.Remove(wednesdayTeam);
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
