using System;
using System.Security.Cryptography;
using System.Text;

namespace Sabresaurus.PlayerPrefsExtensions
{
	// Token: 0x020009FB RID: 2555
	public static class SimpleEncryption
	{
		// Token: 0x06004DBE RID: 19902 RVA: 0x001A12AB File Offset: 0x0019F6AB
		private static void SetupProvider()
		{
			SimpleEncryption.provider = new RijndaelManaged();
			SimpleEncryption.provider.Key = Encoding.ASCII.GetBytes(SimpleEncryption.key);
			SimpleEncryption.provider.Mode = CipherMode.ECB;
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x001A12DC File Offset: 0x0019F6DC
		public static string EncryptString(string sourceString)
		{
			if (SimpleEncryption.provider == null)
			{
				SimpleEncryption.SetupProvider();
			}
			ICryptoTransform cryptoTransform = SimpleEncryption.provider.CreateEncryptor();
			byte[] bytes = Encoding.UTF8.GetBytes(sourceString);
			byte[] inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
			return Convert.ToBase64String(inArray);
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x001A1324 File Offset: 0x0019F724
		public static string DecryptString(string sourceString)
		{
			if (SimpleEncryption.provider == null)
			{
				SimpleEncryption.SetupProvider();
			}
			ICryptoTransform cryptoTransform = SimpleEncryption.provider.CreateDecryptor();
			byte[] array = Convert.FromBase64String(sourceString);
			byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			return Encoding.UTF8.GetString(bytes);
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x001A136C File Offset: 0x0019F76C
		public static string EncryptFloat(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			string sourceString = Convert.ToBase64String(bytes);
			return SimpleEncryption.EncryptString(sourceString);
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x001A1390 File Offset: 0x0019F790
		public static string EncryptInt(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			string sourceString = Convert.ToBase64String(bytes);
			return SimpleEncryption.EncryptString(sourceString);
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x001A13B4 File Offset: 0x0019F7B4
		public static float DecryptFloat(string sourceString)
		{
			string s = SimpleEncryption.DecryptString(sourceString);
			byte[] value = Convert.FromBase64String(s);
			return BitConverter.ToSingle(value, 0);
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x001A13D8 File Offset: 0x0019F7D8
		public static int DecryptInt(string sourceString)
		{
			string s = SimpleEncryption.DecryptString(sourceString);
			byte[] value = Convert.FromBase64String(s);
			return BitConverter.ToInt32(value, 0);
		}

		// Token: 0x040035BB RID: 13755
		private static string key = ":{j%6j?E:t#}G10mM%9hp5S=%}2,Y26C";

		// Token: 0x040035BC RID: 13756
		private static RijndaelManaged provider;
	}
}
