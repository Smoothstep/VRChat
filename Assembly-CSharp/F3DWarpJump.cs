using System;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class F3DWarpJump : MonoBehaviour
{
	// Token: 0x06002815 RID: 10261 RVA: 0x000D0B09 File Offset: 0x000CEF09
	private void Start()
	{
		if (this.SendOnSpawned)
		{
			base.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000D0B24 File Offset: 0x000CEF24
	public void OnSpawned()
	{
		this.isWarping = false;
		this.WarpSpark.transform.localPosition = this.ShipJumpStartPoint;
		this.ShipPos.position = this.WarpSpark.transform.position;
		F3DTime.time.AddTimer(3f, 1, new Action(this.OnWarp));
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000D0B86 File Offset: 0x000CEF86
	private void OnWarp()
	{
		this.isWarping = true;
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x000D0B90 File Offset: 0x000CEF90
	private void ShiftShipPosition()
	{
		this.WarpSpark.transform.localPosition = Vector3.Lerp(this.WarpSpark.transform.localPosition, this.ShipJumpEndPoint, Time.deltaTime * this.ShipJumpSpeed);
		this.ShipPos.position = this.WarpSpark.transform.position;
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x000D0BEF File Offset: 0x000CEFEF
	private void Update()
	{
		if (this.isWarping)
		{
			this.ShiftShipPosition();
		}
	}

	// Token: 0x04001659 RID: 5721
	public ParticleSystem WarpSpark;

	// Token: 0x0400165A RID: 5722
	public Transform ShipPos;

	// Token: 0x0400165B RID: 5723
	public float ShipJumpSpeed;

	// Token: 0x0400165C RID: 5724
	public Vector3 ShipJumpStartPoint;

	// Token: 0x0400165D RID: 5725
	public Vector3 ShipJumpEndPoint;

	// Token: 0x0400165E RID: 5726
	public bool SendOnSpawned;

	// Token: 0x0400165F RID: 5727
	private bool isWarping;
}
