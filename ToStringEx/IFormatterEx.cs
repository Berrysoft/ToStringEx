using System;

namespace ToStringEx
{
    /// <summary>
    /// Exposes a set of members for a formatter.
    /// </summary>
    public interface IFormatterEx
    {
        /// <summary>
        /// The type of object to format.
        /// </summary>
        Type TargetType { get; }
        /// <summary>
        /// Formats the object.
        /// </summary>
        /// <param name="value">The object.</param>
        /// <returns>A humanize string.</returns>
        string Format(object value);
    }

    /// <summary>
    /// Exposes a set of members for a generic formatter.
    /// </summary>
    /// <typeparam name="T">The type of object to format.</typeparam>
    public interface IFormatterEx<in T> : IFormatterEx
    {
        /// <summary>
        /// Formats the object.
        /// </summary>
        /// <param name="value">The object.</param>
        /// <returns>A humanize string.</returns>
        string Format(T value);
    }
}
