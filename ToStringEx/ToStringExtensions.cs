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
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            Type t = obj.GetType();
            if (t.FullName.StartsWith("System.ValueTuple`") || t.FullName.StartsWith("System.Tuple`"))
            {
                return obj.ToStringEx(new TupleFormatter());
            }
            else if (t.GetMethod("ToString", Array.Empty<Type>()).DeclaringType == t)
            {
                return obj.ToString();
            }
            else if (t.IsArray)
            {
                return ((Array)obj).ToStringEx(new ArrayFormatter());
            }
            else if (obj is IEnumerable source)
            {
                return source.ToStringEx(new EnumerableFormatter());
            }
            else
            {
                return obj.ToStringEx(new ReflectionFormatter());
            }
        }

        public static string ToStringEx(this object obj, IFormatterEx formatter)
        {
            if (formatter == null)
                return obj.ToStringEx();
            else
                return formatter.Format(obj);
        }

        public static string ToStringEx<T>(this T obj, IFormatterEx<T> formatter)
        {
            if (formatter == null)
                return obj.ToStringEx();
            else
                return formatter.Format(obj);
        }

        internal static string ToStringEx(this object obj, Dictionary<Type, IFormatterEx> formatters)
        {
            Type t = obj.GetType();
            if (formatters != null && formatters.TryGetValue(t, out IFormatterEx formatter))
            {
                return obj.ToStringEx(formatter);
            }
            else
            {
                return obj.ToStringEx();
            }
        }

        internal static IEnumerable<(string Name, string Value)> EnumerateFields(this object obj, Dictionary<Type, IFormatterEx> formatters)
            => from f in obj.GetType().GetFields()
               where f.Attributes.HasFlag(FieldAttributes.Public)
               select (f.Name, f.GetValue(obj).ToStringEx(formatters));

        internal static IEnumerable<(string Name, string Value)> EnumerateProperties(this object obj, Dictionary<Type, IFormatterEx> formatters)
            => from p in obj.GetType().GetProperties()
               where p.CanRead && p.GetGetMethod().IsPublic && p.GetIndexParameters().Length == 0
               select (p.Name, p.GetValue(obj).ToStringEx(formatters));

        internal static IEnumerable<string> EnumeratePropLike(this IEnumerable<(string Name, string Value)> source)
            => source.Select(t => $"{t.Name}: {t.Value}");

        internal static IEnumerable<string> EnumerateTupleLike(this IEnumerable<(string Name, string Value)> source)
            => source.Select(t => t.Value);
    }
}
