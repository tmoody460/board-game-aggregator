using BoardGameAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAggregator.Repositories
{
    public interface IBoardGameGeekInfoRepository : IDisposable
    {
        IEnumerable<BoardGameGeekInfo> GetBoardGameGeekInfo();
        BoardGameGeekInfo GetBoardGameGeekInfo(Guid id);
        void InsertBoardGameGeekInfo(BoardGameGeekInfo game);
        void DeleteBoardGameGeekInfo(Guid id);
        void UpdateBoardGameGeekInfo(BoardGameGeekInfo game);
        void Save();
        
    }
}
