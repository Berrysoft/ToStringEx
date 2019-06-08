using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToStringEx.Test
{
    [TestClass]
    public class FormatterTest
    {
        class UserDefinedFormattable : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider)
                => "Hello world!";
        }

        class UserDefinedDrivedFormattable : UserDefinedFormattable { }

        [TestMethod]
        public void ConversionTest()
        {
            var f = new FormattableFormatter<UserDefinedFormattable>();
            IFormatterEx<UserDefinedDrivedFormattable> fobj = f;
            Assert.AreEqual("Hello world!", fobj.Format(new UserDefinedDrivedFormattable()));
        }

        [TestMethod]
        public void FormattableTest()
        {
            Assert.AreEqual("00ABCDEF", 0x00ABCDEF.ToStringEx(new FormattableFormatter<int>("X8")));
        }

        [TestMethod]
        public void FuncTest()
        {
            Assert.AreEqual("abc", 123.ToStringEx(new FuncFormatter<int>(_ => "abc")));
        }

        [TestMethod]
        public void ArrayTest()
        {
            int[,] a2 = new int[,] { { 0xA, 0xB, 0xC }, { 0xD, 0xE, 0xF } };
            Assert.AreEqual("{{0A, 0B, 0C}, {0D, 0E, 0F}}", a2.ToStringEx(new ArrayFormatter<int>(new FormattableFormatter<int>("X2"))));
        }

        [TestMethod]
        public void EnumerableTest()
        {
            int[][] twodarr = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 7, 8, 9 } };
            Assert.AreEqual("{{1, 2, 3}, {4, 5, 6}, {7, 8, 9}}", twodarr.ToStringEx(new EnumerableFormatter<IEnumerable<int>>(new EnumerableFormatter<int>())));
        }

        [TestMethod]
        public void MultiTest()
        {
            var f = new MultiFormatter(new EnumerableFormatter<int>(), new TupleFormatter(new FormattableFormatter<int>("X2")));
            Assert.AreEqual("(01, abc)", (1, "abc").ToStringEx(f));
            Assert.AreEqual("{1, 2, 3}", new int[] { 1, 2, 3 }.AsEnumerable().ToStringEx(f));
        }
    }
}
