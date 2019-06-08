using System;
using System.Linq;
using System.Text;

namespace ToStringEx
{
    /// <summary>
    /// Represents a formatter for <see cref="Array"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayFormatter<T> : EnumerableFormatterBase<T>, IFormatterEx<Array>
    {
        /// <summary>
        /// Initializes an instance of <see cref="ArrayFormatter{T}"/>.
        /// </summary>
        public ArrayFormatter() : base() { }
        /// <summary>
        /// Initializes an instance of <see cref="ArrayFormatter{T}"/> with a formatter.
        /// </summary>
        /// <param name="formatter">The formatter for each element.</param>
        public ArrayFormatter(IFormatterEx<T> formatter) : base(formatter) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(Array);

        /// <inhertidoc/>
        public string Format(Array arr)
        {
            int rank = arr.Rank;
            int[] lens = Enumerable.Range(0, rank).Select(r => arr.GetLength(r)).ToArray();
            int[] indices = new int[rank];
            StringBuilder builder = new StringBuilder();
            builder.Append('{', rank);
            while (indices[0] < lens[0])
            {
                builder.Append(((T)arr.GetValue(indices)).ToStringEx(Formatter));
                int i = rank - 1;
                for (; i >= 0; i--)
                {
                    indices[i]++;
                    if (indices[i] < lens[i] || i == 0) break;
                    indices[i] = 0;
                }
                if (indices[0] < lens[0])
                {
                    int repeat = rank - 1 - i;
                    builder.Append('}', repeat);
                    builder.Append(", ");
                    builder.Append('{', repeat);
                }
            }
            builder.Append('}', rank);
            return builder.ToString();
        }

        string IFormatterEx.Format(object value) => Format((Array)value);
    }
}
