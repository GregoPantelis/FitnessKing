using Microsoft.EntityFrameworkCore;

namespace FitnessKing.Infrastructure.Persistance.DbContexts
{
    public class CustomContext : DbContext
    {
        public CustomContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}