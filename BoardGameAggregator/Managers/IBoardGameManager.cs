using BoardGameAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAggregator.Managers
{
    public interface IBoardGameManager
    {
        List<BoardGame> GetBoardGamesSortByRank();

        void SaveGame(BoardGame game);

        BoardGame FindGame(Guid id);

        void RemoveGame(BoardGame game);

        void AddGame(BoardGame game);
    }
}
