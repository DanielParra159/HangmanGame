using System;
using System.Collections.Generic;
using Application.Services.ServiceLocator;
using NUnit.Framework;

namespace Application.Services.Tests.ServiceLocator
{
    public class ServiceLocatorImplTest
    {
        public interface TestService
        {
        }

        public class TestServiceImpl : TestService
        {
        }

        [Test]
        public void WhenRegisterAServiceAndTryToGetIt_ReturnTheRegisteredService()
        {
            var serviceLocatorImpl = new ServiceLocatorImpl();
            var service = new TestServiceImpl();
            serviceLocatorImpl.Register<TestService>(service);

            Assert.AreEqual(service, serviceLocatorImpl.Get<TestService>());
        }

        [Test]
        public void WhenTryToRegisterMultipleTimesTheSameService_ThrowAnException()
        {
            var serviceLocatorImpl = new ServiceLocatorImpl();
            var service = new TestServiceImpl();
            serviceLocatorImpl.Register<TestService>(service);

            Assert.Throws<ArgumentException>(() => serviceLocatorImpl.Register<TestService>(service));
        }
        
        [Test]
        public void WhenTryToGetUnregisterService_ThrowAnException()
        {
            var serviceLocatorImpl = new ServiceLocatorImpl();

            Assert.Throws<KeyNotFoundException>(() => serviceLocatorImpl.Get<TestService>());
        }
    }
}
