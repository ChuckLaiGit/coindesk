using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contract.DAOModel
{
    public class CoinDeskApiResponse
    {
        [JsonPropertyName("time")]
        public CoinDeskTime? Time { get; set; }

        [JsonPropertyName("disclaimer")]
        public string? Disclaimer { get; set; }

        [JsonPropertyName("chartName")]
        public string? ChartName { get; set; }

        [JsonPropertyName("bpi")]
        public CoinDeskBpi? Bpi { get; set; }
    }

    public class CoinDeskTime
    {
        [JsonPropertyName("updated")]
        public string? Updated { get; set; }

        [JsonPropertyName("updatedISO")]
        public string? UpdatedISO { get; set; }

        [JsonPropertyName("updateduk")]
        public string? UpdatedUk { get; set; }
    }

    public class CoinDeskBpi
    {
        [JsonPropertyName("USD")]
        public CurrencyInfo? USD { get; set; }

        [JsonPropertyName("GBP")]
        public CurrencyInfo? GBP { get; set; }

        [JsonPropertyName("EUR")]
        public CurrencyInfo? EUR { get; set; }
    }

    public class CurrencyInfo
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("rate")]
        public string? Rate { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("rate_float")]
        public decimal RateFloat { get; set; }
    }
}
