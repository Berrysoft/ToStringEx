using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToStringEx.Test
{
    [TestClass]
    public class BoxTest
    {
        [TestMethod]
        public void StringFormatTest()
        {
            (int, int[], string) t = (1, new int[] { 1, 2, 3 }, "abc");
            Assert.AreEqual("(1, {1, 2, 3}, abc)", $"{ToStringExBox.Create(t)}");
            int[][] twodarr = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 7, 8, 9 } };
            Assert.AreEqual("{{1, 2, 3}, {4, 5, 6}, {7, 8, 9}}", $"{ToStringExBox.Create(twodarr, new EnumerableFormatter<IEnumerable<int>>(new EnumerableFormatter<int>()))}");
        }
    }
}
