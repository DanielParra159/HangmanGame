using System.Threading.Tasks;

namespace Domain.Services.Web
{
    public class Request
    {
    }

    public class Response
    {
    }

    public interface RestClient
    {
        Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response;

        Task<TResponse> Get<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response;

        Task<Response> PutWithParametersOnUrl<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response;
    }
}