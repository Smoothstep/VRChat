using System;
using UnityEngine;

// Token: 0x020006D8 RID: 1752
public sealed class OVRTouchpadHelper : MonoBehaviour
{
	// Token: 0x060039DC RID: 14812 RVA: 0x00123C09 File Offset: 0x00122009
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060039DD RID: 14813 RVA: 0x00123C16 File Offset: 0x00122016
	private void Start()
	{
		OVRTouchpad.TouchHandler += this.LocalTouchEventCallback;
	}

	// Token: 0x060039DE RID: 14814 RVA: 0x00123C29 File Offset: 0x00122029
	private void Update()
	{
		OVRTouchpad.Update();
	}

	// Token: 0x060039DF RID: 14815 RVA: 0x00123C30 File Offset: 0x00122030
	public void OnDisable()
	{
		OVRTouchpad.OnDisable();
	}

	// Token: 0x060039E0 RID: 14816 RVA: 0x00123C38 File Offset: 0x00122038
	private void LocalTouchEventCallback(object sender, EventArgs args)
	{
		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs)args;
		switch (touchArgs.TouchType)
		{
		}
	}
}
