using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToStringEx
{
    internal static class ReflectionFormatterHelper
    {
        public static IEnumerable<(string Name, string Value)> EnumerateFields(this object obj, IEnumerable<IFormatterEx> formatters)
            => from f in obj.GetType().GetFields()
               select (f.Name, f.GetValue(obj).ToStringEx(formatters));

        public static IEnumerable<(string Name, string Value)> EnumerateProperties(this object obj, IEnumerable<IFormatterEx> formatters)
            => from p in obj.GetType().GetProperties()
               where p.CanRead && p.GetIndexParameters().Length == 0
               select (p.Name, p.GetValue(obj).ToStringEx(formatters));
    }

    /// <summary>
    /// Represents a formatter which formats an object with reflection.
    /// </summary>
    public class ReflectionFormatter : MultiFormatterBase, IFormatterEx<object>
    {
        /// <summary>
        /// Determines whether to format the object in multi-line.
        /// </summary>
        public bool MultiLine { get; }

        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/>.
        /// </summary>
        public ReflectionFormatter() : this(false) { }
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public ReflectionFormatter(IEnumerable<IFormatterEx> formatters) : this(false, formatters) { }
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/> with a set of formatters.
        /// </summary>
        /// <param name="formatters">The set of formatters.</param>
        public ReflectionFormatter(params IFormatterEx[] formatters) : this(false, formatters.AsEnumerable()) { }
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/>.
        /// </summary>
        /// <param name="multiLine">Whether to format the object in multi-line.</param>
        /// <param name="formatters">The set of formatters.</param>
        public ReflectionFormatter(bool multiLine, IEnumerable<IFormatterEx> formatters)
            : base(formatters) => MultiLine = multiLine;
        /// <summary>
        /// Initializes an instance of <see cref="ReflectionFormatter"/>.
        /// </summary>
        /// <param name="multiLine">Whether to format the object in multi-line.</param>
        /// <param name="formatters">The set of formatters.</param>
        public ReflectionFormatter(bool multiLine, params IFormatterEx[] formatters) : this(multiLine, formatters.AsEnumerable()) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(object);

        /// <inhertidoc/>
        public string Format(object obj)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('{');
            if (MultiLine)
                builder.AppendLine();
            else
                builder.Append(' ');
            var ts = obj.EnumerateFields(Formatters).Concat(obj.EnumerateProperties(Formatters));
            var e = ts.GetEnumerator();
            if (e.MoveNext())
            {
                if (MultiLine)
                    builder.Append(' ', 2);
                builder.AppendFormat("{0}: {1}", e.Current.Name, e.Current.Value);
                while (e.MoveNext())
                {
                    if (MultiLine)
                        builder.Append(",\r\n  ");
                    else
                        builder.Append(", ");
                    builder.AppendFormat("{0}: {1}", e.Current.Name, e.Current.Value);
                }
            }
            if (MultiLine)
                builder.AppendLine(",");
            else
                builder.Append(' ');
            builder.Append('}');
            return builder.ToString();
        }
    }
}
