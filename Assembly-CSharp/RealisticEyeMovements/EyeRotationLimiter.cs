using System;
using UnityEngine;

namespace RealisticEyeMovements
{
	// Token: 0x020008A7 RID: 2215
	[Serializable]
	public class EyeRotationLimiter
	{
		// Token: 0x060043EA RID: 17386 RVA: 0x0016745C File Offset: 0x0016585C
		public bool CanImport(EyeRotationLimiter.EyeRotationLimiterForExport import, Transform startXform)
		{
			return Utils.CanGetTransformFromPath(startXform, import.transformPath);
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0016746A File Offset: 0x0016586A
		public float ClampAngle(float angle)
		{
			return Mathf.Clamp(Utils.NormalizedDegAngle(angle), -this.maxUpAngle, this.maxDownAngle);
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x00167484 File Offset: 0x00165884
		public EyeRotationLimiter.EyeRotationLimiterForExport GetExport(Transform startXform)
		{
			return new EyeRotationLimiter.EyeRotationLimiterForExport
			{
				transformPath = Utils.GetPathForTransform(startXform, this.transform),
				defaultQ = this.defaultQ,
				lookUpQ = this.lookUpQ,
				lookDownQ = this.lookDownQ,
				isLookUpSet = this.isLookUpSet,
				isLookDownSet = this.isLookDownSet
			};
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x001674F7 File Offset: 0x001658F7
		public float GetEyeUp01(float angle)
		{
			return (angle < 0f) ? Mathf.InverseLerp(0f, this.maxUpAngle, -angle) : 0f;
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x00167520 File Offset: 0x00165920
		public float GetEyeDown01(float angle)
		{
			return (angle > 0f) ? Mathf.InverseLerp(0f, this.maxDownAngle, angle) : 0f;
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x00167548 File Offset: 0x00165948
		public void Import(EyeRotationLimiter.EyeRotationLimiterForExport import, Transform startXform)
		{
			this.transform = Utils.GetTransformFromPath(startXform, import.transformPath);
			this.defaultQ = import.defaultQ;
			this.lookUpQ = import.lookUpQ;
			this.lookDownQ = import.lookDownQ;
			this.isLookUpSet = import.isLookUpSet;
			this.isLookDownSet = import.isLookDownSet;
			this.UpdateMaxAngles();
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x001675B8 File Offset: 0x001659B8
		public void RestoreDefault()
		{
			this.transform.localRotation = this.defaultQ;
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x001675CB File Offset: 0x001659CB
		public void RestoreLookDown()
		{
			this.transform.localRotation = this.lookDownQ;
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x001675DE File Offset: 0x001659DE
		public void RestoreLookUp()
		{
			this.transform.localRotation = this.lookUpQ;
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x001675F4 File Offset: 0x001659F4
		public void SaveDefault(Transform t)
		{
			this.transform = t;
			this.defaultQ = t.localRotation;
			if (!this.isLookDownSet)
			{
				this.lookDownQ = this.defaultQ * Quaternion.Euler(20f, 0f, 0f);
			}
			if (!this.isLookUpSet)
			{
				this.lookUpQ = this.defaultQ * Quaternion.Euler(-8f, 0f, 0f);
			}
			this.UpdateMaxAngles();
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0016767A File Offset: 0x00165A7A
		public void SaveLookDown()
		{
			this.lookDownQ = this.transform.localRotation;
			this.UpdateMaxAngles();
			this.isLookDownSet = true;
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0016769A File Offset: 0x00165A9A
		public void SaveLookUp()
		{
			this.lookUpQ = this.transform.localRotation;
			this.UpdateMaxAngles();
			this.isLookUpSet = true;
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x001676BC File Offset: 0x00165ABC
		private void UpdateMaxAngles()
		{
			Vector3 eulerAngles = (Quaternion.Inverse(this.defaultQ) * this.lookUpQ).eulerAngles;
			this.maxUpAngle = Mathf.Max(Mathf.Abs(Utils.NormalizedDegAngle(eulerAngles.x)), Mathf.Max(Mathf.Abs(Utils.NormalizedDegAngle(eulerAngles.y)), Mathf.Abs(Utils.NormalizedDegAngle(eulerAngles.z))));
			Vector3 eulerAngles2 = (Quaternion.Inverse(this.defaultQ) * this.lookDownQ).eulerAngles;
			this.maxDownAngle = Mathf.Max(Mathf.Abs(Utils.NormalizedDegAngle(eulerAngles2.x)), Mathf.Max(Mathf.Abs(Utils.NormalizedDegAngle(eulerAngles2.y)), Mathf.Abs(Utils.NormalizedDegAngle(eulerAngles2.z))));
		}

		// Token: 0x04002CE3 RID: 11491
		[SerializeField]
		private Transform transform;

		// Token: 0x04002CE4 RID: 11492
		[SerializeField]
		private Quaternion defaultQ;

		// Token: 0x04002CE5 RID: 11493
		[SerializeField]
		private Quaternion lookUpQ;

		// Token: 0x04002CE6 RID: 11494
		[SerializeField]
		private Quaternion lookDownQ;

		// Token: 0x04002CE7 RID: 11495
		public float maxUpAngle;

		// Token: 0x04002CE8 RID: 11496
		public float maxDownAngle;

		// Token: 0x04002CE9 RID: 11497
		[SerializeField]
		private bool isLookUpSet;

		// Token: 0x04002CEA RID: 11498
		[SerializeField]
		private bool isLookDownSet;

		// Token: 0x020008A8 RID: 2216
		[Serializable]
		public class EyeRotationLimiterForExport
		{
			// Token: 0x04002CEB RID: 11499
			public string transformPath;

			// Token: 0x04002CEC RID: 11500
			public SerializableQuaternion defaultQ;

			// Token: 0x04002CED RID: 11501
			public SerializableQuaternion lookUpQ;

			// Token: 0x04002CEE RID: 11502
			public SerializableQuaternion lookDownQ;

			// Token: 0x04002CEF RID: 11503
			public bool isLookUpSet;

			// Token: 0x04002CF0 RID: 11504
			public bool isLookDownSet;
		}
	}
}
