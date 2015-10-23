using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BoardGameAggregator.Models;
using System.Xml;
using System.Text.RegularExpressions;

namespace BoardGameAggregator.Controllers
{
    public class BoardGameGeekInfoesController : Controller
    {
        private SystemContext db = new SystemContext();

        public ActionResult LookupGame()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LookupGame(string name)
        {
            BoardGame game = new BoardGame();
            BoardGameGeekInfo boardGameGeekInfo = new BoardGameGeekInfo();

            game.Name = name;

            Regex symbols = new Regex("[^a-zA-Z0-9 ]");

            string nameCleanedUp = symbols.Replace(name, " ").ToLower();
            nameCleanedUp = Regex.Replace(nameCleanedUp, @"\s+", " ");

            string requestUrl = "https://www.boardgamegeek.com/xmlapi/search?search=" + name;

            XmlDocument searchResultsXmlDoc = CallRestApi(requestUrl);

            XmlNode games = searchResultsXmlDoc.SelectSingleNode("boardgames");

            XmlNodeList list = games.SelectNodes("boardgame");

            long foundId = -1;

            for(int i = 0; i < list.Count && foundId == -1; ++i)
            {
                XmlNode node = list[i];

                string bggName = node.SelectSingleNode("name").InnerText;
                string bggNameCleanedUp = symbols.Replace(bggName, " ").ToLower();
                bggNameCleanedUp = Regex.Replace(bggNameCleanedUp, @"\s+", " ");

                if(nameCleanedUp == bggNameCleanedUp)
                {
                    foundId = Convert.ToInt64(node.Attributes["objectid"].Value);
                    boardGameGeekInfo.Name = bggName;
                }
            }

            if (foundId > 0)
            {
                requestUrl = "https://boardgamegeek.com/xmlapi/boardgame/" + foundId + "?&stats=1";

                XmlDocument gameInfoXmlDoc = CallRestApi(requestUrl);

                XmlNode gameNode = gameInfoXmlDoc.SelectSingleNode("boardgames").SelectSingleNode("boardgame");

                boardGameGeekInfo.Link = "https://boardgamegeek.com/boardgame/" + foundId;

                boardGameGeekInfo.Description = gameNode.SelectSingleNode("description").InnerText;
                boardGameGeekInfo.ImageLink = gameNode.SelectSingleNode("thumbnail").InnerText;
                boardGameGeekInfo.MinPlayers = Convert.ToInt32(gameNode.SelectSingleNode("minplayers").InnerText);
                boardGameGeekInfo.MaxPlayers = Convert.ToInt32(gameNode.SelectSingleNode("maxplayers").InnerText);
                boardGameGeekInfo.MinPlayingTime = Convert.ToInt32(gameNode.SelectSingleNode("minplaytime").InnerText);
                boardGameGeekInfo.MaxPlayingTime = Convert.ToInt32(gameNode.SelectSingleNode("maxplaytime").InnerText);

                XmlNode statistics = gameNode.SelectSingleNode("statistics").SelectSingleNode("ratings");

                boardGameGeekInfo.Rating = Convert.ToDouble(statistics.SelectSingleNode("average").InnerText);
                boardGameGeekInfo.NumRatings = Convert.ToInt64(statistics.SelectSingleNode("usersrated").InnerText);
                
                boardGameGeekInfo.Rank = Convert.ToInt32(statistics.SelectSingleNode("ranks").SelectSingleNode("rank").Attributes["value"].Value);
            }
            else
            {
                // TODO: Handle game not found
                ViewBag.Error = "Game not found.";
                return View("LookupGame");
            }

            game.Id = Guid.NewGuid();
            game.Info = boardGameGeekInfo;
            game.Owned = false;
            game.Played = false;

            boardGameGeekInfo.Id = Guid.NewGuid();
            boardGameGeekInfo.BoardGame = game;

            db.BoardGames.Add(game);
            db.BoardGameGeekInfoes.Add(boardGameGeekInfo);
            db.SaveChanges();

            return View("Details", boardGameGeekInfo);
        }

        private XmlDocument CallRestApi(string url)
        {
            XmlDocument xmlDoc = null;

            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return xmlDoc;
        }

        // GET: BoardGameGeekInfoes
        public ActionResult Index()
        {
            var boardGameGeekInfoes = db.BoardGameGeekInfoes.Include(b => b.BoardGame);
            return View(boardGameGeekInfoes.ToList());
        }

        // GET: BoardGameGeekInfoes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardGameGeekInfo boardGameGeekInfo = db.BoardGameGeekInfoes.Find(id);
            if (boardGameGeekInfo == null)
            {
                return HttpNotFound();
            }
            return View(boardGameGeekInfo);
        }

        // GET: BoardGameGeekInfoes/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.BoardGames, "Id", "Name");
            return View();
        }

        // POST: BoardGameGeekInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,NumPlayers,Rank,Rating,NumRatings,PlayingTime,Link")] BoardGameGeekInfo boardGameGeekInfo)
        {
            if (ModelState.IsValid)
            {
                boardGameGeekInfo.Id = Guid.NewGuid();
                db.BoardGameGeekInfoes.Add(boardGameGeekInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.BoardGames, "Id", "Name", boardGameGeekInfo.Id);
            return View(boardGameGeekInfo);
        }

        // GET: BoardGameGeekInfoes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardGameGeekInfo boardGameGeekInfo = db.BoardGameGeekInfoes.Find(id);
            if (boardGameGeekInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.BoardGames, "Id", "Name", boardGameGeekInfo.Id);
            return View(boardGameGeekInfo);
        }

        // POST: BoardGameGeekInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NumPlayers,Rank,Rating,NumRatings,PlayingTime,Link")] BoardGameGeekInfo boardGameGeekInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boardGameGeekInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.BoardGames, "Id", "Name", boardGameGeekInfo.Id);
            return View(boardGameGeekInfo);
        }

        // GET: BoardGameGeekInfoes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardGameGeekInfo boardGameGeekInfo = db.BoardGameGeekInfoes.Find(id);
            if (boardGameGeekInfo == null)
            {
                return HttpNotFound();
            }
            return View(boardGameGeekInfo);
        }

        // POST: BoardGameGeekInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BoardGameGeekInfo boardGameGeekInfo = db.BoardGameGeekInfoes.Find(id);
            db.BoardGameGeekInfoes.Remove(boardGameGeekInfo);
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

        public JsonResult GetRelatedGames(string game)
        {


            return Json("");
        }
    }
}
