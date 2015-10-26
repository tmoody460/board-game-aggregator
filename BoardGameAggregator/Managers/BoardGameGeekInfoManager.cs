using BoardGameAggregator.Engines;
using BoardGameAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace BoardGameAggregator.Managers
{
    public class BoardGameGeekInfoManager
    {

        public Dictionary<string, string> LookupBoardGameList(string name)
        {
            Dictionary<string, string> games = new Dictionary<string, string>();

            XmlNodeList list = BoardGameGeekEngine.RequestSearchResults(name);

            foreach (XmlNode game in list)
            {
                XmlNodeList nameNodes = game.SelectNodes("name");
                for (var i = 0; i < nameNodes.Count; i++)
                {
                    XmlNode node = nameNodes[i];
                    if (node.Attributes["primary"] != null && node.Attributes["primary"].Value == "true")
                    {
                        games.Add(game.Attributes["objectid"].Value, node.InnerText);
                    }
                }
            }

            return games;
        }

        public BoardGameGeekInfo LookUpBoardGame(string name)
        {
            XmlNodeList list = BoardGameGeekEngine.RequestSearchResults(name);

            if(list.Count == 0)
            {
                throw new Exception("No games found.");
            }

            long id = FindGameId(name, list);

            if(id == -1)
            {
                throw new Exception("No games match.");
            }

            return LookUpBoardGame(id);
        }

        public BoardGameGeekInfo LookUpBoardGame(long id)
        {
            XmlNode gameNode = BoardGameGeekEngine.RequestGameDetails(id);

            if (gameNode == null)
            {
                // If this happens, something really funky is going on
                throw new Exception("Error retrieving game. Invalid id or no associated data.");
            }

            BoardGameGeekInfo info = new BoardGameGeekInfo();

            foreach (XmlNode node in gameNode.SelectNodes("name"))
            {
                if (node.Attributes["primary"] != null && node.Attributes["primary"].Value == "true")
                {
                    info.Name = node.InnerText;
                }
            }
            info.BggId = id;
            info.Link = "https://boardgamegeek.com/boardgame/" + id;
            info.Description = gameNode.SelectSingleNode("description").InnerText;
            info.ImageLink = gameNode.SelectSingleNode("thumbnail").InnerText;
            info.MinPlayers = Convert.ToInt32(gameNode.SelectSingleNode("minplayers").InnerText);
            info.MaxPlayers = Convert.ToInt32(gameNode.SelectSingleNode("maxplayers").InnerText);
            info.MinPlayingTime = Convert.ToInt32(gameNode.SelectSingleNode("minplaytime").InnerText);
            info.MaxPlayingTime = Convert.ToInt32(gameNode.SelectSingleNode("maxplaytime").InnerText);

            XmlNode statistics = gameNode.SelectSingleNode("statistics").SelectSingleNode("ratings");

            info.Rating = Convert.ToDouble(statistics.SelectSingleNode("average").InnerText);
            info.NumRatings = Convert.ToInt64(statistics.SelectSingleNode("usersrated").InnerText);
            string ranking = statistics.SelectSingleNode("ranks").SelectSingleNode("rank").Attributes["value"].Value;
            if (ranking != "Not Ranked")
            {
                info.Rank = Convert.ToInt32(statistics.SelectSingleNode("ranks").SelectSingleNode("rank").Attributes["value"].Value);
            }

            return info;
        }

        public string LookupBoardGameDetails(long id)
        {
            XmlNode game = BoardGameGeekEngine.RequestGameDetails(id);
            return game.SelectSingleNode("description").InnerText;
        }

        private long FindGameId(string name, XmlNodeList list)
        {
            long foundId = -1;
            Regex symbols = new Regex("[^a-zA-Z0-9 ]");
            string nameCleanedUp = symbols.Replace(name, " ").ToLower();
            nameCleanedUp = Regex.Replace(nameCleanedUp, @"\s+", " ");

            for (int i = 0; i < list.Count && foundId == -1; ++i)
            {
                XmlNode node = list[i];

                string bggName = node.SelectSingleNode("name").InnerText;
                string bggNameCleanedUp = symbols.Replace(bggName, " ").ToLower();
                bggNameCleanedUp = Regex.Replace(bggNameCleanedUp, @"\s+", " ");

                if (nameCleanedUp == bggNameCleanedUp)
                {
                    foundId = Convert.ToInt64(node.Attributes["objectid"].Value);
                }
            }

            return foundId;
        }
    }
}
