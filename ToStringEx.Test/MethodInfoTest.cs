using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToStringEx.Reflection;

namespace ToStringEx.Test
{
    [TestClass]
    public class MethodInfoTest
    {
        private static readonly IFormatterEx<MethodInfo> csFormatter = new MethodInfoFormatter(ReflectionFormatterLanguage.CSharp);
        private static readonly IFormatterEx<MethodInfo> vbFormatter = new MethodInfoFormatter(ReflectionFormatterLanguage.VisualBasic);
        private static readonly IFormatterEx<MethodInfo> fsFormatter = new MethodInfoFormatter(ReflectionFormatterLanguage.FSharp);
        private static readonly IFormatterEx<MethodInfo> cppFormatter = new MethodInfoFormatter(ReflectionFormatterLanguage.CppCli);

        public int BasicMethod(double a) { throw null; }

        [TestMethod]
        public void BasicMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("BasicMethod");
            Assert.AreEqual("public int BasicMethod(double a)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function BasicMethod(a As Double) As Integer", m.ToStringEx(vbFormatter));
            Assert.AreEqual("member BasicMethod (a : double) : int", m.ToStringEx(fsFormatter));
            Assert.AreEqual("int BasicMethod(double a)", m.ToStringEx(cppFormatter));
        }

        public void BasicVoidMethod() { }

        [TestMethod]
        public void BasicVoidTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("BasicVoidMethod");
            Assert.AreEqual("public void BasicVoidMethod()", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Sub BasicVoidMethod()", m.ToStringEx(vbFormatter));
            Assert.AreEqual("member BasicVoidMethod : void", m.ToStringEx(fsFormatter));
            Assert.AreEqual("void BasicVoidMethod()", m.ToStringEx(cppFormatter));
        }

        public ref string RefMethod(ref int i1, out int i2, in int i3) { throw null; }

        [TestMethod]
        public void RefMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("RefMethod");
            Assert.AreEqual("public ref string RefMethod(ref int i1, out int i2, in int i3)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public ByRef Function RefMethod(ByRef i1 As Integer, <Out> ByRef i2 As Integer, <In> ByRef i3 As Integer) As String", m.ToStringEx(vbFormatter));
            Assert.AreEqual("member RefMethod (i1 : byref<int>) (i2 : outref<int>) (i3 : inref<int>) : byref<string>", m.ToStringEx(fsFormatter));
            Assert.AreEqual("System::String^% RefMethod(int% i1, [Out] int% i2, [In] int% i3)", m.ToStringEx(cppFormatter));
        }

        public decimal[] ArrayMethod(object[][] objs) { throw null; }

        [TestMethod]
        public void ArrayMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("ArrayMethod");
            Assert.AreEqual("public decimal[] ArrayMethod(object[][] objs)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function ArrayMethod(objs As Object()()) As Decimal()", m.ToStringEx(vbFormatter));
            Assert.AreEqual("member ArrayMethod (objs : System.Object[][]) : decimal[]", m.ToStringEx(fsFormatter));
            Assert.AreEqual("cli::array<System::Decimal>^ ArrayMethod(cli::array<cli::array<System::Object^>^>^ objs)", m.ToStringEx(cppFormatter));
        }

        public unsafe void* PointerMethod(int* p) { throw null; }

        [TestMethod]
        public void PointerMethodTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("PointerMethod");
            Assert.AreEqual("public unsafe void* PointerMethod(int* p)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function PointerMethod(p As Integer Pointer) As Pointer", m.ToStringEx(vbFormatter));
            Assert.AreEqual("member PointerMethod (p : nativeptr<int>) : nativeint", m.ToStringEx(fsFormatter));
            Assert.AreEqual("void* PointerMethod(int* p)", m.ToStringEx(cppFormatter));
        }

        public void ParamArrayMethod(int a = 1, params string[] strs) { }

        [TestMethod]
        public void ParamArrayTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("ParamArrayMethod");
            Assert.AreEqual("public void ParamArrayMethod(int a = 1, params string[] strs)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Sub ParamArrayMethod(Optional a As Integer = 1, ParamArray strs As String())", m.ToStringEx(vbFormatter));
            Assert.AreEqual("member ParamArrayMethod ([<Optional; DefaultParameterValue(1)>] a : int) ([<ParamArray>] strs : string[]) : void", m.ToStringEx(fsFormatter));
            Assert.AreEqual("void ParamArrayMethod(int a = 1, ... cli::array<System::String^>^ strs)", m.ToStringEx(cppFormatter));
        }

        public void VarArgMethod(string fmt, __arglist) { }

        [TestMethod]
        public void VarArgTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethod("VarArgMethod");
            Assert.AreEqual("public void VarArgMethod(string fmt, __arglist)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Sub VarArgMethod(fmt As String)", m.ToStringEx(vbFormatter));
            Assert.AreEqual("member VarArgMethod (fmt : string) : void", m.ToStringEx(fsFormatter));
            Assert.AreEqual("void VarArgMethod(System::String^ fmt, ...)", m.ToStringEx(cppFormatter));
        }

        abstract class UserDefinedClass
        {
            public virtual void VirtualMethod1() { }
            public virtual void VirtualMethod2() { }
            public abstract void AbstractMethod();
        }

        class UserDerivedClass : UserDefinedClass
        {
            public override void VirtualMethod1() { }
            public sealed override void VirtualMethod2() { }
            public override void AbstractMethod() { }
        }

        [TestMethod]
        public void VirtualTest()
        {
            MethodInfo vm1 = typeof(UserDefinedClass).GetMethod("VirtualMethod1");
            Assert.AreEqual("public virtual void VirtualMethod1()", vm1.ToStringEx(csFormatter));
            Assert.AreEqual("Public Overridable Sub VirtualMethod1()", vm1.ToStringEx(vbFormatter));
            Assert.AreEqual("abstract member VirtualMethod1 : void", vm1.ToStringEx(fsFormatter));
            Assert.AreEqual("virtual void VirtualMethod1()", vm1.ToStringEx(cppFormatter));
            MethodInfo am = typeof(UserDefinedClass).GetMethod("AbstractMethod");
            Assert.AreEqual("public abstract void AbstractMethod()", am.ToStringEx(csFormatter));
            Assert.AreEqual("Public MustOverride Sub AbstractMethod()", am.ToStringEx(vbFormatter));
            Assert.AreEqual("abstract member AbstractMethod : void", am.ToStringEx(fsFormatter));
            Assert.AreEqual("virtual void AbstractMethod() abstract", am.ToStringEx(cppFormatter));
            MethodInfo dvm1 = typeof(UserDerivedClass).GetMethod("VirtualMethod1");
            Assert.AreEqual("public override void VirtualMethod1()", dvm1.ToStringEx(csFormatter));
            Assert.AreEqual("Public Overrides Sub VirtualMethod1()", dvm1.ToStringEx(vbFormatter));
            Assert.AreEqual("override this.VirtualMethod1 : void", dvm1.ToStringEx(fsFormatter));
            Assert.AreEqual("virtual void VirtualMethod1() override", dvm1.ToStringEx(cppFormatter));
            MethodInfo dvm2 = typeof(UserDerivedClass).GetMethod("VirtualMethod2");
            Assert.AreEqual("public sealed override void VirtualMethod2()", dvm2.ToStringEx(csFormatter));
            Assert.AreEqual("Public NotOverridable Overrides Sub VirtualMethod2()", dvm2.ToStringEx(vbFormatter));
            Assert.AreEqual("override this.VirtualMethod2 : void", dvm2.ToStringEx(fsFormatter));
            Assert.AreEqual("virtual void VirtualMethod2() override final", dvm2.ToStringEx(cppFormatter));
            MethodInfo dam = typeof(UserDerivedClass).GetMethod("AbstractMethod");
            Assert.AreEqual("public override void AbstractMethod()", dam.ToStringEx(csFormatter));
            Assert.AreEqual("Public Overrides Sub AbstractMethod()", dam.ToStringEx(vbFormatter));
            Assert.AreEqual("override this.AbstractMethod : void", dam.ToStringEx(fsFormatter));
            Assert.AreEqual("virtual void AbstractMethod() override", dam.ToStringEx(cppFormatter));
        }

        public T GenericMethod<T>(IEnumerable<T> source) { return default; }

        [TestMethod]
        public void GenericTest()
        {
            MethodInfo m = typeof(MethodInfoTest).GetMethods().First(m => m.Name.StartsWith("GenericMethod"));
            Assert.AreEqual("public T GenericMethod<T>(System.Collections.Generic.IEnumerable<T> source)", m.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function GenericMethod(Of T)(source As System.Collections.Generic.IEnumerable(Of T)) As T", m.ToStringEx(vbFormatter));
            Assert.AreEqual("member GenericMethod<'T> (source : System.Collections.Generic.IEnumerable<'T>) : 'T", m.ToStringEx(fsFormatter));
            Assert.AreEqual("generic <typename T> T GenericMethod(System::Collections::Generic::IEnumerable<T> source)", m.ToStringEx(cppFormatter));
            MethodInfo intm = m.MakeGenericMethod(typeof(int));
            Assert.AreEqual("public int GenericMethod<int>(System.Collections.Generic.IEnumerable<int> source)", intm.ToStringEx(csFormatter));
            Assert.AreEqual("Public Function GenericMethod(Of Integer)(source As System.Collections.Generic.IEnumerable(Of Integer)) As Integer", intm.ToStringEx(vbFormatter));
            Assert.AreEqual("member GenericMethod (source : System.Collections.Generic.IEnumerable<int>) : int", intm.ToStringEx(fsFormatter));
            Assert.AreEqual("generic <> int GenericMethod<int>(System::Collections::Generic::IEnumerable<int> source)", intm.ToStringEx(cppFormatter));
        }
    }
}
