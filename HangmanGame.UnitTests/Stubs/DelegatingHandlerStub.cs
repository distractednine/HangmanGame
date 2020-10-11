using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HangmanGame.UnitTests.Stubs
{
    public class DelegatingHandlerStub : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

        public DelegatingHandlerStub(HttpStatusCode statusCode, string stringContent)
        {
            var responseContent = new StringContent(stringContent);
            var responseMessage = new HttpResponseMessage(statusCode)
            {
                Content = responseContent
            };

            _handlerFunc = (request, cancellationToken) => Task.FromResult(responseMessage);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handlerFunc(request, cancellationToken);
        }
    }
}
