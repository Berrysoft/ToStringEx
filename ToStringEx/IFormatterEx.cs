namespace ToStringEx
{
    public interface IFormatterEx<in T>
    {
        string Format(T value);
    }
}
