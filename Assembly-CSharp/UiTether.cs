using System;
using UnityEngine;

// Token: 0x02000B06 RID: 2822
public class UiTether : MonoBehaviour
{
	// Token: 0x06005582 RID: 21890 RVA: 0x001D81D9 File Offset: 0x001D65D9
	private void Awake()
	{
		this._renderer = base.GetComponentInChildren<Renderer>();
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x001D81E8 File Offset: 0x001D65E8
	private void Update()
	{
		if (this.Source == null || this.Target == null || this._renderer == null)
		{
			return;
		}
		Vector3 vector = this.Target.position;
		Collider component = this.Target.GetComponent<Collider>();
		if (component != null)
		{
			vector = PhysicsUtil.ClosestPointOnCollider(component, this.Source.position);
		}
		base.transform.LookAt(vector);
		float magnitude = (vector - this.Source.position).magnitude;
		this.Source.localScale = new Vector3(magnitude, 1f, 1f);
		this._renderer.material.mainTextureScale = new Vector2(magnitude * this.TextureScale, 1f);
		this._renderer.material.mainTextureOffset += new Vector2(Time.deltaTime * this.ScrollSpeed, 0f);
	}

	// Token: 0x04003C68 RID: 15464
	public Transform Source;

	// Token: 0x04003C69 RID: 15465
	public Transform Target;

	// Token: 0x04003C6A RID: 15466
	public float ScrollSpeed = 1f;

	// Token: 0x04003C6B RID: 15467
	public float TextureScale = 10f;

	// Token: 0x04003C6C RID: 15468
	private Renderer _renderer;

	// Token: 0x04003C6D RID: 15469
	private Transform targetMarker;
}
