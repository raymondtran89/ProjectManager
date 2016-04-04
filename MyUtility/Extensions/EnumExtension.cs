/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: EnumExtension
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyUtility.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        ///     Author: ThongNT
        ///     <para>Lay ten Enum hoac Description cua Enum</para>
        /// </summary>
        /// <param name="eEnum"></param>
        /// <returns></returns>
        public static string Text(this Enum eEnum)
        {
            var fi = eEnum.GetType().GetField(eEnum.ToString());

            var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);
            return attributes.Any() ? attributes[0].Description : eEnum.ToString();
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para>Lay value cua Enum</para>
        /// </summary>
        /// <param name="eEnum"></param>
        /// <returns></returns>
        public static int Value(this Enum eEnum)
        {
            var changeType = Convert.ChangeType(eEnum, eEnum.GetTypeCode());
            if (changeType != null)
                return (int) changeType;
            return -9999;
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para>Covert String to Enum object</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this object enumString)
        {
            return (T) Enum.Parse(typeof (T), enumString.ToString());
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para>Convert number value to Enum</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static T NumberToEnum<T>(this int enumValue)
        {
            return (T) Enum.ToObject(typeof (T), enumValue);
        }

        public static List<EnumToList> ToList(this Type eNum)
        {
            var enumValues = Enum.GetValues(eNum).Cast<Enum>();

            var items = (from enumValue in enumValues
                select new EnumToList
                {
                    Key = enumValue.Value(),
                    Value = enumValue.Text()
                }).ToList();

            return items;
        }
    }

    public class EnumToList
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}