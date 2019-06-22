using System.IO;
using ADL.Configs;
using ADL.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL.UnitTests
{
    [TestClass]
    public class DebugOperations
    {
        [TestMethod]
        public void Test_AddOutputStream()
        {
            Debug.SendUpdateMessageOnFirstLog = true;
            Debug.SendWarnings = false;
            Debug.ADLEnabled = false;

            Debug.AddOutputStream(null);

            Assert.AreEqual(0, Debug.LogStreamCount);

            Debug.RemoveAllOutputStreams();
            Debug.ADLEnabled = false;
            var ls = new LogStream(new MemoryStream());
            Debug.AddOutputStream(ls);
            Debug.AddOutputStream(ls);
            Assert.AreEqual(1, Debug.LogStreamCount);

            Debug.RemoveAllOutputStreams();

            Debug.SendWarnings = true;
            Debug.ADLEnabled = true;
        }


        [TestMethod]
        public void Test_AddPrefixForMask()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode =
                PrefixLookupSettings.ADDPREFIXIFAVAILABLE | PrefixLookupSettings.DECONSTRUCTMASKTOFIND;


            var bm = new BitMask(2 | 8);

            Debug.ADLEnabled = false;
            Debug.AddPrefixForMask(bm | 4, "Test");
            Debug.AddPrefixForMask(bm | 4, "Test");
            Debug.RemoveAllPrefixes();

            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.ADLEnabled = true;
            var ret = Debug.GetMaskPrefix(bm) == "HELLO";
            Assert.IsTrue(ret);
        }

        [TestMethod]
        public void Test_RemovePrefixForMask()
        {
            var bm = new BitMask(2 | 8);
            Debug.SendUpdateMessageOnFirstLog = false;
            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.RemovePrefixForMask(bm);


            Debug.ADLEnabled = false;

            Debug.AddPrefixForMask(bm, "AAA");
            Debug.RemovePrefixForMask(bm);
            Debug.ADLEnabled = true;

            Debug.SendUpdateMessageOnFirstLog = true;
            Assert.IsTrue(Debug.GetAllPrefixes().Count == 0);
        }

        [TestMethod]
        public void Test_RemoveAllPrefixes()
        {
            var bm = new BitMask(2 | 8);
            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.RemoveAllPrefixes();
            Assert.IsTrue(Debug.GetAllPrefixes().Count == 0);
        }

        [TestMethod]
        public void Test_SetAllPrefixes()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE;


            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(Debug.GetMaskPrefix(1) == "Hello");
            Assert.IsTrue(Debug.GetMaskPrefix(2) == "HELLO1");
            Assert.IsTrue(Debug.GetMaskPrefix(4) == "HOLA2");

            Debug.RemoveAllPrefixes();

            Debug.ADLEnabled = false;
            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(Debug.GetMaskPrefix(1) == "Hello");
            Assert.IsTrue(Debug.GetMaskPrefix(2) == "HELLO1");
            Assert.IsTrue(Debug.GetMaskPrefix(4) == "HOLA2");
            Debug.ADLEnabled = true;
        }

        [TestMethod]
        public void Test_GetAllPrefixes()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE;


            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(Debug.GetAllPrefixes().Count == 3);

            Debug.RemoveAllPrefixes();

            Debug.ADLEnabled = false;
            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(Debug.GetAllPrefixes().Count == 3);
            Debug.ADLEnabled = true;
        }


        [TestMethod]
        public void Test_GetPrefixMask()
        {
            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            if (Debug.GetPrefixMask("Hello", out var bm)) Assert.IsTrue(bm == 1);
            if (Debug.GetPrefixMask("HELLO1", out bm)) Assert.IsTrue(bm == 2);
            if (Debug.GetPrefixMask("HOLA2", out bm)) Assert.IsTrue(bm == 4);
        }

        [TestMethod]
        public void Test_GetMaskPrefix()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE;

            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            BitMask bm = 2;
            Assert.IsTrue(Debug.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE | PrefixLookupSettings.ONLYONEPREFIX;

            bm = 32;
            Assert.IsFalse(Debug.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE | PrefixLookupSettings.BAKEPREFIXES |
                                     PrefixLookupSettings.DECONSTRUCTMASKTOFIND;

            bm = 32;
            Assert.IsFalse(Debug.GetMaskPrefix(bm) == "HELLO1");
        }

        [TestMethod]
        public void Test_Log()
        {
            var lts = new LogTextStream(new PipeStream())
            {
                AddTimeStamp = false
            };


            Debug.PrefixLookupMode = PrefixLookupSettings.NOPREFIX;
            Debug.SendUpdateMessageOnFirstLog = false;
            Assert.IsTrue(Debug.PrefixLookupMode == PrefixLookupSettings.NOPREFIX);
            Assert.IsFalse(Debug.SendUpdateMessageOnFirstLog);
            Debug.SendUpdateMessageOnFirstLog = true;
            Debug.AddOutputStream(lts);
            Debug.Log(1, "ffffffffff");

            var buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            var s = Debug.TextEncoding.GetString(buf);

            Assert.IsTrue(s.EndsWith("ffffffffff\n")); //ADL is appending the \n when using LogTextStreams


            Debug.LogGen(1, "ffffffffff");
            Debug.ADLEnabled = false;
            Debug.LogGen(1, "ffffffffff");
            Debug.ADLEnabled = true;
            buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            s = Debug.TextEncoding.GetString(buf);

            Assert.IsTrue(s.EndsWith("ffffffffff\n")); //ADL is appending the \n when using LogTextStreams


            Debug.PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE | PrefixLookupSettings.BAKEPREFIXES;

            Debug.Log(2 | 4, "CODE COVERAGE");
            Debug.Log(2 | 4, "CODE COVERAGE");
        }

        [TestMethod]
        public void Test_RemoveOutputStream()
        {
            var lts = new LogTextStream(new PipeStream());
            Debug.AddOutputStream(lts);
            var newCount = Debug.LogStreamCount;

            Debug.ADLEnabled = false;
            Debug.RemoveOutputStream(lts);

            Debug.ADLEnabled = true;
            Debug.RemoveOutputStream(lts);
            Debug.RemoveOutputStream(lts);

            Assert.IsTrue(Debug.LogStreamCount == newCount - 1);
        }

        [TestMethod]
        public void Test_LoadConfig()
        {
            Debug.LoadConfig(ADLConfig.Standard);
            Assert.IsTrue(Debug.ADLEnabled == ADLConfig.Standard.ADLEnabled);
            Assert.IsTrue(Debug.ADLWarningMask == ADLConfig.Standard.WarningMask);
            Assert.IsTrue(Debug.UpdateMask == ADLConfig.Standard.UpdateMask);
            Assert.IsTrue(Debug.SendWarnings == ADLConfig.Standard.SendWarnings);
            Assert.IsTrue(Debug.GetAllPrefixes().Count == ADLConfig.Standard.Prefixes.Keys.Count);
        }
    }
}