/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014
 * Description: Common define common static function
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MyUtility
{
    public class Common
    {
        #region RANDOM_STRING

        /// <summary>
        ///     Trả về chuỗi random
        /// </summary>
        /// <param name="size">độ dài của chuỗi</param>
        /// <param name="lowerCase">viết hoa hay thường.True:Viết hoa,Flase:Viết thường</param>
        /// <returns>Chuỗi sau khi random</returns>
        public static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();

            var rndint = new Random();
            var rnd = new Random();
            for (var i = 0; i < size; i++)
            {
                var so = rnd.Next(0, 2);
                var ch = so != 1 ? Convert.ToChar(rndint.Next(0, 9).ToString()) : Convert.ToChar(Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65)));

                builder.Append(ch);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        #endregion RANDOM_STRING

        #region ma hoa

        #region md5W

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Md5 Encrypt
        /// </summary>
        /// <param name="signOrginal"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string signOrginal)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(signOrginal));
                var hashString = ConvertByteToString(data);
                return hashString;
            }
        }

        public static string MD5_encode(string strEncode)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(strEncode));
                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                foreach (var t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #endregion md5W

        #region HMAC SHA 256

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     SHA256 Encrypt
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetHashHmac(string stringToHash, string password)
        {
            var pass = Encoding.UTF8.GetBytes(password);
            using (var hmacsha256 = new HMACSHA256(pass))
            {
                hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                return ConvertByteToString(hmacsha256.Hash);
            }
        }

        /// <summary>
        ///     <para>Author: TrungLD</para>
        ///     <para>DateCreated: 18/12/2014</para>
        ///     <para>mã hóa sha256</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string sha256_hash(string value)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(value));

                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Author: ThongNT
        ///     <para></para>
        ///     Convert byte array to hexa string
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        private static string ConvertByteToString(IEnumerable<byte> buff)
        {
            return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
        }
        #endregion HMAC SHA 256

        #region TripDES

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2015-07-16</para>
        ///     <para>Description: Giai ma TripDES</para>
        /// </summary>
        /// <returns></returns>
        public static string DescryptTripDes(string stringToDecrypt, string securityKey, bool isUseHashing = true)
        {
            byte[] keyArray;
            var toEncryptArray = Convert.FromBase64String(stringToDecrypt);
            var key = securityKey;

            if (isUseHashing)
            {
                //if hashing was used get the hash code with regards to your key
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));

                //release any resource held by the MD5CryptoServiceProvider
                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                //set the secret key for the tripleDES algorithm
                Key = keyArray,

                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                Mode = CipherMode.ECB,

                //padding mode(if any extra byte added)
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock
                (toEncryptArray, 0, toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor
            tdes.Clear();

            //return the Clear decrypted TEXT
            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        ///     <para>Author: TrungTT</para>
        ///     <para>Date: 2015-07-16</para>
        ///     <para>Description: Ma hoa TripDES</para>
        /// </summary>
        /// <returns></returns>
        public static string EncryptTripDes(string stringToEncrypt, string securityKey, bool isUseHashing = true)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            var key = securityKey;

            if (isUseHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                //set the secret key for the tripleDES algorithm
                Key = keyArray,
                //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
                Mode = CipherMode.ECB,
                //padding mode(if any extra byte added)
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateEncryptor();

            //transform the specified region of bytes array to resultArray
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor
            tdes.Clear();

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion TripDES

        public static TimeSpan ConvetDateTimeToTimeSpan(DateTime dateTime)
        {
            var dateBetween = DateTime.Now - dateTime;
            return dateBetween;
        }

        /// <summary>
        ///     Mã hóa mật khẩu (mã hóa 1 chiều)
        /// </summary>
        /// <param name="cleanString">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi sau khi giải mã</returns>
        public static string Encrypt(string cleanString)
        {
            var clearBytes = new UnicodeEncoding().GetBytes(cleanString);
            var hashedBytes = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            hashedBytes = ((HashAlgorithm) CryptoConfig.CreateFromName("SHA1")).ComputeHash(hashedBytes);

            return BitConverter.ToString(hashedBytes);
        }
        #endregion ma hoa

        #region Triple Des

        public static string DecryptTripleDes(string cipherString, string key, bool useHashing = true)
        {
            byte[] keyArray;
            //get the byte code of the string

            var toEncryptArray = Convert.FromBase64String(cipherString);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            ////Get your key from config file to open the lock!
            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            //padding mode(if any extra byte added)

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock
                (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return Encoding.UTF8.GetString(resultArray);
        }

        public static string EncryptTripleDes(string toEncrypt, string key, bool useHashing = true)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //// Get the key from config file

            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                //of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            //padding mode(if any extra byte added)

            var cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            var resultArray = cTransform.TransformFinalBlock
                (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion Triple Des


        #region ma hoa base64

        public static string DecryptBase64(string cipherString)
        {
            var toEncryptArray = Convert.FromBase64String(cipherString);
            return Encoding.UTF8.GetString(toEncryptArray);
        }

        public static string EncryptBase64(string toEncrypt)
        {
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            return Convert.ToBase64String(toEncryptArray, 0, toEncryptArray.Length);
        }
        #endregion ma hoa base64
    }
}