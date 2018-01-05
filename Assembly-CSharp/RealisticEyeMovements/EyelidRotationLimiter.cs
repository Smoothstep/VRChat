using System;
using UnityEngine;

namespace RealisticEyeMovements
{
	// Token: 0x020008A9 RID: 2217
	[Serializable]
	public class EyelidRotationLimiter
	{
		// Token: 0x060043F9 RID: 17401 RVA: 0x0016779D File Offset: 0x00165B9D
		public bool CanImport(EyelidRotationLimiter.EyelidRotationLimiterForExport import, Transform startXform)
		{
			return Utils.CanGetTransformFromPath(startXform, import.transformPath);
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x001677AC File Offset: 0x00165BAC
		public EyelidRotationLimiter.EyelidRotationLimiterForExport GetExport(Transform startXform)
		{
			return new EyelidRotationLimiter.EyelidRotationLimiterForExport
			{
				transformPath = Utils.GetPathForTransform(startXform, this.transform),
				defaultQ = this.defaultQ,
				closedQ = this.closedQ,
				lookUpQ = this.lookUpQ,
				lookDownQ = this.lookDownQ,
				eyeMaxDownAngle = this.eyeMaxDownAngle,
				eyeMaxUpAngle = this.eyeMaxUpAngle,
				defaultPos = this.defaultPos,
				closedPos = this.closedPos,
				lookUpPos = this.lookUpPos,
				lookDownPos = this.lookDownPos,
				isLookUpSet = this.isLookUpSet,
				isLookDownSet = this.isLookDownSet,
				isDefaultPosSet = this.isDefaultPosSet,
				isClosedPosSet = this.isClosedPosSet,
				isLookUpPosSet = this.isLookUpPosSet,
				isLookDownPosSet = this.isLookDownPosSet
			};
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x001678BC File Offset: 0x00165CBC
		public void GetRotationAndPosition(float eyeAngle, float blink01, float eyeWidenOrSquint, bool isUpper, out Quaternion rotation, ref Vector3 position, ControlData.EyelidBoneMode eyelidBoneMode)
		{
			bool flag = eyeAngle > 0f;
			float t = Mathf.InverseLerp(0f, (!flag) ? (-this.eyeMaxUpAngle) : this.eyeMaxDownAngle, eyeAngle);
			if (eyeWidenOrSquint < 0f)
			{
				blink01 = Mathf.Lerp(blink01, 1f, -eyeWidenOrSquint);
			}
			if (eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
			{
				rotation = Quaternion.Slerp(this.defaultQ, (!flag) ? this.lookUpQ : this.lookDownQ, t);
				rotation = Quaternion.Slerp(rotation, this.closedQ, blink01);
				if (eyeWidenOrSquint > 0f)
				{
					rotation = Quaternion.Slerp(rotation, (!isUpper) ? this.lookDownQ : this.lookUpQ, eyeWidenOrSquint);
				}
			}
			else
			{
				rotation = Quaternion.identity;
			}
			if (eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Position)
			{
				if (flag)
				{
					if (this.isDefaultPosSet && this.isLookDownPosSet)
					{
						position = Vector3.Lerp(this.defaultPos, this.lookDownPos, t);
					}
				}
				else if (this.isDefaultPosSet && this.isLookUpPosSet)
				{
					position = Vector3.Lerp(this.defaultPos, this.lookUpPos, t);
				}
				if (this.isDefaultPosSet && this.isClosedPosSet)
				{
					position = Vector3.Lerp(position, this.closedPos, blink01);
				}
				if (eyeWidenOrSquint > 0f)
				{
					position = Vector3.Lerp(position, (!isUpper) ? this.lookDownPos : this.lookUpPos, eyeWidenOrSquint);
				}
			}
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x00167A8C File Offset: 0x00165E8C
		public void Import(EyelidRotationLimiter.EyelidRotationLimiterForExport import, Transform startXform)
		{
			this.transform = Utils.GetTransformFromPath(startXform, import.transformPath);
			this.defaultQ = import.defaultQ;
			this.closedQ = import.closedQ;
			this.lookUpQ = import.lookUpQ;
			this.lookDownQ = import.lookDownQ;
			this.eyeMaxDownAngle = import.eyeMaxDownAngle;
			this.eyeMaxUpAngle = import.eyeMaxUpAngle;
			this.defaultPos = import.defaultPos;
			this.closedPos = import.closedPos;
			this.lookUpPos = import.lookUpPos;
			this.lookDownPos = import.lookDownPos;
			this.isLookUpSet = import.isLookUpSet;
			this.isLookDownPosSet = import.isLookDownPosSet;
			this.isDefaultPosSet = import.isDefaultPosSet;
			this.isClosedPosSet = import.isClosedPosSet;
			this.isLookUpPosSet = import.isLookUpPosSet;
			this.isLookDownPosSet = import.isLookDownPosSet;
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x00167B94 File Offset: 0x00165F94
		public void RestoreClosed(ControlData.EyelidBoneMode eyelidBoneMode)
		{
			if (eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
			{
				this.transform.localRotation = this.closedQ;
			}
			if ((eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Position) && this.isClosedPosSet)
			{
				this.transform.localPosition = this.closedPos;
			}
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x00167BE8 File Offset: 0x00165FE8
		public void RestoreDefault(ControlData.EyelidBoneMode eyelidBoneMode)
		{
			if (eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
			{
				this.transform.localRotation = this.defaultQ;
			}
			if ((eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Position) && this.isDefaultPosSet)
			{
				this.transform.localPosition = this.defaultPos;
			}
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x00167C3C File Offset: 0x0016603C
		public void RestoreLookDown(ControlData.EyelidBoneMode eyelidBoneMode)
		{
			if (eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
			{
				this.transform.localRotation = this.lookDownQ;
			}
			if ((eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Position) && this.isLookDownPosSet)
			{
				this.transform.localPosition = this.lookDownPos;
			}
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x00167C90 File Offset: 0x00166090
		public void RestoreLookUp(ControlData.EyelidBoneMode eyelidBoneMode)
		{
			if (eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
			{
				this.transform.localRotation = this.lookUpQ;
			}
			if ((eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || eyelidBoneMode == ControlData.EyelidBoneMode.Position) && this.isLookUpPosSet)
			{
				this.transform.localPosition = this.lookUpPos;
			}
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x00167CE4 File Offset: 0x001660E4
		public void SaveClosed()
		{
			this.closedQ = this.transform.localRotation;
			this.closedPos = this.transform.localPosition;
			this.isClosedPosSet = true;
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x00167D10 File Offset: 0x00166110
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
			this.defaultPos = t.localPosition;
			this.isDefaultPosSet = true;
			if (!this.isLookUpPosSet)
			{
				this.lookUpPos = this.defaultPos;
			}
			if (!this.isLookDownPosSet)
			{
				this.lookDownPos = this.defaultPos;
			}
			if (!this.isClosedPosSet)
			{
				this.closedPos = this.defaultPos;
			}
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x00167DE8 File Offset: 0x001661E8
		public void SaveLookDown(float eyeMaxDownAngle)
		{
			this.eyeMaxDownAngle = eyeMaxDownAngle;
			this.lookDownQ = this.transform.localRotation;
			this.lookDownPos = this.transform.localPosition;
			this.isLookDownSet = true;
			this.isLookDownPosSet = true;
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x00167E21 File Offset: 0x00166221
		public void SaveLookUp(float eyeMaxUpAngle)
		{
			this.eyeMaxUpAngle = eyeMaxUpAngle;
			this.lookUpQ = this.transform.localRotation;
			this.lookUpPos = this.transform.localPosition;
			this.isLookUpSet = true;
			this.isLookUpPosSet = true;
		}

		// Token: 0x04002CF1 RID: 11505
		[SerializeField]
		private Transform transform;

		// Token: 0x04002CF2 RID: 11506
		[SerializeField]
		private Quaternion defaultQ;

		// Token: 0x04002CF3 RID: 11507
		[SerializeField]
		private Quaternion closedQ;

		// Token: 0x04002CF4 RID: 11508
		[SerializeField]
		private Quaternion lookUpQ;

		// Token: 0x04002CF5 RID: 11509
		[SerializeField]
		private Quaternion lookDownQ;

		// Token: 0x04002CF6 RID: 11510
		[SerializeField]
		private float eyeMaxDownAngle;

		// Token: 0x04002CF7 RID: 11511
		[SerializeField]
		private float eyeMaxUpAngle;

		// Token: 0x04002CF8 RID: 11512
		[SerializeField]
		private Vector3 defaultPos;

		// Token: 0x04002CF9 RID: 11513
		[SerializeField]
		private Vector3 closedPos;

		// Token: 0x04002CFA RID: 11514
		[SerializeField]
		private Vector3 lookUpPos;

		// Token: 0x04002CFB RID: 11515
		[SerializeField]
		private Vector3 lookDownPos;

		// Token: 0x04002CFC RID: 11516
		[SerializeField]
		private bool isLookUpSet;

		// Token: 0x04002CFD RID: 11517
		[SerializeField]
		private bool isLookDownSet;

		// Token: 0x04002CFE RID: 11518
		[SerializeField]
		private bool isDefaultPosSet;

		// Token: 0x04002CFF RID: 11519
		[SerializeField]
		private bool isClosedPosSet;

		// Token: 0x04002D00 RID: 11520
		[SerializeField]
		private bool isLookUpPosSet;

		// Token: 0x04002D01 RID: 11521
		[SerializeField]
		private bool isLookDownPosSet;

		// Token: 0x020008AA RID: 2218
		[Serializable]
		public class EyelidRotationLimiterForExport
		{
			// Token: 0x04002D02 RID: 11522
			public string transformPath;

			// Token: 0x04002D03 RID: 11523
			public SerializableQuaternion defaultQ;

			// Token: 0x04002D04 RID: 11524
			public SerializableQuaternion closedQ;

			// Token: 0x04002D05 RID: 11525
			public SerializableQuaternion lookUpQ;

			// Token: 0x04002D06 RID: 11526
			public SerializableQuaternion lookDownQ;

			// Token: 0x04002D07 RID: 11527
			public float eyeMaxDownAngle;

			// Token: 0x04002D08 RID: 11528
			public float eyeMaxUpAngle;

			// Token: 0x04002D09 RID: 11529
			public SerializableVector3 defaultPos;

			// Token: 0x04002D0A RID: 11530
			public SerializableVector3 closedPos;

			// Token: 0x04002D0B RID: 11531
			public SerializableVector3 lookUpPos;

			// Token: 0x04002D0C RID: 11532
			public SerializableVector3 lookDownPos;

			// Token: 0x04002D0D RID: 11533
			public bool isLookUpSet;

			// Token: 0x04002D0E RID: 11534
			public bool isLookDownSet;

			// Token: 0x04002D0F RID: 11535
			public bool isDefaultPosSet;

			// Token: 0x04002D10 RID: 11536
			public bool isClosedPosSet;

			// Token: 0x04002D11 RID: 11537
			public bool isLookUpPosSet;

			// Token: 0x04002D12 RID: 11538
			public bool isLookDownPosSet;
		}
	}
}
