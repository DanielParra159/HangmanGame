﻿using System.Threading.Tasks;

namespace Infrastructure.Web
{
    public class Request
    {
    }

    public class Response
    {
    }

    public interface APIRest
    {
        Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response;
    }
}