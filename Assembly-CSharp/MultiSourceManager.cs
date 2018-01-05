using System;
using UnityEngine;
using Windows.Kinect;

// Token: 0x020004B2 RID: 1202
public class MultiSourceManager : MonoBehaviour
{
	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06002A11 RID: 10769 RVA: 0x000D670E File Offset: 0x000D4B0E
	// (set) Token: 0x06002A12 RID: 10770 RVA: 0x000D6716 File Offset: 0x000D4B16
	public int ColorWidth { get; private set; }

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06002A13 RID: 10771 RVA: 0x000D671F File Offset: 0x000D4B1F
	// (set) Token: 0x06002A14 RID: 10772 RVA: 0x000D6727 File Offset: 0x000D4B27
	public int ColorHeight { get; private set; }

	// Token: 0x06002A15 RID: 10773 RVA: 0x000D6730 File Offset: 0x000D4B30
	public Texture2D GetColorTexture()
	{
		return this._ColorTexture;
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x000D6738 File Offset: 0x000D4B38
	public ushort[] GetDepthData()
	{
		return this._DepthData;
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x000D6740 File Offset: 0x000D4B40
	private void Start()
	{
		this._Sensor = KinectSensor.GetDefault();
		if (this._Sensor != null)
		{
			this._Reader = this._Sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth);
			FrameDescription frameDescription = this._Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
			this.ColorWidth = frameDescription.Width;
			this.ColorHeight = frameDescription.Height;
			this._ColorTexture = new Texture2D(frameDescription.Width, frameDescription.Height, TextureFormat.RGBA32, false);
			this._ColorData = new byte[frameDescription.BytesPerPixel * frameDescription.LengthInPixels];
			FrameDescription frameDescription2 = this._Sensor.DepthFrameSource.FrameDescription;
			this._DepthData = new ushort[frameDescription2.LengthInPixels];
			if (!this._Sensor.IsOpen)
			{
				this._Sensor.Open();
			}
		}
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x000D6810 File Offset: 0x000D4C10
	private void Update()
	{
		if (this._Reader != null)
		{
			MultiSourceFrame multiSourceFrame = this._Reader.AcquireLatestFrame();
			if (multiSourceFrame != null)
			{
				ColorFrame colorFrame = multiSourceFrame.ColorFrameReference.AcquireFrame();
				if (colorFrame != null)
				{
					DepthFrame depthFrame = multiSourceFrame.DepthFrameReference.AcquireFrame();
					if (depthFrame != null)
					{
						colorFrame.CopyConvertedFrameDataToArray(this._ColorData, ColorImageFormat.Rgba);
						this._ColorTexture.LoadRawTextureData(this._ColorData);
						this._ColorTexture.Apply();
						depthFrame.CopyFrameDataToArray(this._DepthData);
						depthFrame.Dispose();
					}
					colorFrame.Dispose();
				}
			}
		}
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x000D68A8 File Offset: 0x000D4CA8
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

	// Token: 0x040016FC RID: 5884
	private KinectSensor _Sensor;

	// Token: 0x040016FD RID: 5885
	private MultiSourceFrameReader _Reader;

	// Token: 0x040016FE RID: 5886
	private Texture2D _ColorTexture;

	// Token: 0x040016FF RID: 5887
	private ushort[] _DepthData;

	// Token: 0x04001700 RID: 5888
	private byte[] _ColorData;
}
