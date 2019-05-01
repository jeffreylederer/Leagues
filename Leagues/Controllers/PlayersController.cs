using Leagues.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Leagues.Controllers
{
    public class PlayersController : Controller
    {
        private readonly LeagueEntities db = new LeagueEntities();

        // GET: Players
        public ActionResult Index(string Filter)
        {
            var choiceList = new List<choice>();
            choiceList.Add(new choice()
            {
                value = "1",
                text = "Tuesday"
            });
            choiceList.Add(new choice()
            {
                value = "2",
                text = "Wednesday"
            });
            choiceList.Add(new choice()
            {
                value = "3",
                text = "Both"
            });

            ViewBag.Filter = new SelectList(choiceList, "value", "text", "3");
            List<Player> list;
            switch (Filter)
            {
                case "1":
                    list = db.Players.Where(x => x.TuesdayLeague).ToList();
                    break;
                case "2":
                    list = db.Players.Where(x => x.WednesdayLeague).ToList();
                    break;
                default:
                    list = db.Players.ToList();
                    break;
            }
            list.Sort(delegate(Player b1, Player b2)
            {
                int res = b1.LastName.CompareTo(b2.LastName);
                return res != 0 ? res : b1.FirstName.CompareTo(b2.FirstName);
            });
            ViewBag.Count = list.Count;
            return View(list);
        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,FirstName,LastName,TuesdayLeague,WednesdayLeague")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
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

            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FirstName,LastName,TuesdayLeague,WednesdayLeague")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
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
            return View(player);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
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
                ViewBag.Error =  ex.Message;
            }
            catch (Exception)
            {
                ViewBag.Error= "Delete failed";
            }
            return View(player);
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

    public class choice
    {
        public string value { get; set; }
        public string text { get; set; }
    }
}
