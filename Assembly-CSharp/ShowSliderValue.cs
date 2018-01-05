using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000494 RID: 1172
[RequireComponent(typeof(Text))]
public class ShowSliderValue : MonoBehaviour
{
	// Token: 0x06002837 RID: 10295 RVA: 0x000D122C File Offset: 0x000CF62C
	public void UpdateLabel(float value)
	{
		Text component = base.GetComponent<Text>();
		if (component != null)
		{
			component.text = Mathf.RoundToInt(value * 100f) + "%";
		}
	}
}
