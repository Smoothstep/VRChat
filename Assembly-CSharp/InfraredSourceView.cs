using System;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class InfraredSourceView : MonoBehaviour
{
	// Token: 0x06002A0E RID: 10766 RVA: 0x000D6677 File Offset: 0x000D4A77
	private void Start()
	{
		base.gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1f, 1f));
	}

	// Token: 0x06002A0F RID: 10767 RVA: 0x000D66A4 File Offset: 0x000D4AA4
	private void Update()
	{
		if (this.InfraredSourceManager == null)
		{
			return;
		}
		this._InfraredManager = this.InfraredSourceManager.GetComponent<InfraredSourceManager>();
		if (this._InfraredManager == null)
		{
			return;
		}
		base.gameObject.GetComponent<Renderer>().material.mainTexture = this._InfraredManager.GetInfraredTexture();
	}

	// Token: 0x040016F8 RID: 5880
	public GameObject InfraredSourceManager;

	// Token: 0x040016F9 RID: 5881
	private InfraredSourceManager _InfraredManager;
}
