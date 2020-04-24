namespace FlightPrices.Skyscanner.WebAPI.Models
{
    /// <summary>
    /// The <c>Money</c> class represents a money value in some currency.
    /// </summary>
    public class Money
    {
        public decimal Value { get; private set; }                // The scalar value of the money

        public CurrencyType CurrencyType { get; private set; }    // The money's currency type        

        /// <summary>
        /// <c>Money</c> class constructor.
        /// </summary>
        /// <param name="value">The scalar value of the money.</param>
        /// <param name="type">The currency type of the money.</param>
        public Money(decimal value, CurrencyType type)            
        {
            Value = value;
            CurrencyType = type;
        }

        /// <summary>
        /// Default <c>Money</c> class constructor.
        /// </summary>
        private Money() { }
    }
}
