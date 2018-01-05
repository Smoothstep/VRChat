using System;
using UnityEngine;

// Token: 0x02000648 RID: 1608
[RequireComponent(typeof(UIInput))]
public class UIInputOnGUI : MonoBehaviour
{
	// Token: 0x060035CD RID: 13773 RVA: 0x00111F29 File Offset: 0x00110329
	private void Awake()
	{
		this.mInput = base.GetComponent<UIInput>();
	}

	// Token: 0x060035CE RID: 13774 RVA: 0x00111F37 File Offset: 0x00110337
	private void OnGUI()
	{
		if (Event.current.rawType == EventType.KeyDown)
		{
			this.mInput.ProcessEvent(Event.current);
		}
	}

	// Token: 0x04001F15 RID: 7957
	[NonSerialized]
	private UIInput mInput;
}
