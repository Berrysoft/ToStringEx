Module Program
    Sub Main()
        Console.WriteLine(Enumerable.Range(0, 10).ToStringEx())
        Console.WriteLine((1, {{1, 2}, {3, 4}}).ToStringEx())
        Console.WriteLine("{0}", {&HA, &HB, &HC}.ToStringEx(New EnumerableFormatter(Of Integer)(New FormattableFormatter(Of Integer)("X2"))))
        Console.WriteLine(&HABC.ToStringEx(New FuncFormatter(Of Integer)(Function(i) $"0x{i:X}")))
    End Sub
End Module
