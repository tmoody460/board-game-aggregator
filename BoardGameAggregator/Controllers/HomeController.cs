﻿using BoardGameAggregator.Managers;
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

        public JsonResult LookupGameList(string name)
        {
            try
            {
                BoardGameGeekInfoManager manager = new BoardGameGeekInfoManager();
                var games = manager.LookupBoardGameList(name);
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
                BoardGameGeekInfoManager manager = new BoardGameGeekInfoManager();
                var description = manager.LookupBoardGameDetails(id);
                return Json(description, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LookUpGame(string name)
        {
            BoardGameGeekInfo gameInfo = null;
            BoardGame game = new BoardGame();

            try
            {
                BoardGameGeekInfoManager manager = new BoardGameGeekInfoManager();
                gameInfo = manager.LookUpBoardGame(name);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

            // Unless custom name is set, use BGG's
            game.Name = gameInfo.Name;
            game.Id = Guid.NewGuid();
            game.Rating = 0;
            game.Played = false;
            game.Owned = false;
            game.Comments = "";
            game.Info = gameInfo;

            gameInfo.Id = Guid.NewGuid();
            gameInfo.BoardGame = game;

            db.BoardGames.Add(game);
            db.BoardGameGeekInfoes.Add(gameInfo);
            db.SaveChanges();

            return Json(game, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddGame(long id)
        {
            BoardGameGeekInfo gameInfo = null;
            BoardGame game = new BoardGame();

            try
            {
                BoardGameGeekInfoManager manager = new BoardGameGeekInfoManager();
                gameInfo = manager.LookUpBoardGame(id);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

            // Unless custom name is set, use BGG's
            game.Name = gameInfo.Name;
            game.Id = Guid.NewGuid();
            game.Rating = 0;
            game.Played = false;
            game.Owned = false;
            game.Comments = "";
            game.Info = gameInfo;

            gameInfo.Id = Guid.NewGuid();
            gameInfo.BoardGame = game;

            db.BoardGames.Add(game);
            db.BoardGameGeekInfoes.Add(gameInfo);
            db.SaveChanges();

            return Json(game, JsonRequestBehavior.AllowGet);
        }
    }
}