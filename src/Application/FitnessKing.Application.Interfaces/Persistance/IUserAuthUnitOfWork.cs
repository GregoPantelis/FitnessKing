using FitnessKing.Application.DataObjects;
using FitnessKing.Domain.Types;

namespace FitnessKing.Application.Interfaces.Persistance
{
    public interface IUserAuthUnitOfWork : IUnitOfWork
    {
        Task<OperationResult<UserAuthData>> GetUserAsync(UserDataFilter filter, CancellationToken cancellationToken = default);

    }
}
