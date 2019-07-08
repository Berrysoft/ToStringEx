using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToStringEx.Reflection;

namespace ToStringEx.Test
{
    [TestClass]
    public class TypeTest
    {
        private static readonly IFormatterEx<Type> csFormatter = new TypeFormatter(ReflectionFormatterLanguage.CSharp);
        private static readonly IFormatterEx<Type> vbFormatter = new TypeFormatter(ReflectionFormatterLanguage.VisualBasic);
        private static readonly IFormatterEx<Type> cppFormatter = new TypeFormatter(ReflectionFormatterLanguage.CppCli);

        [TestMethod]
        public void BasicTest()
        {
            Type intt = typeof(int);
            Assert.AreEqual("int", intt.ToStringEx(csFormatter));
            Assert.AreEqual("Integer", intt.ToStringEx(vbFormatter));
            Assert.AreEqual("int", intt.ToStringEx(cppFormatter));
            Type tuplet = typeof(Tuple);
            Assert.AreEqual("System.Tuple", tuplet.ToStringEx(csFormatter));
            Assert.AreEqual("System.Tuple", tuplet.ToStringEx(vbFormatter));
            Assert.AreEqual("System::Tuple^", tuplet.ToStringEx(cppFormatter));
        }

        [TestMethod]
        public void PointerTest()
        {
            Type t = typeof(int*);
            Assert.AreEqual("int*", t.ToStringEx(csFormatter));
            Assert.AreEqual("Integer Pointer", t.ToStringEx(vbFormatter));
            Assert.AreEqual("int*", t.ToStringEx(cppFormatter));
            Type vt = typeof(void*);
            Assert.AreEqual("void*", vt.ToStringEx(csFormatter));
            Assert.AreEqual("Pointer", vt.ToStringEx(vbFormatter));
            Assert.AreEqual("void*", vt.ToStringEx(cppFormatter));
        }

        [TestMethod]
        public void GenericTest()
        {
            Type t = typeof(Tuple<int>);
            Assert.AreEqual("System.Tuple<int>", t.ToStringEx(csFormatter));
            Assert.AreEqual("System.Tuple(Of Integer)", t.ToStringEx(vbFormatter));
            Assert.AreEqual("System::Tuple<int>^", t.ToStringEx(cppFormatter));
        }
    }
}
