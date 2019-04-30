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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cs = new CreateSchedule();
            List<Match> matches = cs.DoIt(9, 12);
            return View(matches);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}