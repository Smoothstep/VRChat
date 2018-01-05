using System;
using UnityEngine;

// Token: 0x02000701 RID: 1793
[RequireComponent(typeof(AudioSource))]
public class OVRLipSyncMicInput : MonoBehaviour
{
	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x06003ABD RID: 15037 RVA: 0x00128A58 File Offset: 0x00126E58
	// (set) Token: 0x06003ABE RID: 15038 RVA: 0x00128A60 File Offset: 0x00126E60
	public float Sensitivity
	{
		get
		{
			return this.sensitivity;
		}
		set
		{
			this.sensitivity = Mathf.Clamp(value, 0f, 100f);
		}
	}

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x06003ABF RID: 15039 RVA: 0x00128A78 File Offset: 0x00126E78
	// (set) Token: 0x06003AC0 RID: 15040 RVA: 0x00128A80 File Offset: 0x00126E80
	public float SourceVolume
	{
		get
		{
			return this.sourceVolume;
		}
		set
		{
			this.sourceVolume = Mathf.Clamp(value, 0f, 100f);
		}
	}

	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x00128A98 File Offset: 0x00126E98
	// (set) Token: 0x06003AC2 RID: 15042 RVA: 0x00128AA1 File Offset: 0x00126EA1
	public float MicFrequency
	{
		get
		{
			return (float)this.micFrequency;
		}
		set
		{
			this.micFrequency = (int)Mathf.Clamp(value, 0f, 96000f);
		}
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x00128ABB File Offset: 0x00126EBB
	private void Awake()
	{
		if (!this.audioSource)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (!this.audioSource)
		{
			return;
		}
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x00128AEC File Offset: 0x00126EEC
	private void Start()
	{
		this.audioSource.loop = true;
		this.audioSource.mute = false;
		if (Microphone.devices.Length != 0)
		{
			this.selectedDevice = Microphone.devices[0].ToString();
			this.micSelected = true;
			this.GetMicCaps();
		}
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x00128B3C File Offset: 0x00126F3C
	private void Update()
	{
		if (!this.focused)
		{
			this.StopMicrophone();
		}
		if (!Application.isPlaying)
		{
			this.StopMicrophone();
		}
		this.audioSource.volume = this.sourceVolume / 100f;
		this.loudness = Mathf.Clamp(this.GetAveragedVolume() * this.sensitivity * (this.sourceVolume / 10f), 0f, 100f);
		if (this.micControl == OVRLipSyncMicInput.micActivation.HoldToSpeak)
		{
			if (Microphone.IsRecording(this.selectedDevice) && !Input.GetKey(KeyCode.Space))
			{
				this.StopMicrophone();
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.StartMicrophone();
			}
			if (Input.GetKeyUp(KeyCode.Space))
			{
				this.StopMicrophone();
			}
		}
		if (this.micControl == OVRLipSyncMicInput.micActivation.PushToSpeak && Input.GetKeyDown(KeyCode.Space))
		{
			if (Microphone.IsRecording(this.selectedDevice))
			{
				this.StopMicrophone();
			}
			else if (!Microphone.IsRecording(this.selectedDevice))
			{
				this.StartMicrophone();
			}
		}
		if (this.micControl == OVRLipSyncMicInput.micActivation.ConstantSpeak && !Microphone.IsRecording(this.selectedDevice))
		{
			this.StartMicrophone();
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			this.micSelected = false;
		}
	}

	// Token: 0x06003AC6 RID: 15046 RVA: 0x00128C7F File Offset: 0x0012707F
	private void OnApplicationFocus(bool focus)
	{
		this.focused = focus;
		if (!this.focused)
		{
			this.StopMicrophone();
		}
	}

	// Token: 0x06003AC7 RID: 15047 RVA: 0x00128C99 File Offset: 0x00127099
	private void OnApplicationPause(bool focus)
	{
		this.focused = focus;
		if (!this.focused)
		{
			this.StopMicrophone();
		}
	}

	// Token: 0x06003AC8 RID: 15048 RVA: 0x00128CB3 File Offset: 0x001270B3
	private void OnDisable()
	{
		this.StopMicrophone();
	}

	// Token: 0x06003AC9 RID: 15049 RVA: 0x00128CBB File Offset: 0x001270BB
	private void OnGUI()
	{
		this.MicDeviceGUI((float)(Screen.width / 2 - 150), (float)(Screen.height / 2 - 75), 300f, 50f, 10f, -300f);
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x00128CF0 File Offset: 0x001270F0
	public void MicDeviceGUI(float left, float top, float width, float height, float buttonSpaceTop, float buttonSpaceLeft)
	{
		if (Microphone.devices.Length >= 1 && this.GuiSelectDevice && !this.micSelected)
		{
			for (int i = 0; i < Microphone.devices.Length; i++)
			{
				if (GUI.Button(new Rect(left + (width + buttonSpaceLeft) * (float)i, top + (height + buttonSpaceTop) * (float)i, width, height), Microphone.devices[i].ToString()))
				{
					this.StopMicrophone();
					this.selectedDevice = Microphone.devices[i].ToString();
					this.micSelected = true;
					this.GetMicCaps();
					this.StartMicrophone();
				}
			}
		}
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x00128D94 File Offset: 0x00127194
	public void GetMicCaps()
	{
		if (!this.micSelected)
		{
			return;
		}
		Microphone.GetDeviceCaps(this.selectedDevice, out this.minFreq, out this.maxFreq);
		if (this.minFreq == 0 && this.maxFreq == 0)
		{
			Debug.LogWarning("GetMicCaps warning:: min and max frequencies are 0");
			this.minFreq = 44100;
			this.maxFreq = 44100;
		}
		if (this.micFrequency > this.maxFreq)
		{
			this.micFrequency = this.maxFreq;
		}
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x00128E18 File Offset: 0x00127218
	public void StartMicrophone()
	{
		if (!this.micSelected)
		{
			return;
		}
		this.audioSource.clip = Microphone.Start(this.selectedDevice, true, 1, this.micFrequency);
		while (Microphone.GetPosition(this.selectedDevice) <= 0)
		{
		}
		this.audioSource.Play();
	}

	// Token: 0x06003ACD RID: 15053 RVA: 0x00128E70 File Offset: 0x00127270
	public void StopMicrophone()
	{
		if (!this.micSelected)
		{
			return;
		}
		if (this.audioSource != null && this.audioSource.clip != null && this.audioSource.clip.name == "Microphone")
		{
			this.audioSource.Stop();
		}
		Microphone.End(this.selectedDevice);
	}

	// Token: 0x06003ACE RID: 15054 RVA: 0x00128EE5 File Offset: 0x001272E5
	private float GetAveragedVolume()
	{
		return 0f;
	}

	// Token: 0x04002381 RID: 9089
	public AudioSource audioSource;

	// Token: 0x04002382 RID: 9090
	public bool GuiSelectDevice = true;

	// Token: 0x04002383 RID: 9091
	[SerializeField]
	private float sensitivity = 100f;

	// Token: 0x04002384 RID: 9092
	[SerializeField]
	private float sourceVolume = 100f;

	// Token: 0x04002385 RID: 9093
	[SerializeField]
	private int micFrequency = 16000;

	// Token: 0x04002386 RID: 9094
	public OVRLipSyncMicInput.micActivation micControl;

	// Token: 0x04002387 RID: 9095
	public string selectedDevice;

	// Token: 0x04002388 RID: 9096
	public float loudness;

	// Token: 0x04002389 RID: 9097
	private bool micSelected;

	// Token: 0x0400238A RID: 9098
	private int minFreq;

	// Token: 0x0400238B RID: 9099
	private int maxFreq;

	// Token: 0x0400238C RID: 9100
	private bool focused = true;

	// Token: 0x02000702 RID: 1794
	public enum micActivation
	{
		// Token: 0x0400238E RID: 9102
		HoldToSpeak,
		// Token: 0x0400238F RID: 9103
		PushToSpeak,
		// Token: 0x04002390 RID: 9104
		ConstantSpeak
	}
}
