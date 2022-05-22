using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataProviderTest
    {
    class WikipediaProvider : IWikipediaProvider
        {
        public string AskWikiPedia(string query)
            {
            try
                {
                //Create url
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //get url
                string url = "https://en.wikipedia.org/api/rest_v1/page/summary/" + HttpUtility.UrlPathEncode(query);
                      //Get JSON

                WebClient wc = new WebClient();
                string json = wc.DownloadString(url);

                //Extract result = i.e. stuff between "extract:" and a period.
                string result = null;
                string start = "\"extract\":\"";
                string end = ".";
                result = extractString(start, end, json);
                //Check if there are multiple results, and put results into a list
                //Method 1: **********************
                /*
                string[] separators = { "\\n" };
                string[] responseArr = result.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string response in responseArr)
                    {
                    int startIndex = response.IndexOf("to:") + 3;
                    string text = response.Substring(startIndex, response.Length - startIndex - 1);
                    string sourceUrl = string.Format("https://en.wikipedia.org/w/index.php?title={0}&action=info", HttpUtility.UrlPathEncode(text.Split()[0]));
                    string html = wc.DownloadString(sourceUrl);
                    int numViews = 0;
                    if(Int32.TryParse(extractString("mw-pvi-month\">", "<", html), out int amount))
                        {
                        numViews = amount;
                        }
                    }
                    */

                //Method 2 *********************
                //Check if the result has multiple lines, to see if there are multiple responses.
                if (result.Contains("\\n"))
                    {
                    result = getResponseFromList(query);
                    /*
                    //Send query to wikishark to get the list of topics in order of most to least relevance (it is automatically sorted as such)
                    string viewsUrl = string.Format("https://www.wikishark.com/autocomplete.php?q={0}", HttpUtility.UrlPathEncode(query));
                    string html = wc.DownloadString(viewsUrl).Replace("[", "").Replace("]", "");
                    string[] separators = {"{\"name\":\""};
                    string[] topicArr = html.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
                    //Send list of subjects to a list
                    List<string> topicList = new List<string>(topicArr);
                    //Clean up each string
                    for(int i = 0; i < topicList.Count(); i++)
                        {
                        string topic = topicList[i];
                        topicList[i] = topic.Substring(0, topic.IndexOf(" (")).Trim().ToLower();
                        }

                    //Remove the original query result from list
                    topicList.Remove(query.ToLower().Trim());
                    foreach(string s in topicList)
                        {
                        Console.WriteLine(string.Format("DID YOU MEAN {0}?", s));
                        if (getYesNo())
                            {
                            return AskWikiPedia(s);
                            }
                        }
                    
                    //   int startIndex = html.IndexOf()*/
                    }
                //return the result.
                return result;
                }
            catch(Exception ex)
                {
                return getResponseFromList(query);
                }
            }

        //Run through list of possible wikipedia subjects to respond to, and return response to selected one by user
        string getResponseFromList(string query)
            {
                {
                //Send query to wikishark to get the list of topics in order of most to least relevance (it is automatically sorted as such)
                WebClient wc = new WebClient();
                string viewsUrl = string.Format("https://www.wikishark.com/autocomplete.php?q={0}", HttpUtility.UrlPathEncode(query));
                string html = wc.DownloadString(viewsUrl).Replace("[", "").Replace("]", "");
                string[] separators = { "{\"name\":\"" };
                string[] topicArr = html.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
                //Send list of subjects to a list
                List<string> topicList = new List<string>(topicArr);
                //Clean up each string
                for (int i = 0; i < topicList.Count(); i++)
                    {
                    string topic = topicList[i];
                    topicList[i] = topic.Substring(0, topic.IndexOf(" (")).Trim();
                    if (topicList[i].ToLower().Equals(query.ToLower().Trim()))
                        {
                        topicList[i] = topicList[i].ToLower();
                        }
                    }

                //Remove the original query result from list
                topicList.Remove(query.ToLower().Trim());
                foreach (string s in topicList)
                    {
                    Console.WriteLine(string.Format("Did you mean {0}?", s));
                    Console.WriteLine();
                    if (getYesNo())
                        {
                        return AskWikiPedia(s);
                        }
                    }
                return null;
                }
            }

        //Get a user response of yes or no
        private bool getYesNo()
            {
            Console.Write("> ");
            if (Console.ReadLine().ToLower().Contains("y"))
                {
                return true;
                }
            return false;
            }

        //Take two strings, and find the string in between them
        private string extractString(string start, string end, string text)
            {
            string result = null;
            int startPos = text.IndexOf(start);//Get starting index
            //If the text contains the starting string, proceed to find the index of the end string
            if (startPos != -1)
                {
                int endPos = text.IndexOf(end, startPos + start.Length);
                if (endPos != -1)
                    {
                    startPos += start.Length;
                    result = text.Substring(startPos, endPos - startPos + 1);
                    }
                }
            return result;
            }
        }
    }
