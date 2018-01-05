using System;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x02000521 RID: 1313
	public static class Perlin
	{
		// Token: 0x06002E1D RID: 11805 RVA: 0x000E1D90 File Offset: 0x000E0190
		public static float Noise(float x)
		{
			int num = Mathf.FloorToInt(x) & 255;
			x -= Mathf.Floor(x);
			float t = Perlin.Fade(x);
			return Perlin.Lerp(t, Perlin.Grad(Perlin.perm[num], x), Perlin.Grad(Perlin.perm[num + 1], x - 1f)) * 2f;
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000E1DEC File Offset: 0x000E01EC
		public static float Noise(float x, float y)
		{
			int num = Mathf.FloorToInt(x) & 255;
			int num2 = Mathf.FloorToInt(y) & 255;
			x -= Mathf.Floor(x);
			y -= Mathf.Floor(y);
			float t = Perlin.Fade(x);
			float t2 = Perlin.Fade(y);
			int num3 = Perlin.perm[num] + num2 & 255;
			int num4 = Perlin.perm[num + 1] + num2 & 255;
			return Perlin.Lerp(t2, Perlin.Lerp(t, Perlin.Grad(Perlin.perm[num3], x, y), Perlin.Grad(Perlin.perm[num4], x - 1f, y)), Perlin.Lerp(t, Perlin.Grad(Perlin.perm[num3 + 1], x, y - 1f), Perlin.Grad(Perlin.perm[num4 + 1], x - 1f, y - 1f)));
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000E1EC3 File Offset: 0x000E02C3
		public static float Noise(Vector2 coord)
		{
			return Perlin.Noise(coord.x, coord.y);
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000E1ED8 File Offset: 0x000E02D8
		public static float Noise(float x, float y, float z)
		{
			int num = Mathf.FloorToInt(x) & 255;
			int num2 = Mathf.FloorToInt(y) & 255;
			int num3 = Mathf.FloorToInt(z) & 255;
			x -= Mathf.Floor(x);
			y -= Mathf.Floor(y);
			z -= Mathf.Floor(z);
			float t = Perlin.Fade(x);
			float t2 = Perlin.Fade(y);
			float t3 = Perlin.Fade(z);
			int num4 = Perlin.perm[num] + num2 & 255;
			int num5 = Perlin.perm[num + 1] + num2 & 255;
			int num6 = Perlin.perm[num4] + num3 & 255;
			int num7 = Perlin.perm[num5] + num3 & 255;
			int num8 = Perlin.perm[num4 + 1] + num3 & 255;
			int num9 = Perlin.perm[num5 + 1] + num3 & 255;
			return Perlin.Lerp(t3, Perlin.Lerp(t2, Perlin.Lerp(t, Perlin.Grad(Perlin.perm[num6], x, y, z), Perlin.Grad(Perlin.perm[num7], x - 1f, y, z)), Perlin.Lerp(t, Perlin.Grad(Perlin.perm[num8], x, y - 1f, z), Perlin.Grad(Perlin.perm[num9], x - 1f, y - 1f, z))), Perlin.Lerp(t2, Perlin.Lerp(t, Perlin.Grad(Perlin.perm[num6 + 1], x, y, z - 1f), Perlin.Grad(Perlin.perm[num7 + 1], x - 1f, y, z - 1f)), Perlin.Lerp(t, Perlin.Grad(Perlin.perm[num8 + 1], x, y - 1f, z - 1f), Perlin.Grad(Perlin.perm[num9 + 1], x - 1f, y - 1f, z - 1f))));
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000E20AE File Offset: 0x000E04AE
		public static float Noise(Vector3 coord)
		{
			return Perlin.Noise(coord.x, coord.y, coord.z);
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000E20CC File Offset: 0x000E04CC
		public static float Fbm(float x, int octave)
		{
			float num = 0f;
			float num2 = 0.5f;
			for (int i = 0; i < octave; i++)
			{
				num += num2 * Perlin.Noise(x);
				x *= 2f;
				num2 *= 0.5f;
			}
			return num;
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000E2114 File Offset: 0x000E0514
		public static float Fbm(Vector2 coord, int octave)
		{
			float num = 0f;
			float num2 = 0.5f;
			for (int i = 0; i < octave; i++)
			{
				num += num2 * Perlin.Noise(coord);
				coord *= 2f;
				num2 *= 0.5f;
			}
			return num;
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000E2160 File Offset: 0x000E0560
		public static float Fbm(float x, float y, int octave)
		{
			return Perlin.Fbm(new Vector2(x, y), octave);
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000E2170 File Offset: 0x000E0570
		public static float Fbm(Vector3 coord, int octave)
		{
			float num = 0f;
			float num2 = 0.5f;
			for (int i = 0; i < octave; i++)
			{
				num += num2 * Perlin.Noise(coord);
				coord *= 2f;
				num2 *= 0.5f;
			}
			return num;
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000E21BC File Offset: 0x000E05BC
		public static float Fbm(float x, float y, float z, int octave)
		{
			return Perlin.Fbm(new Vector3(x, y, z), octave);
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000E21CC File Offset: 0x000E05CC
		private static float Fade(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000E21E9 File Offset: 0x000E05E9
		private static float Lerp(float t, float a, float b)
		{
			return a + t * (b - a);
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000E21F2 File Offset: 0x000E05F2
		private static float Grad(int hash, float x)
		{
			return ((hash & 1) != 0) ? (-x) : x;
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000E2204 File Offset: 0x000E0604
		private static float Grad(int hash, float x, float y)
		{
			return (((hash & 1) != 0) ? (-x) : x) + (((hash & 2) != 0) ? (-y) : y);
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000E2228 File Offset: 0x000E0628
		private static float Grad(int hash, float x, float y, float z)
		{
			int num = hash & 15;
			float num2 = (num >= 8) ? y : x;
			float num3 = (num >= 4) ? ((num != 12 && num != 14) ? z : x) : y;
			return (((num & 1) != 0) ? (-num2) : num2) + (((num & 2) != 0) ? (-num3) : num3);
		}

		// Token: 0x04001866 RID: 6246
		private static int[] perm = new int[]
		{
			151,
			160,
			137,
			91,
			90,
			15,
			131,
			13,
			201,
			95,
			96,
			53,
			194,
			233,
			7,
			225,
			140,
			36,
			103,
			30,
			69,
			142,
			8,
			99,
			37,
			240,
			21,
			10,
			23,
			190,
			6,
			148,
			247,
			120,
			234,
			75,
			0,
			26,
			197,
			62,
			94,
			252,
			219,
			203,
			117,
			35,
			11,
			32,
			57,
			177,
			33,
			88,
			237,
			149,
			56,
			87,
			174,
			20,
			125,
			136,
			171,
			168,
			68,
			175,
			74,
			165,
			71,
			134,
			139,
			48,
			27,
			166,
			77,
			146,
			158,
			231,
			83,
			111,
			229,
			122,
			60,
			211,
			133,
			230,
			220,
			105,
			92,
			41,
			55,
			46,
			245,
			40,
			244,
			102,
			143,
			54,
			65,
			25,
			63,
			161,
			1,
			216,
			80,
			73,
			209,
			76,
			132,
			187,
			208,
			89,
			18,
			169,
			200,
			196,
			135,
			130,
			116,
			188,
			159,
			86,
			164,
			100,
			109,
			198,
			173,
			186,
			3,
			64,
			52,
			217,
			226,
			250,
			124,
			123,
			5,
			202,
			38,
			147,
			118,
			126,
			255,
			82,
			85,
			212,
			207,
			206,
			59,
			227,
			47,
			16,
			58,
			17,
			182,
			189,
			28,
			42,
			223,
			183,
			170,
			213,
			119,
			248,
			152,
			2,
			44,
			154,
			163,
			70,
			221,
			153,
			101,
			155,
			167,
			43,
			172,
			9,
			129,
			22,
			39,
			253,
			19,
			98,
			108,
			110,
			79,
			113,
			224,
			232,
			178,
			185,
			112,
			104,
			218,
			246,
			97,
			228,
			251,
			34,
			242,
			193,
			238,
			210,
			144,
			12,
			191,
			179,
			162,
			241,
			81,
			51,
			145,
			235,
			249,
			14,
			239,
			107,
			49,
			192,
			214,
			31,
			181,
			199,
			106,
			157,
			184,
			84,
			204,
			176,
			115,
			121,
			50,
			45,
			127,
			4,
			150,
			254,
			138,
			236,
			205,
			93,
			222,
			114,
			67,
			29,
			24,
			72,
			243,
			141,
			128,
			195,
			78,
			66,
			215,
			61,
			156,
			180,
			151
		};
	}
}
