using System;
using UnityEngine;

// Token: 0x02000894 RID: 2196
public class CollisionActiveBehaviour : MonoBehaviour
{
	// Token: 0x0600437B RID: 17275 RVA: 0x00164494 File Offset: 0x00162894
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.IsReverse)
		{
			this.effectSettings.RegistreInactiveElement(base.gameObject, this.TimeDelay);
			base.gameObject.SetActive(false);
		}
		else
		{
			this.effectSettings.RegistreActiveElement(base.gameObject, this.TimeDelay);
		}
		if (this.IsLookAt)
		{
			this.effectSettings.CollisionEnter += this.effectSettings_CollisionEnter;
		}
	}

	// Token: 0x0600437C RID: 17276 RVA: 0x00164519 File Offset: 0x00162919
	private void effectSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		base.transform.LookAt(this.effectSettings.transform.position + e.Hit.normal);
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x00164548 File Offset: 0x00162948
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

	// Token: 0x04002C1D RID: 11293
	public bool IsReverse;

	// Token: 0x04002C1E RID: 11294
	public float TimeDelay;

	// Token: 0x04002C1F RID: 11295
	public bool IsLookAt;

	// Token: 0x04002C20 RID: 11296
	private EffectSettings effectSettings;
}
