using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Templateprj.Helpers
{
		public class CryptoAlg
		{
				private string iv = "0123456789ABCDEF";

				#region Encryption
				public string EncryptDes(string toEncrypt)
				{
						return EncryptDes(toEncrypt, GlobalValues.Key);
				}

				public string EncryptDes(string toEncrypt, string key)
				{
						string cypherData;
						try
						{

								byte[] IVector = StringToByteArray(iv);

								byte[] keyArray = new byte[key.Length];
								keyArray = ASCIIEncoding.ASCII.GetBytes(key);

								byte[] toEncryptArray = new byte[toEncrypt.Length];
								toEncryptArray = ASCIIEncoding.ASCII.GetBytes(toEncrypt);

								MemoryStream tempMs = new MemoryStream();
										TripleDES tDes = TripleDES.Create();
										tDes.Padding = PaddingMode.ANSIX923;
										tDes.Key = keyArray;
										tDes.IV = IVector;

										CryptoStream cs = new CryptoStream(tempMs, tDes.CreateEncryptor(), CryptoStreamMode.Write);
										cs.Write(toEncryptArray, 0, toEncryptArray.Length);
										cs.Close();
										byte[] encryptedData = tempMs.ToArray();

										cypherData = ByteArrayToString(encryptedData);
						}
						catch
						{
								return "";
						}
						return cypherData;
				}

				#endregion

				#region Decrypyion
				public string DecryptDes(string cipherString)
				{
						return DecryptDes(cipherString, GlobalValues.Key);
				}

				public string DecryptDes(string cipherString, string key)
				{
						string decData = "";
						try
						{

								byte[] IVector = StringToByteArray(iv);

								byte[] keyArray = new byte[key.Length];
								keyArray = ASCIIEncoding.ASCII.GetBytes(key);

								byte[] toDecrypteArray;
								toDecrypteArray = StringToByteArray(cipherString);

								string a = ByteArrayToString(toDecrypteArray);
								toDecrypteArray = StringToByteArray(a);

								MemoryStream tempMs = new MemoryStream();
								TripleDES tDes = TripleDES.Create();
								tDes.Padding = PaddingMode.ANSIX923;
								tDes.Key = keyArray;
								tDes.IV = IVector;

								CryptoStream cs = new CryptoStream(tempMs, tDes.CreateDecryptor(), CryptoStreamMode.Write);
								cs.Write(toDecrypteArray, 0, toDecrypteArray.Length);
								cs.Close();
								byte[] decryptedData = tempMs.ToArray();

								decData = Encoding.ASCII.GetString(decryptedData);

						}
						catch { return ""; }
						return decData;
				}

				#endregion

				#region Encription SHA1
				public string GetHashSha1(string plainText)
				{
						byte[] hashValue;
						byte[] plainTxt = ASCIIEncoding.ASCII.GetBytes(plainText);

						SHA1Managed hashString = new SHA1Managed();
						string cypherText = "";
						hashValue = hashString.ComputeHash(plainTxt);
						cypherText = ByteArrayToString(hashValue);
						return cypherText;

				}
				#endregion

				#region others
				private byte[] HexStringToByteArray(string hexString)
				{
						int hexStringLength = hexString.Length;
						byte[] b = new byte[hexStringLength / 2];
						for (int i = 0; i < hexStringLength; i += 2)
						{
								int topChar = (hexString[i] > 0x40 ? hexString[i] - 0x37 : hexString[i] - 0x30) << 4;
								int bottomChar = hexString[i + 1] > 0x40 ? hexString[i + 1] - 0x37 : hexString[i + 1] - 0x30;
								b[i / 2] = Convert.ToByte(topChar + bottomChar);
						}
						return b;
				}

				public static string ByteArrayToString(byte[] ba)
				{
						StringBuilder hex = new StringBuilder(ba.Length * 2);
						foreach (byte b in ba)
								hex.AppendFormat("{0:x2}", b);
						return hex.ToString();
				}

				public static byte[] StringToByteArray(String hex)
				{
						int NumberChars = hex.Length;
						byte[] bytes = new byte[NumberChars / 2];
						for (int i = 0; i < NumberChars; i += 2)
								bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
						return bytes;
				}

				#endregion
		}
}