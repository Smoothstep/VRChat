using System;
using UnityEngine;

// Token: 0x02000A3D RID: 2621
public interface IVRCTrackedIk
{
	// Token: 0x17000BB5 RID: 2997
	// (set) Token: 0x06004EDA RID: 20186
	bool enableIk { set; }

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x06004EDB RID: 20187
	// (set) Token: 0x06004EDC RID: 20188
	bool hasLowerBodyTracking { get; set; }

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06004EDD RID: 20189
	bool isCalibrated { get; }

	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x06004EDE RID: 20190
	// (set) Token: 0x06004EDF RID: 20191
	float LeftHandWeight { get; set; }

	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x06004EE0 RID: 20192
	// (set) Token: 0x06004EE1 RID: 20193
	float RightHandWeight { get; set; }

	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x06004EE2 RID: 20194
	// (set) Token: 0x06004EE3 RID: 20195
	float HeadPosWeight { get; set; }

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x06004EE4 RID: 20196
	// (set) Token: 0x06004EE5 RID: 20197
	float HeadRotWeight { get; set; }

	// Token: 0x17000BBC RID: 3004
	// (get) Token: 0x06004EE6 RID: 20198
	// (set) Token: 0x06004EE7 RID: 20199
	float LowerBodyWeight { get; set; }

	// Token: 0x17000BBD RID: 3005
	// (get) Token: 0x06004EE8 RID: 20200
	// (set) Token: 0x06004EE9 RID: 20201
	float SolverWeight { get; set; }

	// Token: 0x17000BBE RID: 3006
	// (get) Token: 0x06004EEA RID: 20202
	// (set) Token: 0x06004EEB RID: 20203
	bool isCulled { get; set; }

	// Token: 0x06004EEC RID: 20204
	bool Initialize(VRC_AnimationController animControl, Animator anim, VRCPlayer player, bool local);

	// Token: 0x06004EED RID: 20205
	void Uninitialize();

	// Token: 0x06004EEE RID: 20206
	void LocomotionChange(bool loco);

	// Token: 0x06004EEF RID: 20207
	void SeatedChange(bool sitting);

	// Token: 0x06004EF0 RID: 20208
	void NeedsReset();

	// Token: 0x06004EF1 RID: 20209
	void Reset(bool restore);
}
