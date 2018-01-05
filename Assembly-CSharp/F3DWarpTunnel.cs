using System;
using UnityEngine;

// Token: 0x0200048D RID: 1165
public class F3DWarpTunnel : MonoBehaviour
{
	// Token: 0x06002820 RID: 10272 RVA: 0x000D0DFB File Offset: 0x000CF1FB
	private void Start()
	{
		this.speed = 0f;
		this.OnDirectionChange();
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x000D0E0E File Offset: 0x000CF20E
	private void OnDirectionChange()
	{
		this.newSpeed = UnityEngine.Random.Range(-this.MaxRotationSpeed, this.MaxRotationSpeed);
		F3DTime.time.AddTimer((float)UnityEngine.Random.Range(1, 5), 1, new Action(this.OnDirectionChange));
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x000D0E48 File Offset: 0x000CF248
	private void Update()
	{
		this.speed = Mathf.Lerp(this.speed, this.newSpeed, Time.deltaTime * this.AdaptationFactor);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, base.transform.rotation * Quaternion.Euler(this.speed, 0f, 0f), Time.deltaTime);
	}

	// Token: 0x0400166C RID: 5740
	public float MaxRotationSpeed;

	// Token: 0x0400166D RID: 5741
	public float AdaptationFactor;

	// Token: 0x0400166E RID: 5742
	private float speed;

	// Token: 0x0400166F RID: 5743
	private float newSpeed;
}
