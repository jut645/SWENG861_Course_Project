using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.Skyscanner.WebAPI.Models
{
    public class Money
    {
        public decimal Value { get; private set; }

        public CurrencyType CurrencyType { get; private set; }

        public Money(decimal value, CurrencyType type)
        {
            Value = value;
            CurrencyType = type;
        }

        private Money() { }

        public static Money operator+ (Money leftHandSide, Money rightHandSide)
        {
            if (leftHandSide.CurrencyType != rightHandSide.CurrencyType)
            {
                throw new InvalidOperationException("The two Money objects do " +
                    "not have the same currency type.");
            }

            var result = new Money();

            result.Value = leftHandSide.Value + rightHandSide.Value;
            result.CurrencyType = leftHandSide.CurrencyType;

            return result;
        }

        public static bool operator>= (Money leftHandSide, Money rightHandSide)
        {
            if (leftHandSide.CurrencyType != rightHandSide.CurrencyType)
            {
                throw new InvalidOperationException("The two Money objects do " +
                    "not have the same currency type.");
            }

            return leftHandSide.Value >= rightHandSide.Value;
        }

        public static bool operator <= (Money leftHandSide, Money rightHandSide)
        {
            return !(leftHandSide > rightHandSide);
        }

        public static bool operator< (Money leftHandSide, Money rightHandSide)
        {
            return !(leftHandSide >= rightHandSide);
        }

        public static bool operator>(Money leftHandSide, Money rightHandSide)
        {
            return !(leftHandSide < rightHandSide);
        }

        public static bool operator==(Money leftHandSide, Money rightHandSide)
        {
            return !(leftHandSide > rightHandSide) && !(leftHandSide < rightHandSide);
        }

        public static bool operator!=(Money leftHandSide, Money rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }


    }
}
