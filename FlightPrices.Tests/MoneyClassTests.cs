using System;
using Xunit;

using FlightPrices.Skyscanner.WebAPI.Models;

namespace FlightPrices.Tests
{
    /// <summary>
    /// Tests for the Money class.
    /// </summary>
    public class MoneyClassTests
    {
        /// <summary>
        /// Tests the Money class comparisons for USD currency money.
        /// </summary>
        [Fact]
        public void UnitedStateOfAmericaDollarComparisonTest()
        {
            // Instantiate arbitrary test objects.
            var moneyOne = new Money(1.75m, CurrencyType.UnitedStatesOfAmericaDollar);
            var moneyTwo = new Money(3.21m, CurrencyType.UnitedStatesOfAmericaDollar);
            
            // Test comparison operators
            Assert.True(moneyOne < moneyTwo);
            Assert.False(moneyOne == moneyTwo);
            Assert.True(moneyOne != moneyTwo);
            Assert.True(moneyOne <= moneyTwo);
            Assert.False(moneyOne >= moneyTwo);
            Assert.False(moneyOne > moneyTwo);
        }
    }
}
