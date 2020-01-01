using System;
using System.Globalization;

namespace LibraryWebSite.Util
{
    public static class Extension
    {
        public static DateTime? ConvertToDateTime(string dateTime)
        {
            if (DateTime.TryParseExact(dateTime, "dd-MM-yyyy HH:mm:ss", null, DateTimeStyles.None, out DateTime dtVal))
            {
                return dtVal;
            }
            else
            {
                return null;
            }
          
        }
    }
}
