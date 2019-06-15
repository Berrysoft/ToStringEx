using System;

namespace ToStringEx
{
    /// <summary>
    /// Represents a default formatter provider with reflection.
    /// </summary>
    public class ReflectionDefaultFormatterProvider : IFormatterProviderEx
    {
        private static readonly ReflectionDefaultFormatterProvider instance = new ReflectionDefaultFormatterProvider();

        /// <summary>
        /// Gets the singleton instance of the provider.
        /// </summary>
        public static IFormatterProviderEx Instance => instance;

        /// <inhertidoc/>
        public bool TryGetProvider(Type t, out IFormatterEx formatter)
        {
            if (t.GetMethod("ToString", Array.Empty<Type>()).DeclaringType == t)
            {
                formatter = new FuncFormatter<object>(obj => obj.ToString());
            }
            else if (Attribute.GetCustomAttribute(t, typeof(DefaultFormatterAttribute)) is DefaultFormatterAttribute attr)
            {
                formatter = (IFormatterEx)Activator.CreateInstance(attr.FormatterType);
            }
            else
            {
                formatter = new ReflectionFormatter();
            }
            return true;
        }
    }
}
