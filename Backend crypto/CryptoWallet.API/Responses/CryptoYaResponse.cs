
using System.Text.Json.Serialization;

namespace CryptoWallet.API.Responses
{
    // ESTA CLASE SIRVE PARA DESERIALIZAR LA RESPUESTA DE LA API DE CRIPTOYA CUANDO SE CONSULTA EL PRECIO DE UNA CRIPTOMONEDA
    //ESTO ES LO QUE DEVUELVE LA API DE CRIPTOYA CUANDO SE CONSULTA EL PRECIO DE UNA CRIPTOMONEDA
    public class CryptoYaResponse
    {
        [JsonPropertyName("ask")] // El precio de compra sin comisiones
        public double Ask { get; set; }
        [JsonPropertyName("totalAsk")] // El precio de compra CON comisiones (lo que realmente pagas al comprar)
        public double TotalAsk { get; set; }
        [JsonPropertyName("bid")] // El precio de venta sin comisiones
        public double Bid { get; set; }
        [JsonPropertyName("totalBid")] // El precio de venta CON comisiones (lo que realmente recibis al vender)
        public double TotalBid { get; set; }

    }
}
