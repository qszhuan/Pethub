using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;

namespace PethubTests
{
    public class TestBase
    {
        protected readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        protected readonly ServiceCollection _serviceCollection = new ServiceCollection();

        public TestBase()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(_httpMessageHandlerMock.Object));
            _serviceCollection.AddScoped(x => httpClientFactoryMock.Object);
        }
        public IServiceProvider GetServiceProvider()
        {
            return _serviceCollection.BuildServiceProvider();
        }

        protected void SetupHttpData(string data)
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(data)
                });
        }
    }
}