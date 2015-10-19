using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BoardGameAggregator.Models;

namespace BoardGameAggregator.Controllers
{
    public class BoardGameGeekInfoesController : Controller
    {
        private SystemContext db = new SystemContext();

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
    }
}
