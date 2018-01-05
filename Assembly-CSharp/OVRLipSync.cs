using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020006F3 RID: 1779
public class OVRLipSync : MonoBehaviour
{
	// Token: 0x06003A7D RID: 14973
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_Initialize(int samplerate, int buffersize);

	// Token: 0x06003A7E RID: 14974
	[DllImport("OVRLipSync")]
	private static extern void ovrLipSyncDll_Shutdown();

	// Token: 0x06003A7F RID: 14975
	[DllImport("OVRLipSync")]
	private static extern IntPtr ovrLipSyncDll_GetVersion(ref int Major, ref int Minor, ref int Patch);

	// Token: 0x06003A80 RID: 14976
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_CreateContext(ref uint context, OVRLipSync.ovrLipSyncContextProvider provider);

	// Token: 0x06003A81 RID: 14977
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_DestroyContext(uint context);

	// Token: 0x06003A82 RID: 14978
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_ResetContext(uint context);

	// Token: 0x06003A83 RID: 14979
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_SendSignal(uint context, OVRLipSync.ovrLipSyncSignals signal, int arg1, int arg2);

	// Token: 0x06003A84 RID: 14980
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_ProcessFrame(uint context, float[] audioBuffer, OVRLipSync.ovrLipSyncFlag flags, ref int frameNumber, ref int frameDelay, float[] visemes, int visemeCount);

	// Token: 0x06003A85 RID: 14981
	[DllImport("OVRLipSync")]
	private static extern int ovrLipSyncDll_ProcessFrameInterleaved(uint context, float[] audioBuffer, OVRLipSync.ovrLipSyncFlag flags, ref int frameNumber, ref int frameDelay, float[] visemes, int visemeCount);

	// Token: 0x06003A86 RID: 14982 RVA: 0x00127500 File Offset: 0x00125900
	private void Awake()
	{
		if (OVRLipSync.sInstance == null)
		{
			OVRLipSync.sInstance = this;
			int outputSampleRate = AudioSettings.outputSampleRate;
			int num;
			int num2;
			AudioSettings.GetDSPBufferSize(out num, out num2);
			string message = string.Format("OvrLipSync Awake: Queried SampleRate: {0:F0} BufferSize: {1:F0}", outputSampleRate, num);
			Debug.LogWarning(message);
			OVRLipSync.sOVRLipSyncInit = OVRLipSync.ovrLipSyncDll_Initialize(outputSampleRate, num);
			if (OVRLipSync.sOVRLipSyncInit != 0)
			{
				Debug.LogWarning(string.Format("OvrLipSync Awake: Failed to init Speech Rec library", new object[0]));
			}
			OVRTouchpad.Create();
			return;
		}
		Debug.LogWarning(string.Format("OVRLipSync Awake: Only one instance of OVRPLipSync can exist in the scene.", new object[0]));
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x0012759A File Offset: 0x0012599A
	private void Start()
	{
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x0012759C File Offset: 0x0012599C
	private void Update()
	{
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x0012759E File Offset: 0x0012599E
	private void OnDestroy()
	{
		if (OVRLipSync.sInstance != this)
		{
			Debug.LogWarning("OVRLipSync OnDestroy: This is not the correct OVRLipSync instance.");
			return;
		}
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x001275BB File Offset: 0x001259BB
	public static int IsInitialized()
	{
		return OVRLipSync.sOVRLipSyncInit;
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x001275C2 File Offset: 0x001259C2
	public static int CreateContext(ref uint context, OVRLipSync.ovrLipSyncContextProvider provider)
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2201;
		}
		return OVRLipSync.ovrLipSyncDll_CreateContext(ref context, provider);
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x001275DB File Offset: 0x001259DB
	public static int DestroyContext(uint context)
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2200;
		}
		return OVRLipSync.ovrLipSyncDll_DestroyContext(context);
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x001275F3 File Offset: 0x001259F3
	public static int ResetContext(uint context)
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2200;
		}
		return OVRLipSync.ovrLipSyncDll_ResetContext(context);
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x0012760B File Offset: 0x00125A0B
	public static int SendSignal(uint context, OVRLipSync.ovrLipSyncSignals signal, int arg1, int arg2)
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2200;
		}
		return OVRLipSync.ovrLipSyncDll_SendSignal(context, signal, arg1, arg2);
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x00127626 File Offset: 0x00125A26
	public static int ProcessFrame(uint context, float[] audioBuffer, OVRLipSync.ovrLipSyncFlag flags, ref OVRLipSync.ovrLipSyncFrame frame)
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2200;
		}
		return OVRLipSync.ovrLipSyncDll_ProcessFrame(context, audioBuffer, flags, ref frame.frameNumber, ref frame.frameDelay, frame.Visemes, frame.Visemes.Length);
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x0012765A File Offset: 0x00125A5A
	public static int ProcessFrameInterleaved(uint context, float[] audioBuffer, OVRLipSync.ovrLipSyncFlag flags, ref OVRLipSync.ovrLipSyncFrame frame)
	{
		if (OVRLipSync.IsInitialized() != 0)
		{
			return -2200;
		}
		return OVRLipSync.ovrLipSyncDll_ProcessFrameInterleaved(context, audioBuffer, flags, ref frame.frameNumber, ref frame.frameDelay, frame.Visemes, frame.Visemes.Length);
	}

	// Token: 0x0400232C RID: 9004
	public const int ovrLipSyncSuccess = 0;

	// Token: 0x0400232D RID: 9005
	public const string strOVRLS = "OVRLipSync";

	// Token: 0x0400232E RID: 9006
	private static int sOVRLipSyncInit = -2200;

	// Token: 0x0400232F RID: 9007
	public static OVRLipSync sInstance;

	// Token: 0x020006F4 RID: 1780
	public enum ovrLipSyncError
	{
		// Token: 0x04002331 RID: 9009
		Unknown = -2200,
		// Token: 0x04002332 RID: 9010
		CannotCreateContext = -2201,
		// Token: 0x04002333 RID: 9011
		InvalidParam = -2202,
		// Token: 0x04002334 RID: 9012
		BadSampleRate = -2203,
		// Token: 0x04002335 RID: 9013
		MissingDLL = -2204,
		// Token: 0x04002336 RID: 9014
		BadVersion = -2205,
		// Token: 0x04002337 RID: 9015
		UndefinedFunction = -2206
	}

	// Token: 0x020006F5 RID: 1781
	public enum ovrLipSyncViseme
	{
		// Token: 0x04002339 RID: 9017
		sil,
		// Token: 0x0400233A RID: 9018
		PP,
		// Token: 0x0400233B RID: 9019
		FF,
		// Token: 0x0400233C RID: 9020
		TH,
		// Token: 0x0400233D RID: 9021
		DD,
		// Token: 0x0400233E RID: 9022
		kk,
		// Token: 0x0400233F RID: 9023
		CH,
		// Token: 0x04002340 RID: 9024
		SS,
		// Token: 0x04002341 RID: 9025
		nn,
		// Token: 0x04002342 RID: 9026
		RR,
		// Token: 0x04002343 RID: 9027
		aa,
		// Token: 0x04002344 RID: 9028
		E,
		// Token: 0x04002345 RID: 9029
		ih,
		// Token: 0x04002346 RID: 9030
		oh,
		// Token: 0x04002347 RID: 9031
		ou,
		// Token: 0x04002348 RID: 9032
		Count
	}

	// Token: 0x020006F6 RID: 1782
	public enum ovrLipSyncFlag
	{
		// Token: 0x0400234A RID: 9034
		None,
		// Token: 0x0400234B RID: 9035
		DelayCompensateAudio
	}

	// Token: 0x020006F7 RID: 1783
	public enum ovrLipSyncSignals
	{
		// Token: 0x0400234D RID: 9037
		VisemeOn,
		// Token: 0x0400234E RID: 9038
		VisemeOff,
		// Token: 0x0400234F RID: 9039
		VisemeAmount,
		// Token: 0x04002350 RID: 9040
		VisemeSmoothing,
		// Token: 0x04002351 RID: 9041
		Count
	}

	// Token: 0x020006F8 RID: 1784
	public enum ovrLipSyncContextProvider
	{
		// Token: 0x04002353 RID: 9043
		Main,
		// Token: 0x04002354 RID: 9044
		Other
	}

	// Token: 0x020006F9 RID: 1785
	public struct ovrLipSyncFrame
	{
		// Token: 0x06003A92 RID: 14994 RVA: 0x0012769A File Offset: 0x00125A9A
		public ovrLipSyncFrame(int init)
		{
			this.frameNumber = 0;
			this.frameDelay = 0;
			this.Visemes = new float[15];
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x001276B7 File Offset: 0x00125AB7
		public void CopyInput(ref OVRLipSync.ovrLipSyncFrame input)
		{
			this.frameNumber = input.frameNumber;
			this.frameDelay = input.frameDelay;
			input.Visemes.CopyTo(this.Visemes, 0);
		}

		// Token: 0x04002355 RID: 9045
		public int frameNumber;

		// Token: 0x04002356 RID: 9046
		public int frameDelay;

		// Token: 0x04002357 RID: 9047
		public float[] Visemes;
	}
}
