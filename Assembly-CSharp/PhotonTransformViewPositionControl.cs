using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200077A RID: 1914
public class PhotonTransformViewPositionControl
{
	// Token: 0x06003E7A RID: 15994 RVA: 0x0013ACC7 File Offset: 0x001390C7
	public PhotonTransformViewPositionControl(PhotonTransformViewPositionModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x0013ACF4 File Offset: 0x001390F4
	private Vector3 GetOldestStoredNetworkPosition()
	{
		Vector3 result = this.m_NetworkPosition;
		if (this.m_OldNetworkPositions.Count > 0)
		{
			result = this.m_OldNetworkPositions.Peek();
		}
		return result;
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x0013AD26 File Offset: 0x00139126
	public void SetSynchronizedValues(Vector3 speed, float turnSpeed)
	{
		this.m_SynchronizedSpeed = speed;
		this.m_SynchronizedTurnSpeed = turnSpeed;
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x0013AD38 File Offset: 0x00139138
	public Vector3 UpdatePosition(Vector3 currentPosition)
	{
		Vector3 vector = this.GetNetworkPosition() + this.GetExtrapolatedPositionOffset();
		switch (this.m_Model.InterpolateOption)
		{
		case PhotonTransformViewPositionModel.InterpolateOptions.Disabled:
			if (!this.m_UpdatedPositionAfterOnSerialize)
			{
				currentPosition = vector;
				this.m_UpdatedPositionAfterOnSerialize = true;
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.FixedSpeed:
			currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * this.m_Model.InterpolateMoveTowardsSpeed);
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed:
			if (this.m_OldNetworkPositions.Count != 0)
			{
				float num = Vector3.Distance(this.m_NetworkPosition, this.GetOldestStoredNetworkPosition()) / (float)this.m_OldNetworkPositions.Count * (float)PhotonNetwork.sendRateOnSerialize;
				currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * num);
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues:
			if (this.m_SynchronizedSpeed.magnitude == 0f)
			{
				currentPosition = vector;
			}
			else
			{
				currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * this.m_SynchronizedSpeed.magnitude);
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.Lerp:
			currentPosition = Vector3.Lerp(currentPosition, vector, Time.deltaTime * this.m_Model.InterpolateLerpSpeed);
			break;
		}
		if (this.m_Model.TeleportEnabled && Vector3.Distance(currentPosition, this.GetNetworkPosition()) > this.m_Model.TeleportIfDistanceGreaterThan)
		{
			currentPosition = this.GetNetworkPosition();
		}
		return currentPosition;
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0013AE9B File Offset: 0x0013929B
	public Vector3 GetNetworkPosition()
	{
		return this.m_NetworkPosition;
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0013AEA4 File Offset: 0x001392A4
	public Vector3 GetExtrapolatedPositionOffset()
	{
		float num = (float)(PhotonNetwork.time - this.m_LastSerializeTime);
		if (this.m_Model.ExtrapolateIncludingRoundTripTime)
		{
			num += (float)PhotonNetwork.GetPing() / 1000f;
		}
		Vector3 result = Vector3.zero;
		PhotonTransformViewPositionModel.ExtrapolateOptions extrapolateOption = this.m_Model.ExtrapolateOption;
		if (extrapolateOption != PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues)
		{
			if (extrapolateOption != PhotonTransformViewPositionModel.ExtrapolateOptions.FixedSpeed)
			{
				if (extrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.EstimateSpeedAndTurn)
				{
					Vector3 a = (this.m_NetworkPosition - this.GetOldestStoredNetworkPosition()) * (float)PhotonNetwork.sendRateOnSerialize;
					result = a * num;
				}
			}
			else
			{
				Vector3 normalized = (this.m_NetworkPosition - this.GetOldestStoredNetworkPosition()).normalized;
				result = normalized * this.m_Model.ExtrapolateSpeed * num;
			}
		}
		else
		{
			Quaternion rotation = Quaternion.Euler(0f, this.m_SynchronizedTurnSpeed * num, 0f);
			result = rotation * (this.m_SynchronizedSpeed * num);
		}
		return result;
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x0013AFA4 File Offset: 0x001393A4
	public void OnPhotonSerializeView(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			this.SerializeData(currentPosition, stream, info);
		}
		else
		{
			this.DeserializeData(stream, info);
		}
		this.m_LastSerializeTime = PhotonNetwork.time;
		this.m_UpdatedPositionAfterOnSerialize = false;
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x0013AFF8 File Offset: 0x001393F8
	private void SerializeData(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(currentPosition);
		this.m_NetworkPosition = currentPosition;
		if (this.m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || this.m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			stream.SendNext(this.m_SynchronizedSpeed);
			stream.SendNext(this.m_SynchronizedTurnSpeed);
		}
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0013B05C File Offset: 0x0013945C
	private void DeserializeData(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 networkPosition = (Vector3)stream.ReceiveNext();
		if (this.m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || this.m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			this.m_SynchronizedSpeed = (Vector3)stream.ReceiveNext();
			this.m_SynchronizedTurnSpeed = (float)stream.ReceiveNext();
		}
		if (this.m_OldNetworkPositions.Count == 0)
		{
			this.m_NetworkPosition = networkPosition;
		}
		this.m_OldNetworkPositions.Enqueue(this.m_NetworkPosition);
		this.m_NetworkPosition = networkPosition;
		while (this.m_OldNetworkPositions.Count > this.m_Model.ExtrapolateNumberOfStoredPositions)
		{
			this.m_OldNetworkPositions.Dequeue();
		}
	}

	// Token: 0x040026F9 RID: 9977
	private PhotonTransformViewPositionModel m_Model;

	// Token: 0x040026FA RID: 9978
	private float m_CurrentSpeed;

	// Token: 0x040026FB RID: 9979
	private double m_LastSerializeTime;

	// Token: 0x040026FC RID: 9980
	private Vector3 m_SynchronizedSpeed = Vector3.zero;

	// Token: 0x040026FD RID: 9981
	private float m_SynchronizedTurnSpeed;

	// Token: 0x040026FE RID: 9982
	private Vector3 m_NetworkPosition;

	// Token: 0x040026FF RID: 9983
	private Queue<Vector3> m_OldNetworkPositions = new Queue<Vector3>();

	// Token: 0x04002700 RID: 9984
	private bool m_UpdatedPositionAfterOnSerialize = true;
}
