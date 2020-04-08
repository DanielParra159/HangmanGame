using System;
using System.Collections.Generic;
using Domain.Services.ServiceLocator;

namespace Application.Services.ServiceLocator
{
    public class ServiceLocatorImpl : ServiceLocatorService, ServiceLocatorRegisterService
    {
        private readonly Dictionary<Type, object> _services;

        public ServiceLocatorImpl()
        {
            _services = new Dictionary<Type, object>();
        }

        public T Get<T>()
        {
            var type = typeof(T);
            T result;
            try
            {
                result = (T) _services[type];
            }
            catch (KeyNotFoundException ex)
            {
                // TODO: LOG
                throw ex;
            }

            return result;
        }

        public void Register<T>(T service)
        {
            var type = typeof(T);
            try
            {
                _services.Add(type, service);
            }
            catch (ArgumentException ex)
            {
                // TODO: LOG
                throw ex;
            }
        }
    }
}