using BoardGameAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAggregator.Repositories
{
    public interface IBoardGameRepository : IDisposable
    {
        IEnumerable<BoardGame> GetBoardGames();
        BoardGame GetBoardGame(Guid id);
        void InsertBoardGame(BoardGame game);
        void DeleteBoardGame(Guid id);
        void UpdateBoardGame(BoardGame game);
        void Save();
        
    }
}
