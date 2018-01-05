using System;
using System.Collections.Generic;
using UnityEngine;
using VRC;
using VRCSDK2;

// Token: 0x02000ACD RID: 2765
public class PlayerModComponentSpeed : MonoBehaviour, IPlayerModComponent
{
	// Token: 0x060053FD RID: 21501 RVA: 0x001D0118 File Offset: 0x001CE518
	private void Start()
	{
		this.controller = base.GetComponent<LocomotionInputController>();
		if (this.controller != null && this.controller.enabled)
		{
			this.ApplyModdedVariabes();
		}
		else
		{
			base.enabled = false;
		}
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x001D0164 File Offset: 0x001CE564
	private void OnDestroy()
	{
		this.ResetModdedVariables();
	}

	// Token: 0x060053FF RID: 21503 RVA: 0x001D016C File Offset: 0x001CE56C
	public void SetProperties(List<VRCPlayerModProperty> properties)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (VRCPlayerModProperty vrcplayerModProperty in properties)
		{
			dictionary[vrcplayerModProperty.name] = vrcplayerModProperty.value();
		}
		this.walkSpeed = (float)Tools.GetOrDefaultFromDictionary(dictionary, "walkSpeed", 2f);
		this.runSpeed = (float)Tools.GetOrDefaultFromDictionary(dictionary, "runSpeed", 4f);
		this.strafeSpeed = (float)Tools.GetOrDefaultFromDictionary(dictionary, "strafeSpeed", 2f);
		this.ApplyModdedVariabes();
	}

	// Token: 0x06005400 RID: 21504 RVA: 0x001D023C File Offset: 0x001CE63C
	private void ApplyModdedVariabes()
	{
		if (this.controller != null)
		{
			if (!this.hasSetOrigValues)
			{
				this.origWalkSpeed = this.controller.walkSpeed;
				this.origRunSpeed = this.controller.runSpeed;
				this.origStrafeSpeed = this.controller.strafeSpeed;
				this.hasSetOrigValues = true;
			}
			this.controller.walkSpeed = this.walkSpeed;
			this.controller.runSpeed = this.runSpeed;
			this.controller.strafeSpeed = this.strafeSpeed;
		}
	}

	// Token: 0x06005401 RID: 21505 RVA: 0x001D02D4 File Offset: 0x001CE6D4
	private void ResetModdedVariables()
	{
		if (this.controller != null && this.controller.enabled)
		{
			this.controller.walkSpeed = this.origWalkSpeed;
			this.controller.runSpeed = this.origRunSpeed;
			this.controller.strafeSpeed = this.origStrafeSpeed;
		}
	}

	// Token: 0x04003B48 RID: 15176
	private LocomotionInputController controller;

	// Token: 0x04003B49 RID: 15177
	private float walkSpeed;

	// Token: 0x04003B4A RID: 15178
	private float origWalkSpeed;

	// Token: 0x04003B4B RID: 15179
	private float runSpeed;

	// Token: 0x04003B4C RID: 15180
	private float origRunSpeed;

	// Token: 0x04003B4D RID: 15181
	private float strafeSpeed;

	// Token: 0x04003B4E RID: 15182
	private float origStrafeSpeed;

	// Token: 0x04003B4F RID: 15183
	private bool hasSetOrigValues;
}
