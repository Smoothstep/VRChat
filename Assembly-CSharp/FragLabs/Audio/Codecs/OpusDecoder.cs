using System;
using FragLabs.Audio.Codecs.Opus;
using MoPhoGames.USpeak.Core.Utils;

namespace FragLabs.Audio.Codecs
{
	// Token: 0x0200070D RID: 1805
	public class OpusDecoder : IDisposable
	{
		// Token: 0x06003AF4 RID: 15092 RVA: 0x001295B0 File Offset: 0x001279B0
		private OpusDecoder(IntPtr decoder, int outputSamplingRate, int outputChannels, int expectedBytesPerSegment)
		{
			this._decoder = decoder;
			this.OutputSamplingRate = outputSamplingRate;
			this.OutputChannels = outputChannels;
			this.MaxDataBytes = outputSamplingRate / 1000 * 2 * 60;
			this._expectedBytesPerSegment = expectedBytesPerSegment;
			if (this._expectedBytesPerSegment <= 0)
			{
				this._expectedBytesPerSegment = this.MaxDataBytes;
			}
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x0012960C File Offset: 0x00127A0C
		public static OpusDecoder Create(int outputSampleRate, int outputChannels, int expectedBytesPerSegment)
		{
			if (outputSampleRate != 8000 && outputSampleRate != 12000 && outputSampleRate != 16000 && outputSampleRate != 24000 && outputSampleRate != 48000)
			{
				throw new ArgumentOutOfRangeException("inputSamplingRate");
			}
			if (outputChannels != 1 && outputChannels != 2)
			{
				throw new ArgumentOutOfRangeException("inputChannels");
			}
			IntPtr value;
			IntPtr decoder = API.opus_decoder_create(outputSampleRate, outputChannels, out value);
			if ((int)value != 0)
			{
				throw new Exception("Exception occured while creating decoder");
			}
			return new OpusDecoder(decoder, outputSampleRate, outputChannels, expectedBytesPerSegment);
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x001296A0 File Offset: 0x00127AA0
		public unsafe byte[] Decode(byte[] inputOpusData, int dataLength, out int decodedLength)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("OpusDecoder");
			}
			byte[] @byte = USpeakPoolUtils.GetByte(this.MaxDataBytes);
			int frame_size = this.FrameCount(this.MaxDataBytes);
			int num;
			fixed (byte* value = (@byte != null && @byte.Length != 0) ? @byte : null)
			{
				IntPtr pcm = new IntPtr((void*)value);
				if (inputOpusData != null)
				{
					num = API.opus_decode(this._decoder, inputOpusData, dataLength, pcm, frame_size, 0);
				}
				else
				{
					num = API.opus_decode(this._decoder, null, 0, pcm, this.FrameCount(this._expectedBytesPerSegment), (!this.ForwardErrorCorrection) ? 0 : 1);
				}
			}
			decodedLength = num * 2;
			if (num < 0)
			{
				USpeakPoolUtils.Return(@byte);
				string str = "Decoding failed - ";
				Errors errors = (Errors)num;
				throw new Exception(str + errors.ToString());
			}
			byte[] byte2 = USpeakPoolUtils.GetByte(decodedLength);
			Buffer.BlockCopy(@byte, 0, byte2, 0, decodedLength);
			USpeakPoolUtils.Return(@byte);
			return byte2;
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x001297A1 File Offset: 0x00127BA1
		public void ResetState()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("OpusDecoder");
			}
			API.opus_decoder_ctl(this._decoder, Ctl.OpusResetState, 0);
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x001297CC File Offset: 0x00127BCC
		public int FrameCount(int bufferSize)
		{
			int num = 16;
			int num2 = num / 8 * this.OutputChannels;
			return bufferSize / num2;
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x001297EA File Offset: 0x00127BEA
		// (set) Token: 0x06003AFA RID: 15098 RVA: 0x001297F2 File Offset: 0x00127BF2
		public int OutputSamplingRate { get; private set; }

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x001297FB File Offset: 0x00127BFB
		// (set) Token: 0x06003AFC RID: 15100 RVA: 0x00129803 File Offset: 0x00127C03
		public int OutputChannels { get; private set; }

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003AFD RID: 15101 RVA: 0x0012980C File Offset: 0x00127C0C
		// (set) Token: 0x06003AFE RID: 15102 RVA: 0x00129814 File Offset: 0x00127C14
		public int MaxDataBytes { get; set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003AFF RID: 15103 RVA: 0x0012981D File Offset: 0x00127C1D
		// (set) Token: 0x06003B00 RID: 15104 RVA: 0x00129825 File Offset: 0x00127C25
		public bool ForwardErrorCorrection { get; set; }

		// Token: 0x06003B01 RID: 15105 RVA: 0x00129830 File Offset: 0x00127C30
		~OpusDecoder()
		{
			this.Dispose();
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x00129860 File Offset: 0x00127C60
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			if (this._decoder != IntPtr.Zero)
			{
				API.opus_decoder_destroy(this._decoder);
				this._decoder = IntPtr.Zero;
			}
			this.disposed = true;
		}

		// Token: 0x040023BE RID: 9150
		private IntPtr _decoder;

		// Token: 0x040023C3 RID: 9155
		private int _expectedBytesPerSegment;

		// Token: 0x040023C4 RID: 9156
		private bool disposed;
	}
}
