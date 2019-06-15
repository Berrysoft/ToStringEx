﻿using System;
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
        FSharp,
        /// <summary>
        /// C++/CLI
        /// </summary>
        CppCli,
        CppWinRT,
        IronPython
    }

    public class MethodInfoFormatter : IFormatterEx<MethodInfo>
    {
        public MethodInfoFormatterLanguage Language { get; }

        public MethodInfoFormatter(MethodInfoFormatterLanguage language) => Language = language;

        public Type TargetType => typeof(MethodInfo);

        public string Format(MethodInfo value)
        {
            switch (Language)
            {
                case MethodInfoFormatterLanguage.CSharp:
                    return CSharpMethodInfoFormatterHelper.FormatInternal(value);
                case MethodInfoFormatterLanguage.VisualBasic:
                    return VisualBasicMethodInfoFormatterHelper.FormatInternal(value);
                case MethodInfoFormatterLanguage.CppCli:
                    return CppCliMethodInfoFormattersHelper.FormatInternal(value);
                default:
                    throw new NotSupportedException();
            }
        }

        string IFormatterEx.Format(object value) => Format((MethodInfo)value);
    }
}
