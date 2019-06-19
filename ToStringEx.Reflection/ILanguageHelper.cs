using System.Reflection;

namespace ToStringEx.Reflection
{
    /// <summary>
    /// Exposes a set of method to format reflection metadata to target language.
    /// </summary>
    public interface ILanguageHelper
    {
        /// <summary>
        /// Format an instance of <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method">The instance of <see cref="MethodInfo"/>.</param>
        /// <returns>A string in target language.</returns>
        string FormatMethodInfo(MethodInfo method);
    }
}
