using System;
using System.Reflection;

namespace ToStringEx.Reflection
{
    /// <summary>
    /// Represents a formatter for <see cref="EventInfo"/>.
    /// </summary>
    public class EventInfoFormatter : ReflectionFormatterBase, IFormatterEx<EventInfo>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EventInfoFormatter"/>.
        /// </summary>
        /// <param name="language">The target language.</param>
        public EventInfoFormatter(ReflectionFormatterLanguage language) : base(language) { }

        /// <inhertidoc/>
        public Type TargetType => typeof(EventInfo);

        /// <inhertidoc/>
        public string Format(EventInfo value)
        {
            if (LanguageHelpers.TryGetValue(Language, out ILanguageHelper helper))
            {
                return helper.FormatEventInfo(value);
            }
            else
            {
                return value.ToString();
            }
        }

        string IFormatterEx.Format(object value) => Format((EventInfo)value);
    }
}
