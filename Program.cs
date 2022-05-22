using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProviderTest
    {
    class Program
        {
        static void Main(string[] args)
            {/*
            IMarketInfoProvider mip = new MarketInfoProvider();
            string mktInfo = mip.GetMarketInfo(Market.USA);
            Console.WriteLine(mktInfo);*/
            IWikipediaProvider wp = new WikipediaProvider();
            while (true)
                {
                Console.Write("> ");
                string question = Console.ReadLine();
                string answer = wp.AskWikiPedia(question);
                Console.WriteLine("ANS: " + answer);
                Console.WriteLine();
                }   
            }
        }
    }
