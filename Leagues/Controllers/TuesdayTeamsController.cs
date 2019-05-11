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

namespace Leagues.Controllers
{
    public class TuesdayTeamsController : Controller
    {
        private LeaguesEntities db = new LeaguesEntities();

        // GET: TuesdayTeams
        public ActionResult Index()
        {
            var tuesdayTeams = db.TuesdayTeams.Include(t => t.Player).Include(t => t.Player1);
            return View(tuesdayTeams.OrderBy(x=>x.id).ToList());
        }

        public ActionResult RemoveLead(int? id)
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
            tuesdayTeam.Lead = null;
            db.Entry(tuesdayTeam).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        

        // GET: TuesdayTeams/Create
        public ActionResult Create()
        {
            var item1 = db.TuesdayTeams.OrderByDescending(x => x.id).First();
            var id = (item1 == null) ? 1 : item1.id + 1;
            var item = new TuesdayTeam()
            {
                id = id
            };
            var teams = db.TuesdayTeams.OrderBy(x => x.id);
            var list = new List<Player>();
            foreach (var player in db.Players.Where(x => x.TuesdayLeague))
            {
                if(!teams.Any(x=>x.Skip == player.id || x.Lead == player.id))
                    list.Add(player);
            }
            ViewBag.Skip = new SelectList(list.OrderBy(x=>x.LastName), "id", "FullName"," ");
            ViewBag.Lead = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName"," ");
            ViewBag.Teams = teams;
            return View(item);
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

            var teams = db.TuesdayTeams.OrderBy(x => x.id);
            var list = new List<Player>();
            foreach (var player in db.Players.Where(x => x.TuesdayLeague))
            {
                if (!teams.Any(x => x.Skip == player.id || x.Lead == player.id))
                    list.Add(player);
            }
            ViewBag.Skip = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", " ");
            ViewBag.Lead = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", " ");
            ViewBag.Teams = teams;
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
            var teams = db.TuesdayTeams.OrderBy(x => x.id);
            var list = new List<Player>();
            foreach (var player in db.Players.Where(x => x.TuesdayLeague))
            {
                if (!teams.Any(x => x.Skip == player.id || x.Lead == player.id))
                    list.Add(player);
            }
            ViewBag.Lead = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", tuesdayTeam.Lead);
            ViewBag.Teams = teams;
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
            var teams = db.TuesdayTeams.OrderBy(x => x.id);

            var list = new List<Player>();
            foreach (var player in db.Players.Where(x => x.TuesdayLeague))
            {
                if (!teams.Any(x => x.Skip == player.id || x.Lead == player.id))
                    list.Add(player);
            }
            ViewBag.Lead = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", tuesdayTeam.Lead);
            ViewBag.Teams = teams;
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
