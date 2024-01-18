using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Open.Trl
{
    public class TeKey
    {
        private const string __defaultEncryptionPassphrase = "JustASimplePassphrase";
        private const string __encryptionPassphraseAppSettingsKey = "appSettings:tamperResistantEncryptionPassphrase";

        private const int __keySize = 128;
        private static readonly byte[] __initializationVectorBytes = new byte[] { 0xCA, 0x83, 0x7E, 0x3F, 0xB3, 0x80, 0x49, 0x8F, 0x04, 0x3F, 0x22, 0x37, 0x03, 0x00, 0x48, 0xE8 };
        private static readonly byte[] __initializationSaltBytes = new byte[] { 0xA3, 0x88, 0xEF, 0x05, 0x70, 0x11, 0xF1, 0x4E, 0xB6, 0x88, 0x71, 0x87 };   //, 0x55, 0x17, 0x07, 0x0D

        private static string _encryptionPassphrase = null;

        private readonly KeyType _keyType;
        private string _stringValue;
        private Guid _guidValue;
        private int _intValue;
        private long _longValue;

        public TeKey(string stringValue)
        {
            _keyType = KeyType.String;
            _stringValue = stringValue;
        }

        public TeKey(Guid guidValue)
        {
            _keyType = KeyType.Guid;
            _guidValue = guidValue;
        }

        public TeKey(int intValue)
        {
            _keyType = KeyType.Int32;
            _intValue = intValue;
        }

        public TeKey(long longValue)
        {
            _keyType = KeyType.Int64;
            _longValue = longValue;
        }

        public string StringValue
        {
            get
            {
                if (!(_keyType == KeyType.String))
                    throw new InvalidOperationException(String.Format("Invalid Key Type. This Key Type is: {0}", _keyType.ToString()));

                return _stringValue;
            }
        }

        public Guid GuidValue
        {
            get
            {
                if (!(_keyType == KeyType.Guid))
                    throw new InvalidOperationException(String.Format("Invalid Key Type. This Key Type is: {0}", _keyType.ToString()));

                return _guidValue;
            }
        }

        public int Int32Value
        {
            get
            {
                if (!(_keyType == KeyType.Int32))
                    throw new InvalidOperationException(String.Format("Invalid Key Type. This Key Type is: {0}", _keyType.ToString()));

                return _intValue;
            }
        }

        public long Int64Value
        {
            get
            {
                if (!(_keyType == KeyType.Int64))
                    throw new InvalidOperationException(String.Format("Invalid Key Type. This Key Type is: {0}", _keyType.ToString()));

                return _longValue;
            }
        }

        public string KeyTypeValue
        {
            get { return _keyType.ToString();  }
        }

        public static TeKey FromTeString(string teString)
        {
            TeKey result = null;
            try
            {
                byte[] cyphertextWithCode = Base32.Decode(teString);
                byte[] cyphertext = cyphertextWithCode.Take(cyphertextWithCode.Length - 1).ToArray();
                byte[] keyTypeCode = cyphertextWithCode.Skip(cyphertextWithCode.Length - 1).ToArray();

                KeyType keyType = KeyType.Guid;
                if (keyTypeCode[0] == 0x73)  // "s"
                {
                    keyType = KeyType.String;
                }
                else if (keyTypeCode[0] == 0x67)  // "g"
                {
                    keyType = KeyType.Guid;
                }
                else if (keyTypeCode[0] == 0x69)  // "i"
                {
                    keyType = KeyType.Int32;
                }
                else if (keyTypeCode[0] == 0x6c)  // "l"
                {
                    keyType = KeyType.Int64;
                }

                byte[] keyBytes = Decrypt(cyphertext);
                switch (keyType)
                {
                    case KeyType.String:
                        string teKeyString;
                        try
                        {
                            teKeyString = System.Text.UTF8Encoding.UTF8.GetString(keyBytes);
                        }
                        catch (Exception)
                        {
                            throw new ApplicationException("The identity key is invalid because the value could not be converted.");
                        }
                        result = new TeKey(teKeyString);
                        break;

                    case KeyType.Guid:
                        Guid teKeyGuid;
                        try
                        {
                            teKeyGuid = new Guid(keyBytes);
                        }
                        catch (Exception)
                        {
                            throw new ApplicationException("The identity key is invalid because the value could not be converted.");
                        }
                        result = new TeKey(teKeyGuid);
                        break;

                    case KeyType.Int32:
                        int teKeyInt;
                        try
                        {
                            teKeyInt = BitConverter.ToInt32(keyBytes, 0);
                        }
                        catch (Exception)
                        {
                            throw new ApplicationException("The identity key is invalid because the value could not be converted.");
                        }
                        result = new TeKey(teKeyInt);
                        break;

                    case KeyType.Int64:
                        long teKeyLong;
                        try
                        {
                            teKeyLong = BitConverter.ToInt64(keyBytes, 0);
                        }
                        catch (Exception)
                        {
                            throw new ApplicationException("The identity key is invalid because the value could not be converted.");
                        }
                        result = new TeKey(teKeyLong);
                        break;

                    default:
                        result = null;
                        break;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("The identity key is invalid because the value could not be interpreted or translated.");
            }

            return result;
        }

        public string ToTeString()
        {
            byte[] keyBytes;
            byte[] keyTypeCode;
            switch (_keyType)
            {
                case KeyType.String:
                    keyBytes = System.Text.UTF8Encoding.UTF8.GetBytes(_stringValue);
                    keyTypeCode = new byte[] { 0x73 }; // "s";
                    break;

                case KeyType.Guid:
                    keyBytes = _guidValue.ToByteArray();
                    keyTypeCode = new byte[] { 0x67 }; // "g";
                    break;

                case KeyType.Int32:
                    keyBytes = BitConverter.GetBytes(_intValue);
                    keyTypeCode = new byte[] { 0x69 };    // "i";
                    break;

                case KeyType.Int64:
                    keyBytes = BitConverter.GetBytes(_longValue);
                    keyTypeCode = new byte[] { 0x6c }; // "l";
                    break;

                default:
                    keyBytes = null;
                    keyTypeCode = null;
                    break;
            }

            if (keyBytes == null)
                return null;

            byte[] cyphertext = Encrypt(keyBytes);
            byte[] cyphertextWithCode = cyphertext.Concat(keyTypeCode).ToArray();
            string cypherKey = Base32.Encode(cyphertextWithCode);

            return cypherKey;
        }

        public static string EncryptionPassphrase
        {
            get
            {
                if (_encryptionPassphrase == null)
                {
                    IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                    IConfiguration configuration = builder.Build();

                    if (configuration != null)
                        _encryptionPassphrase = configuration[__encryptionPassphraseAppSettingsKey];
                    if (_encryptionPassphrase == null)
                        _encryptionPassphrase = __defaultEncryptionPassphrase;
                }

                return _encryptionPassphrase;
            }

            set { _encryptionPassphrase = value; }
        }

        // SHA256CryptoServiceProvider is considered FIPS compliant
        // AesCryptoServiceProvider is considered FIPS compliant
        // Rfc2898DeriveBytes is based on HMACSHA1 which is NOT FIPS compliant. At this time, there are no FIPS compliant password hashing algorithms.
        public static byte[] Encrypt(byte[] cleartext)
        {
            // Make encryption a bit more deterministic
            // Use the cleartext to derive the salt
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] hashOfCleartext = sha256.ComputeHash(cleartext);
            byte[] saltRandomPortion = hashOfCleartext.Take(4).ToArray();

            byte[] randomizedSalt = __initializationSaltBytes.Concat(saltRandomPortion).ToArray();

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(EncryptionPassphrase, __initializationVectorBytes, 3);
            byte[] encryptionKey = rfc2898DeriveBytes.GetBytes(__keySize/8);

            MemoryStream msout = new MemoryStream();

            SymmetricAlgorithm encryptor = AesCryptoServiceProvider.Create(); // default is CBC algoritm with PKCS#7 padding
            CryptoStream encStream = new CryptoStream(msout, encryptor.CreateEncryptor(encryptionKey, randomizedSalt), CryptoStreamMode.Write);
            
            encStream.Write(cleartext, 0, cleartext.Length);
            encStream.FlushFinalBlock();

            byte[] cyphertext = msout.ToArray();
            encStream.Close();

            byte[] cyphertextWithSalt = cyphertext.Concat(saltRandomPortion).ToArray();

            return cyphertextWithSalt;
        }

        // AesCryptoServiceProvider is considered FIPS compliant
        // Rfc2898DeriveBytes is based on HMACSHA1 which is NOT FIPS compliant. At this time, there are no FIPS compliant password hashing algorithms.
        public static byte[] Decrypt(byte[] cyphertextWithSalt)
        {
            byte[] cyphertext = cyphertextWithSalt.Take(cyphertextWithSalt.Length - 4).ToArray();
            byte[] saltRandomPortion = cyphertextWithSalt.Skip(cyphertextWithSalt.Length - 4).ToArray();

            byte[] randomizedSalt = __initializationSaltBytes.Concat(saltRandomPortion).ToArray();

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(EncryptionPassphrase, __initializationVectorBytes, 3);
            byte[] encryptionKey = rfc2898DeriveBytes.GetBytes(__keySize / 8);

            MemoryStream msout = new MemoryStream();

            SymmetricAlgorithm encryptor = AesCryptoServiceProvider.Create(); // default is CBC algoritm with PKCS#7 padding
            CryptoStream encStream = new CryptoStream(msout, encryptor.CreateDecryptor(encryptionKey, randomizedSalt), CryptoStreamMode.Write);

            encStream.Write(cyphertext, 0, cyphertext.Length);
            encStream.FlushFinalBlock();

            byte[] cleartext = msout.ToArray();
            encStream.Close();

            return cleartext;
        }
    }
}
