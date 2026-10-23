using FitnessKing.Application.DataObjects.Enums;
using FitnessKing.Domain.Types;

namespace FitnessKing.Application.Interfaces.Services
{
    public interface IApiClientService
    {
        Task<OperationResult<TResponse>> GetAsync<TResponse>(ApiType apiType, string requestUri, CancellationToken cancellationToken = default)
            where TResponse : class;

        Task<OperationResult<TResponse>> PostAsync<TRequest, TResponse>(ApiType apiType, string requestUri, TRequest request, CancellationToken cancellationToken = default)
            where TResponse : class;
    }
}