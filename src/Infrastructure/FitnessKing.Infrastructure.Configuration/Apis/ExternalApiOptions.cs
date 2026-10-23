using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace FitnessKing.Infrastructure.Configuration.Apis
{
    public sealed class ExternalApiOptions
    {
        public const string SectionName = "ExternalApis";

        [Required]
        [ValidateEnumeratedItems]
        public IEnumerable<ExternalApi> Endpoints { get; set; }
    }
}
