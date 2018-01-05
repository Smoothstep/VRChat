using System;
using UnityEngine;
using UnityEngine.UI;
using VRCSDK2;

// Token: 0x02000CA2 RID: 3234
public class TestCustomScript : MonoBehaviour
{
	// Token: 0x06006430 RID: 25648 RVA: 0x0023D1E2 File Offset: 0x0023B5E2
	[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
	{

	})]
	public void SayHello()
	{
		Debug.Log("Hello World!!");
		this.text.text = "Hello World";
	}

	// Token: 0x04004977 RID: 18807
	public Text text;
}
