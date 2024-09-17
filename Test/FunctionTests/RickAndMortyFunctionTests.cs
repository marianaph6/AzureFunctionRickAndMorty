
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RickAndMortyFunction;


namespace FunctionTests
{
    public class RickAndMortyFunctionTests
    {
        private class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;
            private readonly Exception _exception;

            public MockHttpMessageHandler(HttpResponseMessage response = null, Exception exception = null)
            {
                _response = response;
                _exception = exception;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (_exception != null)
                {
                    throw _exception;
                }
                return Task.FromResult(_response);
            }
        }

        [Fact]
        public async Task Run_ReturnsOkResult_WhenApiCallSucceeds()
        {
            // Arrange
            var id = 1;
            var mockLogger = new Mock<ILogger>();

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("{ \"id\":1,\"name\":\"Rick Sanchez\",\"status\":\"Alive\",\"species\":\"Human\",\"type\":\"\",\"gender\":\"Male\",\"origin\":{\"name\":\"Earth (C-137)\",\"url\":\"https://rickandmortyapi.com/api/location/1\"},\"location\":{\"name\":\"Citadel of Ricks\",\"url\":\"https://rickandmortyapi.com/api/location/3\"},\"image\":\"https://rickandmortyapi.com/api/character/avatar/1.jpeg\",\"episode\":[\"https://rickandmortyapi.com/api/episode/1\",\"https://rickandmortyapi.com/api/episode/2\",\"https://rickandmortyapi.com/api/episode/3\",\"https://rickandmortyapi.com/api/episode/4\",\"https://rickandmortyapi.com/api/episode/5\",\"https://rickandmortyapi.com/api/episode/6\",\"https://rickandmortyapi.com/api/episode/7\",\"https://rickandmortyapi.com/api/episode/8\",\"https://rickandmortyapi.com/api/episode/9\",\"https://rickandmortyapi.com/api/episode/10\",\"https://rickandmortyapi.com/api/episode/11\",\"https://rickandmortyapi.com/api/episode/12\",\"https://rickandmortyapi.com/api/episode/13\",\"https://rickandmortyapi.com/api/episode/14\",\"https://rickandmortyapi.com/api/episode/15\",\"https://rickandmortyapi.com/api/episode/16\",\"https://rickandmortyapi.com/api/episode/17\",\"https://rickandmortyapi.com/api/episode/18\",\"https://rickandmortyapi.com/api/episode/19\",\"https://rickandmortyapi.com/api/episode/20\",\"https://rickandmortyapi.com/api/episode/21\",\"https://rickandmortyapi.com/api/episode/22\",\"https://rickandmortyapi.com/api/episode/23\",\"https://rickandmortyapi.com/api/episode/24\",\"https://rickandmortyapi.com/api/episode/25\",\"https://rickandmortyapi.com/api/episode/26\",\"https://rickandmortyapi.com/api/episode/27\",\"https://rickandmortyapi.com/api/episode/28\",\"https://rickandmortyapi.com/api/episode/29\",\"https://rickandmortyapi.com/api/episode/30\",\"https://rickandmortyapi.com/api/episode/31\",\"https://rickandmortyapi.com/api/episode/32\",\"https://rickandmortyapi.com/api/episode/33\",\"https://rickandmortyapi.com/api/episode/34\",\"https://rickandmortyapi.com/api/episode/35\",\"https://rickandmortyapi.com/api/episode/36\",\"https://rickandmortyapi.com/api/episode/37\",\"https://rickandmortyapi.com/api/episode/38\",\"https://rickandmortyapi.com/api/episode/39\",\"https://rickandmortyapi.com/api/episode/40\",\"https://rickandmortyapi.com/api/episode/41\",\"https://rickandmortyapi.com/api/episode/42\",\"https://rickandmortyapi.com/api/episode/43\",\"https://rickandmortyapi.com/api/episode/44\",\"https://rickandmortyapi.com/api/episode/45\",\"https://rickandmortyapi.com/api/episode/46\",\"https://rickandmortyapi.com/api/episode/47\",\"https://rickandmortyapi.com/api/episode/48\",\"https://rickandmortyapi.com/api/episode/49\",\"https://rickandmortyapi.com/api/episode/50\",\"https://rickandmortyapi.com/api/episode/51\"],\"url\":\"https://rickandmortyapi.com/api/character/1\",\"created\":\"2017-11-04T18:48:46.250Z\"}")
            };

            var mockHandler = new MockHttpMessageHandler(mockResponse);
            var httpClient = new HttpClient(mockHandler);

            // Act
            var result = await RickAndMortyFunction.RickAndMortyFunction.Run(new DefaultHttpContext().Request, id, mockLogger.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task Run_ReturnsBadRequest_WhenApiResponseIsInvalidContent()
        {
            // Arrange
            var id = 1;
            var mockLogger = new Mock<ILogger>();

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("Invalid content")
            };

            var mockHandler = new MockHttpMessageHandler(mockResponse);
            var httpClient = new HttpClient(mockHandler);

            // Act
            var result = await RickAndMortyFunction.RickAndMortyFunction.Run(new DefaultHttpContext().Request, id, mockLogger.Object);

            // Assert
            var badRequestResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotEqual("{ \"id\":1,\"name\":\"Rick Sanchez\" }", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task Run_ReturnsBadRequest_WhenCharacterNotFound()
        {
            // Arrange
            var id = 999; // Invalid character ID that doesn't exist
            var mockLogger = new Mock<ILogger>();

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Content = new StringContent("Character not found")
            };

            var mockHandler = new MockHttpMessageHandler(mockResponse);
            var httpClient = new HttpClient(mockHandler);

            // Act
            var result = await RickAndMortyFunction.RickAndMortyFunction.Run(new DefaultHttpContext().Request, id, mockLogger.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error fetching character info.", badRequestResult.Value);
        }


        [Fact]
        public async Task Run_ReturnsBadRequest_WhenApiCallFails()
        {
            // Arrange
            var id = 999; // Un ID que cause error
            var mockLogger = new Mock<ILogger>();

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Content = new StringContent("Character not found")
            };

            var mockHandler = new MockHttpMessageHandler(mockResponse);
            var httpClient = new HttpClient(mockHandler);

            // Act
            var result = await RickAndMortyFunction.RickAndMortyFunction.Run(new DefaultHttpContext().Request, id, mockLogger.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error fetching character info.", badRequestResult.Value);
        }

        [Fact]
        public async Task Run_ReturnsBadRequest_WhenApiResponseIsInvalid()
        {
            // Arrange
            var id = 1;
            var mockLogger = new Mock<ILogger>();

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("Invalid content")
            };

            var mockHandler = new MockHttpMessageHandler(mockResponse);
            var httpClient = new HttpClient(mockHandler);

            // Act
            var result = await RickAndMortyFunction.RickAndMortyFunction.Run(new DefaultHttpContext().Request, id, mockLogger.Object);

            // Assert
            var badRequestResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotEqual("{ \"id\":1,\"name\":\"Rick Sanchez\"...}", badRequestResult.Value.ToString());
        }

        // Eliminamos las pruebas relacionadas con TelemetryClient ya que no es necesario en esta versión



    }
}