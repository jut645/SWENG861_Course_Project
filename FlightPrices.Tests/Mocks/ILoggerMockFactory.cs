using FlightPrices.WebApp.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPrices.Tests.Mocks
{
    public class ILoggerMockFactory
    {
        public static ILogger<T> Build<T>() where T : class
        {
            var mock = new Mock<ILogger<T>>();
            return mock.Object;
        }
    }
}
