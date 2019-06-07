using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    public static class ToStringExtensions
    {
        public static string ToStringEx(this object obj)
        {
            Type t = obj.GetType();
            if (obj is IEnumerable source)
                return IEnumerableToStringEx(source);
            else if (t.GetMethods().Any(m => m.Name == "ToString" && m.GetParameters().Length == 0))
                return obj.ToString();
            else
            {
                var props = from p in t.GetProperties()
                            where p.CanRead && p.GetIndexParameters().Length == 0
                            select $"{p.Name}: {p.GetValue(obj)}";
                return string.Format("{{{0}}}", string.Join(", ", props));
            }
        }

        private static string IEnumerableToStringEx(IEnumerable source)
        {
            return string.Format("{{{0}}}", string.Join(", ", source.Cast<object>().Select(obj => obj.ToStringEx())));
        }
    }
}
