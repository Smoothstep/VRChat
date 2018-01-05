using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BDF RID: 3039
	public class ChaperoneInfo : MonoBehaviour
	{
		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06005DF7 RID: 24055 RVA: 0x0020E2C8 File Offset: 0x0020C6C8
		// (set) Token: 0x06005DF8 RID: 24056 RVA: 0x0020E2D0 File Offset: 0x0020C6D0
		public bool initialized { get; private set; }

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06005DF9 RID: 24057 RVA: 0x0020E2D9 File Offset: 0x0020C6D9
		// (set) Token: 0x06005DFA RID: 24058 RVA: 0x0020E2E1 File Offset: 0x0020C6E1
		public float playAreaSizeX { get; private set; }

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06005DFB RID: 24059 RVA: 0x0020E2EA File Offset: 0x0020C6EA
		// (set) Token: 0x06005DFC RID: 24060 RVA: 0x0020E2F2 File Offset: 0x0020C6F2
		public float playAreaSizeZ { get; private set; }

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06005DFD RID: 24061 RVA: 0x0020E2FB File Offset: 0x0020C6FB
		// (set) Token: 0x06005DFE RID: 24062 RVA: 0x0020E303 File Offset: 0x0020C703
		public bool roomscale { get; private set; }

		// Token: 0x06005DFF RID: 24063 RVA: 0x0020E30C File Offset: 0x0020C70C
		public static SteamVR_Events.Action InitializedAction(UnityAction action)
		{
			return new SteamVR_Events.ActionNoArgs(ChaperoneInfo.Initialized, action);
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06005E00 RID: 24064 RVA: 0x0020E31C File Offset: 0x0020C71C
		public static ChaperoneInfo instance
		{
			get
			{
				if (ChaperoneInfo._instance == null)
				{
					ChaperoneInfo._instance = new GameObject("[ChaperoneInfo]").AddComponent<ChaperoneInfo>();
					ChaperoneInfo._instance.initialized = false;
					ChaperoneInfo._instance.playAreaSizeX = 1f;
					ChaperoneInfo._instance.playAreaSizeZ = 1f;
					ChaperoneInfo._instance.roomscale = false;
					UnityEngine.Object.DontDestroyOnLoad(ChaperoneInfo._instance.gameObject);
				}
				return ChaperoneInfo._instance;
			}
		}

		// Token: 0x06005E01 RID: 24065 RVA: 0x0020E398 File Offset: 0x0020C798
		private IEnumerator Start()
		{
			CVRChaperone chaperone = OpenVR.Chaperone;
			if (chaperone == null)
			{
				Debug.LogWarning("Failed to get IVRChaperone interface.");
				this.initialized = true;
				yield break;
			}
			float px;
			float pz;
			for (;;)
			{
				px = 0f;
				pz = 0f;
				if (chaperone.GetPlayAreaSize(ref px, ref pz))
				{
					break;
				}
				yield return null;
			}
			this.initialized = true;
			this.playAreaSizeX = px;
			this.playAreaSizeZ = pz;
			this.roomscale = (Mathf.Max(px, pz) > 1.01f);
			Debug.LogFormat("ChaperoneInfo initialized. {2} play area {0:0.00}m x {1:0.00}m", new object[]
			{
				px,
				pz,
				(!this.roomscale) ? "Standing" : "Roomscale"
			});
			ChaperoneInfo.Initialized.Send();
			yield break;
			yield break;
		}

		// Token: 0x040043AE RID: 17326
		public static SteamVR_Events.Event Initialized = new SteamVR_Events.Event();

		// Token: 0x040043AF RID: 17327
		private static ChaperoneInfo _instance;
	}
}
