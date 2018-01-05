using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RootMotion.FinalIK;
using UnityEngine;
using VRC;

namespace RealisticEyeMovements
{
	// Token: 0x020008B8 RID: 2232
	public class EyeAndHeadAnimator : MonoBehaviour
	{
		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06004433 RID: 17459 RVA: 0x0016A0BC File Offset: 0x001684BC
		// (remove) Token: 0x06004434 RID: 17460 RVA: 0x0016A0F4 File Offset: 0x001684F4
		public event Action OnTargetLost;

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06004435 RID: 17461 RVA: 0x0016A12A File Offset: 0x0016852A
		// (set) Token: 0x06004436 RID: 17462 RVA: 0x0016A132 File Offset: 0x00168532
		public float blink01 { get; private set; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06004437 RID: 17463 RVA: 0x0016A13B File Offset: 0x0016853B
		// (set) Token: 0x06004438 RID: 17464 RVA: 0x0016A143 File Offset: 0x00168543
		public float eyeDistance { get; private set; }

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06004439 RID: 17465 RVA: 0x0016A14C File Offset: 0x0016854C
		// (set) Token: 0x0600443A RID: 17466 RVA: 0x0016A154 File Offset: 0x00168554
		public float eyeDistanceScale { get; private set; }

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x0600443B RID: 17467 RVA: 0x0016A15D File Offset: 0x0016855D
		// (set) Token: 0x0600443C RID: 17468 RVA: 0x0016A165 File Offset: 0x00168565
		public Transform headParentXform { get; private set; }

		// Token: 0x0600443D RID: 17469 RVA: 0x0016A170 File Offset: 0x00168570
		public static void Cache(string filename)
		{
			VRCWorker.AddJob<EyeAndHeadAnimatorForExport>(() => EyeAndHeadAnimator.LoadFromFile(filename), delegate(EyeAndHeadAnimatorForExport import)
			{
				EyeAndHeadAnimator.Cache(filename, import);
			});
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x0016A1A8 File Offset: 0x001685A8
		public static void Cache(string filename, EyeAndHeadAnimatorForExport import)
		{
			if (import != null)
			{
				EyeAndHeadAnimator.InvalidateCache(filename);
				EyeAndHeadAnimator.cache.Add(filename, import);
			}
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x0016A1C2 File Offset: 0x001685C2
		public static EyeAndHeadAnimatorForExport FetchCache(string filename)
		{
			if (EyeAndHeadAnimator.cache.ContainsKey(filename))
			{
				return EyeAndHeadAnimator.cache[filename];
			}
			return null;
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0016A1E1 File Offset: 0x001685E1
		public static void InvalidateCache(string filename)
		{
			if (EyeAndHeadAnimator.cache.ContainsKey(filename))
			{
				EyeAndHeadAnimator.cache.Remove(filename);
			}
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0016A1FF File Offset: 0x001685FF
		public static void ClearCache()
		{
			EyeAndHeadAnimator.cache.Clear();
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0016A20B File Offset: 0x0016860B
		private void Awake()
		{
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x0016A210 File Offset: 0x00168610
		public void Blink(bool isShortBlink = true)
		{
			if (this.blinkState != EyeAndHeadAnimator.BlinkState.Idle)
			{
				return;
			}
			this.isShortBlink = isShortBlink;
			this.blinkState = EyeAndHeadAnimator.BlinkState.Closing;
			this.blinkStateTime = 0f;
			this.blinkDuration = ((!isShortBlink) ? 0.12f : 0.085f);
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x0016A260 File Offset: 0x00168660
		public bool CanGetIntoView(Vector3 point)
		{
			Vector3 eulerAngles = Quaternion.LookRotation(this.headParentXform.InverseTransformPoint(point)).eulerAngles;
			float num = Mathf.Abs(Utils.NormalizedDegAngle(eulerAngles.x));
			float num2 = Mathf.Abs(Utils.NormalizedDegAngle(eulerAngles.y));
			bool flag = num2 < this.LimitHorizontalHeadAngle(55f) + this.maxEyeHorizAngle + 20f;
			float f = this.controlData.ClampRightVertEyeAngle(eulerAngles.x);
			bool flag2 = num < this.LimitVerticalHeadAngle(40f) + Mathf.Abs(f) + 12f;
			return flag && flag2;
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x0016A306 File Offset: 0x00168706
		public bool CanChangePointOfAttention()
		{
			return Time.time - this.timeOfLastMacroSaccade >= 2f / (1f + this.nervousness);
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x0016A32C File Offset: 0x0016872C
		private void CheckLatencies()
		{
			if (this.eyeLatency > 0f)
			{
				this.eyeLatency -= Time.deltaTime;
				if (this.eyeLatency <= 0f)
				{
					this.currentEyeTargetPOI = this.nextEyeTargetPOI;
					this.currentTargetLeftEyeXform = this.nextTargetLeftEyeXform;
					this.currentTargetRightEyeXform = this.nextTargetRightEyeXform;
					this.StartEyeMovement(this.currentEyeTargetPOI);
				}
			}
			else if (this.headLatency > 0f)
			{
				this.headLatency -= Time.deltaTime;
				if (this.headLatency <= 0f)
				{
					this.StartHeadMovement(this.nextHeadTargetPOI);
				}
			}
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0016A3E0 File Offset: 0x001687E0
		private void CheckMacroSaccades()
		{
			if (this.lookTarget == EyeAndHeadAnimator.LookTarget.SpecificThing)
			{
				return;
			}
			if (this.controlData.eyeControl == ControlData.EyeControl.None)
			{
				return;
			}
			if (this.eyeLatency > 0f)
			{
				return;
			}
			this.timeToMacroSaccade -= Time.deltaTime;
			if (this.timeToMacroSaccade <= 0f)
			{
				if (this.lookTarget == EyeAndHeadAnimator.LookTarget.GeneralDirection && this.useMacroSaccades)
				{
					bool flag = this.controlData.eyelidControl == ControlData.EyelidControl.Bones;
					float x = UnityEngine.Random.Range(-10f * ((!flag) ? 0.3f : 0.65f), 10f * ((!flag) ? 0.4f : 0.65f));
					float y = UnityEngine.Random.Range(-10f, 10f);
					this.SetMacroSaccadeTarget(this.eyesRootXform.TransformPoint(Quaternion.Euler(x, y, 0f) * this.eyesRootXform.InverseTransformPoint(this.GetCurrentEyeTargetPos())));
					this.timeToMacroSaccade = UnityEngine.Random.Range(5f, 8f);
					this.timeToMacroSaccade *= 1f / (1f + this.nervousness);
				}
				else if (this.lookTarget == EyeAndHeadAnimator.LookTarget.Face && this.currentEyeTargetPOI == null)
				{
					switch (this.faceLookTarget)
					{
					case EyeAndHeadAnimator.FaceLookTarget.EyesCenter:
					case EyeAndHeadAnimator.FaceLookTarget.Mouth:
						this.faceLookTarget = ((UnityEngine.Random.value >= 0.5f) ? EyeAndHeadAnimator.FaceLookTarget.RightEye : EyeAndHeadAnimator.FaceLookTarget.LeftEye);
						break;
					case EyeAndHeadAnimator.FaceLookTarget.LeftEye:
						this.faceLookTarget = ((UnityEngine.Random.value >= 0.75f) ? EyeAndHeadAnimator.FaceLookTarget.Mouth : EyeAndHeadAnimator.FaceLookTarget.RightEye);
						break;
					case EyeAndHeadAnimator.FaceLookTarget.RightEye:
						this.faceLookTarget = ((UnityEngine.Random.value >= 0.75f) ? EyeAndHeadAnimator.FaceLookTarget.Mouth : EyeAndHeadAnimator.FaceLookTarget.LeftEye);
						break;
					}
					this.SetMacroSaccadeTarget(this.GetLookTargetPosForSocialTriangle(this.faceLookTarget));
					this.timeToMacroSaccade = ((this.faceLookTarget != EyeAndHeadAnimator.FaceLookTarget.Mouth) ? UnityEngine.Random.Range(1f, 3f) : UnityEngine.Random.Range(0.4f, 0.9f));
					this.timeToMacroSaccade *= 1f / (1f + this.nervousness);
				}
			}
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0016A624 File Offset: 0x00168A24
		private void CheckMicroSaccades()
		{
			if (!this.useMicroSaccades)
			{
				return;
			}
			if (this.controlData.eyeControl == ControlData.EyeControl.None)
			{
				return;
			}
			if (this.eyeLatency > 0f)
			{
				return;
			}
			if (this.lookTarget == EyeAndHeadAnimator.LookTarget.GeneralDirection || this.lookTarget == EyeAndHeadAnimator.LookTarget.SpecificThing || (this.lookTarget == EyeAndHeadAnimator.LookTarget.Face && this.currentEyeTargetPOI != null))
			{
				this.timeToMicroSaccade -= Time.deltaTime;
				if (this.timeToMicroSaccade <= 0f)
				{
					bool flag = this.controlData.eyelidControl == ControlData.EyelidControl.Bones;
					float num = UnityEngine.Random.Range(-3f * ((!flag) ? 0.5f : 0.8f), 3f * ((!flag) ? 0.6f : 0.85f));
					float num2 = UnityEngine.Random.Range(-3f, 3f);
					if (this.lookTarget == EyeAndHeadAnimator.LookTarget.Face)
					{
						num *= 0.5f;
						num2 *= 0.5f;
					}
					if (this.eyesRootXform != null && this.currentEyeTargetPOI != null)
					{
						this.SetMicroSaccadeTarget(this.eyesRootXform.TransformPoint(Quaternion.Euler(num, num2, 0f) * this.eyesRootXform.InverseTransformPoint(this.currentEyeTargetPOI.TransformPoint(this.macroSaccadeTargetLocal))));
					}
				}
			}
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x0016A790 File Offset: 0x00168B90
		private float ClampLeftHorizEyeAngle(float angle)
		{
			float num = Utils.NormalizedDegAngle(angle);
			bool flag = num > 0f;
			float num2 = (!flag) ? this.maxEyeHorizAngle : this.maxEyeHorizAngleTowardsNose;
			return Mathf.Clamp(num, -num2, num2);
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0016A7D0 File Offset: 0x00168BD0
		private float ClampRightHorizEyeAngle(float angle)
		{
			float num = Utils.NormalizedDegAngle(angle);
			bool flag = num < 0f;
			float num2 = (!flag) ? this.maxEyeHorizAngle : this.maxEyeHorizAngleTowardsNose;
			return Mathf.Clamp(num, -num2, num2);
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x0016A810 File Offset: 0x00168C10
		public void ClearLookTarget()
		{
			this.LookAtAreaAround(this.GetOwnEyeCenter() + base.transform.forward * 1000f * this.eyeDistance);
			this.lookTarget = EyeAndHeadAnimator.LookTarget.ClearingTargetPhase1;
			this.timeOfEnteringClearingPhase = Time.time;
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x0016A860 File Offset: 0x00168C60
		private void DrawSightlinesInEditor()
		{
			if (this.controlData.eyeControl != ControlData.EyeControl.None)
			{
				Vector3 a = this.leftEyeAnchor.parent.rotation * this.leftEyeAnchor.localRotation * this.leftAnchorFromEyeRootQ * Vector3.forward;
				Vector3 a2 = this.rightEyeAnchor.parent.rotation * this.rightEyeAnchor.localRotation * this.rightAnchorFromEyeRootQ * Vector3.forward;
				Debug.DrawLine(this.leftEyeAnchor.position, this.leftEyeAnchor.position + a * 10f * this.eyeDistanceScale);
				Debug.DrawLine(this.rightEyeAnchor.position, this.rightEyeAnchor.position + a2 * 10f * this.eyeDistanceScale);
			}
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0016A958 File Offset: 0x00168D58
		public void ExportToFile(string filename)
		{
			EyeAndHeadAnimatorForExport graph = new EyeAndHeadAnimatorForExport
			{
				headBonePath = Utils.GetPathForTransform(base.transform, this.headBoneNonMecanimXform),
				headSpeedModifier = this.headSpeedModifier,
				headWeight = this.headWeight,
				useMicroSaccades = this.useMicroSaccades,
				useMacroSaccades = this.useMacroSaccades,
				kDrawSightlinesInEditor = this.kDrawSightlinesInEditor,
				controlData = this.controlData.GetExport(base.transform),
				kMaxNextBlinkTime = this.kMaxNextBlinkTime,
				eyelidsFollowEyesVertically = this.eyelidsFollowEyesVertically,
				maxEyeHorizAngle = this.maxEyeHorizAngle,
				maxEyeHorizAngleTowardsNose = this.maxEyeHorizAngleTowardsNose,
				crossEyeCorrection = this.crossEyeCorrection,
				nervousness = this.nervousness,
				limitHeadAngle = this.limitHeadAngle
			};
			FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write);
			new BinaryFormatter().Serialize(fileStream, graph);
			fileStream.Close();
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x0016AA48 File Offset: 0x00168E48
		private Vector3 GetCurrentEyeTargetPos()
		{
			return (!(this.currentEyeTargetPOI != null)) ? (0.5f * (this.currentTargetLeftEyeXform.position + this.currentTargetRightEyeXform.position)) : this.currentEyeTargetPOI.position;
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0016AA9C File Offset: 0x00168E9C
		private Vector3 GetCurrentHeadTargetPos()
		{
			return (!(this.currentHeadTargetPOI != null)) ? (0.5f * (this.currentTargetLeftEyeXform.position + this.currentTargetRightEyeXform.position)) : this.currentHeadTargetPOI.position;
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x0016AAF0 File Offset: 0x00168EF0
		private Vector3 GetLookTargetPosForSocialTriangle(EyeAndHeadAnimator.FaceLookTarget playerFaceLookTarget)
		{
			if (this.currentTargetLeftEyeXform == null || this.currentTargetRightEyeXform == null)
			{
				return this.currentEyeTargetPOI.position;
			}
			Vector3 result = Vector3.zero;
			Vector3 a = 0.5f * (this.currentTargetLeftEyeXform.position + this.currentTargetRightEyeXform.position);
			switch (playerFaceLookTarget)
			{
			case EyeAndHeadAnimator.FaceLookTarget.EyesCenter:
				result = this.GetCurrentEyeTargetPos();
				break;
			case EyeAndHeadAnimator.FaceLookTarget.LeftEye:
				result = Vector3.Lerp(a, this.currentTargetLeftEyeXform.position, 0.75f);
				break;
			case EyeAndHeadAnimator.FaceLookTarget.RightEye:
				result = Vector3.Lerp(a, this.currentTargetRightEyeXform.position, 0.75f);
				break;
			case EyeAndHeadAnimator.FaceLookTarget.Mouth:
			{
				Vector3 a2 = 0.5f * (this.currentTargetLeftEyeXform.up + this.currentTargetRightEyeXform.up);
				result = a - a2 * 0.4f * Vector3.Distance(this.currentTargetLeftEyeXform.position, this.currentTargetRightEyeXform.position);
				break;
			}
			}
			return result;
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0016AC14 File Offset: 0x00169014
		public Vector3 GetOwnEyeCenter()
		{
			if (this.eyesRootXform == null)
			{
				return Vector3.zero;
			}
			return this.eyesRootXform.position;
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0016AC38 File Offset: 0x00169038
		private Vector3 GetOwnLookDirection()
		{
			return (!(this.leftEyeAnchor != null) || !(this.rightEyeAnchor != null)) ? this.eyesRootXform.forward : (Quaternion.Slerp(this.leftEyeAnchor.rotation * this.leftAnchorFromEyeRootQ, this.rightEyeAnchor.rotation * this.rightAnchorFromEyeRootQ, 0.5f) * Vector3.forward);
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0016ACB7 File Offset: 0x001690B7
		public float GetStareAngleMeAtTarget(Vector3 target)
		{
			return Vector3.Angle(this.GetOwnLookDirection(), target - this.eyesRootXform.position);
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0016ACD5 File Offset: 0x001690D5
		public float GetStareAngleTargetAtMe(Transform targetXform)
		{
			return Vector3.Angle(targetXform.forward, this.GetOwnEyeCenter() - targetXform.position);
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0016ACF4 File Offset: 0x001690F4
		private static EyeAndHeadAnimatorForExport LoadFromFile(string filename)
		{
			EyeAndHeadAnimatorForExport result = null;
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				try
				{
					result = (EyeAndHeadAnimatorForExport)new BinaryFormatter().Deserialize(fileStream);
				}
				catch (Exception ex)
				{
					if (!ex.Message.StartsWith("null key"))
					{
						Debug.Log("EyeAndHeadAnim: filename=" + filename + ", exception=" + ex.Message);
					}
				}
			}
			return result;
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0016AD88 File Offset: 0x00169188
		public bool ImportFromFile(string filename)
		{
			EyeAndHeadAnimatorForExport eyeAndHeadAnimatorForExport = EyeAndHeadAnimator.FetchCache(filename);
			if (eyeAndHeadAnimatorForExport == null)
			{
				eyeAndHeadAnimatorForExport = EyeAndHeadAnimator.LoadFromFile(filename);
				EyeAndHeadAnimator.Cache(filename, eyeAndHeadAnimatorForExport);
			}
			if (eyeAndHeadAnimatorForExport == null || !Utils.CanGetTransformFromPath(base.transform, eyeAndHeadAnimatorForExport.headBonePath) || !this.controlData.CanImport(eyeAndHeadAnimatorForExport.controlData, base.transform))
			{
				return false;
			}
			this.headBoneNonMecanimXform = Utils.GetTransformFromPath(base.transform, eyeAndHeadAnimatorForExport.headBonePath);
			this.headSpeedModifier = eyeAndHeadAnimatorForExport.headSpeedModifier;
			this.headWeight = eyeAndHeadAnimatorForExport.headWeight;
			this.useMicroSaccades = eyeAndHeadAnimatorForExport.useMicroSaccades;
			this.useMacroSaccades = eyeAndHeadAnimatorForExport.useMacroSaccades;
			this.kDrawSightlinesInEditor = eyeAndHeadAnimatorForExport.kDrawSightlinesInEditor;
			this.controlData.Import(eyeAndHeadAnimatorForExport.controlData, base.transform);
			this.kMaxNextBlinkTime = eyeAndHeadAnimatorForExport.kMaxNextBlinkTime;
			this.eyelidsFollowEyesVertically = eyeAndHeadAnimatorForExport.eyelidsFollowEyesVertically;
			this.maxEyeHorizAngle = eyeAndHeadAnimatorForExport.maxEyeHorizAngle;
			this.maxEyeHorizAngleTowardsNose = eyeAndHeadAnimatorForExport.maxEyeHorizAngleTowardsNose;
			if (this.maxEyeHorizAngleTowardsNose <= 0f)
			{
				this.maxEyeHorizAngleTowardsNose = this.maxEyeHorizAngle;
			}
			this.crossEyeCorrection = eyeAndHeadAnimatorForExport.crossEyeCorrection;
			this.nervousness = eyeAndHeadAnimatorForExport.nervousness;
			this.limitHeadAngle = eyeAndHeadAnimatorForExport.limitHeadAngle;
			this.isInitialized = false;
			return true;
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x0016AECC File Offset: 0x001692CC
		public void Initialize(bool useFIK = true)
		{
			if (this.isInitialized)
			{
				return;
			}
			if (this.controlData == null)
			{
				return;
			}
			base.StartCoroutine(this.InitializeEnumerator(useFIK));
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0016AEF4 File Offset: 0x001692F4
		private IEnumerator InitializeEnumerator(bool useFIK)
		{
			this.useFinalIK = useFIK;
			this.eyeDistance = 0.064f;
			this.animator = base.GetComponentInChildren<Animator>();
			if (this.useFinalIK)
			{
				this.lookAtIK = this.animator.GetComponent<LookAtIK>();
				this.fik = this.animator.GetComponent<VRIK>();
				if (this.fik != null)
				{
					this.headControl = EyeAndHeadAnimator.HeadControl.FinalIK;
					if (this.lookAtIK != null)
					{
						IKSolverLookAt solver = this.lookAtIK.solver;
						solver.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver.OnPostUpdate, new IKSolver.UpdateDelegate(this.VeryLateUpdate));
					}
					else
					{
						IKSolverVR solver2 = this.fik.solver;
						solver2.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Combine(solver2.OnPostUpdate, new IKSolver.UpdateDelegate(this.VeryLateUpdate));
					}
				}
			}
			else
			{
				this.fik = null;
				this.headControl = EyeAndHeadAnimator.HeadControl.None;
			}
			yield return null;
			Transform headXform = null;
			if (this.headControl == EyeAndHeadAnimator.HeadControl.None)
			{
				if (this.animator != null && this.animator.GetBoneTransform(HumanBodyBones.Head) != null)
				{
					this.headControl = EyeAndHeadAnimator.HeadControl.Mecanim;
					headXform = this.animator.GetBoneTransform(HumanBodyBones.Head);
				}
				else if (this.headBoneNonMecanimXform != null)
				{
					this.headControl = EyeAndHeadAnimator.HeadControl.Transform;
					headXform = this.headBoneNonMecanimXform;
					this.headBoneNonMecanimFromRootQ = Quaternion.Inverse(base.transform.rotation) * this.headBoneNonMecanimXform.rotation;
				}
			}
			else if (this.headControl == EyeAndHeadAnimator.HeadControl.FinalIK)
			{
				if (this.lookAtIK != null)
				{
					headXform = this.lookAtIK.solver.head.transform;
				}
				else
				{
					headXform = this.fik.references.head;
				}
			}
			if (headXform == null)
			{
				headXform = base.transform;
			}
			yield return null;
			if (!this.controlData.CheckConsistency(this.animator, this))
			{
				yield break;
			}
			yield return null;
			this.controlData.Initialize();
			yield return null;
			if (this.createdTargetXforms[0] == null)
			{
				this.createdTargetXforms[0] = new GameObject(base.name + "_createdEyeTarget_1").transform;
				DestroyNotifier destroyNotifier = this.createdTargetXforms[0].gameObject.AddComponent<DestroyNotifier>();
				destroyNotifier.OnDestroyedEvent += this.OnCreatedXformDestroyed;
				UnityEngine.Object.DontDestroyOnLoad(this.createdTargetXforms[0].gameObject);
				this.createdTargetXforms[0].gameObject.hideFlags = HideFlags.HideInHierarchy;
			}
			yield return null;
			if (this.createdTargetXforms[1] == null)
			{
				this.createdTargetXforms[1] = new GameObject(base.name + "_createdEyeTarget_2").transform;
				DestroyNotifier destroyNotifier2 = this.createdTargetXforms[1].gameObject.AddComponent<DestroyNotifier>();
				destroyNotifier2.OnDestroyedEvent += this.OnCreatedXformDestroyed;
				UnityEngine.Object.DontDestroyOnLoad(this.createdTargetXforms[1].gameObject);
				this.createdTargetXforms[1].gameObject.hideFlags = HideFlags.HideInHierarchy;
			}
			yield return null;
			if (this.headParentXform == null)
			{
				Transform transform = null;
				if (this.animator != null)
				{
					transform = this.animator.GetBoneTransform(HumanBodyBones.Chest);
					if (transform == null)
					{
						transform = this.animator.GetBoneTransform(HumanBodyBones.Spine);
					}
				}
				if (transform == null)
				{
					transform = base.transform;
				}
				this.headParentXform = new GameObject(base.name + " head parent").transform;
				this.headParentXform.gameObject.hideFlags = HideFlags.HideInHierarchy;
				this.headParentXform.parent = transform;
				this.headParentXform.position = headXform.position;
				this.headParentXform.rotation = base.transform.rotation;
			}
			yield return null;
			if (this.headTargetPivotXform == null)
			{
				this.headTargetPivotXform = new GameObject(base.name + " head target").transform;
				this.headTargetPivotXform.gameObject.hideFlags = HideFlags.HideInHierarchy;
				this.headTargetPivotXform.parent = this.headParentXform;
				this.headTargetPivotXform.localPosition = Vector3.zero;
				this.headTargetPivotXform.localRotation = Quaternion.identity;
				this.critDampTween = new CritDampTweenQuaternion(this.headTargetPivotXform.localRotation, 3.5f, 2f);
				this.lastHeadEuler = this.headTargetPivotXform.localEulerAngles;
			}
			yield return null;
			if (this.controlData.eyeControl == ControlData.EyeControl.MecanimEyeBones || this.controlData.eyeControl == ControlData.EyeControl.SelectedObjects)
			{
				if (this.controlData.eyeControl == ControlData.EyeControl.MecanimEyeBones)
				{
					Transform boneTransform = this.animator.GetBoneTransform(HumanBodyBones.LeftEye);
					Transform boneTransform2 = this.animator.GetBoneTransform(HumanBodyBones.RightEye);
					this.leftEyeAnchor = boneTransform;
					this.rightEyeAnchor = boneTransform2;
					if (this.leftEyeAnchor == null)
					{
						Debug.LogError("Left eye bone not found in Mecanim rig");
					}
					if (this.rightEyeAnchor == null)
					{
						Debug.LogError("Right eye bone not found in Mecanim rig");
					}
				}
				else if (this.controlData.eyeControl == ControlData.EyeControl.SelectedObjects)
				{
					this.leftEyeAnchor = this.controlData.leftEye;
					this.rightEyeAnchor = this.controlData.rightEye;
				}
			}
			if (this.eyesRootXform == null)
			{
				this.eyesRootXform = new GameObject(base.name + "_eyesRoot").transform;
				this.eyesRootXform.gameObject.hideFlags = HideFlags.HideInHierarchy;
				this.eyesRootXform.rotation = base.transform.rotation;
			}
			if (this.leftEyeAnchor != null && this.rightEyeAnchor != null)
			{
				this.eyeDistance = Vector3.Distance(this.leftEyeAnchor.position, this.rightEyeAnchor.position);
				this.eyeDistanceScale = this.eyeDistance / 0.064f;
				this.controlData.RestoreDefault();
				Quaternion lhs = Quaternion.Inverse(this.eyesRootXform.rotation);
				this.leftEyeRootFromAnchorQ = lhs * this.leftEyeAnchor.rotation;
				this.rightEyeRootFromAnchorQ = lhs * this.rightEyeAnchor.rotation;
				this.leftAnchorFromEyeRootQ = Quaternion.Inverse(this.leftEyeRootFromAnchorQ);
				this.rightAnchorFromEyeRootQ = Quaternion.Inverse(this.rightEyeRootFromAnchorQ);
				this.originalLeftEyeLocalQ = this.leftEyeAnchor.localRotation;
				this.originalRightEyeLocalQ = this.rightEyeAnchor.localRotation;
				this.eyesRootXform.position = 0.5f * (this.leftEyeAnchor.position + this.rightEyeAnchor.position);
				Transform commonAncestor = Utils.GetCommonAncestor(this.leftEyeAnchor, this.rightEyeAnchor);
				this.eyesRootXform.parent = ((!(commonAncestor != null)) ? this.leftEyeAnchor.parent : commonAncestor);
			}
			else if (this.animator != null)
			{
				if (headXform != null)
				{
					this.eyesRootXform.position = headXform.position;
					this.eyesRootXform.parent = headXform;
				}
				else
				{
					this.eyesRootXform.position = base.transform.position;
					this.eyesRootXform.parent = base.transform;
				}
			}
			else
			{
				this.eyesRootXform.position = base.transform.position;
				this.eyesRootXform.parent = base.transform;
			}
			yield return null;
			if (this.controlData.eyelidControl == ControlData.EyelidControl.Bones)
			{
				if (this.controlData.upperEyeLidLeft != null && this.controlData.upperEyeLidRight != null)
				{
					this.useUpperEyelids = true;
				}
				if (this.controlData.lowerEyeLidLeft != null && this.controlData.lowerEyeLidRight != null)
				{
					this.useLowerEyelids = true;
				}
			}
			this.blink01 = 0f;
			this.timeOfNextBlink = Time.time + UnityEngine.Random.Range(3f, 6f);
			this.ikWeight = this.headWeight;
			this.isInitialized = true;
			yield break;
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0016AF18 File Offset: 0x00169318
		public void UnInitialize()
		{
			if (this.useFinalIK && this.fik != null)
			{
				if (this.lookAtIK != null)
				{
					IKSolverLookAt solver = this.lookAtIK.solver;
					solver.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver.OnPostUpdate, new IKSolver.UpdateDelegate(this.VeryLateUpdate));
				}
				else
				{
					IKSolverVR solver2 = this.fik.solver;
					solver2.OnPostUpdate = (IKSolver.UpdateDelegate)Delegate.Remove(solver2.OnPostUpdate, new IKSolver.UpdateDelegate(this.VeryLateUpdate));
				}
			}
			this.isInitialized = false;
			this.useFinalIK = false;
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x0016AFC0 File Offset: 0x001693C0
		public bool IsInView(Vector3 target)
		{
			if (this.leftEyeAnchor == null || this.rightEyeAnchor == null)
			{
				Vector3 eulerAngles = Quaternion.LookRotation(this.eyesRootXform.InverseTransformDirection(target - this.GetOwnEyeCenter())).eulerAngles;
				float f = Utils.NormalizedDegAngle(eulerAngles.x);
				float f2 = Utils.NormalizedDegAngle(eulerAngles.y);
				return Mathf.Abs(f) <= 60f && Mathf.Abs(f2) <= 100f;
			}
			Vector3 eulerAngles2 = (this.leftEyeRootFromAnchorQ * Quaternion.Inverse(this.leftEyeAnchor.rotation) * Quaternion.LookRotation(target - this.leftEyeAnchor.position, this.leftEyeAnchor.up)).eulerAngles;
			float f3 = Utils.NormalizedDegAngle(eulerAngles2.x);
			float f4 = Utils.NormalizedDegAngle(eulerAngles2.y);
			bool flag = Mathf.Abs(f3) <= 60f && Mathf.Abs(f4) <= 100f;
			Vector3 eulerAngles3 = (this.rightEyeRootFromAnchorQ * Quaternion.Inverse(this.rightEyeAnchor.rotation) * Quaternion.LookRotation(target - this.rightEyeAnchor.position, this.rightEyeAnchor.up)).eulerAngles;
			float f5 = Utils.NormalizedDegAngle(eulerAngles3.x);
			float f6 = Utils.NormalizedDegAngle(eulerAngles3.y);
			bool flag2 = Mathf.Abs(f5) <= 60f && Mathf.Abs(f6) <= 100f;
			return flag || flag2;
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0016B180 File Offset: 0x00169580
		public bool IsLookingAtFace()
		{
			return this.lookTarget == EyeAndHeadAnimator.LookTarget.Face;
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0016B18C File Offset: 0x0016958C
		private void LateUpdate()
		{
			this.hasLateUpdateRunThisFrame = true;
			if (!this.isInitialized)
			{
				return;
			}
			if (this.lookTarget == EyeAndHeadAnimator.LookTarget.StraightAhead)
			{
				return;
			}
			if (this.headControl == EyeAndHeadAnimator.HeadControl.FinalIK)
			{
				float b = (this.lookTarget != EyeAndHeadAnimator.LookTarget.StraightAhead && this.lookTarget != EyeAndHeadAnimator.LookTarget.ClearingTargetPhase2 && this.lookTarget != EyeAndHeadAnimator.LookTarget.ClearingTargetPhase1) ? this.headWeight : 0f;
				this.ikWeight = Mathf.Lerp(this.ikWeight, b, Time.deltaTime);
				if (this.lookAtIK != null)
				{
					this.lookAtIK.solver.IKPositionWeight = this.ikWeight;
					this.lookAtIK.solver.IKPosition = this.headTargetPivotXform.TransformPoint(this.eyeDistanceScale * Vector3.forward);
				}
			}
			if (this.headControl == EyeAndHeadAnimator.HeadControl.Transform)
			{
				float b2 = (this.lookTarget != EyeAndHeadAnimator.LookTarget.StraightAhead && this.lookTarget != EyeAndHeadAnimator.LookTarget.ClearingTargetPhase2 && this.lookTarget != EyeAndHeadAnimator.LookTarget.ClearingTargetPhase1) ? this.headWeight : 0f;
				this.ikWeight = Mathf.Lerp(this.ikWeight, b2, Time.deltaTime);
				Vector3 a = this.headTargetPivotXform.TransformPoint(this.eyeDistanceScale * Vector3.forward);
				Quaternion b3 = Quaternion.LookRotation(a - this.headBoneNonMecanimXform.position, base.transform.up);
				Quaternion lhs = Quaternion.Slerp(Quaternion.identity, b3, this.ikWeight);
				this.headBoneNonMecanimXform.rotation = lhs * this.headBoneNonMecanimFromRootQ;
			}
			if (this.controlData.eyeControl != ControlData.EyeControl.None)
			{
				this.CheckMicroSaccades();
				this.CheckMacroSaccades();
				Transform transform = (!(this.currentEyeTargetPOI != null)) ? this.currentTargetLeftEyeXform : this.currentEyeTargetPOI;
				if (transform != null && this.OnTargetLost != null && !this.CanGetIntoView(transform.TransformPoint(this.macroSaccadeTargetLocal)) && this.eyeLatency <= 0f)
				{
					this.OnTargetLost();
				}
			}
			this.UpdateHeadMovement();
			if (!this.useFinalIK)
			{
				this.VeryLateUpdate();
			}
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0016B3C4 File Offset: 0x001697C4
		private float LimitHorizontalHeadAngle(float headAngle)
		{
			float num = Mathf.Lerp(55f, 0f, this.limitHeadAngle);
			headAngle = Utils.NormalizedDegAngle(headAngle);
			float num2 = Mathf.Abs(headAngle);
			return Mathf.Sign(headAngle) * (num2 - (140f - num) / Mathf.Pow(140f, 1.5f) * Mathf.Pow(num2, 1.5f));
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x0016B424 File Offset: 0x00169824
		private float LimitVerticalHeadAngle(float headAngle)
		{
			float num = Mathf.Lerp(40f, 0f, this.limitHeadAngle);
			headAngle = Utils.NormalizedDegAngle(headAngle);
			float num2 = Mathf.Abs(headAngle);
			return Mathf.Sign(headAngle) * (num2 - (80f - num) / Mathf.Pow(80f, 1.5f) * Mathf.Pow(num2, 1.5f));
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0016B484 File Offset: 0x00169884
		public void LookAtFace(Transform eyeCenterXform, float headLatency = 0.075f)
		{
			this.lookTarget = EyeAndHeadAnimator.LookTarget.Face;
			this.headSpeed = EyeAndHeadAnimator.HeadSpeed.Fast;
			this.faceLookTarget = EyeAndHeadAnimator.FaceLookTarget.EyesCenter;
			this.nextHeadTargetPOI = eyeCenterXform;
			this.headLatency = headLatency;
			this.currentTargetLeftEyeXform = (this.currentTargetRightEyeXform = null);
			this.nextTargetLeftEyeXform = (this.nextTargetRightEyeXform = null);
			this.StartEyeMovement(eyeCenterXform);
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0016B4DC File Offset: 0x001698DC
		public void LookAtFace(Transform leftEyeXform, Transform rightEyeXform, float headLatency = 0.075f)
		{
			this.lookTarget = EyeAndHeadAnimator.LookTarget.Face;
			this.headSpeed = EyeAndHeadAnimator.HeadSpeed.Fast;
			this.faceLookTarget = EyeAndHeadAnimator.FaceLookTarget.EyesCenter;
			this.headLatency = headLatency;
			this.currentTargetLeftEyeXform = leftEyeXform;
			this.currentTargetRightEyeXform = rightEyeXform;
			this.nextTargetLeftEyeXform = (this.nextTargetRightEyeXform = null);
			this.nextHeadTargetPOI = null;
			this.StartEyeMovement(null);
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0016B534 File Offset: 0x00169934
		public void LookAtSpecificThing(Transform poi, float headLatency = 0.075f)
		{
			this.lookTarget = EyeAndHeadAnimator.LookTarget.SpecificThing;
			this.headSpeed = EyeAndHeadAnimator.HeadSpeed.Fast;
			this.headLatency = headLatency;
			this.nextHeadTargetPOI = poi;
			this.currentTargetLeftEyeXform = (this.currentTargetRightEyeXform = null);
			this.nextTargetLeftEyeXform = (this.nextTargetRightEyeXform = null);
			this.StartEyeMovement(poi);
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0016B584 File Offset: 0x00169984
		public void LookAtSpecificThing(Vector3 point, float headLatency = 0.075f)
		{
			this.createdTargetXformIndex = (this.createdTargetXformIndex + 1) % this.createdTargetXforms.Length;
			this.createdTargetXforms[this.createdTargetXformIndex].position = point;
			this.LookAtSpecificThing(this.createdTargetXforms[this.createdTargetXformIndex], headLatency);
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0016B5C4 File Offset: 0x001699C4
		public void LookAtAreaAround(Transform poi)
		{
			this.lookTarget = EyeAndHeadAnimator.LookTarget.GeneralDirection;
			this.headSpeed = EyeAndHeadAnimator.HeadSpeed.Slow;
			this.eyeLatency = UnityEngine.Random.Range(0.05f, 0.1f);
			this.nextEyeTargetPOI = poi;
			this.currentTargetLeftEyeXform = (this.currentTargetRightEyeXform = null);
			this.nextTargetLeftEyeXform = (this.nextTargetRightEyeXform = null);
			this.StartHeadMovement(poi);
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x0016B622 File Offset: 0x00169A22
		public void LookAtAreaAround(Vector3 point)
		{
			this.createdTargetXformIndex = (this.createdTargetXformIndex + 1) % this.createdTargetXforms.Length;
			this.createdTargetXforms[this.createdTargetXformIndex].position = point;
			this.LookAtAreaAround(this.createdTargetXforms[this.createdTargetXformIndex]);
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0016B664 File Offset: 0x00169A64
		private void OnAnimatorIK()
		{
			if (this.headControl != EyeAndHeadAnimator.HeadControl.Mecanim)
			{
				return;
			}
			if (this.headTargetPivotXform == null)
			{
				return;
			}
			if (this.controlData.eyeControl == ControlData.EyeControl.None)
			{
				return;
			}
			if (this.headWeight <= 0f)
			{
				return;
			}
			float b = (this.lookTarget != EyeAndHeadAnimator.LookTarget.StraightAhead && this.lookTarget != EyeAndHeadAnimator.LookTarget.ClearingTargetPhase2 && this.lookTarget != EyeAndHeadAnimator.LookTarget.ClearingTargetPhase1) ? this.headWeight : 0f;
			this.ikWeight = Mathf.Lerp(this.ikWeight, b, Time.deltaTime);
			this.animator.SetLookAtWeight(1f, 0.01f, this.ikWeight);
			this.animator.SetLookAtPosition(this.headTargetPivotXform.TransformPoint(this.eyeDistanceScale * Vector3.forward));
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x0016B740 File Offset: 0x00169B40
		private void OnCreatedXformDestroyed(DestroyNotifier destroyNotifer)
		{
			Transform component = destroyNotifer.GetComponent<Transform>();
			for (int i = 0; i < this.createdTargetXforms.Length; i++)
			{
				if (this.createdTargetXforms[i] == component)
				{
					this.createdTargetXforms[i] = null;
				}
			}
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0016B78C File Offset: 0x00169B8C
		private void OnDestroy()
		{
			foreach (Transform transform in this.createdTargetXforms)
			{
				if (transform != null)
				{
					transform.GetComponent<DestroyNotifier>().OnDestroyedEvent -= this.OnCreatedXformDestroyed;
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0016B7E6 File Offset: 0x00169BE6
		private void OnEnable()
		{
			if (!this.isInitialized)
			{
				this.Initialize(true);
			}
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0016B7FC File Offset: 0x00169BFC
		private void SetMacroSaccadeTarget(Vector3 targetGlobal)
		{
			this.macroSaccadeTargetLocal = ((!(this.currentEyeTargetPOI != null)) ? this.currentTargetLeftEyeXform : this.currentEyeTargetPOI).InverseTransformPoint(targetGlobal);
			this.timeOfLastMacroSaccade = Time.time;
			this.SetMicroSaccadeTarget(targetGlobal);
			this.timeToMicroSaccade += 0.75f;
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x0016B85C File Offset: 0x00169C5C
		private void SetMicroSaccadeTarget(Vector3 targetGlobal)
		{
			this.microSaccadeTargetLocal = ((!(this.currentEyeTargetPOI != null)) ? this.currentTargetLeftEyeXform : this.currentEyeTargetPOI).InverseTransformPoint(targetGlobal);
			Vector3 eulerAngles = Quaternion.LookRotation(this.eyesRootXform.InverseTransformDirection(targetGlobal - this.leftEyeAnchor.position)).eulerAngles;
			eulerAngles = new Vector3(this.controlData.ClampLeftVertEyeAngle(eulerAngles.x), this.ClampLeftHorizEyeAngle(eulerAngles.y), eulerAngles.z);
			float num = Mathf.Abs(Mathf.DeltaAngle(this.currentLeftEyeLocalEuler.y, eulerAngles.y));
			this.leftMaxSpeedHoriz = 473f * (1f - Mathf.Exp(-num / 7.8f));
			this.leftHorizDuration = 0.025f + 0.00235f * num;
			float num2 = Mathf.Abs(Mathf.DeltaAngle(this.currentLeftEyeLocalEuler.x, eulerAngles.x));
			this.leftMaxSpeedVert = 473f * (1f - Mathf.Exp(-num2 / 7.8f));
			this.leftVertDuration = 0.025f + 0.00235f * num2;
			Vector3 eulerAngles2 = Quaternion.LookRotation(this.eyesRootXform.InverseTransformDirection(targetGlobal - this.rightEyeAnchor.position)).eulerAngles;
			eulerAngles2 = new Vector3(this.controlData.ClampRightVertEyeAngle(eulerAngles2.x), this.ClampRightHorizEyeAngle(eulerAngles2.y), eulerAngles2.z);
			float num3 = Mathf.Abs(Mathf.DeltaAngle(this.currentRightEyeLocalEuler.y, eulerAngles2.y));
			this.rightMaxSpeedHoriz = 473f * (1f - Mathf.Exp(-num3 / 7.8f));
			this.rightHorizDuration = 0.025f + 0.00235f * num3;
			float num4 = Mathf.Abs(Mathf.DeltaAngle(this.currentRightEyeLocalEuler.x, eulerAngles2.x));
			this.rightMaxSpeedVert = 473f * (1f - Mathf.Exp(-num4 / 7.8f));
			this.rightVertDuration = 0.025f + 0.00235f * num4;
			this.leftMaxSpeedHoriz = (this.rightMaxSpeedHoriz = Mathf.Max(this.leftMaxSpeedHoriz, this.rightMaxSpeedHoriz));
			this.leftMaxSpeedVert = (this.rightMaxSpeedVert = Mathf.Max(this.leftMaxSpeedVert, this.rightMaxSpeedVert));
			this.leftHorizDuration = (this.rightHorizDuration = Mathf.Max(this.leftHorizDuration, this.rightHorizDuration));
			this.leftVertDuration = (this.rightVertDuration = Mathf.Max(this.leftVertDuration, this.rightVertDuration));
			this.timeToMicroSaccade = UnityEngine.Random.Range(0.8f, 1.75f);
			this.timeToMicroSaccade *= 1f / (1f + 0.4f * this.nervousness);
			if (this.useUpperEyelids || this.useLowerEyelids || this.controlData.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				float num5 = Mathf.Max(num, Mathf.Max(num3, Mathf.Max(num2, num4)));
				if (num5 >= 25f)
				{
					this.Blink(false);
				}
			}
			this.startLeftEyeHorizDuration = this.leftHorizDuration;
			this.startLeftEyeVertDuration = this.leftVertDuration;
			this.startLeftEyeMaxSpeedHoriz = this.leftMaxSpeedHoriz;
			this.startLeftEyeMaxSpeedVert = this.leftMaxSpeedVert;
			this.startRightEyeHorizDuration = this.rightHorizDuration;
			this.startRightEyeVertDuration = this.rightVertDuration;
			this.startRightEyeMaxSpeedHoriz = this.rightMaxSpeedHoriz;
			this.startRightEyeMaxSpeedVert = this.rightMaxSpeedVert;
			this.timeOfEyeMovementStart = Time.time;
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x0016BC08 File Offset: 0x0016A008
		private void StartEyeMovement(Transform targetXform = null)
		{
			this.eyeLatency = 0f;
			this.currentEyeTargetPOI = targetXform;
			this.nextEyeTargetPOI = null;
			this.nextTargetLeftEyeXform = (this.nextTargetRightEyeXform = null);
			if (this.controlData.eyeControl != ControlData.EyeControl.None)
			{
				this.SetMacroSaccadeTarget(this.GetCurrentEyeTargetPos());
				this.timeToMacroSaccade = UnityEngine.Random.Range(1.5f, 2.5f);
				this.timeToMacroSaccade *= 1f / (1f + this.nervousness);
			}
			if (this.currentHeadTargetPOI == null)
			{
				this.currentHeadTargetPOI = this.currentEyeTargetPOI;
			}
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0016BCAC File Offset: 0x0016A0AC
		private void StartHeadMovement(Transform targetXform = null)
		{
			this.headLatency = 0f;
			this.currentHeadTargetPOI = targetXform;
			this.nextHeadTargetPOI = null;
			this.maxHeadHorizSpeedSinceSaccadeStart = (this.maxHeadVertSpeedSinceSaccadeStart = 0f);
			this.isHeadTracking = false;
			this.headTrackingFactor = 1f;
			if (this.currentEyeTargetPOI == null && this.currentTargetLeftEyeXform == null)
			{
				this.currentEyeTargetPOI = this.currentHeadTargetPOI;
			}
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0016BD26 File Offset: 0x0016A126
		private void Update()
		{
			this.hasLateUpdateRunThisFrame = false;
			if (!this.isInitialized)
			{
				return;
			}
			this.CheckLatencies();
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0016BD44 File Offset: 0x0016A144
		private void UpdateBlinking()
		{
			if (this.blinkState != EyeAndHeadAnimator.BlinkState.Idle)
			{
				this.blinkStateTime += Time.deltaTime;
				if (this.blinkStateTime >= this.blinkDuration)
				{
					this.blinkStateTime = 0f;
					if (this.blinkState == EyeAndHeadAnimator.BlinkState.Closing)
					{
						if (this.isShortBlink)
						{
							this.blinkState = EyeAndHeadAnimator.BlinkState.Opening;
							this.blinkDuration = ((!this.isShortBlink) ? 0.15f : 0.105f);
							this.blink01 = 1f;
						}
						else
						{
							this.blinkState = EyeAndHeadAnimator.BlinkState.KeepingClosed;
							this.blinkDuration = 0.05f;
							this.blink01 = 1f;
						}
					}
					else if (this.blinkState == EyeAndHeadAnimator.BlinkState.KeepingClosed)
					{
						this.blinkState = EyeAndHeadAnimator.BlinkState.Opening;
						this.blinkDuration = ((!this.isShortBlink) ? 0.15f : 0.105f);
					}
					else if (this.blinkState == EyeAndHeadAnimator.BlinkState.Opening)
					{
						this.blinkState = EyeAndHeadAnimator.BlinkState.Idle;
						float min = Mathf.Max(0.1f, Mathf.Min(this.kMinNextBlinkTime, this.kMaxNextBlinkTime));
						float max = Mathf.Max(0.1f, Mathf.Max(this.kMinNextBlinkTime, this.kMaxNextBlinkTime));
						this.timeOfNextBlink = Time.time + UnityEngine.Random.Range(min, max);
						this.blink01 = 0f;
					}
				}
				else
				{
					this.blink01 = Utils.EaseSineIn(this.blinkStateTime, (float)((this.blinkState != EyeAndHeadAnimator.BlinkState.Closing) ? 1 : 0), (float)((this.blinkState != EyeAndHeadAnimator.BlinkState.Closing) ? -1 : 1), this.blinkDuration);
				}
			}
			if (Time.time >= this.timeOfNextBlink && this.blinkState == EyeAndHeadAnimator.BlinkState.Idle)
			{
				this.Blink(true);
			}
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0016BF04 File Offset: 0x0016A304
		private void UpdateEyelids()
		{
			if (this.controlData.eyelidControl == ControlData.EyelidControl.Bones || this.controlData.eyelidControl == ControlData.EyelidControl.Blendshapes)
			{
				this.controlData.UpdateEyelids(this.currentLeftEyeLocalEuler.x, this.currentRightEyeLocalEuler.x, this.blink01, this.eyelidsFollowEyesVertically);
			}
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0016BF60 File Offset: 0x0016A360
		private void UpdateEyeMovement()
		{
			if (this.leftEyeAnchor == null)
			{
				return;
			}
			if (this.rightEyeAnchor == null)
			{
				return;
			}
			if (this.lookTarget == EyeAndHeadAnimator.LookTarget.ClearingTargetPhase2)
			{
				if (Time.time - this.timeOfEnteringClearingPhase >= 1f)
				{
					this.lookTarget = EyeAndHeadAnimator.LookTarget.StraightAhead;
				}
				else
				{
					this.leftEyeAnchor.localRotation = (this.lastLeftEyeLocalRotation = Quaternion.Slerp(this.lastLeftEyeLocalRotation, this.originalLeftEyeLocalQ, Time.deltaTime));
					this.rightEyeAnchor.localRotation = (this.lastRightEyeLocalQ = Quaternion.Slerp(this.lastRightEyeLocalQ, this.originalRightEyeLocalQ, Time.deltaTime));
				}
				return;
			}
			if (this.lookTarget == EyeAndHeadAnimator.LookTarget.ClearingTargetPhase1 && Time.time - this.timeOfEnteringClearingPhase >= 2f)
			{
				this.lookTarget = EyeAndHeadAnimator.LookTarget.ClearingTargetPhase2;
				this.timeOfEnteringClearingPhase = Time.time;
			}
			bool flag = this.lookTarget == EyeAndHeadAnimator.LookTarget.Face;
			bool flag2 = flag && this.faceLookTarget != EyeAndHeadAnimator.FaceLookTarget.EyesCenter;
			Transform transform = (!(this.currentEyeTargetPOI != null)) ? this.currentTargetLeftEyeXform : this.currentEyeTargetPOI;
			Vector3 a = (!flag2) ? transform.TransformPoint(this.microSaccadeTargetLocal) : this.GetLookTargetPosForSocialTriangle(this.faceLookTarget);
			Vector3 ownEyeCenter = this.GetOwnEyeCenter();
			Vector3 a2 = a - ownEyeCenter;
			float num = a2.magnitude / this.eyeDistanceScale;
			float num2 = (!flag) ? 0.6f : 2f;
			float num3 = (!flag) ? 0.2f : 1.5f;
			if (num < num2)
			{
				float num4 = num3 + num * (num2 - num3) / num2;
				num4 = this.crossEyeCorrection * (num4 - num) + num;
				a = ownEyeCenter + this.eyeDistanceScale * num4 * (a2 / num);
			}
			float num5 = Time.time - (this.timeOfEyeMovementStart + 1.5f * this.startLeftEyeHorizDuration);
			if (num5 > 0f)
			{
				this.leftHorizDuration = 0.005f + this.startLeftEyeHorizDuration / (1f + num5);
				this.leftMaxSpeedHoriz = 600f - this.startLeftEyeMaxSpeedHoriz / (1f + num5);
			}
			float num6 = Time.time - (this.timeOfEyeMovementStart + 1.5f * this.startLeftEyeVertDuration);
			if (num6 > 0f)
			{
				this.leftVertDuration = 0.005f + this.startLeftEyeVertDuration / (1f + num6);
				this.leftMaxSpeedVert = 600f - this.startLeftEyeMaxSpeedVert / (1f + num6);
			}
			float num7 = Time.time - (this.timeOfEyeMovementStart + 1.5f * this.startRightEyeHorizDuration);
			if (num7 > 0f)
			{
				this.rightHorizDuration = 0.005f + this.startRightEyeHorizDuration / (1f + num7);
				this.rightMaxSpeedHoriz = 600f - this.startRightEyeMaxSpeedHoriz / (1f + num7);
			}
			float num8 = Time.time - (this.timeOfEyeMovementStart + 1.5f * this.startRightEyeVertDuration);
			if (num8 > 0f)
			{
				this.rightVertDuration = 0.005f + this.startRightEyeVertDuration / (1f + num8);
				this.rightMaxSpeedVert = 600f - this.startRightEyeMaxSpeedVert / (1f + num8);
			}
			Vector3 eulerAngles = Quaternion.LookRotation(this.eyesRootXform.InverseTransformDirection(a - this.leftEyeAnchor.position)).eulerAngles;
			eulerAngles = new Vector3(this.controlData.ClampLeftVertEyeAngle(eulerAngles.x), this.ClampLeftHorizEyeAngle(eulerAngles.y), 0f);
			float deltaTime = Mathf.Max(0.0001f, Time.deltaTime);
			float a3 = 4f * this.maxHeadHorizSpeedSinceSaccadeStart * Mathf.Sign(this.headEulerSpeed.y);
			float a4 = 4f * this.maxHeadVertSpeedSinceSaccadeStart * Mathf.Sign(this.headEulerSpeed.x);
			this.currentLeftEyeLocalEuler = new Vector3(this.controlData.ClampLeftVertEyeAngle(Mathf.SmoothDampAngle(this.currentLeftEyeLocalEuler.x, eulerAngles.x, ref this.leftCurrentSpeedX, this.leftVertDuration, Mathf.Max(a4, this.leftMaxSpeedVert), deltaTime)), this.ClampLeftHorizEyeAngle(Mathf.SmoothDampAngle(this.currentLeftEyeLocalEuler.y, eulerAngles.y, ref this.leftCurrentSpeedY, this.leftHorizDuration, Mathf.Max(a3, this.leftMaxSpeedHoriz), deltaTime)), eulerAngles.z);
			this.leftEyeAnchor.localRotation = Quaternion.Inverse(this.leftEyeAnchor.parent.rotation) * this.eyesRootXform.rotation * Quaternion.Euler(this.currentLeftEyeLocalEuler) * this.leftEyeRootFromAnchorQ;
			Vector3 eulerAngles2 = Quaternion.LookRotation(this.eyesRootXform.InverseTransformDirection(a - this.rightEyeAnchor.position)).eulerAngles;
			eulerAngles2 = new Vector3(this.controlData.ClampRightVertEyeAngle(eulerAngles2.x), this.ClampRightHorizEyeAngle(eulerAngles2.y), 0f);
			this.currentRightEyeLocalEuler = new Vector3(this.controlData.ClampRightVertEyeAngle(Mathf.SmoothDampAngle(this.currentRightEyeLocalEuler.x, eulerAngles2.x, ref this.rightCurrentSpeedX, this.rightVertDuration, Mathf.Max(a4, this.rightMaxSpeedVert), deltaTime)), this.ClampRightHorizEyeAngle(Mathf.SmoothDampAngle(this.currentRightEyeLocalEuler.y, eulerAngles2.y, ref this.rightCurrentSpeedY, this.rightHorizDuration, Mathf.Max(a3, this.rightMaxSpeedHoriz), deltaTime)), eulerAngles2.z);
			this.rightEyeAnchor.localRotation = Quaternion.Inverse(this.rightEyeAnchor.parent.rotation) * this.eyesRootXform.rotation * Quaternion.Euler(this.currentRightEyeLocalEuler) * this.rightEyeRootFromAnchorQ;
			this.lastLeftEyeLocalRotation = this.leftEyeAnchor.localRotation;
			this.lastRightEyeLocalQ = this.rightEyeAnchor.localRotation;
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0016C584 File Offset: 0x0016A984
		private void UpdateHeadMovement()
		{
			if (this.headControl == EyeAndHeadAnimator.HeadControl.None)
			{
				return;
			}
			if (this.controlData.eyeControl == ControlData.EyeControl.None)
			{
				return;
			}
			if (this.ikWeight <= 0f)
			{
				return;
			}
			Vector3 eulerAngles = Quaternion.LookRotation(this.headParentXform.InverseTransformPoint(this.GetCurrentHeadTargetPos())).eulerAngles;
			eulerAngles = new Vector3(this.LimitVerticalHeadAngle(eulerAngles.x), this.LimitHorizontalHeadAngle(eulerAngles.y), 0f);
			if (!this.isHeadTracking)
			{
				Vector3 eulerAngles2 = this.critDampTween.rotation.eulerAngles;
				this.isHeadTracking = (Mathf.Abs(Mathf.DeltaAngle(eulerAngles2.x, eulerAngles.x)) < 2f && Mathf.Abs(Mathf.DeltaAngle(eulerAngles2.y, eulerAngles.y)) < 2f);
			}
			float num = (this.headSpeed != EyeAndHeadAnimator.HeadSpeed.Slow) ? 1f : 0.5f;
			float b = (float)((!this.isHeadTracking) ? 1 : 5);
			this.headTrackingFactor = Mathf.Lerp(this.headTrackingFactor, b, Time.deltaTime * 3f);
			this.critDampTween.omega = num * this.headSpeedModifier * this.headTrackingFactor * 3.5f;
			this.critDampTween.Step(Quaternion.Euler(eulerAngles));
			float d = Mathf.Max(Time.deltaTime, 0.0001f);
			this.headTargetPivotXform.localEulerAngles = this.critDampTween.rotation.eulerAngles;
			this.headEulerSpeed = (this.headTargetPivotXform.localEulerAngles - this.lastHeadEuler) / d;
			this.lastHeadEuler = this.headTargetPivotXform.localEulerAngles;
			this.maxHeadHorizSpeedSinceSaccadeStart = Mathf.Max(this.maxHeadHorizSpeedSinceSaccadeStart, Mathf.Abs(this.headEulerSpeed.y));
			this.maxHeadVertSpeedSinceSaccadeStart = Mathf.Max(this.maxHeadHorizSpeedSinceSaccadeStart, Mathf.Abs(this.headEulerSpeed.x));
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x0016C794 File Offset: 0x0016AB94
		public void VeryLateUpdate()
		{
			if (!this.isInitialized)
			{
				return;
			}
			if (this.lookTarget == EyeAndHeadAnimator.LookTarget.StraightAhead)
			{
				return;
			}
			if (this.headControl == EyeAndHeadAnimator.HeadControl.FinalIK && !this.hasLateUpdateRunThisFrame)
			{
				Debug.LogError("Since the last update of Realistic Eye Movements (REM), FinalIK scripts must run after REM scripts. Please remove the REM scripts from the script execution order list or move them to run before FinalIK scripts. ");
			}
			if (this.controlData.eyeControl != ControlData.EyeControl.None)
			{
				this.UpdateEyeMovement();
			}
			this.UpdateBlinking();
			this.UpdateEyelids();
			if (this.kDrawSightlinesInEditor)
			{
				this.DrawSightlinesInEditor();
			}
		}

		// Token: 0x04002D9B RID: 11675
		private const float kMaxLimitedHorizontalHeadAngle = 55f;

		// Token: 0x04002D9C RID: 11676
		private const float kMaxLimitedVerticalHeadAngle = 40f;

		// Token: 0x04002D9D RID: 11677
		private const float kMaxHorizViewAngle = 100f;

		// Token: 0x04002D9E RID: 11678
		private const float kMaxVertViewAngle = 60f;

		// Token: 0x04002DA0 RID: 11680
		[HideInInspector]
		public float headSpeedModifier = 1f;

		// Token: 0x04002DA1 RID: 11681
		[HideInInspector]
		public float headWeight = 1f;

		// Token: 0x04002DA2 RID: 11682
		[HideInInspector]
		public Transform headBoneNonMecanimXform;

		// Token: 0x04002DA3 RID: 11683
		private Quaternion headBoneNonMecanimFromRootQ;

		// Token: 0x04002DA4 RID: 11684
		private EyeAndHeadAnimator.HeadControl headControl;

		// Token: 0x04002DA5 RID: 11685
		public bool useMicroSaccades = true;

		// Token: 0x04002DA6 RID: 11686
		public bool useMacroSaccades = true;

		// Token: 0x04002DA7 RID: 11687
		public bool kDrawSightlinesInEditor;

		// Token: 0x04002DA8 RID: 11688
		[HideInInspector]
		public ControlData controlData = new ControlData();

		// Token: 0x04002DA9 RID: 11689
		[Tooltip("Minimum seconds until next blink")]
		public float kMinNextBlinkTime = 3f;

		// Token: 0x04002DAA RID: 11690
		[Tooltip("Maximum seconds until next blink")]
		public float kMaxNextBlinkTime = 15f;

		// Token: 0x04002DAB RID: 11691
		[Tooltip("Whether the eyelids move up a bit when looking up and down when looking down.")]
		public bool eyelidsFollowEyesVertically = true;

		// Token: 0x04002DAD RID: 11693
		private bool useUpperEyelids;

		// Token: 0x04002DAE RID: 11694
		private bool useLowerEyelids;

		// Token: 0x04002DAF RID: 11695
		private float timeOfNextBlink;

		// Token: 0x04002DB0 RID: 11696
		private EyeAndHeadAnimator.BlinkState blinkState;

		// Token: 0x04002DB1 RID: 11697
		private float blinkStateTime;

		// Token: 0x04002DB2 RID: 11698
		private float blinkDuration;

		// Token: 0x04002DB3 RID: 11699
		private bool isShortBlink;

		// Token: 0x04002DB4 RID: 11700
		private const float kBlinkCloseTimeShort = 0.085f;

		// Token: 0x04002DB5 RID: 11701
		private const float kBlinkOpenTimeShort = 0.105f;

		// Token: 0x04002DB6 RID: 11702
		private const float kBlinkCloseTimeLong = 0.12f;

		// Token: 0x04002DB7 RID: 11703
		private const float kBlinkOpenTimeLong = 0.15f;

		// Token: 0x04002DB8 RID: 11704
		private const float kBlinkKeepingClosedTime = 0.05f;

		// Token: 0x04002DB9 RID: 11705
		[Tooltip("Maximum horizontal eye angle (away from nose)")]
		public float maxEyeHorizAngle = 35f;

		// Token: 0x04002DBA RID: 11706
		[Tooltip("Maximum horizontal eye angle towards nose")]
		public float maxEyeHorizAngleTowardsNose = 35f;

		// Token: 0x04002DBB RID: 11707
		[Tooltip("Cross eye correction factor")]
		[Range(0f, 5f)]
		public float crossEyeCorrection = 1f;

		// Token: 0x04002DBC RID: 11708
		[Tooltip("The more nervous, the more often you do micro-and macrosaccades.")]
		[Range(0f, 10f)]
		public float nervousness;

		// Token: 0x04002DBD RID: 11709
		[Tooltip("Limits the angle for the head movement")]
		[Range(0f, 1f)]
		public float limitHeadAngle;

		// Token: 0x04002DC0 RID: 11712
		private Transform leftEyeAnchor;

		// Token: 0x04002DC1 RID: 11713
		private Transform rightEyeAnchor;

		// Token: 0x04002DC2 RID: 11714
		private float leftMaxSpeedHoriz;

		// Token: 0x04002DC3 RID: 11715
		private float leftHorizDuration;

		// Token: 0x04002DC4 RID: 11716
		private float leftMaxSpeedVert;

		// Token: 0x04002DC5 RID: 11717
		private float leftVertDuration;

		// Token: 0x04002DC6 RID: 11718
		private float leftCurrentSpeedX;

		// Token: 0x04002DC7 RID: 11719
		private float leftCurrentSpeedY;

		// Token: 0x04002DC8 RID: 11720
		private float rightMaxSpeedHoriz;

		// Token: 0x04002DC9 RID: 11721
		private float rightHorizDuration;

		// Token: 0x04002DCA RID: 11722
		private float rightMaxSpeedVert;

		// Token: 0x04002DCB RID: 11723
		private float rightVertDuration;

		// Token: 0x04002DCC RID: 11724
		private float rightCurrentSpeedX;

		// Token: 0x04002DCD RID: 11725
		private float rightCurrentSpeedY;

		// Token: 0x04002DCE RID: 11726
		private float startLeftEyeHorizDuration;

		// Token: 0x04002DCF RID: 11727
		private float startLeftEyeVertDuration;

		// Token: 0x04002DD0 RID: 11728
		private float startLeftEyeMaxSpeedHoriz;

		// Token: 0x04002DD1 RID: 11729
		private float startLeftEyeMaxSpeedVert;

		// Token: 0x04002DD2 RID: 11730
		private float startRightEyeHorizDuration;

		// Token: 0x04002DD3 RID: 11731
		private float startRightEyeVertDuration;

		// Token: 0x04002DD4 RID: 11732
		private float startRightEyeMaxSpeedHoriz;

		// Token: 0x04002DD5 RID: 11733
		private float startRightEyeMaxSpeedVert;

		// Token: 0x04002DD6 RID: 11734
		private float timeOfEyeMovementStart;

		// Token: 0x04002DD7 RID: 11735
		private const float kMaxHeadVelocity = 2f;

		// Token: 0x04002DD8 RID: 11736
		private const float kHeadOmega = 3.5f;

		// Token: 0x04002DD9 RID: 11737
		private CritDampTweenQuaternion critDampTween;

		// Token: 0x04002DDA RID: 11738
		private Vector3 headEulerSpeed;

		// Token: 0x04002DDB RID: 11739
		private Vector3 lastHeadEuler;

		// Token: 0x04002DDC RID: 11740
		private float maxHeadHorizSpeedSinceSaccadeStart;

		// Token: 0x04002DDD RID: 11741
		private float maxHeadVertSpeedSinceSaccadeStart;

		// Token: 0x04002DDE RID: 11742
		private bool isHeadTracking;

		// Token: 0x04002DDF RID: 11743
		private float headTrackingFactor = 1f;

		// Token: 0x04002DE0 RID: 11744
		private float headLatency;

		// Token: 0x04002DE1 RID: 11745
		private float eyeLatency;

		// Token: 0x04002DE2 RID: 11746
		private float ikWeight = 1f;

		// Token: 0x04002DE3 RID: 11747
		private Animator animator;

		// Token: 0x04002DE4 RID: 11748
		public bool useFinalIK;

		// Token: 0x04002DE5 RID: 11749
		private LookAtIK lookAtIK;

		// Token: 0x04002DE6 RID: 11750
		private VRIK fik;

		// Token: 0x04002DE7 RID: 11751
		private bool hasLateUpdateRunThisFrame;

		// Token: 0x04002DE8 RID: 11752
		private Transform currentHeadTargetPOI;

		// Token: 0x04002DE9 RID: 11753
		private Transform currentEyeTargetPOI;

		// Token: 0x04002DEA RID: 11754
		private Transform nextHeadTargetPOI;

		// Token: 0x04002DEB RID: 11755
		private Transform nextEyeTargetPOI;

		// Token: 0x04002DEC RID: 11756
		private Transform currentTargetLeftEyeXform;

		// Token: 0x04002DED RID: 11757
		private Transform currentTargetRightEyeXform;

		// Token: 0x04002DEE RID: 11758
		private Transform nextTargetLeftEyeXform;

		// Token: 0x04002DEF RID: 11759
		private Transform nextTargetRightEyeXform;

		// Token: 0x04002DF0 RID: 11760
		private readonly Transform[] createdTargetXforms = new Transform[2];

		// Token: 0x04002DF1 RID: 11761
		private int createdTargetXformIndex;

		// Token: 0x04002DF2 RID: 11762
		private Transform eyesRootXform;

		// Token: 0x04002DF4 RID: 11764
		private Transform headTargetPivotXform;

		// Token: 0x04002DF5 RID: 11765
		private Quaternion leftEyeRootFromAnchorQ;

		// Token: 0x04002DF6 RID: 11766
		private Quaternion rightEyeRootFromAnchorQ;

		// Token: 0x04002DF7 RID: 11767
		private Quaternion leftAnchorFromEyeRootQ;

		// Token: 0x04002DF8 RID: 11768
		private Quaternion rightAnchorFromEyeRootQ;

		// Token: 0x04002DF9 RID: 11769
		private Vector3 currentLeftEyeLocalEuler;

		// Token: 0x04002DFA RID: 11770
		private Vector3 currentRightEyeLocalEuler;

		// Token: 0x04002DFB RID: 11771
		private Quaternion originalLeftEyeLocalQ;

		// Token: 0x04002DFC RID: 11772
		private Quaternion originalRightEyeLocalQ;

		// Token: 0x04002DFD RID: 11773
		private Quaternion lastLeftEyeLocalRotation;

		// Token: 0x04002DFE RID: 11774
		private Quaternion lastRightEyeLocalQ;

		// Token: 0x04002DFF RID: 11775
		private Vector3 macroSaccadeTargetLocal;

		// Token: 0x04002E00 RID: 11776
		private Vector3 microSaccadeTargetLocal;

		// Token: 0x04002E01 RID: 11777
		private float timeOfEnteringClearingPhase;

		// Token: 0x04002E02 RID: 11778
		private float timeOfLastMacroSaccade = -100f;

		// Token: 0x04002E03 RID: 11779
		private float timeToMicroSaccade;

		// Token: 0x04002E04 RID: 11780
		private float timeToMacroSaccade;

		// Token: 0x04002E05 RID: 11781
		private bool isInitialized;

		// Token: 0x04002E06 RID: 11782
		private EyeAndHeadAnimator.HeadSpeed headSpeed;

		// Token: 0x04002E07 RID: 11783
		private EyeAndHeadAnimator.LookTarget lookTarget;

		// Token: 0x04002E08 RID: 11784
		private EyeAndHeadAnimator.FaceLookTarget faceLookTarget;

		// Token: 0x04002E09 RID: 11785
		private static Dictionary<string, EyeAndHeadAnimatorForExport> cache = new Dictionary<string, EyeAndHeadAnimatorForExport>();

		// Token: 0x020008B9 RID: 2233
		private enum HeadControl
		{
			// Token: 0x04002E0B RID: 11787
			None,
			// Token: 0x04002E0C RID: 11788
			Mecanim,
			// Token: 0x04002E0D RID: 11789
			FinalIK,
			// Token: 0x04002E0E RID: 11790
			Transform
		}

		// Token: 0x020008BA RID: 2234
		private enum BlinkState
		{
			// Token: 0x04002E10 RID: 11792
			Idle,
			// Token: 0x04002E11 RID: 11793
			Closing,
			// Token: 0x04002E12 RID: 11794
			KeepingClosed,
			// Token: 0x04002E13 RID: 11795
			Opening
		}

		// Token: 0x020008BB RID: 2235
		public enum HeadSpeed
		{
			// Token: 0x04002E15 RID: 11797
			Slow,
			// Token: 0x04002E16 RID: 11798
			Fast
		}

		// Token: 0x020008BC RID: 2236
		public enum EyeDelay
		{
			// Token: 0x04002E18 RID: 11800
			Simultaneous,
			// Token: 0x04002E19 RID: 11801
			EyesFirst,
			// Token: 0x04002E1A RID: 11802
			HeadFirst
		}

		// Token: 0x020008BD RID: 2237
		private enum LookTarget
		{
			// Token: 0x04002E1C RID: 11804
			StraightAhead,
			// Token: 0x04002E1D RID: 11805
			ClearingTargetPhase1,
			// Token: 0x04002E1E RID: 11806
			ClearingTargetPhase2,
			// Token: 0x04002E1F RID: 11807
			GeneralDirection,
			// Token: 0x04002E20 RID: 11808
			SpecificThing,
			// Token: 0x04002E21 RID: 11809
			Face
		}

		// Token: 0x020008BE RID: 2238
		private enum FaceLookTarget
		{
			// Token: 0x04002E23 RID: 11811
			EyesCenter,
			// Token: 0x04002E24 RID: 11812
			LeftEye,
			// Token: 0x04002E25 RID: 11813
			RightEye,
			// Token: 0x04002E26 RID: 11814
			Mouth
		}
	}
}
