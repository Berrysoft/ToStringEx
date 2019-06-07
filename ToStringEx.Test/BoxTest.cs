using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToStringEx.Test
{
    [TestClass]
    public class BoxTest
    {
        [TestMethod]
        public void StringFormatTest()
        {
            (int, int[], string) t2 = (1, new int[] { 1, 2, 3 }, "abc");
            Assert.AreEqual("(1, {1, 2, 3}, abc)", $"{ToStringExBox.Create(t2)}");
        }
    }
}
