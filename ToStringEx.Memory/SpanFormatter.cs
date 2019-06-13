using System;
using System.Text;

namespace ToStringEx.Memory
{
    /// <summary>
    /// Represents a base formatter for <see cref="Memory{T}"/> and <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of each element.</typeparam>
    public class MemoryFormatterBase<T>
    {
        private readonly SpanFormatter<T> formatter;

        /// <summary>
        /// Initializes an instance of <see cref="MemoryFormatterBase{T}"/>.
        /// </summary>
        public MemoryFormatterBase() : this(null) { }
        /// <summary>
        /// Initializes an instance of <see cref="MemoryFormatterBase{T}"/> with a formatter.
        /// </summary>
        /// <param name="f">The formatter for each element.</param>
        public MemoryFormatterBase(IFormatterEx<T> f) => formatter = new SpanFormatter<T>(f);

        /// <summary>
        /// Formats the memory.
        /// </summary>
        /// <param name="value">The memory.</param>
        /// <returns>A humanized string.</returns>
        protected internal string FormatInternal(ReadOnlyMemory<T> value) => formatter.Format(value.Span);
    }

    /// <summary>
    /// Represents a formatter for <see cref="Memory{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of each element.</typeparam>
    public class MemoryFormatter<T> : MemoryFormatterBase<T>, IFormatterEx<Memory<T>>
    {
        /// <summary>
        /// Initializes an instance of <see cref="MemoryFormatter{T}"/>.
        /// </summary>
        public MemoryFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="MemoryFormatter{T}"/> with a formatter.
        /// </summary>
        /// <param name="f">The formatter for each element.</param>
        public MemoryFormatter(IFormatterEx<T> f) : base(f) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(Memory<T>);

        /// <inhertidoc/>
        public string Format(Memory<T> value) => FormatInternal(value);

        string IFormatterEx.Format(object value) => Format((Memory<T>)value);
    }

    /// <summary>
    /// Represents a formatter for <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of each element.</typeparam>
    public class ReadOnlyMemoryFormatter<T> : MemoryFormatterBase<T>, IFormatterEx<ReadOnlyMemory<T>>
    {
        /// <summary>
        /// Initializes an instance of <see cref="ReadOnlyMemoryFormatter{T}"/>.
        /// </summary>
        public ReadOnlyMemoryFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="ReadOnlyMemoryFormatter{T}"/> with a formatter.
        /// </summary>
        /// <param name="f">The formatter for each element.</param>
        public ReadOnlyMemoryFormatter(IFormatterEx<T> f) : base(f) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(ReadOnlyMemory<T>);

        /// <inhertidoc/>
        public string Format(ReadOnlyMemory<T> value) => FormatInternal(value);

        string IFormatterEx.Format(object value) => Format((ReadOnlyMemory<T>)value);
    }

    /// <summary>
    /// Represents a formatter for <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of each element.</typeparam>
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

        /// <inhertidoc/>
        public string Format(Span<T> span) => Format((ReadOnlySpan<T>)span);

        /// <inhertidoc/>
        public string Format(ReadOnlySpan<T> span)
        {
            if (typeof(T) == typeof(char) && Formatter == null)
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
