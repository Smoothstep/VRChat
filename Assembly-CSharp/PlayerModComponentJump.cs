using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000AC9 RID: 2761
public class PlayerModComponentJump : MonoBehaviour, IPlayerModComponent
{
	// Token: 0x060053F0 RID: 21488 RVA: 0x001CFA50 File Offset: 0x001CDE50
	private void Start()
	{
		this.controller = base.GetComponent<LocomotionInputController>();
		if (this.controller == null || !this.controller.enabled)
		{
			base.enabled = false;
		}
		this.motion = base.GetComponent<VRCMotionState>();
		if (this.motion == null)
		{
			base.enabled = false;
		}
		this.inJump = VRCInputManager.FindInput("Jump");
	}

	// Token: 0x060053F1 RID: 21489 RVA: 0x001CFAC8 File Offset: 0x001CDEC8
	private void Update()
	{
		if (this.controller == null)
		{
			this.controller = base.GetComponent<LocomotionInputController>();
		}
		if (this.motion == null)
		{
			this.motion = base.GetComponent<VRCMotionState>();
		}
		if (this.inJump.down)
		{
			this.motion.Jump(this.jumpPower);
		}
	}

	// Token: 0x060053F2 RID: 21490 RVA: 0x001CFB54 File Offset: 0x001CDF54
	public void SetProperties(List<VRCPlayerModProperty> properties)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (VRCPlayerModProperty vrcplayerModProperty in properties)
		{
			dictionary[vrcplayerModProperty.name] = vrcplayerModProperty.value();
		}
		this.jumpPower = (float)Tools.GetOrDefaultFromDictionary(dictionary, "jumpPower", 3f);
	}

	// Token: 0x04003B34 RID: 15156
	private LocomotionInputController controller;

	// Token: 0x04003B35 RID: 15157
	private VRCMotionState motion;

	// Token: 0x04003B36 RID: 15158
	private float jumpPower = 3f;

	// Token: 0x04003B37 RID: 15159
	private const float timeToJump = 0.1f;

	// Token: 0x04003B38 RID: 15160
	private VRCInput inJump;
}
