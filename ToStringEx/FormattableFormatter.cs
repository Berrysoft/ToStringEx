using System;

namespace ToStringEx
{
    /// <summary>
    /// Represents a formatter for <see cref="IFormattable"/>.
    /// </summary>
    /// <typeparam name="T">A type implements <see cref="IFormattable"/>.</typeparam>
    public class FormattableFormatter<T> : IFormatterEx<T>
        where T : IFormattable
    {
        /// <summary>
        /// The format string.
        /// </summary>
        public string FormatString { get; }
        /// <summary>
        /// The format provider.
        /// </summary>
        public IFormatProvider Provider { get; }

        /// <inhertidoc/>
        public Type TargetType => typeof(T);

        /// <summary>
        /// Initializes an instance of <see cref="FormattableFormatter{T}"/>.
        /// </summary>
        public FormattableFormatter() : this(null, null) { }
        /// <summary>
        /// Initializes an instance of <see cref="FormattableFormatter{T}"/> with format string.
        /// </summary>
        /// <param name="format">The format string.</param>
        public FormattableFormatter(string format) : this(format, null) { }
        /// <summary>
        /// Initializes an instance of <see cref="FormattableFormatter{T}"/> with format provider.
        /// </summary>
        /// <param name="provider">The format provider.</param>
        public FormattableFormatter(IFormatProvider provider) : this(null, provider) { }
        /// <summary>
        /// Initializes an instance of <see cref="FormattableFormatter{T}"/> with format string and format provider.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="provider">The format provider.</param>
        public FormattableFormatter(string format, IFormatProvider provider)
        {
            FormatString = format;
            Provider = provider;
        }

        /// <inhertidoc/>
        public string Format(T value) => value.ToString(FormatString, Provider);

        string IFormatterEx.Format(object value) => Format((T)value);
    }
}
