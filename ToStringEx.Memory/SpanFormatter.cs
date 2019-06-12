using System;
using System.Text;

namespace ToStringEx.Memory
{
    public class MemoryFormatter<T> : IFormatterEx<Memory<T>>, IFormatterEx<ReadOnlyMemory<T>>
    {
        private readonly SpanFormatter<T> formatter;

        public MemoryFormatter() : this(null) { }
        public MemoryFormatter(IFormatterEx<T> f)
        {
            formatter = new SpanFormatter<T>(f);
        }

        public Type TargetType => typeof(ReadOnlyMemory<T>);

        public string Format(Memory<T> value) => formatter.Format(value.Span);

        public string Format(ReadOnlyMemory<T> value) => formatter.Format(value.Span);

        string IFormatterEx.Format(object value)
        {
            switch (value)
            {
                case Memory<T> mem:
                    return Format(mem);
                default:
                    return Format((ReadOnlyMemory<T>)value);
            }
        }
    }

    public class SpanFormatter<T> : EnumerableFormatterBase<T>, ISpanFormatterEx<T>
    {
        /// <summary>
        /// Initializes an instance of <see cref="SpanFormatter{T}"/>.
        /// </summary>
        public SpanFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="SpanFormatter{T}"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public SpanFormatter(IFormatterEx<T> formatter) : base(formatter) { }

        public string Format(Span<T> span) => Format((ReadOnlySpan<T>)span);

        public string Format(ReadOnlySpan<T> span)
        {
            if (typeof(T) == typeof(string) && Formatter == null)
                return span.ToString();
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append('[');
                var e = span.GetEnumerator();
                if (e.MoveNext())
                {
                    builder.Append(e.Current.ToStringEx(Formatter));
                    while (e.MoveNext())
                    {
                        builder.Append(", ");
                        builder.Append(e.Current.ToStringEx(Formatter));
                    }
                }
                builder.Append(']');
                return builder.ToString();
            }
        }
    }
}
