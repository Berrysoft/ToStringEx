using System;
using System.Linq;
using System.Text;

namespace ToStringEx
{
    internal static class ArrayFormatterHelper
    {
        public static string FormatInternal<T>(this Array arr, Func<T, string> func, bool useReturn, int[] maxCount)
        {
            int rank = arr.Rank;
            int[] lens = Enumerable.Range(0, rank).Select(r => arr.GetLength(r)).ToArray();
            int[] indices = new int[rank];
            if (maxCount.Length < rank)
            {
                if (maxCount.Length == 1)
                    maxCount = Enumerable.Repeat(maxCount[0], rank).ToArray();
                else
                    Array.Resize(ref maxCount, rank);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append('{', rank);
            while (indices[0] < lens[0])
            {
                builder.Append(func((T)arr.GetValue(indices)));
                int i = rank - 1;
                bool ep = false;
                for (; i >= 0; i--)
                {
                    indices[i]++;
                    if (indices[i] == maxCount[i] - 1)
                    {
                        indices[i] = lens[i] - 1;
                        ep = lens[i] > maxCount[i];
                    }
                    if (indices[i] < lens[i] || i == 0) break;
                    indices[i] = 0;
                }
                if (indices[0] < lens[0])
                {
                    int repeat = rank - 1 - i;
                    builder.Append('}', repeat);
                    builder.Append(',');
                    if (indices[i] == lens[i] - 1 && ep)
                    {
                        if (useReturn && i != rank - 1)
                        {
                            builder.AppendLine();
                            builder.Append(' ', rank - repeat);
                        }
                        else
                            builder.Append(' ');
                        builder.Append("...");
                    }
                    if (useReturn && repeat > 0)
                    {
                        builder.AppendLine();
                        builder.Append(' ', rank - repeat);
                    }
                    else
                        builder.Append(' ');
                    builder.Append('{', repeat);
                }
            }
            builder.Append('}', rank);
            return builder.ToString();
        }
    }

    public abstract class ArrayFormatterBase : SequenceFormatterBase, IFormatterEx<Array>
    {
        public int[] MaxCount { get; }

        public bool UseReturn { get; }

        public ArrayFormatterBase(IFormatterEx formatter, bool useReturn, int[] maxCount) : base(formatter)
        {
            UseReturn = useReturn;
            MaxCount = maxCount;
        }

        /// <inhertidoc/>
        public Type TargetType => typeof(Array);

        /// <inhertidoc/>
        public abstract string Format(Array arr);

        string IFormatterEx.Format(object value) => Format((Array)value);
    }

    /// <summary>
    /// Represents a formatter for <see cref="Array"/>.
    /// </summary>
    public class ArrayFormatter : ArrayFormatterBase
    {
        /// <summary>
        /// Initializes an instance of <see cref="ArrayFormatter"/>.
        /// </summary>
        public ArrayFormatter() : this(null) { }
        public ArrayFormatter(bool useReturn, params int[] maxCount) : this(null, useReturn, maxCount) { }
        public ArrayFormatter(IFormatterEx formatter, bool useReturn = false, params int[] maxCount) : base(formatter, useReturn, maxCount) { }

        /// <inhertidoc/>
        public override string Format(Array arr)
            => arr.FormatInternal<object>(e => e.ToStringEx(Formatter), UseReturn, MaxCount);
    }

    /// <summary>
    /// Represents a formatter for <see cref="Array"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayFormatter<T> : ArrayFormatterBase
    {
        /// <summary>
        /// Initializes an instance of <see cref="ArrayFormatter{T}"/>.
        /// </summary>
        public ArrayFormatter() : this(null) { }
        public ArrayFormatter(bool useReturn = false, params int[] maxCount) : this(null, useReturn, maxCount) { }
        public ArrayFormatter(IFormatterEx<T> formatter, bool useReturn = false, params int[] maxCount) : base(formatter, useReturn, maxCount) { }

        /// <inhertidoc/>
        public override string Format(Array arr)
            => arr.FormatInternal<T>(e => e.ToStringEx(Formatter), UseReturn, MaxCount);
    }
}
