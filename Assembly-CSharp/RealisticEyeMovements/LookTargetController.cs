using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

namespace RealisticEyeMovements
{
	// Token: 0x020008BF RID: 2239
	public class LookTargetController : MonoBehaviour
	{
		// Token: 0x06004475 RID: 17525 RVA: 0x0016D566 File Offset: 0x0016B966
		private void Awake()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x0016D579 File Offset: 0x0016B979
		public void Blink()
		{
			this.eyeAndHeadAnimator.Blink(true);
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0016D588 File Offset: 0x0016B988
		private void ChangeStateTo(LookTargetController.State newState)
		{
			if (this.state != LookTargetController.State.LookingAtPlayer && newState == LookTargetController.State.LookingAtPlayer && this.OnStartLookingAtPlayer != null)
			{
				this.OnStartLookingAtPlayer.Invoke();
			}
			if (this.state == LookTargetController.State.LookingAtPlayer && newState != LookTargetController.State.LookingAtPlayer && this.OnStopLookingAtPlayer != null)
			{
				this.OnStopLookingAtPlayer.Invoke();
			}
			this.state = newState;
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0016D5EC File Offset: 0x0016B9EC
		private Vector3 ChooseNextHeadTargetPoint()
		{
			bool flag = this.eyeAndHeadAnimator.controlData.eyelidControl == ControlData.EyelidControl.Bones;
			float x = UnityEngine.Random.Range(-0.5f * ((!flag) ? 3f : 6f), (!flag) ? 4f : 6f);
			float y = UnityEngine.Random.Range(-10f, 10f);
			return this.eyeAndHeadAnimator.GetOwnEyeCenter() + this.eyeAndHeadAnimator.eyeDistanceScale * UnityEngine.Random.Range(3f, 5f) * this.eyeAndHeadAnimator.headParentXform.TransformDirection(Quaternion.Euler(x, y, 0f) * Vector3.forward);
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x0016D6AC File Offset: 0x0016BAAC
		private Transform ChooseNextHeadTargetPOI()
		{
			if (this.pointsOfInterest == null || this.pointsOfInterest.Length == 0)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < this.pointsOfInterest.Length; i++)
			{
				if (this.pointsOfInterest[i] != this.targetPOI && this.eyeAndHeadAnimator.CanGetIntoView(this.pointsOfInterest[i].position))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return this.targetPOI;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			int num3 = 0;
			for (int j = 0; j < this.pointsOfInterest.Length; j++)
			{
				if (this.pointsOfInterest[j] != this.targetPOI && this.eyeAndHeadAnimator.CanGetIntoView(this.pointsOfInterest[j].position))
				{
					if (num3 == num2)
					{
						return this.pointsOfInterest[j];
					}
					num3++;
				}
			}
			return null;
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0016D7A8 File Offset: 0x0016BBA8
		public void ClearLookTarget()
		{
			this.eyeAndHeadAnimator.ClearLookTarget();
			this.nextChangePOITime = -1f;
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x0016D7C0 File Offset: 0x0016BBC0
		public void Initialize()
		{
			if (this.isInitialized)
			{
				return;
			}
			if (this.createdVRParentGO != null)
			{
				DestroyNotifier component = this.createdVRParentGO.GetComponent<DestroyNotifier>();
				if (component != null)
				{
					component.OnDestroyedEvent -= this.OnPlayerEyesParentDestroyed;
				}
				UnityEngine.Object.Destroy(this.createdVRParentGO);
				this.createdVRParentGO = null;
				this.createdPlayerEyeCenterGO = null;
				this.createdPlayerLeftEyeGO = null;
				this.createdPlayerRightEyeGO = null;
			}
			this.eyeAndHeadAnimator = base.GetComponent<EyeAndHeadAnimator>();
			this.eyeAndHeadAnimator.Initialize(true);
			this.playerEyeCenterXform = (this.playerLeftEyeXform = (this.playerRightEyeXform = null));
			this.useNativeVRSupport = (this.useVR = (VRDevice.isPresent && VRSettings.enabled));
			this.useNativeVRSupport = (this.useVR = false);
			GameObject gameObject = GameObject.Find("OVRCameraRig");
			if (gameObject != null)
			{
				this.useVR = true;
				this.useNativeVRSupport = false;
				this.playerLeftEyeXform = Utils.FindChildInHierarchy(gameObject, "LeftEyeAnchor").transform;
				this.playerRightEyeXform = Utils.FindChildInHierarchy(gameObject, "RightEyeAnchor").transform;
				this.playerEyeCenterXform = Utils.FindChildInHierarchy(gameObject, "CenterEyeAnchor").transform;
			}
			else if (this.useNativeVRSupport)
			{
				if (Camera.main == null)
				{
					Debug.LogError("Main camera not found. Please set the main camera's tag to 'MainCamera'.");
					this.useVR = false;
					this.useNativeVRSupport = false;
					this.lookAtPlayerRatio = 0f;
				}
				else
				{
					this.createdPlayerEyeCenterGO = new GameObject("CreatedPlayerCenterVREye")
					{
						hideFlags = HideFlags.HideInHierarchy
					};
					this.createdPlayerLeftEyeGO = new GameObject("CreatedPlayerLeftVREye")
					{
						hideFlags = HideFlags.HideInHierarchy
					};
					this.createdPlayerRightEyeGO = new GameObject("CreatedPlayerRightVREye")
					{
						hideFlags = HideFlags.HideInHierarchy
					};
					this.playerEyeCenterXform = this.createdPlayerEyeCenterGO.transform;
					this.playerLeftEyeXform = this.createdPlayerLeftEyeGO.transform;
					this.playerRightEyeXform = this.createdPlayerRightEyeGO.transform;
					Transform transform = Camera.main.transform;
					this.createdVRParentGO = new GameObject("PlayerEyesParent")
					{
						hideFlags = HideFlags.HideInHierarchy
					};
					this.createdVRParentGO = new GameObject("PlayerEyesParent");
					MonoBehaviour.print("created createdVRParentGO");
					DestroyNotifier destroyNotifier = this.createdVRParentGO.AddComponent<DestroyNotifier>();
					destroyNotifier.OnDestroyedEvent += this.OnPlayerEyesParentDestroyed;
					this.createdVRParentGO.transform.position = transform.position;
					this.createdVRParentGO.transform.rotation = transform.rotation;
					this.createdVRParentGO.transform.parent = transform.parent;
					this.createdPlayerEyeCenterGO.transform.parent = this.createdVRParentGO.transform;
					this.createdPlayerLeftEyeGO.transform.parent = this.createdVRParentGO.transform;
					this.createdPlayerRightEyeGO.transform.parent = this.createdVRParentGO.transform;
					this.UpdateNativeVRTransforms();
				}
			}
			if (!this.useVR)
			{
				if (this.playerEyeCenter != null)
				{
					this.playerEyeCenterXform = this.playerEyeCenter;
				}
				else if (Camera.main != null)
				{
					this.playerEyeCenterXform = Camera.main.transform;
				}
				else
				{
					Debug.LogError("Main camera not found. Please set the main camera's tag to 'MainCamera' or set Player Eye Center.");
					this.lookAtPlayerRatio = 0f;
				}
			}
			this.UpdateThirdPersonPlayerEyeCenter();
			this.LookAroundIdly();
			this.nextChangePOITime = 0f;
			this.isInitialized = true;
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x0016DB56 File Offset: 0x0016BF56
		public bool IsPlayerInView()
		{
			this.UpdateCameraTransformIfNecessary();
			return this.playerEyeCenterXform != null && this.eyeAndHeadAnimator.IsInView(this.playerEyeCenterXform.position);
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x0016DB88 File Offset: 0x0016BF88
		private void LateUpdate()
		{
			if (!this.isInitialized || this.playerEyeCenterXform == null)
			{
				return;
			}
			if (this.playerEyeCenter != this.usedThirdPersonPlayerEyeCenter)
			{
				this.UpdateThirdPersonPlayerEyeCenter();
			}
			if (this.useNativeVRSupport && this.usedThirdPersonPlayerEyeCenter == null)
			{
				this.UpdateNativeVRTransforms();
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Vector3 position = this.playerEyeCenterXform.position;
			float num = Vector3.Distance(this.eyeAndHeadAnimator.GetOwnEyeCenter(), position);
			bool flag4 = this.eyeAndHeadAnimator.IsInView(this.playerEyeCenterXform.position);
			bool flag5 = flag4 && num < this.noticePlayerDistance;
			bool flag6 = flag4 && num < this.personalSpaceDistance;
			if (flag5)
			{
				if (Time.time - this.timeOfLastNoticeCheck > 0.1f && this.state != LookTargetController.State.LookingAtPlayer)
				{
					this.timeOfLastNoticeCheck = Time.time;
					bool flag7 = this.lastDistanceToPlayer > num;
					float t = (this.noticePlayerDistance - num) / this.noticePlayerDistance;
					float num2 = Mathf.Lerp(0.1f, 0.5f, t);
					flag2 = (flag7 && this.timeOutsideOfAwarenessZone > 1f && UnityEngine.Random.value < num2);
				}
			}
			else
			{
				this.timeOutsideOfAwarenessZone += Time.deltaTime;
			}
			if (flag6)
			{
				this.timeInsidePersonalSpace += Time.deltaTime * Mathf.Clamp01((this.personalSpaceDistance - num) / (0.5f * this.personalSpaceDistance));
				if (this.timeInsidePersonalSpace >= 1f)
				{
					flag3 = true;
				}
			}
			else
			{
				this.timeInsidePersonalSpace = 0f;
			}
			if (flag3 && this.state != LookTargetController.State.LookingAwayFromPlayer)
			{
				this.LookAwayFromPlayer();
				return;
			}
			if (this.nextChangePOITime >= 0f && Time.time >= this.nextChangePOITime && this.eyeAndHeadAnimator.CanChangePointOfAttention())
			{
				if (UnityEngine.Random.value <= this.lookAtPlayerRatio && this.IsPlayerInView())
				{
					this.LookAtPlayer(UnityEngine.Random.Range(Mathf.Min(this.minLookTime, this.maxLookTime), Mathf.Max(this.minLookTime, this.maxLookTime)), 0.075f);
				}
				else
				{
					this.LookAroundIdly();
				}
				return;
			}
			if (this.stareBackFactor > 0f && this.playerEyeCenterXform != null)
			{
				float stareAngleTargetAtMe = this.eyeAndHeadAnimator.GetStareAngleTargetAtMe(this.playerEyeCenterXform);
				bool flag8 = stareAngleTargetAtMe < 15f;
				this.playerLookingAtMeTime = ((!flag4 || !flag8) ? Mathf.Max(0f, this.playerLookingAtMeTime - Time.deltaTime) : Mathf.Min(10f, this.playerLookingAtMeTime + Mathf.Cos(0.0174532924f * stareAngleTargetAtMe) * Time.deltaTime));
				if (!this.eyeAndHeadAnimator.IsLookingAtFace())
				{
					if (this.stareBackDeadtime > 0f)
					{
						this.stareBackDeadtime -= Time.deltaTime;
					}
					if (this.stareBackDeadtime <= 0f && Time.time - this.timeOfLastLookBackCheck > 0.1f && this.playerLookingAtMeTime > 4f && this.eyeAndHeadAnimator.CanChangePointOfAttention() && flag8)
					{
						this.timeOfLastLookBackCheck = Time.time;
						float num3 = this.stareBackFactor * 2f * (Mathf.Min(10f, this.playerLookingAtMeTime) - 4f) / 6f;
						flag = (UnityEngine.Random.value < num3);
					}
				}
			}
			if (flag || flag2)
			{
				this.LookAtPlayer(UnityEngine.Random.Range(Mathf.Min(this.minLookTime, this.maxLookTime), Mathf.Max(this.minLookTime, this.maxLookTime)), 0.075f);
			}
			this.lastDistanceToPlayer = num;
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0016DF88 File Offset: 0x0016C388
		public void LookAtPlayer(float duration = -1f, float headLatency = 0.075f)
		{
			this.UpdateCameraTransformIfNecessary();
			if (this.playerLeftEyeXform != null && this.playerRightEyeXform != null)
			{
				this.eyeAndHeadAnimator.LookAtFace(this.playerLeftEyeXform, this.playerRightEyeXform, headLatency);
			}
			else
			{
				this.eyeAndHeadAnimator.LookAtFace(this.playerEyeCenterXform, headLatency);
			}
			this.nextChangePOITime = ((duration < 0f) ? -1f : (Time.time + duration));
			this.targetPOI = null;
			this.timeOutsideOfAwarenessZone = 0f;
			this.ChangeStateTo(LookTargetController.State.LookingAtPlayer);
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x0016E028 File Offset: 0x0016C428
		public void LookAroundIdly()
		{
			if (this.state == LookTargetController.State.LookingAtPlayer)
			{
				this.stareBackDeadtime = UnityEngine.Random.Range(10f, 30f);
			}
			this.targetPOI = this.ChooseNextHeadTargetPOI();
			if (this.targetPOI != null)
			{
				this.eyeAndHeadAnimator.LookAtAreaAround(this.targetPOI);
			}
			else
			{
				this.eyeAndHeadAnimator.LookAtAreaAround(this.ChooseNextHeadTargetPoint());
			}
			this.nextChangePOITime = Time.time + UnityEngine.Random.Range(Mathf.Min(this.minLookTime, this.maxLookTime), Mathf.Max(this.minLookTime, this.maxLookTime));
			this.ChangeStateTo(LookTargetController.State.LookingAroundIdly);
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x0016E0D3 File Offset: 0x0016C4D3
		public void LookAtPoiDirectly(Transform poiXform, float duration = -1f, float headLatency = 0.075f)
		{
			this.eyeAndHeadAnimator.LookAtSpecificThing(poiXform, headLatency);
			this.nextChangePOITime = ((duration < 0f) ? -1f : (Time.time + duration));
			this.ChangeStateTo(LookTargetController.State.LookingAtPoiDirectly);
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0016E10B File Offset: 0x0016C50B
		public void LookAtPoiDirectly(Vector3 poi, float duration = -1f, float headLatency = 0.075f)
		{
			this.eyeAndHeadAnimator.LookAtSpecificThing(poi, headLatency);
			this.nextChangePOITime = ((duration < 0f) ? -1f : (Time.time + duration));
			this.ChangeStateTo(LookTargetController.State.LookingAtPoiDirectly);
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0016E144 File Offset: 0x0016C544
		private void LookAwayFromPlayer()
		{
			this.stareBackDeadtime = UnityEngine.Random.Range(5f, 10f);
			bool flag = this.eyeAndHeadAnimator.headParentXform.InverseTransformPoint(this.playerEyeCenterXform.position).x < 0f;
			Vector3 point = this.eyeAndHeadAnimator.headParentXform.TransformPoint(this.eyeAndHeadAnimator.GetOwnEyeCenter() + 10f * (Quaternion.Euler(0f, (float)((!flag) ? -50 : 50), 0f) * Vector3.forward));
			this.eyeAndHeadAnimator.LookAtAreaAround(point);
			this.nextChangePOITime = Time.time + UnityEngine.Random.Range(Mathf.Min(this.minLookTime, this.maxLookTime), Mathf.Max(this.minLookTime, this.maxLookTime));
			this.ChangeStateTo(LookTargetController.State.LookingAwayFromPlayer);
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0016E22C File Offset: 0x0016C62C
		private void OnDestroy()
		{
			if (this.createdVRParentGO != null)
			{
				DestroyNotifier component = this.createdVRParentGO.GetComponent<DestroyNotifier>();
				if (component != null)
				{
					component.OnDestroyedEvent -= this.OnPlayerEyesParentDestroyed;
				}
				UnityEngine.Object.Destroy(this.createdVRParentGO);
			}
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0016E290 File Offset: 0x0016C690
		private void OnPlayerEyesParentDestroyed(DestroyNotifier destroyNotifier)
		{
			if (destroyNotifier.gameObject != this.createdVRParentGO)
			{
				Debug.LogWarning("Received OnPlayerEyesParentDestroyed from unknown gameObject " + destroyNotifier, destroyNotifier.gameObject);
				return;
			}
			this.createdVRParentGO = null;
			this.createdPlayerEyeCenterGO = null;
			this.createdPlayerLeftEyeGO = null;
			this.createdPlayerRightEyeGO = null;
			this.isInitialized = false;
			this.Initialize();
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0016E2F3 File Offset: 0x0016C6F3
		private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			this.isInitialized = false;
			this.Initialize();
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0016E304 File Offset: 0x0016C704
		private void OnTargetLost()
		{
			if (this.eyeAndHeadAnimator.CanChangePointOfAttention())
			{
				float value = UnityEngine.Random.value;
				if (value <= this.lookAtPlayerRatio && this.IsPlayerInView())
				{
					this.LookAtPlayer(UnityEngine.Random.Range(Mathf.Min(this.minLookTime, this.maxLookTime), Mathf.Max(this.minLookTime, this.maxLookTime)), 0.075f);
				}
				else
				{
					this.LookAroundIdly();
				}
			}
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0016E37B File Offset: 0x0016C77B
		private void Start()
		{
			if (!this.isInitialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0016E390 File Offset: 0x0016C790
		private void Update()
		{
			if (!this.isInitialized)
			{
				return;
			}
			if (!this.keepTargetEvenWhenLost && !this.hasRegisteredTargetLostFunction)
			{
				this.eyeAndHeadAnimator.OnTargetLost += this.OnTargetLost;
				this.hasRegisteredTargetLostFunction = true;
			}
			else if (this.keepTargetEvenWhenLost && this.hasRegisteredTargetLostFunction)
			{
				this.eyeAndHeadAnimator.OnTargetLost -= this.OnTargetLost;
				this.hasRegisteredTargetLostFunction = false;
			}
			this.UpdateCameraTransformIfNecessary();
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0016E41C File Offset: 0x0016C81C
		private void UpdateCameraTransformIfNecessary()
		{
			if (!this.useVR && this.playerEyeCenterXform != null)
			{
				this.playerEyeCenterXform = ((!(this.playerEyeCenter != null)) ? Camera.main.transform : this.playerEyeCenter);
			}
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0016E474 File Offset: 0x0016C874
		private void UpdateNativeVRTransforms()
		{
			if (this.useNativeVRSupport && this.usedThirdPersonPlayerEyeCenter == null)
			{
				this.playerEyeCenterXform.localPosition = InputTracking.GetLocalPosition(VRNode.CenterEye);
				this.playerLeftEyeXform.localPosition = InputTracking.GetLocalPosition(VRNode.LeftEye);
				this.playerRightEyeXform.localPosition = InputTracking.GetLocalPosition(VRNode.RightEye);
				this.playerEyeCenterXform.localRotation = InputTracking.GetLocalRotation(VRNode.CenterEye);
				this.playerLeftEyeXform.localRotation = InputTracking.GetLocalRotation(VRNode.LeftEye);
				this.playerRightEyeXform.localRotation = InputTracking.GetLocalRotation(VRNode.RightEye);
			}
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x0016E504 File Offset: 0x0016C904
		private void UpdateThirdPersonPlayerEyeCenter()
		{
			if (this.playerEyeCenter == this.usedThirdPersonPlayerEyeCenter)
			{
				return;
			}
			if (this.playerEyeCenter != null)
			{
				if (Utils.IsEqualOrDescendant(base.transform, this.playerEyeCenter))
				{
					Debug.LogError("Player Eye Center should be part of the player character who this character is supposed to look at, not part of this character itself!");
				}
				this.playerEyeCenterXform = this.playerEyeCenter;
				this.playerLeftEyeXform = (this.playerRightEyeXform = null);
			}
			else if (this.useNativeVRSupport)
			{
				this.playerEyeCenterXform = this.createdPlayerEyeCenterGO.transform;
				this.playerLeftEyeXform = this.createdPlayerLeftEyeGO.transform;
				this.playerRightEyeXform = this.createdPlayerRightEyeGO.transform;
			}
			else if (this.useVR)
			{
				GameObject gameObject = GameObject.Find("OVRCameraRig");
				if (gameObject != null)
				{
					this.playerLeftEyeXform = Utils.FindChildInHierarchy(gameObject, "LeftEyeAnchor").transform;
					this.playerRightEyeXform = Utils.FindChildInHierarchy(gameObject, "RightEyeAnchor").transform;
					this.playerEyeCenterXform = Utils.FindChildInHierarchy(gameObject, "CenterEyeAnchor").transform;
				}
				else
				{
					this.playerEyeCenterXform = Camera.main.transform;
					this.playerLeftEyeXform = (this.playerRightEyeXform = null);
				}
			}
			else
			{
				this.playerEyeCenterXform = Camera.main.transform;
				this.playerLeftEyeXform = (this.playerRightEyeXform = null);
			}
			this.usedThirdPersonPlayerEyeCenter = this.playerEyeCenter;
		}

		// Token: 0x04002E27 RID: 11815
		[Tooltip("Drag objects here for the actor to look at. If empty, actor will look in random directions.")]
		public Transform[] pointsOfInterest;

		// Token: 0x04002E28 RID: 11816
		[Tooltip("Ratio of how often to look at player vs elsewhere. 0: never, 1: always")]
		[Range(0f, 1f)]
		public float lookAtPlayerRatio = 0.1f;

		// Token: 0x04002E29 RID: 11817
		[Tooltip("How likely the actor is to look back at the player when player stares at actor.")]
		[Range(0f, 1f)]
		public float stareBackFactor;

		// Token: 0x04002E2A RID: 11818
		[Tooltip("If player is closer than this, notice him")]
		[Range(0f, 100f)]
		public float noticePlayerDistance;

		// Token: 0x04002E2B RID: 11819
		[Tooltip("If player is closer than this, look away (overrides noticing him)")]
		[Range(0f, 4f)]
		public float personalSpaceDistance;

		// Token: 0x04002E2C RID: 11820
		[Tooltip("Minimum time to look at a target")]
		[Range(1f, 100f)]
		public float minLookTime = 3f;

		// Token: 0x04002E2D RID: 11821
		[Tooltip("Maximum time to look at a target")]
		[Range(1f, 100f)]
		public float maxLookTime = 10f;

		// Token: 0x04002E2E RID: 11822
		[Tooltip("For 3rd person games, set this to the player's eye center transform")]
		public Transform playerEyeCenter;

		// Token: 0x04002E2F RID: 11823
		[Tooltip("Keep trying to track target even when it moves out of sight")]
		public bool keepTargetEvenWhenLost = true;

		// Token: 0x04002E30 RID: 11824
		[Header("Events")]
		public UnityEvent OnStartLookingAtPlayer;

		// Token: 0x04002E31 RID: 11825
		public UnityEvent OnStopLookingAtPlayer;

		// Token: 0x04002E32 RID: 11826
		private EyeAndHeadAnimator eyeAndHeadAnimator;

		// Token: 0x04002E33 RID: 11827
		private const float minLookAtMeTimeToReact = 4f;

		// Token: 0x04002E34 RID: 11828
		private Transform targetPOI;

		// Token: 0x04002E35 RID: 11829
		private Transform playerEyeCenterXform;

		// Token: 0x04002E36 RID: 11830
		private Transform playerLeftEyeXform;

		// Token: 0x04002E37 RID: 11831
		private Transform playerRightEyeXform;

		// Token: 0x04002E38 RID: 11832
		private Transform usedThirdPersonPlayerEyeCenter;

		// Token: 0x04002E39 RID: 11833
		private GameObject createdVRParentGO;

		// Token: 0x04002E3A RID: 11834
		private GameObject createdPlayerEyeCenterGO;

		// Token: 0x04002E3B RID: 11835
		private GameObject createdPlayerLeftEyeGO;

		// Token: 0x04002E3C RID: 11836
		private GameObject createdPlayerRightEyeGO;

		// Token: 0x04002E3D RID: 11837
		private float lastDistanceToPlayer = -1f;

		// Token: 0x04002E3E RID: 11838
		private float playerLookingAtMeTime;

		// Token: 0x04002E3F RID: 11839
		private float nextChangePOITime;

		// Token: 0x04002E40 RID: 11840
		private float stareBackDeadtime;

		// Token: 0x04002E41 RID: 11841
		private float timeOfLastNoticeCheck = -1000f;

		// Token: 0x04002E42 RID: 11842
		private float timeOfLastLookBackCheck = -1000f;

		// Token: 0x04002E43 RID: 11843
		private float timeOutsideOfAwarenessZone = 1000f;

		// Token: 0x04002E44 RID: 11844
		private float timeInsidePersonalSpace;

		// Token: 0x04002E45 RID: 11845
		private bool useNativeVRSupport;

		// Token: 0x04002E46 RID: 11846
		private bool useVR;

		// Token: 0x04002E47 RID: 11847
		private bool isInitialized;

		// Token: 0x04002E48 RID: 11848
		private bool hasRegisteredTargetLostFunction;

		// Token: 0x04002E49 RID: 11849
		private LookTargetController.State state = LookTargetController.State.LookingAroundIdly;

		// Token: 0x020008C0 RID: 2240
		private enum State
		{
			// Token: 0x04002E4B RID: 11851
			LookingAtPlayer,
			// Token: 0x04002E4C RID: 11852
			LookingAroundIdly,
			// Token: 0x04002E4D RID: 11853
			LookingAtPoiDirectly,
			// Token: 0x04002E4E RID: 11854
			LookingAwayFromPlayer
		}
	}
}
