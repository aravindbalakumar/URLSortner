namespace URLSortner.Utils
{


    using System.Security.Cryptography;
    using System.Text;

    public static class Extensions
    {
        private const string Base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ShortenURL(this string url, int length = 8)
        {
            return GenerateShortCode(url);
        }
        private static string GenerateShortCode(this string url)
        {
            // 1. Hash the URL
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(url));

            // 2. Convert first bytes to Base62
            return ToBase62(hash);
        }

        private static string ToBase62(this byte[] bytes)
        {
            ulong value = BitConverter.ToUInt64(bytes, 0);
            var result = new StringBuilder();

            while (value > 0)
            {
                result.Insert(0, Base62Chars[(int)(value % 62)]);
                value /= 62;
            }

            return result.ToString();
        }
    }

}
