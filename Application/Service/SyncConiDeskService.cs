using Application.Model;
using Contract.DAOModel;
using Contract.Service;
using Share.Utiity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoindeskApi.Application.Service
{
    public class SyncConiDeskService : ISyncCoinDeskService
    {
        private readonly IRequestMethod requestMethod;
        public SyncConiDeskService(IRequestMethod requestMethod)
        {
            this.requestMethod = requestMethod;
        }
        private const string url = "https://api.coindesk.com/v1/bpi/currentprice.json";

        /// <summary>
        /// 發送請求並取得CoinDesk結果
        /// </summary>
        /// <returns></returns>
        public CoinDeskApiResponse SendAndGetCoinDeskResult()
        {
            bool isFail = false;
            var coinDeskData = new CoinDeskApiResponse();
            try
            {
                var response = requestMethod.SendGet(url, new Dictionary<string, string>());

                if (!response.IsSuccessStatusCode)
                {
                    isFail = true;
                }
                var content = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(content))
                {
                    isFail = true;
                }
                coinDeskData = JsonSerializer.Deserialize<CoinDeskApiResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (coinDeskData == null || coinDeskData.Bpi == null || coinDeskData.Bpi.USD == null)
                {
                    isFail = true;
                }
            }
            catch
            {
                isFail = true;
            }
            if (isFail)
            {
                // TODO 寫入請求失敗LOG
                // 改取Json檔
                coinDeskData = ReadLocalFallbackJson();
            }
            if (coinDeskData == null) throw new Exception("無法讀取資料");
            return coinDeskData;
        }
        /// <summary>
        /// 讀取Json檔
        /// </summary>
        /// <returns></returns>
        private CoinDeskApiResponse ReadLocalFallbackJson()
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Path.Combine(basePath, "Resource", "data.json");

                if (!File.Exists(filePath))
                {
                    // TODO 另開APIException繼承Exception，於filter層寫入錯誤LOG
                    throw new Exception("[Error] 找不到本地備援檔案: Resource/data.json");
                }
                var jsonString = File.ReadAllText(filePath);

                var retData = JsonSerializer.Deserialize<CoinDeskApiResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (retData == null)
                {
                    throw new Exception("[Error] 找不到本地備援檔案: Resource/data.json");
                }
                return retData;
            }
            catch 
            { 
                throw ;
            }
        }
    }
}
