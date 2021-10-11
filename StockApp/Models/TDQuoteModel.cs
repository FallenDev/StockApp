using Newtonsoft.Json;

namespace StockApp.Models
{
    public class TDQuoteModel
    {
        [JsonProperty("assetType")]
        public string AssetType { get; set; }

        [JsonProperty("assetMainType")]
        public string AssetMainType { get; set; }

        [JsonProperty("cusip")]
        public string Cusip { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("bidPrice")]
        public float? BidPrice { get; set; }

        [JsonProperty("bidSize")]
        public int? BidSize { get; set; }

        [JsonProperty("bidId")]
        public string BidId { get; set; }

        [JsonProperty("askPrice")]
        public float? AskPrice { get; set; }

        [JsonProperty("askSize")]
        public int? AskSize { get; set; }

        [JsonProperty("askId")]
        public string AskId { get; set; }

        [JsonProperty("lastPrice")]
        public float? LastPrice { get; set; }

        [JsonProperty("lastSize")]
        public int? LastSize { get; set; }

        [JsonProperty("lastId")]
        public string LastId { get; set; }

        [JsonProperty("openPrice")]
        public float? OpenPrice { get; set; }

        [JsonProperty("highPrice")]
        public float? HighPrice { get; set; }

        [JsonProperty("lowPrice")]
        public float? LowPrice { get; set; }

        [JsonProperty("bidTick")]
        public string BidTick { get; set; }

        [JsonProperty("closePrice")]
        public float? ClosePrice { get; set; }

        [JsonProperty("netChange")]
        public double? NetChange { get; set; }

        [JsonProperty("totalVolume")]
        public int? TotalVolume { get; set; }

        [JsonProperty("quoteTimeInLong")]
        public long QuoteTimeInLong { get; set; }

        [JsonProperty("tradeTimeInLong")]
        public long TradeTimeInLong { get; set; }

        [JsonProperty("mark")]
        public float Mark { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("marginable")]
        public bool Marginable { get; set; }

        [JsonProperty("shortable")]
        public bool Shortable { get; set; }

        [JsonProperty("volatility")]
        public double Volatility { get; set; }

        [JsonProperty("digits")]
        public int Digits { get; set; }

        [JsonProperty("52WkHigh")]
        public float? F2WkHigh { get; set; }

        [JsonProperty("52WkLow")]
        public float? F2WkLow { get; set; }

        [JsonProperty("nAV")]
        public double Nav { get; set; }

        [JsonProperty("peRatio")]
        public double PeRatio { get; set; }

        [JsonProperty("divAmount")]
        public double DivAmount { get; set; }

        [JsonProperty("divYield")]
        public double DivYield { get; set; }

        [JsonProperty("divDate")]
        public string DivDate { get; set; }

        [JsonProperty("securityStatus")]
        public string SecurityStatus { get; set; }

        [JsonProperty("regularMarketLastPrice")]
        public float? RegularMarketLastPrice { get; set; }

        [JsonProperty("regularMarketLastSize")]
        public int? RegularMarketLastSize { get; set; }

        [JsonProperty("regularMarketNetChange")]
        public float RegularMarketNetChange { get; set; }

        [JsonProperty("regularMarketTradeInLong")]
        public long RegularMarketTradeTimeInLong { get; set; }

        [JsonProperty("netPercentChangeInDouble")]
        public double NetPercentChangeInDouble { get; set; }

        [JsonProperty("markChangeInDouble")]
        public double MarkChangeInDouble { get; set; }

        [JsonProperty("markPercentChangeInDouble")]
        public double MarkPercentChangeInDouble { get; set; }

        [JsonProperty("delayed")]
        public bool Delayed { get; set; }
    }
}