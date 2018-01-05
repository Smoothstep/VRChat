using System;
using System.Collections.Generic;
using UnityEngine;

namespace RealisticEyeMovements
{
	// Token: 0x020008AB RID: 2219
	[Serializable]
	public class ControlData
	{
		// Token: 0x06004407 RID: 17415 RVA: 0x00167F2C File Offset: 0x0016632C
		public bool CanImport(ControlData.ControlDataForExport import, Transform startXform)
		{
			if (!Utils.CanGetTransformFromPath(startXform, import.leftEyePath) || !Utils.CanGetTransformFromPath(startXform, import.rightEyePath) || !Utils.CanGetTransformFromPath(startXform, import.upperEyeLidLeftPath) || !Utils.CanGetTransformFromPath(startXform, import.upperEyeLidRightPath) || !Utils.CanGetTransformFromPath(startXform, import.lowerEyeLidLeftPath) || !Utils.CanGetTransformFromPath(startXform, import.lowerEyeLidRightPath) || !this.leftBoneEyeRotationLimiter.CanImport(import.leftBoneEyeRotationLimiter, startXform) || !this.rightBoneEyeRotationLimiter.CanImport(import.rightBoneEyeRotationLimiter, startXform) || !this.leftEyeballEyeRotationLimiter.CanImport(import.leftEyeballEyeRotationLimiter, startXform) || !this.rightEyeballEyeRotationLimiter.CanImport(import.rightEyeballEyeRotationLimiter, startXform) || !this.upperLeftLimiter.CanImport(import.upperLeftLimiter, startXform) || !this.upperRightLimiter.CanImport(import.upperRightLimiter, startXform) || !this.lowerLeftLimiter.CanImport(import.lowerLeftLimiter, startXform) || !this.lowerRightLimiter.CanImport(import.lowerRightLimiter, startXform))
			{
				return false;
			}
			if (import.blendshapesForBlinking != null)
			{
				foreach (ControlData.EyelidPositionBlendshapeForExport import2 in import.blendshapesForBlinking)
				{
					if (!ControlData.EyelidPositionBlendshape.CanImport(import2, startXform))
					{
						return false;
					}
				}
			}
			if (import.blendshapesForLookingUp != null)
			{
				foreach (ControlData.EyelidPositionBlendshapeForExport import3 in import.blendshapesForLookingUp)
				{
					if (!ControlData.EyelidPositionBlendshape.CanImport(import3, startXform))
					{
						return false;
					}
				}
			}
			if (import.blendshapesForLookingDown != null)
			{
				foreach (ControlData.EyelidPositionBlendshapeForExport import4 in import.blendshapesForLookingDown)
				{
					if (!ControlData.EyelidPositionBlendshape.CanImport(import4, startXform))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x0016811C File Offset: 0x0016651C
		public bool CheckConsistency(Animator animator, EyeAndHeadAnimator eyeAndHeadAnimator)
		{
			if (this.eyeControl == ControlData.EyeControl.MecanimEyeBones)
			{
				if (null == animator)
				{
					Debug.LogError("REM: No Animator found.");
					return false;
				}
				if (null == animator.GetBoneTransform(HumanBodyBones.LeftEye) || null == animator.GetBoneTransform(HumanBodyBones.LeftEye))
				{
					Debug.LogError("REM: Mecanim humanoid eye bones not found.");
					return false;
				}
				if (!this.isEyeBoneDefaultSet)
				{
					this.SaveDefault(eyeAndHeadAnimator);
				}
			}
			else if (this.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				if (null == this.leftEye)
				{
					Debug.LogError("REM: The left eye object hasn't been assigned.");
					return false;
				}
				if (null == this.rightEye)
				{
					Debug.LogError("REM: The right eye object hasn't been assigned.");
					return false;
				}
				if (!this.isEyeBallDefaultSet)
				{
					this.SaveDefault(eyeAndHeadAnimator);
				}
			}
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				if (this.upperEyeLidLeft == null || this.upperEyeLidRight == null)
				{
					Debug.LogError("REM: The upper eyelid bones haven't been assigned.");
					return false;
				}
				if (!this.isEyelidBonesDefaultSet)
				{
					Debug.LogError("REM: The default eyelid position hasn't been saved.");
					return false;
				}
				if (!this.isEyelidBonesClosedSet)
				{
					Debug.LogError("REM: The eyes closed eyelid position hasn't been saved.");
					return false;
				}
				if (!this.isEyelidBonesLookUpSet)
				{
					Debug.LogError("REM: The eyes look up eyelid position hasn't been saved.");
					return false;
				}
				if (!this.isEyelidBonesLookDownSet)
				{
					Debug.LogError("REM: The eyes look down eyelid position hasn't been saved.");
					return false;
				}
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				if (!this.isEyelidBlendshapeDefaultSet)
				{
					Debug.LogError("REM: The default eyelid position hasn't been saved.");
					return false;
				}
				if (!this.isEyelidBlendshapeClosedSet)
				{
					Debug.LogError("REM: The eyes closed eyelid position hasn't been saved.");
					return false;
				}
				if (!this.isEyelidBlendshapeLookUpSet)
				{
					Debug.LogError("REM: The eyes look up eyelid position hasn't been saved.");
					return false;
				}
				if (!this.isEyelidBlendshapeLookDownSet)
				{
					Debug.LogError("REM: The eyes look down eyelid position hasn't been saved.");
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x001682F2 File Offset: 0x001666F2
		public float ClampLeftVertEyeAngle(float angle)
		{
			if (this.eyeControl == ControlData.EyeControl.MecanimEyeBones)
			{
				return this.leftBoneEyeRotationLimiter.ClampAngle(angle);
			}
			if (this.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				return this.leftEyeballEyeRotationLimiter.ClampAngle(angle);
			}
			return angle;
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x00168327 File Offset: 0x00166727
		public float ClampRightVertEyeAngle(float angle)
		{
			if (this.eyeControl == ControlData.EyeControl.MecanimEyeBones)
			{
				return this.rightBoneEyeRotationLimiter.ClampAngle(angle);
			}
			if (this.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				return this.rightEyeballEyeRotationLimiter.ClampAngle(angle);
			}
			return angle;
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x0016835C File Offset: 0x0016675C
		public ControlData.ControlDataForExport GetExport(Transform startXform)
		{
			ControlData.ControlDataForExport controlDataForExport = new ControlData.ControlDataForExport
			{
				eyeControl = this.eyeControl,
				eyelidBoneMode = this.eyelidBoneMode,
				leftEyePath = Utils.GetPathForTransform(startXform, this.leftEye),
				rightEyePath = Utils.GetPathForTransform(startXform, this.rightEye),
				maxEyeUpBoneAngle = this.maxEyeUpBoneAngle,
				maxEyeDownBoneAngle = this.maxEyeDownBoneAngle,
				maxEyeUpEyeballAngle = this.maxEyeUpEyeballAngle,
				maxEyeDownEyeballAngle = this.maxEyeDownEyeballAngle,
				isEyeBallDefaultSet = this.isEyeBallDefaultSet,
				isEyeBoneDefaultSet = this.isEyeBoneDefaultSet,
				isEyeBallLookUpSet = this.isEyeBallLookUpSet,
				isEyeBoneLookUpSet = this.isEyeBoneLookUpSet,
				isEyeBallLookDownSet = this.isEyeBallLookDownSet,
				isEyeBoneLookDownSet = this.isEyeBoneLookDownSet,
				leftBoneEyeRotationLimiter = this.leftBoneEyeRotationLimiter.GetExport(startXform),
				rightBoneEyeRotationLimiter = this.rightBoneEyeRotationLimiter.GetExport(startXform),
				leftEyeballEyeRotationLimiter = this.leftEyeballEyeRotationLimiter.GetExport(startXform),
				rightEyeballEyeRotationLimiter = this.rightEyeballEyeRotationLimiter.GetExport(startXform),
				eyelidControl = this.eyelidControl,
				eyelidsFollowEyesVertically = this.eyelidsFollowEyesVertically,
				upperEyeLidLeftPath = Utils.GetPathForTransform(startXform, this.upperEyeLidLeft),
				upperEyeLidRightPath = Utils.GetPathForTransform(startXform, this.upperEyeLidRight),
				lowerEyeLidLeftPath = Utils.GetPathForTransform(startXform, this.lowerEyeLidLeft),
				lowerEyeLidRightPath = Utils.GetPathForTransform(startXform, this.lowerEyeLidRight),
				isEyelidBonesDefaultSet = this.isEyelidBonesDefaultSet,
				isEyelidBonesClosedSet = this.isEyelidBonesClosedSet,
				isEyelidBonesLookUpSet = this.isEyelidBonesLookUpSet,
				isEyelidBonesLookDownSet = this.isEyelidBonesLookDownSet,
				upperLeftLimiter = this.upperLeftLimiter.GetExport(startXform),
				upperRightLimiter = this.upperRightLimiter.GetExport(startXform),
				lowerLeftLimiter = this.lowerLeftLimiter.GetExport(startXform),
				lowerRightLimiter = this.lowerRightLimiter.GetExport(startXform),
				eyeWidenOrSquint = this.eyeWidenOrSquint,
				isEyelidBlendshapeDefaultSet = this.isEyelidBlendshapeDefaultSet,
				isEyelidBlendshapeClosedSet = this.isEyelidBlendshapeClosedSet,
				isEyelidBlendshapeLookUpSet = this.isEyelidBlendshapeLookUpSet,
				isEyelidBlendshapeLookDownSet = this.isEyelidBlendshapeLookDownSet
			};
			controlDataForExport.blendshapesForBlinking = new ControlData.EyelidPositionBlendshapeForExport[this.blendshapesForBlinking.Length];
			for (int i = 0; i < this.blendshapesForBlinking.Length; i++)
			{
				controlDataForExport.blendshapesForBlinking[i] = this.blendshapesForBlinking[i].GetExport(startXform);
			}
			controlDataForExport.blendshapesForLookingUp = new ControlData.EyelidPositionBlendshapeForExport[this.blendshapesForLookingUp.Length];
			for (int j = 0; j < this.blendshapesForLookingUp.Length; j++)
			{
				controlDataForExport.blendshapesForLookingUp[j] = this.blendshapesForLookingUp[j].GetExport(startXform);
			}
			controlDataForExport.blendshapesForLookingDown = new ControlData.EyelidPositionBlendshapeForExport[this.blendshapesForLookingDown.Length];
			for (int k = 0; k < this.blendshapesForLookingDown.Length; k++)
			{
				controlDataForExport.blendshapesForLookingDown[k] = this.blendshapesForLookingDown[k].GetExport(startXform);
			}
			controlDataForExport.blendshapesConfigs = new ControlData.BlendshapesConfigForExport[this.blendshapesConfigs.Length];
			for (int l = 0; l < this.blendshapesConfigs.Length; l++)
			{
				controlDataForExport.blendshapesConfigs[l] = this.blendshapesConfigs[l].GetExport(startXform);
			}
			return controlDataForExport;
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x00168698 File Offset: 0x00166A98
		public void Import(ControlData.ControlDataForExport import, Transform startXform)
		{
			this.eyeControl = import.eyeControl;
			this.eyelidBoneMode = import.eyelidBoneMode;
			this.leftEye = Utils.GetTransformFromPath(startXform, import.leftEyePath);
			this.rightEye = Utils.GetTransformFromPath(startXform, import.rightEyePath);
			this.maxEyeUpBoneAngle = import.maxEyeUpBoneAngle;
			this.maxEyeDownBoneAngle = import.maxEyeDownBoneAngle;
			this.maxEyeUpEyeballAngle = import.maxEyeUpEyeballAngle;
			this.maxEyeDownEyeballAngle = import.maxEyeDownEyeballAngle;
			this.isEyeBallDefaultSet = import.isEyeBallDefaultSet;
			this.isEyeBoneDefaultSet = import.isEyeBoneDefaultSet;
			this.isEyeBallLookUpSet = import.isEyeBallLookUpSet;
			this.isEyeBoneLookUpSet = import.isEyeBoneLookUpSet;
			this.isEyeBallLookDownSet = import.isEyeBallLookDownSet;
			this.isEyeBoneLookDownSet = import.isEyeBoneLookDownSet;
			this.eyelidControl = import.eyelidControl;
			this.eyelidsFollowEyesVertically = import.eyelidsFollowEyesVertically;
			this.upperEyeLidLeft = Utils.GetTransformFromPath(startXform, import.upperEyeLidLeftPath);
			this.upperEyeLidRight = Utils.GetTransformFromPath(startXform, import.upperEyeLidRightPath);
			this.lowerEyeLidLeft = Utils.GetTransformFromPath(startXform, import.lowerEyeLidLeftPath);
			this.lowerEyeLidRight = Utils.GetTransformFromPath(startXform, import.lowerEyeLidRightPath);
			this.isEyelidBonesDefaultSet = import.isEyelidBonesDefaultSet;
			this.isEyelidBonesClosedSet = import.isEyelidBonesClosedSet;
			this.isEyelidBonesLookUpSet = import.isEyelidBonesLookUpSet;
			this.isEyelidBonesLookDownSet = import.isEyelidBonesLookDownSet;
			this.eyeWidenOrSquint = import.eyeWidenOrSquint;
			this.isEyelidBlendshapeDefaultSet = import.isEyelidBlendshapeDefaultSet;
			this.isEyelidBlendshapeClosedSet = import.isEyelidBlendshapeClosedSet;
			this.isEyelidBlendshapeLookUpSet = import.isEyelidBlendshapeLookUpSet;
			this.isEyelidBlendshapeLookDownSet = import.isEyelidBlendshapeLookDownSet;
			this.leftBoneEyeRotationLimiter.Import(import.leftBoneEyeRotationLimiter, startXform);
			this.rightBoneEyeRotationLimiter.Import(import.rightBoneEyeRotationLimiter, startXform);
			this.leftEyeballEyeRotationLimiter.Import(import.leftEyeballEyeRotationLimiter, startXform);
			this.rightEyeballEyeRotationLimiter.Import(import.rightEyeballEyeRotationLimiter, startXform);
			this.upperLeftLimiter.Import(import.upperLeftLimiter, startXform);
			this.upperRightLimiter.Import(import.upperRightLimiter, startXform);
			this.lowerLeftLimiter.Import(import.lowerLeftLimiter, startXform);
			this.lowerRightLimiter.Import(import.lowerRightLimiter, startXform);
			if (import.blendshapesForBlinking != null)
			{
				this.blendshapesForBlinking = new ControlData.EyelidPositionBlendshape[import.blendshapesForBlinking.Length];
				for (int i = 0; i < import.blendshapesForBlinking.Length; i++)
				{
					ControlData.EyelidPositionBlendshape eyelidPositionBlendshape = new ControlData.EyelidPositionBlendshape();
					eyelidPositionBlendshape.Import(import.blendshapesForBlinking[i], startXform);
					this.blendshapesForBlinking[i] = eyelidPositionBlendshape;
				}
			}
			if (import.blendshapesForLookingUp != null)
			{
				this.blendshapesForLookingUp = new ControlData.EyelidPositionBlendshape[import.blendshapesForLookingUp.Length];
				for (int j = 0; j < import.blendshapesForLookingUp.Length; j++)
				{
					ControlData.EyelidPositionBlendshape eyelidPositionBlendshape2 = new ControlData.EyelidPositionBlendshape();
					eyelidPositionBlendshape2.Import(import.blendshapesForLookingUp[j], startXform);
					this.blendshapesForLookingUp[j] = eyelidPositionBlendshape2;
				}
			}
			if (import.blendshapesForLookingDown != null)
			{
				this.blendshapesForLookingDown = new ControlData.EyelidPositionBlendshape[import.blendshapesForLookingDown.Length];
				for (int k = 0; k < import.blendshapesForLookingDown.Length; k++)
				{
					ControlData.EyelidPositionBlendshape eyelidPositionBlendshape3 = new ControlData.EyelidPositionBlendshape();
					eyelidPositionBlendshape3.Import(import.blendshapesForLookingDown[k], startXform);
					this.blendshapesForLookingDown[k] = eyelidPositionBlendshape3;
				}
			}
			bool flag = false;
			SkinnedMeshRenderer[] componentsInChildren = startXform.GetComponentsInChildren<SkinnedMeshRenderer>();
			if (import.blendshapesConfigs != null && componentsInChildren.Length == import.blendshapesConfigs.Length)
			{
				flag = true;
				foreach (ControlData.BlendshapesConfigForExport import2 in import.blendshapesConfigs)
				{
					if (!ControlData.BlendshapesConfig.CanImport(import2, startXform))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.blendshapesConfigs = new ControlData.BlendshapesConfig[import.blendshapesConfigs.Length];
				for (int m = 0; m < import.blendshapesConfigs.Length; m++)
				{
					ControlData.BlendshapesConfig blendshapesConfig = new ControlData.BlendshapesConfig();
					blendshapesConfig.Import(startXform, import.blendshapesConfigs[m]);
					this.blendshapesConfigs[m] = blendshapesConfig;
				}
			}
			else
			{
				this.blendshapesConfigs = null;
			}
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x00168A8C File Offset: 0x00166E8C
		public void Initialize()
		{
			if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape in this.blendshapesForBlinking)
				{
					eyelidPositionBlendshape.isUsedInEalierConfig = false;
					foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape2 in this.blendshapesForLookingUp)
					{
						if (eyelidPositionBlendshape.skinnedMeshRenderer == eyelidPositionBlendshape2.skinnedMeshRenderer && eyelidPositionBlendshape.index == eyelidPositionBlendshape2.index)
						{
							eyelidPositionBlendshape.isUsedInEalierConfig = true;
							break;
						}
					}
					if (!eyelidPositionBlendshape.isUsedInEalierConfig)
					{
						foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape3 in this.blendshapesForLookingDown)
						{
							if (eyelidPositionBlendshape.skinnedMeshRenderer == eyelidPositionBlendshape3.skinnedMeshRenderer && eyelidPositionBlendshape.index == eyelidPositionBlendshape3.index)
							{
								eyelidPositionBlendshape.isUsedInEalierConfig = true;
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x00168B90 File Offset: 0x00166F90
		private void LerpBlendshapeConfig(ControlData.EyelidPositionBlendshape[] blendshapes, float lerpValue, bool relativeToCurrentValueIfUsedInOtherConfig = false)
		{
			foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape in blendshapes)
			{
				eyelidPositionBlendshape.skinnedMeshRenderer.SetBlendShapeWeight(eyelidPositionBlendshape.index, Mathf.Lerp((!eyelidPositionBlendshape.isUsedInEalierConfig || !relativeToCurrentValueIfUsedInOtherConfig) ? eyelidPositionBlendshape.defaultWeight : eyelidPositionBlendshape.skinnedMeshRenderer.GetBlendShapeWeight(eyelidPositionBlendshape.index), eyelidPositionBlendshape.positionWeight, lerpValue));
			}
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x00168C01 File Offset: 0x00167001
		public bool NeedsSaveDefaultBlendshapeConfig()
		{
			return this.blendshapesConfigs == null || this.blendshapesConfigs.Length == 0;
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x00168C1C File Offset: 0x0016701C
		private void ResetBlendshapeConfig(ControlData.EyelidPositionBlendshape[] blendshapes)
		{
			if (blendshapes == null)
			{
				return;
			}
			foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape in blendshapes)
			{
				eyelidPositionBlendshape.skinnedMeshRenderer.SetBlendShapeWeight(eyelidPositionBlendshape.index, eyelidPositionBlendshape.defaultWeight);
			}
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x00168C61 File Offset: 0x00167061
		private void ResetAllBlendshapesToDefault()
		{
			this.ResetBlendshapeConfig(this.blendshapesForBlinking);
			this.ResetBlendshapeConfig(this.blendshapesForLookingDown);
			this.ResetBlendshapeConfig(this.blendshapesForLookingUp);
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x00168C88 File Offset: 0x00167088
		public void RestoreClosed()
		{
			if (this.eyeControl == ControlData.EyeControl.MecanimEyeBones)
			{
				this.leftBoneEyeRotationLimiter.RestoreDefault();
				this.rightBoneEyeRotationLimiter.RestoreDefault();
			}
			else if (this.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				this.leftEyeballEyeRotationLimiter.RestoreDefault();
				this.rightEyeballEyeRotationLimiter.RestoreDefault();
			}
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				this.upperLeftLimiter.RestoreClosed(this.eyelidBoneMode);
				this.upperRightLimiter.RestoreClosed(this.eyelidBoneMode);
				if (this.lowerEyeLidLeft != null)
				{
					this.lowerLeftLimiter.RestoreClosed(this.eyelidBoneMode);
				}
				if (this.lowerEyeLidRight != null)
				{
					this.lowerRightLimiter.RestoreClosed(this.eyelidBoneMode);
				}
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				this.ResetAllBlendshapesToDefault();
				if (this.blendshapesForBlinking != null)
				{
					foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape in this.blendshapesForBlinking)
					{
						eyelidPositionBlendshape.skinnedMeshRenderer.SetBlendShapeWeight(eyelidPositionBlendshape.index, eyelidPositionBlendshape.positionWeight);
					}
				}
			}
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x00168DA8 File Offset: 0x001671A8
		public void RestoreDefault()
		{
			if (this.eyeControl == ControlData.EyeControl.MecanimEyeBones)
			{
				this.leftBoneEyeRotationLimiter.RestoreDefault();
				this.rightBoneEyeRotationLimiter.RestoreDefault();
			}
			else if (this.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				this.leftEyeballEyeRotationLimiter.RestoreDefault();
				this.rightEyeballEyeRotationLimiter.RestoreDefault();
			}
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				this.upperLeftLimiter.RestoreDefault(this.eyelidBoneMode);
				this.upperRightLimiter.RestoreDefault(this.eyelidBoneMode);
				if (this.lowerEyeLidLeft != null)
				{
					this.lowerLeftLimiter.RestoreDefault(this.eyelidBoneMode);
				}
				if (this.lowerEyeLidRight != null)
				{
					this.lowerRightLimiter.RestoreDefault(this.eyelidBoneMode);
				}
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				this.ResetAllBlendshapesToDefault();
			}
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x00168E88 File Offset: 0x00167288
		public void RestoreLookDown()
		{
			if (this.eyeControl == ControlData.EyeControl.MecanimEyeBones)
			{
				this.leftBoneEyeRotationLimiter.RestoreLookDown();
				this.rightBoneEyeRotationLimiter.RestoreLookDown();
			}
			else if (this.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				this.leftEyeballEyeRotationLimiter.RestoreLookDown();
				this.rightEyeballEyeRotationLimiter.RestoreLookDown();
			}
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				this.upperLeftLimiter.RestoreLookDown(this.eyelidBoneMode);
				this.upperRightLimiter.RestoreLookDown(this.eyelidBoneMode);
				if (this.lowerEyeLidLeft != null)
				{
					this.lowerLeftLimiter.RestoreLookDown(this.eyelidBoneMode);
				}
				if (this.lowerEyeLidRight != null)
				{
					this.lowerRightLimiter.RestoreLookDown(this.eyelidBoneMode);
				}
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				this.ResetAllBlendshapesToDefault();
				foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape in this.blendshapesForLookingDown)
				{
					eyelidPositionBlendshape.skinnedMeshRenderer.SetBlendShapeWeight(eyelidPositionBlendshape.index, eyelidPositionBlendshape.positionWeight);
				}
			}
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x00168FA0 File Offset: 0x001673A0
		public void RestoreLookUp()
		{
			if (this.eyeControl == ControlData.EyeControl.MecanimEyeBones)
			{
				this.leftBoneEyeRotationLimiter.RestoreLookUp();
				this.rightBoneEyeRotationLimiter.RestoreLookUp();
			}
			else if (this.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				this.leftEyeballEyeRotationLimiter.RestoreLookUp();
				this.rightEyeballEyeRotationLimiter.RestoreLookUp();
			}
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				this.upperLeftLimiter.RestoreLookUp(this.eyelidBoneMode);
				this.upperRightLimiter.RestoreLookUp(this.eyelidBoneMode);
				if (this.lowerEyeLidLeft != null)
				{
					this.lowerLeftLimiter.RestoreLookUp(this.eyelidBoneMode);
				}
				if (this.lowerEyeLidRight != null)
				{
					this.lowerRightLimiter.RestoreLookUp(this.eyelidBoneMode);
				}
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				this.ResetAllBlendshapesToDefault();
				foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape in this.blendshapesForLookingUp)
				{
					eyelidPositionBlendshape.skinnedMeshRenderer.SetBlendShapeWeight(eyelidPositionBlendshape.index, eyelidPositionBlendshape.positionWeight);
				}
			}
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x001690B8 File Offset: 0x001674B8
		private void SaveBlendshapesForEyelidPosition(ref ControlData.EyelidPositionBlendshape[] blendshapesForPosition, UnityEngine.Object rootObject, string positionName)
		{
			SkinnedMeshRenderer[] componentsInChildren = (rootObject as MonoBehaviour).GetComponentsInChildren<SkinnedMeshRenderer>();
			List<ControlData.EyelidPositionBlendshape> list = new List<ControlData.EyelidPositionBlendshape>();
			if (componentsInChildren.Length != this.blendshapesConfigs.Length)
			{
				Debug.LogError("The saved data for open eyelids is invalid. Please reset to open eyelids and resave 'Eyes open, looking straight'.");
				this.isEyelidBlendshapeDefaultSet = false;
				this.isEyelidBlendshapeClosedSet = false;
				this.isEyelidBlendshapeLookDownSet = false;
				this.isEyelidBlendshapeLookUpSet = false;
			}
			else
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i];
					ControlData.BlendshapesConfig blendshapesConfig = this.blendshapesConfigs[i];
					if (skinnedMeshRenderer != blendshapesConfig.skinnedMeshRenderer || skinnedMeshRenderer.sharedMesh.blendShapeCount != blendshapesConfig.blendShapeCount)
					{
						Debug.LogError("The saved data for open eyelids is invalid. Please reset to open eyelids and resave 'Eyes open, looking straight'.");
						this.isEyelidBlendshapeDefaultSet = false;
						this.isEyelidBlendshapeClosedSet = false;
						this.isEyelidBlendshapeLookDownSet = false;
						this.isEyelidBlendshapeLookUpSet = false;
					}
					else
					{
						for (int j = 0; j < blendshapesConfig.blendShapeCount; j++)
						{
							if (Mathf.Abs(blendshapesConfig.blendshapeWeights[j] - skinnedMeshRenderer.GetBlendShapeWeight(j)) >= 0.01f)
							{
								ControlData.EyelidPositionBlendshape item = new ControlData.EyelidPositionBlendshape
								{
									skinnedMeshRenderer = skinnedMeshRenderer,
									index = j,
									defaultWeight = blendshapesConfig.blendshapeWeights[j],
									positionWeight = skinnedMeshRenderer.GetBlendShapeWeight(j)
								};
								list.Add(item);
							}
						}
					}
				}
				blendshapesForPosition = list.ToArray();
				Debug.Log(string.Concat(new object[]
				{
					"Found ",
					blendshapesForPosition.Length,
					" blend shapes for ",
					positionName,
					":"
				}));
				foreach (ControlData.EyelidPositionBlendshape eyelidPositionBlendshape in blendshapesForPosition)
				{
					Debug.Log(string.Concat(new object[]
					{
						eyelidPositionBlendshape.skinnedMeshRenderer.name,
						" --> ",
						eyelidPositionBlendshape.skinnedMeshRenderer.sharedMesh.GetBlendShapeName(eyelidPositionBlendshape.index),
						" open: ",
						eyelidPositionBlendshape.defaultWeight,
						" closed: ",
						eyelidPositionBlendshape.positionWeight
					}));
				}
			}
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x001692E0 File Offset: 0x001676E0
		public void SaveClosed(UnityEngine.Object rootObject)
		{
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				this.upperLeftLimiter.SaveClosed();
				this.upperRightLimiter.SaveClosed();
				if (this.lowerEyeLidLeft != null)
				{
					this.lowerLeftLimiter.SaveClosed();
				}
				if (this.lowerEyeLidRight != null)
				{
					this.lowerRightLimiter.SaveClosed();
				}
				this.isEyelidBonesClosedSet = true;
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				this.isEyelidBlendshapeClosedSet = true;
				this.SaveBlendshapesForEyelidPosition(ref this.blendshapesForBlinking, rootObject, "closed eyes");
			}
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x00169378 File Offset: 0x00167778
		public void SaveDefault(UnityEngine.Object rootObject)
		{
			if (this.eyeControl == ControlData.EyeControl.MecanimEyeBones)
			{
				Animator component = (rootObject as MonoBehaviour).GetComponent<Animator>();
				Transform boneTransform = component.GetBoneTransform(HumanBodyBones.LeftEye);
				Transform boneTransform2 = component.GetBoneTransform(HumanBodyBones.RightEye);
				this.leftBoneEyeRotationLimiter.SaveDefault(boneTransform);
				this.rightBoneEyeRotationLimiter.SaveDefault(boneTransform2);
				this.isEyeBoneDefaultSet = true;
			}
			else if (this.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				this.leftEyeballEyeRotationLimiter.SaveDefault(this.leftEye);
				this.rightEyeballEyeRotationLimiter.SaveDefault(this.rightEye);
				this.isEyeBallDefaultSet = true;
			}
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				this.upperLeftLimiter.SaveDefault(this.upperEyeLidLeft);
				this.upperRightLimiter.SaveDefault(this.upperEyeLidRight);
				if (this.lowerEyeLidLeft != null)
				{
					this.lowerLeftLimiter.SaveDefault(this.lowerEyeLidLeft);
				}
				if (this.lowerEyeLidRight != null)
				{
					this.lowerRightLimiter.SaveDefault(this.lowerEyeLidRight);
				}
				this.isEyelidBonesDefaultSet = true;
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				SkinnedMeshRenderer[] componentsInChildren = (rootObject as MonoBehaviour).GetComponentsInChildren<SkinnedMeshRenderer>();
				this.blendshapesConfigs = new ControlData.BlendshapesConfig[componentsInChildren.Length];
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					ControlData.BlendshapesConfig blendshapesConfig = new ControlData.BlendshapesConfig
					{
						skinnedMeshRenderer = componentsInChildren[i]
					};
					blendshapesConfig.blendShapeCount = blendshapesConfig.skinnedMeshRenderer.sharedMesh.blendShapeCount;
					blendshapesConfig.blendshapeWeights = new float[blendshapesConfig.blendShapeCount];
					for (int j = 0; j < blendshapesConfig.blendShapeCount; j++)
					{
						blendshapesConfig.blendshapeWeights[j] = blendshapesConfig.skinnedMeshRenderer.GetBlendShapeWeight(j);
					}
					this.blendshapesConfigs[i] = blendshapesConfig;
				}
				this.isEyelidBlendshapeDefaultSet = true;
			}
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x0016954C File Offset: 0x0016794C
		public void SaveLookDown(UnityEngine.Object rootObject)
		{
			bool flag = this.eyeControl == ControlData.EyeControl.MecanimEyeBones;
			bool flag2 = this.eyeControl == ControlData.EyeControl.SelectedObjects;
			if (flag)
			{
				this.leftBoneEyeRotationLimiter.SaveLookDown();
				this.rightBoneEyeRotationLimiter.SaveLookDown();
				this.isEyeBoneLookDownSet = true;
			}
			else if (flag2)
			{
				this.leftEyeballEyeRotationLimiter.SaveLookDown();
				this.rightEyeballEyeRotationLimiter.SaveLookDown();
				this.isEyeBallLookDownSet = true;
			}
			float eyeMaxDownAngle = (!flag) ? this.leftEyeballEyeRotationLimiter.maxDownAngle : this.leftBoneEyeRotationLimiter.maxDownAngle;
			float eyeMaxDownAngle2 = (!flag) ? this.rightEyeballEyeRotationLimiter.maxDownAngle : this.rightBoneEyeRotationLimiter.maxDownAngle;
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				this.upperLeftLimiter.SaveLookDown(eyeMaxDownAngle);
				this.upperRightLimiter.SaveLookDown(eyeMaxDownAngle2);
				if (this.lowerEyeLidLeft != null)
				{
					this.lowerLeftLimiter.SaveLookDown(eyeMaxDownAngle);
				}
				if (this.lowerEyeLidRight != null)
				{
					this.lowerRightLimiter.SaveLookDown(eyeMaxDownAngle2);
				}
				this.isEyelidBonesLookDownSet = true;
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				this.isEyelidBlendshapeLookDownSet = true;
				this.SaveBlendshapesForEyelidPosition(ref this.blendshapesForLookingDown, rootObject, "looking down");
			}
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x0016968C File Offset: 0x00167A8C
		public void SaveLookUp(UnityEngine.Object rootObject)
		{
			bool flag = this.eyeControl == ControlData.EyeControl.MecanimEyeBones;
			bool flag2 = this.eyeControl == ControlData.EyeControl.SelectedObjects;
			if (flag)
			{
				this.leftBoneEyeRotationLimiter.SaveLookUp();
				this.rightBoneEyeRotationLimiter.SaveLookUp();
				this.isEyeBoneLookUpSet = true;
			}
			else if (flag2)
			{
				this.leftEyeballEyeRotationLimiter.SaveLookUp();
				this.rightEyeballEyeRotationLimiter.SaveLookUp();
				this.isEyeBallLookUpSet = true;
			}
			float eyeMaxUpAngle = (!flag) ? this.leftEyeballEyeRotationLimiter.maxUpAngle : this.leftBoneEyeRotationLimiter.maxUpAngle;
			float eyeMaxUpAngle2 = (!flag) ? this.rightEyeballEyeRotationLimiter.maxUpAngle : this.rightBoneEyeRotationLimiter.maxUpAngle;
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				this.upperLeftLimiter.SaveLookUp(eyeMaxUpAngle);
				this.upperRightLimiter.SaveLookUp(eyeMaxUpAngle2);
				if (this.lowerEyeLidLeft != null)
				{
					this.lowerLeftLimiter.SaveLookUp(eyeMaxUpAngle);
				}
				if (this.lowerEyeLidRight != null)
				{
					this.lowerRightLimiter.SaveLookUp(eyeMaxUpAngle2);
				}
				this.isEyelidBonesLookUpSet = true;
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				this.isEyelidBlendshapeLookUpSet = true;
				this.SaveBlendshapesForEyelidPosition(ref this.blendshapesForLookingUp, rootObject, "looking up");
			}
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x001697CC File Offset: 0x00167BCC
		public void UpdateEyelids(float leftEyeAngle, float rightEyeAngle, float blink01, bool eyelidsFollowEyesVertically)
		{
			leftEyeAngle = Utils.NormalizedDegAngle(leftEyeAngle);
			rightEyeAngle = Utils.NormalizedDegAngle(rightEyeAngle);
			if (this.eyelidControl == ControlData.EyelidControl.Bones)
			{
				Vector3 localPosition = this.upperEyeLidLeft.localPosition;
				Quaternion localRotation;
				this.upperLeftLimiter.GetRotationAndPosition(leftEyeAngle, blink01, this.eyeWidenOrSquint, true, out localRotation, ref localPosition, this.eyelidBoneMode);
				if (this.eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || this.eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
				{
					this.upperEyeLidLeft.localRotation = localRotation;
				}
				if (this.eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || this.eyelidBoneMode == ControlData.EyelidBoneMode.Position)
				{
					this.upperEyeLidLeft.localPosition = localPosition;
				}
				localPosition = this.upperEyeLidRight.localPosition;
				this.upperRightLimiter.GetRotationAndPosition(rightEyeAngle, blink01, this.eyeWidenOrSquint, true, out localRotation, ref localPosition, this.eyelidBoneMode);
				if (this.eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || this.eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
				{
					this.upperEyeLidRight.localRotation = localRotation;
				}
				if (this.eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || this.eyelidBoneMode == ControlData.EyelidBoneMode.Position)
				{
					this.upperEyeLidRight.localPosition = localPosition;
				}
				if (this.lowerEyeLidLeft != null)
				{
					localPosition = this.lowerEyeLidLeft.localPosition;
					this.lowerLeftLimiter.GetRotationAndPosition(leftEyeAngle, blink01, this.eyeWidenOrSquint, false, out localRotation, ref localPosition, this.eyelidBoneMode);
					if (this.eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || this.eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
					{
						this.lowerEyeLidLeft.localRotation = localRotation;
					}
					if (this.eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || this.eyelidBoneMode == ControlData.EyelidBoneMode.Position)
					{
						this.lowerEyeLidLeft.localPosition = localPosition;
					}
				}
				if (this.lowerEyeLidRight != null)
				{
					localPosition = this.lowerEyeLidRight.localPosition;
					this.lowerRightLimiter.GetRotationAndPosition(rightEyeAngle, blink01, this.eyeWidenOrSquint, false, out localRotation, ref localPosition, this.eyelidBoneMode);
					if (this.eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || this.eyelidBoneMode == ControlData.EyelidBoneMode.Rotation)
					{
						this.lowerEyeLidRight.localRotation = localRotation;
					}
					if (this.eyelidBoneMode == ControlData.EyelidBoneMode.RotationAndPosition || this.eyelidBoneMode == ControlData.EyelidBoneMode.Position)
					{
						this.lowerEyeLidRight.localPosition = localPosition;
					}
				}
			}
			else if (this.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				bool flag = leftEyeAngle > 0f;
				float lerpValue = (!flag) ? ((this.eyeControl != ControlData.EyeControl.MecanimEyeBones) ? this.leftEyeballEyeRotationLimiter.GetEyeUp01(leftEyeAngle) : this.leftBoneEyeRotationLimiter.GetEyeUp01(leftEyeAngle)) : 0f;
				float lerpValue2 = flag ? ((this.eyeControl != ControlData.EyeControl.MecanimEyeBones) ? this.leftEyeballEyeRotationLimiter.GetEyeDown01(leftEyeAngle) : this.leftBoneEyeRotationLimiter.GetEyeDown01(leftEyeAngle)) : 0f;
				if (this.eyelidsFollowEyesVertically)
				{
					this.ResetAllBlendshapesToDefault();
				}
				else
				{
					this.ResetBlendshapeConfig(this.blendshapesForBlinking);
				}
				if (eyelidsFollowEyesVertically)
				{
					if (flag)
					{
						this.LerpBlendshapeConfig(this.blendshapesForLookingDown, lerpValue2, false);
					}
					else
					{
						this.LerpBlendshapeConfig(this.blendshapesForLookingUp, lerpValue, false);
					}
				}
				this.LerpBlendshapeConfig(this.blendshapesForBlinking, blink01, eyelidsFollowEyesVertically);
			}
			this.eyelidsFollowEyesVertically = eyelidsFollowEyesVertically;
		}

		// Token: 0x04002D13 RID: 11539
		public ControlData.EyeControl eyeControl;

		// Token: 0x04002D14 RID: 11540
		public Transform leftEye;

		// Token: 0x04002D15 RID: 11541
		public Transform rightEye;

		// Token: 0x04002D16 RID: 11542
		public float maxEyeUpBoneAngle = 20f;

		// Token: 0x04002D17 RID: 11543
		public float maxEyeDownBoneAngle = 20f;

		// Token: 0x04002D18 RID: 11544
		public float maxEyeUpEyeballAngle = 20f;

		// Token: 0x04002D19 RID: 11545
		public float maxEyeDownEyeballAngle = 20f;

		// Token: 0x04002D1A RID: 11546
		public bool isEyeBallDefaultSet;

		// Token: 0x04002D1B RID: 11547
		public bool isEyeBoneDefaultSet;

		// Token: 0x04002D1C RID: 11548
		public bool isEyeBallLookUpSet;

		// Token: 0x04002D1D RID: 11549
		public bool isEyeBoneLookUpSet;

		// Token: 0x04002D1E RID: 11550
		public bool isEyeBallLookDownSet;

		// Token: 0x04002D1F RID: 11551
		public bool isEyeBoneLookDownSet;

		// Token: 0x04002D20 RID: 11552
		[SerializeField]
		private EyeRotationLimiter leftBoneEyeRotationLimiter = new EyeRotationLimiter();

		// Token: 0x04002D21 RID: 11553
		[SerializeField]
		private EyeRotationLimiter rightBoneEyeRotationLimiter = new EyeRotationLimiter();

		// Token: 0x04002D22 RID: 11554
		[SerializeField]
		private EyeRotationLimiter leftEyeballEyeRotationLimiter = new EyeRotationLimiter();

		// Token: 0x04002D23 RID: 11555
		[SerializeField]
		private EyeRotationLimiter rightEyeballEyeRotationLimiter = new EyeRotationLimiter();

		// Token: 0x04002D24 RID: 11556
		public ControlData.EyelidControl eyelidControl;

		// Token: 0x04002D25 RID: 11557
		public ControlData.EyelidBoneMode eyelidBoneMode;

		// Token: 0x04002D26 RID: 11558
		public bool eyelidsFollowEyesVertically;

		// Token: 0x04002D27 RID: 11559
		public Transform upperEyeLidLeft;

		// Token: 0x04002D28 RID: 11560
		public Transform upperEyeLidRight;

		// Token: 0x04002D29 RID: 11561
		public Transform lowerEyeLidLeft;

		// Token: 0x04002D2A RID: 11562
		public Transform lowerEyeLidRight;

		// Token: 0x04002D2B RID: 11563
		public bool isEyelidBonesDefaultSet;

		// Token: 0x04002D2C RID: 11564
		public bool isEyelidBonesClosedSet;

		// Token: 0x04002D2D RID: 11565
		public bool isEyelidBonesLookUpSet;

		// Token: 0x04002D2E RID: 11566
		public bool isEyelidBonesLookDownSet;

		// Token: 0x04002D2F RID: 11567
		[SerializeField]
		private EyelidRotationLimiter upperLeftLimiter = new EyelidRotationLimiter();

		// Token: 0x04002D30 RID: 11568
		[SerializeField]
		private EyelidRotationLimiter upperRightLimiter = new EyelidRotationLimiter();

		// Token: 0x04002D31 RID: 11569
		[SerializeField]
		private EyelidRotationLimiter lowerLeftLimiter = new EyelidRotationLimiter();

		// Token: 0x04002D32 RID: 11570
		[SerializeField]
		private EyelidRotationLimiter lowerRightLimiter = new EyelidRotationLimiter();

		// Token: 0x04002D33 RID: 11571
		[Tooltip("0: normal. 1: max widened, -1: max squint")]
		[Range(-1f, 1f)]
		public float eyeWidenOrSquint;

		// Token: 0x04002D34 RID: 11572
		[SerializeField]
		private ControlData.EyelidPositionBlendshape[] blendshapesForBlinking = new ControlData.EyelidPositionBlendshape[0];

		// Token: 0x04002D35 RID: 11573
		[SerializeField]
		private ControlData.EyelidPositionBlendshape[] blendshapesForLookingUp = new ControlData.EyelidPositionBlendshape[0];

		// Token: 0x04002D36 RID: 11574
		[SerializeField]
		private ControlData.EyelidPositionBlendshape[] blendshapesForLookingDown = new ControlData.EyelidPositionBlendshape[0];

		// Token: 0x04002D37 RID: 11575
		[SerializeField]
		private ControlData.BlendshapesConfig[] blendshapesConfigs = new ControlData.BlendshapesConfig[0];

		// Token: 0x04002D38 RID: 11576
		public bool isEyelidBlendshapeDefaultSet;

		// Token: 0x04002D39 RID: 11577
		public bool isEyelidBlendshapeClosedSet;

		// Token: 0x04002D3A RID: 11578
		public bool isEyelidBlendshapeLookUpSet;

		// Token: 0x04002D3B RID: 11579
		public bool isEyelidBlendshapeLookDownSet;

		// Token: 0x020008AC RID: 2220
		[Serializable]
		public class ControlDataForExport
		{
			// Token: 0x04002D3C RID: 11580
			public ControlData.EyeControl eyeControl;

			// Token: 0x04002D3D RID: 11581
			public ControlData.EyelidBoneMode eyelidBoneMode;

			// Token: 0x04002D3E RID: 11582
			public string leftEyePath;

			// Token: 0x04002D3F RID: 11583
			public string rightEyePath;

			// Token: 0x04002D40 RID: 11584
			public float maxEyeUpBoneAngle;

			// Token: 0x04002D41 RID: 11585
			public float maxEyeDownBoneAngle;

			// Token: 0x04002D42 RID: 11586
			public float maxEyeUpEyeballAngle;

			// Token: 0x04002D43 RID: 11587
			public float maxEyeDownEyeballAngle;

			// Token: 0x04002D44 RID: 11588
			public bool isEyeBallDefaultSet;

			// Token: 0x04002D45 RID: 11589
			public bool isEyeBoneDefaultSet;

			// Token: 0x04002D46 RID: 11590
			public bool isEyeBallLookUpSet;

			// Token: 0x04002D47 RID: 11591
			public bool isEyeBoneLookUpSet;

			// Token: 0x04002D48 RID: 11592
			public bool isEyeBallLookDownSet;

			// Token: 0x04002D49 RID: 11593
			public bool isEyeBoneLookDownSet;

			// Token: 0x04002D4A RID: 11594
			public EyeRotationLimiter.EyeRotationLimiterForExport leftBoneEyeRotationLimiter;

			// Token: 0x04002D4B RID: 11595
			public EyeRotationLimiter.EyeRotationLimiterForExport rightBoneEyeRotationLimiter;

			// Token: 0x04002D4C RID: 11596
			public EyeRotationLimiter.EyeRotationLimiterForExport leftEyeballEyeRotationLimiter;

			// Token: 0x04002D4D RID: 11597
			public EyeRotationLimiter.EyeRotationLimiterForExport rightEyeballEyeRotationLimiter;

			// Token: 0x04002D4E RID: 11598
			public ControlData.EyelidControl eyelidControl;

			// Token: 0x04002D4F RID: 11599
			public bool eyelidsFollowEyesVertically;

			// Token: 0x04002D50 RID: 11600
			public string upperEyeLidLeftPath;

			// Token: 0x04002D51 RID: 11601
			public string upperEyeLidRightPath;

			// Token: 0x04002D52 RID: 11602
			public string lowerEyeLidLeftPath;

			// Token: 0x04002D53 RID: 11603
			public string lowerEyeLidRightPath;

			// Token: 0x04002D54 RID: 11604
			public bool isEyelidBonesDefaultSet;

			// Token: 0x04002D55 RID: 11605
			public bool isEyelidBonesClosedSet;

			// Token: 0x04002D56 RID: 11606
			public bool isEyelidBonesLookUpSet;

			// Token: 0x04002D57 RID: 11607
			public bool isEyelidBonesLookDownSet;

			// Token: 0x04002D58 RID: 11608
			public EyelidRotationLimiter.EyelidRotationLimiterForExport upperLeftLimiter;

			// Token: 0x04002D59 RID: 11609
			public EyelidRotationLimiter.EyelidRotationLimiterForExport upperRightLimiter;

			// Token: 0x04002D5A RID: 11610
			public EyelidRotationLimiter.EyelidRotationLimiterForExport lowerLeftLimiter;

			// Token: 0x04002D5B RID: 11611
			public EyelidRotationLimiter.EyelidRotationLimiterForExport lowerRightLimiter;

			// Token: 0x04002D5C RID: 11612
			public float eyeWidenOrSquint;

			// Token: 0x04002D5D RID: 11613
			public ControlData.EyelidPositionBlendshapeForExport[] blendshapesForBlinking;

			// Token: 0x04002D5E RID: 11614
			public ControlData.EyelidPositionBlendshapeForExport[] blendshapesForLookingUp;

			// Token: 0x04002D5F RID: 11615
			public ControlData.EyelidPositionBlendshapeForExport[] blendshapesForLookingDown;

			// Token: 0x04002D60 RID: 11616
			public ControlData.BlendshapesConfigForExport[] blendshapesConfigs;

			// Token: 0x04002D61 RID: 11617
			public bool isEyelidBlendshapeDefaultSet;

			// Token: 0x04002D62 RID: 11618
			public bool isEyelidBlendshapeClosedSet;

			// Token: 0x04002D63 RID: 11619
			public bool isEyelidBlendshapeLookUpSet;

			// Token: 0x04002D64 RID: 11620
			public bool isEyelidBlendshapeLookDownSet;
		}

		// Token: 0x020008AD RID: 2221
		public enum EyeControl
		{
			// Token: 0x04002D66 RID: 11622
			None,
			// Token: 0x04002D67 RID: 11623
			MecanimEyeBones,
			// Token: 0x04002D68 RID: 11624
			SelectedObjects
		}

		// Token: 0x020008AE RID: 2222
		public enum EyelidControl
		{
			// Token: 0x04002D6A RID: 11626
			None,
			// Token: 0x04002D6B RID: 11627
			Bones,
			// Token: 0x04002D6C RID: 11628
			Blendshapes
		}

		// Token: 0x020008AF RID: 2223
		public enum EyelidBoneMode
		{
			// Token: 0x04002D6E RID: 11630
			RotationAndPosition,
			// Token: 0x04002D6F RID: 11631
			Rotation,
			// Token: 0x04002D70 RID: 11632
			Position
		}

		// Token: 0x020008B0 RID: 2224
		[Serializable]
		public class EyelidPositionBlendshapeForExport
		{
			// Token: 0x04002D71 RID: 11633
			public string skinnedMeshRendererPath;

			// Token: 0x04002D72 RID: 11634
			public float defaultWeight;

			// Token: 0x04002D73 RID: 11635
			public float positionWeight;

			// Token: 0x04002D74 RID: 11636
			public int index;

			// Token: 0x04002D75 RID: 11637
			public string name;

			// Token: 0x04002D76 RID: 11638
			public bool isUsedInEalierConfig;
		}

		// Token: 0x020008B1 RID: 2225
		[Serializable]
		public class EyelidPositionBlendshape
		{
			// Token: 0x0600441F RID: 17439 RVA: 0x00169AE8 File Offset: 0x00167EE8
			public static bool CanImport(ControlData.EyelidPositionBlendshapeForExport import, Transform startXform)
			{
				Transform transformFromPath = Utils.GetTransformFromPath(startXform, import.skinnedMeshRendererPath);
				if (transformFromPath == null)
				{
					return false;
				}
				SkinnedMeshRenderer component = transformFromPath.GetComponent<SkinnedMeshRenderer>();
				if (component == null)
				{
					return false;
				}
				if (import.index >= component.sharedMesh.blendShapeCount)
				{
					return false;
				}
				if (!string.IsNullOrEmpty(import.name))
				{
					bool flag = false;
					for (int i = 0; i < component.sharedMesh.blendShapeCount; i++)
					{
						if (component.sharedMesh.GetBlendShapeName(i).Equals(import.name))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06004420 RID: 17440 RVA: 0x00169B98 File Offset: 0x00167F98
			public ControlData.EyelidPositionBlendshapeForExport GetExport(Transform startXform)
			{
				return new ControlData.EyelidPositionBlendshapeForExport
				{
					skinnedMeshRendererPath = Utils.GetPathForTransform(startXform, this.skinnedMeshRenderer.transform),
					defaultWeight = this.defaultWeight,
					positionWeight = this.positionWeight,
					index = this.index,
					name = this.name,
					isUsedInEalierConfig = this.isUsedInEalierConfig
				};
			}

			// Token: 0x06004421 RID: 17441 RVA: 0x00169C04 File Offset: 0x00168004
			public void Import(ControlData.EyelidPositionBlendshapeForExport export, Transform startXform)
			{
				this.skinnedMeshRenderer = Utils.GetTransformFromPath(startXform, export.skinnedMeshRendererPath).GetComponent<SkinnedMeshRenderer>();
				this.defaultWeight = export.defaultWeight;
				this.positionWeight = export.positionWeight;
				this.index = export.index;
				this.name = export.name;
				this.isUsedInEalierConfig = export.isUsedInEalierConfig;
				if (!string.IsNullOrEmpty(this.name))
				{
					for (int i = 0; i < this.skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
					{
						if (this.skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i).Equals(this.name))
						{
							this.index = i;
							break;
						}
					}
				}
			}

			// Token: 0x04002D77 RID: 11639
			public SkinnedMeshRenderer skinnedMeshRenderer;

			// Token: 0x04002D78 RID: 11640
			public float defaultWeight;

			// Token: 0x04002D79 RID: 11641
			public float positionWeight;

			// Token: 0x04002D7A RID: 11642
			public int index;

			// Token: 0x04002D7B RID: 11643
			public string name;

			// Token: 0x04002D7C RID: 11644
			public bool isUsedInEalierConfig;
		}

		// Token: 0x020008B2 RID: 2226
		[Serializable]
		public class BlendshapesConfigForExport
		{
			// Token: 0x04002D7D RID: 11645
			public string skinnedMeshRendererPath;

			// Token: 0x04002D7E RID: 11646
			public int blendShapeCount;

			// Token: 0x04002D7F RID: 11647
			public string[] blendshapeNames;

			// Token: 0x04002D80 RID: 11648
			public float[] blendshapeWeights;
		}

		// Token: 0x020008B3 RID: 2227
		[Serializable]
		public class BlendshapesConfig
		{
			// Token: 0x06004424 RID: 17444 RVA: 0x00169CD4 File Offset: 0x001680D4
			public static bool CanImport(ControlData.BlendshapesConfigForExport import, Transform startXform)
			{
				Transform transformFromPath = Utils.GetTransformFromPath(startXform, import.skinnedMeshRendererPath);
				if (transformFromPath == null)
				{
					return false;
				}
				SkinnedMeshRenderer component = transformFromPath.GetComponent<SkinnedMeshRenderer>();
				if (component == null)
				{
					return false;
				}
				if (import.blendShapeCount != component.sharedMesh.blendShapeCount)
				{
					return false;
				}
				if (import.blendshapeNames != null && import.blendshapeNames.Length > 0)
				{
					for (int i = 0; i < import.blendShapeCount; i++)
					{
						if (!import.blendshapeNames[i].Equals(component.sharedMesh.GetBlendShapeName(i)))
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x06004425 RID: 17445 RVA: 0x00169D7C File Offset: 0x0016817C
			public ControlData.BlendshapesConfigForExport GetExport(Transform startXform)
			{
				return new ControlData.BlendshapesConfigForExport
				{
					skinnedMeshRendererPath = Utils.GetPathForTransform(startXform, this.skinnedMeshRenderer.transform),
					blendShapeCount = this.blendShapeCount,
					blendshapeNames = this.blendshapeNames,
					blendshapeWeights = this.blendshapeWeights
				};
			}

			// Token: 0x06004426 RID: 17446 RVA: 0x00169DCD File Offset: 0x001681CD
			public void Import(Transform startXform, ControlData.BlendshapesConfigForExport import)
			{
				this.skinnedMeshRenderer = Utils.GetTransformFromPath(startXform, import.skinnedMeshRendererPath).GetComponent<SkinnedMeshRenderer>();
				this.blendShapeCount = import.blendShapeCount;
				this.blendshapeNames = import.blendshapeNames;
				this.blendshapeWeights = import.blendshapeWeights;
			}

			// Token: 0x04002D81 RID: 11649
			public SkinnedMeshRenderer skinnedMeshRenderer;

			// Token: 0x04002D82 RID: 11650
			public int blendShapeCount;

			// Token: 0x04002D83 RID: 11651
			public string[] blendshapeNames;

			// Token: 0x04002D84 RID: 11652
			public float[] blendshapeWeights;
		}
	}
}
