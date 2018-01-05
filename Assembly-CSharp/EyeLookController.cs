using System;
using RealisticEyeMovements;
using UnityEngine;

// Token: 0x02000A34 RID: 2612
public class EyeLookController : MonoBehaviour
{
	// Token: 0x06004E98 RID: 20120 RVA: 0x001A5D10 File Offset: 0x001A4110
	private void Awake()
	{
		int num = 9;
		this._settings = new string[num];
		this._settings[0] = "VrcSkel.dat";
		this._settings[1] = "VrcTemplate01.dat";
		this._settings[2] = "VrcToon01.dat";
		this._settings[3] = "VrcToon02.dat";
		this._settings[4] = "Mixamo.dat";
		this._settings[5] = "Fuse.dat";
		this._settings[6] = "AutodeskBlend.dat";
		this._settings[7] = "MorphNikeiM.dat";
		this._settings[8] = "MorphNikeiF.dat";
		foreach (string str in this._settings)
		{
			EyeAndHeadAnimator.Cache(Application.streamingAssetsPath + "/RealisticEyeMovements/Presets/" + str);
		}
	}

	// Token: 0x06004E99 RID: 20121 RVA: 0x001A5DD8 File Offset: 0x001A41D8
	public bool Initialize(Animator anim, bool useFinalIK)
	{
		this._anim = anim;
		this._waitTime = 0f;
		this._eyeControl = this._anim.gameObject.AddComponent<EyeAndHeadAnimator>();
		this._avatarType = EyeLookController.AvatarType.LAST_UNKNOWN;
		for (int i = 0; i < 9; i++)
		{
			string filename = Application.streamingAssetsPath + "/RealisticEyeMovements/Presets/" + this._settings[i];
			if (this._eyeControl.ImportFromFile(filename))
			{
				this._eyeControl.Initialize(useFinalIK);
				this._avatarType = (EyeLookController.AvatarType)i;
			}
		}
		if (this._avatarType == EyeLookController.AvatarType.LAST_UNKNOWN)
		{
			this.ClearComponents();
			return false;
		}
		this._needLookTarget = true;
		this._eyeControl.headWeight = 0f;
		this._inited = true;
		return true;
	}

	// Token: 0x06004E9A RID: 20122 RVA: 0x001A5E98 File Offset: 0x001A4298
	public void ClearComponents()
	{
		if (this._eyeControl != null)
		{
			UnityEngine.Object.Destroy(this._eyeControl);
			this._eyeControl = null;
		}
		if (this._lookControl != null)
		{
			UnityEngine.Object.Destroy(this._lookControl);
			this._lookControl = null;
		}
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x001A5EEC File Offset: 0x001A42EC
	private void SaveEyeDefaults()
	{
		this._defaultMinBlinkTime = this._eyeControl.kMinNextBlinkTime;
		this._defaultMaxBlinkTime = this._eyeControl.kMaxNextBlinkTime;
		this._defaultNervousness = this._eyeControl.nervousness;
		this._defaultUseMicroSaccades = this._eyeControl.useMicroSaccades;
		this._defaultUseMacroSaccades = this._eyeControl.useMacroSaccades;
	}

	// Token: 0x06004E9C RID: 20124 RVA: 0x001A5F50 File Offset: 0x001A4350
	private void RestoreEyeDefaults()
	{
		this._eyeControl.kMinNextBlinkTime = this._defaultMinBlinkTime;
		this._eyeControl.kMaxNextBlinkTime = this._defaultMaxBlinkTime;
		this._eyeControl.nervousness = this._defaultNervousness;
		this._eyeControl.useMicroSaccades = this._defaultUseMicroSaccades;
		this._eyeControl.useMacroSaccades = this._defaultUseMacroSaccades;
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x001A5FB4 File Offset: 0x001A43B4
	private void InitializeLookTarget()
	{
		if (this._anim == null)
		{
			return;
		}
		this._lookControl = this._anim.gameObject.AddComponent<LookTargetController>();
		if (this._lookControl == null)
		{
			return;
		}
		this._lookControl.lookAtPlayerRatio = 0.75f;
		this._lookControl.stareBackFactor = 1f;
		this._lookControl.noticePlayerDistance = 3f;
		this._lookControl.personalSpaceDistance = 0f;
		this._lookControl.minLookTime = 1f;
		this._lookControl.maxLookTime = 5f;
		this._lookControl.keepTargetEvenWhenLost = false;
		this.SaveEyeDefaults();
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x001A6070 File Offset: 0x001A4470
	public void SetDefaultMode()
	{
		if (this._lookControl != null)
		{
			this._lookControl.lookAtPlayerRatio = 0.75f;
			this._lookControl.stareBackFactor = 1f;
			this._lookControl.noticePlayerDistance = 3f;
			this._lookControl.personalSpaceDistance = 0f;
			this._lookControl.minLookTime = 1f;
			this._lookControl.maxLookTime = 5f;
			this._lookControl.keepTargetEvenWhenLost = false;
			this._lookControl.pointsOfInterest = null;
		}
		this.RestoreEyeDefaults();
	}

	// Token: 0x06004E9F RID: 20127 RVA: 0x001A610C File Offset: 0x001A450C
	public void SetPhotoMode(Transform target)
	{
		if (this._lookControl != null)
		{
			this._lookControl.lookAtPlayerRatio = 0f;
			this._lookControl.stareBackFactor = 0f;
			this._lookControl.noticePlayerDistance = 0f;
			this._lookControl.personalSpaceDistance = 0f;
			this._lookControl.minLookTime = 10000f;
			this._lookControl.maxLookTime = 10000f;
			this._lookControl.keepTargetEvenWhenLost = true;
			this._lookControl.pointsOfInterest = new Transform[1];
			this._lookControl.pointsOfInterest[0] = target;
		}
		if (this._eyeControl != null)
		{
			this._eyeControl.kMinNextBlinkTime = 10000f;
			this._eyeControl.kMaxNextBlinkTime = 10000f;
			this._eyeControl.nervousness = 0f;
			this._eyeControl.useMicroSaccades = false;
			this._eyeControl.useMacroSaccades = false;
		}
	}

	// Token: 0x06004EA0 RID: 20128 RVA: 0x001A620E File Offset: 0x001A460E
	public void UnInitialize()
	{
		this._inited = false;
		if (this._eyeControl != null)
		{
			this._eyeControl.UnInitialize();
		}
		this.ClearComponents();
	}

	// Token: 0x06004EA1 RID: 20129 RVA: 0x001A6239 File Offset: 0x001A4639
	private void Start()
	{
	}

	// Token: 0x06004EA2 RID: 20130 RVA: 0x001A623C File Offset: 0x001A463C
	private void Update()
	{
		if (!this._inited)
		{
			return;
		}
		if (this._needLookTarget)
		{
			this._waitTime += Time.deltaTime;
			if (this._waitTime > this._lookTargetWait)
			{
				this.InitializeLookTarget();
				this._needLookTarget = false;
			}
		}
	}

	// Token: 0x040036C9 RID: 14025
	private Animator _anim;

	// Token: 0x040036CA RID: 14026
	private EyeAndHeadAnimator _eyeControl;

	// Token: 0x040036CB RID: 14027
	private LookTargetController _lookControl;

	// Token: 0x040036CC RID: 14028
	private EyeLookController.AvatarType _avatarType;

	// Token: 0x040036CD RID: 14029
	private string[] _settings;

	// Token: 0x040036CE RID: 14030
	private bool _needLookTarget;

	// Token: 0x040036CF RID: 14031
	private float _lookTargetWait = 1f;

	// Token: 0x040036D0 RID: 14032
	private float _waitTime;

	// Token: 0x040036D1 RID: 14033
	private bool _inited;

	// Token: 0x040036D2 RID: 14034
	private float _defaultMinBlinkTime;

	// Token: 0x040036D3 RID: 14035
	private float _defaultMaxBlinkTime;

	// Token: 0x040036D4 RID: 14036
	private float _defaultNervousness;

	// Token: 0x040036D5 RID: 14037
	private bool _defaultUseMicroSaccades;

	// Token: 0x040036D6 RID: 14038
	private bool _defaultUseMacroSaccades;

	// Token: 0x02000A35 RID: 2613
	private enum AvatarType
	{
		// Token: 0x040036D8 RID: 14040
		VRC_SKEL,
		// Token: 0x040036D9 RID: 14041
		VRC_TEMPLATE_01,
		// Token: 0x040036DA RID: 14042
		VRC_TOON_01,
		// Token: 0x040036DB RID: 14043
		VRC_TOON_02,
		// Token: 0x040036DC RID: 14044
		MIXAMO,
		// Token: 0x040036DD RID: 14045
		FUSE,
		// Token: 0x040036DE RID: 14046
		AUTODESK_BLENDSHAPES,
		// Token: 0x040036DF RID: 14047
		MORPH3D_NIKEI_M,
		// Token: 0x040036E0 RID: 14048
		MORPH3D_NIKEI_F,
		// Token: 0x040036E1 RID: 14049
		LAST_UNKNOWN
	}
}
