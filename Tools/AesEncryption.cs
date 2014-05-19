using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Tools
{
	public class AesEncryption
	{
		public Encoding Utf8 { get { return Encoding.UTF8; } }
		static Encoding Iso88591 { get { return Encoding.GetEncoding("ISO-8859-1"); } }

		private byte[] prepareKey(byte[] password)
		{
			MD5 hasher = MD5.Create();
			byte[] passwordBytesHashed = hasher.ComputeHash(password);

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < passwordBytesHashed.Length; i++)
			{
				sb.Append(passwordBytesHashed[i].ToString("X2"));
			}
			return Utf8.GetBytes(sb.ToString().ToLower());
		}


		public RijndaelManaged GetRijndaelManaged(string secretKey)
		{
			var secretKeyBytes = prepareKey(Utf8.GetBytes(secretKey));

			return new RijndaelManaged
			{
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7,
				BlockSize = 128,
				Key = secretKeyBytes
			};
		}

		private byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
		{
			return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
		}

		private byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
		{
			return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
		}

		public string Encrypt(string plainText, string key)
		{
			var plainBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key))).Replace("+", "-").Replace("/", "_");
		}

		public string Decrypt(string encryptedText, string key)
		{
			try
			{
				encryptedText = encryptedText.Replace("-", "+").Replace("_", "/");
				var encryptedBytes = Convert.FromBase64String(encryptedText);
				byte[] temp = Decrypt(encryptedBytes, GetRijndaelManaged(key));
				return Utf8.GetString(temp);
			}
			catch
			{
				return "-1";
			}
		}
	}
}