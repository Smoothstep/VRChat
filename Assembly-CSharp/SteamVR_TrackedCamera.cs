using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

// Token: 0x02000C10 RID: 3088
public class SteamVR_TrackedCamera
{
	// Token: 0x06005F9A RID: 24474 RVA: 0x0021A224 File Offset: 0x00218624
	public static SteamVR_TrackedCamera.VideoStreamTexture Distorted(int deviceIndex = 0)
	{
		if (SteamVR_TrackedCamera.distorted == null)
		{
			SteamVR_TrackedCamera.distorted = new SteamVR_TrackedCamera.VideoStreamTexture[16];
		}
		if (SteamVR_TrackedCamera.distorted[deviceIndex] == null)
		{
			SteamVR_TrackedCamera.distorted[deviceIndex] = new SteamVR_TrackedCamera.VideoStreamTexture((uint)deviceIndex, false);
		}
		return SteamVR_TrackedCamera.distorted[deviceIndex];
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x0021A25E File Offset: 0x0021865E
	public static SteamVR_TrackedCamera.VideoStreamTexture Undistorted(int deviceIndex = 0)
	{
		if (SteamVR_TrackedCamera.undistorted == null)
		{
			SteamVR_TrackedCamera.undistorted = new SteamVR_TrackedCamera.VideoStreamTexture[16];
		}
		if (SteamVR_TrackedCamera.undistorted[deviceIndex] == null)
		{
			SteamVR_TrackedCamera.undistorted[deviceIndex] = new SteamVR_TrackedCamera.VideoStreamTexture((uint)deviceIndex, true);
		}
		return SteamVR_TrackedCamera.undistorted[deviceIndex];
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x0021A298 File Offset: 0x00218698
	public static SteamVR_TrackedCamera.VideoStreamTexture Source(bool undistorted, int deviceIndex = 0)
	{
		return (!undistorted) ? SteamVR_TrackedCamera.Distorted(deviceIndex) : SteamVR_TrackedCamera.Undistorted(deviceIndex);
	}

	// Token: 0x06005F9D RID: 24477 RVA: 0x0021A2B1 File Offset: 0x002186B1
	private static SteamVR_TrackedCamera.VideoStream Stream(uint deviceIndex)
	{
		if (SteamVR_TrackedCamera.videostreams == null)
		{
			SteamVR_TrackedCamera.videostreams = new SteamVR_TrackedCamera.VideoStream[16];
		}
		if (SteamVR_TrackedCamera.videostreams[(int)((UIntPtr)deviceIndex)] == null)
		{
			SteamVR_TrackedCamera.videostreams[(int)((UIntPtr)deviceIndex)] = new SteamVR_TrackedCamera.VideoStream(deviceIndex);
		}
		return SteamVR_TrackedCamera.videostreams[(int)((UIntPtr)deviceIndex)];
	}

	// Token: 0x04004555 RID: 17749
	private static SteamVR_TrackedCamera.VideoStreamTexture[] distorted;

	// Token: 0x04004556 RID: 17750
	private static SteamVR_TrackedCamera.VideoStreamTexture[] undistorted;

	// Token: 0x04004557 RID: 17751
	private static SteamVR_TrackedCamera.VideoStream[] videostreams;

	// Token: 0x02000C11 RID: 3089
	public class VideoStreamTexture
	{
		// Token: 0x06005F9E RID: 24478 RVA: 0x0021A2ED File Offset: 0x002186ED
		public VideoStreamTexture(uint deviceIndex, bool undistorted)
		{
			this.undistorted = undistorted;
			this.videostream = SteamVR_TrackedCamera.Stream(deviceIndex);
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06005F9F RID: 24479 RVA: 0x0021A30F File Offset: 0x0021870F
		// (set) Token: 0x06005FA0 RID: 24480 RVA: 0x0021A317 File Offset: 0x00218717
		public bool undistorted { get; private set; }

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06005FA1 RID: 24481 RVA: 0x0021A320 File Offset: 0x00218720
		public uint deviceIndex
		{
			get
			{
				return this.videostream.deviceIndex;
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06005FA2 RID: 24482 RVA: 0x0021A32D File Offset: 0x0021872D
		public bool hasCamera
		{
			get
			{
				return this.videostream.hasCamera;
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06005FA3 RID: 24483 RVA: 0x0021A33A File Offset: 0x0021873A
		public bool hasTracking
		{
			get
			{
				this.Update();
				return this.header.standingTrackedDevicePose.bPoseIsValid;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06005FA4 RID: 24484 RVA: 0x0021A352 File Offset: 0x00218752
		public uint frameId
		{
			get
			{
				this.Update();
				return this.header.nFrameSequence;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06005FA5 RID: 24485 RVA: 0x0021A365 File Offset: 0x00218765
		// (set) Token: 0x06005FA6 RID: 24486 RVA: 0x0021A36D File Offset: 0x0021876D
		public VRTextureBounds_t frameBounds { get; private set; }

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06005FA7 RID: 24487 RVA: 0x0021A376 File Offset: 0x00218776
		public EVRTrackedCameraFrameType frameType
		{
			get
			{
				return (!this.undistorted) ? EVRTrackedCameraFrameType.Distorted : EVRTrackedCameraFrameType.Undistorted;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06005FA8 RID: 24488 RVA: 0x0021A38A File Offset: 0x0021878A
		public Texture2D texture
		{
			get
			{
				this.Update();
				return this._texture;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06005FA9 RID: 24489 RVA: 0x0021A398 File Offset: 0x00218798
		public SteamVR_Utils.RigidTransform transform
		{
			get
			{
				this.Update();
				return new SteamVR_Utils.RigidTransform(this.header.standingTrackedDevicePose.mDeviceToAbsoluteTracking);
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06005FAA RID: 24490 RVA: 0x0021A3B8 File Offset: 0x002187B8
		public Vector3 velocity
		{
			get
			{
				this.Update();
				TrackedDevicePose_t standingTrackedDevicePose = this.header.standingTrackedDevicePose;
				return new Vector3(standingTrackedDevicePose.vVelocity.v0, standingTrackedDevicePose.vVelocity.v1, -standingTrackedDevicePose.vVelocity.v2);
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06005FAB RID: 24491 RVA: 0x0021A404 File Offset: 0x00218804
		public Vector3 angularVelocity
		{
			get
			{
				this.Update();
				TrackedDevicePose_t standingTrackedDevicePose = this.header.standingTrackedDevicePose;
				return new Vector3(-standingTrackedDevicePose.vAngularVelocity.v0, -standingTrackedDevicePose.vAngularVelocity.v1, standingTrackedDevicePose.vAngularVelocity.v2);
			}
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x0021A44E File Offset: 0x0021884E
		public TrackedDevicePose_t GetPose()
		{
			this.Update();
			return this.header.standingTrackedDevicePose;
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x0021A461 File Offset: 0x00218861
		public ulong Acquire()
		{
			return this.videostream.Acquire();
		}

		// Token: 0x06005FAE RID: 24494 RVA: 0x0021A470 File Offset: 0x00218870
		public ulong Release()
		{
			ulong result = this.videostream.Release();
			if (this.videostream.handle == 0UL)
			{
				UnityEngine.Object.Destroy(this._texture);
				this._texture = null;
			}
			return result;
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x0021A4B0 File Offset: 0x002188B0
		private void Update()
		{
			if (Time.frameCount == this.prevFrameCount)
			{
				return;
			}
			this.prevFrameCount = Time.frameCount;
			if (this.videostream.handle == 0UL)
			{
				return;
			}
			SteamVR instance = SteamVR.instance;
			if (instance == null)
			{
				return;
			}
			CVRTrackedCamera trackedCamera = OpenVR.TrackedCamera;
			if (trackedCamera == null)
			{
				return;
			}
			IntPtr nativeTex = IntPtr.Zero;
			Texture2D texture2D = (!(this._texture != null)) ? new Texture2D(2, 2) : this._texture;
			uint nFrameHeaderSize = (uint)Marshal.SizeOf(this.header.GetType());
			if (instance.textureType == ETextureType.OpenGL)
			{
				if (this.glTextureId != 0u)
				{
					trackedCamera.ReleaseVideoStreamTextureGL(this.videostream.handle, this.glTextureId);
				}
				if (trackedCamera.GetVideoStreamTextureGL(this.videostream.handle, this.frameType, ref this.glTextureId, ref this.header, nFrameHeaderSize) != EVRTrackedCameraError.None)
				{
					return;
				}
				nativeTex = (IntPtr)((long)((ulong)this.glTextureId));
			}
			else if (instance.textureType == ETextureType.DirectX && trackedCamera.GetVideoStreamTextureD3D11(this.videostream.handle, this.frameType, texture2D.GetNativeTexturePtr(), ref nativeTex, ref this.header, nFrameHeaderSize) != EVRTrackedCameraError.None)
			{
				return;
			}
			if (this._texture == null)
			{
				this._texture = Texture2D.CreateExternalTexture((int)this.header.nWidth, (int)this.header.nHeight, TextureFormat.RGBA32, false, false, nativeTex);
				uint num = 0u;
				uint num2 = 0u;
				VRTextureBounds_t frameBounds = default(VRTextureBounds_t);
				if (trackedCamera.GetVideoStreamTextureSize(this.deviceIndex, this.frameType, ref frameBounds, ref num, ref num2) == EVRTrackedCameraError.None)
				{
					frameBounds.vMin = 1f - frameBounds.vMin;
					frameBounds.vMax = 1f - frameBounds.vMax;
					this.frameBounds = frameBounds;
				}
			}
			else
			{
				this._texture.UpdateExternalTexture(nativeTex);
			}
		}

		// Token: 0x0400455A RID: 17754
		private Texture2D _texture;

		// Token: 0x0400455B RID: 17755
		private int prevFrameCount = -1;

		// Token: 0x0400455C RID: 17756
		private uint glTextureId;

		// Token: 0x0400455D RID: 17757
		private SteamVR_TrackedCamera.VideoStream videostream;

		// Token: 0x0400455E RID: 17758
		private CameraVideoStreamFrameHeader_t header;
	}

	// Token: 0x02000C12 RID: 3090
	private class VideoStream
	{
		// Token: 0x06005FB0 RID: 24496 RVA: 0x0021A690 File Offset: 0x00218A90
		public VideoStream(uint deviceIndex)
		{
			this.deviceIndex = deviceIndex;
			CVRTrackedCamera trackedCamera = OpenVR.TrackedCamera;
			if (trackedCamera != null)
			{
				trackedCamera.HasCamera(deviceIndex, ref this._hasCamera);
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06005FB1 RID: 24497 RVA: 0x0021A6C4 File Offset: 0x00218AC4
		// (set) Token: 0x06005FB2 RID: 24498 RVA: 0x0021A6CC File Offset: 0x00218ACC
		public uint deviceIndex { get; private set; }

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06005FB3 RID: 24499 RVA: 0x0021A6D5 File Offset: 0x00218AD5
		public ulong handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06005FB4 RID: 24500 RVA: 0x0021A6DD File Offset: 0x00218ADD
		public bool hasCamera
		{
			get
			{
				return this._hasCamera;
			}
		}

		// Token: 0x06005FB5 RID: 24501 RVA: 0x0021A6E8 File Offset: 0x00218AE8
		public ulong Acquire()
		{
			if (this._handle == 0UL && this.hasCamera)
			{
				CVRTrackedCamera trackedCamera = OpenVR.TrackedCamera;
				if (trackedCamera != null)
				{
					trackedCamera.AcquireVideoStreamingService(this.deviceIndex, ref this._handle);
				}
			}
			return this.refCount += 1UL;
		}

		// Token: 0x06005FB6 RID: 24502 RVA: 0x0021A740 File Offset: 0x00218B40
		public ulong Release()
		{
			if (this.refCount > 0UL && (this.refCount -= 1UL) == 0UL && this._handle != 0UL)
			{
				CVRTrackedCamera trackedCamera = OpenVR.TrackedCamera;
				if (trackedCamera != null)
				{
					trackedCamera.ReleaseVideoStreamingService(this._handle);
				}
				this._handle = 0UL;
			}
			return this.refCount;
		}

		// Token: 0x04004560 RID: 17760
		private ulong _handle;

		// Token: 0x04004561 RID: 17761
		private bool _hasCamera;

		// Token: 0x04004562 RID: 17762
		private ulong refCount;
	}
}
