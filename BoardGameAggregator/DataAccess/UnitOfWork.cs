using BoardGameAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAggregator.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private SystemContext context = new SystemContext();
        private IBoardGameRepository boardGameRepository;
        private IBoardGameGeekInfoRepository bggInfoRepository;

        public UnitOfWork()
        {
            this.context = new SystemContext();
        }

        public void Save()
        {
            context.SaveChanges();
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

        public IBoardGameRepository GetBoardGameRepository()
        {
            if (boardGameRepository == null)
            {
                boardGameRepository = new BoardGameRepository(context);
            }
            return boardGameRepository;
        }

        public IBoardGameGeekInfoRepository GetBoardGameGeekInfoRepository()
        {
            if (bggInfoRepository == null)
            {
                bggInfoRepository = new BoardGameGeekInfoRepository(context);
            }
            return bggInfoRepository;
        }
    }
}
