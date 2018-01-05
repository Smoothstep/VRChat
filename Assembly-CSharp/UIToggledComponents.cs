using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D9 RID: 1497
[ExecuteInEditMode]
[RequireComponent(typeof(UIToggle))]
[AddComponentMenu("NGUI/Interaction/Toggled Components")]
public class UIToggledComponents : MonoBehaviour
{
	// Token: 0x060031AB RID: 12715 RVA: 0x000F5AC8 File Offset: 0x000F3EC8
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

	// Token: 0x060031AC RID: 12716 RVA: 0x000F5B64 File Offset: 0x000F3F64
	public void Toggle()
	{
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				MonoBehaviour monoBehaviour = this.activate[i];
				monoBehaviour.enabled = UIToggle.current.value;
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				MonoBehaviour monoBehaviour2 = this.deactivate[j];
				monoBehaviour2.enabled = !UIToggle.current.value;
			}
		}
	}

	// Token: 0x04001C3C RID: 7228
	public List<MonoBehaviour> activate;

	// Token: 0x04001C3D RID: 7229
	public List<MonoBehaviour> deactivate;

	// Token: 0x04001C3E RID: 7230
	[HideInInspector]
	[SerializeField]
	private MonoBehaviour target;

	// Token: 0x04001C3F RID: 7231
	[HideInInspector]
	[SerializeField]
	private bool inverse;
}
