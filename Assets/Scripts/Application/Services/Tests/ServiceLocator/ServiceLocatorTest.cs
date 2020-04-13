using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Application.Services.Tests.ServiceLocator
{
    public class ServiceLocatorTest
    {
        private interface ITestService
        {
        }

        private class TestService1 : ITestService
        {
        }

        [Test]
        public void WhenRegisterAServiceAndTryToGetIt_ReturnTheRegisteredService()
        {
            var serviceLocator = new Services.ServiceLocator.ServiceLocator();
            var service = new TestService1();
            serviceLocator.Register<ITestService>(service);

            Assert.AreEqual(service, serviceLocator.Get<ITestService>());
        }

        [Test]
        public void WhenTryToRegisterMultipleTimesTheSameService_ThrowAnException()
        {
            var serviceLocator = new Services.ServiceLocator.ServiceLocator();
            var service = new TestService1();
            serviceLocator.Register<ITestService>(service);

            Assert.Throws<ArgumentException>(() => serviceLocator.Register<ITestService>(service));
        }
        
        [Test]
        public void WhenTryToGetUnregisterService_ThrowAnException()
        {
            var serviceLocator = new Services.ServiceLocator.ServiceLocator();

            Assert.Throws<KeyNotFoundException>(() => serviceLocator.Get<ITestService>());
        }
    }
}
