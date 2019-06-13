# ToStringEx
An extension for ToString in .NET Standard.

|Name|NuGet (preview)|
|-|-|
|ToStringEx|[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ToStringEx.svg)](https://www.nuget.org/packages/ToStringEx/)|
|ToStringEx.Memory|[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ToStringEx.Memory.svg)](https://www.nuget.org/packages/ToStringEx.Memory/)|
## Usage
C#:
``` csharp
ToStringExtensions.FormatterProviders.Add(TupleDefaultFormatterProvider.Instance);
ToStringExtensions.FormatterProviders.Add(EnumerableDefaultFormatterProvider.Instance);
Console.WriteLine(Enumerable.Range(0, 10).ToStringEx());
Console.WriteLine((1, new int[,] { { 1, 2 }, { 3, 4 } }).ToStringEx());
Console.WriteLine(new int[] { 0xA, 0xB, 0xC }.ToStringEx(new EnumerableFormatter<int>(new FormattableFormatter<int>("X2"))));
Console.WriteLine(0xABC.ToStringEx(new FuncFormatter<int>(i => $"0x{i:X}")));
```
VB:
``` vb.net
ToStringExtensions.FormatterProviders.Add(TupleDefaultFormatterProvider.Instance)
ToStringExtensions.FormatterProviders.Add(EnumerableDefaultFormatterProvider.Instance)
Console.WriteLine(Enumerable.Range(0, 10).ToStringEx())
Console.WriteLine((1, {{1, 2}, {3, 4}}).ToStringEx())
Console.WriteLine({&HA, &HB, &HC}.ToStringEx(New EnumerableFormatter(Of Integer)(New FormattableFormatter(Of Integer)("X2"))))
Console.WriteLine(&HABC.ToStringEx(New FuncFormatter(Of Integer)(Function(i) $"0x{i:X}")))
```
Output:
```
{0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
(1, {{1, 2}, {3, 4}})
{0A, 0B, 0C}
0xABC
```
