using System;
using System.Reflection;

namespace ToStringEx.Reflection
{
    /// <summary>
    /// Exposes a set of method to format reflection metadata to target language.
    /// </summary>
    public interface ILanguageHelper
    {
        /// <summary>
        /// Format an instance of <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The instance of <see cref="Type"/>.</param>
        /// <returns>A string in target language.</returns>
        string FormatType(Type type);
        /// <summary>
        /// Format an instance of <see cref="TypeInfo"/>.
        /// </summary>
        /// <param name="type">The instance of <see cref="TypeInfo"/>.</param>
        /// <returns>A string in target language.</returns>
        string FormatTypeInfo(TypeInfo type);
        /// <summary>
        /// Format an instance of <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="eventInfo">The instance of <see cref="EventInfo"/>.</param>
        /// <returns>A string in target language.</returns>
        string FormatEventInfo(EventInfo eventInfo);
        /// <summary>
        /// Format an instance of <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method">The instance of <see cref="MethodInfo"/>.</param>
        /// <returns>A string in target language.</returns>
        string FormatMethodInfo(MethodInfo method);
    }
}
