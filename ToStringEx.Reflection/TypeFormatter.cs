using System;
using System.Reflection;

namespace ToStringEx.Reflection
{
    /// <summary>
    /// Represents a formatter for <see cref="Type"/>.
    /// </summary>
    public class TypeFormatter : ReflectionFormatterBase, IFormatterEx<Type>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeFormatter"/>.
        /// </summary>
        /// <param name="language">The target language.</param>
        public TypeFormatter(ReflectionFormatterLanguage language) : base(language) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(Type);

        /// <inhertidoc/>
        public string Format(Type value)
        {
            if (LanguageHelpers.TryGetValue(Language, out ILanguageHelper helper))
            {
                return helper.FormatType(value);
            }
            else
            {
                return value.ToString();
            }
        }

        string IFormatterEx.Format(object value) => Format((Type)value);
    }

    /// <summary>
    /// Represents a formatter for <see cref="TypeInfo"/>.
    /// </summary>
    public class TypeInfoFormatter : ReflectionFormatterBase, IFormatterEx<TypeInfo>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeInfoFormatter"/>.
        /// </summary>
        /// <param name="language">The target language.</param>
        public TypeInfoFormatter(ReflectionFormatterLanguage language) : base(language) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(TypeInfo);

        /// <inhertidoc/>
        public string Format(TypeInfo value)
        {
            if (LanguageHelpers.TryGetValue(Language, out ILanguageHelper helper))
            {
                return helper.FormatTypeInfo(value);
            }
            else
            {
                return value.ToString();
            }
        }

        string IFormatterEx.Format(object value) => Format((TypeInfo)value);
    }
}
