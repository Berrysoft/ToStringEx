using System.Reflection;

namespace ToStringEx.Reflection
{
    public interface ILanguageHelper
    {
        string FormatMethodInfo(MethodInfo method);
    }
}
