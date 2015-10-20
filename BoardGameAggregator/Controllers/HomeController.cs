using BoardGameAggregator.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoardGameAggregator.Controllers
{
    public class HomeController : Controller
    {
        private SystemContext db = new SystemContext();

        public ActionResult Index()
        {
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

        public JsonResult GetBoardGames(int page)
        {
            int pageSize = 10;

            var games = db.BoardGames.Include("Info").OrderBy(g => g.Name).ToPagedList(page, pageSize);

            return Json(games, JsonRequestBehavior.AllowGet);
        }
    }
}