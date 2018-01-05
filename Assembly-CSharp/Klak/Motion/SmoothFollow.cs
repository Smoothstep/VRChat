using System;
using Klak.Math;
using Klak.VectorMathExtension;
using UnityEngine;

namespace Klak.Motion
{
	// Token: 0x0200052C RID: 1324
	[AddComponentMenu("Klak/Motion/Smooth Follow")]
	public class SmoothFollow : MonoBehaviour
	{
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002E78 RID: 11896 RVA: 0x000E2CA6 File Offset: 0x000E10A6
		// (set) Token: 0x06002E79 RID: 11897 RVA: 0x000E2CAE File Offset: 0x000E10AE
		public SmoothFollow.Interpolator interpolationType
		{
			get
			{
				return this._interpolator;
			}
			set
			{
				this._interpolator = value;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002E7A RID: 11898 RVA: 0x000E2CB7 File Offset: 0x000E10B7
		// (set) Token: 0x06002E7B RID: 11899 RVA: 0x000E2CBF File Offset: 0x000E10BF
		public Transform target
		{
			get
			{
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002E7C RID: 11900 RVA: 0x000E2CC8 File Offset: 0x000E10C8
		// (set) Token: 0x06002E7D RID: 11901 RVA: 0x000E2CD0 File Offset: 0x000E10D0
		public float positionSpeed
		{
			get
			{
				return this._positionSpeed;
			}
			set
			{
				this._positionSpeed = value;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002E7E RID: 11902 RVA: 0x000E2CD9 File Offset: 0x000E10D9
		// (set) Token: 0x06002E7F RID: 11903 RVA: 0x000E2CE1 File Offset: 0x000E10E1
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

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002E80 RID: 11904 RVA: 0x000E2CEA File Offset: 0x000E10EA
		// (set) Token: 0x06002E81 RID: 11905 RVA: 0x000E2CF2 File Offset: 0x000E10F2
		public float jumpDistance
		{
			get
			{
				return this._jumpDistance;
			}
			set
			{
				this._jumpDistance = value;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002E82 RID: 11906 RVA: 0x000E2CFB File Offset: 0x000E10FB
		// (set) Token: 0x06002E83 RID: 11907 RVA: 0x000E2D03 File Offset: 0x000E1103
		public float jumpAngle
		{
			get
			{
				return this._jumpAngle;
			}
			set
			{
				this._jumpAngle = value;
			}
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000E2D0C File Offset: 0x000E110C
		public void Snap()
		{
			if (this._positionSpeed > 0f)
			{
				base.transform.position = this.target.position;
			}
			if (this._rotationSpeed > 0f)
			{
				base.transform.rotation = this.target.rotation;
			}
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000E2D68 File Offset: 0x000E1168
		public void JumpRandomly()
		{
			float d = UnityEngine.Random.Range(0.5f, 1f);
			float num = UnityEngine.Random.Range(0.5f, 1f);
			Vector3 a = UnityEngine.Random.onUnitSphere * this._jumpDistance * d;
			Quaternion lhs = Quaternion.AngleAxis(this._jumpAngle * num, UnityEngine.Random.onUnitSphere);
			base.transform.position = a + this.target.position;
			base.transform.rotation = lhs * this.target.rotation;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000E2DF8 File Offset: 0x000E11F8
		private Vector3 SpringPosition(Vector3 current, Vector3 target)
		{
			this._vposition = ETween.Step(this._vposition, Vector3.zero, 1f + this._positionSpeed * 0.5f);
			this._vposition += (target - current) * (this._positionSpeed * 0.1f);
			return current + this._vposition * Time.deltaTime;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000E2E6C File Offset: 0x000E126C
		private Quaternion SpringRotation(Quaternion current, Quaternion target)
		{
			Vector4 vector = current.ToVector4();
			Vector4 a = target.ToVector4();
			this._vrotation = ETween.Step(this._vrotation, Vector4.zero, 1f + this._rotationSpeed * 0.5f);
			this._vrotation += (a - vector) * (this._rotationSpeed * 0.1f);
			return (vector + this._vrotation * Time.deltaTime).ToNormalizedQuaternion();
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000E2F04 File Offset: 0x000E1304
		private void Update()
		{
			if (this._interpolator == SmoothFollow.Interpolator.Exponential)
			{
				if (this._positionSpeed > 0f)
				{
					base.transform.position = ETween.Step(base.transform.position, this.target.position, this._positionSpeed);
				}
				if (this._rotationSpeed > 0f)
				{
					base.transform.rotation = ETween.Step(base.transform.rotation, this.target.rotation, this._rotationSpeed);
				}
			}
			else if (this._interpolator == SmoothFollow.Interpolator.DampedSpring)
			{
				if (this._positionSpeed > 0f)
				{
					base.transform.position = DTween.Step(base.transform.position, this.target.position, ref this._vposition, this._positionSpeed);
				}
				if (this._rotationSpeed > 0f)
				{
					base.transform.rotation = DTween.Step(base.transform.rotation, this.target.rotation, ref this._vrotation, this._rotationSpeed);
				}
			}
			else
			{
				if (this._positionSpeed > 0f)
				{
					base.transform.position = this.SpringPosition(base.transform.position, this.target.position);
				}
				if (this._rotationSpeed > 0f)
				{
					base.transform.rotation = this.SpringRotation(base.transform.rotation, this.target.rotation);
				}
			}
		}

		// Token: 0x0400189F RID: 6303
		[SerializeField]
		private SmoothFollow.Interpolator _interpolator = SmoothFollow.Interpolator.DampedSpring;

		// Token: 0x040018A0 RID: 6304
		[SerializeField]
		private Transform _target;

		// Token: 0x040018A1 RID: 6305
		[SerializeField]
		[Range(0f, 20f)]
		private float _positionSpeed = 2f;

		// Token: 0x040018A2 RID: 6306
		[SerializeField]
		[Range(0f, 20f)]
		private float _rotationSpeed = 2f;

		// Token: 0x040018A3 RID: 6307
		[SerializeField]
		private float _jumpDistance = 1f;

		// Token: 0x040018A4 RID: 6308
		[SerializeField]
		[Range(0f, 360f)]
		private float _jumpAngle = 60f;

		// Token: 0x040018A5 RID: 6309
		private Vector3 _vposition;

		// Token: 0x040018A6 RID: 6310
		private Vector4 _vrotation;

		// Token: 0x0200052D RID: 1325
		public enum Interpolator
		{
			// Token: 0x040018A8 RID: 6312
			Exponential,
			// Token: 0x040018A9 RID: 6313
			Spring,
			// Token: 0x040018AA RID: 6314
			DampedSpring
		}
	}
}
