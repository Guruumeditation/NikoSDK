﻿using System;
using System.Globalization;

namespace Net.ArcanaStudio.NikoSDK
{
    internal static class Extensions
    {
        public static DateTime? ParseNikoDateTimeString(this string s)
        {
            return string.IsNullOrEmpty(s) ? (DateTime?) null : DateTime.ParseExact(s, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        }
    }
}
