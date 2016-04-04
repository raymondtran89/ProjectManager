using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyUtility
{
    /// <summary>
    ///     Base class for constant methods
    /// </summary>
    public class KeyValueConstant
    {
        protected string GetValueByKey(object key)
        {
            var p = GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var f in p)
            {
                var obj = f.GetValue(null);

                if (obj.GetType().GetProperty("Key") == null)
                {
                    continue;
                }

                if (obj.GetType().GetProperty("Key").GetValue(obj, null).ToString() == key.ToString())
                    return obj.GetType().GetProperty("Value").GetValue(obj, null).ToString();
            }
            return string.Empty;
        }

        protected List<T> GetAll<T>()
        {
            var type = typeof (T);
            var list = new List<T>();

            var p = GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var f in p)
            {
                var newObj = Activator.CreateInstance(type, null);

                var obj = f.GetValue(null);
                var key = obj.GetType().GetProperty("Key");
                var value = obj.GetType().GetProperty("Value");

                if (key == null || value == null)
                {
                    continue;
                }
                type.InvokeMember(key.Name, BindingFlags.SetProperty, null, newObj, new[] {key.GetValue(obj, null)});
                type.InvokeMember(value.Name, BindingFlags.SetProperty, null, newObj, new[] {value.GetValue(obj, null)});

                list.Add((T) newObj);
            }
            return list;
        }
    }

    /// <summary>
    ///     Define key/value with key's datatype is String
    /// </summary>
    public class StringKeyValue
    {
        public StringKeyValue()
        {
            Key = string.Empty;
            Value = string.Empty;
        }

        public StringKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }

    /// <summary>
    ///     Define key/value with key's datatype is Byte
    /// </summary>
    public class ByteKeyValue
    {
        public ByteKeyValue()
        {
            Key = 0;
            Value = string.Empty;
        }

        public ByteKeyValue(byte key, string value)
        {
            Key = key;
            Value = value;
        }

        public byte Key { get; set; }

        public string Value { get; set; }
    }

    /// <summary>
    ///     Define key/value with key's datatype is Integer
    /// </summary>
    public class IntKeyValue
    {
        public IntKeyValue()
        {
            Key = 0;
            Value = string.Empty;
        }

        public IntKeyValue(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public int Key { get; set; }

        public string Value { get; set; }
    }

    /// <summary>
    ///     Define key/value with key's datatype is Integer
    /// </summary>
    public class DateTimeKeyValue
    {
        public DateTimeKeyValue()
        {
            Key = DateTime.Now;
            Value = string.Empty;
        }

        public DateTimeKeyValue(DateTime key, string value)
        {
            Key = key;
            Value = value;
        }

        public DateTime Key { get; set; }

        public string Value { get; set; }
    }
}