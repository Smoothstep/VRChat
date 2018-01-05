using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A06 RID: 2566
[ExecuteInEditMode]
public class DisableMobileContent : MonoBehaviour
{
	// Token: 0x06004E01 RID: 19969 RVA: 0x001A19F8 File Offset: 0x0019FDF8
	private void Awake()
	{
		this.enableMobileControls = false;
		this.SetMobileControlsStatus(this.enableMobileControls);
		this.mobileControlsPreviousState = this.enableMobileControls;
	}

	// Token: 0x06004E02 RID: 19970 RVA: 0x001A1A19 File Offset: 0x0019FE19
	private void UpdateControlStatus()
	{
		if (this.mobileControlsPreviousState != this.enableMobileControls)
		{
			this.SetMobileControlsStatus(this.enableMobileControls);
			this.mobileControlsPreviousState = this.enableMobileControls;
		}
	}

	// Token: 0x06004E03 RID: 19971 RVA: 0x001A1A44 File Offset: 0x0019FE44
	private void SetMobileControlsStatus(bool activeStatus)
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.transform.gameObject.SetActive(activeStatus);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x040035DC RID: 13788
	public bool enableMobileControls;

	// Token: 0x040035DD RID: 13789
	private bool mobileControlsPreviousState;
}
