using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToStringEx.Reflection;

namespace ToStringEx.Test
{
    [TestClass]
    public class EventInfoTest
    {
        private static readonly IFormatterEx<EventInfo> csFormatter = new EventInfoFormatter(ReflectionFormatterLanguage.CSharp);
        private static readonly IFormatterEx<EventInfo> vbFormatter = new EventInfoFormatter(ReflectionFormatterLanguage.VisualBasic);
        private static readonly IFormatterEx<EventInfo> cppFormatter = new EventInfoFormatter(ReflectionFormatterLanguage.CppCli);

        public event EventHandler<int> IntEvent;

        [TestMethod]
        public void PreDefinedTypeTest()
        {
            EventInfo e = typeof(EventInfoTest).GetEvent("IntEvent");
            Assert.AreEqual("event System.EventHandler<int> IntEvent", e.ToStringEx(csFormatter));
            Assert.AreEqual("Event IntEvent As System.EventHandler(Of Integer)", e.ToStringEx(vbFormatter));
            Assert.AreEqual("event System::EventHandler<int>^ IntEvent", e.ToStringEx(cppFormatter));
        }
    }
}
