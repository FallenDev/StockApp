using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockApp.Models;

namespace StockApp.Interfaces
{
    public interface ICnbcController
    {
        Task<object> GetMoversAsync();
        Task CnbcResponseAsync();
        object DeserializeQuote(string jsonData);
        MoversGroup DeserializeQuotesJson(string jsonData);
    }
}
