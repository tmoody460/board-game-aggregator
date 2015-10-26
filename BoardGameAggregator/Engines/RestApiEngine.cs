using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace BoardGameAggregator.Engines
{
    public class RestApiEngine : IRestApiEngine
    {

        public XmlDocument CallRestApi(string url)
        {
            XmlDocument xmlDoc = null;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());

            return xmlDoc;
        }

    }
}