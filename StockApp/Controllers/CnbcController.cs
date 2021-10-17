using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using StockApp.Models;
using RestSharp;
using StockApp.Interfaces;

namespace StockApp.Controllers
{
    public class CnbcController: ICnbcController
    {
        #region Auth Variables

        private const string BaseAdd = "https://gdsapi.cnbc.com/market-mover/groupMover/";
        private const string TrailAdd = "/CHANGE_PCT/BOTH/12.json?source=SAVED&delayed=false&partnerId=2";
        public string Exchange = "";

        #endregion

        #region Quote Variables

        private string _jsonData;

        #endregion

        public async Task<object> GetMoversAsync()
        {
            Analytics.TrackEvent("Movers Data Fetched");
            await CnbcResponseAsync();
            return DeserializeQuote(_jsonData);
        }

        public async Task CnbcResponseAsync()
        {
            if (string.IsNullOrEmpty(Exchange)) return;
            var conv = $"{BaseAdd}{Exchange}{TrailAdd}";
            var client = new RestClient(conv);
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteAsync(request);
            Console.Write(response);
            _jsonData = response.Content;
        }

        public object DeserializeQuote(string jsonData)
        {
            var dict = DeserializeQuotesJson(jsonData);
            var count0 = 0;
            var count1 = 0;
            MainWindow.GroupName = dict.GroupName;

            try
            {
                if (MainWindow.TopOrBottom)
                {
                    foreach (var unused in dict.RankedSymbolList[0].RankedSymbols)
                    {
                        MainWindow.MoversSymbol = dict.RankedSymbolList[0].RankedSymbols[count0].CnbcSymbol;
                        MainWindow.MoversLastPrice = dict.RankedSymbolList[0].RankedSymbols[count0].SymbolData.LastPrice;
                        MainWindow.ChangePct = dict.RankedSymbolList[0].RankedSymbols[count0].SymbolData.ChangePct;
                        MainWindow.ChangeAfter = dict.RankedSymbolList[0].RankedSymbols[count0].SymbolData.ChangeAfter;
                        MainWindow.ChangePre = dict.RankedSymbolList[0].RankedSymbols[count0].SymbolData.ChangePre;
                        MainWindow.PrevClosePrice = dict.RankedSymbolList[0].RankedSymbols[count0].SymbolData.PrevClosePrice;
                        MainWindow.MoversVolume = dict.RankedSymbolList[0].RankedSymbols[count0].SymbolData.Volume;
                        MainWindow.PctTenDayVol = dict.RankedSymbolList[0].RankedSymbols[count0].SymbolData.PctTenDayVol;
                        MainWindow.Movers.Add(new MoversModel() { MoversSym = MainWindow.MoversSymbol, LastPrice = MainWindow.MoversLastPrice, ChangePct = MainWindow.ChangePct, ChangeAfter = MainWindow.ChangeAfter, ChangePre = MainWindow.ChangePre, PrevClosePrice = MainWindow.PrevClosePrice, Volume = MainWindow.MoversVolume, PctTenDayVol = MainWindow.PctTenDayVol });
                        count0++;
                    }
                }
                else
                {
                    foreach (var unused in dict.RankedSymbolList[1].RankedSymbols)
                    {
                        MainWindow.MoversSymbol = dict.RankedSymbolList[1].RankedSymbols[count1].CnbcSymbol;
                        MainWindow.MoversLastPrice = dict.RankedSymbolList[1].RankedSymbols[count1].SymbolData.LastPrice;
                        MainWindow.ChangePct = dict.RankedSymbolList[1].RankedSymbols[count1].SymbolData.ChangePct;
                        MainWindow.ChangeAfter = dict.RankedSymbolList[1].RankedSymbols[count1].SymbolData.ChangeAfter;
                        MainWindow.ChangePre = dict.RankedSymbolList[1].RankedSymbols[count1].SymbolData.ChangePre;
                        MainWindow.PrevClosePrice = dict.RankedSymbolList[1].RankedSymbols[count1].SymbolData.PrevClosePrice;
                        MainWindow.MoversVolume = dict.RankedSymbolList[1].RankedSymbols[count1].SymbolData.Volume;
                        MainWindow.PctTenDayVol = dict.RankedSymbolList[1].RankedSymbols[count1].SymbolData.PctTenDayVol;
                        MainWindow.Movers.Add(new MoversModel() { MoversSym = MainWindow.MoversSymbol, LastPrice = MainWindow.MoversLastPrice, ChangePct = MainWindow.ChangePct, ChangeAfter = MainWindow.ChangeAfter, ChangePre = MainWindow.ChangePre, PrevClosePrice = MainWindow.PrevClosePrice, Volume = MainWindow.MoversVolume, PctTenDayVol = MainWindow.PctTenDayVol });
                        count1++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return dict;
        }

        public MoversGroup DeserializeQuotesJson(string jsonData)
        {
            var moversQuoteDictionary = JsonConvert.DeserializeObject<MoversGroup>(jsonData, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });

            return moversQuoteDictionary;
        }
    }
}
