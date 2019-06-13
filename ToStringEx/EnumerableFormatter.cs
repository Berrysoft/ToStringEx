using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    /// <summary>
    /// Represents a base formatter which formats a set of elements.
    /// </summary>
    public abstract class EnumerableFormatterBase
    {
        /// <summary>
        /// The formatter for each element.
        /// </summary>
        public IFormatterEx Formatter { get; }

        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatterBase"/>.
        /// </summary>
        public EnumerableFormatterBase() : this(null) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatterBase"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatterBase(IFormatterEx formatter)
            => Formatter = formatter;
    }
    /// <summary>
    /// Represents a base formatter which formats a set of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public abstract class EnumerableFormatterBase<T>
    {
        /// <summary>
        /// The formatter for each element.
        /// </summary>
        public IFormatterEx<T> Formatter { get; }

        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatterBase{T}"/>.
        /// </summary>
        public EnumerableFormatterBase() : this(null) { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatterBase{T}"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatterBase(IFormatterEx<T> formatter)
            => Formatter = formatter;
    }

    internal static class EnumerableFormatterHelper
    {
        public static string FormatInternal<T>(this IEnumerable<T> source, Func<T, string> func)
            => string.Format("{{{0}}}", string.Join(", ", source.Select(func)));
    }

    /// <summary>
    /// Represents a formatter for <see cref="IEnumerable"/>.
    /// </summary>
    public class EnumerableFormatter : EnumerableFormatterBase, IFormatterEx<IEnumerable>
    {
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter"/>.
        /// </summary>
        public EnumerableFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatter(IFormatterEx formatter) : base(formatter) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(IEnumerable);

        /// <inhertidoc/>
        public string Format(IEnumerable value)
            => value.Cast<object>().FormatInternal(e => e.ToStringEx(Formatter));

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
        public EnumerableFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="EnumerableFormatter{T}"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public EnumerableFormatter(IFormatterEx<T> formatter) : base(formatter) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(IEnumerable<T>);

        /// <inhertidoc/>
        public string Format(IEnumerable<T> value)
            => value.FormatInternal(e => e.ToStringEx(Formatter));

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
