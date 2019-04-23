using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
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
            Streams.LogStream ls = new Streams.LogStream(new System.IO.MemoryStream());
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
            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.ADDPREFIXIFAVAILABLE | Configs.PrefixLookupSettings.DECONSTRUCTMASKTOFIND;


            BitMask bm = new BitMask(2 | 8);

            Debug.ADLEnabled = false;
            Debug.AddPrefixForMask(bm | 4, "Test");
            Debug.AddPrefixForMask(bm | 4, "Test");
            Debug.RemoveAllPrefixes();

            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.ADLEnabled = true;
            bool ret = Debug.GetMaskPrefix(bm) == "HELLO";
            Assert.IsTrue(ret);

        }

        [TestMethod]
        public void Test_RemovePrefixForMask()
        {
            BitMask bm = new BitMask(2 | 8);
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
            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.ADDPREFIXIFAVAILABLE;


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
            if (Debug.GetPrefixMask("Hello", out BitMask bm))
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

            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.ADDPREFIXIFAVAILABLE | Configs.PrefixLookupSettings.ONLYONEPREFIX;
            
            bm = 32;
            Assert.IsFalse(Debug.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.ADDPREFIXIFAVAILABLE | Configs.PrefixLookupSettings.BAKEPREFIXES | Configs.PrefixLookupSettings.DECONSTRUCTMASKTOFIND;
            
            bm = 32;
            Assert.IsFalse(Debug.GetMaskPrefix(bm) == "HELLO1");



           

        }

        [TestMethod]
        public void Test_Log()
        {

            Streams.LogTextStream lts = new Streams.LogTextStream(new Streams.PipeStream())
            {
                AddTimeStamp = false
            };


            
            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.NOPREFIX;
            Debug.SendUpdateMessageOnFirstLog = false;
            Assert.IsTrue(Debug.PrefixLookupMode == Configs.PrefixLookupSettings.NOPREFIX);
            Assert.IsFalse(Debug.SendUpdateMessageOnFirstLog);
            Debug.SendUpdateMessageOnFirstLog = true;
            Debug.AddOutputStream(lts);
            Debug.Log(1, "ffffffffff");

            byte[] buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            string s = System.Text.Encoding.ASCII.GetString(buf);

            Assert.IsTrue(s.EndsWith("ffffffffff\n")); //ADL is appending the \n when using LogTextStreams



            Debug.LogGen(1, "ffffffffff");
            Debug.ADLEnabled = false;
            Debug.LogGen(1, "ffffffffff");
            Debug.ADLEnabled = true;
            buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            s = System.Text.Encoding.ASCII.GetString(buf);

            Assert.IsTrue(s.EndsWith("ffffffffff\n")); //ADL is appending the \n when using LogTextStreams


            Debug.PrefixLookupMode = Configs.PrefixLookupSettings.ADDPREFIXIFAVAILABLE | Configs.PrefixLookupSettings.BAKEPREFIXES;

            Debug.Log(2 | 4, "CODE COVERAGE");
            Debug.Log(2 | 4, "CODE COVERAGE");

        }

        [TestMethod]
        public void Test_RemoveOutputStream()
        {
            Streams.LogTextStream lts = new Streams.LogTextStream(new Streams.PipeStream());
            Debug.AddOutputStream(lts);
            int newCount = Debug.LogStreamCount;

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
            Debug.LoadConfig(Configs.ADLConfig.Standard);
            Assert.IsTrue(Debug.ADLEnabled == Configs.ADLConfig.Standard.ADLEnabled);
            Assert.IsTrue(Debug.ADLWarningMask == Configs.ADLConfig.Standard.WarningMask);
            Assert.IsTrue(Debug.UpdateMask == Configs.ADLConfig.Standard.UpdateMask);
            Assert.IsTrue(Debug.SendWarnings == Configs.ADLConfig.Standard.SendWarnings);
            Assert.IsTrue(Debug.GetAllPrefixes().Count == Configs.ADLConfig.Standard.Prefixes.Keys.Count);

        }
    }
}
