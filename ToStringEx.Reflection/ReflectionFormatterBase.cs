using System.Collections.Generic;

namespace ToStringEx.Reflection
{
    /// <summary>
    /// Target language for formatters.
    /// </summary>
    public enum ReflectionFormatterLanguage
    {
        /// <summary>
        /// C#
        /// </summary>
        CSharp,
        /// <summary>
        /// Visual Basic
        /// </summary>
        VisualBasic,
        /// <summary>
        /// C++/CLI
        /// </summary>
        CppCli
    }

    /// <summary>
    /// Represents a base formatter for reflection formatters.
    /// </summary>
    public abstract class ReflectionFormatterBase
    {
        /// <summary>
        /// A map from language to helper.
        /// </summary>
        protected static readonly Dictionary<ReflectionFormatterLanguage, ILanguageHelper> LanguageHelpers = new Dictionary<ReflectionFormatterLanguage, ILanguageHelper>
        {
            [ReflectionFormatterLanguage.CSharp] = new CSharpHelper(),
            [ReflectionFormatterLanguage.VisualBasic] = new VisualBasicHelper(),
            [ReflectionFormatterLanguage.CppCli] = new CppHelper()
        };

        /// <summary>
        /// The target language.
        /// </summary>
        public ReflectionFormatterLanguage Language { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ReflectionFormatterBase"/>.
        /// </summary>
        /// <param name="language">The target language.</param>
        public ReflectionFormatterBase(ReflectionFormatterLanguage language) => Language = language;
    }
}
