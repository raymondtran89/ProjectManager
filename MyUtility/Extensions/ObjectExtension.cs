using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace MyUtility.Extensions
{
    public static class ObjectExtension
    {
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(anonymousObject.GetType()))
                expando.Add(property.Name, property.GetValue(anonymousObject));
            return (ExpandoObject) expando;
        }
    }
}