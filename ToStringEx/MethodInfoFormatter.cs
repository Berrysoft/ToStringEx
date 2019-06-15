using System;
using System.Reflection;
using ToStringEx.MethodInfoHelpers;

namespace ToStringEx
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
        CppCli,
        /// <summary>
        /// C++/WinRT
        /// </summary>
        CppWinRT
    }

    /// <summary>
    /// Represents a formatter for <see cref="MethodInfo"/>.
    /// </summary>
    public class MethodInfoFormatter : IFormatterEx<MethodInfo>
    {
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
            switch (Language)
            {
                case MethodInfoFormatterLanguage.CSharp:
                    return CSharpMethodInfoFormatterHelper.FormatInternal(value);
                case MethodInfoFormatterLanguage.VisualBasic:
                    return VisualBasicMethodInfoFormatterHelper.FormatInternal(value);
                case MethodInfoFormatterLanguage.FSharp:
                    return FSharpMethodInfoFormatterHelper.FormatInternal(value);
                case MethodInfoFormatterLanguage.CppCli:
                    return CppMethodInfoFormatterHelper.FormatInternal(value, true);
                case MethodInfoFormatterLanguage.CppWinRT:
                    return CppMethodInfoFormatterHelper.FormatInternal(value, false);
                default:
                    throw new NotSupportedException();
            }
        }

        string IFormatterEx.Format(object value) => Format((MethodInfo)value);
    }
}
