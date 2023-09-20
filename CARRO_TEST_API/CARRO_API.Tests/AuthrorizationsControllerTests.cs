using System;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using CARRO_API.Controllers;
using CARRO_API.Models;
using CARRO_API.Services.Interface;
using CARRO_API.Common.Utilities;

namespace CARRO_API.Tests
{
    [TestFixture]
    public class AuthrorizationsControllerTests
    {
        private AuthrorizationsController _controller;
        private Mock<IAuthrorizationsService> _authrorizationsServiceMock;

        [SetUp]
        public void Setup()
        {
            _authrorizationsServiceMock = new Mock<IAuthrorizationsService>();
            var loggerMock = new Mock<ILogger<AuthrorizationsController>>();

            _controller = new AuthrorizationsController(loggerMock.Object, _authrorizationsServiceMock.Object);
        }

        [Test]
        public void Sigin_WithValidRequest_ReturnsSuccessResponse()
        {
            string FirstName = GenerateRandomFirstName();
            string Password = GenerateRandomPassword(10);
            // Arrange
            var validRequest = new SiginQuery
            {
                Email = $"{FirstName}@example.com",
                Password = Password
            };

            var expectedResult = new SiginModel
            {
                access_token = "valid_token",
                token_type = "Bearer",
                expires_in = 3600
            };

            _authrorizationsServiceMock.Setup(x => x.Sigin(validRequest)).Returns(expectedResult);

            // Act
            var result = _controller.Sigin(validRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Data);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNull(result.Error);
        }

        [Test]
        public void Sigin_WithInvalidRequest_ReturnsErrorResponse()
        {
            // Arrange
            var invalidRequest = new SiginQuery
            {
                // Missing email and password, which is invalid
            };

            _authrorizationsServiceMock.Setup(x => x.Sigin(invalidRequest)).Throws<Exception>();

            // Act
            var result = _controller.Sigin(invalidRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);
        }

        [Test]
        public void Sigup_WithValidRequest_ReturnsSuccessResponse()
        {
            string Password = GenerateRandomPassword(10);
            string FirstName = GenerateRandomFirstName();
            string LastName = GenerateRandomLastName();
            // Arrange
            var validRequest = new SigupQuery
            {
                Email = $"{FirstName}@example.com",
                Password = Password,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = "1234567890",
                Brithdate = DateTime.Now.AddYears(-30)
            };

            var expectedResult = new SiginModel
            {
                access_token = "valid_token",
                token_type = "Bearer",
                expires_in = 3600
            };

            _authrorizationsServiceMock.Setup(x => x.Sigup(validRequest)).Returns(expectedResult);

            // Act
            var result = _controller.Sigup(validRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Data);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNull(result.Error);
        }

        [Test]
        public void Sigup_WithInvalidRequest_ReturnsErrorResponse()
        {
            // Arrange
            var invalidRequest = new SigupQuery
            {
                // Missing email and password, which is invalid
            };

            _authrorizationsServiceMock.Setup(x => x.Sigup(invalidRequest)).Throws<Exception>();

            // Act
            var result = _controller.Sigup(invalidRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);
        }

        private static string GenerateRandomFirstName()
        {
            string[] firstNames = { "John", "Alice", "Michael", "Emily", "David", "Sophia", "Robert", "Olivia", "William", "Emma" };
            Random random = new Random();
            return firstNames[random.Next(0, firstNames.Length)];
        }

        private static string GenerateRandomLastName()
        {
            string[] lastNames = { "Smith", "Johnson", "Brown", "Davis", "Wilson", "Lee", "Miller", "Moore", "Taylor", "Clark" };
            Random random = new Random();
            return lastNames[random.Next(0, lastNames.Length)];
        }
        private static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
