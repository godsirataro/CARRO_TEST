using NUnit.Framework;
using CARRO_API.Controllers;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;

namespace CARRO_API.Tests
{
    [TestFixture]
    public class WeatherForecastControllerTests
    {
        [Test]
        public void Get_ReturnsFiveWeatherForecasts()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastControllerTests>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result); // Check that the result is not null
            Assert.IsInstanceOf<IEnumerable<WeatherForecast>>(result); // Check that it's an IEnumerable of WeatherForecast
            Assert.AreEqual(5, (result as IEnumerable<WeatherForecast>).Count()); // Check that it returns 5 weather forecasts
        }
    }
}
