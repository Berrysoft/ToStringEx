namespace ToStringEx
{
    public struct ToStringExBox
    {
        public static ToStringExBox<T> Create<T>(T data) => new ToStringExBox<T> { Data = data };
        public static ToStringExBox<T> Create<T>(T data, IFormatterEx<T> formatter) => new ToStringExBox<T> { Data = data, Formatter = formatter };
    }

    public struct ToStringExBox<T>
    {
        public T Data;
        public IFormatterEx<T> Formatter;

        public override string ToString() => Data.ToStringEx(Formatter);
    }
}
