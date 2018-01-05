using System;
using UnityEngine;
using Windows.Kinect;

// Token: 0x020004AA RID: 1194
public class ColorSourceManager : MonoBehaviour
{
	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x060029EE RID: 10734 RVA: 0x000D5B25 File Offset: 0x000D3F25
	// (set) Token: 0x060029EF RID: 10735 RVA: 0x000D5B2D File Offset: 0x000D3F2D
	public int ColorWidth { get; private set; }

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x060029F0 RID: 10736 RVA: 0x000D5B36 File Offset: 0x000D3F36
	// (set) Token: 0x060029F1 RID: 10737 RVA: 0x000D5B3E File Offset: 0x000D3F3E
	public int ColorHeight { get; private set; }

	// Token: 0x060029F2 RID: 10738 RVA: 0x000D5B47 File Offset: 0x000D3F47
	public Texture2D GetColorTexture()
	{
		return this._Texture;
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x000D5B50 File Offset: 0x000D3F50
	private void Start()
	{
		this._Sensor = KinectSensor.GetDefault();
		if (this._Sensor != null)
		{
			this._Reader = this._Sensor.ColorFrameSource.OpenReader();
			FrameDescription frameDescription = this._Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
			this.ColorWidth = frameDescription.Width;
			this.ColorHeight = frameDescription.Height;
			this._Texture = new Texture2D(frameDescription.Width, frameDescription.Height, TextureFormat.RGBA32, false);
			this._Data = new byte[frameDescription.BytesPerPixel * frameDescription.LengthInPixels];
			if (!this._Sensor.IsOpen)
			{
				this._Sensor.Open();
			}
		}
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x000D5C00 File Offset: 0x000D4000
	private void Update()
	{
		if (this._Reader != null)
		{
			ColorFrame colorFrame = this._Reader.AcquireLatestFrame();
			if (colorFrame != null)
			{
				colorFrame.CopyConvertedFrameDataToArray(this._Data, ColorImageFormat.Rgba);
				this._Texture.LoadRawTextureData(this._Data);
				this._Texture.Apply();
				colorFrame.Dispose();
			}
		}
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x000D5C5C File Offset: 0x000D405C
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

	// Token: 0x040016D7 RID: 5847
	private KinectSensor _Sensor;

	// Token: 0x040016D8 RID: 5848
	private ColorFrameReader _Reader;

	// Token: 0x040016D9 RID: 5849
	private Texture2D _Texture;

	// Token: 0x040016DA RID: 5850
	private byte[] _Data;
}
