using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToStringEx.Test
{
    [TestClass]
    public class MethodInfoTest
    {
        public int BasicMethod(double a) { throw null; }

        [TestMethod]
        public void BasicMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("BasicMethod");
            Assert.AreEqual("public int BasicMethod(double a)", m.ToStringEx(new MethodInfoFormatter(MethodInfoFormatterLanguage.CSharp)));
            Assert.AreEqual("Public Function BasicMethod(a As Double) As Integer", m.ToStringEx(new MethodInfoFormatter(MethodInfoFormatterLanguage.VisualBasic)));
        }

        public ref string RefMethod(ref int i1, out int i2, in int i3) { throw null; }

        [TestMethod]
        public void RefMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("RefMethod");
            Assert.AreEqual("public ref string RefMethod(ref int i1, out int i2, in int i3)", m.ToStringEx(new MethodInfoFormatter(MethodInfoFormatterLanguage.CSharp)));
        }
    }
}
