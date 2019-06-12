using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ToStringEx
{
    /// <summary>
    /// Provides a set of <see langword="static"/> methods to perform ToStringEx.
    /// </summary>
    public static class ToStringExtensions
    {
        private static readonly List<IFormatterProviderEx> providers = new List<IFormatterProviderEx>();

        /// <summary>
        /// Registers a formatter provider.
        /// </summary>
        /// <param name="provider">An instance of a formatter provider.</param>
        public static void RegisterProvider(IFormatterProviderEx provider)
        {
            if (!providers.Contains(provider))
                providers.Add(provider);
        }

        /// <summary>
        /// Unregisters a formatter provider.
        /// </summary>
        /// <param name="provider">The instance of the formatter provider.</param>
        public static void UnregisterProvider(IFormatterProviderEx provider)
        {
            providers.Remove(provider);
        }

        /// <summary>
        /// Returns a humanize string that represents the current object.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <returns>A humanize string.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="obj"/> is <see langword="null"/> (<see langword="Nothing"/> in Visual Basic).</exception>
        public static string ToStringEx(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            Type t = obj.GetType();
            foreach (var provider in providers)
            {
                if (provider.TryGetProvider(t, out IFormatterEx formatter))
                    return obj.ToStringEx(formatter);
            }
            if (t.GetMethod("ToString", Array.Empty<Type>()).DeclaringType == t)
            {
                return obj.ToString();
            }
            else if (t.IsArray)
            {
                return ((Array)obj).ToStringEx(new ArrayFormatter());
            }
            else if (obj is IEnumerable source)
            {
                return source.ToStringEx(new EnumerableFormatter());
            }
            else
            {
                return obj.ToStringEx(new ReflectionFormatter());
            }
        }

        /// <summary>
        /// Returns a humanize string with a formatter that represents the current object.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <param name="formatter">The formatter.</param>
        /// <returns>A humanize string.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="obj"/> is <see langword="null"/> (<see langword="Nothing"/> in Visual Basic).</exception>
        public static string ToStringEx(this object obj, IFormatterEx formatter)
        {
            if (formatter == null)
                return obj.ToStringEx();
            else
                return formatter.Format(obj);
        }

        /// <summary>
        /// Returns a humanize string with a formatter that represents the current object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The current object.</param>
        /// <param name="formatter">The formatter.</param>
        /// <returns>A humanize string.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="obj"/> is <see langword="null"/> (<see langword="Nothing"/> in Visual Basic).</exception>
        public static string ToStringEx<T>(this T obj, IFormatterEx<T> formatter)
        {
            if (formatter == null)
                return obj.ToStringEx();
            else
                return formatter.Format(obj);
        }

        internal static string ToStringEx(this object obj, IEnumerable<IFormatterEx> formatters)
        {
            Type t = obj.GetType();
            if (formatters != null)
            {
                return obj.ToStringEx(formatters.FirstOrDefault(f => f.TargetType.IsAssignableFrom(t)));
            }
            else
            {
                return obj.ToStringEx();
            }
        }
    }
}
