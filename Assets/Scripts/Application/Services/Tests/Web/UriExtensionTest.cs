using System;
using System.Diagnostics.CodeAnalysis;
using Application.Services.Web;
using NUnit.Framework;

namespace Application.Services.Tests.Web
{
    public class UriExtensionTest
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private class TestClass
        {
            public string param1;
            public string param2;
        }

        [Test]
        public void WhenCallToUriExtension_ReturnTheCorrectUrl()
        {
            var url = new Uri("http://www.domain.com/test");
            var result = url.ExtendQuery(new TestClass {param1 = "val1", param2 = "val2"});
            Assert.That(result, Is.EqualTo(new Uri("http://www.domain.com/test?param1=val1&param2=val2")));
        }
    }
}
