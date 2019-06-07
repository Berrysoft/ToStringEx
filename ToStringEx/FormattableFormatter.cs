using System;

namespace ToStringEx
{
    public class FormattableFormatter<T> : IFormatterEx<T>
        where T : IFormattable
    {
        public string FormatStr { get; }
        public IFormatProvider Provider { get; }

        public FormattableFormatter() : this(null, null) { }
        public FormattableFormatter(string format) : this(format, null) { }
        public FormattableFormatter(IFormatProvider provider) : this(null, provider) { }
        public FormattableFormatter(string format, IFormatProvider provider)
        {
            FormatStr = format;
            Provider = provider;
        }

        public string Format(T value) => value.ToString(FormatStr, Provider);
    }
}
