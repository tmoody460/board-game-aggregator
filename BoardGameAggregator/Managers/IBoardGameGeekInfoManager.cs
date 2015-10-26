using BoardGameAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BoardGameAggregator.Managers
{
    public interface IBoardGameGeekInfoManager
    {

        Dictionary<string, string> LookupBoardGameList(string name);

        BoardGameGeekInfo LookUpBoardGame(string name);

        BoardGameGeekInfo LookUpBoardGame(long id);

        string LookupBoardGameDetails(long id);

        BoardGameGeekInfo FindInfo(Guid id);

        void RemoveInfo(BoardGameGeekInfo bggInfo);

        void AddInfo(BoardGameGeekInfo gameInfo);
    }
}
