using System;
using UnityEngine;

// Token: 0x02000A2B RID: 2603
public class ActiveWithUspeak : MonoBehaviour
{
	// Token: 0x06004E71 RID: 20081 RVA: 0x001A48C4 File Offset: 0x001A2CC4
	private void Start()
	{
		VRCPlayer componentInParent = base.GetComponentInParent<VRCPlayer>();
		if (componentInParent == null || componentInParent != VRCPlayer.Instance)
		{
			base.enabled = false;
			return;
		}
		this.USpeakInput = componentInParent.GetComponentInChildren<USpeaker>();
		this._originalLocalScale = base.transform.localScale;
		this._recentScale = base.transform.lossyScale;
	}

	// Token: 0x06004E72 RID: 20082 RVA: 0x001A492C File Offset: 0x001A2D2C
	private void Update()
	{
		if (this.USpeakInput.IsRecording() != this.VisibleObject.enabled)
		{
			this.VisibleObject.enabled = this.USpeakInput.IsRecording();
		}
		if (base.transform.lossyScale != this._recentScale)
		{
			this._recentScale = base.transform.lossyScale;
			base.transform.localScale = this._originalLocalScale;
		}
	}

	// Token: 0x04003690 RID: 13968
	public Renderer VisibleObject;

	// Token: 0x04003691 RID: 13969
	private USpeaker USpeakInput;

	// Token: 0x04003692 RID: 13970
	private Vector3 _originalLocalScale;

	// Token: 0x04003693 RID: 13971
	private Vector3 _recentScale;
}
