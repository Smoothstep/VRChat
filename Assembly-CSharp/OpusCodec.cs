using System;
using FragLabs.Audio.Codecs;
using FragLabs.Audio.Codecs.Opus;
using MoPhoGames.USpeak.Codec;
using MoPhoGames.USpeak.Core.Utils;
using UnityEngine;

// Token: 0x020009C8 RID: 2504
public class OpusCodec : ICodec
{
	// Token: 0x06004C36 RID: 19510 RVA: 0x001978CB File Offset: 0x00195CCB
	public OpusCodec()
	{
		this.InitSettings(48000, 24000, 20, FragLabs.Audio.Codecs.Opus.Application.Voip);
	}

	// Token: 0x06004C37 RID: 19511 RVA: 0x001978EA File Offset: 0x00195CEA
	private void InitSettings(int frequency, int bitrate, int delay, FragLabs.Audio.Codecs.Opus.Application app)
	{
		if (bitrate > 48000)
		{
			app = FragLabs.Audio.Codecs.Opus.Application.Audio;
		}
		this._frequency = frequency;
		this._bitrate = bitrate;
		this._segmentFrames = delay * (this._frequency / 1000);
		this._app = app;
	}

	// Token: 0x06004C38 RID: 19512 RVA: 0x00197928 File Offset: 0x00195D28
	private void CreateEncoders()
	{
		try
		{
			this._encoder = OpusEncoder.Create(this._frequency, 1, this._app);
			this._encoder.Bitrate = this._bitrate;
			this._bytesPerSegment = this._encoder.FrameByteCount(this._segmentFrames);
			this._decoder = OpusDecoder.Create(this._frequency, 1, this._bytesPerSegment);
			this._encoder.ForwardErrorCorrection = false;
			this._decoder.ForwardErrorCorrection = false;
			this._encoder.ExpectedPacketLossPct = 0;
			this.isInitialized = true;
		}
		catch (DllNotFoundException ex)
		{
			throw ex;
		}
	}

	// Token: 0x06004C39 RID: 19513 RVA: 0x001979D0 File Offset: 0x00195DD0
	public void Initialize(BandMode bandMode, BitRate bitrate, FrameDuration duration)
	{
		OpusCodec.BitRates bitrate2 = (OpusCodec.BitRates)Enum.GetValues(typeof(OpusCodec.BitRates)).GetValue((int)bitrate);
		OpusCodec.Opus_Delay delay = (OpusCodec.Opus_Delay)Enum.GetValues(typeof(OpusCodec.Opus_Delay)).GetValue((int)duration);
		Debug.Log(string.Concat(new string[]
		{
			"OpusCodec: Initialize (",
			bitrate2.ToString(),
			", ",
			delay.ToString(),
			")"
		}));
		this.InitSettings(48000, (int)bitrate2, (int)delay, FragLabs.Audio.Codecs.Opus.Application.Voip);
		this.CreateEncoders();
	}

	// Token: 0x06004C3A RID: 19514 RVA: 0x00197A78 File Offset: 0x00195E78
	public byte[] Encode(short[] data, BandMode mode)
	{
		if (!this.isInitialized)
		{
			this.CreateEncoders();
		}
		if (mode != BandMode.Opus48k)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"OpusCodec: Encode: bandwidth mode must be ",
				BandMode.Opus48k.ToString(),
				"! (set to ",
				mode.ToString(),
				")"
			}));
		}
		if (data.Length != this._segmentFrames)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"OpusCodec: Encode failed! Input PCM data is ",
				data.Length,
				" frames, expected ",
				this._segmentFrames
			}));
			return new byte[0];
		}
		byte[] @byte = USpeakPoolUtils.GetByte(data.Length * 2);
		Buffer.BlockCopy(data, 0, @byte, 0, data.Length * 2);
		int num = 0;
		byte[] result = this._encoder.Encode(@byte, this._bytesPerSegment, out num);
		USpeakPoolUtils.Return(@byte);
		return result;
	}

	// Token: 0x06004C3B RID: 19515 RVA: 0x00197B6C File Offset: 0x00195F6C
	public short[] Decode(byte[] data, BandMode mode)
	{
		if (!this.isInitialized)
		{
			this.CreateEncoders();
		}
		if (mode != BandMode.Opus48k)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"OpusCodec: Decode: bandwidth mode must be ",
				BandMode.Opus48k.ToString(),
				"! (set to ",
				mode.ToString(),
				")"
			}));
		}
		int num = 0;
		byte[] array = this._decoder.Decode(data, (data == null) ? 0 : data.Length, out num);
		if (num != this._bytesPerSegment)
		{
			int num2 = (data == null) ? 0 : data.Length;
			Debug.LogError(string.Concat(new object[]
			{
				"OpusCodec: Decode failed! Output PCM data is ",
				num,
				" bbytes, expected ",
				this._bytesPerSegment,
				" (compressed packet size was ",
				num2,
				")"
			}));
			USpeakPoolUtils.Return(array);
		}
		short[] @short = USpeakPoolUtils.GetShort(this._bytesPerSegment / 2);
		Buffer.BlockCopy(array, 0, @short, 0, this._bytesPerSegment);
		USpeakPoolUtils.Return(array);
		return @short;
	}

	// Token: 0x06004C3C RID: 19516 RVA: 0x00197C94 File Offset: 0x00196094
	public int GetSampleSize(int recordingFrequency)
	{
		return this._segmentFrames;
	}

	// Token: 0x06004C3D RID: 19517 RVA: 0x00197C9C File Offset: 0x0019609C
	public void MarkStreamDiscontinuity()
	{
		if (!this.isInitialized)
		{
			this.CreateEncoders();
		}
		this._encoder.ResetState();
		this._decoder.ResetState();
	}

	// Token: 0x0400340C RID: 13324
	private const int OPUS_SAMPLE_FREQUENCY = 48000;

	// Token: 0x0400340D RID: 13325
	private OpusEncoder _encoder;

	// Token: 0x0400340E RID: 13326
	private OpusDecoder _decoder;

	// Token: 0x0400340F RID: 13327
	private int _segmentFrames;

	// Token: 0x04003410 RID: 13328
	private int _bytesPerSegment;

	// Token: 0x04003411 RID: 13329
	private int _frequency;

	// Token: 0x04003412 RID: 13330
	private int _bitrate;

	// Token: 0x04003413 RID: 13331
	private FragLabs.Audio.Codecs.Opus.Application _app;

	// Token: 0x04003414 RID: 13332
	private bool isInitialized;

	// Token: 0x020009C9 RID: 2505
	public enum BitRates
	{
		// Token: 0x04003416 RID: 13334
		BitRate_8K = 8000,
		// Token: 0x04003417 RID: 13335
		BitRate_10K = 10000,
		// Token: 0x04003418 RID: 13336
		BitRate_16K = 16000,
		// Token: 0x04003419 RID: 13337
		BitRate_18K = 18000,
		// Token: 0x0400341A RID: 13338
		BitRate_20K = 20000,
		// Token: 0x0400341B RID: 13339
		BitRate_24K = 24000,
		// Token: 0x0400341C RID: 13340
		BitRate_32K = 32000,
		// Token: 0x0400341D RID: 13341
		BitRate_48K = 48000,
		// Token: 0x0400341E RID: 13342
		BitRate_64k = 64000,
		// Token: 0x0400341F RID: 13343
		BitRate_96k = 96000,
		// Token: 0x04003420 RID: 13344
		BitRate_128k = 128000,
		// Token: 0x04003421 RID: 13345
		BitRate_256k = 256000,
		// Token: 0x04003422 RID: 13346
		BitRate_384k = 384000,
		// Token: 0x04003423 RID: 13347
		BitRate_512k = 512000
	}

	// Token: 0x020009CA RID: 2506
	public enum Opus_Delay
	{
		// Token: 0x04003425 RID: 13349
		Delay_10ms = 10,
		// Token: 0x04003426 RID: 13350
		Delay_20ms = 20,
		// Token: 0x04003427 RID: 13351
		Delay_40ms = 40,
		// Token: 0x04003428 RID: 13352
		Delay_60ms = 60
	}
}
