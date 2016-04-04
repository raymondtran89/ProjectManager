/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: CommonLogger
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System;
using System.Globalization;

namespace MyUtility.Extensions
{
    public static class NumberExtension
    {
        //        public static string ToCurrencyString(this int number, string unit = "")
        //        {
        //            return ConvertUtility.FormatCurrency(number, unit);
        //        }
        public static DateTime ConvertTimestamp(double ts)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var tt = origin.AddSeconds(ts).ToLocalTime();
            return tt;
        }

        public static string ToCurrencyString(this decimal number, bool enableShorten = false, bool showUnit = true,
            bool enableRound = true)
        {
            var unit = "đ";
            var format = "N00";

            if (enableRound == false)
            {
                if (showUnit)
                {
                    return number.ToString("#,##0.00") + unit;
                }
                return number.ToString("#,##0.00");
            }

            if (enableShorten)
            {
                if (number >= 1000000)
                {
                    number = number/1000000;
                    unit = "tr";
                    format = "N01";
                }
                else if (number >= 1000)
                {
                    number = number/1000;
                    unit = "k";
                    format = "N00";
                }
            }

            var currency = number.ToString(format, new CultureInfo("vi-VN"));

            return showUnit == false ? currency : string.Format("{0}{1}", currency, unit);
        }

        public static string ToCurrencyString(this decimal? number, bool enableShorten = false, bool showUnit = true,
            bool enableRound = true)
        {
            return ToCurrencyString(number.GetValueOrDefault(), enableShorten, showUnit, enableRound);
        }

        public static string ToCurrencyString(this int number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal) number, enableShorten, showUnit);
        }

        public static string ToCurrencyString(this long number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal) number, enableShorten, showUnit);
        }

        public static string ToCurrencyString(this int? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal) number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        public static string ToCurrencyString(this long? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal) number.GetValueOrDefault(0), enableShorten, showUnit);
        }
    }
}