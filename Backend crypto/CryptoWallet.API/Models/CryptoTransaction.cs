using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.API.Models
{
    public class CryptoTransaction
    {
        public int Id{ get; set; }
        public int CryptoId { get; set; }
        public int TransactionActionId { get; set; }
        [Column(TypeName = "decimal(18,8)")] //Acepta hasta 8 decimales
        public decimal CryptoAmount { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Money { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        public Crypto? Crypto { get; set; }
        public TransactionAction? TransactionAction { get; set; }
    }
}
