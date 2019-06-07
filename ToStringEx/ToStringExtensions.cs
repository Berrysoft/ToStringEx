using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ToStringEx
{
    public static class ToStringExtensions
    {
        public static string ToStringEx(this object obj)
        {
            Type t = obj.GetType();
            if (t.FullName.StartsWith("System.ValueTuple`"))
            {
                return string.Format("({0})", string.Join(", ", EnumerateFields(obj, t).EnumerateTupleLike()));
            }
            else if (t.FullName.StartsWith("System.Tuple`"))
            {
                return string.Format("({0})", string.Join(", ", EnumerateProperties(obj, t).EnumerateTupleLike()));
            }
            else if (t.GetMethod("ToString", Array.Empty<Type>()).DeclaringType == t)
            {
                return obj.ToString();
            }
            else if (obj is IEnumerable source)
            {
                return IEnumerableToStringEx(source);
            }
            else
            {
                return string.Format(
                    "{{ {0} }}",
                    string.Join(", ", EnumerateFields(obj, t).EnumeratePropLike().Concat(EnumerateProperties(obj, t).EnumeratePropLike())));
            }
        }

        private static IEnumerable<(string Name, string Value)> EnumerateFields(object obj, Type t)
            => from f in t.GetFields()
               where f.Attributes.HasFlag(FieldAttributes.Public)
               select (f.Name, f.GetValue(obj).ToStringEx());

        private static IEnumerable<(string Name, string Value)> EnumerateProperties(object obj, Type t)
            => from p in t.GetProperties()
               where p.CanRead && p.GetGetMethod().IsPublic && p.GetIndexParameters().Length == 0
               select (p.Name, p.GetValue(obj).ToStringEx());

        private static IEnumerable<string> EnumeratePropLike(this IEnumerable<(string Name, string Value)> source)
            => source.Select(t => $"{t.Name}: {t.Value}");

        private static IEnumerable<string> EnumerateTupleLike(this IEnumerable<(string Name, string Value)> source)
            => source.Select(t => t.Value);

        private static string IEnumerableToStringEx(IEnumerable source)
            => string.Format("{{{0}}}", string.Join(", ", source.Cast<object>().Select(obj => obj.ToStringEx())));
    }
}
