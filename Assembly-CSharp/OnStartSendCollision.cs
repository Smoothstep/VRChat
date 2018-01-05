using System;
using UnityEngine;

// Token: 0x020008A3 RID: 2211
public class OnStartSendCollision : MonoBehaviour
{
	// Token: 0x060043D0 RID: 17360 RVA: 0x00166454 File Offset: 0x00164854
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	// Token: 0x060043D1 RID: 17361 RVA: 0x0016649D File Offset: 0x0016489D
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		this.effectSettings.OnCollisionHandler(new CollisionInfo());
		this.isInitialized = true;
	}

	// Token: 0x060043D2 RID: 17362 RVA: 0x001664C2 File Offset: 0x001648C2
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.effectSettings.OnCollisionHandler(new CollisionInfo());
		}
	}

	// Token: 0x04002CB5 RID: 11445
	private EffectSettings effectSettings;

	// Token: 0x04002CB6 RID: 11446
	private bool isInitialized;
}
