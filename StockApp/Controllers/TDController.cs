using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using RestSharp;
using StockApp.Interfaces;
using StockApp.Models;

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
        public string Refresh = "";

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

        public Task GetQuoteAsync(string sym)
        {
            throw new NotImplementedException();
        }

        public Task TDResponseAsync()
        {
            throw new NotImplementedException();
        }

        public object DeserializeQuote(string jsonData, string sym)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, TDQuoteModel> DeserializeQuotesToJson(string jsonData)
        {
            throw new NotImplementedException();
        }

        public void TDQuoteAssign()
        {
            throw new NotImplementedException();
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
