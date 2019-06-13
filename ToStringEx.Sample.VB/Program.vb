Module Program
    Sub Main()
        ToStringExtensions.FormatterProviders.Add(TupleDefaultFormatterProvider.Instance)
        ToStringExtensions.FormatterProviders.Add(EnumerableDefaultFormatterProvider.Instance)
        Console.WriteLine(Enumerable.Range(0, 10).ToStringEx())
        Console.WriteLine((1, {{1, 2}, {3, 4}}).ToStringEx())
        Console.WriteLine({&HA, &HB, &HC}.ToStringEx(New EnumerableFormatter(Of Integer)(New FormattableFormatter(Of Integer)("X2"))))
        Console.WriteLine(&HABC.ToStringEx(New FuncFormatter(Of Integer)(Function(i) $"0x{i:X}")))
    End Sub
End Module
