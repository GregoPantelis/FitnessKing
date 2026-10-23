using FitnessKing.Application.DataObjects;
using FitnessKing.Application.Interfaces.Persistance;
using FitnessKing.Application.Interfaces.Services;
using FitnessKing.Infrastructure.Authentication.Services;
using FitnessKing.Infrastructure.Configuration.Apis;
using FitnessKing.Infrastructure.Configuration.DbConnections;
using FitnessKing.Infrastructure.Configuration.Smtp;
using FitnessKing.Infrastructure.ExternalApis.ApiClients;
using FitnessKing.Infrastructure.Persistance.DbContexts;
using FitnessKing.Infrastructure.Persistance.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FitnessKing.Infrastructure.DependencyInjection
{
    public static class InfrastructureCollectionExtensions
    {
        public static IServiceCollection AddBlockcypherInfrastructure(this IServiceCollection services, IConfiguration configuration, string contentRootPath)
        {
            services.AddConnectionStrings(configuration);
            services.AddDatabaseContexts(contentRootPath);
            services.AddUnitOfWork();
            services.AddExternalApiSettings(configuration);
            services.AddInfrastructureServices();
            services.AddSmtpSettings(configuration);
            return services;
        }

        private static IServiceCollection AddConnectionStrings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptionsWithValidateOnStart<ConnectionStringOptions>()
                .Bind(configuration.GetSection(ConnectionStringOptions.SectionName))
                .ValidateDataAnnotations()
                .Validate(options => options.ConnectionStrings.Any(x =>
                    string.Equals(x.DatabaseName, ConfigurationConstants.ConnectionStringNames.DefaultDb, StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrEmpty(x.Value)),
                $"{ConfigurationConstants.ConnectionStringNames.DefaultDb} connection string is not configured.");

            return services;
        }

        private static IServiceCollection AddDatabaseContexts(this IServiceCollection services, string contentRootPath)
        {
            #region UserAuthDbContext

            services.AddDbContext<UserAuthDbContext>((provider, options) =>
            {
                ConnectionStringOptions settings = provider
                    .GetRequiredService<IOptions<ConnectionStringOptions>>()
                    .Value;

                ConnectionString connectionString = settings.ConnectionStrings.SingleOrDefault(x =>
                    string.Equals(
                        x.DatabaseName,
                        ConfigurationConstants.ConnectionStringNames.FitnessKingDb,
                        StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrWhiteSpace(connectionString?.Value))
                {
                    throw new InvalidOperationException(
                        $"{ConfigurationConstants.ConnectionStringNames.FitnessKingDb} connection string is not configured.");
                }

                options.UseNpgsql(connectionString.Value);
            });

            #endregion

            // Any other DbContexts can be added here in a similar manner


            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUserAuthUnitOfWork, UserAuthUnitOfWork>();
            return services;
        }

        private static IServiceCollection AddExternalApiSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptionsWithValidateOnStart<ExternalApiOptions>()
                .Bind(configuration.GetSection(ExternalApiOptions.SectionName))
                .ValidateDataAnnotations();

            services.AddHttpClient();
            services.AddScoped<IApiClientService, ApiClientService>();

            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            return services;
        }

        private static IServiceCollection AddSmtpSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptionsWithValidateOnStart<SmtpSettingsOptions>()
                .Bind(configuration.GetSection(SmtpSettingsOptions.SectionName))
                .ValidateDataAnnotations();

            return services;
        }
    }
}
