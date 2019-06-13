using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToStringEx.Memory;

namespace ToStringEx.Test
{
    [TestClass]
    public class ExtensionTest
    {
        [TestInitialize]
        public void InitializeComponent()
        {
            ToStringExtensions.FormatterProviders.Add(TupleDefaultFormatterProvider.Instance);
            ToStringExtensions.FormatterProviders.Add(EnumerableDefaultFormatterProvider.Instance);
            ToStringExtensions.FormatterProviders.Add(MemoryDefaultFormatterProvider.Instance);
        }

        [TestMethod]
        public void StringToString()
        {
            string a = "Hello world!";
            Assert.AreEqual(a, a.ToStringEx());
            char[] arr = a.ToCharArray();
            Assert.AreEqual(a, arr.ToStringEx());
        }

        [TestMethod]
        public void TupleToString()
        {
            (int, int, string) t = (1, 2, "abc");
            Assert.AreEqual("(1, 2, abc)", t.ToStringEx());
            (int, int[], string) t2 = (1, new int[] { 1, 2, 3 }, "abc");
            Assert.AreEqual("(1, {1, 2, 3}, abc)", t2.ToStringEx());
            var t3 = Tuple.Create(1, 2, "abc");
            Assert.AreEqual(t.ToStringEx(), t3.ToStringEx());
        }

        [TestMethod]
        public void HighRankArrayToString()
        {
            int[,] a2 = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            Assert.AreEqual("{{1, 2, 3}, {4, 5, 6}}", a2.ToStringEx());
            int[,,] a3 = new int[,,] { { { 1, 2, 3 }, { 4, 5, 6 } }, { { 7, 8, 9 }, { 10, 11, 12 } } };
            Assert.AreEqual("{{{1, 2, 3}, {4, 5, 6}}, {{7, 8, 9}, {10, 11, 12}}}", a3.ToStringEx());
        }

        [TestMethod]
        public void IEnumerableToString()
        {
            int[] a = new int[] { 1, 2, 3, 4, 5 };
            Assert.AreEqual("{1, 2, 3, 4, 5}", a.ToStringEx());
            var q = a.Where(i => i % 2 == 0);
            Assert.AreEqual("{2, 4}", q.ToStringEx());
        }

        [TestMethod]
        public void MemoryToString()
        {
            Memory<int> m = new int[] { 1, 2, 3, 4, 5 };
            Assert.AreEqual("[1, 2, 3, 4, 5]", m.ToStringEx());
            ReadOnlyMemory<int> rdm = m;
            Assert.AreEqual("[1, 2, 3, 4, 5]", rdm.ToStringEx());
            Span<int> a = m.Span;
            Assert.AreEqual("[1, 2, 3, 4, 5]", a.ToStringEx());
        }

        class UserDefinedType
        {
            public int Key1 { get; set; }
            public string Key2 { get; set; }
            public int[] Key3 { get; set; }
        }
        class UserDefinedTypeWithToString
        {
            public int Key { get; set; }
            public override string ToString() => "Hello world!";
        }

        [TestMethod]
        public void UserTypeToString()
        {
            var c = new UserDefinedType { Key1 = 123, Key2 = "Hello", Key3 = new int[] { 1, 2, 3 } };
            Assert.AreEqual("{ Key1: 123, Key2: Hello, Key3: {1, 2, 3} }", c.ToStringEx());
            var c2 = new UserDefinedTypeWithToString { Key = 123 };
            Assert.AreEqual("Hello world!", c2.ToStringEx());
        }
    }
}
