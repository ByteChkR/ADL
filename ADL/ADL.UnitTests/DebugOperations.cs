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
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.ADDPREFIXIFAVAILABLE | Configs.PrefixLookupSettings.DECONSTRUCTMASKTOFIND;

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
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.ADDPREFIXIFAVAILABLE;


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
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.ADDPREFIXIFAVAILABLE;

            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            BitMask bm = 2;
            Assert.IsTrue(Debug.GetMaskPrefix(bm) == "HELLO1");
        }

        [TestMethod]
        public void Test_Log()
        {
            Streams.LogTextStream lts = new Streams.LogTextStream(new Streams.PipeStream());
            lts.AddTimeStamp = false;
            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.NOPREFIX;
            Debug.SendUpdateMessageOnFirstLog = false;
            Debug.AddOutputStream(lts);
            Debug.Log(1, "ffffffffff");
            byte[] buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            string s = System.Text.Encoding.ASCII.GetString(buf);
            System.Console.WriteLine(s);
            Assert.IsTrue(s == "ffffffffff\n"); //ADL is appending the \n when using LogTextStreams
        }

        [TestMethod]
        public void Test_RemoveOutputStream()
        {
            Streams.LogTextStream lts = new Streams.LogTextStream(new Streams.PipeStream());
            Debug.AddOutputStream(lts);
            int newCount = Debug.LogStreamCount;
            Debug.RemoveOutputStream(lts);
            Assert.IsTrue(Debug.LogStreamCount == newCount-1);
        }

        [TestMethod]
        public void Test_LoadConfig()
        {
            Debug.LoadConfig(Configs.ADLConfig.Standard);
            Assert.IsTrue(Debug.ADLEnabled == Configs.ADLConfig.Standard.ADLEnabled);
            Assert.IsTrue(Debug.ADLWarningMask == Configs.ADLConfig.Standard.WarningMask);
            Assert.IsTrue(Debug.UpdateMask == Configs.ADLConfig.Standard.UpdateMask);
            Assert.IsTrue(Debug.SendWarnings == Configs.ADLConfig.Standard.SendWarnings);
            Assert.IsTrue(Debug.GetAllPrefixes().Count == Configs.ADLConfig.Standard.Prefixes.Keys.Count);

        }
    }
}
