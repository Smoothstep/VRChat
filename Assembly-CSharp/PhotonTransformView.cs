using System;
using UnityEngine;

// Token: 0x02000779 RID: 1913
[RequireComponent(typeof(PhotonView))]
[AddComponentMenu("Photon Networking/Photon Transform View")]
public class PhotonTransformView : MonoBehaviour, IPunObservable
{
	// Token: 0x06003E71 RID: 15985 RVA: 0x0013A9A4 File Offset: 0x00138DA4
	private void Awake()
	{
		this.m_PhotonView = base.GetComponent<PhotonView>();
		this.m_PositionControl = new PhotonTransformViewPositionControl(this.m_PositionModel);
		this.m_RotationControl = new PhotonTransformViewRotationControl(this.m_RotationModel);
		this.m_ScaleControl = new PhotonTransformViewScaleControl(this.m_ScaleModel);
	}

	// Token: 0x06003E72 RID: 15986 RVA: 0x0013A9F0 File Offset: 0x00138DF0
	private void OnEnable()
	{
		this.m_firstTake = true;
	}

	// Token: 0x06003E73 RID: 15987 RVA: 0x0013A9F9 File Offset: 0x00138DF9
	private void Update()
	{
		if (this.m_PhotonView == null || this.m_PhotonView.isMine || !PhotonNetwork.connected)
		{
			return;
		}
		this.UpdatePosition();
		this.UpdateRotation();
		this.UpdateScale();
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x0013AA39 File Offset: 0x00138E39
	private void UpdatePosition()
	{
		if (!this.m_PositionModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localPosition = this.m_PositionControl.UpdatePosition(base.transform.localPosition);
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x0013AA78 File Offset: 0x00138E78
	private void UpdateRotation()
	{
		if (!this.m_RotationModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localRotation = this.m_RotationControl.GetRotation(base.transform.localRotation);
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x0013AAB7 File Offset: 0x00138EB7
	private void UpdateScale()
	{
		if (!this.m_ScaleModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localScale = this.m_ScaleControl.GetScale(base.transform.localScale);
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x0013AAF6 File Offset: 0x00138EF6
	public void SetSynchronizedValues(Vector3 speed, float turnSpeed)
	{
		this.m_PositionControl.SetSynchronizedValues(speed, turnSpeed);
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x0013AB08 File Offset: 0x00138F08
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		this.m_PositionControl.OnPhotonSerializeView(base.transform.localPosition, stream, info);
		this.m_RotationControl.OnPhotonSerializeView(base.transform.localRotation, stream, info);
		this.m_ScaleControl.OnPhotonSerializeView(base.transform.localScale, stream, info);
		if (!this.m_PhotonView.isMine && this.m_PositionModel.DrawErrorGizmo)
		{
			this.DoDrawEstimatedPositionError();
		}
		if (stream.isReading)
		{
			this.m_ReceivedNetworkUpdate = true;
			if (this.m_firstTake)
			{
				this.m_firstTake = false;
				if (this.m_PositionModel.SynchronizeEnabled)
				{
					base.transform.localPosition = this.m_PositionControl.GetNetworkPosition();
				}
				if (this.m_RotationModel.SynchronizeEnabled)
				{
					base.transform.localRotation = this.m_RotationControl.GetNetworkRotation();
				}
				if (this.m_ScaleModel.SynchronizeEnabled)
				{
					base.transform.localScale = this.m_ScaleControl.GetNetworkScale();
				}
			}
		}
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0013AC1C File Offset: 0x0013901C
	private void DoDrawEstimatedPositionError()
	{
		Vector3 vector = this.m_PositionControl.GetNetworkPosition();
		if (base.transform.parent != null)
		{
			vector = base.transform.parent.position + vector;
		}
		Debug.DrawLine(vector, base.transform.position, Color.red, 2f);
		Debug.DrawLine(base.transform.position, base.transform.position + Vector3.up, Color.green, 2f);
		Debug.DrawLine(vector, vector + Vector3.up, Color.red, 2f);
	}

	// Token: 0x040026F0 RID: 9968
	[SerializeField]
	private PhotonTransformViewPositionModel m_PositionModel = new PhotonTransformViewPositionModel();

	// Token: 0x040026F1 RID: 9969
	[SerializeField]
	private PhotonTransformViewRotationModel m_RotationModel = new PhotonTransformViewRotationModel();

	// Token: 0x040026F2 RID: 9970
	[SerializeField]
	private PhotonTransformViewScaleModel m_ScaleModel = new PhotonTransformViewScaleModel();

	// Token: 0x040026F3 RID: 9971
	private PhotonTransformViewPositionControl m_PositionControl;

	// Token: 0x040026F4 RID: 9972
	private PhotonTransformViewRotationControl m_RotationControl;

	// Token: 0x040026F5 RID: 9973
	private PhotonTransformViewScaleControl m_ScaleControl;

	// Token: 0x040026F6 RID: 9974
	private PhotonView m_PhotonView;

	// Token: 0x040026F7 RID: 9975
	private bool m_ReceivedNetworkUpdate;

	// Token: 0x040026F8 RID: 9976
	private bool m_firstTake;
}
