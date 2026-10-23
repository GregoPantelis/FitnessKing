using System.ComponentModel.DataAnnotations;

namespace FitnessKing.Infrastructure.Configuration.DbConnections
{
    public sealed class ConnectionString
    {
        [Required]
        public string DatabaseName { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
