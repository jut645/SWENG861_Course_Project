using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPrices.WebApp.CustomDataAnnotations
{
    public class CurrentDateAttribute : ValidationAttribute
    {
        public CurrentDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var dateTime = (DateTime?)value;
            if (dateTime == null)
            {
                return false;
            }
            

            if (dateTime.Value.Date >= DateTime.Now.Date)
            {
                return true;
            }

            return false;
        }
    }
}
