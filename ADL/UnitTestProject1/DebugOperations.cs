using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
namespace ADL.UnitTests
{
    [TestClass]
    public class DebugOperations
    {
        [TestMethod]
        public void Test_AddPrefixForMask()
        {
            Debug.SendUpdateMessageOnFirstLog = false;


            BitMask bm = new BitMask(2 | 8);
            Debug.AddPrefixForMask(bm, "HELLO");
            bool ret = Debug.GetMaskPrefix(bm) == "HELLO";

            Assert.IsTrue(ret);
            
        }

        [TestMethod]
        public void Test_RemovePrefixForMask()
        {
            BitMask bm = new BitMask(2 | 8);
            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.RemovePrefixForMask(bm);
            Assert.IsTrue(Debug.GetAllPrefixes().Count == 0);
        }

        [TestMethod]
        public void Test_RemoveAllPrefixes()
        {
            BitMask bm = new BitMask(2 | 8);
            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.RemoveAllPrefixes();
            Assert.IsTrue(Debug.GetAllPrefixes().Count == 0);

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
