using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoardGameAggregator.Models;
using System.Data.Entity;

namespace BoardGameAggregator.Repositories
{
    public class BoardGameRepository : IBoardGameRepository
    {
        private SystemContext context;

        public BoardGameRepository(SystemContext context)
        {
            this.context = context;
        }

        public void DeleteBoardGame(Guid id)
        {
            var game = this.context.BoardGames.Find(id);
            this.context.BoardGames.Remove(game);
        }

        public BoardGame GetBoardGame(Guid id)
        {
            var game = context.BoardGames.Find(id);
            context.Entry(game).Reference(p => p.Info).Load();
            return game;
        }

        public IEnumerable<BoardGame> GetBoardGames()
        {
            return context.BoardGames.Include(g => g.Info).ToList();
        }

        public void InsertBoardGame(BoardGame game)
        {
            context.BoardGames.Add(game);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateBoardGame(BoardGame game)
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