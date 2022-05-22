using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProviderTest
    {
    interface IWikipediaProvider
        {
        string AskWikiPedia(string query);
        }
    }
