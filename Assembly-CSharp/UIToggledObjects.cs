using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005DA RID: 1498
[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
	// Token: 0x060031AE RID: 12718 RVA: 0x000F5BFC File Offset: 0x000F3FFC
	private void Awake()
	{
		if (this.target != null)
		{
			if (this.activate.Count == 0 && this.deactivate.Count == 0)
			{
				if (this.inverse)
				{
					this.deactivate.Add(this.target);
				}
				else
				{
					this.activate.Add(this.target);
				}
			}
			else
			{
				this.target = null;
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.Toggle));
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x000F5C98 File Offset: 0x000F4098
	public void Toggle()
	{
		bool value = UIToggle.current.value;
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.Set(this.activate[i], value);
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				this.Set(this.deactivate[j], !value);
			}
		}
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x000F5D1C File Offset: 0x000F411C
	private void Set(GameObject go, bool state)
	{
		if (go != null)
		{
			NGUITools.SetActive(go, state);
		}
	}

	// Token: 0x04001C40 RID: 7232
	public List<GameObject> activate;

	// Token: 0x04001C41 RID: 7233
	public List<GameObject> deactivate;

	// Token: 0x04001C42 RID: 7234
	[HideInInspector]
	[SerializeField]
	private GameObject target;

	// Token: 0x04001C43 RID: 7235
	[HideInInspector]
	[SerializeField]
	private bool inverse;
}
