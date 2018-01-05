using System;
using Klak.Math;
using UnityEngine;

namespace Klak.Motion
{
	// Token: 0x02000528 RID: 1320
	[AddComponentMenu("Klak/Motion/Brownian Motion")]
	public class BrownianMotion : MonoBehaviour
	{
		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002E4C RID: 11852 RVA: 0x000E2790 File Offset: 0x000E0B90
		// (set) Token: 0x06002E4D RID: 11853 RVA: 0x000E2798 File Offset: 0x000E0B98
		public bool enablePositionNoise
		{
			get
			{
				return this._enablePositionNoise;
			}
			set
			{
				this._enablePositionNoise = value;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002E4E RID: 11854 RVA: 0x000E27A1 File Offset: 0x000E0BA1
		// (set) Token: 0x06002E4F RID: 11855 RVA: 0x000E27A9 File Offset: 0x000E0BA9
		public bool enableRotationNoise
		{
			get
			{
				return this._enableRotationNoise;
			}
			set
			{
				this._enableRotationNoise = value;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x000E27B2 File Offset: 0x000E0BB2
		// (set) Token: 0x06002E51 RID: 11857 RVA: 0x000E27BA File Offset: 0x000E0BBA
		public float positionFrequency
		{
			get
			{
				return this._positionFrequency;
			}
			set
			{
				this._positionFrequency = value;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002E52 RID: 11858 RVA: 0x000E27C3 File Offset: 0x000E0BC3
		// (set) Token: 0x06002E53 RID: 11859 RVA: 0x000E27CB File Offset: 0x000E0BCB
		public float rotationFrequency
		{
			get
			{
				return this._rotationFrequency;
			}
			set
			{
				this._rotationFrequency = value;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002E54 RID: 11860 RVA: 0x000E27D4 File Offset: 0x000E0BD4
		// (set) Token: 0x06002E55 RID: 11861 RVA: 0x000E27DC File Offset: 0x000E0BDC
		public float positionAmplitude
		{
			get
			{
				return this._positionAmplitude;
			}
			set
			{
				this._positionAmplitude = value;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x000E27E5 File Offset: 0x000E0BE5
		// (set) Token: 0x06002E57 RID: 11863 RVA: 0x000E27ED File Offset: 0x000E0BED
		public float rotationAmplitude
		{
			get
			{
				return this._rotationAmplitude;
			}
			set
			{
				this._rotationAmplitude = value;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002E58 RID: 11864 RVA: 0x000E27F6 File Offset: 0x000E0BF6
		// (set) Token: 0x06002E59 RID: 11865 RVA: 0x000E27FE File Offset: 0x000E0BFE
		public Vector3 positionScale
		{
			get
			{
				return this._positionScale;
			}
			set
			{
				this._positionScale = value;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002E5A RID: 11866 RVA: 0x000E2807 File Offset: 0x000E0C07
		// (set) Token: 0x06002E5B RID: 11867 RVA: 0x000E280F File Offset: 0x000E0C0F
		public Vector3 rotationScale
		{
			get
			{
				return this._rotationScale;
			}
			set
			{
				this._rotationScale = value;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002E5C RID: 11868 RVA: 0x000E2818 File Offset: 0x000E0C18
		// (set) Token: 0x06002E5D RID: 11869 RVA: 0x000E2820 File Offset: 0x000E0C20
		public int positionFractalLevel
		{
			get
			{
				return this._positionFractalLevel;
			}
			set
			{
				this._positionFractalLevel = value;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002E5E RID: 11870 RVA: 0x000E2829 File Offset: 0x000E0C29
		// (set) Token: 0x06002E5F RID: 11871 RVA: 0x000E2831 File Offset: 0x000E0C31
		public int rotationFractalLevel
		{
			get
			{
				return this._rotationFractalLevel;
			}
			set
			{
				this._rotationFractalLevel = value;
			}
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000E283C File Offset: 0x000E0C3C
		public void Rehash()
		{
			for (int i = 0; i < 6; i++)
			{
				this._time[i] = UnityEngine.Random.Range(-10000f, 0f);
			}
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000E2872 File Offset: 0x000E0C72
		private void Start()
		{
			this._time = new float[6];
			this.Rehash();
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000E2886 File Offset: 0x000E0C86
		private void OnEnable()
		{
			this._initialPosition = base.transform.localPosition;
			this._initialRotation = base.transform.localRotation;
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x000E28AC File Offset: 0x000E0CAC
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			if (this._enablePositionNoise)
			{
				for (int i = 0; i < 3; i++)
				{
					this._time[i] += this._positionFrequency * deltaTime;
				}
				Vector3 vector = new Vector3(Perlin.Fbm(this._time[0], this._positionFractalLevel), Perlin.Fbm(this._time[1], this._positionFractalLevel), Perlin.Fbm(this._time[2], this._positionFractalLevel));
				vector = Vector3.Scale(vector, this._positionScale);
				vector *= this._positionAmplitude * 1.33333337f;
				base.transform.localPosition = this._initialPosition + vector;
			}
			if (this._enableRotationNoise)
			{
				for (int j = 0; j < 3; j++)
				{
					this._time[j + 3] += this._rotationFrequency * deltaTime;
				}
				Vector3 vector2 = new Vector3(Perlin.Fbm(this._time[3], this._rotationFractalLevel), Perlin.Fbm(this._time[4], this._rotationFractalLevel), Perlin.Fbm(this._time[5], this._rotationFractalLevel));
				vector2 = Vector3.Scale(vector2, this._rotationScale);
				vector2 *= this._rotationAmplitude * 1.33333337f;
				base.transform.localRotation = Quaternion.Euler(vector2) * this._initialRotation;
			}
		}

		// Token: 0x0400187A RID: 6266
		[SerializeField]
		private bool _enablePositionNoise = true;

		// Token: 0x0400187B RID: 6267
		[SerializeField]
		private bool _enableRotationNoise = true;

		// Token: 0x0400187C RID: 6268
		[SerializeField]
		private float _positionFrequency = 0.2f;

		// Token: 0x0400187D RID: 6269
		[SerializeField]
		private float _rotationFrequency = 0.2f;

		// Token: 0x0400187E RID: 6270
		[SerializeField]
		private float _positionAmplitude = 0.5f;

		// Token: 0x0400187F RID: 6271
		[SerializeField]
		private float _rotationAmplitude = 10f;

		// Token: 0x04001880 RID: 6272
		[SerializeField]
		private Vector3 _positionScale = Vector3.one;

		// Token: 0x04001881 RID: 6273
		[SerializeField]
		private Vector3 _rotationScale = new Vector3(1f, 1f, 0f);

		// Token: 0x04001882 RID: 6274
		[SerializeField]
		[Range(0f, 8f)]
		private int _positionFractalLevel = 3;

		// Token: 0x04001883 RID: 6275
		[SerializeField]
		[Range(0f, 8f)]
		private int _rotationFractalLevel = 3;

		// Token: 0x04001884 RID: 6276
		private const float _fbmNorm = 1.33333337f;

		// Token: 0x04001885 RID: 6277
		private Vector3 _initialPosition;

		// Token: 0x04001886 RID: 6278
		private Quaternion _initialRotation;

		// Token: 0x04001887 RID: 6279
		private float[] _time;
	}
}
