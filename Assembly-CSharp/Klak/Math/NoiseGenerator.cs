using System;
using UnityEngine;

namespace Klak.Math
{
	// Token: 0x02000520 RID: 1312
	internal struct NoiseGenerator
	{
		// Token: 0x06002E11 RID: 11793 RVA: 0x000E1A3C File Offset: 0x000DFE3C
		public NoiseGenerator(float frequency)
		{
			this._hash1 = XXHash.RandomHash;
			this._hash2 = XXHash.RandomHash;
			this._hash3 = XXHash.RandomHash;
			this._fractal = 2;
			this._frequency = frequency;
			this._time = 0f;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000E1A78 File Offset: 0x000DFE78
		public NoiseGenerator(int seed, float frequency)
		{
			this._hash1 = new XXHash(seed);
			this._hash2 = new XXHash(seed ^ 321341786);
			this._hash3 = new XXHash(seed ^ 1019118834);
			this._fractal = 2;
			this._frequency = frequency;
			this._time = 0f;
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002E13 RID: 11795 RVA: 0x000E1ACE File Offset: 0x000DFECE
		// (set) Token: 0x06002E14 RID: 11796 RVA: 0x000E1AD6 File Offset: 0x000DFED6
		public int FractalLevel
		{
			get
			{
				return this._fractal;
			}
			set
			{
				this._fractal = value;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002E15 RID: 11797 RVA: 0x000E1ADF File Offset: 0x000DFEDF
		// (set) Token: 0x06002E16 RID: 11798 RVA: 0x000E1AE7 File Offset: 0x000DFEE7
		public float Frequency
		{
			get
			{
				return this._frequency;
			}
			set
			{
				this._frequency = value;
			}
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000E1AF0 File Offset: 0x000DFEF0
		public void Step()
		{
			this._time += this._frequency * Time.deltaTime;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000E1B0C File Offset: 0x000DFF0C
		public float Value01(int seed2)
		{
			float num = this._hash1.Range(-100f, 100f, seed2);
			return Perlin.Fbm(this._time + num, this._fractal) * 1.33333337f * 0.5f + 0.5f;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000E1B58 File Offset: 0x000DFF58
		public float Value(int seed2)
		{
			float num = this._hash1.Range(-100f, 100f, seed2);
			return Perlin.Fbm(this._time + num, this._fractal) * 1.33333337f;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000E1B98 File Offset: 0x000DFF98
		public Vector3 Vector(int seed2)
		{
			float num = this._hash1.Range(-100f, 100f, seed2);
			float num2 = this._hash2.Range(-100f, 100f, seed2);
			float num3 = this._hash3.Range(-100f, 100f, seed2);
			return new Vector3(Perlin.Fbm(this._time + num, this._fractal) * 1.33333337f, Perlin.Fbm(this._time + num2, this._fractal) * 1.33333337f, Perlin.Fbm(this._time + num3, this._fractal) * 1.33333337f);
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000E1C3C File Offset: 0x000E003C
		public Quaternion Rotation(int seed2, float angle)
		{
			float num = this._hash1.Range(-100f, 100f, seed2);
			float num2 = this._hash2.Range(-100f, 100f, seed2);
			float num3 = this._hash3.Range(-100f, 100f, seed2);
			return Quaternion.Euler(Perlin.Fbm(this._time + num, this._fractal) * 1.33333337f * angle, Perlin.Fbm(this._time + num2, this._fractal) * 1.33333337f * angle, Perlin.Fbm(this._time + num3, this._fractal) * 1.33333337f * angle);
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000E1CE4 File Offset: 0x000E00E4
		public Quaternion Rotation(int seed2, float rx, float ry, float rz)
		{
			float num = this._hash1.Range(-100f, 100f, seed2);
			float num2 = this._hash2.Range(-100f, 100f, seed2);
			float num3 = this._hash3.Range(-100f, 100f, seed2);
			return Quaternion.Euler(Perlin.Fbm(this._time + num, this._fractal) * 1.33333337f * rx, Perlin.Fbm(this._time + num2, this._fractal) * 1.33333337f * ry, Perlin.Fbm(this._time + num3, this._fractal) * 1.33333337f * rz);
		}

		// Token: 0x0400185F RID: 6239
		private const float _fbmNorm = 1.33333337f;

		// Token: 0x04001860 RID: 6240
		private XXHash _hash1;

		// Token: 0x04001861 RID: 6241
		private XXHash _hash2;

		// Token: 0x04001862 RID: 6242
		private XXHash _hash3;

		// Token: 0x04001863 RID: 6243
		private int _fractal;

		// Token: 0x04001864 RID: 6244
		private float _frequency;

		// Token: 0x04001865 RID: 6245
		private float _time;
	}
}
