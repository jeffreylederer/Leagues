using Leagues.Code;
using System.Collections.Generic;
using System.Web.Mvc;


namespace Leagues.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var cs = new CreateSchedule();
            //List<Match> matches = cs.NoByes(9, 12);
            return View();
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