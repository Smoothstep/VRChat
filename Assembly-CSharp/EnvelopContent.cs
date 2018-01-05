using System;
using UnityEngine;

// Token: 0x02000587 RID: 1415
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Examples/Envelop Content")]
public class EnvelopContent : MonoBehaviour
{
	// Token: 0x06002FC2 RID: 12226 RVA: 0x000E94DF File Offset: 0x000E78DF
	private void Start()
	{
		this.mStarted = true;
		this.Execute();
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x000E94EE File Offset: 0x000E78EE
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.Execute();
		}
	}

	// Token: 0x06002FC4 RID: 12228 RVA: 0x000E9504 File Offset: 0x000E7904
	[ContextMenu("Execute")]
	public void Execute()
	{
		if (this.targetRoot == base.transform)
		{
			Debug.LogError("Target Root object cannot be the same object that has Envelop Content. Make it a sibling instead.", this);
		}
		else if (NGUITools.IsChild(this.targetRoot, base.transform))
		{
			Debug.LogError("Target Root object should not be a parent of Envelop Content. Make it a sibling instead.", this);
		}
		else
		{
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.transform.parent, this.targetRoot, false);
			float num = bounds.min.x + (float)this.padLeft;
			float num2 = bounds.min.y + (float)this.padBottom;
			float num3 = bounds.max.x + (float)this.padRight;
			float num4 = bounds.max.y + (float)this.padTop;
			UIWidget component = base.GetComponent<UIWidget>();
			component.SetRect(num, num2, num3 - num, num4 - num2);
			base.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x04001A2A RID: 6698
	public Transform targetRoot;

	// Token: 0x04001A2B RID: 6699
	public int padLeft;

	// Token: 0x04001A2C RID: 6700
	public int padRight;

	// Token: 0x04001A2D RID: 6701
	public int padBottom;

	// Token: 0x04001A2E RID: 6702
	public int padTop;

	// Token: 0x04001A2F RID: 6703
	private bool mStarted;
}
