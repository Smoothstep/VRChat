using System;
using System.IO;
using System.Security.Cryptography;

namespace PublicKeyConvert
{
	// Token: 0x0200097A RID: 2426
	public class PEMKeyLoader
	{
		// Token: 0x06004989 RID: 18825 RVA: 0x00189F6C File Offset: 0x0018836C
		private static bool CompareBytearrays(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			int num = 0;
			foreach (byte b2 in a)
			{
				if (b2 != b[num])
				{
					return false;
				}
				num++;
			}
			return true;
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x00189FB4 File Offset: 0x001883B4
		public static RSACryptoServiceProvider CryptoServiceProviderFromPublicKeyInfo(byte[] x509key)
		{
			byte[] a = new byte[15];
			if (x509key == null || x509key.Length == 0)
			{
				return null;
			}
			MemoryStream input = new MemoryStream(x509key);
			BinaryReader binaryReader = new BinaryReader(input);
			RSACryptoServiceProvider result;
			try
			{
				ushort num = binaryReader.ReadUInt16();
				if (num == 33072)
				{
					binaryReader.ReadByte();
				}
				else
				{
					if (num != 33328)
					{
						return null;
					}
					binaryReader.ReadInt16();
				}
				a = binaryReader.ReadBytes(15);
				if (!PEMKeyLoader.CompareBytearrays(a, PEMKeyLoader.SeqOID))
				{
					result = null;
				}
				else
				{
					num = binaryReader.ReadUInt16();
					if (num == 33027)
					{
						binaryReader.ReadByte();
					}
					else
					{
						if (num != 33283)
						{
							return null;
						}
						binaryReader.ReadInt16();
					}
					byte b = binaryReader.ReadByte();
					if (b != 0)
					{
						result = null;
					}
					else
					{
						num = binaryReader.ReadUInt16();
						if (num == 33072)
						{
							binaryReader.ReadByte();
						}
						else
						{
							if (num != 33328)
							{
								return null;
							}
							binaryReader.ReadInt16();
						}
						num = binaryReader.ReadUInt16();
						byte b2 = 0;
						byte b3;
						if (num == 33026)
						{
							b3 = binaryReader.ReadByte();
						}
						else
						{
							if (num != 33282)
							{
								return null;
							}
							b2 = binaryReader.ReadByte();
							b3 = binaryReader.ReadByte();
						}
						byte[] array = new byte[4];
						array[0] = b3;
						array[1] = b2;
						byte[] value = array;
						int num2 = BitConverter.ToInt32(value, 0);
						if (binaryReader.PeekChar() == 0)
						{
							binaryReader.ReadByte();
							num2--;
						}
						byte[] modulus = binaryReader.ReadBytes(num2);
						if (binaryReader.ReadByte() != 2)
						{
							result = null;
						}
						else
						{
							int count = (int)binaryReader.ReadByte();
							byte[] exponent = binaryReader.ReadBytes(count);
							RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
							rsacryptoServiceProvider.ImportParameters(new RSAParameters
							{
								Modulus = modulus,
								Exponent = exponent
							});
							result = rsacryptoServiceProvider;
						}
					}
				}
			}
			finally
			{
				binaryReader.Close();
			}
			return result;
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x0018A1F4 File Offset: 0x001885F4
		public static RSACryptoServiceProvider CryptoServiceProviderFromPublicKeyInfo(string base64EncodedKey)
		{
			try
			{
				return PEMKeyLoader.CryptoServiceProviderFromPublicKeyInfo(Convert.FromBase64String(base64EncodedKey));
			}
			catch (FormatException)
			{
			}
			return null;
		}

		// Token: 0x040031E5 RID: 12773
		private static byte[] SeqOID = new byte[]
		{
			48,
			13,
			6,
			9,
			42,
			134,
			72,
			134,
			247,
			13,
			1,
			1,
			1,
			5,
			0
		};
	}
}
