using CryptoWallet.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CryptoWallet.API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CryptoTransaction> transactions { get; set; }
        public DbSet<Crypto> cryptos { get; set; }
        public DbSet<TransactionAction> transactionActions { get; set; }

    }
}
