using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BoardGameAggregator.Engines
{
    public interface IBoardGameGeekEngine
    {
        XmlNodeList RequestSearchResults(string gameName);
        XmlNode RequestGameDetails(long id);
    }
}
