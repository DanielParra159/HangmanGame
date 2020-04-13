using System.Threading.Tasks;

namespace Domain.Services.Web
{
    public interface IRestClient
    {
        Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response;

        Task<TResponse> Get<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response;

        Task<TResponse> PutWithParametersOnUrl<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response;
    }
}
