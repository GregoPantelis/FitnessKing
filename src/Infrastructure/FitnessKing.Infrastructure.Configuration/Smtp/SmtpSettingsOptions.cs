using Microsoft.Extensions.Options;

namespace FitnessKing.Infrastructure.Configuration.Smtp
{
    public sealed class SmtpSettingsOptions
    {
        public const string SectionName = "SmtpSettings";

        [ValidateEnumeratedItems]
        public IEnumerable<SmtpSetting> Settings { get; set; }
    }
}
