using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C49 RID: 3145
public class UiPlayerHeight : MonoBehaviour
{
	// Token: 0x06006186 RID: 24966 RVA: 0x00226B14 File Offset: 0x00224F14
	private void SetLabel(float height)
	{
		int num;
		int num2;
		this.MetricToImperial(height, out num, out num2);
		this.label.text = string.Format("{0:G}' {1:G}\" ({2:F2}m)", num, num2, height);
	}

	// Token: 0x06006187 RID: 24967 RVA: 0x00226B53 File Offset: 0x00224F53
	private void OnEnable()
	{
		this.SetLabel(VRCTrackingManager.GetPlayerHeight());
	}

	// Token: 0x06006188 RID: 24968 RVA: 0x00226B60 File Offset: 0x00224F60
	private void MetricToImperial(float m, out int feet, out int inch)
	{
		float num = m / 0.0254f;
		feet = Mathf.FloorToInt(num / 12f);
		inch = Mathf.RoundToInt(num - (float)(feet * 12));
		if (inch == 12)
		{
			feet++;
			inch = 0;
		}
	}

	// Token: 0x06006189 RID: 24969 RVA: 0x00226BA4 File Offset: 0x00224FA4
	public void HeightUp()
	{
		VRCTrackingManager.IncreasePlayerHeight();
		this.SetLabel(VRCTrackingManager.GetPlayerHeight());
	}

	// Token: 0x0600618A RID: 24970 RVA: 0x00226BB6 File Offset: 0x00224FB6
	public void HeightDown()
	{
		VRCTrackingManager.DecreasePlayerHeight();
		this.SetLabel(VRCTrackingManager.GetPlayerHeight());
	}

	// Token: 0x04004720 RID: 18208
	public Button buttonUp;

	// Token: 0x04004721 RID: 18209
	public Button buttonDown;

	// Token: 0x04004722 RID: 18210
	public Text label;

	// Token: 0x04004723 RID: 18211
	private float _metricHt;
}
