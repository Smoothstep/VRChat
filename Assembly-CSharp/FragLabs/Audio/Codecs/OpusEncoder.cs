using System;
using FragLabs.Audio.Codecs.Opus;
using MoPhoGames.USpeak.Core.Utils;

namespace FragLabs.Audio.Codecs
{
	// Token: 0x0200070E RID: 1806
	public class OpusEncoder : IDisposable
	{
		// Token: 0x06003B03 RID: 15107 RVA: 0x001298B1 File Offset: 0x00127CB1
		private OpusEncoder(IntPtr encoder, int inputSamplingRate, int inputChannels, Application application)
		{
			this._encoder = encoder;
			this.InputSamplingRate = inputSamplingRate;
			this.InputChannels = inputChannels;
			this.Application = application;
			this.MaxDataBytes = 4000;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x001298E4 File Offset: 0x00127CE4
		public static OpusEncoder Create(int inputSamplingRate, int inputChannels, Application application)
		{
			if (inputSamplingRate != 8000 && inputSamplingRate != 12000 && inputSamplingRate != 16000 && inputSamplingRate != 24000 && inputSamplingRate != 48000)
			{
				throw new ArgumentOutOfRangeException("inputSamplingRate");
			}
			if (inputChannels != 1 && inputChannels != 2)
			{
				throw new ArgumentOutOfRangeException("inputChannels");
			}
			IntPtr value;
			IntPtr encoder = API.opus_encoder_create(inputSamplingRate, inputChannels, (int)application, out value);
			if ((int)value != 0)
			{
				throw new Exception("Exception occured while creating encoder");
			}
			return new OpusEncoder(encoder, inputSamplingRate, inputChannels, application);
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x00129978 File Offset: 0x00127D78
		public unsafe byte[] Encode(byte[] inputPcmSamples, int sampleLength, out int encodedLength)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("OpusEncoder");
			}
			int frame_size = this.FrameCount(inputPcmSamples);
			byte[] @byte = USpeakPoolUtils.GetByte(this.MaxDataBytes);
			int num;
			fixed (byte* value = (@byte != null && @byte.Length != 0) ? @byte : null)
			{
				IntPtr data = new IntPtr((void*)value);
				num = API.opus_encode(this._encoder, inputPcmSamples, frame_size, data, sampleLength);
			}
			encodedLength = num;
			if (num < 0)
			{
				USpeakPoolUtils.Return(@byte);
				string str = "Encoding failed - ";
				Errors errors = (Errors)num;
				throw new Exception(str + errors.ToString());
			}
			byte[] byte2 = USpeakPoolUtils.GetByte(encodedLength);
			Buffer.BlockCopy(@byte, 0, byte2, 0, encodedLength);
			USpeakPoolUtils.Return(@byte);
			return byte2;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x00129A39 File Offset: 0x00127E39
		public void ResetState()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("OpusEncoder");
			}
			API.opus_encoder_ctl(this._encoder, Ctl.OpusResetState, 0);
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x00129A64 File Offset: 0x00127E64
		public int FrameCount(byte[] pcmSamples)
		{
			int num = 16;
			int num2 = num / 8 * this.InputChannels;
			return pcmSamples.Length / num2;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x00129A84 File Offset: 0x00127E84
		public int FrameByteCount(int frameCount)
		{
			int num = 16;
			int num2 = num / 8 * this.InputChannels;
			return frameCount * num2;
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003B09 RID: 15113 RVA: 0x00129AA2 File Offset: 0x00127EA2
		// (set) Token: 0x06003B0A RID: 15114 RVA: 0x00129AAA File Offset: 0x00127EAA
		public int InputSamplingRate { get; private set; }

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x00129AB3 File Offset: 0x00127EB3
		// (set) Token: 0x06003B0C RID: 15116 RVA: 0x00129ABB File Offset: 0x00127EBB
		public int InputChannels { get; private set; }

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x00129AC4 File Offset: 0x00127EC4
		// (set) Token: 0x06003B0E RID: 15118 RVA: 0x00129ACC File Offset: 0x00127ECC
		public Application Application { get; private set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x00129AD5 File Offset: 0x00127ED5
		// (set) Token: 0x06003B10 RID: 15120 RVA: 0x00129ADD File Offset: 0x00127EDD
		public int MaxDataBytes { get; set; }

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x00129AE8 File Offset: 0x00127EE8
		// (set) Token: 0x06003B12 RID: 15122 RVA: 0x00129B48 File Offset: 0x00127F48
		public int Bitrate
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int result;
				int num = API.opus_encoder_ctl(this._encoder, Ctl.GetBitrateRequest, out result);
				if (num < 0)
				{
					string str = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(str + errors.ToString());
				}
				return result;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int num = API.opus_encoder_ctl(this._encoder, Ctl.SetBitrateRequest, value);
				if (num < 0)
				{
					string str = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(str + errors.ToString());
				}
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x00129BA4 File Offset: 0x00127FA4
		// (set) Token: 0x06003B14 RID: 15124 RVA: 0x00129C10 File Offset: 0x00128010
		public bool ForwardErrorCorrection
		{
			get
			{
				if (this._encoder == IntPtr.Zero)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int num2;
				int num = API.opus_encoder_ctl(this._encoder, Ctl.GetInbandFECRequest, out num2);
				if (num < 0)
				{
					string str = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(str + errors.ToString());
				}
				return num2 > 0;
			}
			set
			{
				if (this._encoder == IntPtr.Zero)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int num = API.opus_encoder_ctl(this._encoder, Ctl.SetInbandFECRequest, (!value) ? 0 : 1);
				if (num < 0)
				{
					string str = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(str + errors.ToString());
				}
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003B15 RID: 15125 RVA: 0x00129C84 File Offset: 0x00128084
		// (set) Token: 0x06003B16 RID: 15126 RVA: 0x00129CEC File Offset: 0x001280EC
		public int ExpectedPacketLossPct
		{
			get
			{
				if (this._encoder == IntPtr.Zero)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int result;
				int num = API.opus_encoder_ctl(this._encoder, Ctl.GetPacketLossPercRequest, out result);
				if (num < 0)
				{
					string str = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(str + errors.ToString());
				}
				return result;
			}
			set
			{
				if (this._encoder == IntPtr.Zero)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int num = API.opus_encoder_ctl(this._encoder, Ctl.SetPacketLossPercRequest, value);
				if (num < 0)
				{
					string str = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(str + errors.ToString());
				}
			}
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x00129D54 File Offset: 0x00128154
		~OpusEncoder()
		{
			this.Dispose();
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x00129D84 File Offset: 0x00128184
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			if (this._encoder != IntPtr.Zero)
			{
				API.opus_encoder_destroy(this._encoder);
				this._encoder = IntPtr.Zero;
			}
			this.disposed = true;
		}

		// Token: 0x040023C5 RID: 9157
		private IntPtr _encoder;

		// Token: 0x040023CA RID: 9162
		private bool disposed;
	}
}
