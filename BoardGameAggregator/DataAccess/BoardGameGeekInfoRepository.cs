using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoardGameAggregator.Models;
using System.Data.Entity;

namespace BoardGameAggregator.Repositories
{
    public class BoardGameGeekInfoRepository : IBoardGameGeekInfoRepository
    {
        private SystemContext context;

        public BoardGameGeekInfoRepository(SystemContext context)
        {
            this.context = context;
        }

        public void DeleteBoardGameGeekInfo(Guid id)
        {
            var game = this.context.BoardGameGeekInfo.Find(id);
            this.context.BoardGameGeekInfo.Remove(game);
        }

        public BoardGameGeekInfo GetBoardGameGeekInfo(Guid id)
        {
            return context.BoardGameGeekInfo.Find(id);
        }

        public IEnumerable<BoardGameGeekInfo> GetBoardGameGeekInfo()
        {
            return context.BoardGameGeekInfo.ToList<BoardGameGeekInfo>();
        }

        public void InsertBoardGameGeekInfo(BoardGameGeekInfo game)
        {
            context.BoardGameGeekInfo.Add(game);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateBoardGameGeekInfo(BoardGameGeekInfo game)
        {
            context.Entry(game).State =  EntityState.Modified;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}