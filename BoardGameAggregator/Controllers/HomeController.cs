using BoardGameAggregator.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public JsonResult GetBoardGames(int page)
        {
            int pageSize = 10;

            var games = db.BoardGames.Include("Info").OrderBy(g => g.Name).ToPagedList(page, pageSize);

            return Json(games, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveGame(BoardGame game)
        {
            db.Entry(game).State = EntityState.Modified;
            db.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteGame(Guid id)
        {
            var game = db.BoardGames.Find(id);
            db.Entry(game).Reference(p => p.Info).Load();

            var bggInfo = db.BoardGameGeekInfoes.Find(game.Info.Id);

            db.BoardGameGeekInfoes.Remove(bggInfo);
            db.BoardGames.Remove(game);
            db.SaveChanges();

            return Json(true);
        }
    }
}