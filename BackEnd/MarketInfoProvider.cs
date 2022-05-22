using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataProviderTest
    {
    class MarketInfoProvider : IMarketInfoProvider
        {
        public string GetMarketInfo(Market market)
            {
            //get url of yahoo finance page fo rthis market
            string url = GetMarketURL(market);

            //get html of url
            string html = GetHtml(url);

            //extract market info
            string marketInfo = ExtractMarketInfo(html);
            
            //return market info
            return marketInfo;
            }

        private string GetMarketURL(Market m)
            {
            switch (m)
                {
                case Market.India:
                    return "https://in.finance.yahoo.com/";
                case Market.USA:
                    return "https://finance.yahoo.com/";
                default:
                    throw new Exception("Unkown Market");
                }
            }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The HTML or null if error</returns>
        private string GetHtml(string url)
            {
            
            try
                {
                WebClient wc = new WebClient();
                return wc.DownloadString(url);
                }
            catch
                {
                return null;
                }
            }

        private string ExtractMarketInfo(string html)
            {
            //skip to start of "MarketTimeStore"
            string mktTimeStore = "\"MarketTimeStore\"";
            int start = html.IndexOf(mktTimeStore);
            if(start != -1)
                {
                //skip to start of "message" ('reuse start')
                string msgString = "\"message\":\"";
                start = html.IndexOf(msgString, start + mktTimeStore.Length);
                if(start != -1)
                    {
                    int end = html.IndexOf("\"}", start + msgString.Length);
                    start += msgString.Length;
                    if(end != -1)
                        {
                        //return the quoted text
                        return html.Substring(start, end-start);
                        }
                    }
                }
            //Could not extract market info
            return null;
            }
        }
    }
