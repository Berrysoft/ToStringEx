using System;
using System.Collections.Generic;
using System.Reflection;

namespace ToStringEx.Reflection
{
    /// <summary>
    /// Target language for <see cref="MethodInfoFormatter"/>.
    /// </summary>
    public enum MethodInfoFormatterLanguage
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
        /// F#
        /// </summary>
        FSharp,
        /// <summary>
        /// C++/CLI
        /// </summary>
        CppCli
    }

    /// <summary>
    /// Represents a formatter for <see cref="MethodInfo"/>.
    /// </summary>
    public class MethodInfoFormatter : IFormatterEx<MethodInfo>
    {
        private static readonly Dictionary<MethodInfoFormatterLanguage, ILanguageHelper> languageHelpers = new Dictionary<MethodInfoFormatterLanguage, ILanguageHelper>
        {
            [MethodInfoFormatterLanguage.CSharp] = new CSharpHelper(),
            [MethodInfoFormatterLanguage.VisualBasic] = new VisualBasicHelper(),
            [MethodInfoFormatterLanguage.FSharp] = new FSharpHelper(),
            [MethodInfoFormatterLanguage.CppCli] = new CppHelper()
        };

        /// <summary>
        /// The target language.
        /// </summary>
        public MethodInfoFormatterLanguage Language { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="MethodInfoFormatter"/>.
        /// </summary>
        /// <param name="language">The target language.</param>
        public MethodInfoFormatter(MethodInfoFormatterLanguage language) => Language = language;

        /// <inhertidoc/>
        public Type TargetType => typeof(MethodInfo);

        /// <inhertidoc/>
        public string Format(MethodInfo value)
        {
            if (languageHelpers.TryGetValue(Language, out ILanguageHelper helper))
            {
                return helper.FormatMethodInfo(value);
            }
            else
            {
                return value.ToString();
            }
        }

        string IFormatterEx.Format(object value) => Format((MethodInfo)value);
    }
}
