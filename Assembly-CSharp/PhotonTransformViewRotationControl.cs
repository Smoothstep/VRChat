using System;
using UnityEngine;

// Token: 0x0200077E RID: 1918
public class PhotonTransformViewRotationControl
{
	// Token: 0x06003E84 RID: 16004 RVA: 0x0013B231 File Offset: 0x00139631
	public PhotonTransformViewRotationControl(PhotonTransformViewRotationModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0013B240 File Offset: 0x00139640
	public Quaternion GetNetworkRotation()
	{
		return this.m_NetworkRotation;
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0013B248 File Offset: 0x00139648
	public Quaternion GetRotation(Quaternion currentRotation)
	{
		switch (this.m_Model.InterpolateOption)
		{
		default:
			return this.m_NetworkRotation;
		case PhotonTransformViewRotationModel.InterpolateOptions.RotateTowards:
			return Quaternion.RotateTowards(currentRotation, this.m_NetworkRotation, this.m_Model.InterpolateRotateTowardsSpeed * Time.deltaTime);
		case PhotonTransformViewRotationModel.InterpolateOptions.Lerp:
			return Quaternion.Lerp(currentRotation, this.m_NetworkRotation, this.m_Model.InterpolateLerpSpeed * Time.deltaTime);
		}
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x0013B2BC File Offset: 0x001396BC
	public void OnPhotonSerializeView(Quaternion currentRotation, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			stream.SendNext(currentRotation);
			this.m_NetworkRotation = currentRotation;
		}
		else
		{
			this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();
		}
	}

	// Token: 0x0400271A RID: 10010
	private PhotonTransformViewRotationModel m_Model;

	// Token: 0x0400271B RID: 10011
	private Quaternion m_NetworkRotation;
}
