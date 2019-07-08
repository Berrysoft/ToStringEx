using System;
using System.Reflection;

namespace ToStringEx.Reflection
{
    /// <summary>
    /// Represents a default formatter provider for <see cref="Type"/>, <see cref="TypeInfo"/>, <see cref="EventInfo"/> and <see cref="MethodInfo"/>.
    /// </summary>
    public class LanguageReflectionFormatterProvider : IFormatterProviderEx
    {
        private readonly ReflectionFormatterLanguage language;

        private LanguageReflectionFormatterProvider(ReflectionFormatterLanguage language) => this.language = language;

        private static readonly LanguageReflectionFormatterProvider csInstance = new LanguageReflectionFormatterProvider(ReflectionFormatterLanguage.CSharp);

        /// <summary>
        /// Gets the singleton instance of the provider.
        /// </summary>
        public static IFormatterProviderEx CSharpInstance => csInstance;

        /// <inhertidoc/>
        public bool TryGetProvider(Type t, out IFormatterEx formatter)
        {
            if (t == typeof(Type))
            {
                formatter = new TypeFormatter(language);
                return true;
            }
            else if (t == typeof(TypeInfo))
            {
                formatter = new TypeInfoFormatter(language);
                return true;
            }
            else if (t == typeof(EventInfo))
            {
                formatter = new EventInfoFormatter(language);
                return true;
            }
            else if (t == typeof(MethodInfo))
            {
                formatter = new MethodInfoFormatter(language);
                return true;
            }
            else
            {
                formatter = null;
                return false;
            }
        }
    }
}
