using RestSharp;

namespace Framework.Core.ApiClients
{
    public class ApiRequestBuilder
    {
        private readonly RestRequest _request;

        public ApiRequestBuilder(string endpoint, Method method)
        {
            _request = new RestRequest(endpoint, method);
        }

        public ApiRequestBuilder AddHeader(string key, string value)
        {
            _request.AddHeader(key, value);
            return this;
        }

        public ApiRequestBuilder AddParameter(string key, object value, ParameterType parameterType = ParameterType.QueryString)
        {
            _request.AddParameter(key, value, parameterType);
            return this;
        }

        public ApiRequestBuilder AddJsonBody(object body)
        {
            _request.AddJsonBody(body);
            return this;
        }

        public RestRequest Build()
        {
            return _request;
        }
    }
}
