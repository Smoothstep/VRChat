using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoPhoGames.USpeak.Core;
using MoPhoGames.USpeak.Core.Utils;
using MoPhoGames.USpeak.Interface;
using UnityEngine;
using VRC;
using VRC.Core;
using VRCSDK2;

// Token: 0x020009D3 RID: 2515
[AddComponentMenu("USpeak/USpeaker")]
public class USpeaker : MonoBehaviour
{
	// Token: 0x17000B94 RID: 2964
	// (get) Token: 0x06004C46 RID: 19526 RVA: 0x001980A8 File Offset: 0x001964A8
	public int DurationMs
	{
		get
		{
			switch (this.Duration)
			{
			case FrameDuration.FrameDuration_10ms:
				return 10;
			case FrameDuration.FrameDuration_20ms:
				return 20;
			case FrameDuration.FrameDuration_40ms:
				return 40;
			case FrameDuration.FrameDuration_60ms:
				return 60;
			default:
				Debug.LogError("Unknown Duration value: " + this.Duration);
				return 20;
			}
		}
	}

	// Token: 0x17000B95 RID: 2965
	// (get) Token: 0x06004C47 RID: 19527 RVA: 0x001980FF File Offset: 0x001964FF
	// (set) Token: 0x06004C48 RID: 19528 RVA: 0x0019810A File Offset: 0x0019650A
	[Obsolete("Use USpeaker._3DMode instead")]
	public bool Is3D
	{
		get
		{
			return this._3DMode == ThreeDMode.SpeakerPan;
		}
		set
		{
			if (value)
			{
				this._3DMode = ThreeDMode.SpeakerPan;
			}
			else
			{
				this._3DMode = ThreeDMode.None;
			}
		}
	}

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06004C49 RID: 19529 RVA: 0x00198125 File Offset: 0x00196525
	public bool IsTalking
	{
		get
		{
			return this.talkTimer > 0f;
		}
	}

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06004C4A RID: 19530 RVA: 0x00198134 File Offset: 0x00196534
	public static float FrameProcessTimeEstMs
	{
		get
		{
			return Time.maximumDeltaTime * 1000f;
		}
	}

	// Token: 0x06004C4B RID: 19531 RVA: 0x00198141 File Offset: 0x00196541
	public float GetMagnitudeAndAdvance()
	{
		if (this.MagnitudeReadCount >= this.MagnitudeWriteCount)
		{
			return 0f;
		}
		this.MagnitudeReadCount++;
		return this.Magnitude60Hz[this.MagnitudeReadCount & 31];
	}

	// Token: 0x06004C4C RID: 19532 RVA: 0x00198178 File Offset: 0x00196578
	public void RecordMagnitude(float rms)
	{
		if (this.MagnitudeWriteCount > this.MagnitudeReadCount + 30)
		{
			this.MagnitudeReadCount++;
		}
		this.MagnitudeWriteCount++;
		this.Magnitude60Hz[this.MagnitudeWriteCount & 31] = rms;
	}

	// Token: 0x06004C4D RID: 19533 RVA: 0x001981C8 File Offset: 0x001965C8
	private void RecordVolumeStats(float[] samples, out float rms, out float peak, out float floor)
	{
		rms = this.GetRootMeanSquare(samples);
		peak = this.GetPeak(samples);
		floor = this.GetFloor(samples);
		this._volumeStatNumFrames += 1u;
		this._volumeStatRMSMin = Mathf.Min(this._volumeStatRMSMin, rms);
		this._volumeStatRMSMax = Mathf.Max(this._volumeStatRMSMax, rms);
		this._volumeStatRMSTotal += rms;
		this._volumeStatRMSMean = ((this._volumeStatNumFrames <= 0u) ? this._volumeStatRMSMean : (this._volumeStatRMSTotal / this._volumeStatNumFrames));
		this._volumeStatFloorMin = Mathf.Min(this._volumeStatFloorMin, floor);
		this._volumeStatFloorMax = Mathf.Max(this._volumeStatFloorMax, floor);
		this._volumeStatFloorTotal += floor;
		this._volumeStatFloorMean = ((this._volumeStatNumFrames <= 0u) ? this._volumeStatFloorMean : (this._volumeStatFloorTotal / this._volumeStatNumFrames));
		this._volumeStatPeakMin = Mathf.Min(this._volumeStatPeakMin, peak);
		this._volumeStatPeakMax = Mathf.Max(this._volumeStatPeakMax, peak);
		this._volumeStatPeakTotal += peak;
		this._volumeStatPeakMean = ((this._volumeStatNumFrames <= 0u) ? this._volumeStatPeakMean : (this._volumeStatPeakTotal / this._volumeStatNumFrames));
	}

	// Token: 0x06004C4E RID: 19534 RVA: 0x00198328 File Offset: 0x00196728
	private float GetRootMeanSquare(float[] samples)
	{
		float num = 0f;
		for (int i = 0; i < samples.Length; i++)
		{
			num += samples[i] * samples[i];
		}
		return Mathf.Sqrt(num / (float)samples.Length);
	}

	// Token: 0x06004C4F RID: 19535 RVA: 0x00198368 File Offset: 0x00196768
	private float GetPeak(float[] samples)
	{
		float num = 0f;
		for (int i = 0; i < samples.Length; i++)
		{
			float num2 = samples[i] * samples[i];
			num = ((num <= num2) ? num2 : num);
		}
		return Mathf.Sqrt(num);
	}

	// Token: 0x06004C50 RID: 19536 RVA: 0x001983AC File Offset: 0x001967AC
	private float GetFloor(float[] samples)
	{
		float num = float.MaxValue;
		for (int i = 0; i < samples.Length; i++)
		{
			float num2 = samples[i] * samples[i];
			num = ((num >= num2) ? num2 : num);
		}
		return Mathf.Sqrt(num);
	}

	// Token: 0x17000B98 RID: 2968
	// (get) Token: 0x06004C51 RID: 19537 RVA: 0x001983F0 File Offset: 0x001967F0
	private int audioFrequency
	{
		get
		{
			if (this.recFreq == 0)
			{
				switch (this.BandwidthMode)
				{
				case BandMode.Narrow:
					this.recFreq = 8000;
					break;
				case BandMode.Wide:
					this.recFreq = 16000;
					break;
				case BandMode.UltraWide:
					this.recFreq = 32000;
					break;
				case BandMode.Opus48k:
					this.recFreq = 48000;
					break;
				default:
					this.recFreq = 8000;
					break;
				}
			}
			return this.recFreq;
		}
	}

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x06004C52 RID: 19538 RVA: 0x00198480 File Offset: 0x00196880
	private int PlaybackBufferLengthInSamples
	{
		get
		{
			return this.audioFrequency * 10;
		}
	}

	// Token: 0x06004C53 RID: 19539 RVA: 0x0019848C File Offset: 0x0019688C
	public static void SetInputDevice(int deviceID)
	{
		USpeaker.CacheDevices();
		if (deviceID >= 0 && deviceID < USpeaker.devices.Length)
		{
			USpeaker.InputDeviceID = deviceID;
			USpeaker.InputDeviceName = USpeaker.devices[USpeaker.InputDeviceID];
			Debug.Log(string.Concat(new object[]
			{
				"uSpeak: SetInputDevice ",
				deviceID,
				" (",
				USpeaker.devices.Length,
				" total) '",
				USpeaker.InputDeviceName,
				"'"
			}));
		}
		else
		{
			USpeaker.InputDeviceID = 0;
			USpeaker.InputDeviceName = ((USpeaker.devices.Length <= 0) ? string.Empty : USpeaker.devices[0]);
			Debug.Log(string.Concat(new object[]
			{
				"uSpeak: SetInputDevice ",
				deviceID,
				" (",
				USpeaker.devices.Length,
				" total, index out of range, setting to default device) '",
				USpeaker.InputDeviceName,
				"'"
			}));
		}
	}

	// Token: 0x06004C54 RID: 19540 RVA: 0x00198598 File Offset: 0x00196998
	public static void SetInputDevice(string deviceName)
	{
		USpeaker.CacheDevices();
		Debug.Log(string.Concat(new object[]
		{
			"uSpeak: SetInputDevice by name '",
			deviceName,
			"' (",
			USpeaker.devices.Length,
			" total)"
		}));
		if (string.IsNullOrEmpty(deviceName))
		{
			USpeaker.SetInputDevice(0);
			return;
		}
		for (int i = 0; i < USpeaker.devices.Length; i++)
		{
			if (USpeaker.devices[i] == deviceName)
			{
				USpeaker.SetInputDevice(i);
				return;
			}
		}
		Debug.Log("uSpeak: SetInputDevice: Did not find microphone '" + deviceName + "', resetting to default mic..");
		USpeaker.SetInputDevice(0);
	}

	// Token: 0x06004C55 RID: 19541 RVA: 0x00198644 File Offset: 0x00196A44
	public static bool TrySetInputDevice(int deviceID)
	{
		USpeaker.CacheDevices();
		Debug.Log(string.Concat(new object[]
		{
			"uSpeak: TrySetInputDeviceID ",
			deviceID,
			" (",
			USpeaker.devices.Length,
			" total)"
		}));
		if (deviceID >= 0 && deviceID < USpeaker.devices.Length)
		{
			USpeaker.InputDeviceID = deviceID;
			USpeaker.InputDeviceName = USpeaker.devices[USpeaker.InputDeviceID];
			return true;
		}
		return false;
	}

	// Token: 0x06004C56 RID: 19542 RVA: 0x001986C3 File Offset: 0x00196AC3
	public static void SetInputDeviceFromPrefs()
	{
		USpeaker.SetInputDevice(VRCInputManager.micDeviceName);
		if (USpeaker.GetInputDeviceName() != VRCInputManager.micDeviceName)
		{
			VRCInputManager.micDeviceName = string.Empty;
		}
	}

	// Token: 0x06004C57 RID: 19543 RVA: 0x001986ED File Offset: 0x00196AED
	public static int GetInputDeviceID()
	{
		return USpeaker.InputDeviceID;
	}

	// Token: 0x06004C58 RID: 19544 RVA: 0x001986F4 File Offset: 0x00196AF4
	public static string GetInputDeviceName()
	{
		return USpeaker.InputDeviceName;
	}

	// Token: 0x06004C59 RID: 19545 RVA: 0x001986FB File Offset: 0x00196AFB
	public static string GetFriendlyInputDeviceName()
	{
		if (USpeaker.InputDeviceName == "<unknown mic>")
		{
			return "Microphone";
		}
		return USpeaker.InputDeviceName;
	}

	// Token: 0x06004C5A RID: 19546 RVA: 0x0019871C File Offset: 0x00196B1C
	private AudioClip MicrophoneStart(string deviceName)
	{
		Debug.Log("uSpeak: start Microphone: " + deviceName);
		if (deviceName == "<unknown mic>")
		{
			deviceName = string.Empty;
		}
		AudioClip result = null;
		try
		{
			result = Microphone.Start(deviceName, true, 5, this.audioFrequency);
		}
		catch (Exception ex)
		{
			Debug.LogError("Microphone.Start failed - exception: " + ex.ToString());
			Debug.LogException(ex);
		}
		return result;
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x00198798 File Offset: 0x00196B98
	private void MicrophoneEnd(string deviceName)
	{
		if (deviceName == "<unknown mic>")
		{
			deviceName = string.Empty;
		}
		Microphone.End(deviceName);
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x001987B7 File Offset: 0x00196BB7
	private int MicrophoneGetPosition(string deviceName)
	{
		if (deviceName == "<unknown mic>")
		{
			deviceName = string.Empty;
		}
		return Microphone.GetPosition(deviceName);
	}

	// Token: 0x06004C5D RID: 19549 RVA: 0x001987D8 File Offset: 0x00196BD8
	private void DebugPrintMicrophoneDevices()
	{
		string[] array = Microphone.devices;
		Debug.Log("Microphones installed (" + array.Length + " total)");
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			int num = 0;
			int num2 = 0;
			Microphone.GetDeviceCaps(text, out num, out num2);
			Debug.Log(string.Concat(new object[]
			{
				"-- [",
				i,
				"] device name = '",
				text,
				"' min/max freq = ",
				num,
				" / ",
				num2
			}));
		}
	}

	// Token: 0x06004C5E RID: 19550 RVA: 0x00198880 File Offset: 0x00196C80
	public static USpeaker Get(UnityEngine.Object source)
	{
		if (source is GameObject)
		{
			return (source as GameObject).GetComponent<USpeaker>();
		}
		if (source is Transform)
		{
			return (source as Transform).GetComponent<USpeaker>();
		}
		if (source is Component)
		{
			return (source as Component).GetComponent<USpeaker>();
		}
		return null;
	}

	// Token: 0x06004C5F RID: 19551 RVA: 0x001988D4 File Offset: 0x00196CD4
	public static void CacheDevices()
	{
		USpeaker.devices = Microphone.devices;
		if (USpeaker.devices.Length > 0 && string.IsNullOrEmpty(USpeaker.devices[0]))
		{
			USpeaker.devices[0] = "<unknown mic>";
		}
		USpeaker.devices = (from d in USpeaker.devices
		where !string.IsNullOrEmpty(d)
		select d).ToArray<string>();
	}

	// Token: 0x06004C60 RID: 19552 RVA: 0x00198946 File Offset: 0x00196D46
	public static int GetNumDevices()
	{
		return USpeaker.devices.Length;
	}

	// Token: 0x06004C61 RID: 19553 RVA: 0x0019894F File Offset: 0x00196D4F
	public static string GetDeviceName(int deviceID)
	{
		return (deviceID < 0 || deviceID >= USpeaker.devices.Length) ? string.Empty : USpeaker.devices[deviceID];
	}

	// Token: 0x06004C62 RID: 19554 RVA: 0x00198976 File Offset: 0x00196D76
	public void GetInputHandler()
	{
		this.talkController = (IUSpeakTalkController)this.FindInputHandler();
	}

	// Token: 0x06004C63 RID: 19555 RVA: 0x00198989 File Offset: 0x00196D89
	public void DrawTalkControllerUI()
	{
		if (this.talkController != null)
		{
			this.talkController.OnInspectorGUI();
		}
		else
		{
			GUILayout.Label("No component available which implements IUSpeakTalkController\nReverting to default behavior - data is always sent", new GUILayoutOption[0]);
		}
	}

	// Token: 0x06004C64 RID: 19556 RVA: 0x001989B6 File Offset: 0x00196DB6
	public void MutedAudio(byte[] data)
	{
		if (this.SpeakerMode != SpeakerMode.Remote)
		{
			return;
		}
		this.talkTimer = 1f;
	}

	// Token: 0x06004C65 RID: 19557 RVA: 0x001989D0 File Offset: 0x00196DD0
	public void ReceiveAudio(byte[] data)
	{
		this.Log("ReceiveAudio: bytes = " + data.Length, 10f, USpeakDebugLevel.Full);
		if (this.settings == null)
		{
			this.Log("Trying to receive remote audio data without calling InitializeSettings!\nIncoming packet will be ignored", -1f, USpeakDebugLevel.Default);
		}
		if (USpeaker.MuteAll || this.Mute || (this.SpeakerMode == SpeakerMode.Local && !this.DebugPlayback))
		{
			this.Log("ReceiveAudio: player is muted", 20f, USpeakDebugLevel.Full);
			return;
		}
		if (this.SpeakerMode == SpeakerMode.Remote)
		{
			this.talkTimer = 1f;
		}
		int i = 0;
		int num = BitConverter.ToInt32(data, i);
		i += 4;
		int num2 = Mathf.Max(PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds - num, 0);
		int num3 = 0;
		while (i < data.Length)
		{
			USpeakFrameContainer uspeakFrameContainer = default(USpeakFrameContainer);
			int num4 = uspeakFrameContainer.LoadFrom(data, i);
			int num5 = (this.lastReceivedFrameIndex < 0) ? 0 : ((int)uspeakFrameContainer.FrameIndex - this.lastReceivedFrameIndex - 1);
			if (num5 < 0)
			{
				num5 += 65536;
			}
			if (num5 >= 9)
			{
				float num6 = (float)num5 * (float)this.codecMgr.Codecs[this.Codec].GetSampleSize(this.audioFrequency) / (float)this.audioFrequency;
				if (num6 > 1f)
				{
					num6 = 1f;
				}
				this.StartNewPlaybackSegment(num6);
			}
			else if (num5 > 0)
			{
				this.Log(" *** ReceiveAudio: " + num5 + " frame gap in stream", -1f, USpeakDebugLevel.Full);
				for (int j = num5; j > 0; j--)
				{
					float[] array = USpeakAudioClipCompressor.DecompressAudio(null, this.codecMgr.Codecs[this.Codec].GetSampleSize(this.audioFrequency), 1, false, this.settings.bandMode, this.codecMgr.Codecs[this.Codec], USpeaker.RemoteGain);
					this.QueueSampleForPlayback(array);
					USpeakPoolUtils.Return(array);
				}
			}
			this.lastReceivedFrameIndex = (int)uspeakFrameContainer.FrameIndex;
			float[] array2 = USpeakAudioClipCompressor.DecompressAudio(uspeakFrameContainer.encodedData, this.codecMgr.Codecs[this.Codec].GetSampleSize(this.audioFrequency), 1, false, this.settings.bandMode, this.codecMgr.Codecs[this.Codec], USpeaker.RemoteGain);
			this.QueueSampleForPlayback(array2);
			USpeakPoolUtils.Return(array2);
			i += num4;
			num3++;
		}
		if (i != data.Length)
		{
			string message = string.Concat(new object[]
			{
				"uSpeak: ReceiveAudio data length doesn't match, expected ",
				i,
				", actual ",
				data.Length
			});
			Debug.LogError(message);
		}
		int num7 = num3 * this.DurationMs;
		float num8 = (float)(num2 + num7) + USpeaker.FrameProcessTimeEstMs;
		this.Log(string.Concat(new object[]
		{
			"Play voice: estimated latency ms: send delay + transit + buffer = ",
			num2,
			", audio length = ",
			num7,
			", frame delay = ",
			USpeaker.FrameProcessTimeEstMs,
			", TOTAL = ",
			num8
		}), 20f, USpeakDebugLevel.Default);
	}

	// Token: 0x06004C66 RID: 19558 RVA: 0x00198D04 File Offset: 0x00197104
	public void Log(string msg, float everyNSeconds = -1f, USpeakDebugLevel debugLevel = USpeakDebugLevel.Default)
	{
		if (this.DebugLevel >= debugLevel)
		{
			if (everyNSeconds > 0f)
			{
				VRC.Core.Logger.LogOnceEveryHash(everyNSeconds, VRC.Core.Logger.GetCallingStackFrameHash(this), string.Concat(new string[]
				{
					"uSpeak [",
					this.GetOwnerID(),
					"](every ",
					everyNSeconds.ToString("g2"),
					"): ",
					msg
				}));
			}
			else
			{
				Debug.Log("uSpeak [" + this.GetOwnerID() + "]: " + msg);
			}
		}
	}

	// Token: 0x06004C67 RID: 19559 RVA: 0x00198D94 File Offset: 0x00197194
	private void QueueSampleForPlayback(float[] sample)
	{
		if (sample == null || sample.Length == 0)
		{
			this.Log("QueueSampleForPlayback: sample was null or empty!", -1f, USpeakDebugLevel.Default);
			return;
		}
		if (this._audioSource.clip.samples == 0)
		{
			this.Log("QueueSampleForPlayback: _audioSource.clip.samples was zero", -1f, USpeakDebugLevel.Default);
			return;
		}
		int num = this.audioFrequency / 2;
		if (this.playbackBufferUsed + sample.Length + num > this._audioSource.clip.samples)
		{
			this.Log("playback buffer FULL (length: " + this.PlaybackBufferLengthInSamples / this.audioFrequency + "), clearing playback buffer", -1f, USpeakDebugLevel.Default);
			this.StopPlaying();
		}
		if (this.DebugLevel >= USpeakDebugLevel.Full)
		{
			float num2 = (float)(this.playbackBufferUsed + sample.Length + num) / (float)this.audioFrequency;
			float num3 = (float)this._audioSource.clip.samples / (float)this.audioFrequency;
			VRC.Core.Logger.LogOnceEvery(10f, this, string.Concat(new object[]
			{
				"uSpeak (",
				this.GetOwnerID(),
				") playback buffer time used ",
				num2.ToString("g3"),
				" / ",
				num3.ToString("g3"),
				" (",
				num2 / num3,
				" pct)"
			}));
		}
		float rootMeanSquare = this.GetRootMeanSquare(sample);
		this.RecordMagnitude(rootMeanSquare);
		this.Autolevel(sample, rootMeanSquare);
		this.AddSampleTimeToLatestPlaybackSegment(sample.Length);
		this._audioSource.clip.SetData(sample, this.index);
		this.index = (this.index + sample.Length) % this._audioSource.clip.samples;
		this._audioSource.clip.SetData(this.GetSilenceClip(num), this.index);
		this.playbackBufferUsed += sample.Length;
		if (!this.shouldPlay)
		{
			this.shouldPlay = true;
			if (this._playStartRequestTime < 0f)
			{
				this._playStartRequestTime = Time.realtimeSinceStartup;
				float a = Mathf.Max(this.GetCurrentPlaybackSegmentPreDelay() - (Time.realtimeSinceStartup - this.stoppedTime), 0f);
				this.playDelay = Mathf.Max(a, this.kPlayDelayForJitter);
			}
		}
	}

	// Token: 0x06004C68 RID: 19560 RVA: 0x00198FD8 File Offset: 0x001973D8
	private float[] GetSilenceClip(int length)
	{
		if (this._silence == null || this._silence.Length != length)
		{
			this._silence = new float[length];
			Array.Clear(this._silence, 0, length);
		}
		return this._silence;
	}

	// Token: 0x06004C69 RID: 19561 RVA: 0x00199014 File Offset: 0x00197414
	public float GetPlaybackDelayForLatency()
	{
		float num = (float)(PhotonNetwork.networkingPeer.RoundTripTimeVariance + this.GetRemotePingVariance()) + this.GetEstimatedPacketAudioLength() * 1000f;
		float value = num / 1000f;
		return Mathf.Clamp(value, 0.1f, 0.4f);
	}

	// Token: 0x06004C6A RID: 19562 RVA: 0x0019905C File Offset: 0x0019745C
	public float GetEstimatedPacketAudioLength()
	{
		float num = (float)this.DurationMs / 1000f;
		float num2 = Mathf.Ceil(1f / this.SendRate / num);
		return Mathf.Max(num, num2 * num);
	}

	// Token: 0x06004C6B RID: 19563 RVA: 0x00199094 File Offset: 0x00197494
	public int GetTransitTimeFromRemoteMS()
	{
		return PhotonNetwork.GetPing() / 2 + this.GetRemotePing() / 2;
	}

	// Token: 0x06004C6C RID: 19564 RVA: 0x001990A6 File Offset: 0x001974A6
	private int GetMaxTransitTimeFromRemoteMS()
	{
		return (PhotonNetwork.GetPing() + PhotonNetwork.networkingPeer.RoundTripTimeVariance) / 2 + (this.GetRemotePing() + this.GetRemotePingVariance()) / 2;
	}

	// Token: 0x06004C6D RID: 19565 RVA: 0x001990CC File Offset: 0x001974CC
	public float GetMaxValidPacketAgeMS()
	{
		float num = (float)this.GetMaxTransitTimeFromRemoteMS() + 200f;
		return num + 3000f;
	}

	// Token: 0x06004C6E RID: 19566 RVA: 0x001990F0 File Offset: 0x001974F0
	public int GetRemotePing()
	{
		if (this.SpeakerMode == SpeakerMode.Local)
		{
			return PhotonNetwork.GetPing();
		}
		if (this._player != null)
		{
			return (int)this._player.Ping;
		}
		return 0;
	}

	// Token: 0x06004C6F RID: 19567 RVA: 0x00199121 File Offset: 0x00197521
	public int GetRemotePingVariance()
	{
		if (this.SpeakerMode == SpeakerMode.Local)
		{
			return PhotonNetwork.networkingPeer.RoundTripTimeVariance;
		}
		if (this._player != null)
		{
			return (int)this._player.PingVariance;
		}
		return 0;
	}

	// Token: 0x06004C70 RID: 19568 RVA: 0x00199158 File Offset: 0x00197558
	private void AddSampleTimeToLatestPlaybackSegment(int numSamples)
	{
		if (this.playbackSegments.Count == 0)
		{
			this.StartNewPlaybackSegment(0f);
		}
		this.playbackSegments[this.playbackSegments.Count - 1].receivedSamples += numSamples;
	}

	// Token: 0x06004C71 RID: 19569 RVA: 0x001991A5 File Offset: 0x001975A5
	private int GetCurrentPlaybackSegmentLengthInSamples()
	{
		if (this.playbackSegments.Count == 0)
		{
			return 0;
		}
		return this.playbackSegments[0].receivedSamples;
	}

	// Token: 0x06004C72 RID: 19570 RVA: 0x001991CA File Offset: 0x001975CA
	private float GetCurrentPlaybackSegmentPreDelay()
	{
		if (this.playbackSegments.Count == 0)
		{
			return 0f;
		}
		return this.playbackSegments[0].preDelay;
	}

	// Token: 0x06004C73 RID: 19571 RVA: 0x001991F4 File Offset: 0x001975F4
	private void StartNewPlaybackSegment(float gapLengthSec)
	{
		this.Log(string.Concat(new object[]
		{
			" ==== (StartNewPlaybackSegment ",
			this.playbackSegments.Count,
			", gap = ",
			gapLengthSec
		}), -1f, USpeakDebugLevel.Full);
		USpeaker.PlaybackSegment playbackSegment = new USpeaker.PlaybackSegment();
		playbackSegment.preDelay = gapLengthSec;
		this.playbackSegments.Add(playbackSegment);
	}

	// Token: 0x06004C74 RID: 19572 RVA: 0x00199260 File Offset: 0x00197660
	public void InitializeSettings(int data)
	{
		this.settings = new USpeakSettingsData(data);
		this.BandwidthMode = this.settings.bandMode;
		this.Bitrate = this.settings.Bitrate;
		this.Duration = this.settings.Duration;
		this.Codec = this.settings.Codec;
		PhotonView componentInParent = base.GetComponentInParent<PhotonView>();
		string text = (!(componentInParent != null)) ? "<unknown owner>" : componentInParent.viewID.ToString();
		MonoBehaviour.print(string.Concat(new object[]
		{
			"uSpeak: InitializeSettings (owner ID ",
			text,
			"): codec ",
			this.Codec,
			", ",
			this.BandwidthMode,
			", ",
			this.Bitrate,
			", ",
			this.Duration
		}));
		this.codecMgr.Codecs[this.Codec].Initialize(this.BandwidthMode, this.Bitrate, this.Duration);
		this.isInitialized = true;
	}

	// Token: 0x06004C75 RID: 19573 RVA: 0x00199398 File Offset: 0x00197798
	public void DefaultInitializeSettings()
	{
		this.settings = new USpeakSettingsData();
		this.settings.bandMode = this.BandwidthMode;
		this.settings.Codec = this.Codec;
		this.settings.Bitrate = this.Bitrate;
		this.settings.Duration = this.Duration;
		this.InitializeSettings(this.settings.ToInt());
	}

	// Token: 0x06004C76 RID: 19574 RVA: 0x00199408 File Offset: 0x00197808
	private void Awake()
	{
		this._photonView = base.GetComponentInParent<PhotonView>();
		USpeaker.CacheDevices();
		USpeaker.USpeakerList.Add(this);
		this._player = base.GetComponentInParent<VRCPlayer>();
		if (base.GetComponent<AudioSource>() == null)
		{
			base.gameObject.AddComponent<AudioSource>();
		}
		this._audioSource = base.GetComponent<AudioSource>();
		this._audioSource.clip = AudioClip.Create("vc", this.PlaybackBufferLengthInSamples, 1, this.audioFrequency, false);
		this._audioSource.loop = true;
		this._audioSource.spatialBlend = 1f;
		this._onspAudioSource = base.GetComponent<ONSPAudioSource>();
		this._photonSender3D = base.GetComponent<USpeakPhotonSender3D>();
		this.codecMgr = USpeakCodecManager.CreateNewInstance();
		this.lastBandMode = this.BandwidthMode;
		this.lastCodec = this.Codec;
		this.lastBitrate = this.Bitrate;
		this.last3DMode = this._3DMode;
		AudioSettings.OnAudioConfigurationChanged += this.OnAudioConfigurationChanged;
	}

	// Token: 0x06004C77 RID: 19575 RVA: 0x00199509 File Offset: 0x00197909
	private void OnDestroy()
	{
		USpeaker.USpeakerList.Remove(this);
		AudioSettings.OnAudioConfigurationChanged -= this.OnAudioConfigurationChanged;
	}

	// Token: 0x06004C78 RID: 19576 RVA: 0x00199528 File Offset: 0x00197928
	private IEnumerator Start()
	{
		yield return null;
		this.audioHandler = (ISpeechDataHandler)this.FindSpeechHandler();
		this.talkController = (IUSpeakTalkController)this.FindInputHandler();
		if (this.audioHandler == null)
		{
			Debug.LogError("USpeaker requires a component which implements the ISpeechDataHandler interface");
			yield break;
		}
		this.DefaultInitializeSettings();
		this.InitVoiceAudioFalloff();
		this.InitBotAudio();
		if (this.SpeakerMode == SpeakerMode.Remote)
		{
			yield break;
		}
		this.sendt = 1f / this.SendRate;
		if (this.AskPermission && !Application.HasUserAuthorization(UserAuthorization.Microphone))
		{
			yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
		}
		if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
		{
			Debug.LogError("Failed to start recording - user has denied microphone access");
			yield break;
		}
		if (USpeaker.devices.Length == 0)
		{
			Debug.LogWarning("Failed to find a recording device");
			yield break;
		}
		this.UpdateSettings();
		this.DebugPrintMicrophoneDevices();
		USpeaker.SetInputDeviceFromPrefs();
		this.StartMicrophone(true);
		this._audioSource.volume = this.SpeakerVolume;
		yield break;
	}

	// Token: 0x06004C79 RID: 19577 RVA: 0x00199544 File Offset: 0x00197944
	private void Update()
	{
		this.talkTimer -= Time.deltaTime;
		if (this.last3DMode != this._3DMode)
		{
			this.last3DMode = this._3DMode;
			this.StopPlaying();
			this._audioSource.clip = AudioClip.Create("vc", this.PlaybackBufferLengthInSamples, 1, this.audioFrequency, false);
			this._audioSource.loop = true;
			this._audioSource.spatialBlend = 1f;
		}
		if (this._3DMode == ThreeDMode.SpeakerPan)
		{
			Transform transform = Camera.main.transform;
			Vector3 rhs = Vector3.Cross(transform.up, transform.forward);
			rhs.Normalize();
			float y = Vector3.Dot(base.transform.position - transform.position, rhs);
			float x = Vector3.Dot(base.transform.position - transform.position, transform.forward);
			float f = Mathf.Atan2(y, x);
			float panStereo = Mathf.Sin(f);
			this._audioSource.panStereo = panStereo;
		}
		this.UpdateConeAttenuation();
		this.UpdateVoicePriority();
		if (this._audioSource.isPlaying)
		{
			this._audioSource.priority = Mathf.FloorToInt((1f - this._voicePriority) * 10f);
			int num = this._audioSource.timeSamples - this.lastTime;
			if (num < 0)
			{
				num += this._audioSource.clip.samples;
			}
			this.played += num;
			this.playbackBufferUsed = Mathf.Max(this.playbackBufferUsed - num, 0);
			this.lastTime = this._audioSource.timeSamples;
			int num2 = this.GetCurrentPlaybackSegmentLengthInSamples() - this.played;
			if (num2 <= 0)
			{
				this.Log("STOP playing voice", -1f, USpeakDebugLevel.Full);
				this._audioSource.Pause();
				int num3 = -num2;
				int num4 = this._audioSource.timeSamples - num3;
				if (num4 < 0)
				{
					num4 += this._audioSource.clip.samples;
				}
				this._audioSource.timeSamples = (this.lastTime = num4);
				this.played = 0;
				if (this.playbackSegments.Count > 0)
				{
					this.playbackSegments.RemoveAt(0);
				}
				this.shouldPlay = (this.playbackSegments.Count > 0);
				if (this.shouldPlay)
				{
					this._playStartRequestTime = Time.realtimeSinceStartup;
					this.playDelay = this.playbackSegments[0].preDelay;
				}
				else
				{
					this.StopPlaying();
				}
				this.stoppedTime = Time.realtimeSinceStartup;
			}
		}
		else
		{
			this._audioSource.priority = 255;
			if (this.stoppedTime < 0f)
			{
				this.stoppedTime = Time.realtimeSinceStartup;
			}
			if (this.shouldPlay && Time.realtimeSinceStartup - this._playStartRequestTime >= this.playDelay)
			{
				this._audioSource.Play();
				this.UpdateAudioSourceVolume(false);
				this.stoppedTime = -1f;
				this.Log("START playing voice", -1f, USpeakDebugLevel.Full);
			}
		}
		if (this.SpeakerMode == SpeakerMode.Remote)
		{
			return;
		}
		if (this.audioHandler == null)
		{
			return;
		}
		if (this.DebugSetMicDeviceID >= 0)
		{
			USpeaker.SetInputDevice(this.DebugSetMicDeviceID);
			this.DebugSetMicDeviceID = -1;
		}
		if (USpeaker.devices.Length == 0 || this.BotAudio != null)
		{
			if (!string.IsNullOrEmpty(this.currentDeviceName))
			{
				if (USpeaker.devices.Length == 0)
				{
					Debug.Log("uSpeak: Lost microphone device: " + this.currentDeviceName);
				}
				this.StopMicrophone();
			}
		}
		else if (string.IsNullOrEmpty(this.currentDeviceName))
		{
			if (this.waitingToStartRec)
			{
				this.micFoundDelay--;
				if (this.micFoundDelay <= 0)
				{
					this.micFoundDelay = 0;
					this.waitingToStartRec = false;
					USpeaker.SetInputDevice(USpeaker.InputDeviceName);
					MonoBehaviour.print("uSpeak: New microphone found: " + USpeaker.InputDeviceName);
					this.StartMicrophone(true);
					this.UpdateSettings();
				}
			}
			else
			{
				this.waitingToStartRec = true;
				this.micFoundDelay = 5;
			}
		}
		else
		{
			if (USpeaker.InputDeviceName != this.currentDeviceName)
			{
				MonoBehaviour.print("uSpeak: Changed microphone input device: " + USpeaker.InputDeviceName);
				this.StartMicrophone(true);
			}
			if (USpeaker.devices[Mathf.Min(USpeaker.InputDeviceID, USpeaker.devices.Length - 1)] != this.currentDeviceName)
			{
				bool flag = false;
				for (int i = 0; i < USpeaker.devices.Length; i++)
				{
					if (USpeaker.devices[i] == this.currentDeviceName)
					{
						USpeaker.InputDeviceID = i;
						flag = true;
					}
				}
				if (!flag)
				{
					USpeaker.SetInputDevice(0);
					MonoBehaviour.print("uSpeak: Microphone unplugged, switching to: " + USpeaker.InputDeviceName);
					this.StartMicrophone(true);
				}
				else
				{
					MonoBehaviour.print(string.Concat(new object[]
					{
						"uSpeak: Microphone device list changed, ",
						this.currentDeviceName,
						" is now device ID ",
						USpeaker.InputDeviceID
					}));
				}
			}
		}
		if (this.lastBandMode != this.BandwidthMode || this.lastCodec != this.Codec || this.lastBitrate != this.Bitrate)
		{
			Debug.Log(string.Concat(new object[]
			{
				"uSpeak: settings modified: ",
				this.lastBandMode,
				" => ",
				this.BandwidthMode,
				", codec ",
				this.lastCodec,
				" => ",
				this.Codec,
				", bitrate ",
				this.lastBitrate,
				" => ",
				this.Bitrate
			}));
			this.UpdateSettings();
			this.lastBandMode = this.BandwidthMode;
			this.lastCodec = this.Codec;
			this.lastBitrate = this.Bitrate;
		}
		int num5 = this.codecMgr.Codecs[this.Codec].GetSampleSize(this.audioFrequency);
		if (num5 == 0)
		{
			num5 = 100;
		}
		if (this.BotAudio)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num6 = realtimeSinceStartup - this.lastBotReadTime;
			this.lastBotReadTime = realtimeSinceStartup;
			bool flag2 = false;
			this.botReadPos += Mathf.CeilToInt((float)this.BotAudio.frequency * num6);
			if (this.botReadPos >= this.BotAudio.samples)
			{
				this.botReadPos -= this.BotAudio.samples;
				flag2 = true;
			}
			int num7 = this.botReadPos + this.BotAudio.samples * this.botRecordedChunkCount;
			if (num7 < this.lastBotReadPos)
			{
				this.botRecordedChunkCount++;
			}
			this.botReadPos += this.BotAudio.samples * this.botRecordedChunkCount;
			try
			{
				int num8 = this.botReadPos - this.lastBotReadPos;
				int num9 = this.lastBotReadPos;
				int num10 = Mathf.FloorToInt((float)(num8 / num5));
				for (int j = 0; j < num10; j++)
				{
					if (this.talkController == null || this.talkController.ShouldSend())
					{
						float[] @float = USpeakPoolUtils.GetFloat(num5);
						this.BotAudio.GetData(@float, num9 % this.BotAudio.samples);
						ushort inputFrameIndex = (ushort)(num9 / num5 % 65536);
						this.OnAudioAvailable(@float, inputFrameIndex);
					}
					num9 += num5;
				}
				this.lastBotReadPos = num9;
			}
			catch (Exception exception)
			{
				Debug.LogError("uSpeak: exception reading bot recording!");
				Debug.LogException(exception);
			}
			if (flag2 && !this.BotAudioShouldLoop)
			{
				this.ClearOverrideRecording();
			}
		}
		else if (USpeaker.devices.Length > 0 && this.recording != null)
		{
			int num11 = this.MicrophoneGetPosition(this.currentDeviceName);
			int num12 = num11 + this.recording.samples * this.recordedChunkCount;
			if (num12 < this.lastReadPos)
			{
				this.recordedChunkCount++;
			}
			num11 += this.recording.samples * this.recordedChunkCount;
			if (num11 <= this.overlap)
			{
				return;
			}
			try
			{
				int num13 = num11 - this.lastReadPos;
				int num14 = this.lastReadPos;
				int num15 = Mathf.FloorToInt((float)(num13 / num5));
				for (int k = 0; k < num15; k++)
				{
					if (this.talkController == null || this.talkController.ShouldSend())
					{
						this.talkTimer = 0f;
						float[] float2 = USpeakPoolUtils.GetFloat(num5);
						this.recording.GetData(float2, num14 % this.recording.samples);
						ushort inputFrameIndex2 = (ushort)(num14 / num5 % 65536);
						this.OnAudioAvailable(float2, inputFrameIndex2);
					}
					num14 += num5;
				}
				this.lastReadPos = num14;
			}
			catch (Exception exception2)
			{
				Debug.LogError("uSpeak: exception reading mic recording!");
				Debug.LogException(exception2);
			}
		}
		this.ProcessPendingEncodeBuffer();
		bool flag3 = true;
		if (this.SendingMode == SendBehavior.RecordThenSend && this.talkController != null)
		{
			flag3 = !this.talkController.ShouldSend();
		}
		if (Time.realtimeSinceStartup - this._lastSendTime >= this.sendt && flag3)
		{
			this._lastSendTime = Time.realtimeSinceStartup;
			if (this.sendBuffer.Count > 0)
			{
				this.tempSendBytes.Clear();
				int serverTimeInMilliSeconds = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds;
				this.tempSendBytes.AddRange(BitConverter.GetBytes(serverTimeInMilliSeconds));
				foreach (USpeakFrameContainer uspeakFrameContainer in this.sendBuffer)
				{
					byte[] collection = uspeakFrameContainer.ToByteArray();
					this.tempSendBytes.AddRange(collection);
				}
				this.sendBuffer.Clear();
				if (this.tempSendBytes.Count > 0)
				{
					this.audioHandler.USpeakOnSerializeAudio(this.tempSendBytes.ToArray());
				}
			}
		}
		this.UpdateTrackedBandwidthUsage();
	}

	// Token: 0x06004C7A RID: 19578 RVA: 0x00199FE0 File Offset: 0x001983E0
	private void LateUpdate()
	{
		this.UpdateAudioSourceVolume(true);
	}

	// Token: 0x06004C7B RID: 19579 RVA: 0x00199FE9 File Offset: 0x001983E9
	private string GetOwnerID()
	{
		return (!(this._photonView != null)) ? "<no owner ID>" : this._photonView.ownerId.ToString();
	}

	// Token: 0x06004C7C RID: 19580 RVA: 0x0019A01C File Offset: 0x0019841C
	private void DebugPrintAttenutation()
	{
		if (this._player == null)
		{
			return;
		}
		Debug.Log("*** DebugPrintAttenutation:");
		for (int i = 0; i < USpeaker.USpeakerList.Count; i++)
		{
			USpeaker uspeaker = USpeaker.USpeakerList[i];
			if (!(uspeaker == null) && uspeaker.isActiveAndEnabled)
			{
				Debug.Log(string.Concat(new object[]
				{
					"[",
					uspeaker.GetOwnerID(),
					"] ",
					(!(uspeaker == this)) ? " " : "**LOCAL** ",
					" volume ",
					uspeaker._audioSource.volume,
					", target vol ",
					uspeaker._dynamicVolumePct,
					", priority ",
					uspeaker.GetVoicePriority()
				}));
			}
		}
	}

	// Token: 0x06004C7D RID: 19581 RVA: 0x0019A118 File Offset: 0x00198518
	private void UpdateAudioSourceVolume(bool lerp)
	{
		float num = this.CalculateDynamicVolumePct();
		if (lerp && this._lastVolumeUpdateTime >= 0.0)
		{
			float num2 = Mathf.Min((float)(AudioSettings.dspTime - this._lastVolumeUpdateTime), Time.maximumDeltaTime);
			this._audioSource.volume = Mathf.MoveTowards(this._audioSource.volume, num, (this._audioSource.volume >= num) ? (num2 / 0.25f) : (num2 / 0.75f));
		}
		else
		{
			this._audioSource.volume = num;
		}
		this._lastVolumeUpdateTime = AudioSettings.dspTime;
	}

	// Token: 0x06004C7E RID: 19582 RVA: 0x0019A1BC File Offset: 0x001985BC
	private void UpdateConeAttenuation()
	{
		if (this.AttenuationConeAngleDeg <= 0f)
		{
			this._coneAttenuationPct = 1f;
			this._localPlayerFacingPriorityFactor = 1f;
			return;
		}
		Transform transform = VRCTrackingManager.GetAudioListener().transform;
		Vector3 vector = transform.position - base.transform.position;
		float magnitude = vector.magnitude;
		if (magnitude < 0.1f)
		{
			this._coneAttenuationPct = 1f;
			this._localPlayerFacingPriorityFactor = 1f;
			return;
		}
		vector /= magnitude;
		float num = Vector3.Dot(vector, base.transform.forward);
		float num2 = Mathf.Cos(this.AttenuationConeAngleDeg);
		if (num >= num2)
		{
			this._coneAttenuationPct = 1f;
		}
		else
		{
			float t = Mathf.InverseLerp(-1f, num2, num);
			this._coneAttenuationPct = Mathf.Lerp(this.AttentuationConeMinVolumePct, 1f, t);
		}
		Vector3 lhs = -vector;
		float num3 = Vector3.Dot(lhs, transform.forward);
		float num4 = Mathf.Cos(this.FacingPriorityConeAngleDeg);
		if (num3 >= num4)
		{
			this._localPlayerFacingPriorityFactor = 1f;
		}
		else
		{
			float t2 = Mathf.InverseLerp(-1f, num4, num3);
			this._localPlayerFacingPriorityFactor = Mathf.Lerp(this.FacingPriorityConeMinVolumePct, 1f, t2);
		}
	}

	// Token: 0x06004C7F RID: 19583 RVA: 0x0019A308 File Offset: 0x00198708
	private void UpdateVoicePriority()
	{
		float num = 1f;
		num *= this._localPlayerFacingPriorityFactor;
		Transform transform = VRCTrackingManager.GetAudioListener().transform;
		float magnitude = (transform.position - base.transform.position).magnitude;
		if (magnitude < 0.1f)
		{
			this._voicePriority = 1f;
			return;
		}
		num *= this.GetVoicePriorityAttenuationAtDistance(magnitude);
		this._voicePriority = Mathf.Clamp01(num);
	}

	// Token: 0x06004C80 RID: 19584 RVA: 0x0019A37C File Offset: 0x0019877C
	private float GetVoicePriorityAttenuationAtDistance(float dist)
	{
		float near = this._onspAudioSource.Near;
		float num = (!(this._photonSender3D != null)) ? 25f : this._photonSender3D.distanceThreshold;
		float num2 = num - near;
		if (num2 <= 0f)
		{
			return (dist <= num) ? 1f : 0f;
		}
		float num3 = Mathf.Clamp01((num - dist) / num2);
		return Mathf.Clamp01(num3 * num3);
	}

	// Token: 0x06004C81 RID: 19585 RVA: 0x0019A3F8 File Offset: 0x001987F8
	private float CalculateVoicePriorityAttenuationPct()
	{
		float num = -1f;
		for (int i = 0; i < USpeaker.USpeakerList.Count; i++)
		{
			USpeaker uspeaker = USpeaker.USpeakerList[i];
			if (!(uspeaker == null) && uspeaker.isActiveAndEnabled && uspeaker.IsVoicePlaying())
			{
				num = Mathf.Max(num, USpeaker.USpeakerList[i].GetVoicePriority());
			}
		}
		if (num <= 0f)
		{
			return 1f;
		}
		float num2 = Mathf.Clamp01(this._voicePriority / num);
		return num2 * num2;
	}

	// Token: 0x06004C82 RID: 19586 RVA: 0x0019A492 File Offset: 0x00198892
	public float GetVoicePriority()
	{
		return this._voicePriority;
	}

	// Token: 0x06004C83 RID: 19587 RVA: 0x0019A49A File Offset: 0x0019889A
	public bool IsVoicePrioritizationEnabled()
	{
		return this.EnableVoicePriorityAttenuation && VRCInputManager.voicePrioritization;
	}

	// Token: 0x06004C84 RID: 19588 RVA: 0x0019A4B0 File Offset: 0x001988B0
	private float CalculateDynamicVolumePct()
	{
		float num = this.SpeakerVolume;
		if (this.EnableConeAttenuation)
		{
			num *= this._coneAttenuationPct;
		}
		if (this.IsVoicePrioritizationEnabled())
		{
			num *= this.CalculateVoicePriorityAttenuationPct();
		}
		this._dynamicVolumePct = num;
		return this._dynamicVolumePct;
	}

	// Token: 0x06004C85 RID: 19589 RVA: 0x0019A4F9 File Offset: 0x001988F9
	private void OnAudioConfigurationChanged(bool devicesChanged)
	{
		if (this.SpeakerMode == SpeakerMode.Local)
		{
			Debug.Log("uSpeak: OnAudioConfigurationChanged - devicesChanged = " + devicesChanged + ", resetting mic..");
			this.StopMicrophone();
			USpeaker.SetInputDevice(USpeaker.InputDeviceName);
		}
	}

	// Token: 0x06004C86 RID: 19590 RVA: 0x0019A530 File Offset: 0x00198930
	private void InitVoiceAudioFalloff()
	{
		VRC_SceneDescriptor instance = VRC_SceneDescriptor.Instance;
		if (instance.UseCustomVoiceFalloffRange)
		{
			this._onspAudioSource.Near = instance.VoiceFalloffRangeNear;
			this._onspAudioSource.Far = instance.VoiceFalloffRangeFar;
			if (this._photonSender3D != null)
			{
				this._photonSender3D.distanceThreshold = Mathf.Max(this._photonSender3D.distanceThreshold, instance.VoiceFalloffRangeFar * 0.1f);
			}
		}
	}

	// Token: 0x06004C87 RID: 19591 RVA: 0x0019A5A8 File Offset: 0x001989A8
	public bool IsVoicePlaying()
	{
		return this._audioSource.isPlaying;
	}

	// Token: 0x06004C88 RID: 19592 RVA: 0x0019A5B8 File Offset: 0x001989B8
	private void StartMicrophone(bool restoreDefaultMicSettingsOnFail = true)
	{
		if (!string.IsNullOrEmpty(this.currentDeviceName) && USpeaker.InputDeviceName != this.currentDeviceName)
		{
			this.MicrophoneEnd(this.currentDeviceName);
		}
		this.recording = null;
		if (this.BotAudio != null)
		{
			return;
		}
		this.currentDeviceName = USpeaker.InputDeviceName;
		if (this.ValidateCurrentInputDeviceSettings())
		{
			this.recording = this.MicrophoneStart(this.currentDeviceName);
			if (this.recording == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"MicrophoneStart: failed to start recording from microphone device ",
					USpeaker.InputDeviceID,
					" '",
					this.currentDeviceName,
					"'. This can be caused by the device being in use, a sound card or microphone device driver issue, or an unsupported microphone."
				}));
			}
			this.ClearMicData();
		}
		if (this.recording == null && restoreDefaultMicSettingsOnFail)
		{
			this.currentDeviceName = string.Empty;
			this.RestoreDefaultMicSettings();
		}
	}

	// Token: 0x06004C89 RID: 19593 RVA: 0x0019A6B4 File Offset: 0x00198AB4
	private void StopMicrophone()
	{
		if (!string.IsNullOrEmpty(this.currentDeviceName))
		{
			Debug.Log("uSpeak: Stop Microphone: " + this.currentDeviceName);
			this.MicrophoneEnd(this.currentDeviceName);
		}
		this.currentDeviceName = string.Empty;
		this.recording = null;
		this.ClearMicData();
	}

	// Token: 0x06004C8A RID: 19594 RVA: 0x0019A70A File Offset: 0x00198B0A
	private bool ValidateCurrentInputDeviceSettings()
	{
		return USpeaker.InputDeviceID >= 0 && USpeaker.InputDeviceID < USpeaker.devices.Length && USpeaker.InputDeviceName == USpeaker.devices[USpeaker.InputDeviceID];
	}

	// Token: 0x06004C8B RID: 19595 RVA: 0x0019A740 File Offset: 0x00198B40
	private void RestoreDefaultMicSettings()
	{
		USpeaker.SetInputDevice(0);
		if (USpeaker.devices.Length > 0)
		{
			this.StartMicrophone(false);
		}
	}

	// Token: 0x06004C8C RID: 19596 RVA: 0x0019A75C File Offset: 0x00198B5C
	private void ClearMicData()
	{
		this.lastReadPos = 0;
		this.sendBuffer.Clear();
		this.recordedChunkCount = 0;
		this.pendingEncode.Clear();
		this.pendingEncodeFrameIndices.Clear();
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x0019A78D File Offset: 0x00198B8D
	private void ClearBotData()
	{
		this.BotAudio = null;
		this.lastBotReadTime = Time.realtimeSinceStartup;
		this.botRecordedChunkCount = 0;
		this.botReadPos = 0;
		this.lastBotReadPos = 0;
	}

	// Token: 0x06004C8E RID: 19598 RVA: 0x0019A7B8 File Offset: 0x00198BB8
	private void StopPlaying()
	{
		this._audioSource.Stop();
		this._audioSource.time = 0f;
		this.index = 0;
		this.played = 0;
		this.playbackSegments.Clear();
		this.lastTime = 0;
		this.playDelay = 0f;
		this._playStartRequestTime = -1f;
		this.stoppedTime = -1f;
		this.shouldPlay = false;
		this.playbackBufferUsed = 0;
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x0019A830 File Offset: 0x00198C30
	private void UpdateSettings()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		PhotonView componentInParent = base.GetComponentInParent<PhotonView>();
		string text = (!(componentInParent != null)) ? "<unknown owner>" : componentInParent.viewID.ToString();
		Debug.Log(string.Concat(new object[]
		{
			"uSpeak: UpdateSettings locally (owner ID ",
			text,
			"), sending to remotes: codec ",
			this.Codec,
			", ",
			this.BandwidthMode,
			", ",
			this.Bitrate,
			", ",
			this.Duration
		}));
		this.settings = new USpeakSettingsData();
		this.settings.bandMode = this.BandwidthMode;
		this.settings.Codec = this.Codec;
		this.settings.Bitrate = this.Bitrate;
		this.settings.Duration = this.Duration;
		this.audioHandler.USpeakInitializeSettings(this.settings.ToInt());
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x0019A958 File Offset: 0x00198D58
	public void SendSettingsToPlayer(VRC.Player p)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (p == null)
		{
			Debug.LogError("SendSettingsToPlayer Failed bc player is null");
			return;
		}
		PhotonView componentInParent = base.GetComponentInParent<PhotonView>();
		if (componentInParent == null)
		{
			Debug.LogError("SendSettingsToPlayer Failed bc photonview is null");
			return;
		}
		string text = (!(componentInParent != null)) ? "<unknown owner>" : componentInParent.viewID.ToString();
		Debug.Log(string.Concat(new object[]
		{
			"uSpeak: UpdateSettings locally (owner ID ",
			text,
			"), sending to remotes: codec ",
			this.Codec,
			", ",
			this.BandwidthMode,
			", ",
			this.Bitrate,
			", ",
			this.Duration
		}));
		this.settings = new USpeakSettingsData();
		this.settings.bandMode = this.BandwidthMode;
		this.settings.Codec = this.Codec;
		this.settings.Bitrate = this.Bitrate;
		this.settings.Duration = this.Duration;
		if (this.audioHandler == null)
		{
			Debug.LogError("SendSettingsToPlayer Failed bc audioHandler is null");
			return;
		}
		this.audioHandler.USpeakInitializeSettingsForPlayer(this.settings.ToInt(), p);
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x0019AAC4 File Offset: 0x00198EC4
	private Component FindSpeechHandler()
	{
		Component[] components = base.GetComponents<Component>();
		foreach (Component component in components)
		{
			if (component is ISpeechDataHandler)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06004C92 RID: 19602 RVA: 0x0019AB00 File Offset: 0x00198F00
	private Component FindInputHandler()
	{
		Component[] components = base.GetComponents<Component>();
		foreach (Component component in components)
		{
			if (component is IUSpeakTalkController)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x0019AB3C File Offset: 0x00198F3C
	private void OnAudioAvailable(float[] pcmData, ushort inputFrameIndex)
	{
		if (this.UseVAD && !this.CheckVAD(pcmData))
		{
			USpeakPoolUtils.Return(pcmData);
			return;
		}
		this.talkTimer = 1f;
		this.pendingEncode.Add(pcmData);
		this.pendingEncodeFrameIndices.Add(inputFrameIndex);
	}

	// Token: 0x06004C94 RID: 19604 RVA: 0x0019AB8C File Offset: 0x00198F8C
	private void ProcessPendingEncodeBuffer()
	{
		int num = 100;
		float num2 = (float)num / 1000f;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup <= realtimeSinceStartup + num2 && this.pendingEncode.Count > 0)
		{
			float[] array = this.pendingEncode[0];
			this.pendingEncode.RemoveAt(0);
			ushort inputFrameIndex = this.pendingEncodeFrameIndices[0];
			this.pendingEncodeFrameIndices.RemoveAt(0);
			this.ProcessPendingEncode(array, inputFrameIndex);
			USpeakPoolUtils.Return(array);
		}
	}

	// Token: 0x06004C95 RID: 19605 RVA: 0x0019AC10 File Offset: 0x00199010
	private void ProcessPendingEncode(float[] pcm, ushort inputFrameIndex)
	{
		byte[] array = USpeakAudioClipCompressor.CompressAudioData(pcm, 1, this.lastBandMode, this.codecMgr.Codecs[this.lastCodec], USpeaker.LocalGain);
		USpeakFrameContainer item = default(USpeakFrameContainer);
		item.FrameIndex = inputFrameIndex;
		item.encodedData = array;
		this.sendBuffer.Add(item);
		this.StatCompressedBytesSinceLastAvg += (uint)array.Length;
		this.StatAudioSampleTimeSinceLastAvg += (float)pcm.Length / (float)this.audioFrequency;
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x0019AC94 File Offset: 0x00199094
	private float[] normalize(float[] samples, float magnitude)
	{
		float[] array = new float[samples.Length];
		for (int i = 0; i < samples.Length; i++)
		{
			array[i] = samples[i] / magnitude;
		}
		return array;
	}

	// Token: 0x06004C97 RID: 19607 RVA: 0x0019ACC8 File Offset: 0x001990C8
	private float amplitude(float[] x)
	{
		float num = 0f;
		for (int i = 0; i < x.Length; i++)
		{
			num = Mathf.Max(num, Mathf.Abs(x[i]));
		}
		return num;
	}

	// Token: 0x06004C98 RID: 19608 RVA: 0x0019AD00 File Offset: 0x00199100
	private bool CheckVAD(float[] samples)
	{
		if (Time.realtimeSinceStartup < this.lastVTime + this.vadHangover)
		{
			return true;
		}
		float rootMeanSquare = this.GetRootMeanSquare(samples);
		float peak = this.GetPeak(samples);
		if (rootMeanSquare >= this.VolumeThresholdRMS || peak >= this.VolumeThresholdPeak)
		{
			this.lastVTime = Time.realtimeSinceStartup;
			return true;
		}
		return false;
	}

	// Token: 0x06004C99 RID: 19609 RVA: 0x0019AD5C File Offset: 0x0019915C
	public bool IsRecording()
	{
		return this.BotAudio != null || (this.talkController != null && this.talkController.ShouldSend());
	}

	// Token: 0x06004C9A RID: 19610 RVA: 0x0019AD89 File Offset: 0x00199189
	private void InitBotAudio()
	{
		if (this.DefaultBotAudioClip != null)
		{
			this.OverrideRecording(this.DefaultBotAudioClip, this.DefaultBotAudioShouldLoop);
		}
	}

	// Token: 0x06004C9B RID: 19611 RVA: 0x0019ADB0 File Offset: 0x001991B0
	public void OverrideRecording(AudioClip C, bool loop = true)
	{
		this.ClearBotData();
		this.BotAudio = C;
		this.DefaultBotAudioClip = C;
		this.BotAudioShouldLoop = loop;
		this.DefaultBotAudioShouldLoop = loop;
	}

	// Token: 0x06004C9C RID: 19612 RVA: 0x0019ADE3 File Offset: 0x001991E3
	public void ClearOverrideRecording()
	{
		this.ClearBotData();
	}

	// Token: 0x06004C9D RID: 19613 RVA: 0x0019ADEB File Offset: 0x001991EB
	public void SetMaxDistance(float distance)
	{
		this._audioSource.maxDistance = distance;
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x0019ADFC File Offset: 0x001991FC
	private void UpdateTrackedBandwidthUsage()
	{
		if (this.StatAudioSampleTimeSinceLastAvg >= 1f)
		{
			if (this.StatBytesSinceBeginning > 4293967295u || this.StatTimeSinceBeginning > 600f)
			{
				this.StatBytesSinceBeginning = 0u;
				this.StatTimeSinceBeginning = 0f;
			}
			this.StatBytesSinceBeginning += this.StatCompressedBytesSinceLastAvg;
			this.StatTimeSinceBeginning += this.StatAudioSampleTimeSinceLastAvg;
			this.StatCompressedBytesSinceLastAvg = 0u;
			this.StatAudioSampleTimeSinceLastAvg = 0f;
		}
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x0019AE84 File Offset: 0x00199284
	private void Autolevel(float[] samples, float rms)
	{
		if (rms > this.rmsTarget)
		{
			this.Log(string.Concat(new object[]
			{
				"Auto-leveling high RMS!: ",
				rms,
				" => ",
				this.rmsTarget
			}), 5f, USpeakDebugLevel.Full);
			float num = this.rmsTarget / rms;
			float num2 = (float)samples.Length / 2f;
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] *= Mathf.Lerp(this.currentScale, num, (float)i / num2);
			}
			this.currentScale = num;
			this.runningScale = this.runningScale * 0.95f + num * 0.05f;
		}
		else
		{
			this.runningScale = this.runningScale * 0.9975f + 0.0025f;
			float num3 = (float)samples.Length / 2f;
			if (this.currentScale < 1f || this.runningScale < 1f)
			{
				for (int j = 0; j < samples.Length; j++)
				{
					samples[j] *= Mathf.Lerp(this.currentScale, this.runningScale, (float)j / num3);
				}
				this.currentScale = this.runningScale;
			}
		}
	}

	// Token: 0x06004CA0 RID: 19616 RVA: 0x0019AFD0 File Offset: 0x001993D0
	public static IEnumerator DebugSpawnUSpeakBots()
	{
		yield return new WaitForSeconds(1f);
		int spawnCount = 0;
		int spawnTotal = 6;
		int count = 4;
		GameObject[] prefabs = new GameObject[count];
		for (int i = 0; i < count; i++)
		{
			prefabs[i] = (Resources.Load("Voice/uSpeakBot" + (i + 1).ToString(), typeof(GameObject)) as GameObject);
		}
		Vector3 offset = new Vector3(0f, 1.8f, 0f);
		VRC_SceneDescriptor sceneDesc = VRC_SceneDescriptor.Instance;
		if (sceneDesc != null)
		{
			foreach (Transform transform in sceneDesc.spawns)
			{
				if (spawnCount == spawnTotal)
				{
					break;
				}
				if (transform)
				{
					GameObject[] array = prefabs;
					int num;
					spawnCount = (num = spawnCount) + 1;
					UnityEngine.Object.Instantiate<GameObject>(array[num % count], transform.position + offset, transform.rotation);
				}
			}
		}
		yield break;
	}

	// Token: 0x06004CA1 RID: 19617 RVA: 0x0019AFE4 File Offset: 0x001993E4
	public static void DebugSpawnUSpeakBot(GameObject parent, AudioClip clip)
	{
		Debug.Log("DebugSpawnUSpeakBot: " + parent.name + ", " + clip.name);
		GameObject original = Resources.Load("Voice/uSpeakBot", typeof(GameObject)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.GetComponent<USpeaker>().OverrideRecording(clip, true);
	}

	// Token: 0x04003456 RID: 13398
	public static float RemoteGain = 1f;

	// Token: 0x04003457 RID: 13399
	public static float LocalGain = 1f;

	// Token: 0x04003458 RID: 13400
	public static bool MuteAll = false;

	// Token: 0x04003459 RID: 13401
	public static List<USpeaker> USpeakerList = new List<USpeaker>();

	// Token: 0x0400345A RID: 13402
	private static int InputDeviceID = 0;

	// Token: 0x0400345B RID: 13403
	public static string InputDeviceName = string.Empty;

	// Token: 0x0400345C RID: 13404
	public SpeakerMode SpeakerMode;

	// Token: 0x0400345D RID: 13405
	public BandMode BandwidthMode;

	// Token: 0x0400345E RID: 13406
	public BitRate Bitrate = BitRate.BitRate_24K;

	// Token: 0x0400345F RID: 13407
	public FrameDuration Duration = FrameDuration.FrameDuration_20ms;

	// Token: 0x04003460 RID: 13408
	public float SendRate = 16f;

	// Token: 0x04003461 RID: 13409
	public SendBehavior SendingMode;

	// Token: 0x04003462 RID: 13410
	public bool UseVAD;

	// Token: 0x04003463 RID: 13411
	public ThreeDMode _3DMode;

	// Token: 0x04003464 RID: 13412
	public bool DebugPlayback;

	// Token: 0x04003465 RID: 13413
	public bool AskPermission = true;

	// Token: 0x04003466 RID: 13414
	public bool Mute;

	// Token: 0x04003467 RID: 13415
	public float SpeakerVolume = 1f;

	// Token: 0x04003468 RID: 13416
	public float VolumeThresholdRMS = 0.01f;

	// Token: 0x04003469 RID: 13417
	public float VolumeThresholdPeak = 0.02f;

	// Token: 0x0400346A RID: 13418
	public int Codec;

	// Token: 0x0400346B RID: 13419
	public bool isInitialized;

	// Token: 0x0400346C RID: 13420
	private static string[] devices = new string[0];

	// Token: 0x0400346D RID: 13421
	public const string kInvalidMicName = "<unknown mic>";

	// Token: 0x0400346E RID: 13422
	private USpeakCodecManager codecMgr;

	// Token: 0x0400346F RID: 13423
	private VRCPlayer _player;

	// Token: 0x04003470 RID: 13424
	private AudioSource _audioSource;

	// Token: 0x04003471 RID: 13425
	private ONSPAudioSource _onspAudioSource;

	// Token: 0x04003472 RID: 13426
	private USpeakPhotonSender3D _photonSender3D;

	// Token: 0x04003473 RID: 13427
	private AudioClip recording;

	// Token: 0x04003474 RID: 13428
	private AudioClip BotAudio;

	// Token: 0x04003475 RID: 13429
	private bool BotAudioShouldLoop;

	// Token: 0x04003476 RID: 13430
	private int lastBotReadPos;

	// Token: 0x04003477 RID: 13431
	private int botReadPos;

	// Token: 0x04003478 RID: 13432
	private int botRecordedChunkCount;

	// Token: 0x04003479 RID: 13433
	private float lastBotReadTime;

	// Token: 0x0400347A RID: 13434
	private int recFreq;

	// Token: 0x0400347B RID: 13435
	private int lastReadPos;

	// Token: 0x0400347C RID: 13436
	private float _lastSendTime = -1f;

	// Token: 0x0400347D RID: 13437
	private float sendt = 1f;

	// Token: 0x0400347E RID: 13438
	private List<USpeakFrameContainer> sendBuffer = new List<USpeakFrameContainer>();

	// Token: 0x0400347F RID: 13439
	private List<byte> tempSendBytes = new List<byte>();

	// Token: 0x04003480 RID: 13440
	private ISpeechDataHandler audioHandler;

	// Token: 0x04003481 RID: 13441
	private IUSpeakTalkController talkController;

	// Token: 0x04003482 RID: 13442
	private int overlap;

	// Token: 0x04003483 RID: 13443
	private USpeakSettingsData settings;

	// Token: 0x04003484 RID: 13444
	private string currentDeviceName = string.Empty;

	// Token: 0x04003485 RID: 13445
	private float talkTimer;

	// Token: 0x04003486 RID: 13446
	private float vadHangover = 0.5f;

	// Token: 0x04003487 RID: 13447
	private float lastVTime;

	// Token: 0x04003488 RID: 13448
	private List<ushort> pendingEncodeFrameIndices = new List<ushort>();

	// Token: 0x04003489 RID: 13449
	private List<float[]> pendingEncode = new List<float[]>();

	// Token: 0x0400348A RID: 13450
	private int played;

	// Token: 0x0400348B RID: 13451
	private int playbackBufferUsed;

	// Token: 0x0400348C RID: 13452
	private int index;

	// Token: 0x0400348D RID: 13453
	private List<USpeaker.PlaybackSegment> playbackSegments = new List<USpeaker.PlaybackSegment>();

	// Token: 0x0400348E RID: 13454
	private float _playStartRequestTime = -1f;

	// Token: 0x0400348F RID: 13455
	private float playDelay;

	// Token: 0x04003490 RID: 13456
	private readonly float kPlayDelayForJitter = 0.06666667f;

	// Token: 0x04003491 RID: 13457
	private bool shouldPlay;

	// Token: 0x04003492 RID: 13458
	private int lastTime;

	// Token: 0x04003493 RID: 13459
	private float stoppedTime = -1f;

	// Token: 0x04003494 RID: 13460
	private const float MAX_STOPPED_TIME = 120f;

	// Token: 0x04003495 RID: 13461
	private int lastReceivedFrameIndex = -1;

	// Token: 0x04003496 RID: 13462
	private const int MIN_DISCONTINOUS_GAP_SIZE_IN_FRAMES = 9;

	// Token: 0x04003497 RID: 13463
	private const float MAX_GAP_LENGTH = 1f;

	// Token: 0x04003498 RID: 13464
	private BandMode lastBandMode;

	// Token: 0x04003499 RID: 13465
	private int lastCodec;

	// Token: 0x0400349A RID: 13466
	private BitRate lastBitrate;

	// Token: 0x0400349B RID: 13467
	private ThreeDMode last3DMode;

	// Token: 0x0400349C RID: 13468
	private int recordedChunkCount;

	// Token: 0x0400349D RID: 13469
	private float StatAudioSampleTimeSinceLastAvg;

	// Token: 0x0400349E RID: 13470
	private uint StatCompressedBytesSinceLastAvg;

	// Token: 0x0400349F RID: 13471
	private float StatTimeSinceBeginning;

	// Token: 0x040034A0 RID: 13472
	private uint StatBytesSinceBeginning;

	// Token: 0x040034A1 RID: 13473
	private const float kStatTimeBetweenBandwidthAvg = 1f;

	// Token: 0x040034A2 RID: 13474
	private const int MAX_PACKET_LATE_TIME_MS = 3000;

	// Token: 0x040034A3 RID: 13475
	private int micFoundDelay;

	// Token: 0x040034A4 RID: 13476
	private bool waitingToStartRec;

	// Token: 0x040034A5 RID: 13477
	private float[] _silence;

	// Token: 0x040034A6 RID: 13478
	private PhotonView _photonView;

	// Token: 0x040034A7 RID: 13479
	private uint _volumeStatNumFrames;

	// Token: 0x040034A8 RID: 13480
	private float _volumeStatRMSMin = float.MaxValue;

	// Token: 0x040034A9 RID: 13481
	private float _volumeStatRMSMax;

	// Token: 0x040034AA RID: 13482
	private float _volumeStatRMSTotal;

	// Token: 0x040034AB RID: 13483
	private float _volumeStatRMSMean;

	// Token: 0x040034AC RID: 13484
	private float _volumeStatFloorMin = float.MaxValue;

	// Token: 0x040034AD RID: 13485
	private float _volumeStatFloorMax;

	// Token: 0x040034AE RID: 13486
	private float _volumeStatFloorTotal;

	// Token: 0x040034AF RID: 13487
	private float _volumeStatFloorMean;

	// Token: 0x040034B0 RID: 13488
	private float _volumeStatPeakMin = float.MaxValue;

	// Token: 0x040034B1 RID: 13489
	private float _volumeStatPeakMax;

	// Token: 0x040034B2 RID: 13490
	private float _volumeStatPeakTotal;

	// Token: 0x040034B3 RID: 13491
	private float _volumeStatPeakMean;

	// Token: 0x040034B4 RID: 13492
	public bool EnableConeAttenuation = true;

	// Token: 0x040034B5 RID: 13493
	public float AttenuationConeAngleDeg = 120f;

	// Token: 0x040034B6 RID: 13494
	public float AttentuationConeMinVolumePct = 0.7f;

	// Token: 0x040034B7 RID: 13495
	public bool EnableVoicePriorityAttenuation = true;

	// Token: 0x040034B8 RID: 13496
	public float FacingPriorityConeAngleDeg = 120f;

	// Token: 0x040034B9 RID: 13497
	public float FacingPriorityConeMinVolumePct = 0.7f;

	// Token: 0x040034BA RID: 13498
	private float _coneAttenuationPct = 1f;

	// Token: 0x040034BB RID: 13499
	private float _localPlayerFacingPriorityFactor = 1f;

	// Token: 0x040034BC RID: 13500
	private float _voicePriority = 1f;

	// Token: 0x040034BD RID: 13501
	private float _dynamicVolumePct = 1f;

	// Token: 0x040034BE RID: 13502
	private double _lastVolumeUpdateTime = -1.0;

	// Token: 0x040034BF RID: 13503
	private const float VOLUME_LERP_DOWN_SPEED = 0.25f;

	// Token: 0x040034C0 RID: 13504
	private const float VOLUME_LERP_UP_SPEED = 0.75f;

	// Token: 0x040034C1 RID: 13505
	public AudioClip DefaultBotAudioClip;

	// Token: 0x040034C2 RID: 13506
	public bool DefaultBotAudioShouldLoop = true;

	// Token: 0x040034C3 RID: 13507
	public USpeakDebugLevel DebugLevel = USpeakDebugLevel.Default;

	// Token: 0x040034C4 RID: 13508
	public int DebugSetMicDeviceID = -1;

	// Token: 0x040034C5 RID: 13509
	private int MagnitudeWriteCount;

	// Token: 0x040034C6 RID: 13510
	private int MagnitudeReadCount;

	// Token: 0x040034C7 RID: 13511
	public float[] Magnitude60Hz = new float[32];

	// Token: 0x040034C8 RID: 13512
	private float currentScale = 1f;

	// Token: 0x040034C9 RID: 13513
	private float runningScale = 1f;

	// Token: 0x040034CA RID: 13514
	public float rmsTarget = 0.1f;

	// Token: 0x020009D4 RID: 2516
	private class PlaybackSegment
	{
		// Token: 0x040034CC RID: 13516
		public int receivedSamples;

		// Token: 0x040034CD RID: 13517
		public float preDelay;
	}
}
