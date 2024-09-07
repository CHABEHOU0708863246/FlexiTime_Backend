using System.Security.Cryptography;

namespace FlexiTime_Backend.Utilities.Security
{
    public static class CryptographyHashing
    {
        /// <summary>
        /// Compute a MD5 from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ComputeMd5(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
