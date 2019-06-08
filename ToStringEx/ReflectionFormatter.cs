using System;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    internal static class ReflectionFormatterHelper
    {
        public static IEnumerable<(string Name, string Value)> EnumerateFields(this object obj, IEnumerable<IFormatterEx> formatters)
            => from f in obj.GetType().GetFields()
               where f.IsPublic
               select (f.Name, f.GetValue(obj).ToStringEx(formatters));

        public static IEnumerable<(string Name, string Value)> EnumerateProperties(this object obj, IEnumerable<IFormatterEx> formatters)
            => from p in obj.GetType().GetProperties()
               where p.CanRead && p.GetGetMethod().IsPublic && p.GetIndexParameters().Length == 0
               select (p.Name, p.GetValue(obj).ToStringEx(formatters));

        public static IEnumerable<string> EnumeratePropLike(this IEnumerable<(string Name, string Value)> source)
            => source.Select(t => $"{t.Name}: {t.Value}");

        public static IEnumerable<string> EnumerateTupleLike(this IEnumerable<(string Name, string Value)> source)
            => source.Select(t => t.Value);
    }

    /// <summary>
    /// Represents a formatter which formats an object with reflection.
    /// </summary>
    public class ReflectionFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/>.
        /// </summary>
        public ReflectionFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public ReflectionFormatter(IEnumerable<IFormatterEx> formatters) : base(formatters) { }
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public ReflectionFormatter(params IFormatterEx[] formatters) : base(formatters) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(object);

        /// <inhertidoc/>
        public string Format(object obj)
            => string.Format(
                    "{{ {0} }}",
                    string.Join(", ", obj.EnumerateFields(Formatters).EnumeratePropLike().Concat(obj.EnumerateProperties(Formatters).EnumeratePropLike())));
    }
}
