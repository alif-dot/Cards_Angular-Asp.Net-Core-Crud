using Microsoft.EntityFrameworkCore;
using Cards.Api.Models;

namespace Cards.Api.Data
{
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions<CardDbContext> options) : base(options) { }
        
        public DbSet<Card> Card { get; set; }
    }
}
