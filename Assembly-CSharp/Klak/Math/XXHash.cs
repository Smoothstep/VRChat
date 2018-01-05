using System;

namespace Klak.Math
{
	// Token: 0x02000527 RID: 1319
	public struct XXHash
	{
		// Token: 0x06002E42 RID: 11842 RVA: 0x000E2632 File Offset: 0x000E0A32
		public XXHash(int seed)
		{
			this.seed = seed;
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000E263B File Offset: 0x000E0A3B
		private static uint rotl32(uint x, int r)
		{
			return x << r | x >> 32 - r;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000E2650 File Offset: 0x000E0A50
		public static uint GetHash(int data, int seed)
		{
			uint num = (uint)(seed + 374761393);
			num += 4u;
			num += (uint)(data * -1028477379);
			num = XXHash.rotl32(num, 17) * 668265263u;
			num ^= num >> 15;
			num *= 2246822519u;
			num ^= num >> 13;
			num *= 3266489917u;
			return num ^ num >> 16;
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002E45 RID: 11845 RVA: 0x000E26A8 File Offset: 0x000E0AA8
		public static XXHash RandomHash
		{
			get
			{
				return new XXHash((int)XXHash.GetHash(51966, XXHash._counter++));
			}
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000E26C6 File Offset: 0x000E0AC6
		public uint GetHash(int data)
		{
			return XXHash.GetHash(data, this.seed);
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x000E26D4 File Offset: 0x000E0AD4
		public int Range(int max, int data)
		{
			return (int)(this.GetHash(data) % (uint)max);
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x000E26DF File Offset: 0x000E0ADF
		public int Range(int min, int max, int data)
		{
			return (int)(this.GetHash(data) % (uint)(max - min) + (uint)min);
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000E26EE File Offset: 0x000E0AEE
		public float Value01(int data)
		{
			return this.GetHash(data) / 4.2949673E+09f;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x000E26FF File Offset: 0x000E0AFF
		public float Range(float min, float max, int data)
		{
			return this.Value01(data) * (max - min) + min;
		}

		// Token: 0x04001873 RID: 6259
		private const uint PRIME32_1 = 2654435761u;

		// Token: 0x04001874 RID: 6260
		private const uint PRIME32_2 = 2246822519u;

		// Token: 0x04001875 RID: 6261
		private const uint PRIME32_3 = 3266489917u;

		// Token: 0x04001876 RID: 6262
		private const uint PRIME32_4 = 668265263u;

		// Token: 0x04001877 RID: 6263
		private const uint PRIME32_5 = 374761393u;

		// Token: 0x04001878 RID: 6264
		private static int _counter;

		// Token: 0x04001879 RID: 6265
		public int seed;
	}
}
