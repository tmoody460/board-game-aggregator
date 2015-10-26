using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace BoardGameAggregator.Engines
{
    public class BoardGameGeekEngine : IBoardGameGeekEngine
    {
        private IRestApiEngine engine;

        public BoardGameGeekEngine(IRestApiEngine engine)
        {
            this.engine = engine;
        }
        
        public XmlNodeList RequestSearchResults(string gameName)
        {
            try
            {
                string requestUrl = "https://www.boardgamegeek.com/xmlapi/search?search=" + gameName;

                XmlDocument searchResultsXmlDoc = engine.CallRestApi(requestUrl);

                XmlNode games = searchResultsXmlDoc.SelectSingleNode("boardgames");

                XmlNodeList list = games.SelectNodes("boardgame");

                return list;
            }
            catch (Exception e)
            {
                throw new Exception("Error requesting search results from BGG.", e);
            }
        }

        public XmlNode RequestGameDetails(long id)
        {
            try { 
                string requestUrl = "https://boardgamegeek.com/xmlapi/boardgame/" + id + "?&stats=1";

                XmlDocument gameInfoXmlDoc = engine.CallRestApi(requestUrl);

                XmlNode gameNode = gameInfoXmlDoc.SelectSingleNode("boardgames").SelectSingleNode("boardgame");

                return gameNode;
            }
            catch (Exception e)
            {
                throw new Exception("Error requesting game results from BGG.", e);
            }
        }

    }
}