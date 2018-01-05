using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000662 RID: 1634
public class OSPManager : MonoBehaviour
{
	// Token: 0x0600372C RID: 14124
	[DllImport("OculusSpatializerPlugin")]
	private static extern bool OSP_Init(int sample_rate, int buffer_size);

	// Token: 0x0600372D RID: 14125
	[DllImport("OculusSpatializerPlugin")]
	private static extern bool OSP_Exit();

	// Token: 0x0600372E RID: 14126
	[DllImport("OculusSpatializerPlugin")]
	private static extern bool OSP_UpdateRoomModel(ref OSPManager.RoomModel rm);

	// Token: 0x0600372F RID: 14127
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetReflectionsRangeMax(float range);

	// Token: 0x06003730 RID: 14128
	[DllImport("OculusSpatializerPlugin")]
	private static extern int OSP_AcquireContext(int audioSourceInstanceID);

	// Token: 0x06003731 RID: 14129
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_ReturnContext(int audioSourceInstanceID, int context);

	// Token: 0x06003732 RID: 14130
	[DllImport("OculusSpatializerPlugin")]
	private static extern bool OSP_WasSoundStolen(int audioSourceInstanceID);

	// Token: 0x06003733 RID: 14131
	[DllImport("OculusSpatializerPlugin")]
	private static extern bool OSP_GetBypass();

	// Token: 0x06003734 RID: 14132
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetBypass(bool bypass);

	// Token: 0x06003735 RID: 14133
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetGlobalScale(float globalScale);

	// Token: 0x06003736 RID: 14134
	[DllImport("OculusSpatializerPlugin")]
	private static extern bool OSP_GetUseInverseSquareAttenuation();

	// Token: 0x06003737 RID: 14135
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetUseInverseSquareAttenuation(bool useInvSq);

	// Token: 0x06003738 RID: 14136
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetFalloffRangeGlobal(float nearRange, float farRange);

	// Token: 0x06003739 RID: 14137
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetFalloffRangeLocal(int contextAndSound, float nearRange, float farRange);

	// Token: 0x0600373A RID: 14138
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetGain(float gain);

	// Token: 0x0600373B RID: 14139
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetDisableReflectionsOnSound(int contextAndSound, bool disable);

	// Token: 0x0600373C RID: 14140
	[DllImport("OculusSpatializerPlugin")]
	private static extern float OSP_GetDrainTime(int context);

	// Token: 0x0600373D RID: 14141
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_SetPositonRelXYZ(int context, float x, float y, float z);

	// Token: 0x0600373E RID: 14142
	[DllImport("OculusSpatializerPlugin")]
	private static extern void OSP_Spatialize(int context, float[] ioBuf, bool useInvSq, float near, float far);

	// Token: 0x0600373F RID: 14143
	[DllImport("OculusSpatializerPlugin")]
	private static extern int OSP_GetMaxNumSpatializedSounds();

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x06003740 RID: 14144 RVA: 0x0011ACB7 File Offset: 0x001190B7
	// (set) Token: 0x06003741 RID: 14145 RVA: 0x0011ACBF File Offset: 0x001190BF
	public int BufferSize
	{
		get
		{
			return this.bufferSize;
		}
		set
		{
			this.bufferSize = value;
		}
	}

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x06003742 RID: 14146 RVA: 0x0011ACC8 File Offset: 0x001190C8
	// (set) Token: 0x06003743 RID: 14147 RVA: 0x0011ACD0 File Offset: 0x001190D0
	public int SampleRate
	{
		get
		{
			return this.sampleRate;
		}
		set
		{
			this.sampleRate = value;
		}
	}

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x06003744 RID: 14148 RVA: 0x0011ACD9 File Offset: 0x001190D9
	// (set) Token: 0x06003745 RID: 14149 RVA: 0x0011ACE0 File Offset: 0x001190E0
	public bool Bypass
	{
		get
		{
			return OSPManager.OSP_GetBypass();
		}
		set
		{
			this.bypass = value;
			OSPManager.OSP_SetBypass(this.bypass);
		}
	}

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06003746 RID: 14150 RVA: 0x0011ACF4 File Offset: 0x001190F4
	// (set) Token: 0x06003747 RID: 14151 RVA: 0x0011ACFC File Offset: 0x001190FC
	public float GlobalScale
	{
		get
		{
			return this.globalScale;
		}
		set
		{
			this.globalScale = Mathf.Clamp(value, 1E-05f, 10000f);
			OSPManager.OSP_SetGlobalScale(this.globalScale);
		}
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06003748 RID: 14152 RVA: 0x0011AD1F File Offset: 0x0011911F
	// (set) Token: 0x06003749 RID: 14153 RVA: 0x0011AD27 File Offset: 0x00119127
	public float Gain
	{
		get
		{
			return this.gain;
		}
		set
		{
			this.gain = Mathf.Clamp(value, -24f, 24f);
			OSPManager.OSP_SetGain(this.gain);
		}
	}

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x0600374A RID: 14154 RVA: 0x0011AD4A File Offset: 0x0011914A
	// (set) Token: 0x0600374B RID: 14155 RVA: 0x0011AD52 File Offset: 0x00119152
	public bool UseInverseSquare
	{
		get
		{
			return this.useInverseSquare;
		}
		set
		{
			this.useInverseSquare = value;
			OSPManager.OSP_SetUseInverseSquareAttenuation(this.useInverseSquare);
		}
	}

	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x0600374C RID: 14156 RVA: 0x0011AD66 File Offset: 0x00119166
	// (set) Token: 0x0600374D RID: 14157 RVA: 0x0011AD6E File Offset: 0x0011916E
	public float FalloffNear
	{
		get
		{
			return this.falloffNear;
		}
		set
		{
			this.falloffNear = Mathf.Clamp(value, 0f, 1000000f);
			OSPManager.OSP_SetFalloffRangeGlobal(this.falloffNear, this.falloffFar);
		}
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x0600374E RID: 14158 RVA: 0x0011AD97 File Offset: 0x00119197
	// (set) Token: 0x0600374F RID: 14159 RVA: 0x0011AD9F File Offset: 0x0011919F
	public float FalloffFar
	{
		get
		{
			return this.falloffFar;
		}
		set
		{
			this.falloffFar = Mathf.Clamp(value, 0f, 1000000f);
			OSPManager.OSP_SetFalloffRangeGlobal(this.falloffNear, this.falloffFar);
		}
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x0011ADC8 File Offset: 0x001191C8
	public void GetNearFarFalloffValues(ref float n, ref float f)
	{
		n = this.falloffNear;
		f = this.falloffFar;
	}

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06003751 RID: 14161 RVA: 0x0011ADDA File Offset: 0x001191DA
	// (set) Token: 0x06003752 RID: 14162 RVA: 0x0011ADE2 File Offset: 0x001191E2
	public bool EnableReflections
	{
		get
		{
			return this.enableReflections;
		}
		set
		{
			this.enableReflections = value;
			this.dirtyReflection = true;
		}
	}

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06003753 RID: 14163 RVA: 0x0011ADF2 File Offset: 0x001191F2
	// (set) Token: 0x06003754 RID: 14164 RVA: 0x0011ADFA File Offset: 0x001191FA
	public bool EnableReverb
	{
		get
		{
			return this.enableReverb;
		}
		set
		{
			this.enableReverb = value;
			this.dirtyReflection = true;
		}
	}

	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x06003755 RID: 14165 RVA: 0x0011AE0A File Offset: 0x0011920A
	// (set) Token: 0x06003756 RID: 14166 RVA: 0x0011AE14 File Offset: 0x00119214
	public Vector3 Dimensions
	{
		get
		{
			return this.dimensions;
		}
		set
		{
			this.dimensions = value;
			this.dimensions.x = Mathf.Clamp(this.dimensions.x, 1f, 200f);
			this.dimensions.y = Mathf.Clamp(this.dimensions.y, 1f, 200f);
			this.dimensions.z = Mathf.Clamp(this.dimensions.z, 1f, 200f);
			this.dirtyReflection = true;
		}
	}

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x06003757 RID: 14167 RVA: 0x0011AE9E File Offset: 0x0011929E
	// (set) Token: 0x06003758 RID: 14168 RVA: 0x0011AEA8 File Offset: 0x001192A8
	public Vector2 RK01
	{
		get
		{
			return this.rK01;
		}
		set
		{
			this.rK01 = value;
			this.rK01.x = Mathf.Clamp(this.rK01.x, 0f, 0.97f);
			this.rK01.y = Mathf.Clamp(this.rK01.y, 0f, 0.97f);
			this.dirtyReflection = true;
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x06003759 RID: 14169 RVA: 0x0011AF0D File Offset: 0x0011930D
	// (set) Token: 0x0600375A RID: 14170 RVA: 0x0011AF18 File Offset: 0x00119318
	public Vector2 RK23
	{
		get
		{
			return this.rK23;
		}
		set
		{
			this.rK23 = value;
			this.rK23.x = Mathf.Clamp(this.rK23.x, 0f, 0.95f);
			this.rK23.y = Mathf.Clamp(this.rK23.y, 0f, 0.95f);
			this.dirtyReflection = true;
		}
	}

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x0600375B RID: 14171 RVA: 0x0011AF7D File Offset: 0x0011937D
	// (set) Token: 0x0600375C RID: 14172 RVA: 0x0011AF88 File Offset: 0x00119388
	public Vector2 RK45
	{
		get
		{
			return this.rK45;
		}
		set
		{
			this.rK45 = value;
			this.rK45.x = Mathf.Clamp(this.rK45.x, 0f, 0.95f);
			this.rK45.y = Mathf.Clamp(this.rK45.y, 0f, 0.95f);
			this.dirtyReflection = true;
		}
	}

	// Token: 0x0600375D RID: 14173 RVA: 0x0011AFF0 File Offset: 0x001193F0
	private void Awake()
	{
		if (OSPManager.sInstance == null)
		{
			OSPManager.sInstance = this;
			bool flag = true;
			int outputSampleRate = AudioSettings.outputSampleRate;
			int num;
			int num2;
			AudioSettings.GetDSPBufferSize(out num, out num2);
			Debug.LogWarning(string.Format("OSP: Queried SampleRate: {0:F0} BufferSize: {1:F0}", outputSampleRate, num));
			if (flag)
			{
			}
			Debug.LogWarning(string.Format("OSP: sample rate: {0:F0}", outputSampleRate));
			Debug.LogWarning(string.Format("OSP: buffer size: {0:F0}", num));
			Debug.LogWarning(string.Format("OSP: num buffers: {0:F0}", num2));
			OSPManager.sOSPInit = OSPManager.OSP_Init(outputSampleRate, num);
			OSPManager.OSP_SetBypass(this.bypass);
			OSPManager.OSP_SetGlobalScale(this.globalScale);
			OSPManager.OSP_SetGain(this.gain);
			OSPManager.OSP_SetFalloffRangeGlobal(this.falloffNear, this.falloffFar);
			this.dirtyReflection = true;
			return;
		}
		Debug.LogWarning(string.Format("OSPManager-Awake: Only one instance of OSPManager can exist in the scene.", new object[0]));
	}

	// Token: 0x0600375E RID: 14174 RVA: 0x0011B0E4 File Offset: 0x001194E4
	private void Start()
	{
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x0011B0E6 File Offset: 0x001194E6
	private void Update()
	{
		if (this.dirtyReflection)
		{
			this.UpdateEarlyReflections();
			this.dirtyReflection = false;
		}
	}

	// Token: 0x06003760 RID: 14176 RVA: 0x0011B100 File Offset: 0x00119500
	private void OnDestroy()
	{
		OSPManager.sOSPInit = false;
	}

	// Token: 0x06003761 RID: 14177 RVA: 0x0011B108 File Offset: 0x00119508
	public static bool IsInitialized()
	{
		return OSPManager.sOSPInit;
	}

	// Token: 0x06003762 RID: 14178 RVA: 0x0011B10F File Offset: 0x0011950F
	public static int AcquireContext(int audioSourceInstanceID)
	{
		return OSPManager.OSP_AcquireContext(audioSourceInstanceID);
	}

	// Token: 0x06003763 RID: 14179 RVA: 0x0011B117 File Offset: 0x00119517
	public static void ReleaseContext(int audioSourceInstanceID, int context)
	{
		OSPManager.OSP_ReturnContext(audioSourceInstanceID, context);
	}

	// Token: 0x06003764 RID: 14180 RVA: 0x0011B120 File Offset: 0x00119520
	public static bool GetBypass()
	{
		return OSPManager.OSP_GetBypass();
	}

	// Token: 0x06003765 RID: 14181 RVA: 0x0011B127 File Offset: 0x00119527
	public static bool GetUseInverseSquareAttenuation()
	{
		return OSPManager.OSP_GetUseInverseSquareAttenuation();
	}

	// Token: 0x06003766 RID: 14182 RVA: 0x0011B12E File Offset: 0x0011952E
	public static void SetDisableReflectionsOnSound(int context, bool disable)
	{
		OSPManager.OSP_SetDisableReflectionsOnSound(context, disable);
	}

	// Token: 0x06003767 RID: 14183 RVA: 0x0011B137 File Offset: 0x00119537
	public static float GetDrainTime(int context)
	{
		return OSPManager.OSP_GetDrainTime(context);
	}

	// Token: 0x06003768 RID: 14184 RVA: 0x0011B13F File Offset: 0x0011953F
	public static void SetPositionRel(int context, float x, float y, float z)
	{
		if (!OSPManager.sOSPInit)
		{
			return;
		}
		OSPManager.OSP_SetPositonRelXYZ(context, x, y, z);
	}

	// Token: 0x06003769 RID: 14185 RVA: 0x0011B155 File Offset: 0x00119555
	public static void Spatialize(int context, float[] ioBuf, bool useInvSq, float near, float far)
	{
		if (!OSPManager.sOSPInit)
		{
			return;
		}
		OSPManager.OSP_Spatialize(context, ioBuf, useInvSq, near, far);
	}

	// Token: 0x0600376A RID: 14186 RVA: 0x0011B16D File Offset: 0x0011956D
	public static void SetFalloffRangeLocal(int contextAndSound, float nearRange, float farRange)
	{
		OSPManager.OSP_SetFalloffRangeLocal(contextAndSound, nearRange, farRange);
	}

	// Token: 0x0600376B RID: 14187 RVA: 0x0011B177 File Offset: 0x00119577
	public static int GetMaxNumSpatializedSources()
	{
		return OSPManager.OSP_GetMaxNumSpatializedSounds();
	}

	// Token: 0x0600376C RID: 14188 RVA: 0x0011B180 File Offset: 0x00119580
	private void UpdateEarlyReflections()
	{
		OSPManager.RoomModel roomModel;
		roomModel.Enable = this.enableReflections;
		roomModel.ReverbOn = this.enableReverb;
		roomModel.ReflectionOrder = 0;
		roomModel.DimensionX = this.dimensions.x;
		roomModel.DimensionY = this.dimensions.y;
		roomModel.DimensionZ = this.dimensions.z;
		roomModel.Reflection_K0 = this.rK01.x;
		roomModel.Reflection_K1 = this.rK01.y;
		roomModel.Reflection_K2 = this.rK23.x;
		roomModel.Reflection_K3 = this.rK23.y;
		roomModel.Reflection_K4 = this.rK45.x;
		roomModel.Reflection_K5 = this.rK45.y;
		OSPManager.OSP_UpdateRoomModel(ref roomModel);
	}

	// Token: 0x04002007 RID: 8199
	public const string strOSP = "OculusSpatializerPlugin";

	// Token: 0x04002008 RID: 8200
	private int bufferSize = 512;

	// Token: 0x04002009 RID: 8201
	private int sampleRate = 48000;

	// Token: 0x0400200A RID: 8202
	[SerializeField]
	private bool bypass;

	// Token: 0x0400200B RID: 8203
	[SerializeField]
	private float globalScale = 1f;

	// Token: 0x0400200C RID: 8204
	[SerializeField]
	private float gain;

	// Token: 0x0400200D RID: 8205
	[SerializeField]
	private bool useInverseSquare;

	// Token: 0x0400200E RID: 8206
	[SerializeField]
	private float falloffNear = 10f;

	// Token: 0x0400200F RID: 8207
	[SerializeField]
	private float falloffFar = 1000f;

	// Token: 0x04002010 RID: 8208
	private bool dirtyReflection;

	// Token: 0x04002011 RID: 8209
	[SerializeField]
	private bool enableReflections;

	// Token: 0x04002012 RID: 8210
	[SerializeField]
	private bool enableReverb;

	// Token: 0x04002013 RID: 8211
	[SerializeField]
	private Vector3 dimensions = new Vector3(0f, 0f, 0f);

	// Token: 0x04002014 RID: 8212
	[SerializeField]
	private Vector2 rK01 = new Vector2(0f, 0f);

	// Token: 0x04002015 RID: 8213
	[SerializeField]
	private Vector2 rK23 = new Vector2(0f, 0f);

	// Token: 0x04002016 RID: 8214
	[SerializeField]
	private Vector2 rK45 = new Vector2(0f, 0f);

	// Token: 0x04002017 RID: 8215
	private static bool sOSPInit;

	// Token: 0x04002018 RID: 8216
	public static OSPManager sInstance;

	// Token: 0x02000663 RID: 1635
	public struct RoomModel
	{
		// Token: 0x04002019 RID: 8217
		public bool Enable;

		// Token: 0x0400201A RID: 8218
		public int ReflectionOrder;

		// Token: 0x0400201B RID: 8219
		public float DimensionX;

		// Token: 0x0400201C RID: 8220
		public float DimensionY;

		// Token: 0x0400201D RID: 8221
		public float DimensionZ;

		// Token: 0x0400201E RID: 8222
		public float Reflection_K0;

		// Token: 0x0400201F RID: 8223
		public float Reflection_K1;

		// Token: 0x04002020 RID: 8224
		public float Reflection_K2;

		// Token: 0x04002021 RID: 8225
		public float Reflection_K3;

		// Token: 0x04002022 RID: 8226
		public float Reflection_K4;

		// Token: 0x04002023 RID: 8227
		public float Reflection_K5;

		// Token: 0x04002024 RID: 8228
		public bool ReverbOn;
	}
}
