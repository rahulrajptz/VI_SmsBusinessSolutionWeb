namespace Templateprj.Helpers
{
    using System;
    using System.Text;

    public static class StringHelper
    {
        public static string ConvertToUnicode(this string codeforConversion)
        {
            byte[] unibyte = Encoding.Unicode.GetBytes(codeforConversion.Trim());
            string uniString = string.Empty;
            string tmp = string.Empty;
            int i = 0;
            foreach (byte b in unibyte)
            {
                if (i == 0)
                {
                    tmp = string.Format("{0}{1}", @"", b.ToString("X2"));
                    i = 1;
                }
                else
                {
                    uniString += string.Format("{0}{1}", @"", b.ToString("X2")) + tmp;
                    i = 0;
                }
            }
            return uniString;
        }

        public static string ConvertToHex(this string codeforConversion)
        {
            byte[] bytes = Encoding.Default.GetBytes(codeforConversion);
            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", "");
            return hexString;
        }
    }
}