using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace ADL.UnitTests
{
    [TestClass]
    public class MaskOperations
    {
        [TestMethod]
        public void Test_IsContainedInMask()
        {
            BitMask bm1 = new BitMask(1 | 4 | 16);
            BitMask bm2 = new BitMask(4);
            BitMask bm3 = new BitMask(2);

            Assert.IsTrue(BitMask.IsContainedInMask(bm1, bm2, false)); //True
            Assert.IsFalse(BitMask.IsContainedInMask(bm1, bm3, false)); //False
            Assert.IsFalse(BitMask.IsContainedInMask(bm1, bm2 | bm3, true)); //False
        }

        [TestMethod]
        public void Test_GetUniqueMaskSet()
        {
            BitMask bm = new BitMask(1 | 2 | 8 | 16 | 64);
            List<int> ret = BitMask.GetUniqueMasksSet(bm);

            Assert.IsTrue(ret.Count == 5);
            Assert.IsTrue(ret.Contains(1));
            Assert.IsTrue(ret.Contains(2));
            Assert.IsTrue(ret.Contains(8));
            Assert.IsTrue(ret.Contains(16));
            Assert.IsTrue(ret.Contains(64));
        }

        [TestMethod]
        public void Test_IsUniqueMask()
        {
            BitMask bm = new BitMask(2 | 4);
            BitMask bm1 = new BitMask(8);

            Assert.IsTrue(BitMask.IsUniqueMask(bm1));
            Assert.IsFalse(BitMask.IsUniqueMask(bm));
        }

        [TestMethod]
        public void Test_CombineMasks()
        {
            BitMask bm1 = new BitMask(2 | 8 | 32);
            BitMask bm2 = new BitMask(2 | 16 | 64);
            BitMask ret = BitMask.CombineMasks(MaskCombineType.BIT_AND, bm1, bm2);
            Assert.IsTrue(ret == 2);
            ret = BitMask.CombineMasks(MaskCombineType.BIT_OR, bm1, bm2);
            Assert.IsTrue(BitMask.IsContainedInMask(ret, 16, false));
            Assert.IsTrue(BitMask.IsContainedInMask(ret, 2, false));
        }

        [TestMethod]
        public void Test_RemoveFlags()
        {
            BitMask bm1 = new BitMask(2 | 8 | 16);
            BitMask ret = BitMask.RemoveFlags(bm1, 2);
            Assert.IsFalse(BitMask.IsContainedInMask(ret, 2, true));
            Assert.IsTrue(BitMask.IsContainedInMask(bm1, 2, true));
        }

    }
}
