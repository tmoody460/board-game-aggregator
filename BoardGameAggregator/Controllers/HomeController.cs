using BoardGameAggregator.Managers;
using BoardGameAggregator.Models;
using BoardGameAggregator.Repositories;
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
        private IUnitOfWork unitOfWork;
        private IBoardGameManager boardGameManager;
        private IBoardGameGeekInfoManager bggInfoManager;

        public HomeController(IUnitOfWork unitOfWork, IBoardGameGeekInfoManager bggInfoManager, IBoardGameManager boardGameManager)
        {
            this.unitOfWork = unitOfWork;
            this.bggInfoManager = bggInfoManager;
            this.boardGameManager = boardGameManager;

        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetBoardGames(int page)
        {
            // TODO: Rethink paging mechanisms, especially with regard to filtering, sorting, adding
            //int pageSize = 10;
            //var games = db.BoardGames.Include("Info").OrderBy(g => g.Name).ToPagedList(page, pageSize);

            var games = boardGameManager.GetBoardGamesSortByRank();
            return Json(games, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveGame(BoardGame game)
        {
            boardGameManager.SaveGame(game);
            unitOfWork.Save();
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteGame(Guid id)
        {
            var game = boardGameManager.FindGame(id);
            var bggInfo = bggInfoManager.FindInfo(game.Info.Id);

            bggInfoManager.RemoveInfo(bggInfo);
            boardGameManager.RemoveGame(game);

            unitOfWork.Save();

            return Json(true);
        }

        public JsonResult LookupGameList(string name)
        {
            try
            {
                var games = bggInfoManager.LookupBoardGameList(name);
                return Json(games, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult LookupGameDescription(long id)
        {
            try
            {
                var description = bggInfoManager.LookupBoardGameDetails(id);
                return Json(description, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddGame(long id)
        {
            BoardGameGeekInfo gameInfo = null;
            BoardGame game = new BoardGame();

            try
            {
                gameInfo = bggInfoManager.LookUpBoardGame(id);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

            // Until custom name is set, use BGG's
            game.Name = gameInfo.Name;
            game.Id = Guid.NewGuid();
            game.Rating = 0;
            game.Played = false;
            game.Owned = false;
            game.Comments = "";
            game.Info = gameInfo;

            gameInfo.Id = Guid.NewGuid();
            gameInfo.BoardGame = game;

            bggInfoManager.AddInfo(gameInfo);
            boardGameManager.AddGame(game);
            unitOfWork.Save();

            return Json(game, JsonRequestBehavior.AllowGet);
        }
    }
}