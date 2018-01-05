using System;
using System.Collections.Generic;

// Token: 0x02000B18 RID: 2840
public class VRCInputProcessorHydra : VRCInputProcessor
{
	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x06005649 RID: 22089 RVA: 0x001DB207 File Offset: 0x001D9607
	public override string StorageTag
	{
		get
		{
			return "VRC_INPUT_HYDRA";
		}
	}

	// Token: 0x0600564A RID: 22090 RVA: 0x001DB20E File Offset: 0x001D960E
	public override void Apply()
	{
		VRCInput.ClearChanges();
		this._anyKey = VRCInput.IsChanged();
	}

	// Token: 0x0600564B RID: 22091 RVA: 0x001DB220 File Offset: 0x001D9620
	public override void ConnectInputs(Dictionary<string, VRCInput> inputs)
	{
	}
}
