
using System.Text.Json.Serialization;

namespace CryptoWallet.API.Responses
{
    
    public class CryptoYaResponse
    {
        [JsonPropertyName("ask")] 
        public double Ask { get; set; }
        [JsonPropertyName("totalAsk")] 
        public double TotalAsk { get; set; }
        [JsonPropertyName("bid")] 
        public double Bid { get; set; }
        [JsonPropertyName("totalBid")] 
        public double TotalBid { get; set; }

    }
}
