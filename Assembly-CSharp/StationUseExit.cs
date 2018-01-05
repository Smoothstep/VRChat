using System;
using System.Linq;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000AFB RID: 2811
public class StationUseExit : MonoBehaviour
{
	// Token: 0x06005509 RID: 21769 RVA: 0x001D5138 File Offset: 0x001D3538
	private void Start()
	{
		this.inAxisVertical = VRCInputManager.FindInput("Vertical");
		this.inAxisHorizontal = VRCInputManager.FindInput("Horizontal");
	}

	// Token: 0x0600550A RID: 21770 RVA: 0x001D515C File Offset: 0x001D355C
	private void Update()
	{
		Vector2 zero = Vector2.zero;
		if (this.inAxisHorizontal == null || this.inAxisVertical == null)
		{
			Debug.LogError("StationUseExit input(s) are null!");
		}
		zero.x = this.inAxisHorizontal.axis;
		zero.y = this.inAxisVertical.axis;
		bool flag = zero.sqrMagnitude > 0f;
		if (flag)
		{
			this.UseExit();
		}
	}

	// Token: 0x0600550B RID: 21771 RVA: 0x001D51D0 File Offset: 0x001D35D0
	public void UseExit()
	{
		VRC_EventHandler vrc_EventHandler = base.GetComponent<VRC_EventHandler>();
		if (vrc_EventHandler == null)
		{
			vrc_EventHandler = base.GetComponentInParent<VRC_EventHandler>();
		}
		foreach (VRC_EventHandler.VrcEvent e2 in from e in vrc_EventHandler.Events
		where e.Name == "Exit" || e.Name == "ExitStation"
		select e)
		{
			vrc_EventHandler.TriggerEvent(e2, VRC_EventHandler.VrcBroadcastType.Local, this.localUser, 0f);
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x04003C0B RID: 15371
	public GameObject localUser;

	// Token: 0x04003C0C RID: 15372
	private VRCInput inAxisVertical;

	// Token: 0x04003C0D RID: 15373
	private VRCInput inAxisHorizontal;
}
