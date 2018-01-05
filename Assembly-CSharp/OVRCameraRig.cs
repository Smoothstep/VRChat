using System;
using UnityEngine;
using UnityEngine.VR;

// Token: 0x0200066E RID: 1646
[ExecuteInEditMode]
public class OVRCameraRig : MonoBehaviour
{
	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x0600379F RID: 14239 RVA: 0x0011BC8B File Offset: 0x0011A08B
	public Camera leftEyeCamera
	{
		get
		{
			return (!this.usePerEyeCameras) ? this._centerEyeCamera : this._leftEyeCamera;
		}
	}

	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x060037A0 RID: 14240 RVA: 0x0011BCA9 File Offset: 0x0011A0A9
	public Camera rightEyeCamera
	{
		get
		{
			return (!this.usePerEyeCameras) ? this._centerEyeCamera : this._rightEyeCamera;
		}
	}

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x060037A1 RID: 14241 RVA: 0x0011BCC7 File Offset: 0x0011A0C7
	// (set) Token: 0x060037A2 RID: 14242 RVA: 0x0011BCCF File Offset: 0x0011A0CF
	public Transform trackingSpace { get; private set; }

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x060037A3 RID: 14243 RVA: 0x0011BCD8 File Offset: 0x0011A0D8
	// (set) Token: 0x060037A4 RID: 14244 RVA: 0x0011BCE0 File Offset: 0x0011A0E0
	public Transform leftEyeAnchor { get; private set; }

	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x060037A5 RID: 14245 RVA: 0x0011BCE9 File Offset: 0x0011A0E9
	// (set) Token: 0x060037A6 RID: 14246 RVA: 0x0011BCF1 File Offset: 0x0011A0F1
	public Transform centerEyeAnchor { get; private set; }

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x060037A7 RID: 14247 RVA: 0x0011BCFA File Offset: 0x0011A0FA
	// (set) Token: 0x060037A8 RID: 14248 RVA: 0x0011BD02 File Offset: 0x0011A102
	public Transform rightEyeAnchor { get; private set; }

	// Token: 0x170008C4 RID: 2244
	// (get) Token: 0x060037A9 RID: 14249 RVA: 0x0011BD0B File Offset: 0x0011A10B
	// (set) Token: 0x060037AA RID: 14250 RVA: 0x0011BD13 File Offset: 0x0011A113
	public Transform leftHandAnchor { get; private set; }

	// Token: 0x170008C5 RID: 2245
	// (get) Token: 0x060037AB RID: 14251 RVA: 0x0011BD1C File Offset: 0x0011A11C
	// (set) Token: 0x060037AC RID: 14252 RVA: 0x0011BD24 File Offset: 0x0011A124
	public Transform rightHandAnchor { get; private set; }

	// Token: 0x170008C6 RID: 2246
	// (get) Token: 0x060037AD RID: 14253 RVA: 0x0011BD2D File Offset: 0x0011A12D
	// (set) Token: 0x060037AE RID: 14254 RVA: 0x0011BD35 File Offset: 0x0011A135
	public Transform trackerAnchor { get; private set; }

	// Token: 0x1400003A RID: 58
	// (add) Token: 0x060037AF RID: 14255 RVA: 0x0011BD40 File Offset: 0x0011A140
	// (remove) Token: 0x060037B0 RID: 14256 RVA: 0x0011BD78 File Offset: 0x0011A178
	public event Action<OVRCameraRig> UpdatedAnchors;

	// Token: 0x060037B1 RID: 14257 RVA: 0x0011BDAE File Offset: 0x0011A1AE
	private void Awake()
	{
		this.EnsureGameObjectIntegrity();
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x0011BDB6 File Offset: 0x0011A1B6
	private void Start()
	{
		this.EnsureGameObjectIntegrity();
		if (!Application.isPlaying)
		{
			return;
		}
		this.UpdateAnchors();
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x0011BDCF File Offset: 0x0011A1CF
	private void Update()
	{
		this.EnsureGameObjectIntegrity();
		if (!Application.isPlaying)
		{
			return;
		}
		this.UpdateAnchors();
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x0011BDE8 File Offset: 0x0011A1E8
	private void UpdateAnchors()
	{
		bool monoscopic = OVRManager.instance.monoscopic;
		OVRPose pose = OVRManager.tracker.GetPose(0);
		this.trackerAnchor.localRotation = pose.orientation;
		this.leftEyeAnchor.localRotation = ((!monoscopic) ? InputTracking.GetLocalRotation(VRNode.LeftEye) : this.centerEyeAnchor.localRotation);
		this.rightEyeAnchor.localRotation = ((!monoscopic) ? InputTracking.GetLocalRotation(VRNode.RightEye) : this.centerEyeAnchor.localRotation);
		this.leftHandAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
		this.rightHandAnchor.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
		this.trackerAnchor.localPosition = pose.position;
		this.leftEyeAnchor.localPosition = ((!monoscopic) ? InputTracking.GetLocalPosition(VRNode.LeftEye) : this.centerEyeAnchor.localPosition);
		this.rightEyeAnchor.localPosition = ((!monoscopic) ? InputTracking.GetLocalPosition(VRNode.RightEye) : this.centerEyeAnchor.localPosition);
		this.leftHandAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
		this.rightHandAnchor.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
		if (this.UpdatedAnchors != null)
		{
			this.UpdatedAnchors(this);
		}
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x0011BF28 File Offset: 0x0011A328
	public void EnsureGameObjectIntegrity()
	{
		if (this.trackingSpace == null)
		{
			this.trackingSpace = this.ConfigureRootAnchor(this.trackingSpaceName);
		}
		if (this.leftEyeAnchor == null)
		{
			this.leftEyeAnchor = this.ConfigureEyeAnchor(this.trackingSpace, VRNode.LeftEye);
		}
		if (this.centerEyeAnchor == null)
		{
			this.centerEyeAnchor = this.ConfigureEyeAnchor(this.trackingSpace, VRNode.CenterEye);
		}
		if (this.rightEyeAnchor == null)
		{
			this.rightEyeAnchor = this.ConfigureEyeAnchor(this.trackingSpace, VRNode.RightEye);
		}
		if (this.leftHandAnchor == null)
		{
			this.leftHandAnchor = this.ConfigureHandAnchor(this.trackingSpace, OVRPlugin.Node.HandLeft);
		}
		if (this.rightHandAnchor == null)
		{
			this.rightHandAnchor = this.ConfigureHandAnchor(this.trackingSpace, OVRPlugin.Node.HandRight);
		}
		if (this.trackerAnchor == null)
		{
			this.trackerAnchor = this.ConfigureTrackerAnchor(this.trackingSpace);
		}
		if (this._centerEyeCamera == null || this._leftEyeCamera == null || this._rightEyeCamera == null)
		{
			this._centerEyeCamera = this.centerEyeAnchor.GetComponent<Camera>();
			this._leftEyeCamera = this.leftEyeAnchor.GetComponent<Camera>();
			this._rightEyeCamera = this.rightEyeAnchor.GetComponent<Camera>();
			if (this._centerEyeCamera == null)
			{
				this._centerEyeCamera = this.centerEyeAnchor.gameObject.AddComponent<Camera>();
				this._centerEyeCamera.tag = "MainCamera";
			}
			if (this._leftEyeCamera == null)
			{
				this._leftEyeCamera = this.leftEyeAnchor.gameObject.AddComponent<Camera>();
				this._leftEyeCamera.tag = "MainCamera";
			}
			if (this._rightEyeCamera == null)
			{
				this._rightEyeCamera = this.rightEyeAnchor.gameObject.AddComponent<Camera>();
				this._rightEyeCamera.tag = "MainCamera";
			}
			this._centerEyeCamera.stereoTargetEye = StereoTargetEyeMask.Both;
			this._leftEyeCamera.stereoTargetEye = StereoTargetEyeMask.Left;
			this._rightEyeCamera.stereoTargetEye = StereoTargetEyeMask.Right;
		}
		this._centerEyeCamera.enabled = !this.usePerEyeCameras;
		this._leftEyeCamera.enabled = this.usePerEyeCameras;
		this._rightEyeCamera.enabled = this.usePerEyeCameras;
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x0011C194 File Offset: 0x0011A594
	private Transform ConfigureRootAnchor(string name)
	{
		Transform transform = base.transform.Find(name);
		if (transform == null)
		{
			transform = new GameObject(name).transform;
		}
		transform.parent = base.transform;
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		return transform;
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x0011C1F4 File Offset: 0x0011A5F4
	private Transform ConfigureEyeAnchor(Transform root, VRNode eye)
	{
		string str = (eye != VRNode.CenterEye) ? ((eye != VRNode.LeftEye) ? "Right" : "Left") : "Center";
		string text = str + this.eyeAnchorName;
		Transform transform = base.transform.Find(root.name + "/" + text);
		if (transform == null)
		{
			transform = base.transform.Find(text);
		}
		if (transform == null)
		{
			string name = this.legacyEyeAnchorName + eye.ToString();
			transform = base.transform.Find(name);
		}
		if (transform == null)
		{
			transform = new GameObject(text).transform;
		}
		transform.name = text;
		transform.parent = root;
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		return transform;
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x0011C2E8 File Offset: 0x0011A6E8
	private Transform ConfigureHandAnchor(Transform root, OVRPlugin.Node hand)
	{
		string str = (hand != OVRPlugin.Node.HandLeft) ? "Right" : "Left";
		string text = str + this.handAnchorName;
		Transform transform = base.transform.Find(root.name + "/" + text);
		if (transform == null)
		{
			transform = base.transform.Find(text);
		}
		if (transform == null)
		{
			transform = new GameObject(text).transform;
		}
		transform.name = text;
		transform.parent = root;
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		return transform;
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x0011C398 File Offset: 0x0011A798
	private Transform ConfigureTrackerAnchor(Transform root)
	{
		string text = this.trackerAnchorName;
		Transform transform = base.transform.Find(root.name + "/" + text);
		if (transform == null)
		{
			transform = new GameObject(text).transform;
		}
		transform.parent = root;
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		return transform;
	}

	// Token: 0x0400204B RID: 8267
	public bool usePerEyeCameras;

	// Token: 0x0400204C RID: 8268
	private readonly string trackingSpaceName = "TrackingSpace";

	// Token: 0x0400204D RID: 8269
	private readonly string trackerAnchorName = "TrackerAnchor";

	// Token: 0x0400204E RID: 8270
	private readonly string eyeAnchorName = "EyeAnchor";

	// Token: 0x0400204F RID: 8271
	private readonly string handAnchorName = "HandAnchor";

	// Token: 0x04002050 RID: 8272
	private readonly string legacyEyeAnchorName = "Camera";

	// Token: 0x04002051 RID: 8273
	private Camera _centerEyeCamera;

	// Token: 0x04002052 RID: 8274
	private Camera _leftEyeCamera;

	// Token: 0x04002053 RID: 8275
	private Camera _rightEyeCamera;
}
