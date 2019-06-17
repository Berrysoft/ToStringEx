using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToStringEx
{
    /// <summary>
    /// Represents a base formatter which formats a set of elements.
    /// </summary>
    public abstract class EnumerableFormatterBase : SequenceFormatterBase
    {
        /// <summary>
        /// Maximum count of the elements.
        /// </summary>
        public int MaxCount { get; }

        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatterBase"/>
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        /// <param name="maxCount">Maximum count of the elements.</param>
        public EnumerableFormatterBase(IFormatterEx formatter, int maxCount) : base(formatter) => MaxCount = maxCount;
    }
    /// <summary>
    /// Represents a base formatter which formats a set of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public abstract class EnumerableFormatterBase<T> : SequenceFormatterBase<T>
    {
        /// <summary>
        /// Maximum count of the elements.
        /// </summary>
        public int MaxCount { get; }

        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatterBase{T}"/>
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        /// <param name="maxCount">Maximum count of the elements.</param>
        public EnumerableFormatterBase(IFormatterEx<T> formatter, int maxCount) : base(formatter) => MaxCount = maxCount;
    }

    internal static class EnumerableFormatterHelper
    {
        public static string FormatInternal<T>(this IEnumerable<T> source, Func<T, string> func, int maxCount)
        {
            if (maxCount > 0)
            {
                maxCount--;
                StringBuilder builder = new StringBuilder();
                builder.Append('{');
                using (var e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        builder.Append(func(e.Current));
                        int i = 1;
                        for (; i < maxCount && e.MoveNext(); i++)
                        {
                            builder.Append(", ");
                            builder.Append(func(e.Current));
                        }
                        if (i == maxCount && e.MoveNext())
                        {
                            T c = e.Current;
                            bool ep = false;
                            while (e.MoveNext())
                            {
                                c = e.Current;
                                ep = true;
                            }
                            builder.Append(", ");
                            if (ep)
                                builder.Append("... ");
                            builder.Append(func(c));
                        }
                    }
                }
                builder.Append('}');
                return builder.ToString();
            }
            else
            {
                return string.Format("{{{0}}}", string.Join(", ", source.Select(func)));
            }
        }
    }

    /// <summary>
    /// Represents a formatter for <see cref="IEnumerable"/>.
    /// </summary>
    public class EnumerableFormatter : EnumerableFormatterBase, IFormatterEx<IEnumerable>
    {
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter"/>.
        /// </summary>
        public EnumerableFormatter() : this(null, 0) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatter(IFormatterEx formatter) : this(formatter, 0) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter"/> with maximum count.
        /// </summary>
        /// <param name="maxCount">Maximum count of the elements.</param>
        public EnumerableFormatter(int maxCount) : this(null, maxCount) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter"/> with a formatter and maximum count.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        /// <param name="maxCount">Maximum count of the elements.</param>
        public EnumerableFormatter(IFormatterEx formatter, int maxCount) : base(formatter, maxCount) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(IEnumerable);

        /// <inhertidoc/>
        public string Format(IEnumerable value)
            => value.Cast<object>().FormatInternal(e => e.ToStringEx(Formatter), MaxCount);

        string IFormatterEx.Format(object value) => Format((IEnumerable)value);
    }

    /// <summary>
    /// Represents a formatter for <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public class EnumerableFormatter<T> : EnumerableFormatterBase<T>, IFormatterEx<IEnumerable<T>>
    {
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter{T}"/>.
        /// </summary>
        public EnumerableFormatter() : this(null, 0) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter{T}"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatter(IFormatterEx<T> formatter) : this(formatter, 0) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter{T}"/> with maximum count.
        /// </summary>
        /// <param name="maxCount">Maximum count of the elements.</param>
        public EnumerableFormatter(int maxCount) : this(null, maxCount) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter{T}"/> with a formatter and maximum count.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        /// <param name="maxCount">Maximum count of the elements.</param>
        public EnumerableFormatter(IFormatterEx<T> formatter, int maxCount) : base(formatter, maxCount) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(IEnumerable<T>);

        /// <inhertidoc/>
        public string Format(IEnumerable<T> value)
            => value.FormatInternal(e => e.ToStringEx(Formatter), MaxCount);

        string IFormatterEx.Format(object value) => Format((IEnumerable<T>)value);
    }

    /// <summary>
    /// Represents a formatter for <see cref="IEnumerable{Char}"/>.
    /// </summary>
    public class CharEnumerableFormatter : IFormatterEx<IEnumerable<char>>
    {
        /// <inhertidoc/>
        public Type TargetType => typeof(IEnumerable<char>);

        /// <inhertidoc/>
        public string Format(IEnumerable<char> value)
        {
            if (value is string str)
            {
                return str;
            }
            else
            {
                char[] arr = value.ToArray();
                return new string(arr);
            }
        }

        string IFormatterEx.Format(object value) => Format((IEnumerable<char>)value);
    }
}
