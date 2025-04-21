using RestSharp;
using Framework.Tests.Configuration;
using Framework.Core.Utilities;

namespace Framework.Core.ApiClients
{
    public abstract class BaseApiClient
    {
        private readonly RestClient _restClient;

        protected BaseApiClient()
        {
            string baseUrl = ConfigurationManager.Instance.Settings.Api.BaseUrl;
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("API Base URL is not configured!");
            }

            _restClient = new RestClient(baseUrl);

            Logger.LogInfo($"RestClient initialized with base URL: {baseUrl}");
        }

        //refactored to use the ApiRequestBuilder 
        protected RestRequest CreateRequest(string endpoint, Method method)
        {
            Logger.LogInfo($"Creating {method} request to endpoint: {endpoint}");
            var builder = new ApiRequestBuilder(endpoint, method);
            return builder.Build(); 
        }

        protected async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            var response = await _restClient.ExecuteAsync(request);
            return response;
        }

        //added constraints to be a class and have a parameterless constructor
        //to exclude non-class types (e.g. primitive types) and ensures that T can be instantiated.
        protected async Task<T> ExecuteWithDeserializationAsync<T>(RestRequest request) 
            where T : class, new()
        {
            var response = await _restClient.ExecuteAsync<T>(request);
            return response.Data;
        }
    }
}
