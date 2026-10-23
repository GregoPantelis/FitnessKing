using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace FitnessKing.Infrastructure.Configuration.DbConnections
{
    public sealed class ConnectionStringOptions
    {
        public const string SectionName = "DatabaseConnections";

        [Required]
        [ValidateEnumeratedItems]
        public IEnumerable<ConnectionString> ConnectionStrings { get; set; }

    }
}
