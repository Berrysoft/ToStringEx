using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToStringEx.Reflection;

namespace ToStringEx.Test
{
    [TestClass]
    public class MethodInfoTest
    {
        private static readonly IFormatterEx<MethodInfo> csFormatter = new MethodInfoFormatter(MethodInfoFormatterLanguage.CSharp);
        private static readonly IFormatterEx<MethodInfo> vbFormatter = new MethodInfoFormatter(MethodInfoFormatterLanguage.VisualBasic);
        private static readonly IFormatterEx<MethodInfo> fsFormatter = new MethodInfoFormatter(MethodInfoFormatterLanguage.FSharp);
        private static readonly IFormatterEx<MethodInfo> cppFormatter = new MethodInfoFormatter(MethodInfoFormatterLanguage.CppCli);
        private static readonly IFormatterEx<MethodInfo> cppwinrtFormatter = new MethodInfoFormatter(MethodInfoFormatterLanguage.CppWinRT);

        public int BasicMethod(double a) { throw null; }

        [TestMethod]
        public void BasicMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("BasicMethod");
            Assert.AreEqual("public int BasicMethod(double a)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function BasicMethod(a As Double) As Integer", m.ToStringEx(vbFormatter));
            Assert.AreEqual("let BasicMethod (a : double) : int", m.ToStringEx(fsFormatter));
            Assert.AreEqual("int BasicMethod(double a)", m.ToStringEx(cppFormatter));
            Assert.AreEqual("std::int32_t BasicMethod(double a)", m.ToStringEx(cppwinrtFormatter));
        }

        public void BasicVoidMethod() { }

        [TestMethod]
        public void BasicVoidTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("BasicVoidMethod");
            Assert.AreEqual("public void BasicVoidMethod()", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Sub BasicVoidMethod()", m.ToStringEx(vbFormatter));
            Assert.AreEqual("let BasicVoidMethod : void", m.ToStringEx(fsFormatter));
            Assert.AreEqual("void BasicVoidMethod()", m.ToStringEx(cppFormatter));
            Assert.AreEqual("void BasicVoidMethod()", m.ToStringEx(cppwinrtFormatter));
        }

        public ref string RefMethod(ref int i1, out int i2, in int i3) { throw null; }

        [TestMethod]
        public void RefMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("RefMethod");
            Assert.AreEqual("public ref string RefMethod(ref int i1, out int i2, in int i3)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public ByRef Function RefMethod(ByRef i1 As Integer, <Out> ByRef i2 As Integer, <In> ByRef i3 As Integer) As String", m.ToStringEx(vbFormatter));
            Assert.AreEqual("let RefMethod (i1 : byref<int>) (i2 : outref<int>) (i3 : inref<int>) : byref<string>", m.ToStringEx(fsFormatter));
            Assert.AreEqual("System::String^% RefMethod(int% i1, [Out] int% i2, [In] int% i3)", m.ToStringEx(cppFormatter));
            Assert.AreEqual("hstring& RefMethod(std::int32_t& i1, std::int32_t& i2, std::int32_t& i3)", m.ToStringEx(cppwinrtFormatter));
        }

        public decimal[] ArrayMethod(object[][] objs) { throw null; }

        [TestMethod]
        public void ArrayMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("ArrayMethod");
            Assert.AreEqual("public decimal[] ArrayMethod(object[][] objs)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function ArrayMethod(objs As Object()()) As Decimal()", m.ToStringEx(vbFormatter));
            Assert.AreEqual("let ArrayMethod (objs : System.Object[][]) : decimal[]", m.ToStringEx(fsFormatter));
            Assert.AreEqual("cli::array<System::Decimal>^ ArrayMethod(cli::array<cli::array<System::Object^>^>^ objs)", m.ToStringEx(cppFormatter));
            Assert.AreEqual("array_view<System::Decimal> ArrayMethod(array_view<array_view<Windows::Foundation::IInspectable>> objs)", m.ToStringEx(cppwinrtFormatter));
        }

        public unsafe void* PointerMethod(int* p) { throw null; }

        [TestMethod]
        public void PointerMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("PointerMethod");
            Assert.AreEqual("public unsafe void* PointerMethod(int* p)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function PointerMethod(p As Integer Pointer) As Pointer", m.ToStringEx(vbFormatter));
            Assert.AreEqual("let PointerMethod (p : int*) : void*", m.ToStringEx(fsFormatter));
            Assert.AreEqual("void* PointerMethod(int* p)", m.ToStringEx(cppFormatter));
            Assert.AreEqual("void* PointerMethod(std::int32_t* p)", m.ToStringEx(cppwinrtFormatter));
        }

        public void ParamArrayMethod(int a = 1, params string[] strs) { }

        [TestMethod]
        public void ParamArrayTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("ParamArrayMethod");
            Assert.AreEqual("public void ParamArrayMethod(int a = 1, params string[] strs)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Sub ParamArrayMethod(Optional a As Integer = 1, ParamArray strs As String())", m.ToStringEx(vbFormatter));
            Assert.AreEqual("let ParamArrayMethod ([<Optional; DefaultParameterValue(1)>] a : int) ([<ParamArray>] strs : string[]) : void", m.ToStringEx(fsFormatter));
            Assert.AreEqual("void ParamArrayMethod(int a = 1, ... cli::array<System::String^>^ strs)", m.ToStringEx(cppFormatter));
            Assert.AreEqual("void ParamArrayMethod(std::int32_t a = 1, ... array_view<hstring> strs)", m.ToStringEx(cppwinrtFormatter));
        }

        public void VarArgMethod(string fmt, __arglist) { }

        [TestMethod]
        public void VarArgTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("VarArgMethod");
            Assert.AreEqual("public void VarArgMethod(string fmt, __arglist)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Sub VarArgMethod(fmt As String, ParamArray)", m.ToStringEx(vbFormatter));
            Assert.AreEqual("let VarArgMethod (fmt : string) : void", m.ToStringEx(fsFormatter));
            Assert.AreEqual("void VarArgMethod(System::String^ fmt, ...)", m.ToStringEx(cppFormatter));
            Assert.AreEqual("void VarArgMethod(hstring const& fmt, ...)", m.ToStringEx(cppwinrtFormatter));
        }
    }
}
