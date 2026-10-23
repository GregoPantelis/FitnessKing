using FitnessKing.Application.Interfaces.Persistance;
using FitnessKing.Domain.Types;
using FitnessKing.Infrastructure.Persistance.DbContexts;
using FitnessKing.Infrastructure.Persistance.Repository;
using Microsoft.Extensions.Logging;

namespace FitnessKing.Infrastructure.Persistance.UoW
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : CustomContext
    {
        private readonly ILogger<UnitOfWork<TContext>> _logger;
        private readonly TContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(TContext context, ILogger<UnitOfWork<TContext>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<TContext, TEntity>(_context, _logger);
                _repositories[type] = repositoryInstance;
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public async Task<OperationResult<bool>> CompleteAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return new OperationResult<bool>(OperationResults.Common.Successful, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes to the database.");
                return new OperationResult<bool>(OperationResults.Common.InternalServerError, false);
            }
        }

        public void Dispose() => _context.Dispose();
    }
}
