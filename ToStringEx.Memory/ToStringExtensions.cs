using System;

namespace ToStringEx.Memory
{
    public static class SpanToStringExtensions
    {
        public static string ToStringEx<T>(this Span<T> span)
            => ((ReadOnlySpan<T>)span).ToStringEx();

        public static string ToStringEx<T>(this ReadOnlySpan<T> span)
            => span.ToStringEx(new SpanFormatter<T>());

        public static string ToStringEx<T>(this Span<T> span, ISpanFormatterEx<T> formatter)
            => ((ReadOnlySpan<T>)span).ToStringEx(formatter);

        public static string ToStringEx<T>(this ReadOnlySpan<T> span, ISpanFormatterEx<T> formatter)
        {
            if (formatter != null)
                return formatter.Format(span);
            else
                return span.ToStringEx();
        }
    }
}
