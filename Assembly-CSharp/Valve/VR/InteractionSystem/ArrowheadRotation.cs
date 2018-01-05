using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BD1 RID: 3025
	public class ArrowheadRotation : MonoBehaviour
	{
		// Token: 0x06005DAD RID: 23981 RVA: 0x0020C49C File Offset: 0x0020A89C
		private void Start()
		{
			float x = UnityEngine.Random.Range(0f, 180f);
			base.transform.localEulerAngles = new Vector3(x, -90f, 90f);
		}
	}
}
