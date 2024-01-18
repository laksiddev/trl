using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Open.Trl;

namespace Open.Trl.Test
{
    [TestClass]
    public class TeKeyTests
    {
        [TestMethod]
        public void ToStringGuidTest()
        {
            Guid testItem = new Guid("68DE157C-35DA-46D1-933E-0BF036AD6676");

            TeKey uut = new TeKey(testItem);
            string result = uut.ToString();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParseGuidTest()
        {
            string testItem = "gykovaqcdyg4k2d7667cqbahc6o2rauk2r7eglawjyitrtf7nj7wmjv3gjtq";
            TeKey teKeyResult = TeKey.Parse(testItem);

            Assert.IsNotNull(teKeyResult);
            Assert.AreEqual(TeKeyType.Guid, teKeyResult.KeyType);
            Guid testNoException = teKeyResult.GuidValue;

            Assert.AreEqual(new Guid("68DE157C-35DA-46D1-933E-0BF036AD6676"), testNoException);
        }

        [TestMethod]
        public void TryParseGuidTest()
        {
            string testItem = "gykovaqcdyg4k2d7667cqbahc6o2rauk2r7eglawjyitrtf7nj7wmjv3gjtq";
            TeKey teKeyResult;
            bool wasSuccessful = TeKey.TryParse(testItem, out teKeyResult);

            Assert.IsTrue(wasSuccessful);
            Assert.IsNotNull(teKeyResult);
            Assert.AreEqual(TeKeyType.Guid, teKeyResult.KeyType);
            Guid testNoException = teKeyResult.GuidValue;

            Assert.AreEqual(new Guid("68DE157C-35DA-46D1-933E-0BF036AD6676"), testNoException);
        }

        [TestMethod]
        public void ToStringLongTest()
        {
            long testItem = DateTime.Now.Ticks;

            TeKey uut = new TeKey(testItem);
            string result = uut.ToString();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParseLongTest()
        {
            string testItem = "mxm2tmmbm3bzxs4neybaitq5j3vfujudnq";
            TeKey teKeyResult = TeKey.Parse(testItem);

            Assert.IsNotNull(teKeyResult);
            Assert.AreEqual(TeKeyType.Int64, teKeyResult.KeyType);
            long testNoException = teKeyResult.Int64Value;

            Assert.AreEqual(636856714056840387L, testNoException);
        }

        [TestMethod]
        public void TryParseLongTest()
        {
            string testItem = "mxm2tmmbm3bzxs4neybaitq5j3vfujudnq";
            TeKey teKeyResult;
            bool wasSuccessful = TeKey.TryParse(testItem, out teKeyResult);

            Assert.IsTrue(wasSuccessful);
            Assert.IsNotNull(teKeyResult);
            Assert.AreEqual(TeKeyType.Int64, teKeyResult.KeyType);
            long testNoException = teKeyResult.Int64Value;

            Assert.AreEqual(636856714056840387L, testNoException);
        }

        [TestMethod]
        public void ToStringIntTest()
        {
            long testLong = DateTime.Now.Ticks;
            int testItem = (int)(testLong & 0x7fffffff);

            TeKey uut = new TeKey(testItem);
            string result = uut.ToString();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParseIntTest()
        {
            string testItem = "d4fpamchncdw3z5tjil66qurmkxl23dxne";
            TeKey teKeyResult = TeKey.Parse(testItem);

            Assert.IsNotNull(teKeyResult);
            Assert.AreEqual(TeKeyType.Int32, teKeyResult.KeyType);
            int testNoException = teKeyResult.Int32Value;

            Assert.AreEqual(1421786691, testNoException);
        }

        [TestMethod]
        public void TryParseIntTest()
        {
            string testItem = "d4fpamchncdw3z5tjil66qurmkxl23dxne";
            TeKey teKeyResult;
            bool wasSuccessful = TeKey.TryParse(testItem, out teKeyResult);

            Assert.IsTrue(wasSuccessful);
            Assert.IsNotNull(teKeyResult);
            Assert.AreEqual(TeKeyType.Int32, teKeyResult.KeyType);
            int testNoException = teKeyResult.Int32Value;

            Assert.AreEqual(1421786691, testNoException);
        }

        [TestMethod]
        public void ToStringStringTest()
        {
            string testItem = "The quick brown fox.";

            TeKey uut = new TeKey(testItem);
            string result = uut.ToString();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParseStringTest()
        {
            string testItem = "4cnpamdidfgzuhbbduryejo2pgnfu2r6uzp4ffkqamqsveylgpnkjw7kafzq";
            TeKey teKeyResult = TeKey.Parse(testItem);

            Assert.IsNotNull(teKeyResult);
            Assert.AreEqual(TeKeyType.String, teKeyResult.KeyType);
            string testNoException = teKeyResult.StringValue;

            Assert.AreEqual("The quick brown fox.", testNoException);
        }

        [TestMethod]
        public void TryParseStringTest()
        {
            string testItem = "4cnpamdidfgzuhbbduryejo2pgnfu2r6uzp4ffkqamqsveylgpnkjw7kafzq";
            TeKey teKeyResult;
            bool wasSuccessful = TeKey.TryParse(testItem, out teKeyResult);

            Assert.IsTrue(wasSuccessful);
            Assert.IsNotNull(teKeyResult);
            Assert.AreEqual(TeKeyType.String, teKeyResult.KeyType);
            string testNoException = teKeyResult.StringValue;

            Assert.AreEqual("The quick brown fox.", testNoException);
        }

        [TestMethod]
        public void CaseInsensitiveTest()
        {
            string testItem = "Four score and seven years ago.";

            TeKey uut = new TeKey(testItem);
            string teResult = uut.ToString();

            TeKey teKeyResult = TeKey.Parse(teResult.ToLower());

            Assert.IsNotNull(teKeyResult);
            string stringResult = teKeyResult.StringValue;

            Assert.AreEqual(testItem, stringResult);

            teKeyResult = TeKey.Parse(teResult.ToUpper());

            Assert.IsNotNull(teKeyResult);
            stringResult = teKeyResult.StringValue;

            Assert.AreEqual(testItem, stringResult);
        }

        [TestMethod]
        public void ParseStringConfirmExceptionDecodeTest()
        {
            string testItem = "4cnpamdidfgzuhbbduryejo2pgnfu2r6uzp4ffkqamqsveylgpnkjw7kafzq";
            testItem = testItem.Replace('a', 'b').Replace('e', 'f');

            Assert.ThrowsException<TamperingException>(() => TeKey.Parse(testItem));
        }

        [TestMethod]
        public void TryParseStringConfirmExceptionDecodeTest()
        {
            string testItem = "4cnpamdidfgzuhbbduryejo2pgnfu2r6uzp4ffkqamqsveylgpnkjw7kafzq";
            testItem = testItem.Replace('a', 'b').Replace('e', 'f');

            TeKey teKeyResult;
            bool wasSuccessful = TeKey.TryParse(testItem, out teKeyResult);

            Assert.IsFalse(wasSuccessful);
            Assert.IsNull(teKeyResult);
        }

        [TestMethod]
        public void ParseConfirmExceptionKeyCodeTest()
        {
            string testItem = "v2d3xgkx3lra7tony7abpu4pkpfig4xp64meub5slwe7yhumbfjaq";

            Assert.ThrowsException<TamperingException>(() => TeKey.Parse(testItem));
        }

        [TestMethod]
        public void TryParseConfirmExceptionKeyCodeTest()
        {
            string testItem = "v2d3xgkx3lra7tony7abpu4pkpfig4xp64meub5slwe7yhumbfjaq";

            TeKey teKeyResult;
            bool wasSuccessful = TeKey.TryParse(testItem, out teKeyResult);

            Assert.IsFalse(wasSuccessful);
            Assert.IsNull(teKeyResult);
        }

        [TestMethod]
        public void ParseConfirmExceptionMismatchTypeTest()
        {
            string testItem = "nsmenc47duf4k3fb6vbzms4aig";

            Assert.ThrowsException<TamperingException>(() => TeKey.Parse(testItem));
        }

        [TestMethod]
        public void TryParseConfirmExceptionMismatchTypeTest()
        {
            string testItem = "nsmenc47duf4k3fb6vbzms4aig";

            TeKey teKeyResult;
            bool wasSuccessful = TeKey.TryParse(testItem, out teKeyResult);

            Assert.IsFalse(wasSuccessful);
            Assert.IsNull(teKeyResult);
        }

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
