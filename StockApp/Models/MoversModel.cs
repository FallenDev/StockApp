using System.Collections.Generic;

using Newtonsoft.Json;

namespace StockApp.Models
{
    public class MoversModel
    {
        public string MoversSym { get; set; }

        [JsonProperty("lastPrice")]
        public double? LastPrice { get; set; }

        [JsonProperty("changePct")]
        public double? ChangePct { get; set; }

        [JsonProperty("changeAfter")]
        public double? ChangeAfter { get; set; }

        [JsonProperty("changePre")]
        public double? ChangePre { get; set; }

        [JsonProperty("prevClosePrice")]
        public double? PrevClosePrice { get; set; }

        [JsonProperty("volume")]
        public int? Volume { get; set; }

        [JsonProperty("pctTenDayVol")]
        public double? PctTenDayVol { get; set; }
    }

    public class MoversSymbol
    {
        [JsonProperty("cnbcSymbol")]
        public string CnbcSymbol { get; set; }

        [JsonProperty("symbolData")]
        public MoversModel SymbolData { get; set; }
    }

    public class MoversOrder
    {
        [JsonProperty("rankOrder")]
        public string RankOrder { get; set; }

        [JsonProperty("rankedSymbols")]
        public IList<MoversSymbol> RankedSymbols { get; set; }
    }

    public class MoversGroup
    {
        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("delayed")]
        public bool Delayed { get; set; }

        [JsonProperty("rankDateTime")]
        public string RankDateTime { get; set; }

        [JsonProperty("rankedSymbolList")]
        public IList<MoversOrder> RankedSymbolList { get; set; }
    }
}