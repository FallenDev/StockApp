using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using RestSharp;
using StockApp.Interfaces;
using StockApp.Models;
using StockApp.Services;

namespace StockApp.Controllers
{
    public class TDController : ITDController
    {
        #region Auth Variables

        private const string BaseAdd = "https://api.tdameritrade.com";
        private const string Token = "/v1/oauth2/token";
        private string _apiKey = "";
        private string _authToken = "";
        private string _requestUri;
        private const string Refresh = "KD+3zqT9YSY0zX4/C4SDeHe0apnHcH4x+oKiab4F+fma/5bdKHd2dUEwc8P521lAGt90BE6/0orE4oRAalkDcn/CENkXKriKlaWVNVqbTrNX98qgwxcSh5XCciuaJ7Uz6JkwWG15z/ALIF0I5dUkywcg7VbVPJ/xRrsgC6MoTsQdlFCMwMKqvd8ixikpajiJx/LiKbLjF9NEWGLEwhgq/nr8Zwpa3EXjEnhS3sBaWK9im14sA1IDmXMNOutjC+qrO4x0PSVVrDJNtfrFwJoveR3Vw9JAK8Txyr9WDYo7gXQ2S7ZWu98mNfU5YJGs+qGHGUBZUnjcE42wjR/E7t1Lf5QRXZwI0v0/7fbwGdMBEXaRhfWHmVPfAl2ZQ3U2FfF/6wdchsUFUEnFv54jOYkUvvpVKkpgoH5DNqoAGyitgyviM4Y36W2hDiyZgko100MQuG4LYrgoVi/JHHvlDToiDxBz6Cnsf60WZ9KDyV1CDNDDX6CHqF8wXWIozCkruegQhWzxemrBjoDnZofTiTkGKFi0tVbfmEIphwV0Hk8W/vzb7MlNlm+v+M0sSyo+FWu6yVfrwy//epngrDAK9q1Bx3ZwEGmZ88wULXep9ls814K6RFEv/W8DKr3UMWfo/DYrcmhJ0gBvp6pRhqInePreBpZRSGmcq51nHBPBspFyfy6eezIdz59f9KB5usCluIiM4x64yqBaQtLduJC/U9rrzMv2dZ1bv6VDy8G/3GXhQuCqbpafFe8+w4TDkBQ5DTdzVQN30umsttuBP0V5RDS4lHtTmEeQ9dg6HIArdrntt+JtwbKWfbrRZMaS6W2cIhagzCI77JzgPLoYI+gs6Uux2nYZXrDwgk+uHXQmyDq34IoDBqs62aY360uVismR88gMHKux2sBchPw=212FD3x19z9sWBHDJACbC00B75E";
        #endregion

        #region Quote Variables

        private string _jsonData;
        private Dictionary<string, TDQuoteModel> _quoteDictionary;
        private string _symbol;
        private float? _bidPrice;
        private float? _askPrice;
        private float? _lastPrice;
        private float? _openPrice;
        private float? _highPrice;
        private float? _lowPrice;
        private float? _closePrice;
        private double? _netChange;
        private int? _volume;
        private string _exchangeName;
        private bool _margin;
        private bool _short;
        private double _peRatio;
        private string _secStatus;
        private double _volatility;
        private string _divDate;
        private bool _delayed;

        #endregion

        public void Initiate()
        {
            FetchAuth();
            RefreshAuth();
        }

        public async Task<object> GetQuoteAsync(string sym)
        {
            Analytics.TrackEvent("TD API Request");
            if (string.IsNullOrEmpty(sym)) return null;

            _requestUri = $"/v1/marketdata/{sym}/quotes?apikey={_apiKey}";

            await TDResponseAsync().ConfigureAwait(false);
            return _authToken == null ? null : DeserializeQuote(_jsonData, sym);
        }

        public async Task TDResponseAsync()
        {
            var client = new RestClient(BaseAdd + _requestUri);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_authToken}");

            var response = await client.ExecuteAsync(request).ConfigureAwait(false);
            _jsonData = response.Content;
        }

        public object DeserializeQuote(string jsonData, string sym)
        {
            var dict = DeserializeQuotesToJson(jsonData);
            _symbol = dict[sym].Symbol;
            _netChange = dict[sym].NetChange;
            _lastPrice = dict[sym].LastPrice;
            _bidPrice = dict[sym].BidPrice;
            _askPrice = dict[sym].AskPrice;
            _margin = dict[sym].Marginable;
            _short = dict[sym].Shortable;
            _volatility = dict[sym].Volatility;
            _peRatio = dict[sym].PeRatio;
            _secStatus = dict[sym].SecurityStatus;
            _divDate = dict[sym].DivDate;
            _volume = dict[sym].TotalVolume;
            _openPrice = dict[sym].OpenPrice;
            _highPrice = dict[sym].HighPrice;
            _lowPrice = dict[sym].LowPrice;
            _closePrice = dict[sym].ClosePrice;
            _exchangeName = dict[sym].ExchangeName;
            _delayed = dict[sym].Delayed;
            return dict;
        }

        public Dictionary<string, TDQuoteModel> DeserializeQuotesToJson(string jsonData)
        {
            _quoteDictionary = JsonSerializer.Deserialize<Dictionary<string, TDQuoteModel>>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new NullableDateTimeOffsetService()
                }
            });

            return _quoteDictionary;
        }

        public void TDQuoteAssign()
        {
            MainWindow.TdStocks.Add(new TDQuoteModel { Symbol = _symbol, NetChange = _netChange, LastPrice = _lastPrice, BidPrice = _bidPrice, AskPrice = _askPrice, TotalVolume = _volume, Volatility = _volatility, PeRatio = _peRatio });
            MainWindow.TdDetails.Add(new TDQuoteModel { OpenPrice = _openPrice, HighPrice = _highPrice, LowPrice = _lowPrice, ClosePrice = _closePrice, ExchangeName = _exchangeName, Marginable = _margin, Shortable = _short, Delayed = _delayed, SecurityStatus = _secStatus, DivDate = _divDate });
        }

        public void FetchAuth()
        {
            if (!string.IsNullOrEmpty(_apiKey)) return;

            var lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Content\TDAuth.txt"));
            if (lines.Length > 0)
                _apiKey = System.Web.HttpUtility.UrlEncode(lines[0]);
        }

        public void RefreshAuth()
        {
            try
            {
                var client = new RestClient(BaseAdd + Token);
                var request = new RestRequest(Method.POST);
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", Refresh);
                request.AddParameter("client_id", _apiKey);
                request.AddParameter("redirect_uri", "http://127.0.0.1");

                var response = client.Execute(request);
                var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TDAuthModel>(response.Content);

                if (jsonResponse == null) return;

                _authToken = jsonResponse.AccessToken;
                _ = jsonResponse.Scope;
                _ = jsonResponse.ExpiresIn;
                _ = jsonResponse.TokenType;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (_authToken != null)
                    Analytics.TrackEvent("Auth Token Granted");
            }
        }
    }
}
