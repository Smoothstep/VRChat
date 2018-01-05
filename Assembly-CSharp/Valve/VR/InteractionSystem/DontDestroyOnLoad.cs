using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BA1 RID: 2977
	public class DontDestroyOnLoad : MonoBehaviour
	{
		// Token: 0x06005C69 RID: 23657 RVA: 0x002046E5 File Offset: 0x00202AE5
		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
	}
}
