using System;
using UnityEngine;
using Windows.Kinect;

// Token: 0x020004B0 RID: 1200
public class InfraredSourceManager : MonoBehaviour
{
	// Token: 0x06002A09 RID: 10761 RVA: 0x000D64A2 File Offset: 0x000D48A2
	public Texture2D GetInfraredTexture()
	{
		return this._Texture;
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x000D64AC File Offset: 0x000D48AC
	private void Start()
	{
		this._Sensor = KinectSensor.GetDefault();
		if (this._Sensor != null)
		{
			this._Reader = this._Sensor.InfraredFrameSource.OpenReader();
			FrameDescription frameDescription = this._Sensor.InfraredFrameSource.FrameDescription;
			this._Data = new ushort[frameDescription.LengthInPixels];
			this._RawData = new byte[frameDescription.LengthInPixels * 4u];
			this._Texture = new Texture2D(frameDescription.Width, frameDescription.Height, TextureFormat.BGRA32, false);
			if (!this._Sensor.IsOpen)
			{
				this._Sensor.Open();
			}
		}
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x000D6554 File Offset: 0x000D4954
	private void Update()
	{
		if (this._Reader != null)
		{
			InfraredFrame infraredFrame = this._Reader.AcquireLatestFrame();
			if (infraredFrame != null)
			{
				infraredFrame.CopyFrameDataToArray(this._Data);
				int num = 0;
				foreach (ushort num2 in this._Data)
				{
					byte b = (byte)(num2 >> 8);
					this._RawData[num++] = b;
					this._RawData[num++] = b;
					this._RawData[num++] = b;
					this._RawData[num++] = byte.MaxValue;
				}
				this._Texture.LoadRawTextureData(this._RawData);
				this._Texture.Apply();
				infraredFrame.Dispose();
			}
		}
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x000D6618 File Offset: 0x000D4A18
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

	// Token: 0x040016F3 RID: 5875
	private KinectSensor _Sensor;

	// Token: 0x040016F4 RID: 5876
	private InfraredFrameReader _Reader;

	// Token: 0x040016F5 RID: 5877
	private ushort[] _Data;

	// Token: 0x040016F6 RID: 5878
	private byte[] _RawData;

	// Token: 0x040016F7 RID: 5879
	private Texture2D _Texture;
}
