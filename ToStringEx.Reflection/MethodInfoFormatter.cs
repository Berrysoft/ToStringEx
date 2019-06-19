using System;
using System.Reflection;

namespace ToStringEx.Reflection
{
    /// <summary>
    /// Represents a formatter for <see cref="MethodInfo"/>.
    /// </summary>
    public class MethodInfoFormatter : ReflectionFormatterBase, IFormatterEx<MethodInfo>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MethodInfoFormatter"/>.
        /// </summary>
        /// <param name="language">The target language.</param>
        public MethodInfoFormatter(ReflectionFormatterLanguage language) : base(language) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(MethodInfo);

        /// <inhertidoc/>
        public string Format(MethodInfo value)
        {
            if (LanguageHelpers.TryGetValue(Language, out ILanguageHelper helper))
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
