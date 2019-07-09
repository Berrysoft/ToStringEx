# ToStringEx
An extension for ToString in .NET Standard.

[![Azure DevOps builds](https://strawberry-vs.visualstudio.com/ToStringEx/_apis/build/status/Berrysoft.ToStringEx?branch=master)](https://strawberry-vs.visualstudio.com/ToStringEx/_build?definitionId=3)

|Name|NuGet|
|-|-|
|ToStringEx|[![Nuget](https://img.shields.io/nuget/v/ToStringEx.svg)](https://www.nuget.org/packages/ToStringEx/)|
|ToStringEx.Memory|[![Nuget](https://img.shields.io/nuget/v/ToStringEx.Memory.svg)](https://www.nuget.org/packages/ToStringEx.Memory/)|
|ToStringEx.Reflection|[![Nuget](https://img.shields.io/nuget/v/ToStringEx.Reflection.svg)](https://www.nuget.org/packages/ToStringEx.Reflection/)|
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
