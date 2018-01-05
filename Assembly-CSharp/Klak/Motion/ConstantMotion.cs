using System;
using UnityEngine;

namespace Klak.Motion
{
	// Token: 0x02000529 RID: 1321
	[AddComponentMenu("Klak/Motion/Constant Motion")]
	public class ConstantMotion : MonoBehaviour
	{
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002E65 RID: 11877 RVA: 0x000E2A5E File Offset: 0x000E0E5E
		// (set) Token: 0x06002E66 RID: 11878 RVA: 0x000E2A66 File Offset: 0x000E0E66
		public ConstantMotion.TranslationMode translationMode
		{
			get
			{
				return this._translationMode;
			}
			set
			{
				this._translationMode = value;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002E67 RID: 11879 RVA: 0x000E2A6F File Offset: 0x000E0E6F
		// (set) Token: 0x06002E68 RID: 11880 RVA: 0x000E2A77 File Offset: 0x000E0E77
		public Vector3 translationVector
		{
			get
			{
				return this._translationVector;
			}
			set
			{
				this._translationVector = value;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002E69 RID: 11881 RVA: 0x000E2A80 File Offset: 0x000E0E80
		// (set) Token: 0x06002E6A RID: 11882 RVA: 0x000E2A88 File Offset: 0x000E0E88
		public float translationSpeed
		{
			get
			{
				return this._translationSpeed;
			}
			set
			{
				this._translationSpeed = value;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002E6B RID: 11883 RVA: 0x000E2A91 File Offset: 0x000E0E91
		// (set) Token: 0x06002E6C RID: 11884 RVA: 0x000E2A99 File Offset: 0x000E0E99
		public ConstantMotion.RotationMode rotationMode
		{
			get
			{
				return this._rotationMode;
			}
			set
			{
				this._rotationMode = value;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002E6D RID: 11885 RVA: 0x000E2AA2 File Offset: 0x000E0EA2
		// (set) Token: 0x06002E6E RID: 11886 RVA: 0x000E2AAA File Offset: 0x000E0EAA
		public Vector3 rotationAxis
		{
			get
			{
				return this._rotationAxis;
			}
			set
			{
				this._rotationAxis = value;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002E6F RID: 11887 RVA: 0x000E2AB3 File Offset: 0x000E0EB3
		// (set) Token: 0x06002E70 RID: 11888 RVA: 0x000E2ABB File Offset: 0x000E0EBB
		public float rotationSpeed
		{
			get
			{
				return this._rotationSpeed;
			}
			set
			{
				this._rotationSpeed = value;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002E71 RID: 11889 RVA: 0x000E2AC4 File Offset: 0x000E0EC4
		// (set) Token: 0x06002E72 RID: 11890 RVA: 0x000E2ACC File Offset: 0x000E0ECC
		public bool useLocalCoordinate
		{
			get
			{
				return this._useLocalCoordinate;
			}
			set
			{
				this._useLocalCoordinate = value;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002E73 RID: 11891 RVA: 0x000E2AD8 File Offset: 0x000E0ED8
		private Vector3 TranslationVector
		{
			get
			{
				switch (this._translationMode)
				{
				case ConstantMotion.TranslationMode.XAxis:
					return Vector3.right;
				case ConstantMotion.TranslationMode.YAxis:
					return Vector3.up;
				case ConstantMotion.TranslationMode.ZAxis:
					return Vector3.forward;
				case ConstantMotion.TranslationMode.Vector:
					return this._translationVector;
				default:
					return this._randomVectorT;
				}
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002E74 RID: 11892 RVA: 0x000E2B28 File Offset: 0x000E0F28
		private Vector3 RotationVector
		{
			get
			{
				switch (this._rotationMode)
				{
				case ConstantMotion.RotationMode.XAxis:
					return Vector3.right;
				case ConstantMotion.RotationMode.YAxis:
					return Vector3.up;
				case ConstantMotion.RotationMode.ZAxis:
					return Vector3.forward;
				case ConstantMotion.RotationMode.Vector:
					return this._rotationAxis;
				default:
					return this._randomVectorR;
				}
			}
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000E2B78 File Offset: 0x000E0F78
		private void Start()
		{
			this._randomVectorT = UnityEngine.Random.onUnitSphere;
			this._randomVectorR = UnityEngine.Random.onUnitSphere;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000E2B90 File Offset: 0x000E0F90
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			if (this._translationMode != ConstantMotion.TranslationMode.Off)
			{
				Vector3 b = this.TranslationVector * this._translationSpeed * deltaTime;
				if (this._useLocalCoordinate)
				{
					base.transform.localPosition += b;
				}
				else
				{
					base.transform.position += b;
				}
			}
			if (this._rotationMode != ConstantMotion.RotationMode.Off)
			{
				Quaternion lhs = Quaternion.AngleAxis(this._rotationSpeed * deltaTime, this.RotationVector);
				if (this._useLocalCoordinate)
				{
					base.transform.localRotation = lhs * base.transform.localRotation;
				}
				else
				{
					base.transform.rotation = lhs * base.transform.rotation;
				}
			}
		}

		// Token: 0x04001888 RID: 6280
		[SerializeField]
		private ConstantMotion.TranslationMode _translationMode;

		// Token: 0x04001889 RID: 6281
		[SerializeField]
		private Vector3 _translationVector = Vector3.forward;

		// Token: 0x0400188A RID: 6282
		[SerializeField]
		private float _translationSpeed = 1f;

		// Token: 0x0400188B RID: 6283
		[SerializeField]
		private ConstantMotion.RotationMode _rotationMode;

		// Token: 0x0400188C RID: 6284
		[SerializeField]
		private Vector3 _rotationAxis = Vector3.up;

		// Token: 0x0400188D RID: 6285
		[SerializeField]
		private float _rotationSpeed = 30f;

		// Token: 0x0400188E RID: 6286
		[SerializeField]
		private bool _useLocalCoordinate = true;

		// Token: 0x0400188F RID: 6287
		private Vector3 _randomVectorT;

		// Token: 0x04001890 RID: 6288
		private Vector3 _randomVectorR;

		// Token: 0x0200052A RID: 1322
		public enum TranslationMode
		{
			// Token: 0x04001892 RID: 6290
			Off,
			// Token: 0x04001893 RID: 6291
			XAxis,
			// Token: 0x04001894 RID: 6292
			YAxis,
			// Token: 0x04001895 RID: 6293
			ZAxis,
			// Token: 0x04001896 RID: 6294
			Vector,
			// Token: 0x04001897 RID: 6295
			Random
		}

		// Token: 0x0200052B RID: 1323
		public enum RotationMode
		{
			// Token: 0x04001899 RID: 6297
			Off,
			// Token: 0x0400189A RID: 6298
			XAxis,
			// Token: 0x0400189B RID: 6299
			YAxis,
			// Token: 0x0400189C RID: 6300
			ZAxis,
			// Token: 0x0400189D RID: 6301
			Vector,
			// Token: 0x0400189E RID: 6302
			Random
		}
	}
}
