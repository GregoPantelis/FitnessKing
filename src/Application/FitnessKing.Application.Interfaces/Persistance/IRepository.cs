using FitnessKing.Domain.Types;
using System.Linq.Expressions;

namespace FitnessKing.Application.Interfaces.Persistance
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        Task<OperationResult<bool>> AddAsync(TEntity entity);

        Task<OperationResult<bool>> DeleteAsync(TEntity entity);

        Task<OperationResult<bool>> UpdateAsync(TEntity entity);

    }
}
