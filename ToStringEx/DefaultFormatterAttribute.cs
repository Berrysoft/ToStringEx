using System;

namespace ToStringEx
{
    /// <summary>
    /// Specifies a default formatter for a type, when it doesn't override <see cref="object.ToString"/>.
    /// This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class DefaultFormatterAttribute : Attribute
    {
        /// <summary>
        /// The type of the formatter.
        /// </summary>
        public Type FormatterType { get; }

        /// <summary>
        /// Initializes an instance of <see cref="DefaultFormatterAttribute"/> with the type of the formatter.
        /// </summary>
        /// <param name="formatterType">The type of the formatter.</param>
        public DefaultFormatterAttribute(Type formatterType) => FormatterType = formatterType;
    }
}
