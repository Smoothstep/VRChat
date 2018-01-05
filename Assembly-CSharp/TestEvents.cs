using System;
using UnityEngine;

// Token: 0x02000AFF RID: 2815
public class TestEvents : MonoBehaviour
{
	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x0600551C RID: 21788 RVA: 0x001D56D8 File Offset: 0x001D3AD8
	// (set) Token: 0x0600551D RID: 21789 RVA: 0x001D56E0 File Offset: 0x001D3AE0
	public bool testBool
	{
		get
		{
			return this._testBool;
		}
		set
		{
			this._testBool = value;
			Debug.Log("Test Bool set to " + value);
		}
	}

	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x0600551E RID: 21790 RVA: 0x001D56FE File Offset: 0x001D3AFE
	// (set) Token: 0x0600551F RID: 21791 RVA: 0x001D5706 File Offset: 0x001D3B06
	public float testFloat
	{
		get
		{
			return this._testFloat;
		}
		set
		{
			this._testFloat = value;
			Debug.Log("Test Float set to " + value);
		}
	}

	// Token: 0x06005520 RID: 21792 RVA: 0x001D5724 File Offset: 0x001D3B24
	public void TestButton()
	{
		Debug.Log("Test Button pressed");
	}

	// Token: 0x04003C17 RID: 15383
	private bool _testBool;

	// Token: 0x04003C18 RID: 15384
	private float _testFloat;
}
