using System;

namespace Backend.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeFrom1970(this long value)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(value).ToUniversalTime();
            return dateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLongFrom1970(this DateTime value)
        {
            TimeSpan span = value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return Convert.ToInt64(span.TotalMilliseconds);
        }
    }
}
