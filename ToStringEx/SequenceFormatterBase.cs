namespace ToStringEx
{
    /// <summary>
    /// Represents a base formatter for sequences.
    /// </summary>
    public abstract class SequenceFormatterBase
    {
        /// <summary>
        /// The formatter for each element.
        /// </summary>
        public IFormatterEx Formatter { get; }

        /// <summary>
        /// Initializes an instance of <see cref="SequenceFormatterBase"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public SequenceFormatterBase(IFormatterEx formatter) => Formatter = formatter;
    }

    /// <summary>
    /// Represents a base formatter for sequences.
    /// </summary>
    /// <typeparam name="T">The type of each element.</typeparam>
    public abstract class SequenceFormatterBase<T>
    {
        /// <summary>
        /// The formatter for each element.
        /// </summary>
        public IFormatterEx<T> Formatter { get; }

        /// <summary>
        /// Initializes an instance of <see cref="SequenceFormatterBase{T}"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public SequenceFormatterBase(IFormatterEx<T> formatter) => Formatter = formatter;
    }
}
