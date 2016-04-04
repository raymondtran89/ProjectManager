/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: ResourceExtension
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System;
using System.Resources;

namespace MyUtility.Extensions
{
    public static class ResourceExtension
    {
        /// <summary>
        ///     Author: ThongNT
        ///     <para>Lay resource theo Enum key</para>
        /// </summary>
        /// <param name="resourceManager"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumString(this ResourceManager resourceManager, Enum enumValue)
        {
            return resourceManager.GetString(string.Format("{0}.{1}", enumValue.GetType().Name, enumValue));
        }
    }
}