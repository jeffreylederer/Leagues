using Leagues.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Leagues.Controllers
{
    [Authorize]
    public class WednesdayTeamsController : Controller
    {
        private LeaguesEntities db = new LeaguesEntities();

        // GET: WednesdayTeams
        public ActionResult Index()
        {
            var WednesdayTeams = db.WednesdayTeams.Include(t => t.Player).Include(t => t.Player1);
            return View(WednesdayTeams.OrderBy(x => x.id).ToList());
        }

        public ActionResult RemoveLead(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayTeam WednesdayTeam = db.WednesdayTeams.Find(id);
            if (WednesdayTeam == null)
            {
                return HttpNotFound();
            }
            WednesdayTeam.Lead = null;
            db.Entry(WednesdayTeam).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveViceSkip(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayTeam WednesdayTeam = db.WednesdayTeams.Find(id);
            if (WednesdayTeam == null)
            {
                return HttpNotFound();
            }
            WednesdayTeam.ViceSkip = null;
            db.Entry(WednesdayTeam).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        // GET: WednesdayTeams/Create
        public ActionResult Create()
        {
            var items = db.WednesdayTeams.ToList();
            int id = 1;
            if (items.Count > 1)
            {
                items.Sort((a, b) => a.id.CompareTo(b.id));
                id = items[items.Count - 1].id + 1;
            }

            var item = new WednesdayTeam()
            {
                id = id
            };
            var teams = db.WednesdayTeams.OrderBy(x => x.id);
            var list = new List<Player>();
            foreach (var player in db.Players.Where(x => x.WednesdayLeague))
            {
                if (!teams.Any(x => x.Skip == player.id || x.Lead == player.id || x.ViceSkip == player.id))
                    list.Add(player);
            }
            ViewBag.Skip = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", " ");
            ViewBag.ViceSkip = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", " ");
            ViewBag.Lead = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", " ");
            ViewBag.Teams = teams;
            return View(item);
        }

        // POST: WednesdayTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Skip,Lead,ViceSkip")] WednesdayTeam WednesdayTeam)
        {
            if (ModelState.IsValid)
            {
                db.WednesdayTeams.Add(WednesdayTeam);
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

            var teams = db.WednesdayTeams.OrderBy(x => x.id);
            var list = new List<Player>();
            foreach (var player in db.Players.Where(x => x.WednesdayLeague))
            {
                if (!teams.Any(x => x.Skip == player.id || x.Lead == player.id || x.ViceSkip == player.id))
                    list.Add(player);
            }
            ViewBag.Skip = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", WednesdayTeam.Skip);
            ViewBag.ViceSkip = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", WednesdayTeam.ViceSkip);
            ViewBag.Lead = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", WednesdayTeam.Lead);
            ViewBag.Teams = teams;
            return View(WednesdayTeam);
        }

        // GET: WednesdayTeams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayTeam WednesdayTeam = db.WednesdayTeams.Find(id);
            if (WednesdayTeam == null)
            {
                return HttpNotFound();
            }
            var teams = db.WednesdayTeams.OrderBy(x => x.id);
            var list = new List<Player>();
            foreach (var player in db.Players.Where(x => x.WednesdayLeague))
            {
                if(!teams.Any(x => x.Skip == player.id || x.Lead == player.id || x.ViceSkip == player.id))
                    list.Add(player);
            }
            if (WednesdayTeam.ViceSkip != null)
                list.Add(WednesdayTeam.Player1);
            if (WednesdayTeam.Lead != null)
                list.Add(WednesdayTeam.Player2);
            ViewBag.Lead = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", WednesdayTeam.Lead);
            ViewBag.ViceSkip = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", WednesdayTeam.ViceSkip);
            ViewBag.Teams = teams;
            return View(WednesdayTeam);
        }

        // POST: WednesdayTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Skip,Lead,ViceSkip")] WednesdayTeam WednesdayTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(WednesdayTeam).State = EntityState.Modified;
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
            var teams = db.WednesdayTeams.OrderBy(x => x.id);

            var list = new List<Player>();
            foreach (var player in db.Players.Where(x => x.WednesdayLeague))
            {
                if (!teams.Any(x => x.Skip == player.id || x.Lead == player.id || x.ViceSkip == player.id))
                    list.Add(player);
            }
            if (WednesdayTeam.ViceSkip != null)
                list.Add(WednesdayTeam.Player1);
            if (WednesdayTeam.Lead != null)
                list.Add(WednesdayTeam.Player2);
            ViewBag.Lead = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", WednesdayTeam.Lead);
            ViewBag.ViceSkip = new SelectList(list.OrderBy(x => x.LastName), "id", "FullName", WednesdayTeam.ViceSkip);
            ViewBag.Teams = teams;
            return View(WednesdayTeam);
        }

        // GET: WednesdayTeams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WednesdayTeam WednesdayTeam = db.WednesdayTeams.Find(id);
            if (WednesdayTeam == null)
            {
                return HttpNotFound();
            }
            return View(WednesdayTeam);
        }

        // POST: WednesdayTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WednesdayTeam WednesdayTeam = db.WednesdayTeams.Find(id);
            db.WednesdayTeams.Remove(WednesdayTeam);
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
            return View(WednesdayTeam);
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
