using System;
using System.Collections.Generic;

namespace MoPhoGames.USpeak.Core.Utils
{
	// Token: 0x020009D9 RID: 2521
	public class USpeakPoolUtils
	{
		// Token: 0x06004CB2 RID: 19634 RVA: 0x0019B7A8 File Offset: 0x00199BA8
		public static float[] GetFloat(int length)
		{
			for (int i = 0; i < USpeakPoolUtils.FloatPool.Count; i++)
			{
				if (USpeakPoolUtils.FloatPool[i].Length == length)
				{
					float[] result = USpeakPoolUtils.FloatPool[i];
					USpeakPoolUtils.FloatPool.RemoveAt(i);
					return result;
				}
			}
			return new float[length];
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x0019B804 File Offset: 0x00199C04
		public static short[] GetShort(int length)
		{
			for (int i = 0; i < USpeakPoolUtils.ShortPool.Count; i++)
			{
				if (USpeakPoolUtils.ShortPool[i].Length == length)
				{
					short[] result = USpeakPoolUtils.ShortPool[i];
					USpeakPoolUtils.ShortPool.RemoveAt(i);
					return result;
				}
			}
			return new short[length];
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x0019B860 File Offset: 0x00199C60
		public static int[] GetInt(int length)
		{
			for (int i = 0; i < USpeakPoolUtils.IntPool.Count; i++)
			{
				if (USpeakPoolUtils.IntPool[i].Length == length)
				{
					int[] result = USpeakPoolUtils.IntPool[i];
					USpeakPoolUtils.IntPool.RemoveAt(i);
					return result;
				}
			}
			return new int[length];
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x0019B8BC File Offset: 0x00199CBC
		public static byte[] GetByte(int length)
		{
			for (int i = 0; i < USpeakPoolUtils.BytePool.Count; i++)
			{
				if (USpeakPoolUtils.BytePool[i].Length == length)
				{
					byte[] result = USpeakPoolUtils.BytePool[i];
					USpeakPoolUtils.BytePool.RemoveAt(i);
					return result;
				}
			}
			return new byte[length];
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0019B916 File Offset: 0x00199D16
		public static void Return(float[] d)
		{
			USpeakPoolUtils.FloatPool.Add(d);
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x0019B923 File Offset: 0x00199D23
		public static void Return(byte[] d)
		{
			USpeakPoolUtils.BytePool.Add(d);
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x0019B930 File Offset: 0x00199D30
		public static void Return(short[] d)
		{
			USpeakPoolUtils.ShortPool.Add(d);
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x0019B93D File Offset: 0x00199D3D
		public static void Return(int[] d)
		{
			USpeakPoolUtils.IntPool.Add(d);
		}

		// Token: 0x040034D5 RID: 13525
		private static List<byte[]> BytePool = new List<byte[]>();

		// Token: 0x040034D6 RID: 13526
		private static List<short[]> ShortPool = new List<short[]>();

		// Token: 0x040034D7 RID: 13527
		private static List<int[]> IntPool = new List<int[]>();

		// Token: 0x040034D8 RID: 13528
		private static List<float[]> FloatPool = new List<float[]>();
	}
}
