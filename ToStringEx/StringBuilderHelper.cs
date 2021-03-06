﻿using System.Collections.Generic;
using System.Text;

namespace ToStringEx
{
#if NETSTANDARD2_0
    internal static class StringBuilderHelper
    {
        public static StringBuilder AppendJoin(this StringBuilder builder, string separater, IEnumerable<string> values)
        {
            using (var e = values.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    builder.Append(e.Current);
                    while (e.MoveNext())
                    {
                        builder.Append(separater);
                        builder.Append(e.Current);
                    }
                }
                return builder;
            }
        }
    }
#endif
}
