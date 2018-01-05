using System;
using System.Collections.Generic;

// Token: 0x02000B17 RID: 2839
public class VRCInputProcessorGaze : VRCInputProcessor
{
	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x06005641 RID: 22081 RVA: 0x001DB153 File Offset: 0x001D9553
	public override bool required
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x06005642 RID: 22082 RVA: 0x001DB156 File Offset: 0x001D9556
	public override bool supported
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x06005643 RID: 22083 RVA: 0x001DB159 File Offset: 0x001D9559
	public override bool platformDefaultEnable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C7F RID: 3199
	// (get) Token: 0x06005644 RID: 22084 RVA: 0x001DB15C File Offset: 0x001D955C
	public override bool present
	{
		get
		{
			return base.enabled && HMDManager.IsHmdDetected();
		}
	}

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x06005645 RID: 22085 RVA: 0x001DB171 File Offset: 0x001D9571
	public override string StorageTag
	{
		get
		{
			return "VRC_INPUT_GAZE";
		}
	}

	// Token: 0x06005646 RID: 22086 RVA: 0x001DB178 File Offset: 0x001D9578
	public override void Apply()
	{
		VRCInput.ClearChanges();
		this._anyKey = VRCInput.IsChanged();
	}

	// Token: 0x06005647 RID: 22087 RVA: 0x001DB18C File Offset: 0x001D958C
	public override void ConnectInputs(Dictionary<string, VRCInput> inputs)
	{
		this.inSelect = inputs["Select"];
		this.inMenu = inputs["Menu"];
		this.inComfortLeft = inputs["ComfortLeft"];
		this.inComfortRight = inputs["ComfortRight"];
		this.inDrop = inputs["DropRight"];
		this.inUse = inputs["UseRight"];
	}

	// Token: 0x04003D06 RID: 15622
	private VRCInput inSelect;

	// Token: 0x04003D07 RID: 15623
	private VRCInput inMenu;

	// Token: 0x04003D08 RID: 15624
	private VRCInput inComfortLeft;

	// Token: 0x04003D09 RID: 15625
	private VRCInput inComfortRight;

	// Token: 0x04003D0A RID: 15626
	private VRCInput inDrop;

	// Token: 0x04003D0B RID: 15627
	private VRCInput inUse;
}
