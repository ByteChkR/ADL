using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ADL.UnitTests
{
    [TestClass]
    public class DebugOperations
    {
        [TestMethod]
        public void Test_AddPrefixForMask()
        {
            BitMask bm = new BitMask(2 | 8);
            Debug.AddPrefixForMask(bm, "HELLO");
            Assert.IsTrue(Debug.GetMaskPrefix(bm) == "HELLO");
        }

        [TestMethod]
        public void Test_RemovePrefixForMask()
        {
            BitMask bm = new BitMask(2 | 8);
            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.RemovePrefixForMask(bm);
            Assert.IsTrue(Debug.GetAllTags().Count == 0);
        }

        [TestMethod]
        public void Test_RemoveAllPrefixes()
        {
            BitMask bm = new BitMask(2 | 8);
            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.RemoveAllPrefixes();
            Assert.IsTrue(Debug.GetAllTags().Count == 0);

        }

        [TestMethod]
        public void Test_SetAllPrefixes()
        {
            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            Assert.IsTrue(Debug.GetMaskPrefix(1) == "Hello");
            Assert.IsTrue(Debug.GetMaskPrefix(2) == "HELLO1");
            Assert.IsTrue(Debug.GetMaskPrefix(4) == "HOLA2");
        }

        [TestMethod]
        public void Test_GetPrefixMask()
        {

            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            BitMask bm;
            if (Debug.GetPrefixMask("Hello", out bm))
            {
                Assert.IsTrue(bm == 1);
            }
            if (Debug.GetPrefixMask("HELLO1", out bm))
            {
                Assert.IsTrue(bm == 2);
            }
            if (Debug.GetPrefixMask("HOLA2", out bm))
            {
                Assert.IsTrue(bm == 4);
            }
        }

        [TestMethod]
        public void Test_GetMaskPrefix()
        {

            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            BitMask bm = 2;
            Assert.IsTrue(Debug.GetMaskPrefix(bm) == "HELLO1");
        }



    }
}
