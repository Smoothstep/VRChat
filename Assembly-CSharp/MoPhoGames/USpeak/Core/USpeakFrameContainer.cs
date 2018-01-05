using System;

namespace MoPhoGames.USpeak.Core
{
	// Token: 0x020009D8 RID: 2520
	public struct USpeakFrameContainer
	{
		// Token: 0x06004CAE RID: 19630 RVA: 0x0019B6D8 File Offset: 0x00199AD8
		public int LoadFrom(byte[] source, int sourceOffset)
		{
			this.FrameIndex = BitConverter.ToUInt16(source, sourceOffset);
			int num = sourceOffset + 2;
			int num2 = BitConverter.ToInt32(source, num);
			num += 4;
			this.encodedData = new byte[num2];
			Array.Copy(source, num, this.encodedData, 0, num2);
			return num2 + 6;
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x0019B724 File Offset: 0x00199B24
		public static ushort ParseFrameIndex(byte[] source, int sourceOffset)
		{
			return BitConverter.ToUInt16(source, sourceOffset);
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x0019B73C File Offset: 0x00199B3C
		public byte[] ToByteArray()
		{
			byte[] array = new byte[6 + this.encodedData.Length];
			int num = 0;
			byte[] bytes = BitConverter.GetBytes(this.FrameIndex);
			Array.Copy(bytes, 0, array, num, 2);
			num += 2;
			byte[] bytes2 = BitConverter.GetBytes(this.encodedData.Length);
			bytes2.CopyTo(array, num);
			num += 4;
			this.encodedData.CopyTo(array, num);
			return array;
		}

		// Token: 0x040034D3 RID: 13523
		public ushort FrameIndex;

		// Token: 0x040034D4 RID: 13524
		public byte[] encodedData;
	}
}
