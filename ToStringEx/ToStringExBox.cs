namespace ToStringEx
{
    public struct ToStringExBox
    {
        public static ToStringExBox<T> Create<T>(T data) => new ToStringExBox<T> { Data = data };
    }

    public struct ToStringExBox<T>
    {
        public T Data;

        public override string ToString() => Data.ToStringEx();
    }
}
