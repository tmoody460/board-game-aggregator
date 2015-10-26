using BoardGameAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAggregator.Repositories
{
    public interface IUnitOfWork
    {
        IBoardGameRepository GetBoardGameRepository();
        IBoardGameGeekInfoRepository GetBoardGameGeekInfoRepository();
        void Save();

    }
}
