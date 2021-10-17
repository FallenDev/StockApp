using System.Collections.Generic;
using System.Threading.Tasks;
using StockApp.Models;

namespace StockApp.Interfaces
{
    public interface ITDController
    {
        void Initiate();
        Task GetQuoteAsync(string sym);
        Task TDResponseAsync();
        object DeserializeQuote(string jsonData, string sym);
        Dictionary<string, TDQuoteModel> DeserializeQuotesToJson(string jsonData);
        void TDQuoteAssign();
        void FetchAuth();
        void RefreshAuth();
    }
}
