using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Open.Trl.Test
{
    [TestClass]
    public class TeKeyTests
    {
        [TestMethod]
        public void ToTeStringGuidTest()
        {
            Guid testItem = new Guid("68DE157C-35DA-46D1-933E-0BF036AD6676");

            TeKey uut = new TeKey(testItem);
            string result = uut.ToTeString();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FromTeStringGuidTest()
        {
            string testItem = "gykovaqcdyg4k2d7667cqbahc6o2rauk2r7eglawjyitrtf7nj7wmjv3gjtq";
            TeKey result = TeKey.FromTeString(testItem);

            Assert.IsNotNull(result);
            Guid testNoException = result.GuidValue;

            Assert.AreEqual(new Guid("68DE157C-35DA-46D1-933E-0BF036AD6676"), testNoException);
        }

        [TestMethod]
        public void ToTeStringLongTest()
        {
            long testItem = DateTime.Now.Ticks;

            TeKey uut = new TeKey(testItem);
            string result = uut.ToTeString();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FromTeStringLongTest()
        {
            string testItem = "mxm2tmmbm3bzxs4neybaitq5j3vfujudnq";
            TeKey result = TeKey.FromTeString(testItem);

            Assert.IsNotNull(result);
            long testNoException = result.Int64Value;

            Assert.AreEqual(636856714056840387L, testNoException);
        }

        [TestMethod]
        public void ToTeStringIntTest()
        {
            long testLong = DateTime.Now.Ticks;
            int testItem = (int)(testLong & 0x7fffffff);

            TeKey uut = new TeKey(testItem);
            string result = uut.ToTeString();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FromTeStringIntTest()
        {
            string testItem = "d4fpamchncdw3z5tjil66qurmkxl23dxne";
            TeKey result = TeKey.FromTeString(testItem);

            Assert.IsNotNull(result);
            int testNoException = result.Int32Value;

            Assert.AreEqual(1421786691, testNoException);
        }

        [TestMethod]
        public void ToTeStringStringTest()
        {
            string testItem = "The quick brown fox.";

            TeKey uut = new TeKey(testItem);
            string result = uut.ToTeString();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FromTeStringStringTest()
        {
            string testItem = "4cnpamdidfgzuhbbduryejo2pgnfu2r6uzp4ffkqamqsveylgpnkjw7kafzq";
            TeKey result = TeKey.FromTeString(testItem);

            Assert.IsNotNull(result);
            string testNoException = result.StringValue;

            Assert.AreEqual("The quick brown fox.", testNoException);
        }

        [TestMethod]
        public void CaseInsensitiveTest()
        {
            string testItem = "Four score and seven years ago.";

            TeKey uut = new TeKey(testItem);
            string teResult = uut.ToTeString();

            TeKey teKeyResult = TeKey.FromTeString(teResult.ToLower());

            Assert.IsNotNull(teKeyResult);
            string stringResult = teKeyResult.StringValue;

            Assert.AreEqual(testItem, stringResult);

            teKeyResult = TeKey.FromTeString(teResult.ToUpper());

            Assert.IsNotNull(teKeyResult);
            stringResult = teKeyResult.StringValue;

            Assert.AreEqual(testItem, stringResult);
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void FromTeStringConfirmExceptionDecodeTest()
        {
            string testItem = "4cnpamdidfgzuhbbduryejo2pgnfu2r6uzp4ffkqamqsveylgpnkjw7kafzq";
            testItem = testItem.Replace('a', 'b').Replace('e', 'f');
            TeKey result = TeKey.FromTeString(testItem);
        }

        //[ExpectedException(typeof(ApplicationException))]
        //[TestMethod]
        //public void FromTeStringConfirmExceptionKeyCodeTest()
        //{
        //    string testItem = "v2d3xgkx3lra7tony7abpu4pkpfig4xp64meub5slwe7yhumbfjaq";
        //    TeKey result = TeKey.FromTeString(testItem);
        //}

        //[ExpectedException(typeof(ApplicationException))]
        //[TestMethod]
        //public void FromTeStringConfirmExceptionMismatchTypeTest()
        //{
        //    string testItem = "nsmenc47duf4k3fb6vbzms4aig";
        //    TeKey result = TeKey.FromTeString(testItem);
        //}

        [TestMethod]
        public void EncryptDecryptTest()
        {
            Guid testItem = new Guid("A5F435AB-E0F0-40E4-A781-AFCBF44324FD");

            TeKey uut = new TeKey(testItem);

            byte[] cleartext = testItem.ToByteArray();

            byte[] cyphertext = TeKey.Encrypt(cleartext);
            byte[] compareCleartext = TeKey.Decrypt(cyphertext);

            Assert.IsTrue(compareCleartext.Length == 16);
            Guid compareGuid = new Guid(compareCleartext);

            Assert.AreEqual(testItem, compareGuid);
        }

        [TestMethod]
        public void EncodeDecodeTest()
        {
            Guid testItem = new Guid("A5F435AB-E0F0-40E4-A781-AFCBF44324FD");

            TeKey uut = new TeKey(testItem);

            byte[] cleartext = testItem.ToByteArray();

            string cyphertext = Base32.Encode(cleartext);
            byte[] compareCleartext = Base32.Decode(cyphertext);

            Assert.IsTrue(compareCleartext.Length == 16);
            Guid compareGuid = new Guid(compareCleartext);

            Assert.AreEqual(testItem, compareGuid);
        }
    }
}
