using System;

namespace ToStringEx.Memory
{
    /// <summary>
    /// Provides a set of <see langword="static"/> methods to perform ToStringEx for <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    public static class SpanToStringExtensions
    {
        /// <summary>
        /// Returns a humanize string that represents the current <see cref="Span{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of each element.</typeparam>
        /// <param name="span">The current span.</param>
        /// <returns>A humanize string.</returns>
        public static string ToStringEx<T>(this Span<T> span)
            => ((ReadOnlySpan<T>)span).ToStringEx();

        /// <summary>
        /// Returns a humanize string that represents the current <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of each element.</typeparam>
        /// <param name="span">The current span.</param>
        /// <returns>A humanize string.</returns>
        public static string ToStringEx<T>(this ReadOnlySpan<T> span)
            => span.ToStringEx(new SpanFormatter<T>());

        /// <summary>
        /// Returns a humanize string with a formatter that represents the current <see cref="Span{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of each element.</typeparam>
        /// <param name="span">The current span.</param>
        /// <param name="formatter">The formatter.</param>
        /// <returns>A humanize string.</returns>
        public static string ToStringEx<T>(this Span<T> span, ISpanFormatterEx<T> formatter)
            => ((ReadOnlySpan<T>)span).ToStringEx(formatter);

        /// <summary>
        /// Returns a humanize string with a formatter that represents the current <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of each element.</typeparam>
        /// <param name="span">The current span.</param>
        /// <param name="formatter">The formatter.</param>
        /// <returns>A humanize string.</returns>
        public static string ToStringEx<T>(this ReadOnlySpan<T> span, ISpanFormatterEx<T> formatter)
        {
            if (formatter != null)
                return formatter.Format(span);
            else
                return span.ToStringEx();
        }
    }
}
