using System;

namespace ToStringEx
{
    /// <summary>
    /// Represents a formatter which formats an object with a function.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public class FuncFormatter<T> : IFormatterEx<T>
    {
        /// <summary>
        /// The format function.
        /// </summary>
        public Func<T, string> Func { get; }

        /// <inheritdoc/>
        public Type TargetType => typeof(T);

        /// <summary>
        /// Initializes an instance of <see cref="FuncFormatter{T}"/> with a function.
        /// </summary>
        /// <param name="func">The format function.</param>
        public FuncFormatter(Func<T, string> func) => Func = func;

        /// <inheritdoc/>
        public string Format(T value)
        {
            if (Func == null)
                return value.ToStringEx();
            else
                return Func(value);
        }

        string IFormatterEx.Format(object value) => Format((T)value);
    }
}
