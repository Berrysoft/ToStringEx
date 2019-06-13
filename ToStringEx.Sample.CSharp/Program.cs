using System;
using System.Linq;

namespace ToStringEx.Sample.CSharp
{
    class Program
    {
        static void Main()
        {
            ToStringExtensions.FormatterProviders.Add(TupleDefaultFormatterProvider.Instance);
            ToStringExtensions.FormatterProviders.Add(EnumerableDefaultFormatterProvider.Instance);
            Console.WriteLine(Enumerable.Range(0, 10).ToStringEx());
            Console.WriteLine((1, new int[,] { { 1, 2 }, { 3, 4 } }).ToStringEx());
            Console.WriteLine(new int[] { 0xA, 0xB, 0xC }.ToStringEx(new EnumerableFormatter<int>(new FormattableFormatter<int>("X2"))));
            Console.WriteLine(0xABC.ToStringEx(new FuncFormatter<int>(i => $"0x{i:X}")));
        }
    }
}
