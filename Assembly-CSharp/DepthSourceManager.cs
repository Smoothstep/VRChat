using System;
using UnityEngine;
using Windows.Kinect;

// Token: 0x020004AC RID: 1196
public class DepthSourceManager : MonoBehaviour
{
	// Token: 0x060029FA RID: 10746 RVA: 0x000D5D52 File Offset: 0x000D4152
	public ushort[] GetData()
	{
		return this._Data;
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000D5D5C File Offset: 0x000D415C
	private void Start()
	{
		this._Sensor = KinectSensor.GetDefault();
		if (this._Sensor != null)
		{
			this._Reader = this._Sensor.DepthFrameSource.OpenReader();
			this._Data = new ushort[this._Sensor.DepthFrameSource.FrameDescription.LengthInPixels];
		}
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x000D5DB8 File Offset: 0x000D41B8
	private void Update()
	{
		if (this._Reader != null)
		{
			DepthFrame depthFrame = this._Reader.AcquireLatestFrame();
			if (depthFrame != null)
			{
				depthFrame.CopyFrameDataToArray(this._Data);
				depthFrame.Dispose();
			}
		}
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x000D5DF8 File Offset: 0x000D41F8
	private void OnApplicationQuit()
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

	// Token: 0x040016DD RID: 5853
	private KinectSensor _Sensor;

	// Token: 0x040016DE RID: 5854
	private DepthFrameReader _Reader;

	// Token: 0x040016DF RID: 5855
	private ushort[] _Data;
}
