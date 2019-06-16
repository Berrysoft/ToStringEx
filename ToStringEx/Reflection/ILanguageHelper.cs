using System.Reflection;

namespace ToStringEx.Reflection
{
    public interface ILanguageHelper
    {
        string Language { get; }
        string FormatMethodInfo(MethodInfo method);
    }
}
