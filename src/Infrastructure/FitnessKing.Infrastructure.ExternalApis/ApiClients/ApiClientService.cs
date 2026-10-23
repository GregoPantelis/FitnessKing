using FitnessKing.Application.DataObjects.Enums;
using FitnessKing.Application.Interfaces.Services;
using FitnessKing.Domain.Types;
using FitnessKing.Infrastructure.ExternalApis.Helpers;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace FitnessKing.Infrastructure.ExternalApis.ApiClients
{
    public sealed class ApiClientService : IApiClientService
    {
        private readonly ILogger<ApiClientService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiClientService(IHttpClientFactory httpClientFactory, ILogger<ApiClientService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<OperationResult<TResponse>> GetAsync<TResponse>(ApiType apiType, string requestUri, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        string message = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"{apiType}: API request failed with status code: {response.StatusCode}, reason: {response.ReasonPhrase}. Message: {message}");
                        return response.MapHttpResponseCodeToOperationResult<TResponse>();
                    }

                    TResponse responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);

                    if (responseContent == null)
                    {
                        _logger.LogError($"{apiType}: API request returned null content for {requestUri}");
                        return new OperationResult<TResponse>(OperationResults.Api.ApiEmptyResponse, null);
                    }

                    return new OperationResult<TResponse>(OperationResults.Api.ApiSuccessful, responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{apiType}: An error occurred while making a GET request to {requestUri}", ex);
                return new OperationResult<TResponse>(OperationResults.Api.ApiInternalServerError, null);
            }
        }

        public async Task<OperationResult<TResponse>> PostAsync<TRequest, TResponse>(ApiType apiType, string requestUri, TRequest request, CancellationToken cancellationToken = default)
            where TResponse : class
        {
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, request, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"{apiType}: API request failed with status code: {response.StatusCode}, reason: {response.ReasonPhrase}");
                        return response.MapHttpResponseCodeToOperationResult<TResponse>();
                    }

                    TResponse responseContent = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);

                    if (responseContent == null)
                    {
                        _logger.LogError($"{apiType}: API request returned null content for {requestUri}");
                        return new OperationResult<TResponse>(OperationResults.Api.ApiEmptyResponse, null);
                    }

                    return new OperationResult<TResponse>(OperationResults.Api.ApiSuccessful, responseContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{apiType}: An error occurred while making a POST request to {requestUri}", ex);
                return new OperationResult<TResponse>(OperationResults.Api.ApiInternalServerError, null);
            }
        }
    }
}
