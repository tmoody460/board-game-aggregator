using BoardGameAggregator.Engines;
using BoardGameAggregator.Models;
using BoardGameAggregator.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace BoardGameAggregator.Managers
{
    public class BoardGameManager : IBoardGameManager
    {
        IUnitOfWork unitOfWork;
        IBoardGameRepository boardGameRepo;

        public BoardGameManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.boardGameRepo = unitOfWork.GetBoardGameRepository();
        }

        public List<BoardGame> GetBoardGamesSortByRank()
        {
            return boardGameRepo.GetBoardGames().OrderByDescending(g => g.Info.Rating).ToList();
        }

        public void SaveGame(BoardGame game)
        {
            boardGameRepo.UpdateBoardGame(game);
        }

        public BoardGame FindGame(Guid id)
        {
            return boardGameRepo.GetBoardGame(id);
        }

        public void RemoveGame(BoardGame game)
        {
            boardGameRepo.DeleteBoardGame(game.Id);
        }

        public void AddGame(BoardGame game)
        {
            boardGameRepo.InsertBoardGame(game);
        }
    }

}
