using System;
using UnityEngine;

// Token: 0x02000594 RID: 1428
public class Tutorial5 : MonoBehaviour
{
	// Token: 0x06002FEA RID: 12266 RVA: 0x000EA834 File Offset: 0x000E8C34
	public void SetDurationToCurrentProgress()
	{
		UITweener[] componentsInChildren = base.GetComponentsInChildren<UITweener>();
		foreach (UITweener uitweener in componentsInChildren)
		{
			uitweener.duration = Mathf.Lerp(2f, 0.5f, UIProgressBar.current.value);
		}
	}
}
