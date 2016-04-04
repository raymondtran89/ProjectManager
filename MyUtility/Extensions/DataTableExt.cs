using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MyUtility.Extensions
{
    public static class DataTableExt
    {
        /// <summary>
        ///     Author : ThongNT
        ///     <para>Converts DataTable To List</para>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        {
            var dataList = new List<TSource>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo aProp in typeof (TSource).GetProperties(flags)
                select new
                {
                    aProp.Name,
                    Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                }).ToList();
            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                select new {Name = aHeader.ColumnName, Type = aHeader.DataType}).ToList();
            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

            foreach (var dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    var propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                    propertyInfos.SetValue(aTSource, dataRow[aField.Name], null);
                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

        public static IEnumerable<T> Flatten<T, TR>(this IEnumerable<T> source, Func<T, TR> recursion)
            where TR : IEnumerable<T>
        {
            return source.SelectMany(
                x => recursion(x) != null && recursion(x).Any() ? recursion(x).Flatten(recursion) : null)
                .Where(x => x != null);
        }

        /// <summary>
        ///     <para>Author:TrungLD</para>
        ///     <para>DateCreated: 09/03/2015</para>
        ///     <para>convert List to table</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> data)
        {
            var enumerable = data as T[] ?? data.ToArray();
            var list = enumerable.Cast<IDataRecord>().ToList();

            PropertyDescriptorCollection props = null;
            var table = new DataTable();
            if (list.Count > 0)
            {
                props = TypeDescriptor.GetProperties(list[0]);
                for (var i = 0; i < props.Count; i++)
                {
                    var prop = props[i];
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }
            if (props == null) return table;
            {
                var values = new object[props.Count];
                foreach (var item in enumerable)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item) ?? DBNull.Value;
                    }
                    table.Rows.Add(values);
                }
            }
            return table;
        }

        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            var props = TypeDescriptor.GetProperties(typeof (T));
            var table = new DataTable();
            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            var values = new object[props.Count];
            foreach (var item in list)
            {
                for (var i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }
    }
}