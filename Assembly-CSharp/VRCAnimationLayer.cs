using System;
using UnityEngine;

// Token: 0x02000A4B RID: 2635
public abstract class VRCAnimationLayer : MonoBehaviour
{
	// Token: 0x06004F72 RID: 20338
	public abstract void Initialize(bool local, Transform avatarRoot, Transform cameraRoot);

	// Token: 0x06004F73 RID: 20339
	public abstract void Attach(Animator a);

	// Token: 0x06004F74 RID: 20340
	public abstract void Detach();

	// Token: 0x06004F75 RID: 20341
	public abstract VRCAnimationLayer.LimbSet GetLimbSet();

	// Token: 0x06004F76 RID: 20342
	public abstract void Apply(bool disableVertical);

	// Token: 0x02000A4C RID: 2636
	[Flags]
	public enum LimbSet
	{
		// Token: 0x040037F7 RID: 14327
		None = 0,
		// Token: 0x040037F8 RID: 14328
		LowerBodyLimbs = 1,
		// Token: 0x040037F9 RID: 14329
		UpperBodyLimbs = 2,
		// Token: 0x040037FA RID: 14330
		Feet = 4,
		// Token: 0x040037FB RID: 14331
		Head = 8,
		// Token: 0x040037FC RID: 14332
		Hands = 16,
		// Token: 0x040037FD RID: 14333
		Fingers = 32,
		// Token: 0x040037FE RID: 14334
		Eyes = 64,
		// Token: 0x040037FF RID: 14335
		Jaw = 128,
		// Token: 0x04003800 RID: 14336
		EyeLids = 256,
		// Token: 0x04003801 RID: 14337
		Root = 512
	}
}
