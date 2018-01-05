using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000B9C RID: 2972
	public class DebugUI : MonoBehaviour
	{
		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06005C5B RID: 23643 RVA: 0x00204431 File Offset: 0x00202831
		public static DebugUI instance
		{
			get
			{
				if (DebugUI._instance == null)
				{
					DebugUI._instance = UnityEngine.Object.FindObjectOfType<DebugUI>();
				}
				return DebugUI._instance;
			}
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x00204452 File Offset: 0x00202852
		private void Start()
		{
			this.player = Player.instance;
		}

		// Token: 0x06005C5D RID: 23645 RVA: 0x0020445F File Offset: 0x0020285F
		private void OnGUI()
		{
			this.player.Draw2DDebug();
		}

		// Token: 0x040041F3 RID: 16883
		private Player player;

		// Token: 0x040041F4 RID: 16884
		private static DebugUI _instance;
	}
}
