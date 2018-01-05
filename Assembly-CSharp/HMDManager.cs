using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000A92 RID: 2706
public class HMDManager : MonoBehaviour
{
	// Token: 0x17000BFF RID: 3071
	// (get) Token: 0x06005172 RID: 20850 RVA: 0x001BE63A File Offset: 0x001BCA3A
	public static string strFPS
	{
		get
		{
			return HMDManager._strFPS;
		}
	}

	// Token: 0x06005173 RID: 20851 RVA: 0x001BE641 File Offset: 0x001BCA41
	private void Awake()
	{
		if (HMDManager.Instance == null)
		{
			HMDManager.Instance = this;
		}
		else
		{
			Debug.LogWarning("HMD Manager Already Exists!");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.hmd = HMDManager.HMDType.Steam;
	}

	// Token: 0x06005174 RID: 20852 RVA: 0x001BE67A File Offset: 0x001BCA7A
	private void Start()
	{
		this.inReset = VRCInputManager.FindInput("Reset Orientation");
		this.inToggleSitStand = VRCInputManager.FindInput("ToggleSitStand");
	}

	// Token: 0x06005175 RID: 20853 RVA: 0x001BE69C File Offset: 0x001BCA9C
	private void Update()
	{
		if (this.inReset == null)
		{
			return;
		}
		if (this.inReset.down && !UICamera.inputHasFocus)
		{
			Debug.Log("Resetting");
			VRCTrackingManager.ResetHMDOrientation();
		}
		if (this.inToggleSitStand.down && !UICamera.inputHasFocus)
		{
			Debug.Log("Toggle Seated Play");
			VRCTrackingManager.ToggleSeatedPlay();
		}
		this.UpdateFPS();
		this.AttemptToSendHmdAnalytic();
	}

	// Token: 0x06005176 RID: 20854 RVA: 0x001BE714 File Offset: 0x001BCB14
	private void UpdateFPS()
	{
		HMDManager._TimeLeft -= Time.deltaTime;
		HMDManager._Accum += Time.timeScale / Time.deltaTime;
		HMDManager._Frames++;
		if ((double)HMDManager._TimeLeft <= 0.0)
		{
			float num = HMDManager._Accum / (float)HMDManager._Frames;
			HMDManager._strFPS = string.Format("FPS: {0:F2}", num);
			HMDManager._TimeLeft += HMDManager._UpdateInterval;
			HMDManager._Accum = 0f;
			HMDManager._Frames = 0;
			if (num < 35f)
			{
				if (!this.isAtVeryLowFramerate)
				{
					this.isAtVeryLowFramerate = true;
					this.veryLowFramerateTimeStarted = Time.unscaledTime;
				}
			}
			else
			{
				this.isAtVeryLowFramerate = false;
			}
			if (this.isAtVeryLowFramerate && Time.unscaledTime - this.veryLowFramerateTimeStarted > 3f)
			{
				VRCLog.UploadMiniLog("Prolonged framerate less than " + 35f + " FPS!");
			}
		}
	}

	// Token: 0x06005177 RID: 20855 RVA: 0x001BE81D File Offset: 0x001BCC1D
	public static Vector3 GetEyePivotOffset()
	{
		if (HMDManager.Instance != null)
		{
			return HMDManager.Instance.eyePivotOffset;
		}
		return new Vector3(0f, 0f, 0f);
	}

	// Token: 0x06005178 RID: 20856 RVA: 0x001BE850 File Offset: 0x001BCC50
	public static GameObject InstantiateCameraPrefab()
	{
		if (HMDManager.Instance == null)
		{
			return null;
		}
		GameObject gameObject;
		switch (HMDManager.Instance.hmd)
		{
		case HMDManager.HMDType.Unity:
			gameObject = HMDManager.Instance.UnityCameraRigPrefab;
			break;
		case HMDManager.HMDType.Oculus:
			gameObject = HMDManager.Instance.OculusCameraRigPrefab;
			break;
		case HMDManager.HMDType.Steam:
			gameObject = HMDManager.Instance.SteamVRCameraRigPrefab;
			break;
		case HMDManager.HMDType.Gameface:
			gameObject = HMDManager.Instance.CardboardCameraRigPrefab;
			break;
		case HMDManager.HMDType.Cardboard:
			gameObject = HMDManager.Instance.CardboardCameraRigPrefab;
			break;
		default:
			gameObject = null;
			break;
		}
		GameObject result = null;
		if (gameObject != null)
		{
			result = (AssetManagement.Instantiate(gameObject) as GameObject);
		}
		return result;
	}

	// Token: 0x06005179 RID: 20857 RVA: 0x001BE90D File Offset: 0x001BCD0D
	public static HMDManager.HMDType GetHmdType()
	{
		if (HMDManager.Instance == null)
		{
			return HMDManager.HMDType.Uninitialized;
		}
		return HMDManager.Instance.hmd;
	}

	// Token: 0x0600517A RID: 20858 RVA: 0x001BE92C File Offset: 0x001BCD2C
	private void AttemptToSendHmdAnalytic()
	{
		if (VRCInputManager.GetLastUsedInputMethod() != VRCInputMethod.Count)
		{
			if (!this.hasSentLoggedInHmdAnalytics && APIUser.IsLoggedIn)
			{
				this.SendHmdAnalytic();
				this.hasSentLoggedInHmdAnalytics = true;
				this.hasSentAnonymousHmdAnalytics = true;
			}
			if (!this.hasSentAnonymousHmdAnalytics && VRCFlowManager.Instance.HasAttemptedCachedLogin)
			{
				this.SendHmdAnalytic();
				this.hasSentAnonymousHmdAnalytics = true;
			}
		}
	}

	// Token: 0x0600517B RID: 20859 RVA: 0x001BE994 File Offset: 0x001BCD94
	private void SendHmdAnalytic()
	{
		string analyticName = "hmdType";
		string value = "none";
		string value2 = "none";
		string value3 = "steam";
		if (HMDManager.IsHmdDetected())
		{
			if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
			{
				value = "oculus";
			}
			else
			{
				value = "vive";
			}
		}
		switch (VRCInputManager.GetLastUsedInputMethod())
		{
		case VRCInputMethod.Keyboard:
			value2 = "keyboard";
			break;
		case VRCInputMethod.Mouse:
			value2 = "keyboard";
			break;
		case VRCInputMethod.Controller:
			value2 = "controller";
			break;
		case VRCInputMethod.Vive:
			value2 = "hand";
			break;
		case VRCInputMethod.Oculus:
			value2 = "hand";
			break;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["deviceId"] = ApiModel.DeviceID;
		dictionary["buildType"] = value3;
		dictionary["hmdType"] = value;
		dictionary["inputType"] = value2;
		dictionary["platform"] = Application.platform.ToString();
		dictionary["store"] = VRCApplicationSetup.GetBuildStoreID();
		dictionary["buildVersion"] = VRCApplicationSetup.GetBuildVersionString();
		Debug.Log("Sending deviceInfo analytic: " + string.Join(", ", (from kvp in dictionary
		select kvp.Key + " = " + kvp.Value).ToArray<string>()));
		base.StartCoroutine(this.SendUnityHmdAnalytic(analyticName, dictionary));
		global::Analytics.Send(ApiAnalyticEvent.EventType.deviceInfo, dictionary, null, null);
	}

	// Token: 0x0600517C RID: 20860 RVA: 0x001BEB40 File Offset: 0x001BCF40
	private IEnumerator SendUnityHmdAnalytic(string analyticName, Dictionary<string, object> analyticDict)
	{
		while (!APIUser.IsLoggedIn)
		{
			yield return null;
		}
		string userId = User.CurrentUser.id;
		if (string.IsNullOrEmpty(userId))
		{
			Debug.LogError("SendUnityHmdAnalytic: logged in but userId was null!");
			yield break;
		}
		UnityEngine.Analytics.Analytics.SetUserId(userId);
		Debug.Log("SendUnityHmdAnalytic: result: " + UnityEngine.Analytics.Analytics.CustomEvent(analyticName, analyticDict).ToString());
		yield break;
	}

	// Token: 0x0600517D RID: 20861 RVA: 0x001BEB62 File Offset: 0x001BCF62
	public static bool IsHmdDetected()
	{
		return SteamVR.instance != null;
	}

	// Token: 0x040039C1 RID: 14785
	public GameObject UnityCameraRigPrefab;

	// Token: 0x040039C2 RID: 14786
	public GameObject OculusCameraRigPrefab;

	// Token: 0x040039C3 RID: 14787
	public GameObject SteamVRCameraRigPrefab;

	// Token: 0x040039C4 RID: 14788
	public GameObject CardboardCameraRigPrefab;

	// Token: 0x040039C5 RID: 14789
	private static float _UpdateInterval = 0.5f;

	// Token: 0x040039C6 RID: 14790
	private static float _Accum;

	// Token: 0x040039C7 RID: 14791
	private static int _Frames;

	// Token: 0x040039C8 RID: 14792
	private static float _TimeLeft;

	// Token: 0x040039C9 RID: 14793
	private static string _strFPS = "FPS: 0";

	// Token: 0x040039CA RID: 14794
	private const float VERY_LOW_FRAMERATE_WARNING_LEVEL = 35f;

	// Token: 0x040039CB RID: 14795
	private float veryLowFramerateTimeStarted;

	// Token: 0x040039CC RID: 14796
	private bool isAtVeryLowFramerate;

	// Token: 0x040039CD RID: 14797
	private bool hasSentLoggedInHmdAnalytics;

	// Token: 0x040039CE RID: 14798
	private bool hasSentAnonymousHmdAnalytics;

	// Token: 0x040039CF RID: 14799
	public HMDManager.HMDType hmd = HMDManager.HMDType.Uninitialized;

	// Token: 0x040039D0 RID: 14800
	public Vector3 eyePivotOffset = new Vector3(0f, 0.07f, 0.08f);

	// Token: 0x040039D1 RID: 14801
	private static HMDManager Instance;

	// Token: 0x040039D2 RID: 14802
	private VRCInput inReset;

	// Token: 0x040039D3 RID: 14803
	private VRCInput inToggleSitStand;

	// Token: 0x02000A93 RID: 2707
	public enum HMDType
	{
		// Token: 0x040039D6 RID: 14806
		Uninitialized = -1,
		// Token: 0x040039D7 RID: 14807
		Unity,
		// Token: 0x040039D8 RID: 14808
		Oculus,
		// Token: 0x040039D9 RID: 14809
		Steam,
		// Token: 0x040039DA RID: 14810
		Gameface,
		// Token: 0x040039DB RID: 14811
		Cardboard
	}
}
