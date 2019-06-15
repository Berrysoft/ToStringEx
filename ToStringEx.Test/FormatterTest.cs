using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToStringEx.Memory;

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
            Assert.AreEqual(
@"{{0A, 0B, 0C},
 {0D, 0E, 0F}}", a2.ToStringEx(new ArrayFormatter<int>(new FormattableFormatter<int>("X2"), true)));
            int[,,] a3 = new[, ,]
            { { { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 } },
              { { 5, 6, 7, 8, 9 },
                { 5, 6, 7, 8, 9 },
                { 5, 6, 7, 8, 9 },
                { 5, 6, 7, 8, 9 },
                { 5, 6, 7, 8, 9 },
                { 5, 6, 7, 8, 9 },
                { 5, 6, 7, 8, 9 } },
              { { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 } },
              { { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 } },
              { { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 },
                { 1, 2, 3, 4, 5 } } };
            Assert.AreEqual(
@"{{{1, 2, ... 5},
  {1, 2, ... 5},
  ...
  {1, 2, ... 5}},
 {{5, 6, ... 9},
  {5, 6, ... 9},
  ...
  {5, 6, ... 9}},
 ...
 {{1, 2, ... 5},
  {1, 2, ... 5},
  ...
  {1, 2, ... 5}}}", a3.ToStringEx(new ArrayFormatter<int>(true, 3)));
        }

        [TestMethod]
        public void EnumerableTest()
        {
            int[][] twodarr = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 7, 8, 9 } };
            Assert.AreEqual("{{1, 2, 3}, {4, 5, 6}, {7, 8, 9}}", twodarr.ToStringEx(new EnumerableFormatter<IEnumerable<int>>(new EnumerableFormatter<int>())));
            int[] longarr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Assert.AreEqual("{1, 2, 3, ... 10}", longarr.ToStringEx(new EnumerableFormatter<int>(4)));
            Assert.AreEqual("{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}", longarr.ToStringEx(new EnumerableFormatter<int>(10)));
        }

        [TestMethod]
        public void MemoryTest()
        {
            Memory<int> m = new int[] { 1, 2, 3, 4, 5 };
            Assert.AreEqual("[1, 2, 3, 4, 5]", m.ToStringEx(new MemoryFormatter<int>()));
            Assert.AreEqual("[1, 2, ... 5]", m.ToStringEx(new MemoryFormatter<int>(3)));
            ReadOnlyMemory<char> strmem = "Hello world!".AsMemory();
            Assert.AreEqual("Hello world!", strmem.ToStringEx(new ReadOnlyMemoryFormatter<char>()));
        }

        [TestMethod]
        public void MultiTest()
        {
            var f = new MultiFormatter(new EnumerableFormatter<int>(), new TupleFormatter(new FormattableFormatter<int>("X2")));
            Assert.AreEqual("(01, abc)", (1, "abc").ToStringEx(f));
            Assert.AreEqual("{1, 2, 3}", new int[] { 1, 2, 3 }.AsEnumerable().ToStringEx(f));
        }

        [TestMethod]
        public void ReflectionTest()
        {
            var c = new { Key1 = 123, Key2 = "abc" };
            Assert.AreEqual(
@"{
  Key1: 123,
  Key2: abc,
}", c.ToStringEx(new ReflectionFormatter(true)));
        }

        class UserDefinedFormatter : IFormatterEx<object>
        {
            public Type TargetType => typeof(object);

            public string Format(object value) => "Hello world!";
        }

        [DefaultFormatter(typeof(UserDefinedFormatter))]
        class UserDefinedTypeWithDefaultFormatter { }

        [TestMethod]
        public void DefaultFormatterAttributeTest()
        {
            Assert.AreEqual("Hello world!", new UserDefinedTypeWithDefaultFormatter().ToStringEx());
        }
    }
}
