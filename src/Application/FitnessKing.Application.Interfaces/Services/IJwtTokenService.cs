using FitnessKing.Application.DataObjects;
using FitnessKing.Domain.Types;

namespace FitnessKing.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates jwt token inhouse with Blockcypher Issuer 
        /// </summary>
        /// <param name="userAuthData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        OperationResult<UserTokenData> GenerateInHouseJwtToken(UserAuthData userAuthData, CancellationToken cancellationToken = default);
    }
}
