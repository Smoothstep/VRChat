using System;
using UnityEngine;

// Token: 0x0200046B RID: 1131
[RequireComponent(typeof(Camera))]
public class FlipCameraUpSideDown : MonoBehaviour
{
	// Token: 0x06002761 RID: 10081 RVA: 0x000CAFD6 File Offset: 0x000C93D6
	private void OnEnable()
	{
		this.cam = base.GetComponent<Camera>();
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x000CAFE4 File Offset: 0x000C93E4
	private void OnDisable()
	{
		this.ResetCamera();
	}

	// Token: 0x06002763 RID: 10083 RVA: 0x000CAFEC File Offset: 0x000C93EC
	private void OnPreCull()
	{
		this.ResetCamera();
		this.cam.projectionMatrix = this.cam.projectionMatrix * Matrix4x4.Scale(new Vector3(1f, -1f, 1f));
	}

	// Token: 0x06002764 RID: 10084 RVA: 0x000CB028 File Offset: 0x000C9428
	private void OnPreRender()
	{
		GL.invertCulling = true;
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x000CB030 File Offset: 0x000C9430
	private void OnPostRender()
	{
		GL.invertCulling = false;
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x000CB038 File Offset: 0x000C9438
	private void ResetCamera()
	{
		this.cam.ResetWorldToCameraMatrix();
		this.cam.ResetProjectionMatrix();
	}

	// Token: 0x04001522 RID: 5410
	private Camera cam;
}
