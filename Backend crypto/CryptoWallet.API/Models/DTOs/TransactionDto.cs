namespace CryptoWallet.API.Models.DTOs
{
    public class TransactionDto
    {
        public string CryptoCode { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public decimal CryptoAmount { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public decimal Money { get; set; }

    }
}
