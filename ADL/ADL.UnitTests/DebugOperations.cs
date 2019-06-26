using System.IO;
using ADL.Configs;
using ADL.Streams;

using NUnit.Framework;

namespace ADL.UnitTests
{
    [TestFixture]
    public class DebugOperations
    {
        [Test]
        public void Test_AddOutputStream()
        {
            Debug.SendUpdateMessageOnFirstLog = true;
            Debug.SendWarnings = false;
            Debug.AdlEnabled = false;

            Debug.AddOutputStream(null);

            Assert.AreEqual(0, Debug.LogStreamCount);

            Debug.RemoveAllOutputStreams();
            Debug.AdlEnabled = false;
            var ls = new LogStream(new MemoryStream());
            Debug.AddOutputStream(ls);
            Debug.AddOutputStream(ls);
            Assert.AreEqual(1, Debug.LogStreamCount);

            Debug.RemoveAllOutputStreams();

            Debug.SendWarnings = true;
            Debug.AdlEnabled = true;
        }


        [Test]
        public void Test_AddPrefixForMask()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode =
                PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Deconstructmasktofind;


            var bm = new BitMask(2 | 8);

            Debug.AdlEnabled = false;
            Debug.AddPrefixForMask(bm | 4, "Test");
            Debug.AddPrefixForMask(bm | 4, "Test");
            Debug.RemoveAllPrefixes();

            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.AdlEnabled = true;
            var ret = Debug.GetMaskPrefix(bm) == "HELLO";
            Assert.IsTrue(ret);
        }

        [Test]
        public void Test_RemovePrefixForMask()
        {
            var bm = new BitMask(2 | 8);
            Debug.SendUpdateMessageOnFirstLog = false;
            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.RemovePrefixForMask(bm);


            Debug.AdlEnabled = false;

            Debug.AddPrefixForMask(bm, "AAA");
            Debug.RemovePrefixForMask(bm);
            Debug.AdlEnabled = true;

            Debug.SendUpdateMessageOnFirstLog = true;
            Assert.IsTrue(Debug.GetAllPrefixes().Count == 0);
        }

        [Test]
        public void Test_RemoveAllPrefixes()
        {
            var bm = new BitMask(2 | 8);
            Debug.AddPrefixForMask(bm, "HELLO");
            Debug.RemoveAllPrefixes();
            Assert.IsTrue(Debug.GetAllPrefixes().Count == 0);
        }

        [Test]
        public void Test_SetAllPrefixes()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable;


            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(Debug.GetMaskPrefix(1) == "Hello");
            Assert.IsTrue(Debug.GetMaskPrefix(2) == "HELLO1");
            Assert.IsTrue(Debug.GetMaskPrefix(4) == "HOLA2");

            Debug.RemoveAllPrefixes();

            Debug.AdlEnabled = false;
            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(Debug.GetMaskPrefix(1) == "Hello");
            Assert.IsTrue(Debug.GetMaskPrefix(2) == "HELLO1");
            Assert.IsTrue(Debug.GetMaskPrefix(4) == "HOLA2");
            Debug.AdlEnabled = true;
        }

        [Test]
        public void Test_GetAllPrefixes()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable;


            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(Debug.GetAllPrefixes().Count == 3);

            Debug.RemoveAllPrefixes();

            Debug.AdlEnabled = false;
            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");

            Assert.IsTrue(Debug.GetAllPrefixes().Count == 3);
            Debug.AdlEnabled = true;
        }


        [Test]
        public void Test_GetPrefixMask()
        {
            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            if (Debug.GetPrefixMask("Hello", out var bm)) Assert.IsTrue(bm == 1);
            if (Debug.GetPrefixMask("HELLO1", out bm)) Assert.IsTrue(bm == 2);
            if (Debug.GetPrefixMask("HOLA2", out bm)) Assert.IsTrue(bm == 4);
        }

        [Test]
        public void Test_GetMaskPrefix()
        {
            //Flag is required to find tags made up of unique masks(example: 2|8)
            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable;

            Debug.SetAllPrefixes("Hello", "HELLO1", "HOLA2");
            BitMask bm = 2;
            Assert.IsTrue(Debug.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Onlyoneprefix;

            bm = 32;
            Assert.IsFalse(Debug.GetMaskPrefix(bm) == "HELLO1");

            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Bakeprefixes |
                                     PrefixLookupSettings.Deconstructmasktofind;

            bm = 32;
            Assert.IsFalse(Debug.GetMaskPrefix(bm) == "HELLO1");
        }

        [Test]
        public void Test_LogSerialization()
        {
            var l = new Log(-1, "Hello");
            var buf = l.Serialize();
            var ms = new MemoryStream(buf);
            var ltest = Log.Deserialize(buf, 0, out _);
            var lp = LogPackage.ReadBlock(ms, 13);

            Assert.IsTrue(buf.Length == lp.GetSerialized(false).Length);

            Assert.IsTrue(lp.Logs.Count == 1 && lp.Logs[0].Mask == ltest.Mask && lp.Logs[0].Message == ltest.Message);

            Assert.IsTrue(ltest.Mask == l.Mask && ltest.Message == l.Message);
        }


        [Test]
        public void Test_Log()
        {
            Debug.SendUpdateMessageOnFirstLog = false;
            var lts = new LogTextStream(new PipeStream())
            {
                AddTimeStamp = false
            };


            Debug.PrefixLookupMode = PrefixLookupSettings.Noprefix;
            Debug.SendUpdateMessageOnFirstLog = false;
            Assert.IsTrue(Debug.PrefixLookupMode == PrefixLookupSettings.Noprefix);
            Assert.IsFalse(Debug.SendUpdateMessageOnFirstLog);
            
            Debug.AddOutputStream(lts);
            Debug.Log(1, "ffffffffff");

            var buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            var s = Debug.TextEncoding.GetString(buf);

            Assert.IsTrue(s.EndsWith("ffffffffff\n")); //ADL is appending the \n when using LogTextStreams
            

            Debug.LogGen(1, "ffffffffff");
            Debug.AdlEnabled = false;
            Debug.LogGen(1, "ffffffffff");
            Debug.AdlEnabled = true;
            buf = new byte[lts.Length];
            lts.Read(buf, 0, buf.Length);
            s = Debug.TextEncoding.GetString(buf);

            Assert.IsTrue(s.EndsWith("ffffffffff\n"));


            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable | PrefixLookupSettings.Bakeprefixes;

            Debug.Log(2 | 4, "CODE COVERAGE");
            Debug.Log(2 | 4, "CODE COVERAGE");
            Debug.SendUpdateMessageOnFirstLog = true;
        }

        [Test]
        public void Test_RemoveOutputStream()
        {
            var lts = new LogTextStream(new PipeStream());
            Debug.AddOutputStream(lts);
            var newCount = Debug.LogStreamCount;

            Debug.AdlEnabled = false;
            Debug.RemoveOutputStream(lts);

            Debug.AdlEnabled = true;
            Debug.RemoveOutputStream(lts);
            Debug.RemoveOutputStream(lts);

            Assert.IsTrue(Debug.LogStreamCount == newCount - 1);
        }

        [Test]
        public void Test_LoadConfig()
        {
            Debug.LoadConfig(AdlConfig.Standard);
            Assert.IsTrue(Debug.AdlEnabled == AdlConfig.Standard.AdlEnabled);
            Assert.IsTrue(Debug.AdlWarningMask == AdlConfig.Standard.WarningMask);
            Assert.IsTrue(Debug.UpdateMask == AdlConfig.Standard.UpdateMask);
            Assert.IsTrue(Debug.SendWarnings == AdlConfig.Standard.SendWarnings);
            Assert.IsTrue(Debug.GetAllPrefixes().Count == AdlConfig.Standard.Prefixes.Keys.Count);
        }
    }
}