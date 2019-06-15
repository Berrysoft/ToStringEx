using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToStringEx.Test
{
    [TestClass]
    public class MethodInfoTest
    {
        private static readonly IFormatterEx<MethodInfo> csFormatter = new MethodInfoFormatter(MethodInfoFormatterLanguage.CSharp);
        private static readonly IFormatterEx<MethodInfo> vbFormatter = new MethodInfoFormatter(MethodInfoFormatterLanguage.VisualBasic);
        private static readonly IFormatterEx<MethodInfo> cppFormatter = new MethodInfoFormatter(MethodInfoFormatterLanguage.CppCli);

        public int BasicMethod(double a) { throw null; }

        [TestMethod]
        public void BasicMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("BasicMethod");
            Assert.AreEqual("public int BasicMethod(double a)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function BasicMethod(a As Double) As Integer", m.ToStringEx(vbFormatter));
            Assert.AreEqual("int BasicMethod(double a)", m.ToStringEx(cppFormatter));
        }

        public void BasicVoidMethod() { }

        [TestMethod]
        public void BasicVoidTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("BasicVoidMethod");
            Assert.AreEqual("public void BasicVoidMethod()", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Sub BasicVoidMethod()", m.ToStringEx(vbFormatter));
            Assert.AreEqual("void BasicVoidMethod()", m.ToStringEx(cppFormatter));
        }

        public ref string RefMethod(ref int i1, out int i2, in int i3) { throw null; }

        [TestMethod]
        public void RefMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("RefMethod");
            Assert.AreEqual("public ref string RefMethod(ref int i1, out int i2, in int i3)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public ByRef Function RefMethod(ByRef i1 As Integer, <Out> ByRef i2 As Integer, <In> ByRef i3 As Integer) As String", m.ToStringEx(vbFormatter));
            Assert.AreEqual("System::String^% RefMethod(int% i1, [OutAttribute] int% i2, [InAttribute] int% i3)", m.ToStringEx(cppFormatter));
        }
    }
}
