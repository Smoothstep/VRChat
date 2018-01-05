using System;
using UnityEngine;

// Token: 0x02000C19 RID: 3097
public class AnimateOnEnabled : MonoBehaviour
{
	// Token: 0x06005FEC RID: 24556 RVA: 0x0021BBF0 File Offset: 0x00219FF0
	private void OnEnable()
	{
		Animator component = base.GetComponent<Animator>();
		if (component != null)
		{
			component.SetTrigger("enabled");
		}
	}
}
