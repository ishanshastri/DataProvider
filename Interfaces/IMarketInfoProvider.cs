using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProviderTest
    {
    interface IMarketInfoProvider
        {
        string GetMarketInfo(Market market);   
        }
    }
