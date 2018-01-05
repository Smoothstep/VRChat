using System;
using UnityEngine;

// Token: 0x02000781 RID: 1921
public class PhotonTransformViewScaleControl
{
	// Token: 0x06003E89 RID: 16009 RVA: 0x0013B333 File Offset: 0x00139733
	public PhotonTransformViewScaleControl(PhotonTransformViewScaleModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x0013B34D File Offset: 0x0013974D
	public Vector3 GetNetworkScale()
	{
		return this.m_NetworkScale;
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x0013B358 File Offset: 0x00139758
	public Vector3 GetScale(Vector3 currentScale)
	{
		switch (this.m_Model.InterpolateOption)
		{
		default:
			return this.m_NetworkScale;
		case PhotonTransformViewScaleModel.InterpolateOptions.MoveTowards:
			return Vector3.MoveTowards(currentScale, this.m_NetworkScale, this.m_Model.InterpolateMoveTowardsSpeed * Time.deltaTime);
		case PhotonTransformViewScaleModel.InterpolateOptions.Lerp:
			return Vector3.Lerp(currentScale, this.m_NetworkScale, this.m_Model.InterpolateLerpSpeed * Time.deltaTime);
		}
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x0013B3CC File Offset: 0x001397CC
	public void OnPhotonSerializeView(Vector3 currentScale, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			stream.SendNext(currentScale);
			this.m_NetworkScale = currentScale;
		}
		else
		{
			this.m_NetworkScale = (Vector3)stream.ReceiveNext();
		}
	}

	// Token: 0x04002724 RID: 10020
	private PhotonTransformViewScaleModel m_Model;

	// Token: 0x04002725 RID: 10021
	private Vector3 m_NetworkScale = Vector3.one;
}
