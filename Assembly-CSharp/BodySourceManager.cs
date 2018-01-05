using System;
using UnityEngine;
using Windows.Kinect;

// Token: 0x020004A8 RID: 1192
public class BodySourceManager : MonoBehaviour
{
	// Token: 0x060029E1 RID: 10721 RVA: 0x000D5506 File Offset: 0x000D3906
	public Body[] GetData()
	{
		return this._Data;
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x000D5510 File Offset: 0x000D3910
	private void Start()
	{
		this._Sensor = KinectSensor.GetDefault();
		if (this._Sensor != null)
		{
			this._Reader = this._Sensor.BodyFrameSource.OpenReader();
			if (!this._Sensor.IsOpen)
			{
				this._Sensor.Open();
			}
		}
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x000D5564 File Offset: 0x000D3964
	public void Refresh()
	{
		if (this._Reader != null)
		{
			BodyFrame bodyFrame = this._Reader.AcquireLatestFrame();
			if (bodyFrame != null)
			{
				if (this._Data == null)
				{
					this._Data = new Body[this._Sensor.BodyFrameSource.BodyCount];
				}
				bodyFrame.GetAndRefreshBodyData(this._Data);
				this.FloorClipPlane = bodyFrame.FloorClipPlane;
				bodyFrame.Dispose();
			}
		}
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000D55D4 File Offset: 0x000D39D4
	private void ShutDown()
	{
		if (this._Reader != null)
		{
			this._Reader.Dispose();
			this._Reader = null;
		}
		if (this._Sensor != null)
		{
			if (this._Sensor.IsOpen)
			{
				this._Sensor.Close();
			}
			this._Sensor = null;
		}
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x000D562B File Offset: 0x000D3A2B
	private void OnDestroy()
	{
		this.ShutDown();
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x000D5633 File Offset: 0x000D3A33
	private void OnApplicationQuit()
	{
		this.ShutDown();
	}

	// Token: 0x040016CC RID: 5836
	private KinectSensor _Sensor;

	// Token: 0x040016CD RID: 5837
	private BodyFrameReader _Reader;

	// Token: 0x040016CE RID: 5838
	private Body[] _Data;

	// Token: 0x040016CF RID: 5839
	public Windows.Kinect.Vector4 FloorClipPlane = default(Windows.Kinect.Vector4);
}
